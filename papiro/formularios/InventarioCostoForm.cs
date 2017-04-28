using System;
using System.Drawing;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class InventarioCostoForm : Form
    {
        private readonly usuarios _user;

        private decimal _importeTotal;

        public InventarioCostoForm(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _importeTotal = 0;
        }

        private void Reload()
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    _importeTotal = 0;
                    // Unidades de medida.
                    productosDataGridView.Rows.Clear();
                    foreach (var producto in entities.producto)
                    {
                        decimal importe = producto.cantidad*producto.precio;
                        productosDataGridView.Rows.Add(new object[]
                                                           {
                                                               producto.codigo, producto.nombre,
                                                               producto.tipo_producto.valor, producto.cantidad,
                                                               producto.unidad_medida.siglas,
                                                               Math.Round(producto.precio, 2),
                                                               Math.Round(importe, 2)
                                                           });
                        _importeTotal += importe;
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Costo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EntradaProductosFormLoad(object sender, EventArgs e)
        {
            Font font = productosDataGridView.Font;
            productosDataGridView.Font = new Font(font.Name, 10, font.Style, font.Unit);
            productosDataGridView.AutoGenerateColumns = false;

            Reload();

            importeTotalLabel.Text = @"Importe total: " + Math.Round(_importeTotal, 2);
        }

        private void ProductosDataGridViewCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //string value = e.FormattedValue == null ? "" : e.FormattedValue.ToString().Trim();

            //// Validar que el nombre del producto no está vacío y no exista.
            //if (productosDataGridView.Columns[e.ColumnIndex].Name == "NombreColumn")
            //{
            //    if (value == "")
            //    {
            //        productosDataGridView.Rows[e.RowIndex].ErrorText =
            //            "El nombre del producto no puede estar vacío.";
            //        e.Cancel = true;
            //    }
            //    else if (productosDataGridView.Rows.Cast<DataGridViewRow>().Any(
            //                row =>
            //                row.Index != e.RowIndex &&
            //                (row.Cells["NombreColumn"].Value == null ? "" : row.Cells["NombreColumn"].Value.ToString()) ==
            //                value))
            //    {
            //        {
            //            productosDataGridView.Rows[e.RowIndex].ErrorText =
            //                "Ya introdujo un producto con el nombre especificado.";
            //            e.Cancel = true;
            //        }
            //    }
            //}

            //// Validar que la cantidad sea un número entero positivo.
            //if (productosDataGridView.Columns[e.ColumnIndex].Name == "CantidadColumn")
            //{
            //    int cantidad;
            //    if (!int.TryParse(value, out cantidad) || cantidad < 0)
            //    {
            //        productosDataGridView.Rows[e.RowIndex].ErrorText =
            //            "La cantidad especificada debe ser un número entero positivo.";
            //        e.Cancel = true;
            //    }
            //}

            //// Validar que la unidad de medida este definida.
            //if (productosDataGridView.Columns[e.ColumnIndex].Name == "UnidadMedidaColumn")
            //{
            //    if (value == "(Seleccione)")
            //    {
            //        productosDataGridView.Rows[e.RowIndex].ErrorText = "Debe seleccionar una unidad de medida.";
            //        e.Cancel = true;
            //    }
            //}

            //// Validar que el precio sea un número positivo.
            //if (productosDataGridView.Columns[e.ColumnIndex].Name == "PrecioColumn")
            //{
            //    decimal precio;
            //    if (!decimal.TryParse(value, out precio) || precio < 0)
            //    {
            //        productosDataGridView.Rows[e.RowIndex].ErrorText =
            //            "La precio especificado debe ser un número positivo.";
            //        e.Cancel = true;
            //    }
            //}
        }

        private void ProductosDataGridViewCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //// Limpiar la fila de error en caso de que el usuario presione esc.
            //productosDataGridView.Rows[e.RowIndex].ErrorText = String.Empty;

            //// Calcular el importe del producto en la fila activa y del total.
            //if (productosDataGridView.Columns[e.ColumnIndex].Name == "CantidadColumn" || 
            //    productosDataGridView.Columns[e.ColumnIndex].Name == "PrecioColumn")
            //{
            //    productosDataGridView.Rows[e.RowIndex].Cells["ImporteColumn"].Value =
            //        Convert.ToInt32(productosDataGridView.Rows[e.RowIndex].Cells["CantidadColumn"].Value)*
            //        Convert.ToDecimal(productosDataGridView.Rows[e.RowIndex].Cells["PrecioColumn"].Value);

            //    _importeTotal = 0;
            //    foreach (DataGridViewRow row in productosDataGridView.Rows)
            //        _importeTotal += Convert.ToDecimal(row.Cells["ImporteColumn"].Value);

            //    importeTotalLabel.Text = @"Importe total: " + _importeTotal;
            //}
        }
    }
}
