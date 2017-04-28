using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class EntradaProductosForm : Form
    {
        private readonly usuarios _user;

        private int _proveedorId;

        private readonly List<int> _pagoEfectivoId;

        private readonly List<producto> _nuevosProductos = new List<producto>();

        private decimal _importeTotal;

        public EntradaProductosForm(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _pagoEfectivoId = new List<int>();
            _importeTotal = 0;
        }

        private void Reload()
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    // Tipo de productos.
                    TipoProductoColumn.Items.Clear();
                    TipoProductoColumn.Items.Add("(Seleccione)");
                    TipoProductoColumn.DefaultCellStyle.NullValue = "(Seleccione)";
                    foreach (var tipoProducto in entities.tipo_producto) TipoProductoColumn.Items.Add(tipoProducto.valor);
                    
                    // Unidades de medida.
                    UnidadMedidaColumn.Items.Clear();
                    UnidadMedidaColumn.Items.Add("(Seleccione)");
                    UnidadMedidaColumn.DefaultCellStyle.NullValue = "(Seleccione)";
                    foreach (var unidadMedida in entities.unidad_medida) UnidadMedidaColumn.Items.Add(unidadMedida.siglas);

                    // Formas de pago en efectivo.
                    pagoEfectivoComboBox.Items.Clear();
                    pagoEfectivoComboBox.Items.Add("(Seleccione)");
                    _pagoEfectivoId.Clear();
                    _pagoEfectivoId.Add(-1);
                    foreach (var formaPagoEfectivo in entities.forma_pago_efectivo)
                    {
                        _pagoEfectivoId.Add(formaPagoEfectivo.id);
                        pagoEfectivoComboBox.Items.Add(formaPagoEfectivo.descripcion);
                    }
                    pagoEfectivoComboBox.SelectedIndex = 0;

                    // Código de producto existente.
                    codPProdExisttoolStripComboBox.Items.Clear();
                    codPProdExisttoolStripComboBox.Items.Add("(Seleccione)");
                    foreach (var producto in entities.producto)
                    {
                        codPProdExisttoolStripComboBox.Items.Add(producto.codigo);
                    }
                    codPProdExisttoolStripComboBox.SelectedIndex = 0;


                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Entrada de productos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EntradaProductosFormLoad(object sender, EventArgs e)
        {
            bindingSource.DataSource = _nuevosProductos;
            
            //Font font = productosDataGridView.Font;
            //productosDataGridView.Font = new Font(font.Name, 10, font.Style, font.Unit);
            productosDataGridView.AutoGenerateColumns = false;
            productosDataGridView.DataSource = bindingSource;

            // Asociar las columnas del grid.

            CodigoColumn.DataPropertyName = "codigo";

            NombreColumn.DataPropertyName = "nombre";

            TipoProductoColumn.ValueType = typeof (string);
            TipoProductoColumn.AutoComplete = true;
            //TipoProductoColumn.DisplayMember = "valor";
            //TipoProductoColumn.ValueMember = "Self";

            CantidadColumn.DataPropertyName = "cantidad";

            UnidadMedidaColumn.ValueType  = typeof (string);
            UnidadMedidaColumn.AutoComplete = true;
            //UnidadMedidaColumn.DisplayMember = "siglas";
            //UnidadMedidaColumn.ValueMember = "Self";

            PrecioColumn.DataPropertyName = "precio";

            bindingNavigator.BindingSource = bindingSource;

            Reload();

            importeTotalLabel.Text = @"Importe total: " + Math.Round(_importeTotal, 2);
        }

        private void CancelarButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private string ToString(producto producto)
        {
            return
                string.Format(
                    "Producto con Id: {0}, Código: {1}, Nombre: {2}, Tipo de producto: {3}, Cantidad: {4}, Precio: {5}, Fecha de entrada: {6}",
                    producto.id, producto.codigo, producto.nombre, producto.tipo_producto.valor, producto.cantidad,
                    Math.Round(producto.precio, 2),
                    fechaDateTimePicker.Value);
        }

        private void AceptarButtonClick(object sender, EventArgs e)
        {
            // Validar que se ha realizado el pago.
            if (!pagoXFacturacheckBox.Checked && pagoEfectivoComboBox.SelectedIndex == 0)
            {
                MessageBox.Show(@"Debe especificar la forma de pago de los productos", @"Entrada de productos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar que al menos se haya registrado un producto.
            if (_nuevosProductos.Count == 0)
            {
                MessageBox.Show(@"Debe registrar al menos un producto.", @"Entrada de productos", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            // Validar que la fecha de entrada no sea mayor que la fecha actual.
            if (fechaDateTimePicker.Value > DateTime.Now)
            {
                MessageBox.Show(@"La fecha de entrada del producto debe ser menor o igual que la fecha de hoy.",
                                @"Entrada de productos", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            // Validar que cada una de las celdas tenga un valor correcto.
            bool validationErrors = false;
            foreach (DataGridViewRow row in
                productosDataGridView.Rows.Cast<DataGridViewRow>().
                    Where(row => (from DataGridViewCell cell in row.Cells
                                  let value = cell.FormattedValue == null ? "" : cell.FormattedValue.ToString()
                                  where value == "" || 
                                        (productosDataGridView.Columns[cell.ColumnIndex].Name == "UnidadMedidaColumn" && 
                                            value == "(Seleccione)") ||
                                        (productosDataGridView.Columns[cell.ColumnIndex].Name == "TipoProductoColumn" &&
                                            value == "(Seleccione)")
                                  select cell).Any() && row.Index != productosDataGridView.RowCount - 1))
            {
                row.ErrorText = "Existen valores no válidos para registrar el producto.";
                validationErrors = true;
            }
            if (validationErrors)
            {
                MessageBox.Show(@"Existen valores de celda no válidos.", @"Entrada de productos", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    var entrada = new entrada
                                      {
                                          fecha = fechaDateTimePicker.Value,
                                          descripcion = descripttextBox.Text.Trim(),
                                          importe = _importeTotal
                                      };

                    // Actualizar el balance.
                    balance balance = entities.balance.ToList().Last();

                    // Inventario
                    entities.AddTosubmayor_inventario(new submayor_inventario
                                                          {
                                                              fecha = DateTime.Now,
                                                              id_usuario = _user.id,
                                                              descripcion = "Se le da entrada a nuevos productos.",
                                                              saldo = balance.inventario + _importeTotal,
                                                              debito = _importeTotal
                                                          });
                    balance.inventario += _importeTotal;
                    
                    if (pagoXFacturacheckBox.Checked)
                    {
                        entities.AddTocuentas_por_pagar(new cuentas_por_pagar
                                                            {
                                                                descripcion = "Se le da entrada a productos",
                                                                monto = _importeTotal,
                                                                fecha = fechaDateTimePicker.Value,
                                                                ejecutada = 0
                                                            });

                        entities.AddTosubmayor_cuentas_por_pagar(new submayor_cuentas_por_pagar
                                                                     {
                                                                         fecha = DateTime.Now,
                                                                         id_usuario = _user.id,
                                                                         descripcion =
                                                                             "Se le da entrada a nuevos productos.",
                                                                         saldo = balance.cuentas_por_pagar + _importeTotal,
                                                                         credito = _importeTotal
                                                                     });
                        balance.cuentas_por_pagar += _importeTotal;

                        entrada.id_proveedor = _proveedorId;
                    }
                    else if (pagoEfectivoComboBox.SelectedIndex != 0)
                    {
                        entrada.id_pago_efectivo = _pagoEfectivoId[pagoEfectivoComboBox.SelectedIndex];
                        switch (pagoEfectivoComboBox.SelectedIndex)
                        {
                            case 1:
                                if (balance.efectivo_caja - _importeTotal < 0)
                                {
                                    MessageBox.Show(
                                        "No hay dinero suficiente en la caja para darle entrada a los productos",
                                        @"Entrada de productos", MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    return;
                                }
                                entities.AddTosubmayor_efectivo_caja(new submayor_efectivo_caja
                                                                         {
                                                                             fecha = DateTime.Now,
                                                                             id_usuario = _user.id,
                                                                             descripcion =
                                                                                 "Se le da entrada a nuevos productos.",
                                                                             saldo = balance.efectivo_caja - _importeTotal,
                                                                             credito = _importeTotal
                                                                         });
                                balance.efectivo_caja -= _importeTotal;
                                break;
                            case 2:
                                if (balance.efectivo_banco - _importeTotal < 0)
                                {
                                    MessageBox.Show(
                                        "No hay dinero suficiente en el banco para darle entrada a los productos",
                                        @"Entrada de productos", MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    return;
                                }
                                entities.AddTosubmayor_efectivo_banco(new submayor_efectivo_banco
                                                                          {
                                                                              fecha = DateTime.Now,
                                                                              id_usuario = _user.id,
                                                                              descripcion =
                                                                                  "Se le da entrada a nuevos productos.",
                                                                              saldo =
                                                                                  balance.efectivo_banco - _importeTotal,
                                                                              credito = _importeTotal
                                                                          });
                                balance.efectivo_banco -= _importeTotal;
                                break;
                        }
                    }

                    int index = 0;
                    // Guardar los productos nuevos.
                    var auxProdList = new List<producto>();
                    foreach (var prod in _nuevosProductos)
                    {
                        producto auxProd;
                        if (productosDataGridView.Rows[index].Cells["ExistenteColumn"].Value != null &&
                            (bool) productosDataGridView.Rows[index].Cells["ExistenteColumn"].Value)
                        {
                            auxProd =
                                (producto)
                                entities.GetObjectByKey(new EntityKey("papiro_finalEntities.producto", "id", prod.id));
                            var cantidadAumentar =
                                Convert.ToInt32(
                                    productosDataGridView.Rows[index].Cells["AumentarCantidadColumn"].Value,
                                    CultureInfo.InvariantCulture);
                            if (cantidadAumentar > 0)
                            {
                                auxProd.cantidad += cantidadAumentar;
                                var nuevoprecio =
                                    Convert.ToDecimal(
                                        productosDataGridView.Rows[index].Cells["NuevoPrecioColumn"].Value,
                                        CultureInfo.InvariantCulture);
                                if (nuevoprecio > 0)
                                    auxProd.precio = (auxProd.precio + nuevoprecio)/2;
                            }
                        }
                        else
                        {
                            auxProd = new producto
                                          {
                                              codigo = prod.codigo,
                                              nombre = prod.nombre,
                                              id_tipo_producto =
                                                  entities.tipo_producto.ToList().Find(
                                                      tp =>
                                                      tp.valor ==
                                                      (string)
                                                      productosDataGridView.Rows[index].Cells["TipoProductoColumn"].
                                                          Value).id,
                                              precio = prod.precio,
                                              cantidad = prod.cantidad,
                                              cantidad_real = 0,
                                              id_unidad_medida = entities.unidad_medida.ToList().Find(
                                                  um =>
                                                  um.siglas ==
                                                  (string)
                                                  productosDataGridView.Rows[index].Cells["UnidadMedidaColumn"].
                                                      Value).id
                                          };
                            entrada.producto.Add(auxProd);
                        }
                        auxProdList.Add(auxProd);
                        
                        index++;
                    }
                    entities.AddToentrada(entrada);
                    entities.SaveChanges();

                    index = 0;
                    foreach (var prod in auxProdList)
                    {
                        //se guarda en bitacora
                        entities.AddTobitacora(new bitacora
                                                   {
                                                       id_usuario = _user.id,
                                                       nombre_usuario = _user.login_nombre,
                                                       fecha = DateTime.Now,
                                                       accion_realizada =
                                                           (productosDataGridView.Rows[index].Cells["ExistenteColumn"].
                                                                Value != null &&
                                                            ((bool)
                                                             productosDataGridView.Rows[index].Cells["ExistenteColumn"].
                                                                 Value)
                                                                ? "Se modificó el "
                                                                : "Se agregó un nuevo ") + ToString(prod)
                                                   });
                        index++;
                    }
                    entities.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Entrada de productos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Close();
        }

        private void ProductosDataGridViewCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    string value = e.FormattedValue == null ? "" : e.FormattedValue.ToString().Trim();

                    if (productosDataGridView.Rows[e.RowIndex].Cells["ExistenteColumn"].Value != null && 
                        (bool) productosDataGridView.Rows[e.RowIndex].Cells["ExistenteColumn"].Value == false)
                    {
                        // Validar que el código del producto no está vacío y no existe.
                        if (productosDataGridView.Columns[e.ColumnIndex].Name == "CodigoColumn")
                        {
                            if (value == "")
                            {
                                productosDataGridView.Rows[e.RowIndex].ErrorText =
                                    "El código del producto no puede estar vacío.";
                                e.Cancel = true;
                            }
                            else if (entities.producto.Any(prod => prod.codigo == value.Trim()))
                            {
                                {
                                    productosDataGridView.Rows[e.RowIndex].ErrorText =
                                        "Ya existe en el almacén un producto con el código especificado.";
                                    e.Cancel = true;
                                }
                            }
                        }

                        // Validar que el nombre del producto no está vacío y no exista.
                        if (productosDataGridView.Columns[e.ColumnIndex].Name == "NombreColumn")
                        {
                            if (value == "")
                            {
                                productosDataGridView.Rows[e.RowIndex].ErrorText =
                                    "El nombre del producto no puede estar vacío.";
                                e.Cancel = true;
                            }
                        }

                        // Validar que la unidad de medida este definida.
                        if (productosDataGridView.Columns[e.ColumnIndex].Name == "UnidadMedidaColumn")
                        {
                            if (value == "(Seleccione)")
                            {
                                productosDataGridView.Rows[e.RowIndex].ErrorText =
                                    "Debe seleccionar una unidad de medida.";
                                e.Cancel = true;
                            }
                        }

                        // Validar que la cantidad sea un número entero positivo.
                        if (productosDataGridView.Columns[e.ColumnIndex].Name == "CantidadColumn")
                        {
                            int cantidad;
                            if (!int.TryParse(value, out cantidad) || cantidad < 0)
                            {
                                productosDataGridView.Rows[e.RowIndex].ErrorText =
                                    "La cantidad especificada debe ser un número entero positivo.";
                                e.Cancel = true;
                            }
                        }
                        
                        // Validar que el tipo de producto este definido.
                        if (productosDataGridView.Columns[e.ColumnIndex].Name == "TipoProductoColumn")
                        {
                            if (value == "(Seleccione)")
                            {
                                productosDataGridView.Rows[e.RowIndex].ErrorText =
                                    "Debe seleccionar un tipo de producto.";
                                e.Cancel = true;
                            }
                            else if (entities.producto.Any(prod => prod.tipo_producto.valor == value) ||
                                     productosDataGridView.Rows.Cast<DataGridViewRow>().Any(
                                         row =>
                                         row.Index != e.RowIndex &&
                                         row.Cells["TipoProductoColumn"].FormattedValue.ToString() == value))
                            {
                                productosDataGridView.Rows[e.RowIndex].ErrorText =
                                    "El tipo de producto debe ser único para cada producto.";
                                e.Cancel = true;
                            }
                        }

                        // Validar que el precio sea un número positivo.
                        if (productosDataGridView.Columns[e.ColumnIndex].Name == "PrecioColumn")
                        {
                            decimal precio;
                            if (!decimal.TryParse(value, out precio) || precio < 0)
                            {
                                productosDataGridView.Rows[e.RowIndex].ErrorText =
                                    "La precio especificado debe ser un número positivo.";
                                e.Cancel = true;
                            }
                        }
                    }

                    // Validar que la cantidad a aumentar sea un número entero positivo.
                    if (productosDataGridView.Columns[e.ColumnIndex].Name == "AumentarCantidadColumn") 
                    {
                        int nuevaCantidad;
                        if (!int.TryParse(value, out nuevaCantidad) || nuevaCantidad < 0)
                        {
                            productosDataGridView.Rows[e.RowIndex].ErrorText =
                                "La cantidad a aumentar especificada debe ser un número entero positivo.";
                            e.Cancel = true;
                        }
                    }

                    // Validar que el nuevo precio sea un número entero positivo.
                    if (productosDataGridView.Columns[e.ColumnIndex].Name == "NuevoPrecioColumn")
                    { 
                        decimal nuevoprecio;
                        if (!decimal.TryParse(value, out nuevoprecio) || nuevoprecio < 0)
                        {
                            productosDataGridView.Rows[e.RowIndex].ErrorText =
                                "El nuevo precio especificado debe ser un número positivo.";
                            e.Cancel = true;
                        }
                        else if (nuevoprecio > 0 && 
                                Convert.ToInt32(productosDataGridView.Rows[e.RowIndex].
                                    Cells["AumentarCantidadColumn"].Value, CultureInfo.InvariantCulture) == 0)
                        {
                            productosDataGridView.Rows[e.RowIndex].ErrorText =
                                "Debe incrementar la cantidad para especificar un nuevo precio.";
                            e.Cancel = true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Entrada de productos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProductosDataGridViewCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Limpiar la fila de error en caso de que el usuario presione esc.
            productosDataGridView.Rows[e.RowIndex].ErrorText = String.Empty;

            // Calcular el importe del producto en la fila activa y del total.
            if (productosDataGridView.Columns[e.ColumnIndex].Name == "CantidadColumn" || 
                productosDataGridView.Columns[e.ColumnIndex].Name == "PrecioColumn" ||
                productosDataGridView.Columns[e.ColumnIndex].Name == "AumentarCantidadColumn" ||
                productosDataGridView.Columns[e.ColumnIndex].Name == "NuevoPrecioColumn")
            {
                var existente = productosDataGridView.Rows[e.RowIndex].Cells["ExistenteColumn"].Value != null &&
                                (bool) productosDataGridView.Rows[e.RowIndex].Cells["ExistenteColumn"].Value;

                int cant = existente &&
                           Convert.ToInt32(
                               productosDataGridView.Rows[e.RowIndex].Cells["AumentarCantidadColumn"].Value,
                               CultureInfo.InvariantCulture) > 0
                               ? Convert.ToInt32(
                                   productosDataGridView.Rows[e.RowIndex].Cells["AumentarCantidadColumn"].Value,
                                   CultureInfo.InvariantCulture)
                               : Convert.ToInt32(productosDataGridView.Rows[e.RowIndex].Cells["CantidadColumn"].Value,
                                                 CultureInfo.InvariantCulture);

                decimal precio = existente &&
                                 Convert.ToDecimal(
                                     productosDataGridView.Rows[e.RowIndex].Cells["NuevoPrecioColumn"].Value,
                                     CultureInfo.InvariantCulture) > 0
                                     ? (Convert.ToDecimal(
                                         productosDataGridView.Rows[e.RowIndex].Cells["PrecioColumn"].Value,
                                         CultureInfo.InvariantCulture) +
                                        Convert.ToDecimal(
                                            productosDataGridView.Rows[e.RowIndex].Cells["NuevoPrecioColumn"].Value,
                                            CultureInfo.InvariantCulture))/2
                                     : Convert.ToDecimal(
                                         productosDataGridView.Rows[e.RowIndex].Cells["PrecioColumn"].Value,
                                         CultureInfo.InvariantCulture);

                productosDataGridView.Rows[e.RowIndex].Cells["ImporteColumn"].Value = Math.Round(cant*precio, 2);

                _importeTotal = 0;
                foreach (DataGridViewRow row in productosDataGridView.Rows)
                    _importeTotal += Convert.ToDecimal(row.Cells["ImporteColumn"].Value, CultureInfo.InvariantCulture);

                importeTotalLabel.Text = @"Importe total: " + Math.Round(_importeTotal, 2);
            }
        }

        private void PagoEfectivoComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            if (pagoEfectivoComboBox.SelectedIndex == 0) return;

            pagoXFacturacheckBox.Checked = false;
            detallesClientLabel.Text = "";
            detallesClientLabel.Visible = false;
        }

        private void PagoXFacturacheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (!pagoXFacturacheckBox.Checked)
            {
                detallesClientLabel.Visible = false;
                detallesClientLabel.Text = "";
                _proveedorId = 0;
                return;
            }

            pagoEfectivoComboBox.SelectedIndex = 0;
            detallesClientLabel.Visible = true;

            var proveedorForm = new ProveedorForm(_user);
            proveedorForm.ShowDialog();
            _proveedorId = proveedorForm.IdProveedor;
            if (_proveedorId == 0)
            {
                pagoXFacturacheckBox.Checked = false;
                detallesClientLabel.Visible = false;
            }
            else
            {
                try
                {
                    using (var entities = new papiro_finalEntities())
                    {
                        var prov = (entities.proveedor.Where(p => p.id == proveedorForm.IdProveedor)).Single();
                        detallesClientLabel.Text = "Proveedor: " + prov.nombre;
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                        exception.Message +
                        (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                        @"Entrada de productos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ProductosDataGridViewRowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            productosDataGridView.Rows[e.RowIndex].Cells["ExistenteColumn"].Value = false;
            
            productosDataGridView.Rows[e.RowIndex].Cells["ImporteColumn"].Value = 0m;

            DataGridViewCell aumentarCantidadCell =
                productosDataGridView.Rows[e.RowIndex].Cells["AumentarCantidadColumn"];
            aumentarCantidadCell.Value = 0;
            aumentarCantidadCell.ReadOnly = true;

            DataGridViewCell nuevoPrecioCell = productosDataGridView.Rows[e.RowIndex].Cells["NuevoPrecioColumn"];
            nuevoPrecioCell.Value = 0m;
            nuevoPrecioCell.ReadOnly = true;
        }

        private void ProductosDataGridViewRowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (productosDataGridView.RowCount <= 0) return;
            _importeTotal = 0;
            foreach (DataGridViewRow row in productosDataGridView.Rows)
                _importeTotal += Convert.ToDecimal(row.Cells["ImporteColumn"].Value, CultureInfo.InvariantCulture);

            importeTotalLabel.Text = @"Importe total: " + Math.Round(_importeTotal, 2);
        }

        private void AgregarExistentetoolStripButtonClick(object sender, EventArgs e)
        {
            // Agregar al dataGrid el prod para modificar cantidad o precio.

            if (codPProdExisttoolStripComboBox.SelectedIndex < 1)
            {
                MessageBox.Show(@"Debe seleccionar el código del producto.", @"Entrada de productos",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            if (productosDataGridView.Rows.Cast<DataGridViewRow>().
                    Any(row => 
                        row.Cells["ExistenteColumn"].Value != null &&
                        (bool) row.Cells["ExistenteColumn"].Value &&
                        (string) row.Cells["CodigoColumn"].Value == codPProdExisttoolStripComboBox.Text))
            {
                MessageBox.Show(@"El producto al que desea dar entrada ya se encuentra listado.",
                                @"Entrada de productos",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    producto product =
                        entities.producto.Where(prod => prod.codigo == codPProdExisttoolStripComboBox.Text.Trim()).
                            Single();
                    _nuevosProductos.Add(product);

                    // Guardar los valores de las columnas no bindiadas.
                    var existenteList = new List<object>();
                    var tipoProdList = new List<object>();
                    var tipoUmList = new List<object>();
                    var importeList = new List<object>();
                    var aumentarCantList = new List<object>();
                    var nuevoPrecioList = new List<object>();
                    foreach(DataGridViewRow row in productosDataGridView.Rows)
                    {
                        existenteList.Add(row.Cells["ExistenteColumn"].Value);
                        tipoProdList.Add(row.Cells["TipoProductoColumn"].Value);
                        tipoUmList.Add(row.Cells["UnidadMedidaColumn"].Value);
                        importeList.Add(row.Cells["ImporteColumn"].Value);
                        aumentarCantList.Add(row.Cells["AumentarCantidadColumn"].Value);
                        nuevoPrecioList.Add(row.Cells["NuevoPrecioColumn"].Value);
                    }

                    bindingSource.ResetBindings(false);

                    // Escribir los valores guardados.
                    int count = existenteList.Count;
                    for (int index = 0; index < count; index++)
                    {
                        DataGridViewRow row = productosDataGridView.Rows[index];
                        row.Cells["ExistenteColumn"].Value = existenteList[index];
                        row.Cells["TipoProductoColumn"].Value = tipoProdList[index];
                        row.Cells["UnidadMedidaColumn"].Value = tipoUmList[index];
                        row.Cells["ImporteColumn"].Value = importeList[index];
                        row.Cells["AumentarCantidadColumn"].Value = aumentarCantList[index];
                        row.Cells["NuevoPrecioColumn"].Value = nuevoPrecioList[index];
                    }

                    DataGridViewRow lastAdded = productosDataGridView.Rows[productosDataGridView.Rows.Count - 2];
                    lastAdded.Cells["ExistenteColumn"].Value = true;
                    lastAdded.Cells["ImporteColumn"].Value =
                        Math.Round(
                            Convert.ToInt32(lastAdded.Cells["CantidadColumn"].Value, CultureInfo.InvariantCulture)*
                            Convert.ToDecimal(lastAdded.Cells["PrecioColumn"].Value, CultureInfo.InvariantCulture), 2);
                    lastAdded.Cells["AumentarCantidadColumn"].Value = 0;
                    lastAdded.Cells["AumentarCantidadColumn"].ReadOnly = false;
                    lastAdded.Cells["NuevoPrecioColumn"].Value = 0m;
                    lastAdded.Cells["NuevoPrecioColumn"].ReadOnly = false;
                    lastAdded.Cells["TipoProductoColumn"].Value = product.tipo_producto.valor;
                    lastAdded.Cells["UnidadMedidaColumn"].Value = product.unidad_medida.siglas;

                    lastAdded.Cells["CodigoColumn"].ReadOnly = true;
                    lastAdded.Cells["NombreColumn"].ReadOnly = true;
                    lastAdded.Cells["TipoProductoColumn"].ReadOnly = true;
                    lastAdded.Cells["CantidadColumn"].ReadOnly = true;
                    lastAdded.Cells["UnidadMedidaColumn"].ReadOnly = true;
                    lastAdded.Cells["PrecioColumn"].ReadOnly = true;  
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Entrada de productos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CodPProdExisttoolStripComboBoxLeave(object sender, EventArgs e)
        {
            if (codPProdExisttoolStripComboBox.SelectedIndex == -1)
                codPProdExisttoolStripComboBox.SelectedIndex = 0;
        }

        private void productosDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void fechaDateTimePicker_ValueChanged(object sender, EventArgs e)
        {

        }

        private void descripttextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void importeTotalLabel_Click(object sender, EventArgs e)
        {

        }

        private void detallesClientLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
