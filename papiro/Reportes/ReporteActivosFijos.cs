﻿using System;
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
    public partial class ReporteActivosFijos : Form
    {
        public ReporteActivosFijos()
        {
            InitializeComponent();
        }

        private void ReporteActivosFijos_Load(object sender, EventArgs e)
        {
            ActualizarReporte();
        }
        private void ActualizarReporte()
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
                    if (numericUpDown.Value == 0)
                    {
                        foreach (var activos in entities.activos_fijos)
                        {
                            printableLVProducto.Items.Add(new ListViewItem(new[]
                                                {
                                                   activos.nombre,
                                                   activos.tipo_activos_fijos.valor,
                                                   activos.unidad_medida.siglas,
                                                   activos.cantidad.ToString(),
                                                   Math.Round(activos.precio, 2).ToString(CultureInfo.InvariantCulture),
                                                   Math.Round(activos.cantidad * activos.precio, 2).ToString(CultureInfo.InvariantCulture)                                                
                                                }
                                                   ));
                        }
                    }
                    else
                    {
                        foreach (var activos in entities.activos_fijos.Where(a => a.cantidad < numericUpDown.Value))
                        {
                            printableLVProducto.Items.Add(new ListViewItem(new[]
                                                {
                                                   activos.nombre,
                                                   activos.tipo_activos_fijos.valor,
                                                   activos.unidad_medida.siglas,
                                                   activos.cantidad.ToString(),
                                                   Math.Round(activos.precio, 2).ToString(CultureInfo.InvariantCulture),
                                                   Math.Round(activos.cantidad * activos.precio, 2).ToString(CultureInfo.InvariantCulture)                                                
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

        private void filtrar_Click(object sender, EventArgs e)
        {
            printableLVProducto.Items.Clear();
            ActualizarReporte();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.printableLVProducto.Title = "Reporte de Activos FIjos";
            this.printableLVProducto.PrintPreview();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.printableLVProducto.Title = "Reporte de Activos FIjos";
            this.printableLVProducto.Print();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
