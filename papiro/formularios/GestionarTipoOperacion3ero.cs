using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class GestionarTipoOperacion3Ero : Form
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

        private readonly List<int> _tipOpIds;

        public GestionarTipoOperacion3Ero(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _entities = new papiro_finalEntities();
            _estado = Estado.List;
            _selectedIndex = -1;
            _tipOpIds = new List<int>();
        }

        private void ListarTiposOperacion()
        {
            tipOplistView.Items.Clear();
            _tipOpIds.Clear();
            
            foreach (var tipoOperacion in _entities.tipo_operacion_3ero)
            {
                _tipOpIds.Add(tipoOperacion.id);
                tipOplistView.Items.Add(
                    new ListViewItem(new[]
                                         {
                                             tipoOperacion.valor,
                                             Math.Round(tipoOperacion.valor_operacion, 2).ToString(
                                                 CultureInfo.InvariantCulture)
                                         }));
            }

            ResetEditForm(true);

            _selectedIndex = -1;
        }

        private void GestionarTipoOperacionLoad(object sender, EventArgs e)
        {
            ListarTiposOperacion();
        }

        private void ResetEditForm(bool resetText = false)
        {
            _estado = Estado.List;

            topOpGroupBox.Text = @"Detalles de tipo de operación de terceros";
            NuevoButton.Enabled = true;
            NuevoButton.Text = @"&Nuevo";
            modificarButton.Enabled = true;
            modificarButton.Text = @"&Modificar";
            eliminarButton.Enabled = true;
            eliminarButton.Text = @"&Eliminar";

            descripcionTextBox.ReadOnly = true;
            valorNumericUpDown.ReadOnly = true;

            if (!resetText) return;

            valorNumericUpDown.Value = 0;
            descripcionTextBox.Text = "";
        }

        private void ModificarButtonClick(object sender, EventArgs e)
        {
            switch (_estado)
            {
                case Estado.List:
                    if (_selectedIndex == -1)
                    {
                        MessageBox.Show(@"Debe seleccionar el tipo de operación que desea modificar.",
                                        @"Actualizar tipo de operación de terceros",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    _estado = Estado.Update;
                    topOpGroupBox.Text = @"Edición de tipo de operación de terceros";
                    NuevoButton.Text = @"&Guardar";
                    modificarButton.Enabled = false;
                    eliminarButton.Text = @"&Cancelar";
                    PrepareForEdit();
                    descripcionTextBox.Focus();
                    break;

                case Estado.Insert:
                    Agregar();
                    break;
            }
        }

        private void TipOplistViewItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ResetEditForm();
            
            ListViewItem selectedItem = e.Item;
            selectedItem.Checked = true;

            if (_selectedIndex != -1 && _selectedIndex != e.ItemIndex)
                tipOplistView.Items[_selectedIndex].Checked = false;

            _selectedIndex = e.ItemIndex;

            // Mostrar todos los elementos en el form de editar.
            descripcionTextBox.Text = selectedItem.SubItems[0].Text;
            valorNumericUpDown.Text = selectedItem.SubItems[1].Text;
        }

        private string ValidarDatosForm(int idTipOp)
        {
            // Validar datos del usuario a crear.
            string validationMessage = "";

            if (descripcionTextBox.Text.Trim() == "")
                validationMessage = "Debe especificar el tipo de operación.";

            if (valorNumericUpDown.Value == 0)
                validationMessage += "\nDebe especificar un valor mayor que cero para el tipo de operación.";

            if (_entities.tipo_operacion_3ero.Any(tip => tip.valor == descripcionTextBox.Text.Trim() && tip.id != idTipOp))
                validationMessage += "\nYa existe un tipo de operación con el nombre especificado.";

            return validationMessage;
        }

        private void Agregar()
        {
            try
            {
                var validationMessage = ValidarDatosForm(0);

                if (validationMessage != "")
                {
                    MessageBox.Show(validationMessage, @"Agregar tipo de operación de terceros",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                
                var tipOp = new tipo_operacion_3ero
                             {
                                 valor = descripcionTextBox.Text.Trim(),
                                 valor_operacion = valorNumericUpDown.Value
                             };

                _entities.AddTotipo_operacion_3ero(tipOp);
                
                _entities.SaveChanges();

                //se guarda en bitacora
                var bit = new bitacora
                              {
                                  id_usuario = _user.id,
                                  nombre_usuario = _user.login_nombre,
                                  fecha = DateTime.Now,
                                  accion_realizada =
                                      string.Format("Se agregó el tipo de operación de terceros: {0}", tipOp.valor)
                              };

                _entities.AddTobitacora(bit);
                
                _entities.SaveChanges();

                _estado = Estado.List;

                ListarTiposOperacion();
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                                + @"Excepción: " + exception.Message,
                                @"Agregar tipo de operación de terceros",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Actualizar(int selectedIndex)
        {
            try
            {
                int idTipOp = _tipOpIds[selectedIndex];
                
                var validationMessage = ValidarDatosForm(idTipOp);

                if (validationMessage != "")
                {
                    MessageBox.Show(validationMessage, @"Modificar tipo de operación de terceros",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                var tipoOperacion =
                    (tipo_operacion_3ero)
                    _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.tipo_operacion_3ero", "id", idTipOp));
                tipoOperacion.valor = descripcionTextBox.Text.Trim();
                tipoOperacion.valor_operacion = valorNumericUpDown.Value;
              
                //se guarda en bitacora
                var bit = new bitacora
                              {
                                  id_usuario = _user.id,
                                  nombre_usuario = _user.login_nombre,
                                  fecha = DateTime.Now,
                                  accion_realizada =
                                      string.Format("Se actualizó la tipo de operación de terceros: {0}", tipoOperacion.valor)
                              };

                _entities.AddTobitacora(bit);

                _entities.SaveChanges();

                _estado = Estado.List;

                ListarTiposOperacion();
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                                + @"Excepción: " + exception.Message,
                                @"Modificar tipo de operación de terceros",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrepareForEdit()
        {
            descripcionTextBox.ReadOnly = false;
            valorNumericUpDown.ReadOnly = false;
        }

        private void NuevoButtonClick(object sender, EventArgs e)
        {
            switch (_estado)
            {
                case Estado.List:
                    ResetEditForm(true);
                    PrepareForEdit();
                    _estado = Estado.Insert;
                    topOpGroupBox.Text = @"Nuevo tipo de operación de terceros";
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
            var checkedListViewItemCollection = tipOplistView.CheckedItems;
            var countChecked = checkedListViewItemCollection.Count;

            if (countChecked == 0)
            {
                MessageBox.Show(@"Debe seleccionar al menos un tipo de operación.",
                                @"Eliminar tipo de operación de terceros",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (countChecked == 1)
            {
                if (MessageBox.Show(@"¿Estás seguro de querer eliminar el tipo de operación?",
                                    @"Eliminar tipo de operación de terceros", 
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    == DialogResult.No)
                {
                    return;
                }
            }

            if (countChecked > 1)
            {
                if (MessageBox.Show(@"¿Estás seguro de querer eliminar los tipos de operación seleccionados?",
                                    @"Eliminar tipo de operación de terceros", 
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    == DialogResult.No)
                {
                    return;
                }
            }

            try
            {
                string tipOpNoElimino = "";
                
                // Eliminar todos los 'CheckedItems' del listview

                foreach (tipo_operacion_3ero tipOpByKey in
                                from ListViewItem checkedItem in checkedListViewItemCollection
                                select _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.tipo_operacion_3ero",
                                                                "id", _tipOpIds[checkedItem.Index])))
                {
                    if (_entities.operacion_3ero.Any(p => p.id_tipo_operacion_3ero == tipOpByKey.id))
                    {
                        tipOpNoElimino += ", " + tipOpByKey.valor;
                        continue;
                    }
                    
                    // Adicionar en bitacora.
                    var b = new bitacora
                                {
                                    id_usuario = _user.id,
                                    nombre_usuario = _user.login_nombre,
                                    fecha = DateTime.Now,
                                    accion_realizada =
                                        string.Format("Eliminado el tipo de operación de terceros: {0}",
                                                      tipOpByKey.valor)
                                };

                    _entities.AddTobitacora(b);
                    
                    _entities.DeleteObject(tipOpByKey);
                }

                _entities.SaveChanges();

                if (tipOpNoElimino != "")
                {
                    tipOpNoElimino = tipOpNoElimino.Remove(0, 2);

                    if (tipOpNoElimino.IndexOf(',') == -1)
                        MessageBox.Show(
                            @"No se pudo eliminar el tipo de operación de terceros: '" + tipOpNoElimino +
                            @"', porque tiene operaciones asociadas.", @"Eliminar tipo de operación de terceros",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show(
                            @"No se pudieron eliminar los tipos de operación de terceros: (" + tipOpNoElimino +
                            @"), porque tienen operaciones asociadas.", @"Eliminar tipo de operación de terceros",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                ListarTiposOperacion();
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
