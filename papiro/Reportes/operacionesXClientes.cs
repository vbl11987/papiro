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
    public partial class operacionesXClientes : Form
    {
        public operacionesXClientes()
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
                    else if(filtroHasta.Value.Date > DateTime.Today)
                        MessageBox.Show("La fecha final no puede ser mayor que la fecha actual", "Error en las fechas del filtro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //Si las fechas son las que salen por defecto calculo pata todos las operaciones
                    else if (filtroDesde.Value.Date == DateTime.Today && filtroHasta.Value.Date == DateTime.Today)
                    {
                        foreach (var client in entities.cliente)
                        {
                            if (client.id == 13) continue;
                            List<Object> aux = new List<Object>();
 
                            if (entities.operaciones.Where(op => op.cliente.id == client.id).Count() > 0)
                            {
                                aux.Add(client.nombre);
                                aux.Add(entities.operaciones.Where(op => op.cliente.id == client.id).Count());
                                aux.Add(entities.operaciones.Where(op => op.cliente.id == client.id).Sum(ing => ing.monto));
                                lista.Add(aux);
                            }
                            else
                                continue;
                        }
                    }
                    //de lo contrario busco para el rango de fechas
                    else
                    {
                        foreach (var client in entities.cliente)
                        {
                            if (client.id == 13) continue;
                            List<Object> aux = new List<Object>();

                            if (entities.operaciones.Where(op => op.cliente.id == client.id &&
                                ((op.fecha.Year == filtroDesde.Value.Year && op.fecha.Month == filtroDesde.Value.Month && op.fecha.Day > filtroDesde.Value.Day)
                                || (op.fecha.Year == filtroDesde.Value.Year && op.fecha.Month > filtroDesde.Value.Month) || op.fecha.Year > filtroDesde.Value.Year
                                || (op.fecha.Year == filtroHasta.Value.Year && op.fecha.Month == filtroHasta.Value.Month && op.fecha.Day < filtroHasta.Value.Day)
                                || (op.fecha.Year == filtroHasta.Value.Year && op.fecha.Month < filtroHasta.Value.Month) || op.fecha.Year < filtroHasta.Value.Year
                                || (op.fecha.Year == filtroDesde.Value.Year && op.fecha.Month == filtroDesde.Value.Month && op.fecha.Day == filtroDesde.Value.Day)
                                || (op.fecha.Year == filtroHasta.Value.Year && op.fecha.Month == filtroHasta.Value.Month && op.fecha.Day == filtroHasta.Value.Day))).Count() > 0)
                            {
                                aux.Add(client.nombre);
                                aux.Add(entities.operaciones.Where(op => op.cliente.id == client.id &&
                                ((op.fecha.Year == filtroDesde.Value.Year && op.fecha.Month == filtroDesde.Value.Month && op.fecha.Day > filtroDesde.Value.Day)
                                || (op.fecha.Year == filtroDesde.Value.Year && op.fecha.Month > filtroDesde.Value.Month) || op.fecha.Year > filtroDesde.Value.Year
                                || (op.fecha.Year == filtroHasta.Value.Year && op.fecha.Month == filtroHasta.Value.Month && op.fecha.Day < filtroHasta.Value.Day)
                                || (op.fecha.Year == filtroHasta.Value.Year && op.fecha.Month < filtroHasta.Value.Month) || op.fecha.Year < filtroHasta.Value.Year
                                || (op.fecha.Year == filtroDesde.Value.Year && op.fecha.Month == filtroDesde.Value.Month && op.fecha.Day == filtroDesde.Value.Day)
                                || (op.fecha.Year == filtroHasta.Value.Year && op.fecha.Month == filtroHasta.Value.Month && op.fecha.Day == filtroHasta.Value.Day))).Count());
                                aux.Add(entities.operaciones.Where(op => op.cliente.id == client.id &&
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
                    //ordeno la lista por el que mas ingresos tenga
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

        private void operacionesXClientes_Load(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableListView1.Title = "Operaciones por clientes";
            else
                this.printableListView1.Title = "Operaciones por clientes desde " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            this.printableListView1.PrintPreview();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableListView1.Title = "Operaciones por clientes";
            else
                this.printableListView1.Title = "Operaciones por clientes desde " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            this.printableListView1.Print();
        }
    }
}
