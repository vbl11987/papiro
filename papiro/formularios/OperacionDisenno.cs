using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class OperacionDisenno : Form
    {
        private readonly usuarios _user;

        private readonly papiro_finalEntities _entities;

        private readonly List<int> _clientesId;
        
        private readonly List<int> _tipoOperacionId;

        private readonly List<int> _tipoProductoId;

        public OperacionDisenno(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _entities  = new papiro_finalEntities();
            _clientesId = new List<int> {-1};
            _tipoOperacionId = new List<int> {-1};
            _tipoProductoId = new List<int> { -1 };
        }

        private void OperacionLoad(object sender, EventArgs e)
        {
            foreach (cliente client in _entities.cliente)
            {
                _clientesId.Add(client.id);
                cb_cliente.Items.Add(client.nombre);
            }

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

        private string OperacionToString(operaciones operacion)
        {
            string str = "Operación de diseño con Id: " + operacion.id + ", Cliente: " + operacion.cliente.nombre;
            if (propioRadioButton.Checked)
            {
                if (operacion.contrato != null)
                    str += ", Contrato: " + operacion.contrato.descripcion;
                str += ", Tipo de operación: " + operacion.tipo_operacion.valor + ", Tipo de producto: " +
                       operacion.tipo_producto.valor;
            }
            else if (terceroRadioButton2.Checked)
            {
                str += ", Precio de operación: " + precionumericUpDown.Value;
            }
            str += ", Pendiente a impresión: " + (operacion.pendiente_imprimir == 1 ? "Si" : "No");
            return str;
        }

        private void AceptarClick(object sender, EventArgs e)
        {
            try
            {
                //Validacion de los datos
                string validationMessage = "";

                // Verificar el cliente.
                if (cb_cliente.SelectedIndex < 1)
                    validationMessage = "Seleccione el cliente al que se le ofrece el servicio.";

                if (propioRadioButton.Checked)
                {
                    if (cb_tipo_operacion.SelectedIndex < 1)
                        validationMessage += "\nSeleccione el tipo de operación que se va a realizar.";

                    //verifico la forma de pago
                    if (cb_tipo_producto.SelectedIndex < 1)
                        validationMessage += "\nDebe seleccionar el tipo de producto.";    
                }

                if(validationMessage != "")
                {
                    MessageBox.Show(validationMessage, @"Registrar operación", MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);
                    return;
                }
                //----------------------------------se termina las validaciones---------------------

                //-------------Chequeo que el user que esta realizando la operacion es disennador

                //--------------No valido que el usuario sea disennador porque la operacion solo se puede realizar
                //si el usuario es disennador o administrador de sistema

                //var id_user_disennador = 0;
                //foreach (var rol in _user.rol)
                //{
                //    if (rol.id == 4)
                //        id_user_disennador = rol.id;
                //}

                //-----------------registro la 
                var op = new operaciones
                             {
                                 id_cliente = _clientesId[cb_cliente.SelectedIndex],
                                 fecha = DateTime.Now,
                                 pendiente = 0,
                                 descripcion = descripciontextBox.Text,
                                 id_usuario = _user.id,
                                 gasto = 0,
                                 costo = 0,
                                 cantidad = 0,
                                 facturada = false,
                                 valor_tipo_operacion = 0,
                                 id_disenador = _user.id
                             };

                if (propioRadioButton.Checked)
                {
                    op.id_tipo_operacion = _tipoOperacionId[cb_tipo_operacion.SelectedIndex];

                    op.id_tipo_producto = _tipoProductoId[cb_tipo_producto.SelectedIndex];

                    var contract =
                        _entities.contrato.FirstOrDefault(c => c.terminado == 0 && c.id_cliente == op.id_cliente);
                    if (contract != null)
                        op.id_contrato = contract.id;

                    op.pendiente_imprimir = 1;
                }
                else
                {
                    op.monto = precionumericUpDown.Value;

                    op.pendiente_imprimir = 0;

                    // Obtener último balance para actualizar.
                    balance balance = _entities.balance.ToList().Last();

                    // Registrar el ingreso.
                    _entities.AddTosubmayor_ingreso(new submayor_ingreso
                                                        {
                                                            fecha = DateTime.Now,
                                                            id_usuario = _user.id,
                                                            descripcion = "Se realiza operación de diseño.",
                                                            saldo = balance.ingreso + op.monto,
                                                            credito = op.monto
                                                        });
                    balance.ingreso += op.monto;

                    // Pongo el ingreso en la caja
                    _entities.AddTosubmayor_efectivo_caja(new submayor_efectivo_caja
                                                        {
                                                            fecha = DateTime.Now,
                                                            id_usuario = _user.id,
                                                            descripcion = "Se realiza operación de impresión.",
                                                            saldo = balance.efectivo_caja + op.monto,
                                                            debito = op.monto
                                                        });
                    balance.efectivo_caja += op.monto;
                }

                // Poner en el salario extra del user logueado el % por operación del monto.
                //var user =
                //    (usuarios)
                //    _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.usuarios", "id", _user.id));
                //user.salario_extra_operaciones += op.monto * (decimal)user.por_ciento_operaciones / 100;
                //op.costo += user.salario_extra_operaciones.Value;

                _entities.AddTooperaciones(op);

                _entities.SaveChanges();
                
                //se guarda en bitacora
                _entities.AddTobitacora(new bitacora
                                            {
                                                id_usuario = _user.id,
                                                nombre_usuario = _user.login_nombre,
                                                fecha = DateTime.Now,
                                                accion_realizada = "Se registró una nueva " + OperacionToString(op)
                                            });
                _entities.SaveChanges();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Operación de diseño", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Close();
        }

        private void CbClienteSelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_cliente.SelectedIndex < 1)
            {
                prioridadLabel.Text = "0";
                contratotextBox.Text = "";
                return;
            }

            int id = _clientesId[cb_cliente.SelectedIndex];

            var prioridad = cb_cliente.SelectedIndex == 1
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
            cb_cliente.Items.Add("(Seleccione)");
            _clientesId.Clear();
            _clientesId.Add(-1);
            foreach(var client in _entities.cliente)
            {
                _clientesId.Add(client.id);
                cb_cliente.Items.Add(client.nombre);
            }
            cb_cliente.SelectedIndex = 0;

            prioridadLabel.Text = "0";
            contratotextBox.Text = "";
        }

        private void PropioRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            if (!propioRadioButton.Checked) return;

            precionumericUpDown.Enabled = false;
            addbutton.Enabled = true;
            cb_tipo_operacion.Enabled = true;
            cb_tipo_producto.Enabled = true;

            if (cb_cliente.SelectedIndex > 0)
            {
                int id = _clientesId[cb_cliente.SelectedIndex];
                var contract = _entities.contrato.FirstOrDefault(c => c.terminado == 0 && c.id_cliente == id);
                contratotextBox.Text = contract == null
                                           ? "El cliente seleccionado no posee contratos."
                                           : cb_cliente.Text + " - " + contract.descripcion;
            }
        }

        private void TerceroRadioButton2CheckedChanged(object sender, EventArgs e)
        {
            if (!terceroRadioButton2.Checked) return;

            precionumericUpDown.Enabled = true;
            addbutton.Enabled = false;
            cb_tipo_operacion.Enabled = false;
            cb_tipo_producto.Enabled = false;
        }
    }
}
