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
    public partial class ReporteUtiles : Form
    {
        public ReporteUtiles()
        {
            InitializeComponent();
        }

        public void ActualizarReporte()
        {
            if(numericUpDown.Value < 0)
            {
                MessageBox.Show("El número debe ser mayor que 0", "Error en la entrada de datos", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    if(numericUpDown.Value == 0)
                    {
                        foreach (var utiles in entities.utiles_herramientas)
                        {
                            printableLVProducto.Items.Add(new ListViewItem(new[]
                                                {
                                                   utiles.nombre,
                                                   utiles.tipo_utiles.valor,
                                                   utiles.unidad_medida.siglas,
                                                   utiles.cantidad.ToString(),
                                                   Math.Round(utiles.precio, 2).ToString(CultureInfo.InvariantCulture),
                                                   Math.Round(utiles.cantidad * utiles.precio, 2).ToString(CultureInfo.InvariantCulture)                                                
                                                }
                                                   ));
                        }
                    }
                    else
                    {
                        foreach (var utiles in entities.utiles_herramientas.Where(u => u.cantidad < numericUpDown.Value))
                        {
                            printableLVProducto.Items.Add(new ListViewItem(new[]
                                                {
                                                   utiles.nombre,
                                                   utiles.tipo_utiles.valor,
                                                   utiles.unidad_medida.siglas,
                                                   utiles.cantidad.ToString(),
                                                   Math.Round(utiles.precio, 2).ToString(CultureInfo.InvariantCulture),
                                                   Math.Round(utiles.cantidad * utiles.precio, 2).ToString(CultureInfo.InvariantCulture)                                                
                                                }
                                                   ));
                        }
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void ReporteUtiles_Load(object sender, EventArgs e)
        {
            ActualizarReporte();
        }

        private void filtrar_Click(object sender, EventArgs e)
        {
            printableLVProducto.Items.Clear();
            ActualizarReporte();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.printableLVProducto.Title = "Reporte de Útiles y Herramientas";
            this.printableLVProducto.PrintPreview();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.printableLVProducto.Title = "Reporte de Útiles y Herramientas";
            this.printableLVProducto.Print();
        }
    }
}
