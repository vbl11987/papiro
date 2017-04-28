using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class EntradaActivos : Form
    {
        private readonly usuarios _user;

        private int _proveedorId;

        private readonly List<int> _pagoEfectivoId;

        private readonly List<activos_fijos> _nuevosActivos = new List<activos_fijos>();

        private decimal _importeTotal;

        public EntradaActivos(usuarios user)
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
                    foreach (var activoF in entities.tipo_activos_fijos) TipoProductoColumn.Items.Add(activoF.valor);

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
                    foreach (var activoF in entities.activos_fijos)
                    {
                        codPProdExisttoolStripComboBox.Items.Add(activoF.codigo);
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
                    @"Entrada de activos fijos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EntradaActivos_Load(object sender, EventArgs e)
        {
            bindingSource.DataSource = _nuevosActivos;

            //Font font = productosDataGridView.Font;
            //productosDataGridView.Font = new Font(font.Name, 10, font.Style, font.Unit);
            productosDataGridView.AutoGenerateColumns = false;
            productosDataGridView.DataSource = bindingSource;

            // Asociar las columnas del grid.

            CodigoColumn.DataPropertyName = "codigo";

            NombreColumn.DataPropertyName = "nombre";

            TipoProductoColumn.ValueType = typeof(string);
            TipoProductoColumn.AutoComplete = true;
            //TipoProductoColumn.DisplayMember = "valor";
            //TipoProductoColumn.ValueMember = "Self";

            CantidadColumn.DataPropertyName = "cantidad";

            UnidadMedidaColumn.ValueType = typeof(string);
            UnidadMedidaColumn.AutoComplete = true;
            //UnidadMedidaColumn.DisplayMember = "siglas";
            //UnidadMedidaColumn.ValueMember = "Self";

            PrecioColumn.DataPropertyName = "precio";

            bindingNavigator.BindingSource = bindingSource;

            Reload();

            importeTotalLabel.Text = @"Importe total: " + Math.Round(_importeTotal, 2);
        }

        private void CancelarButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string ToString(activos_fijos activoF)
        {
            return
                string.Format(
                    "Producto con Id: {0}, Código: {1}, Nombre: {2}, Tipo de activo fijo: {3}, Cantidad: {4}, Precio: {5}, Fecha de entrada: {6}",
                    activoF.id, activoF.codigo, activoF.nombre, activoF.tipo_activos_fijos.valor, activoF.cantidad,
                    Math.Round(activoF.precio, 2),
                    fechaDateTimePicker.Value);
        }

        private void AceptarButton_Click(object sender, EventArgs e)
        {
            // Validar que se ha realizado el pago.
            if (!pagoXFacturacheckBox.Checked && pagoEfectivoComboBox.SelectedIndex == 0)
            {
                MessageBox.Show(@"Debe especificar la forma de pago de los activos fijos.", @"Entrada de activos fijos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar que al menos se haya registrado un producto.
            if (_nuevosActivos.Count == 0)
            {
                MessageBox.Show(@"Debe registrar al menos un activo fijo.", @"Entrada de activos fijos", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            // Validar que la fecha de entrada no sea mayor que la fecha actual.
            if (fechaDateTimePicker.Value > DateTime.Now)
            {
                MessageBox.Show(@"La fecha de entrada del activo dijo debe ser menor o igual que la fecha de hoy.",
                                @"Entrada de activos fijos", MessageBoxButtons.OK,
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
                row.ErrorText = "Existen valores no válidos para registrar el activo fijo.";
                validationErrors = true;
            }
            if (validationErrors)
            {
                MessageBox.Show(@"Existen valores de celda no válidos.", @"Entrada de activos fijos", MessageBoxButtons.OK,
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

                    // Activos fijos
                    entities.AddTosubmayor_activos_fijo_tangible(new submayor_activos_fijo_tangible()
                                                          {
                                                              fecha = DateTime.Now,
                                                              id_usuario = _user.id,
                                                              descripcion = "Se le da entrada a nuevos activos fijos.",
                                                              saldo = balance.activos_fijo_tangible + _importeTotal,
                                                              debito = _importeTotal
                                                          });
                    balance.activos_fijo_tangible += _importeTotal;
                    
                    if (pagoXFacturacheckBox.Checked)
                    {
                        entities.AddTocuentas_por_pagar(new cuentas_por_pagar
                                                            {
                                                                descripcion = "Se le da entrada a activos fijos.",
                                                                monto = _importeTotal,
                                                                fecha = fechaDateTimePicker.Value,
                                                                ejecutada = 0
                                                            });

                        entities.AddTosubmayor_cuentas_por_pagar(new submayor_cuentas_por_pagar
                                                                     {
                                                                         fecha = DateTime.Now,
                                                                         id_usuario = _user.id,
                                                                         descripcion =
                                                                             "Se le da entrada a nuevos activos fijos.",
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
                                        "No hay dinero suficiente en la caja para darle entrada a los activos fijos",
                                        @"Entrada de activos fijos", MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    return;
                                }
                                entities.AddTosubmayor_efectivo_caja(new submayor_efectivo_caja
                                                                         {
                                                                             fecha = DateTime.Now,
                                                                             id_usuario = _user.id,
                                                                             descripcion =
                                                                                 "Se le da entrada a nuevos activos fijos.",
                                                                             saldo = balance.efectivo_caja - _importeTotal,
                                                                             credito = _importeTotal
                                                                         });
                                balance.efectivo_caja -= _importeTotal;
                                break;
                            case 2:
                                if (balance.efectivo_banco - _importeTotal < 0)
                                {
                                    MessageBox.Show(
                                        "No hay dinero suficiente en el banco para darle entrada a los activos fijos",
                                        @"Entrada de activos fijos", MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    return;
                                }
                                entities.AddTosubmayor_efectivo_banco(new submayor_efectivo_banco
                                                                          {
                                                                              fecha = DateTime.Now,
                                                                              id_usuario = _user.id,
                                                                              descripcion =
                                                                                  "Se le da entrada a nuevos activos fijos.",
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
                    var auxProdList = new List<activos_fijos>();
                    foreach (var activo in _nuevosActivos)
                    {
                        activos_fijos auxAtivF;
                        if (productosDataGridView.Rows[index].Cells["ExistenteColumn"].Value != null &&
                            (bool) productosDataGridView.Rows[index].Cells["ExistenteColumn"].Value)
                        {
                            auxAtivF =
                                (activos_fijos)
                                entities.GetObjectByKey(new EntityKey("papiro_finalEntities.activos_fijos", "id", activo.id));
                            var cantidadAumentar =
                                Convert.ToInt32(
                                    productosDataGridView.Rows[index].Cells["AumentarCantidadColumn"].Value,
                                    CultureInfo.InvariantCulture);
                            if (cantidadAumentar > 0)
                            {
                                auxAtivF.cantidad += cantidadAumentar;
                                var nuevoprecio =
                                    Convert.ToDecimal(
                                        productosDataGridView.Rows[index].Cells["NuevoPrecioColumn"].Value,
                                        CultureInfo.InvariantCulture);
                                if (nuevoprecio > 0)
                                    auxAtivF.precio = (auxAtivF.precio + nuevoprecio)/2;
                            }
                        }
                        else
                        {
                            auxAtivF = new activos_fijos()
                                          {
                                              codigo = activo.codigo,
                                              nombre = activo.nombre,
                                              id_tipo_activo_fijo = 
                                                  entities.tipo_activos_fijos.ToList().Find(
                                                      tp =>
                                                      tp.valor ==
                                                      (string)
                                                      productosDataGridView.Rows[index].Cells["TipoProductoColumn"].
                                                          Value).id,
                                              precio = activo.precio,
                                              cantidad = activo.cantidad,
                                              cantidad_real = 0,
                                              id_unidad_medida = entities.unidad_medida.ToList().Find(
                                                  um =>
                                                  um.siglas ==
                                                  (string)
                                                  productosDataGridView.Rows[index].Cells["UnidadMedidaColumn"].
                                                      Value).id
                                          };
                            entrada.activos_fijos.Add(auxAtivF);
                        }
                        auxProdList.Add(auxAtivF);
                        
                        index++;
                    }
                    entities.AddToentrada(entrada);
                    entities.SaveChanges();

                    index = 0;
                    foreach (var activo in auxProdList)
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
                                                                : "Se agregó un nuevo ") + ToString(activo)
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
                    @"Entrada de activos fijos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Close();
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
                        @"Entrada de activos fijos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

        private void productosDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
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
                                (bool)productosDataGridView.Rows[e.RowIndex].Cells["ExistenteColumn"].Value;

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
                                            CultureInfo.InvariantCulture)) / 2
                                     : Convert.ToDecimal(
                                         productosDataGridView.Rows[e.RowIndex].Cells["PrecioColumn"].Value,
                                         CultureInfo.InvariantCulture);

                productosDataGridView.Rows[e.RowIndex].Cells["ImporteColumn"].Value = Math.Round(cant * precio, 2);

                _importeTotal = 0;
                foreach (DataGridViewRow row in productosDataGridView.Rows)
                    _importeTotal += Convert.ToDecimal(row.Cells["ImporteColumn"].Value, CultureInfo.InvariantCulture);

                importeTotalLabel.Text = @"Importe total: " + Math.Round(_importeTotal, 2);
            }
        }

        private void productosDataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    string value = e.FormattedValue == null ? "" : e.FormattedValue.ToString().Trim();

                    if (productosDataGridView.Rows[e.RowIndex].Cells["ExistenteColumn"].Value != null &&
                        (bool)productosDataGridView.Rows[e.RowIndex].Cells["ExistenteColumn"].Value == false)
                    {
                        // Validar que el código del activo no está vacío y no existe.
                        if (productosDataGridView.Columns[e.ColumnIndex].Name == "CodigoColumn")
                        {
                            if (value == "")
                            {
                                productosDataGridView.Rows[e.RowIndex].ErrorText =
                                    "El código del activo fijo no puede estar vacío.";
                                e.Cancel = true;
                            }
                            else if (entities.activos_fijos.Any(prod => prod.codigo == value.Trim()))
                            {
                                {
                                    productosDataGridView.Rows[e.RowIndex].ErrorText =
                                        "Ya existe en el almacén un activo fijo con el código especificado.";
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
                                    "El nombre del activo fijo no puede estar vacío.";
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
                                    "Debe seleccionar un tipo de activo fijo.";
                                e.Cancel = true;
                            }
                            else if (entities.activos_fijos.Any(prod => prod.tipo_activos_fijos.valor == value) ||
                                     productosDataGridView.Rows.Cast<DataGridViewRow>().Any(
                                         row =>
                                         row.Index != e.RowIndex &&
                                         row.Cells["TipoProductoColumn"].FormattedValue.ToString() == value))
                            {
                                productosDataGridView.Rows[e.RowIndex].ErrorText =
                                    "El tipo de activo fijo debe ser único para cada activo fijo.";
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
                    @"Entrada de activos fijos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void productosDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
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

        private void productosDataGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (productosDataGridView.RowCount <= 0) return;
            _importeTotal = 0;
            foreach (DataGridViewRow row in productosDataGridView.Rows)
                _importeTotal += Convert.ToDecimal(row.Cells["ImporteColumn"].Value, CultureInfo.InvariantCulture);

            importeTotalLabel.Text = @"Importe total: " + Math.Round(_importeTotal, 2);
        }

        private void agregarExistentetoolStripButton_Click(object sender, EventArgs e)
        {
            // Agregar al dataGrid el prod para modificar cantidad o precio.

            if (codPProdExisttoolStripComboBox.SelectedIndex < 1)
            {
                MessageBox.Show(@"Debe seleccionar el código del activo fijo.", @"Entrada de activos fijos",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            if (productosDataGridView.Rows.Cast<DataGridViewRow>().
                    Any(row =>
                        row.Cells["ExistenteColumn"].Value != null &&
                        (bool)row.Cells["ExistenteColumn"].Value &&
                        (string)row.Cells["CodigoColumn"].Value == codPProdExisttoolStripComboBox.Text))
            {
                MessageBox.Show(@"El activo fijo al que desea dar entrada ya se encuentra listado.",
                                @"Entrada de activos fijos",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    activos_fijos activoF =
                        entities.activos_fijos.Where(prod => prod.codigo == codPProdExisttoolStripComboBox.Text.Trim()).
                            Single();
                    _nuevosActivos.Add(activoF);

                    // Guardar los valores de las columnas no bindiadas.
                    var existenteList = new List<object>();
                    var tipoProdList = new List<object>();
                    var tipoUmList = new List<object>();
                    var importeList = new List<object>();
                    var aumentarCantList = new List<object>();
                    var nuevoPrecioList = new List<object>();
                    foreach (DataGridViewRow row in productosDataGridView.Rows)
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
                            Convert.ToInt32(lastAdded.Cells["CantidadColumn"].Value, CultureInfo.InvariantCulture) *
                            Convert.ToDecimal(lastAdded.Cells["PrecioColumn"].Value, CultureInfo.InvariantCulture), 2);
                    lastAdded.Cells["AumentarCantidadColumn"].Value = 0;
                    lastAdded.Cells["AumentarCantidadColumn"].ReadOnly = false;
                    lastAdded.Cells["NuevoPrecioColumn"].Value = 0m;
                    lastAdded.Cells["NuevoPrecioColumn"].ReadOnly = false;
                    lastAdded.Cells["TipoProductoColumn"].Value = activoF.tipo_activos_fijos.valor;
                    lastAdded.Cells["UnidadMedidaColumn"].Value = activoF.unidad_medida.siglas;

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
                    @"Entrada de activos fijos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
