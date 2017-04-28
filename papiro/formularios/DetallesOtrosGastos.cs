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
    public partial class DetallesOtrosGastos : Form
    {
        private DateTime desde;
        private DateTime hasta;
        private string tipo_gasto;

        public DetallesOtrosGastos(DateTime desde, DateTime hasta, string  tipo_gasto)
        {
            InitializeComponent();
            this.desde = desde;
            this.hasta = hasta;
            this.tipo_gasto = tipo_gasto;
        }

        private void DetallesOtrosGastos_Load(object sender, EventArgs e)
        {
            filtroDesde.Value = desde;
            filtroHasta.Value = hasta;

            //Si el criterio de busqueda es para las mermas preparo el listview con las columnas que necesita
            if(tipo_gasto.Equals("Gastos por Merma"))
            {
                this.Text = "Gastos por Merma";
                this.printableLV.Columns.Add("Tipo de Elemento", 250);
                this.printableLV.Columns.Add("Gasto", 150);
                //this.printableLV.Columns.Add("Fecha");
                ActualizarReporte();
            }
            //Si el criterio de busqueda es para los gastos de salario
            else if (tipo_gasto.Equals("Gastos por Salario"))
            {
                this.Text = "Gastos por Salario";
                this.printableLV.Columns.Add("Usuario", 250);
                this.printableLV.Columns.Add("Gasto", 150);
                ActualizarReporte();
            }
            else if (tipo_gasto.Equals("Gastos por operaciones"))
            {
                this.Text = "Gastos por operaciones";
                this.printableLV.Columns.Add("Fecha", 250);
                this.printableLV.Columns.Add("Gasto", 150);
                this.printableLV.Columns.Add("Descripción", 350);
                ActualizarReporte();
            }
            else
            {
                this.Text = "Gastos por operaciones con 3eros";
                this.printableLV.Columns.Add("Fecha", 250);
                this.printableLV.Columns.Add("Gasto", 150);
                this.printableLV.Columns.Add("Proveedor", 250);
                //this.printableLV.Columns.Add("Descripción");
                ActualizarReporte();
            }
        }

        private void ActualizarReporte()
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    //Relleno la lista para cada uno de los criterios

                    //Si es por merma
                    if(tipo_gasto.Equals("Gastos por Merma"))
                    {
                        //Mermas de productos
                        printableLV.Items.Add(
                                  new ListViewItem(new[]
                                        {
                                            "Mermas de Productos",
                                             "(*****************)"
                                        }
                                      )
                                  );
                        foreach (var prod in entities.producto)
                        {
                            var suma_gasto =
                                entities.submayor_gasto.Where(g => g.id_producto == prod.id &&
                                ((g.fecha.Year == filtroDesde.Value.Year && g.fecha.Month == filtroDesde.Value.Month && g.fecha.Day > filtroDesde.Value.Day)
                                || (g.fecha.Year == filtroDesde.Value.Year && g.fecha.Month > filtroDesde.Value.Month) || g.fecha.Year > filtroDesde.Value.Year
                                || (g.fecha.Year == filtroHasta.Value.Year && g.fecha.Month== filtroHasta.Value.Month && g.fecha.Day < filtroHasta.Value.Day)
                                || (g.fecha.Year == filtroHasta.Value.Year && g.fecha.Month < filtroHasta.Value.Month) || g.fecha.Year < filtroHasta.Value.Year
                                || (g.fecha.Year == filtroDesde.Value.Year && g.fecha.Month == filtroDesde.Value.Month && g.fecha.Day == filtroDesde.Value.Day)
                                || (g.fecha.Year == filtroHasta.Value.Year && g.fecha.Month == filtroHasta.Value.Month && g.fecha.Day == filtroHasta.Value.Day))).Sum(gas => gas.debito);
                            if(suma_gasto != null && suma_gasto != 0)
                            {
                                printableLV.Items.Add(
                                    new ListViewItem(new[]
                                                         {
                                                             prod.nombre,
                                                             Math.Round((decimal) suma_gasto, 2).ToString(
                                                                 CultureInfo.InvariantCulture)
                                                         }
                                        )
                                    );
                            }
                        }
                        //Mermas de activos fijos
                        printableLV.Items.Add(
                                 new ListViewItem(new[]
                                        {
                                            "Mermas de Activos Fijos",
                                             "(*****************)"
                                        }
                                     )
                                 );
                        foreach (var activoF in entities.activos_fijos)
                        {
                            var suma_gasto =
                                entities.submayor_gasto.Where(g => g.id_activos_fijos == activoF.id &&
                                 ((g.fecha.Year == filtroDesde.Value.Year && g.fecha.Month == filtroDesde.Value.Month && g.fecha.Day > filtroDesde.Value.Day)
                                || (g.fecha.Year == filtroDesde.Value.Year && g.fecha.Month > filtroDesde.Value.Month) || g.fecha.Year > filtroDesde.Value.Year
                                || (g.fecha.Year == filtroHasta.Value.Year && g.fecha.Month == filtroHasta.Value.Month && g.fecha.Day < filtroHasta.Value.Day)
                                || (g.fecha.Year == filtroHasta.Value.Year && g.fecha.Month < filtroHasta.Value.Month) || g.fecha.Year < filtroHasta.Value.Year
                                || (g.fecha.Year == filtroDesde.Value.Year && g.fecha.Month == filtroDesde.Value.Month && g.fecha.Day == filtroDesde.Value.Day)
                                || (g.fecha.Year == filtroHasta.Value.Year && g.fecha.Month == filtroHasta.Value.Month && g.fecha.Day == filtroHasta.Value.Day))).Sum(gas => gas.debito);
                            if (suma_gasto != null && suma_gasto != 0)
                            {
                                printableLV.Items.Add(
                                    new ListViewItem(new[]
                                                         {
                                                             activoF.nombre,
                                                             Math.Round((decimal) suma_gasto, 2).ToString(
                                                                 CultureInfo.InvariantCulture)
                                                         }
                                        )
                                    );
                            }
                        }
                        //Mermas de utiles y herramientas
                        printableLV.Items.Add(
                                 new ListViewItem(new[]
                                        {
                                            "Mermas de Útiles y Herramientas",
                                             "(*****************)"
                                        }
                                     )
                                 );
                        foreach (var utiles in entities.utiles_herramientas)
                        {
                            var suma_gasto =
                                entities.submayor_gasto.Where(g => g.id_utiles == utiles.id &&
                                 ((g.fecha.Year == filtroDesde.Value.Year && g.fecha.Month == filtroDesde.Value.Month && g.fecha.Day > filtroDesde.Value.Day)
                                || (g.fecha.Year == filtroDesde.Value.Year && g.fecha.Month > filtroDesde.Value.Month) || g.fecha.Year > filtroDesde.Value.Year
                                || (g.fecha.Year == filtroHasta.Value.Year && g.fecha.Month == filtroHasta.Value.Month && g.fecha.Day < filtroHasta.Value.Day)
                                || (g.fecha.Year == filtroHasta.Value.Year && g.fecha.Month < filtroHasta.Value.Month) || g.fecha.Year < filtroHasta.Value.Year
                                || (g.fecha.Year == filtroDesde.Value.Year && g.fecha.Month == filtroDesde.Value.Month && g.fecha.Day == filtroDesde.Value.Day)
                                || (g.fecha.Year == filtroHasta.Value.Year && g.fecha.Month == filtroHasta.Value.Month && g.fecha.Day == filtroHasta.Value.Day))).Sum(gas => gas.debito);
                            if (suma_gasto != null && suma_gasto != 0)
                            {
                                printableLV.Items.Add(
                                    new ListViewItem(new[]
                                                         {
                                                             utiles.nombre,
                                                             Math.Round((decimal) suma_gasto, 2).ToString(
                                                                 CultureInfo.InvariantCulture)
                                                         }
                                        )
                                    );
                            }
                        }
                    }
                        //Gastos por salarios
                    else if (tipo_gasto.Equals("Gastos por Salario"))
                    {
                        foreach (var user in entities.usuarios.Where(u => u.id != 1 && u.id != 16))
                        {
                            decimal aux = 0;
                            foreach (var g in entities.submayor_gasto.Where(g => g.id_usuario_salario == user.id))
                            {
                                if (g.fecha.Date < filtroDesde.Value.Date) continue;

                                if (g.fecha.Date > filtroHasta.Value.Date) continue;

                                if (g.debito != null)
                                    aux += g.debito.Value;
                            }
                            //decimal aux = Enumerable.Sum((from g in entities.submayor_gasto.Where(g => g.id_usuario_salario == user.id)
                            //                              where g.fecha.Date >= filtroDesde.Value.Date
                            //                              where g.fecha.Date <= filtroHasta.Value.Date
                            //                              where g.debito != null
                            //                              select g.debito.Value));
                            if (user.salario_extra_operaciones != null)
                                printableLV.Items.Add(
                                    new ListViewItem(new[]
                                                         {
                                                             user.nombre,
                                                             Math.Round(aux + user.salario_fijo + user.salario_extra_operaciones.Value, 2).ToString(
                                                                 CultureInfo.InvariantCulture)
                                                         }
                                        )
                                    );
                            else
                            {
                                printableLV.Items.Add(
                                   new ListViewItem(new[]
                                                         {
                                                             user.nombre,
                                                             Math.Round(aux + user.salario_fijo, 2).ToString(
                                                                 CultureInfo.InvariantCulture)
                                                         }
                                       )
                                   );
                            }
                        }
                    }
                        //Gastos por operaciones
                     else if (tipo_gasto.Equals("Gastos por operaciones"))
                     {
                         foreach(var op in entities.operaciones)
                         {
                             if(op.fecha.Date < filtroDesde.Value.Date) continue;

                             if(op.fecha.Date > filtroHasta.Value.Date) continue;

                             if (op.gasto != null)
                             {
                                 printableLV.Items.Add(
                                     new ListViewItem(new[]
                                                          {
                                                              op.fecha.ToString("dd/MM/yyyy"),
                                                              Math.Round(op.gasto.Value, 2).ToString(
                                                                  CultureInfo.InvariantCulture),
                                                              op.descripcion
                                                          }
                                         )
                                     );
                             }
                         }
                     }
                     else
                     {
                         foreach (var op_3ero in entities.operacion_3ero.Where(op => op.operaciones.cuentas_por_pagar.Count == 0))
                         {
                             if (op_3ero.operaciones.fecha.Date < filtroDesde.Value.Date) continue;
                             
                             if(op_3ero.operaciones.fecha.Date > filtroHasta.Value.Date) continue;

                             printableLV.Items.Add(
                                     new ListViewItem(new[]
                                                          {
                                                              op_3ero.operaciones.fecha.ToString("dd/MM/yyyy"),
                                                              Math.Round(op_3ero.valor_operacion_3ero.Value, 2).ToString(
                                                                  CultureInfo.InvariantCulture),
                                                              op_3ero.proveedor.nombre
                                                          }
                                         )
                                     );
                         }
                     }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tipo_gasto.Equals("Gastos por Merma"))
            {
                if(filtroDesde.Value.Date == filtroHasta.Value.Date)
                    this.printableLV.Title = "Gastos por Merma";
                else
                {
                    this.printableLV.Title = "Gastos por Merma desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta " +filtroHasta.Value.Date.ToString("dd/MM/yyyy");
                }
            }
            else if (tipo_gasto.Equals("Gastos por Salario"))
            {
                if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                    this.printableLV.Title = "Gastos por Salario";
                else
                {
                    this.printableLV.Title = "Gastos por Salario desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
                }
            }
            else if (tipo_gasto.Equals("Gastos por operaciones"))
            {
                if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                    this.printableLV.Title = "Gastos por operaciones";
                else
                {
                    this.printableLV.Title = "Gastos por operaciones desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
                }
            }
            else
            {
                if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                    this.printableLV.Title = "Gastos por operaciones con 3eros";
                else
                {
                    this.printableLV.Title = "Gastos por operaciones con 3eros desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
                }
            }
            this.printableLV.PrintPreview();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (tipo_gasto.Equals("Gastos por Merma"))
            {
                if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                    this.printableLV.Title = "Gastos por Merma";
                else
                {
                    this.printableLV.Title = "Gastos por Merma desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
                }
            }
            else if (tipo_gasto.Equals("Gastos por Salario"))
            {
                if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                    this.printableLV.Title = "Gastos por Salario";
                else
                {
                    this.printableLV.Title = "Gastos por Salario desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
                }
            }
            else if (tipo_gasto.Equals("Gastos por operaciones"))
            {
                if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                    this.printableLV.Title = "Gastos por operaciones";
                else
                {
                    this.printableLV.Title = "Gastos por operaciones desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
                }
            }
            else
            {
                if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                    this.printableLV.Title = "Gastos por operaciones con 3eros";
                else
                {
                    this.printableLV.Title = "Gastos por operaciones con 3eros desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
                }
            }
            this.printableLV.Print();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
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
    }
}
