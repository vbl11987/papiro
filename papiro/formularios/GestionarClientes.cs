using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class GestionarClientes : Form
    {
        private readonly usuarios _user;

        private readonly papiro_finalEntities _entities;

        private int _selectedIndex;

        private List<telefonos> _telefonos;

        private enum Estado
        {
            Insert,
            Update,
            List
        }

        private Estado _estado;

        public GestionarClientes(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _entities = new papiro_finalEntities();
            _estado = Estado.List;
            _selectedIndex = -1;
            _telefonos = new List<telefonos>();
        }

        private void ListarClientes()
        {
            // Filtrar de acuerdo a las opciones de filtro seleccionadas.

            string queryString = @"SELECT VALUE c FROM papiro_finalEntities.cliente AS c 
                                                  WHERE c.nombre <> 'Cliente casual' ";

            string nombre = filtroNombre.Text.Trim();

            if (nombre != "")
                queryString += " and SqlServer.UPPER(c.nombre) LIKE @nombre ";

            string empNombre = empresaToolStripTextBox.Text.Trim();

            if (empNombre != "")
                queryString += " and SqlServer.UPPER(c.empresa) LIKE @empNombre ";

            ObjectQuery<cliente> objectQuery = _entities.CreateQuery<cliente>(queryString);

            if (nombre != "")
                objectQuery.Parameters.Add(new ObjectParameter("nombre", "%" + nombre.ToUpper() + "%"));

            if (empNombre != "")
                objectQuery.Parameters.Add(new ObjectParameter("empNombre", "%" + empNombre.ToUpper() + "%"));

            // Mostrarlos en el listview.

            //clientesListView.Items.Clear();
            printableLVclient.Items.Clear();

            var invariantCulture = CultureInfo.InvariantCulture;

            foreach (var client in objectQuery.ToList())
            {
                var prioridad = (from prio in client.prioridad
                                 orderby prio.fecha descending
                                 select prio.valor_ambos).FirstOrDefault();
                var item = new ListViewItem(
                    new[]
                        {
                            client.id.ToString(invariantCulture),
                            client.nombre,
                            client.direccion ?? "",
                            client.empresa ?? "",
                            prioridad == null ? "" : prioridad.Value.ToString(invariantCulture),
                            client.privado == 1 ? "Privada" : "Estatal"
                        });
                //clientesListView.Items.Add(item);
                printableLVclient.Items.Add(item);
            }

            ResetEditForm(true);

            _selectedIndex = -1;
        }

        private void ListarProductosLoad(object sender, EventArgs e)
        {
            this.printableLVclient.Title = "Clientes " + DateTime.Now.ToString("dd/MM/yyyy");
            ListarClientes();
        }

        private void ResetEditForm(bool resetText = false)
        {
            _estado = Estado.List;
            usuarioGroupBox.Text = @"Detalles del cliente";
            NuevoButton.Enabled = true;
            NuevoButton.Text = @"&Nuevo";
            modificarButton.Enabled = true;
            modificarButton.Text = @"&Modificar";
            eliminarButton.Enabled = true;
            eliminarButton.Text = @"&Eliminar";
            _telefonos = new List<telefonos>();
            telefonosListView.Items.Clear();

            nombreTextBox.ReadOnly = true;
            direccionTextBox.ReadOnly = true;
            nacDatePicker.Enabled = false;
            preferenciasTextBox.ReadOnly = true;
            prioridadNumericUpDown.ReadOnly = true;
            prioridadNumericUpDown.Increment = 0;
            prioridadFijaCheckBox.Enabled = false;
            prioridadFijaCheckBox.Checked = false;
            telefonoButton.Enabled = false;
            editarButton.Enabled = false;
            deleteTelButton.Enabled = false;
            telefonosListView.BackColor = Color.FromName(KnownColor.Control.ToString());
            empresaTextBox.ReadOnly = true;
            contactNameTextBox.ReadOnly = true;
            contactAddressTextBox.ReadOnly = true;
            rbEstatal.Enabled = false;
            rbParticular.Enabled = false;
            txtboxCargo.ReadOnly = true;

            if (!resetText) return;

            nombreTextBox.Text = "";
            direccionTextBox.Text = "";
            nacDatePicker.Value = DateTime.Now;
            preferenciasTextBox.Text = "";
            prioridadNumericUpDown.Value = 0;
            prioridadFijaCheckBox.Checked = false;
            telefonosListView.Items.Clear();
            empresaTextBox.Text = "";
            contactNameTextBox.Text = "";
            contactAddressTextBox.Text = "";
            txtboxCargo.Text = "";
            rbEstatal.Checked = true;
            rbParticular.Checked = false;
        }

        private void FiltrarClick(object sender, EventArgs e)
        {
            ResetEditForm(true);
            ListarClientes();
        }

        private void ModificarButtonClick(object sender, EventArgs e)
        {
            switch (_estado)
            {
                case Estado.List:
                    if (_selectedIndex == -1)
                    {
                        MessageBox.Show(@"Debe seleccionar el cliente que desea modificar.", @"Actualizar cliente",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    _estado = Estado.Update;
                    usuarioGroupBox.Text = @"Edición del cliente";
                    NuevoButton.Text = @"&Guardar";
                    modificarButton.Enabled = false;
                    eliminarButton.Text = @"&Cancelar";
                    PrepareForEdit();
                    nombreTextBox.Focus();
                    break;

                case Estado.Insert:
                    Agregar();
                    break;
            }
        }

        private void ClienteslistViewItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ResetEditForm();

            ListViewItem selectedItem = e.Item;
            selectedItem.Checked = true;

            if (_selectedIndex != -1 && _selectedIndex != e.ItemIndex)
                //clientesListView.Items[_selectedIndex].Checked = false;

            _selectedIndex = e.ItemIndex;

            // Mostrar todos los elementos en el form de editar.

            var cliente = (cliente)_entities.GetObjectByKey(new EntityKey("papiro_finalEntities.cliente",
                                                                           "id",
                                                                           Convert.ToInt32(
                                                                               selectedItem.SubItems[0].Text,
                                                                               CultureInfo.InvariantCulture)));
            nombreTextBox.Text = cliente.nombre;
            direccionTextBox.Text = cliente.direccion;
            if (cliente.contacto_brithday == null)
                nacDatePicker.Text = "";
            else
                nacDatePicker.Value = cliente.contacto_brithday.Value;
            var prioridad = (from prio in cliente.prioridad
                             orderby prio.fecha descending
                             select prio.valor_ambos).FirstOrDefault();
            prioridadNumericUpDown.Value = prioridad ?? 0;
            prioridadFijaCheckBox.Checked = cliente.prioridad_fija == 1;
            preferenciasTextBox.Text = cliente.preferencia_contacto;
            if (cliente.privado != 1) rbEstatal.Checked = true;
            else
                rbParticular.Checked = true;

            _telefonos.Clear();

            foreach (var userTelefCliente in cliente.user_telef_cliente)
            {
                _telefonos.Add(userTelefCliente.telefonos);

                telefonosListView.Items.Add(
                    new ListViewItem(new[]
                                         {
                                             userTelefCliente.telefonos.telefono,
                                             userTelefCliente.telefonos.tipo_telefono.valor
                                         }));
            }

            empresaTextBox.Text = cliente.empresa;
            contactNameTextBox.Text = cliente.nombre_contacto;
            contactAddressTextBox.Text = cliente.direccion_contacto;
            txtboxCargo.Text = cliente.cargo;
        }

        private string ValidarDatosForm(int idCliente)
        {
            // Validar datos del usuario a crear.
            string validationMessage = "";

            if (string.IsNullOrEmpty(nombreTextBox.Text.Trim()))
                validationMessage = "El nombre del producto no puede estar vacío.";

            if (string.IsNullOrEmpty(contactNameTextBox.Text.Trim()))
                validationMessage = "El nombre del contacto no puede estar vacío.";

            if (string.IsNullOrEmpty(direccionTextBox.Text.Trim()))
                validationMessage = "La dirección no puede estar vacía.";

            if (nacDatePicker.Value.Date >= DateTime.Now.Date)
                validationMessage += "\nLa fecha de nacimiento debe ser menor que la fecha actual.";

            if (_entities.cliente.Any(cliente => cliente.nombre == nombreTextBox.Text.Trim() && cliente.id != idCliente))
                validationMessage += "\nYa existe un cliente con el nombre especificado.";

            if (prioridadNumericUpDown.Text.Trim() == "")
                validationMessage += "\nDebe definir la prioridad para el cliente.";

            return validationMessage;
        }

        private void Agregar()
        {
            try
            {
                var validationMessage = ValidarDatosForm(0);

                if (validationMessage != "")
                {
                    MessageBox.Show(validationMessage, @"Agregar cliente",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                var prioridad = new prioridad
                {
                    valor_ambos = (int)prioridadNumericUpDown.Value,
                    valor_cant_operaciones = (int)prioridadNumericUpDown.Value,
                    valor_por_ingreso = (int)prioridadNumericUpDown.Value,
                    fecha = DateTime.Now
                };

                var cliente = new cliente
                {
                    nombre = nombreTextBox.Text,
                    direccion = direccionTextBox.Text,
                    contacto_brithday = nacDatePicker.Value,
                    preferencia_contacto = preferenciasTextBox.Text,
                    empresa = empresaTextBox.Text,
                    nombre_contacto = contactNameTextBox.Text,
                    direccion_contacto = contactAddressTextBox.Text,
                    prioridad_fija = prioridadFijaCheckBox.Checked ? 1 : 0,
                    privado = rbEstatal.Checked ? 0 : 1,
                    cargo = txtboxCargo.Text
                };

                cliente.prioridad.Add(prioridad);

                _entities.AddTocliente(cliente);

                //se guarda en bitacora
                var bit = new bitacora
                {
                    id_usuario = _user.id,
                    nombre_usuario = _user.login_nombre,
                    fecha = DateTime.Now,
                    accion_realizada = string.Format("Se agregó un nuevo cliente con Nombre: {0}", cliente.nombre)
                };

                _entities.AddTobitacora(bit);

                _entities.SaveChanges();

                // Guardando los telefonos.)
                foreach (var telefono in _telefonos)
                {
                    _entities.AddTotelefonos(telefono);

                    //se guarda en bitacora
                    var b = new bitacora
                    {
                        id_usuario = _user.id,
                        nombre_usuario = _user.login_nombre,
                        fecha = DateTime.Now,
                        accion_realizada = string.Format("Se agregó un nuevo teléfono con número: {0}", telefono.telefono)
                    };
                    _entities.AddTobitacora(b);

                    var userTelefCliente = new user_telef_cliente
                    {
                        id_cliente = cliente.id,
                        telefonos = telefono
                    };
                    _entities.AddTouser_telef_cliente(userTelefCliente);
                }

                _entities.SaveChanges();

                _estado = Estado.List;

                ListarClientes();
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                                + @"Excepción: " + exception.Message,
                                @"Agregar cliente",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Actualizar(int selectedIndex)
        {
            try
            {
                int clientId = Convert.ToInt32(printableLVclient.Items[selectedIndex].SubItems[0].Text,
                                               CultureInfo.InvariantCulture);


                string validationMessage = ValidarDatosForm(clientId);

                if (validationMessage != "")
                {
                    MessageBox.Show(validationMessage, @"Modificar cliente",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                var cliente = (cliente)_entities.GetObjectByKey(new EntityKey("papiro_finalEntities.cliente",
                                                                            "id", clientId));
                cliente.nombre = nombreTextBox.Text;
                cliente.direccion = direccionTextBox.Text;
                cliente.contacto_brithday = nacDatePicker.Value;
                cliente.preferencia_contacto = preferenciasTextBox.Text;
                var prioridad = (from prio in cliente.prioridad
                                 orderby prio.fecha descending
                                 select prio).FirstOrDefault();
                if (prioridad != null)
                {
                    if (prioridad.valor_ambos != prioridadNumericUpDown.Value)
                    {
                        cliente.prioridad.Add(new prioridad
                        {
                            valor_ambos = (int)prioridadNumericUpDown.Value,
                            valor_cant_operaciones = (int)prioridadNumericUpDown.Value,
                            valor_por_ingreso = (int)prioridadNumericUpDown.Value,
                            fecha = DateTime.Now
                        });
                    }
                }
                cliente.prioridad_fija = prioridadFijaCheckBox.Checked ? 1 : 0;
                cliente.privado = rbEstatal.Checked ? 0 : 1;
                cliente.empresa = empresaTextBox.Text;
                cliente.nombre_contacto = contactNameTextBox.Text;
                cliente.direccion_contacto = contactAddressTextBox.Text;
                cliente.cargo = txtboxCargo.Text;

                // Actualizar los teléfonos.

                var telToRemove = new List<EntityKey>();

                foreach (var usertelefono in
                    _entities.user_telef_cliente.Where(userTelefCliente => userTelefCliente.id_cliente == cliente.id))
                {
                    telefonos singleOrDefaultTel =
                        _telefonos.Where(tel => tel.id == usertelefono.telefonos.id).SingleOrDefault();

                    if (singleOrDefaultTel == default(telefonos))
                        telToRemove.Add(usertelefono.EntityKey);
                    else
                    {
                        usertelefono.telefonos.telefono = singleOrDefaultTel.telefono;
                        usertelefono.telefonos.id_tipo_telefono = singleOrDefaultTel.id_tipo_telefono;
                    }
                }

                // Eliminar los que se quitaron de la lista de telefonos.
                foreach (var id in telToRemove)
                {
                    var userTelefCliente = (user_telef_cliente)_entities.GetObjectByKey(id);
                    _entities.user_telef_cliente.DeleteObject(userTelefCliente);

                    var tel = (telefonos)_entities.GetObjectByKey(new EntityKey("papiro_finalEntities.telefonos",
                                                                                 "id",
                                                                                 userTelefCliente.id_telefono));
                    _entities.telefonos.DeleteObject(tel);

                    //se guarda en bitacora
                    var b = new bitacora
                    {
                        id_usuario = _user.id,
                        nombre_usuario = _user.login_nombre,
                        fecha = DateTime.Now,
                        accion_realizada = string.Format("Se eliminó el teléfono con número: {0}", tel.telefono)
                    };
                    _entities.AddTobitacora(b);
                }

                // Agregar los telefonos que tienen id = 0 que se deben agregar.
                foreach (var telefono in _telefonos.Where(telefono => telefono.id == 0))
                {
                    _entities.AddTotelefonos(telefono);
                    _entities.AddTouser_telef_cliente(new user_telef_cliente
                    {
                        id_cliente = cliente.id,
                        telefonos = telefono
                    });
                    //se guarda en bitacora
                    var b = new bitacora
                    {
                        id_usuario = _user.id,
                        nombre_usuario = _user.login_nombre,
                        fecha = DateTime.Now,
                        accion_realizada = string.Format("Se agregó un nuevo teléfono con número: {0}", telefono.telefono)
                    };
                    _entities.AddTobitacora(b);
                }

                //se guarda en bitacora
                var bit = new bitacora
                {
                    id_usuario = _user.id,
                    nombre_usuario = _user.login_nombre,
                    fecha = DateTime.Now,
                    accion_realizada = string.Format("Se actualizó el cliente con Nombre: {0}", cliente.nombre)
                };

                _entities.AddTobitacora(bit);

                _entities.SaveChanges();

                _estado = Estado.List;

                ListarClientes();
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                                + @"Excepción: " + exception.Message,
                                @"Actualizar cliente",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrepareForEdit()
        {
            nombreTextBox.ReadOnly = false;
            direccionTextBox.ReadOnly = false;
            nacDatePicker.Enabled = true;
            preferenciasTextBox.ReadOnly = false;
            prioridadNumericUpDown.ReadOnly = false;
            prioridadNumericUpDown.Increment = 1;
            prioridadFijaCheckBox.Checked = false;
            prioridadFijaCheckBox.Enabled = true;
            telefonoButton.Enabled = true;
            editarButton.Enabled = true;
            deleteTelButton.Enabled = true;
            telefonosListView.BackColor = Color.FromName(KnownColor.Window.ToString());
            empresaTextBox.ReadOnly = false;
            txtboxCargo.ReadOnly = false;
            contactNameTextBox.ReadOnly = false;
            contactAddressTextBox.ReadOnly = false;
            rbEstatal.Enabled = true;
            rbEstatal.Checked = true;
            rbParticular.Enabled = true;
        }

        private void NuevoButtonClick(object sender, EventArgs e)
        {
            switch (_estado)
            {
                case Estado.List:
                    ResetEditForm(true);
                    PrepareForEdit();
                    _estado = Estado.Insert;
                    usuarioGroupBox.Text = @"Nuevo cliente";
                    NuevoButton.Enabled = false;
                    modificarButton.Text = @"&Guardar";
                    eliminarButton.Text = @"&Cancelar";
                    nombreTextBox.Focus();
                    break;
                case Estado.Update:
                    // Guardar cambios en la BD
                    if (_selectedIndex != -1)
                        Actualizar(_selectedIndex);
                    break;
            }
        }

        private void Eliminar()
        {
            var checkedListViewItemCollection = printableLVclient.CheckedItems;
            var countChecked = checkedListViewItemCollection.Count;

            if (countChecked == 0)
            {
                MessageBox.Show(@"Debe seleccionar al menos un cliente.", @"Eliminar cliente",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (countChecked == 1)
            {
                if (MessageBox.Show(@"¿Estás seguro de querer eliminar el cliente seleccionado?",
                                    @"Eliminar cliente", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    == DialogResult.No)
                {
                    return;
                }
            }

            if (countChecked > 1)
            {
                if (MessageBox.Show(@"¿Estás seguro de querer eliminar los clientes seleccionados?",
                                    @"Eliminar cliente", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    == DialogResult.No)
                {
                    return;
                }
            }

            try
            {
                // Eliminar todos los 'CheckedItems' del listview de productos

                string contratoNoElimino = "";
                string cuentasCobrarNoElimino = "";
                string operacionesNoElimino = "";

                foreach (cliente clientByKey in
                                from ListViewItem checkedItem in checkedListViewItemCollection
                                select _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.cliente",
                                                                "id", Convert.ToInt32(checkedItem.SubItems[0].Text,
                                                                                    CultureInfo.InvariantCulture))))
                {
                    int idClient = clientByKey.id;

                    if (_entities.contrato.Any(c => c.id_cliente == idClient))
                    {
                        contratoNoElimino += ", " + clientByKey.nombre;
                        continue;
                    }

                    if (_entities.cuentas_por_cobrar.Any(c => c.id_cliente == idClient))
                    {
                        cuentasCobrarNoElimino += ", " + clientByKey.nombre;
                        continue;
                    }

                    if (_entities.operaciones.Any(op => op.id_cliente == idClient))
                    {
                        operacionesNoElimino += ", " + clientByKey.nombre;
                        continue;
                    }

                    // Eliminar todos los teléfonos asociados
                    while (clientByKey.user_telef_cliente.Count > 0)
                    {
                        user_telef_cliente userTelefCliente = clientByKey.user_telef_cliente.First();

                        telefonos tel = userTelefCliente.telefonos;

                        _entities.user_telef_cliente.DeleteObject(userTelefCliente);

                        _entities.telefonos.DeleteObject(tel);

                        //se guarda en bitacora
                        var bit = new bitacora
                        {
                            id_usuario = _user.id,
                            nombre_usuario = _user.login_nombre,
                            fecha = DateTime.Now,
                            accion_realizada = string.Format("Se eliminó el teléfono con número: {0}", tel.telefono)
                        };
                        _entities.AddTobitacora(bit);
                    }

                    while (clientByKey.prioridad.Count > 0)
                        _entities.prioridad.DeleteObject(clientByKey.prioridad.First());

                    _entities.cliente.DeleteObject(clientByKey);

                    // Adicionar en bitacora.
                    var b = new bitacora
                    {
                        id_usuario = _user.id,
                        nombre_usuario = _user.login_nombre,
                        fecha = DateTime.Now,
                        accion_realizada = string.Format("Eliminado cliente con Nombre: {0}", clientByKey.nombre)
                    };

                    _entities.AddTobitacora(b);
                }

                if (contratoNoElimino != "")
                {
                    contratoNoElimino = contratoNoElimino.Remove(0, 2);

                    if (contratoNoElimino.IndexOf(',') == -1)
                        MessageBox.Show(
                            @"No se pudo eliminar el cliente: '" + contratoNoElimino +
                            @"', porque tiene contratos asociados.", @"Eliminar cliente",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show(
                            @"No se pudieron eliminar los clientes: (" + contratoNoElimino +
                            @"), porque tienen contratos asociados.", @"Eliminar cliente",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (cuentasCobrarNoElimino != "")
                {
                    cuentasCobrarNoElimino = cuentasCobrarNoElimino.Remove(0, 2);

                    if (cuentasCobrarNoElimino.IndexOf(',') == -1)
                        MessageBox.Show(
                            @"No se pudo eliminar el cliente: '" + cuentasCobrarNoElimino +
                            @"', porque tiene cuentas por cobrar asociadas.", @"Eliminar cliente",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show(
                            @"No se pudieron eliminar los clientes: (" + cuentasCobrarNoElimino +
                            @"), porque tienen cuentas por cobrar asociadas.", @"Eliminar cliente",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (operacionesNoElimino != "")
                {
                    operacionesNoElimino = operacionesNoElimino.Remove(0, 2);

                    if (operacionesNoElimino.IndexOf(',') == -1)
                        MessageBox.Show(
                            @"No se pudo eliminar el cliente: '" + operacionesNoElimino +
                            @"', porque tiene operaciones asociadas.", @"Eliminar cliente",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show(
                            @"No se pudieron eliminar los clientes: (" + operacionesNoElimino +
                            @"), porque tienen operaciones asociadas.", @"Eliminar cliente",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                _entities.SaveChanges();

                ListarClientes();
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"Error en el sistema.\nExcepción: " + exception.Message, @"Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarButtonClick(object sender, EventArgs e)
        {
            switch (_estado)
            {
                case Estado.List:
                    // Eliminar
                    Eliminar();
                    ResetEditForm();
                    break;

                default:
                    _estado = Estado.List;
                    ResetEditForm();
                    break;
            }
        }

        private void TelefonoButtonClick(object sender, EventArgs e)
        {
            // Agregar al user los teléfonos

            var gestionarTelefono = new GestionarTelefono();
            gestionarTelefono.ShowDialog();
            var tel = gestionarTelefono.TelefonoSeleccionado;
            if (tel == null) return;

            _telefonos.Add(tel);
            var tipTelValue = (from tipoTelefono in _entities.tipo_telefono
                               where tipoTelefono.id == tel.id_tipo_telefono
                               select tipoTelefono.valor).First();
            var row = new[] { tel.telefono, tipTelValue };
            telefonosListView.Items.Add(new ListViewItem(row));

        }

        private void EditarButtonClick(object sender, EventArgs e)
        {
            if (telefonosListView.SelectedItems.Count == 0)
            {
                MessageBox.Show(@"Seleccione el teléfono a modificar", @"Modificar Teléfono", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            // Editar el teléfono seleccionado

            int selectedIndex = telefonosListView.SelectedIndices[0];

            var gestionarTelefono = new GestionarTelefono(true, _telefonos[selectedIndex]);
            gestionarTelefono.ShowDialog();
            var tel = gestionarTelefono.TelefonoSeleccionado;
            if (tel == null) return;

            _telefonos[selectedIndex] = tel;
            var tipTelValue = (from tipoTelefono in _entities.tipo_telefono
                               where tipoTelefono.id == tel.id_tipo_telefono
                               select tipoTelefono.valor).First();
            var row = new[] { tel.telefono, tipTelValue };
            telefonosListView.Items[selectedIndex] = new ListViewItem(row);
        }

        private void DeleteTelButtonClick(object sender, EventArgs e)
        {
            if (telefonosListView.SelectedItems.Count == 0)
            {
                MessageBox.Show(@"Seleccione el teléfono a eliminar", @"Eliminar Teléfono", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            int selectedIndex = telefonosListView.SelectedIndices[0];
            _telefonos.RemoveAt(selectedIndex);
            telefonosListView.Items.RemoveAt(selectedIndex);
        }

        private void PrioridadFijaCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            prioridadNumericUpDown.Enabled = !prioridadFijaCheckBox.Checked;
        }

        private void printableLVclient_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ResetEditForm();

            ListViewItem selectedItem = e.Item;
            selectedItem.Checked = true;

            if (_selectedIndex != -1 && _selectedIndex != e.ItemIndex)
                printableLVclient.Items[_selectedIndex].Checked = false;

            _selectedIndex = e.ItemIndex;

            // Mostrar todos los elementos en el form de editar.

            var cliente = (cliente)_entities.GetObjectByKey(new EntityKey("papiro_finalEntities.cliente",
                                                                           "id",
                                                                           Convert.ToInt32(
                                                                               selectedItem.SubItems[0].Text,
                                                                               CultureInfo.InvariantCulture)));
            nombreTextBox.Text = cliente.nombre;
            direccionTextBox.Text = cliente.direccion;
            if (cliente.contacto_brithday == null)
                nacDatePicker.Text = "";
            else
                nacDatePicker.Value = cliente.contacto_brithday.Value;
            var prioridad = (from prio in cliente.prioridad
                             orderby prio.fecha descending
                             select prio.valor_ambos).FirstOrDefault();
            prioridadNumericUpDown.Value = prioridad ?? 0;
            prioridadFijaCheckBox.Checked = cliente.prioridad_fija == 1;
            preferenciasTextBox.Text = cliente.preferencia_contacto;
            if (cliente.privado != 1) rbEstatal.Checked = true;
            else
                rbParticular.Checked = true;

            _telefonos.Clear();

            foreach (var userTelefCliente in cliente.user_telef_cliente)
            {
                _telefonos.Add(userTelefCliente.telefonos);

                telefonosListView.Items.Add(
                    new ListViewItem(new[]
                                         {
                                             userTelefCliente.telefonos.telefono,
                                             userTelefCliente.telefonos.tipo_telefono.valor
                                         }));
            }

            empresaTextBox.Text = cliente.empresa;
            txtboxCargo.Text = cliente.cargo;
            contactNameTextBox.Text = cliente.nombre_contacto;
            contactAddressTextBox.Text = cliente.direccion_contacto;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.printableLVclient.Title = "Clientes";
            this.printableLVclient.Print();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.printableLVclient.Title = "Clientes";
            this.printableLVclient.PrintPreview();
        }
    }
}
