using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;
using Range = Microsoft.Office.Interop.Word.Range;

namespace papiro.formularios
{
    public partial class GestionarContratos : Form
    {
        private readonly usuarios _user;

        private readonly papiro_finalEntities _entities;

        private readonly List<int> _clientesId;

        private readonly List<int> _tipoContratoId;

        private readonly List<int> _tipoPagoId;

        private int _selectedIndex;

        private enum Estado
        {
            Insert,
            Update,
            List
        }
        
        private Estado _estado;

        public GestionarContratos(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _entities = new papiro_finalEntities();
            _clientesId = new List<int> { -1 };
            _tipoContratoId = new List<int> { -1 };
            _tipoPagoId = new List<int> { -1 };
            _estado = Estado.List;
            _selectedIndex = -1;
        }

        private void ListarContratos()
        {
            // Filtrar de acuerdo a las opciones de filtro seleccionadas.

            string queryString = "SELECT VALUE c FROM papiro_finalEntities.contrato AS c WHERE c.terminado = 0";

            string description = filtroDescripción.Text.Trim();

            if (description != "")
                queryString += " and SqlServer.UPPER(c.descripcion) LIKE @descript ";

            if (clienteToolStripComboBox.SelectedIndex > 0)
                queryString += " and c.id_cliente = @client ";

            ObjectQuery<contrato> objectQuery = _entities.CreateQuery<contrato>(queryString);

            if (description != "")
                objectQuery.Parameters.Add(new ObjectParameter("descript", "%" + description.ToUpper() + "%"));

            if (clienteToolStripComboBox.SelectedIndex > 0)
                objectQuery.Parameters.Add(new ObjectParameter("client",
                                                               _clientesId[clienteToolStripComboBox.SelectedIndex]));

            // Mostrarlos en el listview.
            
            //ContratoslistView.Items.Clear();
            printableLVcontrato.Items.Clear();

            foreach (var item in objectQuery.ToList().Select(
                                        contract =>
                                            new ListViewItem(
                                                new[]
                                                    {
                                                        contract.dir_fich_contrato, 
                                                        contract.descripcion ?? "", 
                                                        contract.cliente.nombre, 
                                                        contract.tipo_contrato.valor,
                                                        contract.tipo_pago.valor,
                                                        contract.fecha_inic.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("es")),
                                                        contract.fecha_fin.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("es"))
                                                    })))
            {
                //ContratoslistView.Items.Add(item);
                printableLVcontrato.Items.Add(item);
            }

            ResetEditForm(true);

            _selectedIndex = -1;
        }

        private void GestionarProductosLoad(object sender, EventArgs e)
        {
            foreach (cliente cliente in _entities.cliente.Where(c => c.nombre != "Cliente casual"))
            {
                clientesComboBox.Items.Add(cliente.nombre);
                clienteToolStripComboBox.Items.Add(cliente.nombre);
                _clientesId.Add(cliente.id);
            }

            foreach (tipo_contrato tipoContrato in _entities.tipo_contrato)
            {
                tipoContractComboBox.Items.Add(tipoContrato.valor);
                _tipoContratoId.Add(tipoContrato.id);
            }

            foreach (tipo_pago tipoPago in _entities.tipo_pago)
            {
                formaPagoComboBox.Items.Add(tipoPago.valor);
                _tipoPagoId.Add(tipoPago.id);
            }

            ListarContratos();
        }

        private void ResetEditForm(bool resetText = false)
        {
            _estado = Estado.List;

            productoGroupBox.Text = @"Detalles del contrato";
            NuevoButton.Enabled = true;
            NuevoButton.Text = @"&Nuevo";
            modificarButton.Enabled = true;
            modificarButton.Text = @"&Modificar";
            eliminarButton.Enabled = true;
            eliminarButton.Text = @"&Eliminar";

            descripcionTextBox.ReadOnly = true;
            fechaInicio.Enabled = false;
            fechaFin.Enabled = false;
            tipoContractComboBox.Enabled = false;
            clientesComboBox.Enabled = false;
            tipoContractComboBox.Enabled = false;
            formaPagoComboBox.Enabled = false;
            ficherobutton.Text = @"...";
            ficherobutton.Enabled = false;
            cerrarContratobutton.Enabled = false;
            mostrarfacturasbutton.Enabled = false;

            if (!resetText) return;

            descripcionTextBox.Text = "";
            clientesComboBox.SelectedIndex = 0;
            fechaInicio.Value = DateTime.Now;
            fechaFin.Value = DateTime.Now;
            tipoContractComboBox.SelectedIndex = 0;
            tipoContractComboBox.SelectedIndex = 0;
            formaPagoComboBox.SelectedIndex = 0;
        }

        private void FiltrarClick(object sender, EventArgs e)
        {
            ResetEditForm(true);
            ListarContratos();
        }

        private void ModificarButtonClick(object sender, EventArgs e)
        {
            switch (_estado)
            {
                case Estado.List:
                    if (_selectedIndex == -1)
                    {
                        MessageBox.Show(@"Debe seleccionar el contrato que desea modificar.", @"Actualizar contrato",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    _estado = Estado.Update;
                    productoGroupBox.Text = @"Edición de contrato";
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

        private void ContratoslistViewItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            //ResetEditForm();
            
            //ListViewItem selectedItem = e.Item;
            //selectedItem.Checked = true;

            //if (_selectedIndex != -1 && _selectedIndex != e.ItemIndex)
            //    //ContratoslistView.Items[_selectedIndex].Checked = false;

            //_selectedIndex = e.ItemIndex;

            ////string fich = ContratoslistView.Items[_selectedIndex].SubItems[0].Text;
            ////var contrato = _entities.contrato.Single(c => c.dir_fich_contrato == fich);
            
            //descripcionTextBox.Text = contrato.descripcion;

            //for (int indClient = 0; indClient < _clientesId.Count; indClient++)
            //{
            //    if (_clientesId[indClient] != contrato.id_cliente) continue;
            //    clientesComboBox.SelectedIndex = indClient;
            //    break;
            //}

            //for (int indTipoContract = 0; indTipoContract < _tipoContratoId.Count; indTipoContract++)
            //{
            //    if (_tipoContratoId[indTipoContract] != contrato.id_tipo_contrato) continue;
            //    tipoContractComboBox.SelectedIndex = indTipoContract;
            //    break;
            //}

            //for (int indTipoPago = 0; indTipoPago < _tipoPagoId.Count; indTipoPago++)
            //{
            //    if (_tipoPagoId[indTipoPago] != contrato.id_tipo_pago) continue;
            //    formaPagoComboBox.SelectedIndex = indTipoPago;
            //    break;
            //}

            //fechaInicio.Value = contrato.fecha_inic;
            //fechaFin.Value = contrato.fecha_fin;

            //string path = Assembly.GetExecutingAssembly().CodeBase.Remove(0, 8);
            //path = path.Substring(0, path.LastIndexOf('/') + 1).Replace('/', '\\');
            //if (File.Exists(path + "Contratos" + "\\" + contrato.dir_fich_contrato + ".docx"))
            //{
            //    string fichero = contrato.dir_fich_contrato;
            //    if (fichero.Length > 100)
            //        fichero = fichero.Substring(0, 100) + "...";
            //    ficherobutton.Enabled = true;
            //    ficherobutton.Text = fichero;
            //}
            //else
            //{
            //    ficherobutton.Text = @"Documento no encontrado";
            //    ficherobutton.Enabled = false;
            //}

            //cerrarContratobutton.Enabled = true;
            //mostrarfacturasbutton.Enabled = true;
        }

        private string ValidarDatosForm(int idContract)
        {
            // Validar datos del producto a crear.
            string validationMessage = "";

            if (clientesComboBox.SelectedIndex < 1)
                validationMessage += "\nDebe especificar el cliente con el cual se realiza el contrato.";
            
            if (tipoContractComboBox.SelectedIndex < 1)
                validationMessage += "\nDebe especificar el tipo de contrato.";

            if (formaPagoComboBox.SelectedIndex < 1)
                validationMessage += "\nDebe especificar la forma de pago del contrato.";

            if (fechaFin.Value.Date < fechaInicio.Value.Date)
                validationMessage += "\nLa fecha inicial debe ser menor o igual a la final.";

            int idCliente = _clientesId[clientesComboBox.SelectedIndex];
            if (_entities.contrato.Any(c => c.terminado == 0 && c.id != idContract && c.id_cliente == idCliente))
                validationMessage += "\nYa el cliente " + clientesComboBox.Text + " tiene un contrato.";

            return validationMessage;
        }

        private static void Replace(Document document, string key, string value)
        {
            object missing = Type.Missing;

            foreach (Range range in document.StoryRanges)
            {
                range.Find.Text = key;
                range.Find.Replacement.Text = value;
                range.Find.Wrap = WdFindWrap.wdFindContinue;
                object replaceAll = WdReplace.wdReplaceAll;

                range.Find.Execute(ref missing, ref missing, ref missing,
                                   ref missing, ref missing, ref missing, ref missing,
                                   ref missing, ref missing, ref missing, ref replaceAll,
                                   ref missing, ref missing, ref missing, ref missing);
            }
        }

        private void Agregar()
        {
            Application wordApp = null;
            Document document = null;
            object missing = Type.Missing;
            
            try
            {
                string validationMessage = ValidarDatosForm(0);

                if (validationMessage != "")
                {
                    MessageBox.Show(validationMessage, @"Registrar nuevo contrato",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                var contract = new contrato
                                   {
                                       descripcion = descripcionTextBox.Text.Trim(),
                                       id_cliente = _clientesId[clientesComboBox.SelectedIndex],
                                       id_tipo_contrato = _tipoContratoId[tipoContractComboBox.SelectedIndex],
                                       id_tipo_pago = _tipoPagoId[formaPagoComboBox.SelectedIndex],
                                       fecha_inic = fechaInicio.Value,
                                       fecha_fin = fechaFin.Value,
                                       terminado = 0,
                                   };

                int max = _entities.contrato.Count() == 0
                              ? 0
                              : Enumerable.Max(_entities.contrato,
                                               c => Convert.ToInt32(c.dir_fich_contrato.Substring(0, 2)));
                string num = (max + 1).ToString(CultureInfo.InvariantCulture);
                if (num.Length == 1)
                    num = "0" + num;
                string fich = num + DateTime.Today.ToString("yy", CultureInfo.InvariantCulture);
                contract.dir_fich_contrato = fich;

                _entities.AddTocontrato(contract);
                
                _entities.SaveChanges();

                //se guarda en bitacora
                var bit = new bitacora
                              {
                                  id_usuario = _user.id,
                                  nombre_usuario = _user.login_nombre,
                                  fecha = DateTime.Now,
                                  accion_realizada = "Se registró un nuevo " + ToString(contract)
                              };

                _entities.AddTobitacora(bit);
                
                _entities.SaveChanges();

                _estado = Estado.List;

                ListarContratos();

                wordApp = new Application();

                string path = Assembly.GetExecutingAssembly().CodeBase.Remove(0, 8);
                path = path.Substring(0, path.LastIndexOf('/') + 1).Replace('/', '\\');

                if (!File.Exists(path + "PLANTILLA_CONTRATO.docx"))
                    throw new Exception("No existe la plantilla a partir de la cual se crea el documento de contrato");

                object fileName = path + "PLANTILLA_CONTRATO.docx";
                document = wordApp.Documents.Open(ref fileName,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, true, ref missing,
                    ref missing, ref missing, ref missing);

                document.Activate();

                var today = DateTime.Today;
                var invariantCulture = CultureInfo.InvariantCulture;
                string cliente = contract.cliente.nombre;
                string empresa = contract.cliente.empresa;
                if (string.IsNullOrEmpty(empresa))
                    empresa = "          ";
                Replace(document, "<%Client%>", cliente);
                Replace(document, "<%Enterprise%>", empresa);
                Replace(document, "<%Day%>", today.Day.ToString(invariantCulture));
                Replace(document, "<%Month%>", today.ToString("MMMM", CultureInfo.GetCultureInfo("es")));
                Replace(document, "<%Year%>", today.Year.ToString(invariantCulture));

                if (!Directory.Exists(path + "Contratos"))
                    Directory.CreateDirectory(path + "Contratos");

                document.SaveAs(path + "Contratos" + "\\" + contract.dir_fich_contrato);

                wordApp.Visible = true;

                _entities.SaveChanges();
            }
            catch (Exception exception)
            {
                MessageBox.Show("No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                                + @"Excepción: " + exception.Message,
                                "Agregar contrato",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (document != null)
                    document.Close(ref missing, ref missing, ref missing);
                if (wordApp != null)
                    wordApp.Application.Quit(ref missing, ref missing, ref missing);
            }
        }

        private void Actualizar(int selectedIndex)
        {
            Application wordApp = null;
            Document document = null;
            object missing = Type.Missing;
            
            try
            {
                //string fich = ContratoslistView.Items[_selectedIndex].SubItems[0].Text;
                string fich = printableLVcontrato.Items[_selectedIndex].SubItems[0].Text;
                var contrato = _entities.contrato.Single(c => c.dir_fich_contrato == fich);
                
                string validationMessage = ValidarDatosForm(contrato.id);

                if (validationMessage != "")
                {
                    MessageBox.Show(validationMessage, @"Modificar contrato",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                contrato.descripcion = descripcionTextBox.Text.Trim();
                contrato.id_cliente = _clientesId[clientesComboBox.SelectedIndex];
                contrato.id_tipo_contrato = _tipoContratoId[tipoContractComboBox.SelectedIndex];
                contrato.id_tipo_pago = _tipoPagoId[formaPagoComboBox.SelectedIndex];
                contrato.fecha_inic = fechaInicio.Value;
                contrato.fecha_fin = fechaFin.Value;

                //se guarda en bitacora
                var bit = new bitacora
                              {
                                  id_usuario = _user.id,
                                  nombre_usuario = _user.login_nombre,
                                  fecha = DateTime.Now,
                                  accion_realizada = "Se actualizó el " + ToString(contrato)
                              };

                _entities.AddTobitacora(bit);

                _entities.SaveChanges();

                // Actualizar el documento de contrato.
                string path = Assembly.GetExecutingAssembly().CodeBase.Remove(0, 8);
                path = path.Substring(0, path.LastIndexOf('/') + 1).Replace('/', '\\');

                if (MessageBox.Show(@"¿Desea actualizar el documento de contrato?",
                                @"Actualizar contrato", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
                {
                    if (!File.Exists(path + "PLANTILLA_CONTRATO.docx"))
                        throw new Exception(
                            "No existe la plantilla a partir de la cual se crea el documento de contrato");

                    wordApp = new Application();

                    object fileName = path + "PLANTILLA_CONTRATO.docx";
                    document = wordApp.Documents.Open(ref fileName,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, true, ref missing,
                        ref missing, ref missing, ref missing);

                    document.Activate();

                    var today = DateTime.Today;
                    var invariantCulture = CultureInfo.InvariantCulture;
                    string cliente = contrato.cliente.nombre;
                    string empresa = contrato.cliente.empresa;
                    if (string.IsNullOrEmpty(empresa))
                        empresa = "          ";
                    Replace(document, "<%Client%>", cliente);
                    Replace(document, "<%Enterprise%>", empresa);
                    Replace(document, "<%Day%>", today.Day.ToString(invariantCulture));
                    Replace(document, "<%Month%>", today.ToString("MMMM", CultureInfo.GetCultureInfo("es")));
                    Replace(document, "<%Year%>", today.Year.ToString(invariantCulture));

                    if (!Directory.Exists(path + "Contratos"))
                        Directory.CreateDirectory(path + "Contratos");

                    document.SaveAs(path + "Contratos" + "\\" + contrato.dir_fich_contrato);

                    wordApp.Visible = true;
                }

                _estado = Estado.List;

                ListarContratos();
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                                + @"Excepción: " + exception.Message,
                                @"Actualizar contrato",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (document != null)
                    document.Close(ref missing, ref missing, ref missing);
                if (wordApp != null)
                    wordApp.Application.Quit(ref missing, ref missing, ref missing);
            }
        }

        private void PrepareForEdit()
        {
            descripcionTextBox.ReadOnly = false;
            fechaInicio.Enabled = true;
            fechaFin.Enabled = true;
            clientesComboBox.Enabled = true;
            tipoContractComboBox.Enabled = true;
            formaPagoComboBox.Enabled = true;
            cerrarContratobutton.Enabled = false;
        }

        private void NuevoButtonClick(object sender, EventArgs e)
        {
            switch (_estado)
            {
                case Estado.List:
                    ResetEditForm(true);
                    PrepareForEdit();
                    _estado = Estado.Insert;
                    productoGroupBox.Text = @"Nuevo contrato";
                    NuevoButton.Enabled = false;
                    modificarButton.Text = @"&Guardar";
                    eliminarButton.Text = @"&Cancelar";
                    descripcionTextBox.Focus();
                    break;
                case Estado.Update:
                    _estado = Estado.List;
                    // Guardar cambios en la BD
                    if (_selectedIndex != -1)
                        Actualizar(_selectedIndex);
                    break;
            }
        }

        private static string ToString(contrato contract)
        {
            return "Contrato con Id: " + contract.dir_fich_contrato + ", Cliente:" + contract.cliente.nombre;
        }

        private void Eliminar()
        {
            //var checkedListViewItemCollection = ContratoslistView.CheckedItems;
            var checkedListViewItemCollection = printableLVcontrato.CheckedItems;
            var countChecked = checkedListViewItemCollection.Count;

            if (countChecked == 0)
            {
                MessageBox.Show(@"Debe seleccionar al menos un contrato.", @"Eliminar contrato",
                                    MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }

            if (countChecked == 1)
            {
                if (MessageBox.Show(@"¿Estás seguro de querer eliminar el contrato seleccionado?",
                                    @"Eliminar contrato", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.No)
                {
                    return;
                }
            }

            if (countChecked > 1)
            {
                if (MessageBox.Show(@"¿Estás seguro de querer eliminar los contratos seleccionados?",
                                    @"Eliminar contratos", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.No)
                {
                    return;
                }
            }

            try
            {
                // Eliminar todos los 'CheckedItems' del listview de productos

                string operacionesNoElimino = "";
                var docsToDelete = new List<string>();
                string path = Assembly.GetExecutingAssembly().CodeBase.Remove(0, 8);
                path = path.Substring(0, path.LastIndexOf('/') + 1).Replace('/', '\\');

                foreach (contrato contractByKey in
                                from ListViewItem checkedItem in checkedListViewItemCollection
                                let fich = checkedItem.SubItems[0].Text
                                select _entities.contrato.Single(c => c.dir_fich_contrato == fich))
                {
                    int idContrato = contractByKey.id;

                    if (_entities.operaciones.Any(op => op.id_contrato == idContrato))
                    {
                        operacionesNoElimino += ", " + contractByKey.dir_fich_contrato;
                        continue;
                    }
                    
                    // Adicionar en bitacora.
                    var b = new bitacora
                                {
                                    id_usuario = _user.id,
                                    nombre_usuario = _user.login_nombre,
                                    fecha = DateTime.Now,
                                    accion_realizada = "Eliminado el " + ToString(contractByKey)
                                };

                    _entities.AddTobitacora(b);
                    
                    if (File.Exists(path + "Contratos" + "\\" + contractByKey.dir_fich_contrato + ".docx"))
                    {
                        if (MessageBox.Show(@"¿Desea eliminar el documento del contrato con código " + contractByKey.dir_fich_contrato + @"?",
                                    @"Eliminar contrato", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            docsToDelete.Add(path + "Contratos" + "\\" + contractByKey.dir_fich_contrato + ".docx");
                        }
                    }
                    
                    _entities.DeleteObject(contractByKey);
                }

                if (operacionesNoElimino != "")
                {
                    operacionesNoElimino = operacionesNoElimino.Remove(0, 2);

                    if (operacionesNoElimino.IndexOf(',') == -1)
                        MessageBox.Show(
                            @"No se pudo eliminar el contrato: '" + operacionesNoElimino +
                            @"', porque tiene operaciones asociadas.", @"Eliminar contrato",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show(
                            @"No se pudieron eliminar los contratos: (" + operacionesNoElimino +
                            @"), porque tienen operaciones asociadas.", @"Eliminar contrato",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                _entities.SaveChanges();

                foreach (var doc in docsToDelete)
                    File.Delete(doc);

                ListarContratos();
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

        private void FicherobuttonClick(object sender, EventArgs e)
        {
            Application wordApp = null;
            Document document = null;
            object missing = Type.Missing;

            try
            {
                //string fichero = ContratoslistView.Items[_selectedIndex].SubItems[0].Text;
                string fichero = printableLVcontrato.Items[_selectedIndex].SubItems[0].Text;

                string path = Assembly.GetExecutingAssembly().CodeBase.Remove(0, 8);
                path = path.Substring(0, path.LastIndexOf('/') + 1).Replace('/', '\\');
                
                if (!File.Exists(path + "Contratos" + "\\" + fichero + ".docx"))
                {
                    MessageBox.Show("No existe el documento de contrato solicitado.\n", "Abrir documento de contrato",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                wordApp = new Application();

                object fileName = path + "Contratos" + "\\" + fichero + ".docx";
                document = wordApp.Documents.Open(ref fileName,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, true, ref missing,
                    ref missing, ref missing, ref missing);

                document.Activate();

                wordApp.Visible = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show("No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                                + @"Excepción: " + exception.Message,
                                "Abrir documento de contrato",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (document != null)
                    document.Close(ref missing, ref missing, ref missing);
                if (wordApp != null)
                    wordApp.Application.Quit(ref missing, ref missing, ref missing);
            }
        }

        private void CerrarContratobuttonClick(object sender, EventArgs e)
        {
            // Facturar.
            
            Microsoft.Office.Interop.Excel.Application application = null;
            _Workbook book = null;
            object missing = Type.Missing;
            
            try
            {
                //string fich = ContratoslistView.Items[_selectedIndex].SubItems[0].Text;
                string fich = printableLVcontrato.Items[_selectedIndex].SubItems[0].Text;
                var contract = _entities.contrato.Single(c => c.dir_fich_contrato == fich);

                string path = Assembly.GetExecutingAssembly().CodeBase.Remove(0, 8);
                path = path.Substring(0, path.LastIndexOf('/') + 1).Replace('/', '\\');

                if (!File.Exists(path + "PLANTILLA_FACTURA.xlsx"))
                    throw new Exception(
                        "No existe la plantilla a partir de la cual se crea la factura.");

                // Operaciones
                var operacionesSeleccionadas = new SeleccionOperaciones(contract);
                operacionesSeleccionadas.ShowDialog();
                List<int> opSeleccionadas = operacionesSeleccionadas.OpSeleccionadas;

                if (opSeleccionadas.Count == 0)
                {
                    MessageBox.Show("Para generar el documento de factura debe seleccionar alguna operación.",
                                "Facturar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                application = new Microsoft.Office.Interop.Excel.Application();

                book = application.Workbooks.Open(path + "PLANTILLA_FACTURA.xlsx", missing, missing,
                                                  missing, missing, missing, missing, missing,
                                                  missing, missing, missing, missing, missing, missing,
                                                  missing);
                _Worksheet sheet = book.Worksheets[1];

                int max = _entities.factura.Count() == 0
                              ? 0
                              : Enumerable.Max(_entities.factura,
                                               f => Convert.ToInt32(f.dir_fich_factura.Substring(0, 3)));
                string num = (max + 1).ToString(CultureInfo.InvariantCulture);
                if (num.Length == 1)
                    num = "00" + num;
                else if (num.Length == 2)
                    num = "0" + num;
                string id = num + DateTime.Today.ToString("MMyy", CultureInfo.InvariantCulture);

                // llenar el sheet.

                sheet.Cells[7, "C"] = id;
                sheet.Cells[8, "C"] = contract.cliente.nombre ?? "";
                sheet.Cells[9, "C"] = DateTime.Now.ToString("dd/MM/yyyy");

                // Tipo de contrato.
                switch (contract.id_tipo_contrato)
                {
                    case 2: // Puntual
                        sheet.Cells[11, "D"] = "X";
                        break;
                    case 1: // Lineal
                        sheet.Cells[11, "F"] = "X";
                        break;
                }

                // Tipo de pago.
                switch (contract.id_tipo_pago)
                {
                    case 1: sheet.Cells[12, "D"] = "X";  // puntual
                        break;
                    case 3: sheet.Cells[13, "D"] = "X";  // anticipado
                        break;
                    case 2: sheet.Cells[14, "D"] = "X";  // a credito
                        break;
                }

                int pos = 17;
                foreach (var op in contract.operaciones)
                {
                    if (!opSeleccionadas.Contains(op.id)) continue;

                    op.facturada = true;
                    
                    // Obtener el producto por su tipo.
                    if (op.id_tipo_producto == null || op.id_tipo_producto == 0)
                        continue;
                    int idTipProd = op.id_tipo_producto.Value;
                    producto producto =
                        _entities.producto.Where(prod => prod.id_tipo_producto == idTipProd).SingleOrDefault
                            ();

                    sheet.Cells[pos, "B"] = op.tipo_operacion.valor;
                    sheet.Cells[pos, "C"] = op.cantidad;
                    sheet.Cells[pos, "D"] = producto.unidad_medida.siglas;
                    sheet.Cells[pos, "E"] = Math.Round(op.valor_tipo_operacion.Value, 2);
                    pos++;
                }

                if (!Directory.Exists(path + "Facturas"))
                    Directory.CreateDirectory(path + "Facturas");

                book.SaveAs(path + "Facturas" + "\\" + id);

                var fact = new factura
                               {
                                   id_contrato = contract.id,
                                   fecha = DateTime.Now,
                                   dir_fich_factura = id
                               };

                _entities.AddTofactura(fact);

                _entities.SaveChanges();

                foreach (var op in contract.operaciones)
                {
                    if (!opSeleccionadas.Contains(op.id)) continue;
                    op.id_factura = fact.id;
                }

                _entities.SaveChanges();

                application.Visible = true;

                ListarContratos();
            }
            catch (Exception exception)
            {
                MessageBox.Show("No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                                + @"Excepción: " + exception.Message,
                                "Facturar",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (book != null) book.Close(missing, missing, missing);
                if (application != null) application.Application.Quit();
            }
        }

        private void MostrarfacturasbuttonClick(object sender, EventArgs e)
        {
            // Mostrar el formulario con las facturas relacionadas al contrato.

            //string fich = ContratoslistView.Items[_selectedIndex].SubItems[0].Text;
            string fich = printableLVcontrato.Items[_selectedIndex].SubItems[0].Text;
            var contrato = _entities.contrato.Single(c => c.dir_fich_contrato == fich);

            var mostrarFact = new MostrarFacturasForm(contrato);
            mostrarFact.ShowDialog();
        }

        private void printableLVcontrato_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ResetEditForm();

            ListViewItem selectedItem = e.Item;
            selectedItem.Checked = true;

            if (_selectedIndex != -1 && _selectedIndex != e.ItemIndex)
                printableLVcontrato.Items[_selectedIndex].Checked = false;

            _selectedIndex = e.ItemIndex;

            string fich = printableLVcontrato.Items[_selectedIndex].SubItems[0].Text;
            var contrato = _entities.contrato.Single(c => c.dir_fich_contrato == fich);

            descripcionTextBox.Text = contrato.descripcion;

            for (int indClient = 0; indClient < _clientesId.Count; indClient++)
            {
                if (_clientesId[indClient] != contrato.id_cliente) continue;
                clientesComboBox.SelectedIndex = indClient;
                break;
            }

            for (int indTipoContract = 0; indTipoContract < _tipoContratoId.Count; indTipoContract++)
            {
                if (_tipoContratoId[indTipoContract] != contrato.id_tipo_contrato) continue;
                tipoContractComboBox.SelectedIndex = indTipoContract;
                break;
            }

            for (int indTipoPago = 0; indTipoPago < _tipoPagoId.Count; indTipoPago++)
            {
                if (_tipoPagoId[indTipoPago] != contrato.id_tipo_pago) continue;
                formaPagoComboBox.SelectedIndex = indTipoPago;
                break;
            }

            fechaInicio.Value = contrato.fecha_inic;
            fechaFin.Value = contrato.fecha_fin;

            string path = Assembly.GetExecutingAssembly().CodeBase.Remove(0, 8);
            path = path.Substring(0, path.LastIndexOf('/') + 1).Replace('/', '\\');
            if (File.Exists(path + "Contratos" + "\\" + contrato.dir_fich_contrato + ".docx"))
            {
                string fichero = contrato.dir_fich_contrato;
                if (fichero.Length > 100)
                    fichero = fichero.Substring(0, 100) + "...";
                ficherobutton.Enabled = true;
                ficherobutton.Text = fichero;
            }
            else
            {
                ficherobutton.Text = @"Documento no encontrado";
                ficherobutton.Enabled = false;
            }

            cerrarContratobutton.Enabled = true;
            mostrarfacturasbutton.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            printableLVcontrato.Title = "Contratos";
            printableLVcontrato.PrintPreview();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printableLVcontrato.Title = "Contratos";
            printableLVcontrato.Print();
        }
    }
}
