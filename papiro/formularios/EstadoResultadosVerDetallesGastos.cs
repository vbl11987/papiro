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
    public partial class EstadoResultadosVerDetallesGastos : Form
    {
        private readonly List<string> _nombreC;
        private DateTime desde;
        private DateTime hasta;

        public EstadoResultadosVerDetallesGastos(DateTime desde, DateTime hasta)
        {
            InitializeComponent();
            _nombreC = new List<string> { "" };
            this.desde = desde;
            this.hasta = hasta;
        }
        public void ActualizarReporte()
        {
            try 
            {
                using (var entities = new papiro_finalEntities())
                {
                    decimal total = 0;

                    //Esto es para obtener todos los diferentes tipos de gastos organizados por nombre de cuentas
                    var id_cuenta = "";
                    decimal gastos = 0;
                    var nombre_cuenta = "";
                    var count = entities.gasto.Count();
                    var x = 0;

                    foreach (var g in entities.gasto)
                    {
                        if (g.fecha.Date < filtroDesde.Value.Date)
                        {
                            x++;
                            continue;
                        }
                        if (g.fecha.Date > filtroHasta.Value.Date)
                        {
                            x++;
                            continue;
                        }
                        string[] elements = g.id.Split('\\');

                        if (id_cuenta.Equals(elements[0]))
                        {
                            gastos += g.gasto1;
                        }

                        if (id_cuenta == "")
                        {
                            nombre_cuenta = g.nombre_cuenta;
                            id_cuenta = elements[0];
                            gastos += g.gasto1;
                        }

                        if (x == count -1)
                        {
                            if (id_cuenta.Equals(elements[0]))
                            {
                                cbxdetalles.Items.Add(g.nombre_cuenta);
                                _nombreC.Add(g.nombre_cuenta);
                                printableLV.Items.Add(
                                    new ListViewItem(new[]
                                            {
                                                g.nombre_cuenta,
                                                    Math.Round(gastos, 2).ToString(CultureInfo.InvariantCulture)
                                            }
                                        )
                                    );
                                total += gastos;
                            }
                            else
                            {
                                //imprimo el penultimo elemento
                                cbxdetalles.Items.Add(nombre_cuenta);
                                _nombreC.Add(nombre_cuenta);
                                printableLV.Items.Add(
                                    new ListViewItem(new[]
                                            {
                                                nombre_cuenta,
                                                    Math.Round(gastos, 2).ToString(CultureInfo.InvariantCulture)
                                            }
                                        )
                                    );
                                total += gastos;
                                //imprimo el ultimo
                                cbxdetalles.Items.Add(g.nombre_cuenta);
                                _nombreC.Add(g.nombre_cuenta);
                                gastos = g.gasto1;
                                printableLV.Items.Add(
                                    new ListViewItem(new[]
                                            {
                                                g.nombre_cuenta,
                                                    Math.Round(gastos, 2).ToString(CultureInfo.InvariantCulture)
                                            }
                                        )
                                    );
                                total += gastos;
                            }
                        }
                        if (!id_cuenta.Equals(elements[0]) && x != count-1)
                        {
                           cbxdetalles.Items.Add(nombre_cuenta);
                           _nombreC.Add(nombre_cuenta);
                           printableLV.Items.Add(
                           new ListViewItem(new[]
                                    {
                                        nombre_cuenta,
                                         Math.Round(gastos, 2).ToString(CultureInfo.InvariantCulture)
                                    }
                               )
                           );
                           total += gastos;
                           id_cuenta = elements[0];
                           nombre_cuenta = g.nombre_cuenta;
                           gastos = 0;
                           gastos += g.gasto1;

                        }
                        x++;
                    }

                    //Obtengo todos los gastos de los submayores
                    decimal g2 = 0;
                    decimal g1 = 0;
                    decimal gasto_3ro = 0;
                    foreach (var sub_gastos in entities.submayor_gasto)
                    {
                        if (sub_gastos.fecha.Date < filtroDesde.Value.Date) continue;

                        if (sub_gastos.fecha.Date > filtroHasta.Value.Date) continue;

                        if (sub_gastos.descripcion.Equals("Merma."))
                        {
                            if (sub_gastos.debito != null) g2 += sub_gastos.debito.Value;
                        }
                        else if (sub_gastos.descripcion.Equals("Se registra gastos de usuarios.") || sub_gastos.descripcion.Equals("Se registra el plus del salario.") || sub_gastos.descripcion.Equals("Se registra el plus del salario."))
                        {
                            if (sub_gastos.debito != null) g1 += sub_gastos.debito.Value;
                        }
                        else if (sub_gastos.descripcion.Equals("Se realiza operación de terceros."))
                        {
                            if (sub_gastos.debito != null) gasto_3ro += sub_gastos.debito.Value;
                        }
                    }
                    if (filtroDesde.Value == DateTime.Now && filtroHasta.Value == DateTime.Now)
                    {
                        foreach (var user in entities.usuarios.Where(u => u.id != 1 && u.id != 16))
                        {
                            g1 += user.salario_fijo;
                            if (user.salario_extra_operaciones != null) g1 += user.salario_extra_operaciones.Value;
                        }
                    }
                    //decimal gasto_op = 0;
                    ////Obtengo todos los gastos de las operaciones
                    //foreach (var op in entities.operaciones)
                    //{
                    //    if (op.fecha.Date < filtroDesde.Value.Date) continue;

                    //    if (op.fecha.Date > filtroHasta.Value.Date) continue;

                    //    if (op.gasto != null) gasto_op += op.gasto.Value;   
                    //}
                    cbxdetalles.Items.Add("Gastos por Merma");
                    printableLV.Items.Add(
                           new ListViewItem(new[]
                                    {
                                        "Gastos por Merma",
                                         Math.Round(g2, 2).ToString(CultureInfo.InvariantCulture)
                                    }
                               )
                           );
                    total += g2;
                    cbxdetalles.Items.Add("Gastos por Salario");
                    printableLV.Items.Add(
                           new ListViewItem(new[]
                                    {
                                        "Gastos por Salario",
                                         Math.Round(g1, 2).ToString(CultureInfo.InvariantCulture)
                                    }
                               )
                           );
                    total += g1;
                    //cbxdetalles.Items.Add("Gastos por operaciones");
                    //printableLV.Items.Add(
                    //       new ListViewItem(new[]
                    //                {
                    //                    "Gastos por operaciones",
                    //                     Math.Round(gasto_op, 2).ToString(CultureInfo.InvariantCulture)
                    //                }
                    //           )
                    //       );
                    //total += gasto_op;
                    cbxdetalles.Items.Add("Gastos por operaciones con 3eros");
                    printableLV.Items.Add(
                           new ListViewItem(new[]
                                    {
                                        "Gastos por operaciones con 3eros",
                                         Math.Round(gasto_3ro, 2).ToString(CultureInfo.InvariantCulture)
                                    }
                               )
                           );
                    total += gasto_3ro;
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
                    @"Detalles de gastos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableLV.Title = "Detalles de gastos";
            else
            {
                this.printableLV.Title = "Detalles de gastos desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta el " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            }
            printableLV.PrintPreview();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableLV.Title = "Detalles de gastos";
            else
            {
                this.printableLV.Title = "Detalles de gastos desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta el " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            }
            printableLV.Print();
        }

        private void EstadoResultadosVerDetallesGastos_Load(object sender, EventArgs e)
        {
            filtroDesde.Value = desde;
            filtroHasta.Value = hasta;
            cbxdetalles.Items.Add("<Seleccione>");
            cbxdetalles.SelectedIndex = 0;

            ActualizarReporte();
        }

        private void filtrar_Click(object sender, EventArgs e)
        {
            if(filtroDesde.Value.Date > filtroHasta.Value.Date)
            {
                MessageBox.Show("La fecha de inicio debe ser menor que la fecha final", "Error en la entrada de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            cbxdetalles.Items.Clear();
            printableLV.Items.Clear();
            ActualizarReporte();
        }

        private void verdetalles_Click(object sender, EventArgs e)
        {
            if (cbxdetalles.SelectedIndex < 1 || cbxdetalles.SelectedIndex == 0)
            {
                MessageBox.Show("Debe seleccionar un gasto", "Ver detalles", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (filtroDesde.Value.Date > filtroHasta.Value.Date)
            {
                MessageBox.Show("La fecha de inicio debe ser menor que la fecha final", "Error en la entrada de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var index = cbxdetalles.SelectedIndex;

            if(index > _nombreC.Count -1)
            {
                DetallesOtrosGastos g = new DetallesOtrosGastos(this.desde, this.hasta,
                                                                cbxdetalles.SelectedItem.ToString());
                g.ShowDialog();
            }
            else
            {
                DetallesGastos dg = new DetallesGastos(this.filtroDesde.Value, this.filtroHasta.Value, _nombreC[cbxdetalles.SelectedIndex]);
                dg.ShowDialog();
            }
        }

        private void cbxdetalles_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
