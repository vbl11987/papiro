using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class GestionarProveedor : Form
    {
        private readonly usuarios _user;

        private readonly papiro_finalEntities _entities;

        private int _selectedIndex;

        private enum Estado
        {
            Insert,
            Update,
            List
        }
        
        private Estado _estado;

        private readonly List<int> _provIds;

        public GestionarProveedor(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _entities = new papiro_finalEntities();
            _estado = Estado.List;
            _selectedIndex = -1;
            _provIds = new List<int>();
        }

        private void ListarTipProd()
        {
            printableLV.Items.Clear();
            _provIds.Clear();

            foreach (var prov in _entities.proveedor)
            {
                _provIds.Add(prov.id);
                printableLV.Items.Add(new ListViewItem(new[] { prov.nombre }));
            }

            ResetEditForm(true);

            _selectedIndex = -1;
        }

        private void GestionarGastoTelefonoLoad(object sender, EventArgs e)
        {
            ListarTipProd();
        }

        private void ResetEditForm(bool resetText = false)
        {
            _estado = Estado.List;

            NuevoButton.Enabled = true;
            NuevoButton.Text = @"&Nuevo";
            modificarButton.Enabled = true;
            modificarButton.Text = @"&Modificar";
            eliminarButton.Enabled = true;
            eliminarButton.Text = @"&Eliminar";

            nombProvTextBox.ReadOnly = true;

            if (!resetText) return;

            nombProvTextBox.Text = "";
        }

        private void ModificarButtonClick(object sender, EventArgs e)
        {
            switch (_estado)
            {
                case Estado.List:
                    if (_selectedIndex == -1)
                    {
                        MessageBox.Show(@"Debe seleccionar el proveedor que desea modificar.",
                                        @"Actualizar proveedor",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    _estado = Estado.Update;
                    NuevoButton.Text = @"&Guardar";
                    modificarButton.Enabled = false;
                    eliminarButton.Text = @"&Cancelar";
                    PrepareForEdit();
                    break;

                case Estado.Insert:
                    Agregar();
                    break;
            }
        }

        //private void ProvlistViewItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        //{
        //    ResetEditForm();
            
        //    ListViewItem selectedItem = e.Item;
        //    selectedItem.Checked = true;

        //    if (_selectedIndex != -1 && _selectedIndex != e.ItemIndex)
        //        provlistView.Items[_selectedIndex].Checked = false;

        //    _selectedIndex = e.ItemIndex;

        //    // Mostrar todos los elementos en el form de editar.
        //    nombProvTextBox.Text = selectedItem.SubItems[0].Text;
        //}

        private string ValidarDatosForm(int idprov)
        {
            string validationMessage = "";

            if (nombProvTextBox.Text.Trim() == "")
                validationMessage += "\nDebe especificar el nombre del proveedor.";

            if (_entities.proveedor.Any(prov => prov.nombre == nombProvTextBox.Text.Trim() && prov.id != idprov))
                validationMessage += "\nYa existe un proveedor con el nombre especificado.";

            return validationMessage;
        }

        private void Agregar()
        {
            try
            {
                var validationMessage = ValidarDatosForm(0);

                if (validationMessage != "")
                {
                    MessageBox.Show(validationMessage, @"Agregar nuevo proveedor",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                // Crear el proveedor.
                var proveedor = new proveedor
                                    {
                                        nombre = nombProvTextBox.Text.Trim(),
                                    };
                _entities.AddToproveedor(proveedor);
                _entities.SaveChanges();

                // Guardar en bitácora.
                _entities.AddTobitacora(new bitacora
                                            {
                                                id_usuario = _user.id,
                                                nombre_usuario = _user.login_nombre,
                                                fecha = DateTime.Now,
                                                accion_realizada =
                                                    "Se ha registrado un nuevo Proveedor con Id: " + proveedor.id +
                                                    ", Nombre: " + proveedor.nombre
                                            });
                _entities.SaveChanges();

                _estado = Estado.List;

                ListarTipProd();
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                                + @"Excepción: " + exception.Message,
                                @"Agregar proveedor",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Actualizar(int selectedIndex)
        {
            try
            {
                int idProv = _provIds[selectedIndex];
                
                var validationMessage = ValidarDatosForm(idProv);

                if (validationMessage != "")
                {
                    MessageBox.Show(validationMessage, @"Modificar proveedor",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                var prov =
                    (proveedor)
                    _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.proveedor", "id", idProv));
                prov.nombre = nombProvTextBox.Text.Trim();

                // Guardar en bitácora.
                _entities.AddTobitacora(new bitacora
                                            {
                                                id_usuario = _user.id,
                                                nombre_usuario = _user.login_nombre,
                                                fecha = DateTime.Now,
                                                accion_realizada =
                                                    "Se ha actualizado el proveedor con Id: " + prov.id +
                                                    ", Nombre: " + prov.nombre
                                            });
                _entities.SaveChanges();

                _estado = Estado.List;

                ListarTipProd();
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                                + @"Excepción: " + exception.Message,
                                @"Modificar proveedor",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrepareForEdit()
        {
            nombProvTextBox.ReadOnly = false;
        }

        private void NuevoButtonClick(object sender, EventArgs e)
        {
            switch (_estado)
            {
                case Estado.List:
                    ResetEditForm(true);
                    PrepareForEdit();
                    _estado = Estado.Insert;
                    NuevoButton.Enabled = false;
                    modificarButton.Text = @"&Guardar";
                    eliminarButton.Text = @"&Cancelar";
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
                MessageBox.Show(@"Debe seleccionar al menos un proveedor.", @"Eliminar proveedor",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (countChecked == 1)
            {
                if (MessageBox.Show(@"¿Estás seguro de querer eliminar el proveedor?",
                                    @"Eliminar proveedor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    == DialogResult.No)
                {
                    return;
                }
            }

            if (countChecked > 1)
            {
                if (MessageBox.Show(@"¿Estás seguro de querer eliminar los proveedores seleccionados?",
                                    @"Eliminar proveedor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    == DialogResult.No)
                {
                    return;
                }
            }

            try
            {
                string entradaNoElimino = "";

                string op3EroNoElimino = "";
                
                foreach (proveedor prov in
                                from ListViewItem checkedItem in checkedListViewItemCollection
                                select _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.proveedor",
                                                                "id", _provIds[checkedItem.Index])))
                {
                    int provId = prov.id;
                    
                    if (_entities.entrada.Any(ent => ent.id_proveedor == provId))
                    {
                        entradaNoElimino += ", " + prov.nombre;
                        continue;
                    }

                    if (_entities.operacion_3ero.Any(op => op.id_proveedor == provId))
                    {
                        op3EroNoElimino += ", " + prov.nombre;
                        continue;
                    }
                    
                    // Adicionar en bitacora.
                    var b = new bitacora
                                {
                                    id_usuario = _user.id,
                                    nombre_usuario = _user.login_nombre,
                                    fecha = DateTime.Now,
                                    accion_realizada =
                                        string.Format("Eliminado el proveedor con Id: {0}, Nombre: {1}", prov.id,
                                                      prov.nombre)
                                };
                    _entities.AddTobitacora(b);
                    
                    _entities.DeleteObject(prov);
                }

                _entities.SaveChanges();

                if (entradaNoElimino != "")
                {
                    entradaNoElimino = entradaNoElimino.Remove(0, 2);

                    if (entradaNoElimino.IndexOf(',') == -1)
                        MessageBox.Show(
                            @"No se pudo eliminar el proveedor: '" + entradaNoElimino +
                            @"', porque tiene asociado entrada de productos.", @"Eliminar proveedor",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show(
                            @"No se pudieron eliminar los proveedores: (" + entradaNoElimino +
                            @"), porque tienen asociados entrada de productos.", @"Eliminar proveedor",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (op3EroNoElimino != "")
                {
                    op3EroNoElimino = op3EroNoElimino.Remove(0, 2);

                    if (op3EroNoElimino.IndexOf(',') == -1)
                        MessageBox.Show(
                            @"No se pudo eliminar el proveedor: '" + entradaNoElimino +
                            @"', porque tiene asociado operaciones de terceros.", @"Eliminar proveedor",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show(
                            @"No se pudieron eliminar los proveedores: (" + entradaNoElimino +
                            @"), porque tienen asociados operaciones de terceros.", @"Eliminar proveedor",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                ListarTipProd();
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

        private void printableLV_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ResetEditForm();

            ListViewItem selectedItem = e.Item;
            selectedItem.Checked = true;

            if (_selectedIndex != -1 && _selectedIndex != e.ItemIndex)
                printableLV.Items[_selectedIndex].Checked = false;

            _selectedIndex = e.ItemIndex;

            // Mostrar todos los elementos en el form de editar.
            nombProvTextBox.Text = selectedItem.SubItems[0].Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printableLV.Title = "Proveedores";
            printableLV.PrintPreview();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            printableLV.Title = "Proveedores";
            printableLV.Print();
        }
    }
}
