using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class DetallesGastosXPartidas : Form
    {
        private DateTime desde;
        private DateTime hasta;
        private string nombre_partida;

        public DetallesGastosXPartidas(DateTime desde, DateTime hasta, string nombre_partida)
        {
            InitializeComponent();
            this.desde = desde;
            this.hasta = hasta;
            this.nombre_partida = nombre_partida;
            this.Text += @"-" + nombre_partida;
        }

        private void DetallesGastosXPartidas_Load(object sender, EventArgs e)
        {
            ActualizarReporte();
        }

        private void ActualizarReporte()
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    foreach (var g in entities.gasto.Where(g => g.nombre_partida.Equals(this.nombre_partida)))
                    {
                        printableLV.Items.Add(
                        new ListViewItem(new[]
                                                 {
                                                    g.nombre_elemento,
                                                    Math.Round(g.gasto1, 2).ToString(CultureInfo.InvariantCulture),
                                                    g.fecha.ToString("dd/MM/yyyy")
                                                 }));
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void filtrar_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date > filtroHasta.Value.Date)
            {
                MessageBox.Show("La fecha de inicio debe ser menor que la fecha final", "Error en la entrada de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ActualizarReporte();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableLV.Title = "Gastos de " + nombre_partida;
            else
            {
                this.printableLV.Title = "Gastos de " + nombre_partida + " desde el " + filtroDesde.Value.ToString("dd/MM/yyyy") +
                                         " hasta el " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            }
            printableLV.PrintPreview();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableLV.Title = "Gastos de " + nombre_partida;
            else
            {
                this.printableLV.Title = "Gastos de " + nombre_partida + " desde el " + filtroDesde.Value.ToString("dd/MM/yyyy") +
                                         " hasta el " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            }
            printableLV.Print();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
