using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class CostoPorOperaciones : Form
    {
        public CostoPorOperaciones()
        {
            InitializeComponent();
        }

        private void ActualizarReporte()
        {
            try
            {
                printableLV.Items.Clear();
                
                using (var entities = new papiro_finalEntities())
                {
                    foreach (var op in entities.operaciones)
                    {
                        if (op.fecha.Date < filtroDesde.Value.Date) continue;

                        if (op.fecha.Date > filtroHasta.Value.Date) continue;
                        
                        //decimal monto = op.monto;
                        //if (op.operacion_3ero.Count() != 0)
                        //    monto -= op.operacion_3ero.Sum(g => g.cantidad * g.valor_operacion_3ero.Value);

                        printableLV.Items.Add(
                            new ListViewItem(new[]
                                                 {
                                                     op.descripcion, op.fecha.ToString("dd/MM/yyyy"),
                                                     op.id_tipo_operacion == null ? "" : op.tipo_operacion.valor,
                                                     Math.Round(op.costo, 2).ToString(CultureInfo.InvariantCulture)
                                                     ,
                                                     Math.Round(op.gasto.Value, 2).ToString(CultureInfo.InvariantCulture)
                                                     ,
                                                     Math.Round(op.monto, 2).ToString(CultureInfo.InvariantCulture),
                                                     Math.Round(op.monto - op.gasto.Value - op.costo, 2).ToString(
                                                         CultureInfo.InvariantCulture)
                                                 }));
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Costo por operaciones", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GestionarCuentasCobrarLoad(object sender, EventArgs e)
        {
            filtroDesde.Value = DateTime.Today;
            filtroHasta.Value = DateTime.Today;
            
            ActualizarReporte();
        }

        private void FiltrarClick(object sender, EventArgs e)
        {
            ActualizarReporte();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printableLV.PrintPreview();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            printableLV.Print();
        }
    }
}
