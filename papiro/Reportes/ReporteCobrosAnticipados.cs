using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace papiro.Reportes
{
    public partial class ReporteCobrosAnticipados : Form
    {
        private readonly papiro_finalEntities _entities;
        public ReporteCobrosAnticipados()
        {
            InitializeComponent();
            _entities = new papiro_finalEntities();
        }

        private void ReporteCobrosAnticipados_Load(object sender, EventArgs e)
        {
            filtroDesde.Value = DateTime.Now;
            filtroHasta.Value = DateTime.Now;
            this.Reload();
        }
        private void Reload()
        {
            try
            {
                foreach (var ca in _entities.cobro_anticipado)
                {
                    if(filtroDesde.Value.Date > ca.fecha.Value.Date)
                        continue;
                    if (filtroHasta.Value.Date < ca.fecha.Value.Date)
                        continue;

                    printableListView1.Items.Add(new ListViewItem(new[]
                                                                      {
                                                                          ca.cliente.nombre,
                                                                          Math.Round(ca.cantidad,2).ToString(CultureInfo.InvariantCulture),
                                                                          ca.tipo_cobro,
                                                                          ca.descripcion,
                                                                          ca.fecha.Value.Date.ToString("dd/MM/yyyy")
                                                                      }));
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.printableListView1.Title = "Reporte de Cobros Anticipados";
            this.printableListView1.PrintPreview();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.printableListView1.Title = "Reporte de Cobros Anticipados";
            this.printableListView1.Print();
        }

        private void filtrar_Click(object sender, EventArgs e)
        {
            this.Reload();
        }
    }
}
