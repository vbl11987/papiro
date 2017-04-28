using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class InventarioGastoForm : Form
    {
        private readonly usuarios _user;

        private decimal _gastoTotal;

        private readonly List<int> _productosId;

        public InventarioGastoForm(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _gastoTotal = 0;
            _productosId = new List<int>();
        }

        private void Reload()
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    _gastoTotal = 0;
                    _productosId.Clear();
                    productosDataGridView.Rows.Clear();
                    foreach (var producto in entities.producto)
                    {
                        _productosId.Add(producto.id);
                        productosDataGridView.Rows.Add(new object[]
                                                           {
                                                               producto.codigo, producto.nombre,
                                                               producto.tipo_producto.valor, producto.cantidad,
                                                               producto.unidad_medida.siglas,
                                                               Math.Round(producto.precio, 2),
                                                               Math.Round(producto.cantidad*producto.precio, 2), 0, 0m
                                                           });
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Gasto", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EntradaProductosFormLoad(object sender, EventArgs e)
        {
            Font font = productosDataGridView.Font;
            productosDataGridView.Font = new Font(font.Name, 10, font.Style, font.Unit);
            productosDataGridView.AutoGenerateColumns = false;

            Reload();

            GastoTotalLabel.Text = @"Gasto total: " + Math.Round(_gastoTotal, 2);
        }

        private void ProductosDataGridViewCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string value = e.FormattedValue == null ? "" : e.FormattedValue.ToString().Trim();

            // Validar que la cantidad sea un número entero positivo.
            if (productosDataGridView.Columns[e.ColumnIndex].Name == "CantidadUtilizarColumn")
            {
                int cantidad;
                if (value == "")
                {
                    productosDataGridView.Rows[e.RowIndex].ErrorText =
                        "La cantidad de producto a utilizar no puede estar vacía.";
                    e.Cancel = true;
                }
                else if (!int.TryParse(value, out cantidad) || cantidad < 0)
                {
                    productosDataGridView.Rows[e.RowIndex].ErrorText =
                        "La cantidad especificada debe ser un número entero positivo.";
                    e.Cancel = true;
                }
                else if (cantidad > int.Parse(productosDataGridView.Rows[e.RowIndex].Cells["CantidadColumn"].Value.ToString()))
                {
                    productosDataGridView.Rows[e.RowIndex].ErrorText =
                        "La cantidad a utilizar debe ser menor o igual que la cantidad disponible en almacén.";
                    e.Cancel = true;
                }
            }
        }

        private void ProductosDataGridViewCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Limpiar la fila de error en caso de que el usuario presione esc.
            productosDataGridView.Rows[e.RowIndex].ErrorText = String.Empty;

            // Calcular el costo del producto en la fila activa y del total.
            if (productosDataGridView.Columns[e.ColumnIndex].Name != "CantidadUtilizarColumn") return;

            productosDataGridView.Rows[e.RowIndex].Cells["CostoColumn"].Value =
                Math.Round(Convert.ToInt32(productosDataGridView.Rows[e.RowIndex].Cells["CantidadUtilizarColumn"].Value,
                                           CultureInfo.InvariantCulture)*
                           Convert.ToDecimal(productosDataGridView.Rows[e.RowIndex].Cells["PrecioColumn"].Value,
                                             CultureInfo.InvariantCulture), 2);

            _gastoTotal = 0;
            foreach (DataGridViewRow row in productosDataGridView.Rows)
                _gastoTotal += Convert.ToDecimal(row.Cells["CostoColumn"].Value, CultureInfo.InvariantCulture);

            GastoTotalLabel.Text = @"Gasto total: " + Math.Round(_gastoTotal, 2);
        }

        private void CancelarButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private static string ToString(producto producto)
        {
            return
                string.Format(
                    "Producto con Id: {0}, Código: {1}, Nombre: {2}, Tipo de producto: {3}, Cantidad: {4}, Precio: {5}",
                    producto.id, producto.codigo, producto.nombre, producto.tipo_producto.valor, producto.cantidad,
                    Math.Round(producto.precio, 2));
        }

        private void AceptarButtonClick(object sender, EventArgs e)
        {
            // Guardar el gasto.

            try
            {
                if (_gastoTotal == 0)
                {
                    MessageBox.Show(
                        @"Debe utilizar algún producto para realizar la operación.",
                        @"Gasto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                using (var entities = new papiro_finalEntities())
                {
                    // Actualizar el balance.
                    balance balance = entities.balance.ToList().Last();

                    decimal total_gasto = 0;

                    // Actualizar las cantidades de los productos utilizados.
                    foreach (DataGridViewRow row in productosDataGridView.Rows)
                    {
                        if (row.Cells["CantidadUtilizarColumn"].Value.ToString() == "0") continue;
                        var prod =
                            (producto)
                            entities.GetObjectByKey(new EntityKey("papiro_finalEntities.producto", "id",
                                                                  _productosId[row.Index]));
                        // Gasto
                        entities.AddTosubmayor_gasto(new submayor_gasto
                        {
                            fecha = DateTime.Now,
                            id_usuario = _user.id,
                            descripcion = "Merma.",
                            saldo = balance.gasto + Math.Round(int.Parse(row.Cells["CantidadUtilizarColumn"].Value.ToString()) * prod.precio, 2),
                            debito = Math.Round(int.Parse(row.Cells["CantidadUtilizarColumn"].Value.ToString()) * prod.precio, 2),
                            id_producto =  prod.id
                        });
                        balance.gasto += Math.Round(int.Parse(row.Cells["CantidadUtilizarColumn"].Value.ToString()) * prod.precio, 2);
                        total_gasto += Math.Round(int.Parse(row.Cells["CantidadUtilizarColumn"].Value.ToString()) * prod.precio, 2);


                        prod.cantidad -= int.Parse(row.Cells["CantidadUtilizarColumn"].Value.ToString());
                        // Registrar en la bitácora.
                        entities.AddTobitacora(new bitacora
                                                   {
                                                       id_usuario = _user.id,
                                                       nombre_usuario = _user.login_nombre,
                                                       fecha = DateTime.Now,
                                                       accion_realizada = "Se modificó el " + ToString(prod)
                                                   });
                    }
                    
                  
                    
                    // Inventario
                    entities.AddTosubmayor_inventario(new submayor_inventario
                                                          {
                                                              fecha = DateTime.Now,
                                                              id_usuario = _user.id,
                                                              descripcion = "Se registra un nuevo gasto.",
                                                              saldo = balance.inventario - total_gasto,
                                                              credito = total_gasto
                                                          });
                    balance.inventario -= total_gasto;
                    
                    //// Gasto
                    //entities.AddTosubmayor_gasto(new submayor_gasto
                    //                                 {
                    //                                     fecha = DateTime.Now,
                    //                                     id_usuario = _user.id,
                    //                                     descripcion = "Se registra un nuevo gasto.",
                    //                                     saldo = balance.gasto + _gastoTotal,
                    //                                     debito = _gastoTotal
                    //                                 });
                    //balance.gasto += _gastoTotal;

                    // Registrar en la bitácora.
                    entities.AddTobitacora(new bitacora
                                               {
                                                   id_usuario = _user.id,
                                                   nombre_usuario = _user.login_nombre,
                                                   fecha = DateTime.Now,
                                                   accion_realizada =
                                                       "Se registró un nuevo gasto de " + Math.Round(_gastoTotal, 2)
                                               });
                    entities.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Gasto", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Close();
        }
    }
}
