using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace papiro.Reportes
{
    public partial class oepracionesXTipo : Form
    {
        public oepracionesXTipo()
        {
            InitializeComponent();
        }
        private void actualizarLista()
        {
            try
            {
                List<List<Object>> lista = new List<List<Object>>();
                using (var entities = new papiro_finalEntities())
                {
                    if (filtroDesde.Value.Date > filtroHasta.Value.Date)
                        MessageBox.Show("La fecha de inicio debe ser menor que la final", "Error en las fechas del filtro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (filtroHasta.Value.Date > DateTime.Today)
                        MessageBox.Show("La fecha final no puede ser mayor que la fecha actual", "Error en las fechas del filtro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Comienzo a calcular para cada tipo de operaciones
                    else if (filtroDesde.Value.Date == DateTime.Today && filtroHasta.Value.Date == DateTime.Today)
                    {
                        foreach (var tip_op in entities.tipo_operacion)
                        {
                            List<Object> aux = new List<Object>();
                            if (entities.operaciones.Where(op => op.tipo_operacion.id == tip_op.id).Count() > 0)
                            {
                                aux.Add(tip_op.valor);
                                aux.Add(entities.operaciones.Where(op => op.tipo_operacion.id == tip_op.id).Count());
                                aux.Add(entities.operaciones.Where(op => op.tipo_operacion.id == tip_op.id).Sum(ing => ing.monto));
                                lista.Add(aux);
                            }
                            else
                                continue;
                        }
                    }
                    else
                    {
                        foreach (var tip_op in entities.tipo_operacion)
                        {
                            List<Object> aux = new List<Object>();
                            if (entities.operaciones.Where(op => op.tipo_operacion.id == tip_op.id &&
                                 ((op.fecha.Year == filtroDesde.Value.Year && op.fecha.Month == filtroDesde.Value.Month && op.fecha.Day > filtroDesde.Value.Day)
                                || (op.fecha.Year == filtroDesde.Value.Year && op.fecha.Month > filtroDesde.Value.Month) || op.fecha.Year > filtroDesde.Value.Year
                                || (op.fecha.Year == filtroHasta.Value.Year && op.fecha.Month == filtroHasta.Value.Month && op.fecha.Day < filtroHasta.Value.Day)
                                || (op.fecha.Year == filtroHasta.Value.Year && op.fecha.Month < filtroHasta.Value.Month) || op.fecha.Year < filtroHasta.Value.Year
                                || (op.fecha.Year == filtroDesde.Value.Year && op.fecha.Month == filtroDesde.Value.Month && op.fecha.Day == filtroDesde.Value.Day)
                                || (op.fecha.Year == filtroHasta.Value.Year && op.fecha.Month == filtroHasta.Value.Month && op.fecha.Day == filtroHasta.Value.Day))).Count() > 0)
                            {
                                aux.Add(tip_op.valor);
                                aux.Add(entities.operaciones.Where(op => op.tipo_operacion.id == tip_op.id && 
                                 ((op.fecha.Year == filtroDesde.Value.Year && op.fecha.Month == filtroDesde.Value.Month && op.fecha.Day > filtroDesde.Value.Day)
                                || (op.fecha.Year == filtroDesde.Value.Year && op.fecha.Month > filtroDesde.Value.Month) || op.fecha.Year > filtroDesde.Value.Year
                                || (op.fecha.Year == filtroHasta.Value.Year && op.fecha.Month == filtroHasta.Value.Month && op.fecha.Day < filtroHasta.Value.Day)
                                || (op.fecha.Year == filtroHasta.Value.Year && op.fecha.Month < filtroHasta.Value.Month) || op.fecha.Year < filtroHasta.Value.Year
                                || (op.fecha.Year == filtroDesde.Value.Year && op.fecha.Month == filtroDesde.Value.Month && op.fecha.Day == filtroDesde.Value.Day)
                                || (op.fecha.Year == filtroHasta.Value.Year && op.fecha.Month == filtroHasta.Value.Month && op.fecha.Day == filtroHasta.Value.Day))).Count());
                                aux.Add(entities.operaciones.Where(op => op.tipo_operacion.id == tip_op.id &&
                                 ((op.fecha.Year == filtroDesde.Value.Year && op.fecha.Month == filtroDesde.Value.Month && op.fecha.Day > filtroDesde.Value.Day)
                                || (op.fecha.Year == filtroDesde.Value.Year && op.fecha.Month > filtroDesde.Value.Month) || op.fecha.Year > filtroDesde.Value.Year
                                || (op.fecha.Year == filtroHasta.Value.Year && op.fecha.Month == filtroHasta.Value.Month && op.fecha.Day < filtroHasta.Value.Day)
                                || (op.fecha.Year == filtroHasta.Value.Year && op.fecha.Month < filtroHasta.Value.Month) || op.fecha.Year < filtroHasta.Value.Year
                                || (op.fecha.Year == filtroDesde.Value.Year && op.fecha.Month == filtroDesde.Value.Month && op.fecha.Day == filtroDesde.Value.Day)
                                || (op.fecha.Year == filtroHasta.Value.Year && op.fecha.Month == filtroHasta.Value.Month && op.fecha.Day == filtroHasta.Value.Day))).Sum(ing => ing.monto));
                                lista.Add(aux);
                            }
                            else
                                continue;
                        }
                    }
                    while (lista.Count != 0)
                    {
                        var x = lista.Max(l => l[2]);
                        var list = lista.Find(l => l[2] == x);
                        printableListView1.Items.Add(
                          new ListViewItem(new[]
                                                 {
                                                    (string)list[0],
                                                    ((Int32)list[1]).ToString(),
                                                     Math.Round((decimal)list[2], 2).ToString(CultureInfo.InvariantCulture)
                                                 }));
                        lista.Remove(list);

                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error en la aplicación, contacte al equipo técnico", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void oepracionesXTipo_Load(object sender, EventArgs e)
        {
            filtroDesde.Value = DateTime.Today;
            filtroHasta.Value = DateTime.Today;
            actualizarLista();
        }

        private void filtrar_Click(object sender, EventArgs e)
        {
            printableListView1.Items.Clear();
            actualizarLista();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableListView1.Title = "Operaciones por tipo";
            else
                this.printableListView1.Title = "Operaciones por tipo desde " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            this.printableListView1.PrintPreview();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableListView1.Title = "Operaciones por tipo";
            else
                this.printableListView1.Title = "Operaciones por tipo desde " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            this.printableListView1.Print();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
    }
}
