using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class GestionarUnidadMedida : Form
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

        private readonly List<int> _uniMedIds;

        public GestionarUnidadMedida(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _entities = new papiro_finalEntities();
            _estado = Estado.List;
            _selectedIndex = -1;
            _uniMedIds = new List<int>();
        }

        private void ListarUm()
        {
            umlistView.Items.Clear();
            _uniMedIds.Clear();

            foreach (var unidadMedida in _entities.unidad_medida)
            {
                _uniMedIds.Add(unidadMedida.id);
                umlistView.Items.Add(new ListViewItem(new[] {unidadMedida.siglas, unidadMedida.descripcion}));
            }

            ResetEditForm(true);

            _selectedIndex = -1;
        }

        private void GestionarGastoTelefonoLoad(object sender, EventArgs e)
        {
            ListarUm();
        }

        private void ResetEditForm(bool resetText = false)
        {
            _estado = Estado.List;

            umGroupBox.Text = @"Detalles de unidad de medida";
            NuevoButton.Enabled = true;
            NuevoButton.Text = @"&Nuevo";
            modificarButton.Enabled = true;
            modificarButton.Text = @"&Modificar";
            eliminarButton.Enabled = true;
            eliminarButton.Text = @"&Eliminar";

            nombretextBox.ReadOnly = true;
            descripcionTextBox.ReadOnly = true;

            if (!resetText) return;

            nombretextBox.Text = "";
            descripcionTextBox.Text = "";
        }

        private void ModificarButtonClick(object sender, EventArgs e)
        {
            switch (_estado)
            {
                case Estado.List:
                    if (_selectedIndex == -1)
                    {
                        MessageBox.Show(@"Debe seleccionar la unidad de medida que desea modificar.",
                                        @"Actualizar unidad de medida",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    _estado = Estado.Update;
                    umGroupBox.Text = @"Edición de unidad de medida";
                    NuevoButton.Text = @"&Guardar";
                    modificarButton.Enabled = false;
                    eliminarButton.Text = @"&Cancelar";
                    PrepareForEdit();
                    nombretextBox.Focus();
                    break;

                case Estado.Insert:
                    Agregar();
                    break;
            }
        }

        private void UmlistViewItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ResetEditForm();
            
            ListViewItem selectedItem = e.Item;
            selectedItem.Checked = true;

            if (_selectedIndex != -1 && _selectedIndex != e.ItemIndex)
                umlistView.Items[_selectedIndex].Checked = false;

            _selectedIndex = e.ItemIndex;

            // Mostrar todos los elementos en el form de editar.
            nombretextBox.Text = selectedItem.SubItems[0].Text;
            descripcionTextBox.Text = selectedItem.SubItems[1].Text;
        }

        private string ValidarDatosForm(int idUm)
        {
            // Validar datos del usuario a crear.
            string validationMessage = "";

            if (nombretextBox.Text.Trim() == "")
                validationMessage = "Debe especificar el nombre de la unidad de medida.";

            if (descripcionTextBox.Text.Trim() == "")
                validationMessage += "\nDebe especificar la descripción de la unidad de medida.";

            if (_entities.unidad_medida.Any(um => um.siglas == nombretextBox.Text.Trim() && um.id != idUm))
                validationMessage += "\nYa existe una unidad de medida con el nombre especificado.";

            return validationMessage;
        }

        private void Agregar()
        {
            try
            {
                var validationMessage = ValidarDatosForm(0);

                if (validationMessage != "")
                {
                    MessageBox.Show(validationMessage, @"Agregar unidad de medida",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                
                var um = new unidad_medida
                             {
                                 siglas = nombretextBox.Text.Trim(),
                                 descripcion = descripcionTextBox.Text.Trim()
                             };

                _entities.AddTounidad_medida(um);
                
                _entities.SaveChanges();

                //se guarda en bitacora
                var bit = new bitacora
                              {
                                  id_usuario = _user.id,
                                  nombre_usuario = _user.login_nombre,
                                  fecha = DateTime.Now,
                                  accion_realizada =
                                      string.Format("Se agregó una nueva unidad de medida con nombre: {0}", um.siglas)
                              };

                _entities.AddTobitacora(bit);
                
                _entities.SaveChanges();

                _estado = Estado.List;

                ListarUm();
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                                + @"Excepción: " + exception.Message,
                                @"Agregar unidad de medida",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Actualizar(int selectedIndex)
        {
            try
            {
                int idUm = _uniMedIds[selectedIndex];
                
                var validationMessage = ValidarDatosForm(idUm);

                if (validationMessage != "")
                {
                    MessageBox.Show(validationMessage, @"Modificar unidad de medida",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                var unidadMedida =
                    (unidad_medida)
                    _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.unidad_medida", "id", idUm));
                unidadMedida.siglas = nombretextBox.Text.Trim();
                unidadMedida.descripcion = descripcionTextBox.Text.Trim();
              
                //se guarda en bitacora
                var bit = new bitacora
                              {
                                  id_usuario = _user.id,
                                  nombre_usuario = _user.login_nombre,
                                  fecha = DateTime.Now,
                                  accion_realizada =
                                      string.Format("Se actualizó la unidad de medida: {0}", unidadMedida.siglas)
                              };

                _entities.AddTobitacora(bit);

                _entities.SaveChanges();

                _estado = Estado.List;

                ListarUm();
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                                + @"Excepción: " + exception.Message,
                                @"Modificar unidad de medida",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrepareForEdit()
        {
            nombretextBox.ReadOnly = false;
            descripcionTextBox.ReadOnly = false;
        }

        private void NuevoButtonClick(object sender, EventArgs e)
        {
            switch (_estado)
            {
                case Estado.List:
                    ResetEditForm(true);
                    PrepareForEdit();
                    _estado = Estado.Insert;
                    umGroupBox.Text = @"Nueva unidad de medida";
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
            var checkedListViewItemCollection = umlistView.CheckedItems;
            var countChecked = checkedListViewItemCollection.Count;

            if (countChecked == 0)
            {
                MessageBox.Show(@"Debe seleccionar al menos una unidad de medida.", @"Eliminar unidad de medida",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (countChecked == 1)
            {
                if (MessageBox.Show(@"¿Estás seguro de querer eliminar la unidad de medida?",
                                    @"Eliminar unidad de medida", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    == DialogResult.No)
                {
                    return;
                }
            }

            if (countChecked > 1)
            {
                if (MessageBox.Show(@"¿Estás seguro de querer eliminar las unidades de medida seleccionadas?",
                                    @"Eliminar unidad de medida", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    == DialogResult.No)
                {
                    return;
                }
            }

            try
            {
                string uniNoElimino = "";
                
                // Eliminar todos los 'CheckedItems' del listview de gastos

                foreach (unidad_medida umByKey in
                                from ListViewItem checkedItem in checkedListViewItemCollection
                                select _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.unidad_medida",
                                                                "id", _uniMedIds[checkedItem.Index])))
                {
                    if (_entities.producto.Any(prod => prod.id_unidad_medida == umByKey.id))
                    {
                        uniNoElimino += ", " + umByKey.siglas;
                        continue;
                    }
                    
                    // Adicionar en bitacora.
                    var b = new bitacora
                                {
                                    id_usuario = _user.id,
                                    nombre_usuario = _user.login_nombre,
                                    fecha = DateTime.Now,
                                    accion_realizada = string.Format("Eliminada unidad de medida: {0}", umByKey.siglas)
                                };

                    _entities.AddTobitacora(b);
                    
                    _entities.DeleteObject(umByKey);
                }

                _entities.SaveChanges();

                if (uniNoElimino != "")
                {
                    uniNoElimino = uniNoElimino.Remove(0, 2);

                    if (uniNoElimino.IndexOf(',') == -1)
                        MessageBox.Show(
                            @"No se pudo eliminar la unidad de medida: '" + uniNoElimino +
                            @"', porque tiene productos asociados.", @"Eliminar unidad de medida",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show(
                            @"No se pudieron eliminar las unidades de medida: (" + uniNoElimino +
                            @"), porque tienen productos asociados.", @"Eliminar unidad de medida",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                ListarUm();
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
    }
}
