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
    public partial class DetallesGastos : Form
    {
        private DateTime desde;
        private DateTime hasta;
        private string nombre_cuenta;

        public DetallesGastos(DateTime desde, DateTime hasta, string nombre_cuenta)
        {
            InitializeComponent();
            this.desde = desde;
            this.hasta = hasta;
            this.nombre_cuenta = nombre_cuenta;
            this.Text += @"-" + nombre_cuenta;
        }

        private void DetallesGastos_Load(object sender, EventArgs e)
        {
            filtroDesde.Value = desde;
            filtroHasta.Value = hasta;
            cbxdetalles.Items.Add("<Seleccione>");
            cbxdetalles.SelectedIndex = 0;

            ActualizarReporte();
        }

        private void ActualizarReporte()
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    List<List<Object>> aux = new List<List<object>>();

                    foreach (var gasto in entities.gasto.Where(g => g.nombre_cuenta.Equals(nombre_cuenta)))
                    {
                        if(gasto.fecha.Date < filtroDesde.Value.Date) continue;

                        if(gasto.fecha.Date > filtroHasta.Value.Date) continue;

                        List<Object> auxiliar = new List<object>();
                        auxiliar.Add(gasto.nombre_partida);
                        auxiliar.Add(gasto.gasto1);
                        aux.Add(auxiliar);
                    }

                    List<string> lista_partida = new List<string>();
                    List<decimal> lista_gasto = new List<decimal>();

                    //Realizo un ciclo para calcular todos los totales de gastos para las partidas
                    foreach (var elemento in aux)
                    {
                        if(lista_partida.Count == 0)
                        {
                            lista_partida.Add((string)elemento[0]);
                            cbxdetalles.Items.Add((string)elemento[0]);
                            lista_gasto.Add((decimal)elemento[1]);
                            continue;
                        }
                        var auxiliar = lista_partida.Find(l => l.Equals(elemento[0]));
                        if (auxiliar == null)
                        {
                            lista_partida.Add((string)elemento[0]);
                            //Agrego al combobox el nombre de la partida
                            cbxdetalles.Items.Add((string) elemento[0]);
                            lista_gasto.Add((decimal)elemento[1]);
                            continue;
                        }
                        else
                        {
                            var x = lista_partida.FindIndex(l => l.Equals(elemento[0]));
                            lista_gasto[x] += (decimal) elemento[1];
                        }
                    }

                    for (int i = 0; i < lista_partida.Count; i++)
                    {
                        printableLV.Items.Add(
                         new ListViewItem(new[]
                                                 {
                                                    lista_partida[i],
                                                    Math.Round(lista_gasto[i], 2).ToString(CultureInfo.InvariantCulture)
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
            if(filtroDesde.Value.Date > filtroHasta.Value.Date)
            {
                MessageBox.Show("La fecha de inicio debe ser menor que la fecha final", "Error en la entrada de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ActualizarReporte();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableLV.Title = "Gastos de " + nombre_cuenta;
            else
            {
                this.printableLV.Title = "Gastos de " + nombre_cuenta + " desde el " + filtroDesde.Value.ToString("dd/MM/yyyy") +
                                         " hasta el " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            }
            this.printableLV.PrintPreview();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableLV.Title = "Gastos de " + nombre_cuenta;
            else
            {
                this.printableLV.Title = "Gastos de " + nombre_cuenta + " desde el " + filtroDesde.Value.ToString("dd/MM/yyyy") +
                                         " hasta el " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            }
            this.printableLV.Print();
        }

        private void verdetalles_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date > filtroHasta.Value.Date)
            {
                MessageBox.Show("La fecha de inicio debe ser menor que la fecha final", "Error en la entrada de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DetallesGastosXPartidas dxp = new DetallesGastosXPartidas(desde, hasta, cbxdetalles.SelectedItem.ToString());
            dxp.ShowDialog();
        }
    }
}
