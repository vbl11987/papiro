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
    public partial class reporteProductos : Form
    {
        public reporteProductos()
        {
            InitializeComponent();
        }

        private void reporteProductos_Load(object sender, EventArgs e)
        {
            actualizarLista();
        }
        private void actualizarLista()
        {
            if (numericUpDown.Value < 0)
            {
                MessageBox.Show("El número debe ser mayor que 0", "Error en la entrada de datos", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    if (numericUpDown.Value == 0)
                    {
                        foreach (var prod in entities.producto)
                        {
                            printableLVProducto.Items.Add(new ListViewItem(new[]
                                                {
                                                   prod.nombre,
                                                   prod.tipo_producto.valor,
                                                   prod.unidad_medida.siglas,
                                                   prod.cantidad.ToString(),
                                                   Math.Round(prod.precio, 2).ToString(CultureInfo.InvariantCulture),
                                                   Math.Round(prod.cantidad * prod.precio, 2).ToString(CultureInfo.InvariantCulture)                                                
                                                }
                                                   ));
                        }
                    }
                    else if (numericUpDown.Value > 0)
                    {
                        foreach (var prod in entities.producto.Where(p => p.cantidad < numericUpDown.Value))
                        {
                            printableLVProducto.Items.Add(new ListViewItem(new[]
                                                {
                                                   prod.nombre,
                                                   prod.tipo_producto.valor,
                                                   prod.unidad_medida.siglas,
                                                   prod.cantidad.ToString(),
                                                   Math.Round(prod.precio, 2).ToString(CultureInfo.InvariantCulture),
                                                   Math.Round(prod.cantidad * prod.precio, 2).ToString(CultureInfo.InvariantCulture)                                                
                                                }
                                                   ));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Debe escoger un número mayor que 0", "Error de selección", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error en la aplicación, contacte al equipo técnico", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void filtrar_Click(object sender, EventArgs e)
        {
            printableLVProducto.Items.Clear();
            actualizarLista();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            printableLVProducto.Title = "Reporte de Productos del " + DateTime.Now.ToString("dd/MM/yyyy");
            printableLVProducto.PrintPreview();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            printableLVProducto.Title = "Reporte de Productos del " + DateTime.Now.ToString("dd/MM/yyyy");
            printableLVProducto.Print();
        }
    }
}
