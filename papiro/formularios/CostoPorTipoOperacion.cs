using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace papiro.formularios
{
    public partial class CostoPorTipoOperacion : Form
    {
        private int id_top;
        private DateTime desde;
        private DateTime hasta;
        public CostoPorTipoOperacion(int id_top, DateTime desde, DateTime hasta)
        {
            InitializeComponent();
            this.id_top = id_top;
            this.desde = desde;
            this.hasta = hasta;
        }

        private void CostoPorTipoOperacion_Load(object sender, EventArgs e)
        {
            filtroDesde.Value = desde;
            filtroHasta.Value = hasta;

            ActualizarReporte();
        }
        private void ActualizarReporte()
        {
            try 
            {
                using (var _entities = new papiro_finalEntities())
                {
                    decimal total = 0;
                    foreach (var op in _entities.operaciones)
                    {
                        if (op.fecha.Date < filtroDesde.Value.Date) continue;

                        if (op.fecha.Date > filtroHasta.Value.Date) continue;

                        if (op.tipo_operacion.id == this.id_top)
                        {
                            printableLV.Items.Add(
                            new ListViewItem(new[]
                                    {
                                        op.descripcion,
                                        op.tipo_operacion.valor,
                                         Math.Round(op.costo, 2).ToString(CultureInfo.InvariantCulture),
                                         op.fecha.ToString("dd/MM/yyyy")
                                    }
                                )
                            );
                            total += op.costo;
                        }
                    }
                    printableLV.Items.Add(
                            new ListViewItem(new[]
                                    {
                                        "",
                                        "Total",
                                         Math.Round(total, 2).ToString(CultureInfo.InvariantCulture),
                                         ""
                                    }
                                )
                            );
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableLV.Title = "Costo por tipo de operación";
            else
                this.printableLV.Title = "Costo por tipo de operación desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta el " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            printableLV.PrintPreview();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableLV.Title = "Costo por tipo de operación";
            else
                this.printableLV.Title = "Costo por tipo de operación desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta el " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            printableLV.Print();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void filtrar_Click(object sender, EventArgs e)
        {
            if(filtroDesde.Value.Date > filtroHasta.Value.Date)
            {
                MessageBox.Show("La fecha de inicio debe ser menor que la fecha final", "Error en la entrada de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ActualizarReporte();
        }
    }
}
