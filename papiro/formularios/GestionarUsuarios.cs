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
    public partial class GestionarUsuarios : Form
    {
        private readonly usuarios _user;

        private readonly papiro_finalEntities _entities;

        private int _selectedIndex;

        private readonly List<int> _rolesId;

        private List<telefonos> _telefonos;

        private readonly List<int> _usersId;

        private enum Estado
        {
            Insert,
            Update,
            List
        }

        private Estado _estado;

        public GestionarUsuarios(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _entities = new papiro_finalEntities();
            _estado = Estado.List;
            _selectedIndex = -1;
            _rolesId = new List<int>();
            _telefonos = new List<telefonos>();
            _usersId = new List<int>();
        }

        private void ListarUsuarios()
        {
            // Filtrar de acuerdo a las opciones de filtro seleccionadas.

            string queryString = @"SELECT VALUE u FROM papiro_finalEntities.usuarios AS u
                                                  WHERE u.login_nombre <> 'Sistema'";
            
            string nombre = filtroNombre.Text.Trim();

            if (nombre != "")
                queryString += " and SqlServer.UPPER(u.login_nombre) LIKE @nombre ";

            ObjectQuery<usuarios> objectQuery = _entities.CreateQuery<usuarios>(queryString);

            if (nombre != "")
                objectQuery.Parameters.Add(new ObjectParameter("nombre", "%" + nombre.ToUpper() + "%"));

            // Mostrarlos en el listview.

            //usuariosListView.Items.Clear();
            printableLV.Items.Clear();
            _usersId.Clear();

            var invariantCulture = CultureInfo.InvariantCulture;

            foreach (var usuario in objectQuery)
            {
                _usersId.Add(usuario.id);
                printableLV.Items.Add(new ListViewItem(
                                               new[]
                                                   {
                                                       usuario.login_nombre,
                                                       usuario.nombre,
                                                       usuario.direccion ?? "",
                                                       usuario.user_telef_cliente.Aggregate("",
                                                                                            (current, userTelefCliente)
                                                                                            =>
                                                                                            current +
                                                                                            userTelefCliente.telefonos.
                                                                                                telefono + ",")
                                                           .TrimEnd(new[] {','}),
                                                       Math.Round(usuario.salario_fijo, 2).ToString(invariantCulture),
                                                       Math.Round(usuario.por_ciento_operaciones, 2).ToString(
                                                           invariantCulture),
                                                       Math.Round(usuario.salario_extra_operaciones.Value, 2).ToString(
                                                           invariantCulture),
                                                       usuario.activado ? "Si" : "No"
                                                   }));
            }

            ResetEditForm(true);

            _selectedIndex = -1;
        }

        private void ListarProductosLoad(object sender, EventArgs e)
        {
            ListarUsuarios();

            foreach (rol rol in _entities.rol)
            {
                _rolesId.Add(rol.id);
                rolesListView.Items.Add(rol.nombre);
            }
        }

        private void ResetEditForm(bool resetText = false)
        {
            _estado = Estado.List;
            usuarioGroupBox.Text = @"Detalles de usuario";
            NuevoButton.Enabled = true;
            NuevoButton.Text = @"&Nuevo";
            modificarButton.Enabled = true;
            modificarButton.Text = @"&Modificar";
            eliminarButton.Enabled = true;
            eliminarButton.Text = @"&Eliminar";
            _telefonos = new List<telefonos>();
            telefonosListView.Items.Clear();

            nombreTextBox.ReadOnly = true;
            logintextBox.ReadOnly = true;
            direccionTextBox.ReadOnly = true;
            nacDatePicker.Enabled = false;
            preferenciasTextBox.ReadOnly = true;
            salarioNumericUpDown.ReadOnly = true;
            salarioNumericUpDown.Increment = 0;
            porcientoNumericUpDown.ReadOnly = true;
            porcientoNumericUpDown.Increment = 0;
            pass1.ReadOnly = true;
            pass2.ReadOnly = true;
            rolesListView.Enabled = false;
            telefonoButton.Enabled = false;
            editarButton.Enabled = false;
            deleteTelButton.Enabled = false;
            telefonosListView.BackColor = Color.FromName(KnownColor.Control.ToString());
            salarioExtraLabel.Text = @"0";
            activocheckBox.Enabled = false;
            
            if (!resetText) return;

            nombreTextBox.Text = "";
            logintextBox.Text = "";
            direccionTextBox.Text = "";
            nacDatePicker.Value = DateTime.Now;
            preferenciasTextBox.Text = "";
            salarioNumericUpDown.Value = 0;
            porcientoNumericUpDown.Value = 0;
            pass1.Text = "";
            pass2.Text = "";
            foreach (ListViewItem item in rolesListView.Items)
                item.Checked = false;
            telefonosListView.Items.Clear();
            activocheckBox.Checked = true;
        }

        private void FiltrarClick(object sender, EventArgs e)
        {
            ResetEditForm(true);
            ListarUsuarios();
        }

        private void ModificarButtonClick(object sender, EventArgs e)
        {
            switch (_estado)
            {
                case Estado.List:
                    if (_selectedIndex == -1)
                    {
                        MessageBox.Show(@"Debe seleccionar el usuario que desea modificar.", @"Actualizar usuario",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    _estado = Estado.Update;
                    usuarioGroupBox.Text = @"Edición de usuario";
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

        private void ProductoslistViewItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ResetEditForm();
            
            ListViewItem selectedItem = e.Item;
            selectedItem.Checked = true;

            if (_selectedIndex != -1 && _selectedIndex != e.ItemIndex)
                printableLV.Items[_selectedIndex].Checked = false;

            _selectedIndex = e.ItemIndex;

            // Mostrar todos los elementos en el form de editar.

            var user = (usuarios)_entities.GetObjectByKey(new EntityKey("papiro_finalEntities.usuarios",
                                                                         "id",
                                                                         _usersId[selectedItem.Index]));
            nombreTextBox.Text = user.nombre;
            logintextBox.Text = user.login_nombre;
            direccionTextBox.Text = user.direccion;
            if (user.fecha_nacimiento == null)
                nacDatePicker.Text = "";
            else
                nacDatePicker.Value = user.fecha_nacimiento.Value;
            salarioNumericUpDown.Value = user.salario_fijo;
            porcientoNumericUpDown.Value = (decimal) user.por_ciento_operaciones;
            preferenciasTextBox.Text = user.preferencias;
            pass1.Text = user.pass;
            pass2.Text = user.pass;

            for (int index = 0; index < _rolesId.Count; index++)
            {
                bool rolExist = user.rol.Any(rol => _rolesId[index] == rol.id);
                rolesListView.Items[index].Checked = rolExist;
            }
            
            _telefonos.Clear();

            foreach (var userTelefCliente in user.user_telef_cliente)
            {
                _telefonos.Add(userTelefCliente.telefonos);

                telefonosListView.Items.Add(
                    new ListViewItem(new[]
                                         {
                                             userTelefCliente.telefonos.telefono,
                                             userTelefCliente.telefonos.tipo_telefono.valor
                                         }));
            }

            salarioExtraLabel.Text = user.salario_extra_operaciones == null
                                         ? "0"
                                         : user.salario_extra_operaciones.ToString();

            activocheckBox.Checked = user.activado;
        }

        private string ValidarDatosForm(int idUser)
        {
            // Validar datos del usuario a crear.
            string validationMessage = "";

            if (string.IsNullOrEmpty(logintextBox.Text))
                validationMessage = "El nombre de usuario no puede estar vacío.";

            if (string.IsNullOrEmpty(nombreTextBox.Text))
                validationMessage = "El nombre del producto no puede estar vacío.";

            if (pass1.Text.Trim() != pass2.Text.Trim())
                validationMessage += "\nAmbas contraseñas deben coincidir.";

            if (nacDatePicker.Value.Date >= DateTime.Now.Date)
                validationMessage += "\nLa fecha de nacimiento debe ser menor que la fecha actual.";

            if (!rolesListView.Items.Cast<ListViewItem>().Any(item => item.Checked))
                validationMessage += "\nDebe especificar al menos un rol.";

            if (_entities.usuarios.Any(usuario => usuario.login_nombre == logintextBox.Text.Trim() && usuario.id != idUser))
                validationMessage += "\nYa existe un usuario con el nombre de usuario especificado.";
        
            return validationMessage;
        }

        private void Agregar()
        {
            try
            {
                string validationMessage = ValidarDatosForm(0);

                if (validationMessage != "")
                {
                    MessageBox.Show(validationMessage, @"Agregar usuario",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                var user = new usuarios
                               {
                                   login_nombre = logintextBox.Text.Trim(),
                                   nombre = nombreTextBox.Text,
                                   direccion = direccionTextBox.Text,
                                   fecha_nacimiento = nacDatePicker.Value,
                                   preferencias = preferenciasTextBox.Text,
                                   salario_fijo = salarioNumericUpDown.Value,
                                   salario_extra_operaciones = 0,
                                   por_ciento_operaciones = (float) porcientoNumericUpDown.Value,
                                   activado = activocheckBox.Checked,
                                   a_pagar =  0
                               };

                if (pass1.Text != "")
                    user.pass = pass1.Text;

                int count = rolesListView.Items.Count;
                for (int index = 0; index < count; index++)
                {
                    if (!rolesListView.Items[index].Checked) continue;
                    var rol = (rol)_entities.GetObjectByKey(new EntityKey("papiro_finalEntities.rol",
                                                                          "id",
                                                                          _rolesId[index]));
                    user.rol.Add(rol);
                }

                // Guardando los telefonos.
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
                                                   usuarios = user,
                                                   telefonos = telefono
                                               };
                    _entities.AddTouser_telef_cliente(userTelefCliente);
                }

                _entities.AddTousuarios(user);

                //se guarda en bitacora
                var bit = new bitacora
                {
                    id_usuario = _user.id,
                    nombre_usuario = _user.login_nombre,
                    fecha = DateTime.Now,
                    accion_realizada = string.Format("Se agregó un nuevo usuario con Nombre: {0}", user.login_nombre)
                };

                _entities.AddTobitacora(bit);

                _entities.SaveChanges();

                _estado = Estado.List;

                ListarUsuarios();
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                                + @"Excepción: " + exception.Message,
                                @"Agregar usuario",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Actualizar(int selectedIndex)
        {
            try
            {
                int userId = _usersId[selectedIndex];
                
                string validationMessage = ValidarDatosForm(userId);

                if (validationMessage != "")
                {
                    MessageBox.Show(validationMessage, @"Modificar usuario",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                var user = (usuarios) _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.usuarios",
                                                                             "id", userId));
                user.login_nombre = logintextBox.Text;
                user.nombre = nombreTextBox.Text;
                user.direccion = direccionTextBox.Text;
                user.fecha_nacimiento = nacDatePicker.Value;
                user.preferencias = preferenciasTextBox.Text;
                user.salario_fijo = salarioNumericUpDown.Value;
                user.por_ciento_operaciones = (float) porcientoNumericUpDown.Value;
                user.activado = activocheckBox.Checked;

                if (pass1.Text != "")
                    user.pass = pass1.Text;

                var listToAdd = new List<int>();
                var listToRemove = new List<int>();

                int count = _rolesId.Count;
                for (int index = 0; index < count; index++)
                {
                    bool rolExist = user.rol.Any(rol => rol.id == _rolesId[index]);

                    if (!rolesListView.Items[index].Checked && rolExist)
                        listToRemove.Add(index);
                    else if (rolesListView.Items[index].Checked && !rolExist)
                        listToAdd.Add(index);
                }

                foreach (var id in listToRemove)
                {
                    user.rol.Remove((rol)_entities.GetObjectByKey(new EntityKey("papiro_finalEntities.rol",
                                                                                 "id",
                                                                                 _rolesId[id])));
                }

                foreach (var id in listToAdd)
                {
                    user.rol.Add((rol)_entities.GetObjectByKey(new EntityKey("papiro_finalEntities.rol",
                                                                              "id",
                                                                              _rolesId[id])));
                }

                // Actualizar los teléfonos.

                var telToRemove = new List<EntityKey>();

                foreach (var usertelefono in user.user_telef_cliente)
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
                    var userTelefCliente = (user_telef_cliente) _entities.GetObjectByKey(id);
                    _entities.user_telef_cliente.DeleteObject(userTelefCliente);

                    var tel = (telefonos) _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.telefonos",
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
                                                              id_usuario = user.id,
                                                              id_telefono = telefono.id
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
                    accion_realizada = string.Format("Se actualizó el usuario con Nombre: {0}", user.login_nombre)
                };

                _entities.AddTobitacora(bit);
                
                _entities.SaveChanges();

                _estado = Estado.List;

                ListarUsuarios();
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                                + @"Excepción: " + exception.Message,
                                @"Actualizar usuario",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrepareForEdit()
        {
            nombreTextBox.ReadOnly = false;
            logintextBox.ReadOnly = false;
            direccionTextBox.ReadOnly = false;
            nacDatePicker.Enabled = true;
            preferenciasTextBox.ReadOnly = false;
            salarioNumericUpDown.ReadOnly = false;
            salarioNumericUpDown.Increment = 1;
            porcientoNumericUpDown.ReadOnly = false;
            porcientoNumericUpDown.Increment = 1;
            pass1.ReadOnly = false;
            pass2.ReadOnly = false;
            rolesListView.Enabled = true;
            telefonoButton.Enabled = true;
            editarButton.Enabled = true;
            deleteTelButton.Enabled = true;
            telefonosListView.BackColor = Color.FromName(KnownColor.Window.ToString());
            activocheckBox.Enabled = true;
        }

        private void NuevoButtonClick(object sender, EventArgs e)
        {
            switch (_estado)
            {
                case Estado.List:
                    ResetEditForm(true);
                    PrepareForEdit();
                    _estado = Estado.Insert;
                    usuarioGroupBox.Text = @"Nuevo usuario";
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
            var checkedListViewItemCollection = printableLV.CheckedItems;
            var countChecked = checkedListViewItemCollection.Count;

            if (countChecked == 0)
            {
                MessageBox.Show(@"Debe seleccionar al menos un usuario.", @"Eliminar usuario",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (countChecked == 1)
            {
                if (MessageBox.Show(@"¿Estás seguro de querer eliminar el usuario seleccionado?",
                                    @"Eliminar usuario", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    == DialogResult.No)
                {
                    return;
                }
            }

            if (countChecked > 1)
            {
                if (MessageBox.Show(@"¿Estás seguro de querer eliminar los usuarios seleccionados?",
                                    @"Eliminar usuario", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    == DialogResult.No)
                {
                    return;
                }
            }

            try
            {
                // Eliminar todos los 'CheckedItems' del listview de productos

                string userNoElimino = "";

                foreach (usuarios userByKey in
                                from ListViewItem checkedItem in checkedListViewItemCollection
                                select _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.usuarios",
                                                                "id", _usersId[checkedItem.Index])))
                {
                    int idUser = userByKey.id;
                    
                    if (idUser == _user.id)
                    {
                        userNoElimino = userByKey.login_nombre;
                        continue;
                    }

                    if (_entities.bitacora.Any(bitacora => bitacora.id_usuario == idUser) || 
                        _entities.operaciones.Any(op => op.id_comercial == idUser || 
                                                        op.id_disenador == idUser || op.id_operador == idUser))
                    {
                        userByKey.activado = false;
                        //se guarda en bitacora
                        var bit = new bitacora
                                      {
                                          id_usuario = _user.id,
                                          nombre_usuario = _user.login_nombre,
                                          fecha = DateTime.Now,
                                          accion_realizada =
                                              string.Format("Desactivado el usuario: {0}", userByKey.login_nombre)
                                      };
                        _entities.AddTobitacora(bit);
                        continue;
                    }
                    
                    // Eliminar todos los teléfonos asociados
                    while (userByKey.user_telef_cliente.Count > 0)
                    {
                        var userTelefCliente =
                            (user_telef_cliente)
                            _entities.GetObjectByKey(userByKey.user_telef_cliente.First().EntityKey);
                        _entities.user_telef_cliente.DeleteObject(userTelefCliente);

                        var tel = (telefonos)_entities.GetObjectByKey(new EntityKey("papiro_finalEntities.telefonos",
                                                                                     "id",
                                                                                     userTelefCliente.id_telefono));
                        _entities.telefonos.DeleteObject(tel);

                        //se guarda en bitacora
                        var bit = new bitacora
                                      {
                                          id_usuario = _user.id,
                                          nombre_usuario = _user.login_nombre,
                                          fecha = DateTime.Now,
                                          accion_realizada =
                                              string.Format("Se eliminó el teléfono con número: {0}", tel.telefono)
                                      };
                        _entities.AddTobitacora(bit);
                    }

                    while (userByKey.avisos.Count > 0)
                        userByKey.avisos.Remove(userByKey.avisos.ElementAt(0));
                    
                    _entities.usuarios.DeleteObject(userByKey);
                    
                    // Adicionar en bitacora.
                    var b = new bitacora
                                {
                                    id_usuario = _user.id,
                                    nombre_usuario = _user.login_nombre,
                                    fecha = DateTime.Now,
                                    accion_realizada =
                                        string.Format("Eliminado usuario con Nombre: {0}", userByKey.login_nombre)
                                };

                    _entities.AddTobitacora(b);
                }

                _entities.SaveChanges();

                if (userNoElimino != "")
                    MessageBox.Show(
                            @"No se puede eliminar el usuario: '" + userNoElimino +
                            @"', porque tiene su sesión activa en estos momentos.", @"Eliminar usuario",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                ListarUsuarios();
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
            var row = new[] {tel.telefono, tipTelValue };
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

        private void button1_Click(object sender, EventArgs e)
        {
            printableLV.Title = "Usuarios";
            printableLV.PrintPreview();
        }

        private void printableLV_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ResetEditForm();

            ListViewItem selectedItem = e.Item;
            selectedItem.Checked = true;

            if (_selectedIndex != -1 && _selectedIndex != e.ItemIndex)
                printableLV.Items[_selectedIndex].Checked = false;

            _selectedIndex = e.ItemIndex;

            // Mostrar todos los elementos en el form de editar.

            var user = (usuarios)_entities.GetObjectByKey(new EntityKey("papiro_finalEntities.usuarios",
                                                                         "id",
                                                                         _usersId[selectedItem.Index]));
            nombreTextBox.Text = user.nombre;
            logintextBox.Text = user.login_nombre;
            direccionTextBox.Text = user.direccion;
            if (user.fecha_nacimiento == null)
                nacDatePicker.Text = "";
            else
                nacDatePicker.Value = user.fecha_nacimiento.Value;
            salarioNumericUpDown.Value = user.salario_fijo;
            porcientoNumericUpDown.Value = (decimal)user.por_ciento_operaciones;
            preferenciasTextBox.Text = user.preferencias;
            pass1.Text = user.pass;
            pass2.Text = user.pass;

            for (int index = 0; index < _rolesId.Count; index++)
            {
                bool rolExist = user.rol.Any(rol => _rolesId[index] == rol.id);
                rolesListView.Items[index].Checked = rolExist;
            }

            _telefonos.Clear();

            foreach (var userTelefCliente in user.user_telef_cliente)
            {
                _telefonos.Add(userTelefCliente.telefonos);

                telefonosListView.Items.Add(
                    new ListViewItem(new[]
                                         {
                                             userTelefCliente.telefonos.telefono,
                                             userTelefCliente.telefonos.tipo_telefono.valor
                                         }));
            }

            salarioExtraLabel.Text = user.salario_extra_operaciones == null
                                         ? "0"
                                         : user.salario_extra_operaciones.ToString();

            activocheckBox.Checked = user.activado;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            printableLV.Title = "Usuarios";
            printableLV.Print();
        }
    }
}
