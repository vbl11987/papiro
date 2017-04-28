using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class GestionarTipoProducto : Form
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

        private readonly List<int> _tipProdIds;

        public GestionarTipoProducto(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _entities = new papiro_finalEntities();
            _estado = Estado.List;
            _selectedIndex = -1;
            _tipProdIds = new List<int>();
        }

        private void ListarTipProd()
        {
            tipProdlistView.Items.Clear();
            _tipProdIds.Clear();

            foreach (var tipoProducto in _entities.tipo_producto)
            {
                _tipProdIds.Add(tipoProducto.id);
                tipProdlistView.Items.Add(new ListViewItem(new[] {tipoProducto.valor}));
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

            tipoProdTextBox.ReadOnly = true;

            if (!resetText) return;

            tipoProdTextBox.Text = "";
        }

        private void ModificarButtonClick(object sender, EventArgs e)
        {
            switch (_estado)
            {
                case Estado.List:
                    if (_selectedIndex == -1)
                    {
                        MessageBox.Show(@"Debe seleccionar el tipo de producto que desea modificar.",
                                        @"Actualizar tipo de producto",
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

        private void UmlistViewItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ResetEditForm();
            
            ListViewItem selectedItem = e.Item;
            selectedItem.Checked = true;

            if (_selectedIndex != -1 && _selectedIndex != e.ItemIndex)
                tipProdlistView.Items[_selectedIndex].Checked = false;

            _selectedIndex = e.ItemIndex;

            // Mostrar todos los elementos en el form de editar.
            tipoProdTextBox.Text = selectedItem.SubItems[0].Text;
        }

        private string ValidarDatosForm(int idTipProd)
        {
            // Validar datos del usuario a crear.
            string validationMessage = "";

            if (tipoProdTextBox.Text.Trim() == "")
                validationMessage += "\nDebe especificar el tipo de producto.";

            if (_entities.tipo_producto.Any(tipProd => tipProd.valor == tipoProdTextBox.Text.Trim() && tipProd.id != idTipProd))
                validationMessage += "\nYa existe el tipo de producto especificado.";

            return validationMessage;
        }

        private void Agregar()
        {
            try
            {
                var validationMessage = ValidarDatosForm(0);

                if (validationMessage != "")
                {
                    MessageBox.Show(validationMessage, @"Agregar nuevo tipo de producto",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                
                var tipoProducto = new tipo_producto()
                             {
                                 valor = tipoProdTextBox.Text.Trim()
                             };

                _entities.AddTotipo_producto(tipoProducto);
                
                _entities.SaveChanges();

                //se guarda en bitacora
                var bit = new bitacora
                              {
                                  id_usuario = _user.id,
                                  nombre_usuario = _user.login_nombre,
                                  fecha = DateTime.Now,
                                  accion_realizada =
                                      string.Format("Se agregó el tipo de productos: {0}", tipoProducto.valor)
                              };

                _entities.AddTobitacora(bit);
                
                _entities.SaveChanges();

                _estado = Estado.List;

                ListarTipProd();
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                                + @"Excepción: " + exception.Message,
                                @"Agregar tipo de producto",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Actualizar(int selectedIndex)
        {
            try
            {
                int idTipProd = _tipProdIds[selectedIndex];
                
                var validationMessage = ValidarDatosForm(idTipProd);

                if (validationMessage != "")
                {
                    MessageBox.Show(validationMessage, @"Modificar tipo de producto",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                var tipoProducto =
                    (tipo_producto)
                    _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.tipo_producto", "id", idTipProd));
                tipoProducto.valor = tipoProdTextBox.Text.Trim();
              
                //se guarda en bitacora
                var bit = new bitacora
                              {
                                  id_usuario = _user.id,
                                  nombre_usuario = _user.login_nombre,
                                  fecha = DateTime.Now,
                                  accion_realizada =
                                      string.Format("Se actualizó el tipo de producto: {0}", tipoProducto.valor)
                              };

                _entities.AddTobitacora(bit);

                _entities.SaveChanges();

                _estado = Estado.List;

                ListarTipProd();
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                                + @"Excepción: " + exception.Message,
                                @"Modificar tipo de producto",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrepareForEdit()
        {
            tipoProdTextBox.ReadOnly = false;
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
            var checkedListViewItemCollection = tipProdlistView.CheckedItems;
            var countChecked = checkedListViewItemCollection.Count;

            if (countChecked == 0)
            {
                MessageBox.Show(@"Debe seleccionar al menos un tipo de producto.", @"Eliminar tipo de producto",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (countChecked == 1)
            {
                if (MessageBox.Show(@"¿Estás seguro de querer eliminar el tipo de producto?",
                                    @"Eliminar tipo de producto", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    == DialogResult.No)
                {
                    return;
                }
            }

            if (countChecked > 1)
            {
                if (MessageBox.Show(@"¿Estás seguro de querer eliminar los tipos de producto seleccionados?",
                                    @"Eliminar tipo de producto", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    == DialogResult.No)
                {
                    return;
                }
            }

            try
            {
                string tipProdNoEliminoProd = "";

                string opNoElimino = "";
                
                foreach (tipo_producto tipProdByKey in
                                from ListViewItem checkedItem in checkedListViewItemCollection
                                select _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.tipo_producto",
                                                                "id", _tipProdIds[checkedItem.Index])))
                {
                    int tipOpId = tipProdByKey.id;
                    
                    if (_entities.producto.Any(prod => prod.id_tipo_producto == tipOpId))
                    {
                        tipProdNoEliminoProd += ", " + tipProdByKey.valor;
                        continue;
                    }

                    if (_entities.operaciones.Any(op => op.id_tipo_producto == tipOpId))
                    {
                        opNoElimino += ", " + tipProdByKey.valor;
                        continue;
                    }
                    
                    // Adicionar en bitacora.
                    var b = new bitacora
                                {
                                    id_usuario = _user.id,
                                    nombre_usuario = _user.login_nombre,
                                    fecha = DateTime.Now,
                                    accion_realizada = string.Format("Eliminado tipo de producto: {0}", tipProdByKey.valor)
                                };

                    _entities.AddTobitacora(b);
                    
                    _entities.DeleteObject(tipProdByKey);
                }

                _entities.SaveChanges();

                if (tipProdNoEliminoProd != "")
                {
                    tipProdNoEliminoProd = tipProdNoEliminoProd.Remove(0, 2);

                    if (tipProdNoEliminoProd.IndexOf(',') == -1)
                        MessageBox.Show(
                            @"No se pudo eliminar el tipo de producto: '" + tipProdNoEliminoProd +
                            @"', porque tiene productos asociados.", @"Eliminar tipo de producto",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show(
                            @"No se pudieron eliminar los tipos de producto: (" + tipProdNoEliminoProd +
                            @"), porque tienen productos asociados.", @"Eliminar tipo de producto",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (opNoElimino != "")
                {
                    opNoElimino = opNoElimino.Remove(0, 2);

                    if (opNoElimino.IndexOf(',') == -1)
                        MessageBox.Show(
                            @"No se pudo eliminar el tipo de producto: '" + tipProdNoEliminoProd +
                            @"', porque tiene operaciones asociadas.", @"Eliminar tipo de producto",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show(
                            @"No se pudieron eliminar los tipos de producto: (" + tipProdNoEliminoProd +
                            @"), porque tienen operaciones asociadas.", @"Eliminar tipo de producto",
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
    }
}
