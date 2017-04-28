using System;
using System.Globalization;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class EstadoResultados : Form
    {
        public EstadoResultados()
        {
            InitializeComponent();
        }

        private void ActualizarReporte()
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    decimal costos = 0.0m;
                    decimal gastos = 0.0m;
                    decimal ingresos = 0.0m;
                    decimal otros_gastos = 0.0m;
                    printableLV.Items.Clear();
                    
                    foreach (var op in entities.operaciones)
                    {
                        if (op.fecha.Date < filtroDesde.Value.Date) continue;

                        if (op.fecha.Date > filtroHasta.Value.Date) continue;

                        costos += op.costo;
                        if (op.gasto != null) gastos += op.gasto.Value;
                        ingresos += op.monto;
                    }

                    foreach (var s_gastos in entities.submayor_gasto)
                    {
                        if (s_gastos.fecha.Date < filtroDesde.Value.Date) continue;

                        if (s_gastos.fecha.Date > filtroHasta.Value.Date) continue;

                        if (s_gastos.debito != null) otros_gastos += s_gastos.debito.Value;
                    }
                    if (filtroDesde.Value == DateTime.Now && filtroHasta.Value == DateTime.Now)
                    {
                        foreach (var user in entities.usuarios)
                        {
                            if (user.id == 1) continue;
                            if (user.id == 16) continue;

                            otros_gastos += user.salario_fijo;
                            if (user.salario_extra_operaciones != null)
                                otros_gastos += user.salario_extra_operaciones.Value;
                        }
                        gastos += otros_gastos;
                    }

                    //Imprimo los costos en el prinLV
                    printableLV.Items.Add(new ListViewItem(new[]
                                                {
                                                    @"Costos", 
                                                    Math.Round(costos, 2).ToString(CultureInfo.InvariantCulture)
                                                }
                                            ));
                    //Imprimo los gastos en el printLV
                    printableLV.Items.Add(new ListViewItem(new[]
                                                {
                                                    @"Gastos", 
                                                    Math.Round(gastos, 2).ToString(CultureInfo.InvariantCulture)
                                                }
                                            ));
                    //Imprimo los ingresos en el printLV
                    printableLV.Items.Add(new ListViewItem(new[]
                                                {
                                                    @"Ingresos", 
                                                    Math.Round(ingresos, 2).ToString(CultureInfo.InvariantCulture)
                                                }
                                           ));
                    //Imprimo las utilidades
                    printableLV.Items.Add(new ListViewItem(new[]
                                                {
                                                    @"Utilidad", 
                                                    Math.Round((ingresos - (gastos + costos)), 2).ToString(CultureInfo.InvariantCulture)
                                                }
                                           ));
                    //IMprimo Costo por pesos
                    printableLV.Items.Add(new ListViewItem(new[]
                                                {
                                                    @"Costo por pesos", 
                                                    (ingresos != 0
                                                      ? Math.Round(costos/ingresos, 2).ToString(CultureInfo.InvariantCulture)
                                                      : "0.00")
                                                }
                                           ));

                    //IMprimo gastos por pesos
                    printableLV.Items.Add(new ListViewItem(new[]
                                                {
                                                    @"Gasto por pesos", 
                                                    (ingresos != 0
                                                         ? Math.Round(gastos/ingresos, 2).ToString(
                                                             CultureInfo.InvariantCulture)
                                                         : "0.00")
                                                }
                                           ));
                    //Imprimo costo mas gasto por pesos
                    printableLV.Items.Add(new ListViewItem(new[]
                                                {
                                                    @"Costo mas gasto por pesos", 
                                                    (ingresos != 0
                                                      ? Math.Round((costos + gastos)/ingresos, 2).ToString(
                                                          CultureInfo.InvariantCulture)
                                                      : "0.00")
                                                }
                                           ));

                    //costoslabel.Text = @"Costos: $" + Math.Round(costos, 2).ToString(CultureInfo.InvariantCulture);
                    //gastoslabel.Text = @"Gastos: $" + Math.Round(gastos, 2).ToString(CultureInfo.InvariantCulture);
                    //Ingresoslabel.Text = @"Ingresos: $" + Math.Round(ingresos, 2).ToString(CultureInfo.InvariantCulture);
                    //utilidadLabel.Text = @"Utilidad: $" + Math.Round((ingresos - (gastos + costos)), 2).ToString(CultureInfo.InvariantCulture);

                    //costosPesoslabel.Text = @"Costo por pesos: $" +
                    //                         (ingresos != 0
                    //                              ? Math.Round(costos/ingresos, 2).ToString(CultureInfo.InvariantCulture)
                    //                              : "0.00");
                    //gastoPesoslabel.Text = @"Gasto por pesos: $" + (ingresos != 0
                    //                                     ? Math.Round(gastos/ingresos, 2).ToString(
                    //                                         CultureInfo.InvariantCulture)
                    //                                     : "0.00");
                    //costoGastoPesoslabel.Text = @"Costo mas gasto por pesos: $" +
                    //                             (ingresos != 0
                    //                                  ? Math.Round((costos + gastos)/ingresos, 2).ToString(
                    //                                      CultureInfo.InvariantCulture)
                    //                                  : "0.00");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Estado de resultados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EstadoResultadosLoad(object sender, EventArgs e)
        {
            filtroDesde.Value = DateTime.Today;
            filtroHasta.Value = DateTime.Today;
            
            ActualizarReporte();
        }

        private void FiltrarClick(object sender, EventArgs e)
        {
            if(filtroDesde.Value.Date > filtroHasta.Value.Date)
            {
                MessageBox.Show("La fecha de inicio debe ser menor que la fecha final", "Error en la entrada de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ActualizarReporte();
        }

        private void Button1Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableLV.Title = "Estado de resultados";
            else
            {
                this.printableLV.Title = "Estado de resultados desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta el " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            }
            printableLV.PrintPreview();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableLV.Title = "Estado de resultados";
            else
            {
                this.printableLV.Title = "Estado de resultados desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta el " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            }
            printableLV.Print();
        }

        private void verdetalles_Click(object sender, EventArgs e)
        {
            EstadoResultadosVerDetalles x = new EstadoResultadosVerDetalles(filtroDesde.Value.Date, filtroHasta.Value.Date);
            x.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            EstadoResultadosVerDetallesGastos g = new EstadoResultadosVerDetallesGastos(filtroDesde.Value.Date, filtroHasta.Value.Date);
            g.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            EstadoResultadosVerDetallesIngresos i = new EstadoResultadosVerDetallesIngresos(filtroDesde.Value.Date, filtroHasta.Value.Date);
            i.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            EstadoResultadosVerDetallesUtilidades u = new EstadoResultadosVerDetallesUtilidades(filtroDesde.Value.Date, filtroHasta.Value.Date);
            u.ShowDialog();
        }
    }
}
