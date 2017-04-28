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
    public partial class EstadoResultadosVerDetalles : Form
    {
        private DateTime desde;
        private DateTime hasta;
        private readonly List<int> _tipoOperacionId;
        public EstadoResultadosVerDetalles(DateTime desde, DateTime hasta)
        {
            InitializeComponent();
            this.desde = desde.Date;
            this.hasta = hasta.Date;
            _tipoOperacionId = new List<int> { -1 };
        }
        public void ActualizarReporte()
        {
            try
            {
                printableLV.Items.Clear();

                using (var entities = new papiro_finalEntities())
                {
                    decimal total = 0;
                    foreach (var t_op in entities.tipo_operacion)
                    {
                        decimal operacion_costo = 0;
                        foreach (var op in entities.operaciones)
                        {
                            if (op.fecha.Date < filtroDesde.Value.Date) continue;

                            if (op.fecha.Date > filtroHasta.Value.Date) continue;

                            if (op.tipo_operacion.id == t_op.id)
                                operacion_costo += op.costo;
                        }
                        printableLV.Items.Add(
                            new ListViewItem(new []
                                    {
                                        t_op.valor,
                                         Math.Round(operacion_costo, 2).ToString(CultureInfo.InvariantCulture)
                                    }
                                )
                            );
                        total += operacion_costo;
                    }
                    printableLV.Items.Add(
                           new ListViewItem(new[]
                                    {
                                        "Total",
                                         Math.Round(total, 2).ToString(CultureInfo.InvariantCulture)
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

        private void EstadoResultadosVerDetalles_Load(object sender, EventArgs e)
        {
            filtroDesde.Value = desde;
            filtroHasta.Value = hasta;

            //inicializo el combo de tipo de operaciones
            cbxTipoOP.Items.Add("<Seleccione>");
            using(var _entities = new papiro_finalEntities())
            {
                foreach (tipo_operacion tipoOperacion in _entities.tipo_operacion)
                {
                    _tipoOperacionId.Add(tipoOperacion.id);
                    cbxTipoOP.Items.Add(tipoOperacion.valor);
                }
            }
            cbxTipoOP.SelectedIndex = 0;
            ActualizarReporte();
        }

        private void filtrar_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date > filtroHasta.Value.Date)
            {
                MessageBox.Show("La fecha de inicio debe ser menor que la fecha final", "Error en la entrada de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            this.printableLV.Items.Clear();
            ActualizarReporte();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableLV.Title = "Detalles de los costos por tipos de operaciones";
            else
            {
                this.printableLV.Title = "Detalles de los costos por tipos de operaciones desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            }
            printableLV.PrintPreview();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableLV.Title = "Detalles de los costos por tipos de operaciones";
            else
            {
                this.printableLV.Title = "Detalles de los costos por tipos de operaciones desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            }
            printableLV.Print();
        }

        private void verdetalles_Click(object sender, EventArgs e)
        {
            if (cbxTipoOP.SelectedIndex < 1)
            {
                MessageBox.Show("Debe seleccionar un tipo de operación", "@Ver detalles", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (filtroDesde.Value.Date > filtroHasta.Value.Date)
            {
                MessageBox.Show("La fecha de inicio debe ser menor que la fecha final", "Error en la entrada de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            CostoPorTipoOperacion ctop = new CostoPorTipoOperacion(_tipoOperacionId[cbxTipoOP.SelectedIndex], this.desde, this.hasta);
            ctop.ShowDialog();
        }
    }
}
