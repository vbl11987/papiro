using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class OperacionesPendientes : Form
    {
        private readonly usuarios _user;

        private readonly List<int> _operacionesId;

        private readonly List<int> _clientesId;

        private readonly List<int> _tipOpId;

        private readonly List<int> _tipProdId;
        
        public OperacionesPendientes(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _operacionesId = new List<int>();
           _clientesId = new List<int>{-1};
            _tipOpId = new List<int> {-1};
            _tipProdId = new List<int> {-1};
        }

        private void ListarOperacionesPendientes()
        {
            try
            {
                printableLV.Items.Clear();
                _operacionesId.Clear();
                
                using (var entities = new papiro_finalEntities())
                {
                    int clientId = _clientesId[clienteToolStripComboBox.SelectedIndex];
                    int tipOpId = _tipOpId[tipoOptoolStripComboBox.SelectedIndex];
                    int tipProdId = _tipProdId[tipProdtoolStripComboBox.SelectedIndex];
                    foreach (var op in entities.operaciones.
                           Where(op => 
                                 op.pendiente_imprimir == 1 && 
                                 (clienteToolStripComboBox.SelectedIndex == 0 || clientId == op.id_cliente) &&
                                 (tipoOptoolStripComboBox.SelectedIndex == 0 || tipOpId == op.id_tipo_operacion) &&
                                 (tipProdtoolStripComboBox.SelectedIndex == 0 || tipProdId == op.id_tipo_producto)))
                    {
                        _operacionesId.Add(op.id);

                        printableLV.Items.Add(
                            new ListViewItem(new[]
                                                 {
                                                     op.cliente.nombre,
                                                     (op.contrato == null ? "" : op.contrato.descripcion),
                                                     (op.tipo_operacion == null ? "" : op.tipo_operacion.valor),
                                                     (op.tipo_producto == null ? "" : op.tipo_producto.valor),
                                                     op.fecha.ToString("dd/MM/yyyy"), op.descripcion,
                                                     Math.Round(op.monto, 2).ToString(CultureInfo.InvariantCulture)
                                                 }));
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                    + @"Excepción: " + exception.Message,
                    "Listar operaciones pendientes",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OperacionesPendientesLoad(object sender, EventArgs e)
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    clienteToolStripComboBox.Items.Clear();
                    clienteToolStripComboBox.Items.Add("(Cliente)");
                    foreach(var client in entities.cliente)
                    {
                        _clientesId.Add(client.id);
                        clienteToolStripComboBox.Items.Add(client.nombre);
                    }
                    clienteToolStripComboBox.SelectedIndex = 0;

                    tipoOptoolStripComboBox.Items.Clear();
                    tipoOptoolStripComboBox.Items.Add("(Tipo de operación)");
                    foreach (var tipoOp in entities.tipo_operacion)
                    {
                        _tipOpId.Add(tipoOp.id);
                        tipoOptoolStripComboBox.Items.Add(tipoOp.valor);
                    }
                    tipoOptoolStripComboBox.SelectedIndex = 0;

                    tipProdtoolStripComboBox.Items.Clear();
                    tipProdtoolStripComboBox.Items.Add("(Tipo de producto)");
                    foreach (var tipoProducto in entities.tipo_producto)
                    {
                        _tipProdId.Add(tipoProducto.id);
                        tipProdtoolStripComboBox.Items.Add(tipoProducto.valor);
                    }
                    tipProdtoolStripComboBox.SelectedIndex = 0;
                    
                    ListarOperacionesPendientes();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                    + "Excepción: " + exception.Message,
                    "Operaciones pendientes",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            ListarOperacionesPendientes();
        }

        private void FiltrarClick(object sender, EventArgs e)
        {
            ListarOperacionesPendientes();
        }

        private void SalirbuttonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void EjecutaroperacionbuttonClick(object sender, EventArgs e)
        {
            if (printableLV.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Debe seleccionar la operación que desea ejecutar.", "Operaciones pendientes",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    var operacionPendiente =
                        (operaciones)
                        entities.GetObjectByKey(new EntityKey("papiro_finalEntities.operaciones", "id",
                                                              _operacionesId[printableLV.SelectedIndices[0]]));
                    
                    var opImpresion = new OperacionImpresion(_user, operacionPendiente);
                    opImpresion.ShowDialog();        
                }

                ListarOperacionesPendientes();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                    + "Excepción: " + exception.Message,
                    "Operaciones pendientes",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelOpbuttonClick(object sender, EventArgs e)
        {
            // Cancelar operacion de diseño.

            if (printableLV.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Debe seleccionar la operación que desea cancelar.", "Operaciones pendientes",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    var operacionPendiente =
                        (operaciones)
                        entities.GetObjectByKey(new EntityKey("papiro_finalEntities.operaciones", "id",
                                                              _operacionesId[printableLV.SelectedIndices[0]]));

                    entities.operaciones.DeleteObject(operacionPendiente);

                    entities.SaveChanges();
                }

                ListarOperacionesPendientes();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                    + "Excepción: " + exception.Message,
                    "Operaciones pendientes",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            printableLV.Title = "Operaciones pendientes";
            printableLV.PrintPreview();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printableLV.Title = "Operaciones pendientes";
            printableLV.Print();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (printableLV.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Debe seleccionar la operación que desea ejecutar.", "Operaciones pendientes",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    var operacionPendiente =
                        (operaciones)
                        entities.GetObjectByKey(new EntityKey("papiro_finalEntities.operaciones", "id",
                                                              _operacionesId[printableLV.SelectedIndices[0]]));
                    var aux = operacionPendiente.id_cliente;
                    if(!entities.cobro_anticipado.Where(ca => ca.id_cliente == aux).Any())
                    {
                        MessageBox.Show("El cliente de esta operación no tiene registrado cobro anticipado",
                                        "Error de selección", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var opImpresion = new OperacionCobroAnticipado(_user, operacionPendiente);
                    opImpresion.ShowDialog();
                }

                ListarOperacionesPendientes();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                    + "Excepción: " + exception.Message,
                    "Operaciones pendientes",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
