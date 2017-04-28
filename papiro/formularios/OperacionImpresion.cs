using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class OperacionImpresion : Form
    {
        private readonly usuarios _user;

        private readonly papiro_finalEntities _entities;

        private readonly List<int> _clientesId;

        private readonly List<int> _tipoOperacionId;

        private readonly List<int> _tipoProductoId;

        private readonly List<int> _comercialId;

        private readonly List<int> _operadorId;

        private readonly operaciones _operacionPendiente;

        private readonly bool _ejecutarOperacionPendiente;

        private readonly List<int> _proveedorIds;

        private readonly operaciones _operacion;

        public OperacionImpresion(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _entities = new papiro_finalEntities();
            _clientesId = new List<int>();
            _tipoOperacionId = new List<int> { -1 };
            _tipoProductoId = new List<int> { -1 };
            _comercialId = new List<int> { -1 };
            _operadorId = new List<int> { -1 };
            _operacionPendiente = null;
            _ejecutarOperacionPendiente = false;
            _proveedorIds = new List<int> { 0 };
            _operacion = new operaciones();
        }

        public OperacionImpresion(usuarios user, operaciones operacionPendiente)
        {
            InitializeComponent();
            _user = user;
            _operacionPendiente = operacionPendiente;
            _ejecutarOperacionPendiente = true;
            _entities = new papiro_finalEntities();
            _clientesId = new List<int>();
            _tipoOperacionId = new List<int> { -1 };
            _tipoProductoId = new List<int> { -1 };
            _comercialId = new List<int> { -1 };
            _operadorId = new List<int> { -1 };
            _proveedorIds = new List<int> { 0 };
            _operacion = new operaciones();
        }

        private void OperacionLoad(object sender, EventArgs e)
        {
            foreach (var usuario in _entities.usuarios.Where(u => u.activado))
            {
                if (usuario.rol.Any(rol => rol.id == 1)) continue;

                if (usuario.rol.Any(rol => rol.id == 5))
                {
                    comercialcomboBox.Items.Add(usuario.nombre);
                    _comercialId.Add(usuario.id);
                }

                if (usuario.rol.Any(rol => rol.id == 6))
                {
                    operadorcomboBox.Items.Add(usuario.nombre);
                    _operadorId.Add(usuario.id);
                }
            }

            // Operaciones de 3eros.
            TipoOperacion3eroColumn.Items.Add("(Seleccione)");
            TipoOperacion3eroColumn.DefaultCellStyle.NullValue = "(Seleccione)";
            foreach (var tipoOperacion in _entities.tipo_operacion_3ero)
                TipoOperacion3eroColumn.Items.Add(tipoOperacion.valor);

            operaciones3erosdataGridView.Rows[0].Cells["PrecioColumn"].Value = 0m;
            operaciones3erosdataGridView.Rows[0].Cells["ImporteColumn"].Value = 0m;
            operaciones3erosdataGridView.Rows[0].Cells["CantidadColumn"].Value = 0;

            if (_ejecutarOperacionPendiente)
            {
                if (_operacionPendiente == null) return;

                // Cliente.
                _clientesId.Add(_operacionPendiente.cliente.id);
                cb_cliente.Items.Add(_operacionPendiente.cliente.nombre);
                cb_cliente.SelectedIndex = 0;

                if (_operacionPendiente.contrato != null)
                {
                    contratotextBox.Text = _operacionPendiente.cliente.nombre + " - " +
                                           _operacionPendiente.contrato.descripcion;
                }
                else
                    contratotextBox.Text = "El cliente " + _operacionPendiente.cliente.nombre + " no posee contratos.";

                groupBox1.Enabled = false;
                addclientbutton.Enabled = false;

                // Tipo de operación.
                if (_operacionPendiente.tipo_operacion != null)
                {
                    _tipoOperacionId.Add(_operacionPendiente.tipo_operacion.id);
                    cb_tipo_operacion.Items.Add(_operacionPendiente.tipo_operacion.valor);
                    cb_tipo_operacion.SelectedIndex = 1;
                }
                cb_tipo_operacion.Enabled = false;

                // Tipo de producto.
                if (_operacionPendiente.tipo_producto != null)
                {
                    _tipoProductoId.Add(_operacionPendiente.tipo_producto.id);
                    cb_tipo_producto.Items.Add(_operacionPendiente.tipo_producto.valor);
                    cb_tipo_producto.SelectedIndex = 1;
                }
                cb_tipo_producto.Enabled = false;

                // Descripción.
                descripciontextBox.Text = _operacionPendiente.descripcion;

                return;
            }


            //Arreglar esto ponerlo con 3 radio buttons
            rBCasual.Checked = true;
            //foreach (cliente client in _entities.cliente)
            //{
            //    _clientesId.Add(client.id);
            //    cb_cliente.Items.Add(client.nombre);
            //}
            //cb_cliente.SelectedIndex = 0;

            foreach (tipo_operacion tipoOperacion in _entities.tipo_operacion)
            {
                _tipoOperacionId.Add(tipoOperacion.id);
                cb_tipo_operacion.Items.Add(tipoOperacion.valor);
            }

            foreach (tipo_producto tipoProducto in _entities.tipo_producto)
            {
                _tipoProductoId.Add(tipoProducto.id);
                cb_tipo_producto.Items.Add(tipoProducto.valor);
            }
        }

        private void CalcularMontoTotal()
        {
            montoTotal.Text =
                Math.Round(valorOpnumericUpDown.Value * cantidad.Value, 2).ToString(CultureInfo.InvariantCulture);
        }

        private void CbTipoOperacionSelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_tipo_operacion.SelectedIndex < 1)
            {
                valorOpnumericUpDown.Value = 0;
                CalcularMontoTotal();
                return;
            }

            foreach (tipo_operacion tipoOperacion in _entities.tipo_operacion)
            {
                if (tipoOperacion.id != _tipoOperacionId[cb_tipo_operacion.SelectedIndex]) continue;
                valorOpnumericUpDown.Value = tipoOperacion.valor_operacion;
                CalcularMontoTotal();
                break;
            }
        }

        private static string OperacionToString(operaciones operacion)
        {
            string str = "Operación de impresión con Id: " + operacion.id + ", Cliente: " + operacion.cliente.nombre;
            if (operacion.contrato != null)
                str += ", Contrato: " + operacion.contrato.descripcion;
            str += ", Tipo de operación: " + operacion.tipo_operacion.valor + ", Valor operación: " +
                   Math.Round(operacion.valor_tipo_operacion.Value, 2) + ", Monto: " + Math.Round(operacion.monto, 2);
            if (operacion.id_comercial != null)
                str += ", Comercial: " + operacion.usuarios.nombre;
            if (operacion.id_operador != null)
                str += ", Operador: " + operacion.usuarios2.nombre;
            str += ", Tipo de producto: " + operacion.tipo_producto.valor + ", Cobrada: " +
                   (operacion.pendiente == 0 ? "Si" : "No");
            return str;
        }

        private static string CuentaPorCobrarToSTring(cuentas_por_cobrar cuentasPorCobrar)
        {
            return string.Format("Cuenta por cobrar con Id de operación: {0}, Cliente: {1}, Ejecutada: {2}",
                                 cuentasPorCobrar.id_operacion, cuentasPorCobrar.cliente.nombre,
                                 cuentasPorCobrar.ejecutada == 1 ? "Si" : "No");
        }

        private void AceptarClick(object sender, EventArgs e)
        {
            try
            {
                //Validacion de los datos
                string validationMessage = "";

                // Verificar el cliente.
                //if (cb_cliente.SelectedIndex < 0)
                //    validationMessage = "Seleccione el cliente al que se le ofrece el servicio.";

                if (cb_tipo_operacion.SelectedIndex < 1)
                    validationMessage = "Seleccione el tipo de operación que se va a realizar.";

                //verifico la forma de pago
                if (cb_tipo_producto.SelectedIndex < 1)
                    validationMessage += "\nDebe seleccionar el tipo de producto.";

                if (validationMessage != "")
                {
                    MessageBox.Show(validationMessage, @"Registrar operación", MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                    return;
                }

                // Validar que cada una de las celdas tenga un valor correcto.
                bool validationErrors = false;
                foreach (DataGridViewRow row in
                    operaciones3erosdataGridView.Rows.Cast<DataGridViewRow>().
                        Where(row => (from DataGridViewCell cell in row.Cells
                                      let value = cell.FormattedValue == null ? "" : cell.FormattedValue.ToString()
                                      where value == "" ||
                                            (operaciones3erosdataGridView.Columns[cell.ColumnIndex].Name ==
                                                "TipoOperacion3eroColumn" && value == "(Seleccione)")
                                      select cell).Any() && row.Index != operaciones3erosdataGridView.RowCount - 1))
                {
                    row.ErrorText = "Existen valores no válidos para registrar las operaciones realizadas por terceros.";
                    validationErrors = true;
                }
                if (validationErrors)
                {
                    MessageBox.Show(@"Existen valores de celda no válidos.", @"Operación de impresión", MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

                decimal montTotal = apagar3eronumericUpDown.Value != 0 && operaciones3erosdataGridView.RowCount > 1
                                        ? apagar3eronumericUpDown.Value
                                        : Convert.ToDecimal(montoTotal.Text.Trim(), CultureInfo.InvariantCulture);

                _operacion.id_cliente = _clientesId[cb_cliente.SelectedIndex];
                _operacion.id_tipo_operacion = _tipoOperacionId[cb_tipo_operacion.SelectedIndex];
                _operacion.valor_tipo_operacion = valorOpnumericUpDown.Value;
                _operacion.id_tipo_producto = _tipoProductoId[cb_tipo_producto.SelectedIndex];
                _operacion.monto = montTotal;
                _operacion.fecha = DateTime.Now;
                _operacion.pendiente = 1;
                _operacion.descripcion = descripciontextBox.Text;
                _operacion.pendiente_imprimir = 0;
                _operacion.id_usuario = _user.id;
                _operacion.cantidad = (int)cantidad.Value;
                _operacion.gasto = 0;

                var contract =
                    _entities.contrato.FirstOrDefault(c => c.terminado == 0 && c.id_cliente == _operacion.id_cliente);
                if (contract != null)
                    _operacion.id_contrato = contract.id;

                cuentas_por_cobrar cuentasPorCobrar = null;

                // Obtener último balance para actualizar.
                balance balance = _entities.balance.ToList().Last();

                // Obtener el producto por su tipo.
                int idTipProd = _tipoProductoId[cb_tipo_producto.SelectedIndex];
                producto producto =
                    _entities.producto.Where(prod => prod.id_tipo_producto == idTipProd).SingleOrDefault();

                if (producto == null)
                {
                    MessageBox.Show("No hay productos del tipo de producto especificado", @"Registrar operación",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

                if (producto.cantidad < cantidad.Value)
                {
                    MessageBox.Show("No hay en el almacén la cantidad de productos necesaria", @"Registrar operación",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

                producto.cantidad -= (int)cantidad.Value;

                if (producto.cantidad < 50)
                {
                    foreach (var usuario in _entities.usuarios.Where(u => u.rol.Any(r => r.id == 1 || r.id == 2)))
                    {
                        _entities.AddToavisos(new avisos
                        {
                            descripcion =
                                "Quedan en el almacéen solo " + producto.cantidad +
                                " productos de tipo " + producto.tipo_producto.valor,
                            fecha = DateTime.Now,
                            id_usuario = usuario.id
                        });
                    }
                }

                // Inventario
                decimal precioTotal = producto.precio * cantidad.Value;
                _entities.AddTosubmayor_inventario(new submayor_inventario
                {
                    fecha = DateTime.Now,
                    id_usuario = _user.id,
                    descripcion = "Se realiza operación de impresión.",
                    saldo = balance.inventario - precioTotal,
                    credito = precioTotal
                });
                balance.inventario -= precioTotal;

                // Costo
                _entities.AddTosubmayor_costo(new submayor_costo
                {
                    fecha = DateTime.Now,
                    id_usuario = _user.id,
                    descripcion = "Se realiza operación de impresión.",
                    saldo = balance.costo + precioTotal,
                    debito = precioTotal
                });
                balance.costo += precioTotal;

                _operacion.costo = precioTotal;

                if (finalizada.Checked)
                {
                    if (montTotal != 0)
                    {
                        // Ingreso
                        _entities.AddTosubmayor_ingreso(new submayor_ingreso
                        {
                            fecha = DateTime.Now,
                            id_usuario = _user.id,
                            descripcion = "Se realiza operación de impresión.",
                            saldo = balance.ingreso + montTotal,
                            credito = montTotal
                        });
                        balance.ingreso += montTotal;

                        // Efectivo en caja
                        _entities.AddTosubmayor_efectivo_caja(new submayor_efectivo_caja
                        {
                            fecha = DateTime.Now,
                            id_usuario = _user.id,
                            descripcion = "Se realiza operación de impresión.",
                            saldo = balance.efectivo_caja + montTotal,
                            debito = montTotal
                        });
                        balance.efectivo_caja += montTotal;
                    }
                    _operacion.pendiente = 0;
                }
                else
                {
                    cuentasPorCobrar = new cuentas_por_cobrar
                    {
                        id_cliente = _clientesId[cb_cliente.SelectedIndex],
                        ejecutada = 0
                    };
                    _operacion.cuentas_por_cobrar.Add(cuentasPorCobrar);

                    var client = _entities.cliente.FirstOrDefault(c => c.id == cuentasPorCobrar.id_cliente);
                    if (client == null) return;

                    _entities.AddTosubmayor_cuentas_cobrar(new submayor_cuentas_cobrar
                    {
                        fecha = DateTime.Now,
                        id_usuario = _user.id,
                        descripcion =
                            "Se registra cuenta por cobrar para el cliente " +
                            client.nombre,
                        saldo = balance.cuentas_por_cobrar + montTotal,
                        debito = montTotal
                    });
                    balance.cuentas_por_cobrar += montTotal;

                    // Ingreso
                    _entities.AddTosubmayor_ingreso(new submayor_ingreso
                    {
                        fecha = DateTime.Now,
                        id_usuario = _user.id,
                        descripcion = "Se realiza operación de impresión.",
                        saldo = balance.ingreso + montTotal,
                        credito = montTotal
                    });
                    balance.ingreso += montTotal;
                }

                // Registrar las operaciones realizadas por terceros.
                if (operaciones3erosdataGridView.RowCount > 1)
                {
                    for (int index = 0; index < operaciones3erosdataGridView.RowCount - 1; index++)
                    {
                        var tipOp3 =
                            (string)operaciones3erosdataGridView.Rows[index].Cells["TipoOperacion3eroColumn"].Value;
                        var tipoOperacion3Ero = _entities.tipo_operacion_3ero.First(op3 => op3.valor == tipOp3);
                        int cant =
                            Convert.ToInt32(operaciones3erosdataGridView.Rows[index].Cells["CantidadColumn"].Value);
                        decimal precio =
                            Convert.ToDecimal(operaciones3erosdataGridView.Rows[index].Cells["PrecioColumn"].Value);
                        int idProv = _proveedorIds[index];
                        _operacion.operacion_3ero.Add(new operacion_3ero
                        {
                            id_proveedor = idProv,
                            id_tipo_operacion_3ero = tipoOperacion3Ero.id,
                            cantidad = cant,
                            valor_operacion_3ero = precio
                        });
                        decimal importe = precio * cant;
                        proveedor prov = _entities.proveedor.First(p => p.id == idProv);

                        // Crear cuenta por pagar.
                        _operacion.cuentas_por_pagar.Add(new cuentas_por_pagar
                        {
                            monto = importe,
                            descripcion =
                                "Pagar a " + prov.nombre + " por la operación: " +
                                tipoOperacion3Ero.valor,
                            fecha = _operacion.fecha,
                            ejecutada = 0
                        });

                        // Actualizar el balance
                        _entities.AddTosubmayor_cuentas_por_pagar(new submayor_cuentas_por_pagar
                        {
                            fecha = DateTime.Now,
                            id_usuario = _user.id,
                            descripcion =
                                "Se realiza operación de terceros.",
                            saldo = balance.cuentas_por_pagar + importe,
                            credito = importe
                        });
                        balance.cuentas_por_pagar += importe;

                        //Actualizar gastos
                        _entities.AddTosubmayor_gasto(new submayor_gasto
                        {
                            fecha = DateTime.Now,
                            id_usuario = _user.id,
                            descripcion = "Se realiza operación de terceros.",
                            saldo = balance.gasto + importe,
                            debito = importe
                        });
                        balance.gasto += importe;

                        _operacion.gasto += importe;

                        // Registrar en bitacora
                        _entities.AddTobitacora(new bitacora
                        {
                            id_usuario = _user.id,
                            nombre_usuario = _user.login_nombre,
                            fecha = DateTime.Now,
                            accion_realizada =
                                "Se registró una cuenta por pagar a " +
                                prov.nombre + " por la operación: " +
                                tipoOperacion3Ero.valor
                        });
                        montTotal -= importe;
                    }
                }

                // Poner en el salario extra del user logueado el % por operación del monto.
                var user =
                    (usuarios)
                    _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.usuarios", "id", _user.id));
                decimal extra = montTotal * (decimal)user.por_ciento_operaciones / 100;
                _operacion.gasto += extra;
                user.salario_extra_operaciones += extra;

                if (comercialcomboBox.SelectedIndex > 0)
                {
                    _operacion.id_comercial = _comercialId[comercialcomboBox.SelectedIndex];
                    var comercial =
                        (usuarios)
                        _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.usuarios", "id",
                                                               _operacion.id_comercial));
                    extra = montTotal * (decimal)comercial.por_ciento_operaciones / 100;
                    _operacion.gasto += extra;
                    comercial.salario_extra_operaciones += extra;
                }

                if (operadorcomboBox.SelectedIndex > 0)
                {
                    _operacion.id_operador = _operadorId[operadorcomboBox.SelectedIndex];
                    var operador =
                        (usuarios)
                        _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.usuarios", "id",
                                                               _operacion.id_operador));
                    extra = montTotal * (decimal)operador.por_ciento_operaciones / 100;
                    _operacion.gasto += extra;
                    operador.salario_extra_operaciones += extra;
                }

                if(_ejecutarOperacionPendiente)
                {
                    if (_operacionPendiente.id_disenador != null && _operacionPendiente.id_disenador != 0)
                    {
                        var disennador = (usuarios)_entities.GetObjectByKey(new EntityKey("papiro_finalEntities.usuarios", "id", _operacionPendiente.id_disenador));
                        extra = montTotal * (decimal)disennador.por_ciento_operaciones / 100;
                        _operacion.gasto += extra;
                        disennador.salario_extra_operaciones += extra;
                        _operacion.id_disenador = _operacionPendiente.id_disenador;
                    }
                }

                _entities.AddTooperaciones(_operacion);

                _entities.SaveChanges();

                if (cuentasPorCobrar != null)
                {
                    var bit = new bitacora
                    {
                        id_usuario = _user.id,
                        nombre_usuario = _user.login_nombre,
                        fecha = DateTime.Now,
                        accion_realizada = "Se registró una " + CuentaPorCobrarToSTring(cuentasPorCobrar)
                    };
                    _entities.AddTobitacora(bit);
                }

                //se guarda en bitacora
                var b = new bitacora
                {
                    id_usuario = _user.id,
                    nombre_usuario = _user.login_nombre,
                    fecha = DateTime.Now,
                    accion_realizada = "Se registró una nueva " + OperacionToString(_operacion)
                };
                _entities.AddTobitacora(b);

                if (_ejecutarOperacionPendiente)
                {
                    // Poner que la operacion pendiente ya no lo está.
                    var operacionPendiente =
                        (operaciones)
                        _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.operaciones", "id",
                                                               _operacionPendiente.id));
                    // Eliminar la op pendiente.
                    _entities.operaciones.DeleteObject(operacionPendiente);
                }

                _entities.SaveChanges();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Operación de impresión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Close();
        }

        private void CbClienteSelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_cliente.Text.Trim() == "Cliente casual")
            {
                finalizada.Checked = true;
                finalizada.Enabled = false;
            }
            else
            {
                finalizada.Checked = false;
                finalizada.Enabled = true;
            }

            int id = _clientesId[cb_cliente.SelectedIndex];

            var prioridad = cb_cliente.SelectedIndex == 0
                                ? 0
                                : (from prio in _entities.prioridad
                                   where prio.id_cliente == id
                                   orderby prio.fecha descending
                                   select prio.valor_ambos).FirstOrDefault();

            if (prioridad == null) return;

            prioridadLabel.Text = prioridad.Value.ToString(CultureInfo.InvariantCulture);

            var contract = _entities.contrato.FirstOrDefault(c => c.terminado == 0 && c.id_cliente == id);
            contratotextBox.Text = contract == null
                                       ? "El cliente seleccionado no posee contrato."
                                       : cb_cliente.Text + " - " + contract.descripcion;
            facturarbutton.Enabled = contract != null;
        }

        private void CantidadValueChanged(object sender, EventArgs e)
        {
            CalcularMontoTotal();
        }

        private void AddbuttonClick(object sender, EventArgs e)
        {
            var gestContrato = new GestionarContratos(_user);
            gestContrato.ShowDialog();

            if (cb_cliente.SelectedIndex < 1) return;

            int id = _clientesId[cb_cliente.SelectedIndex];

            var contract = _entities.contrato.FirstOrDefault(c => c.terminado == 0 && c.id_cliente == id);
            contratotextBox.Text = contract == null
                                       ? "El cliente seleccionado no posee contrato."
                                       : cb_cliente.Text + " - " + contract.descripcion;
            facturarbutton.Enabled = contract != null;
        }

        private void Button2Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddclientbuttonClick(object sender, EventArgs e)
        {
            var gestClient = new GestionarClientes(_user);
            gestClient.ShowDialog();

            cb_cliente.Items.Clear();
            _clientesId.Clear();
            rBCasual.Checked = true;
            foreach (var client in _entities.cliente)
            {
                _clientesId.Add(client.id);
                cb_cliente.Items.Add(client.nombre);
            }
            cb_cliente.SelectedIndex = 0;

            prioridadLabel.Text = "0";
            contratotextBox.Text = "";
        }

        private void DataGridView1CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1) return;

            if (operaciones3erosdataGridView.Columns[e.ColumnIndex].Name == "ProveedorColumn")
            {
                try
                {
                    // Mostrar el formulario del proveedor.
                    var proveedor = new ProveedorForm(_user);
                    proveedor.ShowDialog();
                    int idProv = proveedor.IdProveedor;
                    if (idProv == 0) return;
                    var prov =
                        (proveedor)
                        _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.proveedor", "id", idProv));
                    var rowIndex = e.RowIndex;
                    operaciones3erosdataGridView.Rows[rowIndex].Cells["ProveedorColumn"].Value = prov.nombre;
                    _proveedorIds[rowIndex] = idProv;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                        exception.Message +
                        (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                        @"Operación de impresión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Operaciones3ErosdataGridViewRowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            operaciones3erosdataGridView.Rows[e.RowIndex].Cells["PrecioColumn"].Value = 0m;
            operaciones3erosdataGridView.Rows[e.RowIndex].Cells["ImporteColumn"].Value = 0m;
            operaciones3erosdataGridView.Rows[e.RowIndex].Cells["CantidadColumn"].Value = 0;
            _proveedorIds.Add(0);
        }

        private void Operaciones3ErosdataGridViewRowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            _proveedorIds.RemoveAt(e.RowIndex);
            UpdateOperaciones3ErosTotal();
        }

        private void Operaciones3ErosdataGridViewCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string value = e.FormattedValue == null ? "" : e.FormattedValue.ToString().Trim();

            if (operaciones3erosdataGridView.Columns[e.ColumnIndex].Name == "ProveedorColumn")
            {
                if (_proveedorIds[e.RowIndex] == 0)
                {
                    operaciones3erosdataGridView.Rows[e.RowIndex].ErrorText =
                        "Seleccione el proveedor que realiza la operación de terceros.";
                    e.Cancel = true;
                }
            }

            if (operaciones3erosdataGridView.Columns[e.ColumnIndex].Name == "TipoOperacion3eroColumn")
            {
                if (value == "(Seleccione)")
                {
                    operaciones3erosdataGridView.Rows[e.RowIndex].Cells["PrecioColumn"].Value = 0m;
                    operaciones3erosdataGridView.Rows[e.RowIndex].ErrorText =
                        "Debe seleccionar el tipo de operación de terceros.";
                    e.Cancel = true;
                }
            }

            if (operaciones3erosdataGridView.Columns[e.ColumnIndex].Name == "CantidadColumn")
            {
                int nuevaCantidad;
                if (!int.TryParse(value, out nuevaCantidad) || nuevaCantidad < 0)
                {
                    operaciones3erosdataGridView.Rows[e.RowIndex].ErrorText =
                        "La cantidad especificada debe ser un número entero positivo.";
                    e.Cancel = true;
                }
            }

            if (operaciones3erosdataGridView.Columns[e.ColumnIndex].Name == "PrecioColumn")
            {
                decimal precio;
                if (!decimal.TryParse(value, out precio) || precio < 0)
                {
                    operaciones3erosdataGridView.Rows[e.RowIndex].ErrorText =
                        "El precio de la operación debe ser un número positivo.";
                    e.Cancel = true;
                }
            }
        }

        private void UpdateOperaciones3ErosTotal()
        {
            decimal total = 0;

            for (int index = 0; index < operaciones3erosdataGridView.RowCount - 1; index++)
                total += Convert.ToDecimal(operaciones3erosdataGridView.Rows[index].Cells["ImporteColumn"].Value);

            op3erosTotallabel.Text = @"Operaciones de terceros: " +
                                     Math.Round(total, 2).ToString(CultureInfo.InvariantCulture) + @"$";
        }

        private void Operaciones3ErosdataGridViewCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Limpiar la fila de error en caso de que el usuario presione esc.
            operaciones3erosdataGridView.Rows[e.RowIndex].ErrorText = String.Empty;

            if (operaciones3erosdataGridView.Columns[e.ColumnIndex].Name == "TipoOperacion3eroColumn")
            {
                try
                {
                    var value = operaciones3erosdataGridView.Rows[e.RowIndex].Cells["TipoOperacion3eroColumn"].Value;
                    operaciones3erosdataGridView.Rows[e.RowIndex].Cells["PrecioColumn"].Value =
                        Math.Round(
                            _entities.tipo_operacion_3ero.FirstOrDefault(op => op.valor == (string)value).
                                valor_operacion, 2);
                    operaciones3erosdataGridView.Rows[e.RowIndex].Cells["ImporteColumn"].Value =
                        Math.Round(
                            Convert.ToDecimal(operaciones3erosdataGridView.Rows[e.RowIndex].Cells["PrecioColumn"].Value) *
                            Convert.ToInt32(operaciones3erosdataGridView.Rows[e.RowIndex].Cells["CantidadColumn"].Value),
                            2);

                    UpdateOperaciones3ErosTotal();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        "No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\nExcepción: " +
                        exception.Message +
                        (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                        @"Operación de impresión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (operaciones3erosdataGridView.Columns[e.ColumnIndex].Name == "CantidadColumn")
            {
                operaciones3erosdataGridView.Rows[e.RowIndex].Cells["ImporteColumn"].Value =
                    Math.Round(
                        Convert.ToDecimal(operaciones3erosdataGridView.Rows[e.RowIndex].Cells["PrecioColumn"].Value) *
                        Convert.ToInt32(operaciones3erosdataGridView.Rows[e.RowIndex].Cells["CantidadColumn"].Value), 2);

                UpdateOperaciones3ErosTotal();
            }
        }

        private void ValorOpnumericUpDownValueChanged(object sender, EventArgs e)
        {
            CalcularMontoTotal();
        }

        private void FacturarbuttonClick(object sender, EventArgs e)
        {
            // facturar.

            //Validacion de los datos
            string validationMessage = "";

            if (cb_tipo_operacion.SelectedIndex < 1)
                validationMessage = "Seleccione el tipo de operación que se va a realizar.";

            //verifico la forma de pago
            if (cb_tipo_producto.SelectedIndex < 1)
                validationMessage += "\nDebe seleccionar el tipo de producto.";

            if (validationMessage != "")
            {
                MessageBox.Show(validationMessage, @"Operación de impresión.", MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                int idContract = _clientesId[cb_cliente.SelectedIndex];
                var contract =
                    _entities.contrato.FirstOrDefault(
                        c => c.terminado == 0 && c.id_cliente == idContract);

                // Obtener el producto por su tipo.
                int idTipProd = _tipoProductoId[cb_tipo_producto.SelectedIndex];
                producto producto =
                    _entities.producto.Where(prod => prod.id_tipo_producto == idTipProd).SingleOrDefault();

                if (producto == null)
                {
                    MessageBox.Show("No hay productos del tipo de producto especificado", @"Registrar operación",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

                var facturar = new FacturarOperacionImpresion(contract, cb_tipo_operacion.Text.Trim(),
                                                              (int)cantidad.Value, producto.unidad_medida.siglas,
                                                              valorOpnumericUpDown.Value);
                facturar.ShowDialog();
                if (facturar.FacturadaActual)
                {
                    _operacion.facturada = true;
                    _operacion.id_factura = facturar.IdFactura;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Operación de impresión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboxEstatal_CheckedChanged(object sender, EventArgs e)
        {
            cb_cliente.Items.Clear();
            _clientesId.Clear();
            rBCasual.Checked = true;
            foreach (var client in _entities.cliente)
            {
                _clientesId.Add(client.id);
                cb_cliente.Items.Add(client.nombre);
            }
            cb_cliente.SelectedIndex = 0;

            prioridadLabel.Text = "0";
            contratotextBox.Text = "";
        }

        private void rBCasual_CheckedChanged(object sender, EventArgs e)
        {
            cb_cliente.Items.Clear();
            _clientesId.Clear();
            foreach (cliente client in _entities.cliente)
            {
                _clientesId.Add(client.id);
                cb_cliente.Items.Add(client.nombre);
            }
            cb_cliente.SelectedIndex = 0;
        }

        private void rBEstatal_CheckedChanged(object sender, EventArgs e)
        {
            cb_cliente.Items.Clear();
            _clientesId.Clear();
            foreach (cliente client in _entities.cliente)
            {
                if (client.privado == 0)
                {
                    _clientesId.Add(client.id);
                    cb_cliente.Items.Add(client.nombre);
                }
            }
            cb_cliente.SelectedIndex = 0;
        }

        private void rBParticular_CheckedChanged(object sender, EventArgs e)
        {
            cb_cliente.Items.Clear();
            _clientesId.Clear();
            foreach (cliente client in _entities.cliente)
            {
                if (client.privado == 1)
                {
                    _clientesId.Add(client.id);
                    cb_cliente.Items.Add(client.nombre);
                }
            }
            cb_cliente.SelectedIndex = 0;
        }
    }
}
