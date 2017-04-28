using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class CerrarCuentaPorPagar : Form
    {
        private readonly usuarios _user;

        private readonly papiro_finalEntities _entities;

        private readonly List<int> _cuentasId;

        public CerrarCuentaPorPagar(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _entities = new papiro_finalEntities();
            _cuentasId = new List<int>();
        }

        private void ListarCuentasPorCobrar()
        {
            // Filtrar de acuerdo a las opciones de filtro seleccionadas.

            var objectQuery = new List<cuentas_por_pagar>();
            _cuentasId.Clear();
            foreach (var cuentasPorPagar in _entities.cuentas_por_pagar)
            {
                if (cuentasPorPagar.ejecutada == 1) continue;
                
                if (filtroDesde.Checked)
                    if (cuentasPorPagar.fecha.Date < filtroDesde.Value.Date) continue;

                if (filtroHasta.Checked)
                    if (cuentasPorPagar.fecha.Date > filtroHasta.Value.Date) continue;

                _cuentasId.Add(cuentasPorPagar.id);

                objectQuery.Add(cuentasPorPagar);
            }

            // Mostrarlos en el listview.

            printableLV.Items.Clear();

            var invariantCulture = CultureInfo.InvariantCulture;

            foreach (var item in objectQuery.Select(
                                        cuenta =>
                                            new ListViewItem(
                                                new[]
                                                    {
                                                        cuenta.descripcion, 
                                                        Math.Round(cuenta.monto, 2).ToString(invariantCulture),
                                                        cuenta.fecha.ToString("dd/MM/yyyy", 
                                                            CultureInfo.GetCultureInfo("es")),
                                                    })))
            {
                printableLV.Items.Add(item);
            }
        }

        private void GestionarCuentasCobrarLoad(object sender, EventArgs e)
        {
            ListarCuentasPorCobrar();
        }

        private void FiltrarClick(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date > filtroHasta.Value.Date)
            {
                MessageBox.Show("La fecha de inicio debe de ser menor que la fecha final",
                                "Error en la entrada de datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ListarCuentasPorCobrar();
        }

        private void Button1Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Button2Click(object sender, EventArgs e)
        {
            // Cerrar las cuentas seleccionadas.

            if (efectivoEncomboBox.SelectedIndex < 1)
            {
                MessageBox.Show(
                    "Debe especificar si se va a pagar con el efectivo en banco o con el efectivo en caja",
                    @"Cierre de cuentas por pagar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var checkedListViewItemCollection = printableLV.CheckedItems;

                foreach (ListViewItem item in checkedListViewItemCollection)
                {
                    var cuentaPorpagar =
                        (cuentas_por_pagar)
                        _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.cuentas_por_pagar", "id",
                                                               _cuentasId[item.Index]));
                    
                    cuentaPorpagar.ejecutada = 1;

                    // Obtener último balance para actualizar.
                    balance lastBalance = _entities.balance.ToList().Last();

                    decimal monto = cuentaPorpagar.monto;

                    switch (efectivoEncomboBox.SelectedIndex)
                    {
                        case 1:
                            if (lastBalance.efectivo_banco - monto < 0)
                            {
                                MessageBox.Show("No hay dinero suficiente en el banco para pagar",
                                                @"Cierre de cuentas por pagar", MessageBoxButtons.OK,
                                                MessageBoxIcon.Warning);
                                return;
                            }
                            _entities.AddTosubmayor_efectivo_banco(new submayor_efectivo_banco
                                                                       {
                                                                           fecha = DateTime.Now,
                                                                           id_usuario = _user.id,
                                                                           descripcion = "Se cierra cuenta por pagar.",
                                                                           saldo =
                                                                               lastBalance.efectivo_banco - monto,
                                                                           credito = monto
                                                                       });
                            lastBalance.efectivo_banco -= monto;
                            break;

                        case 2:
                            if (lastBalance.efectivo_caja - monto < 0)
                            {
                                MessageBox.Show("No hay dinero suficiente en la caja para pagar",
                                                @"Cierre de cuentas por pagar", MessageBoxButtons.OK,
                                                MessageBoxIcon.Warning);
                                return;
                            }
                            _entities.AddTosubmayor_efectivo_caja(new submayor_efectivo_caja
                                                                      {
                                                                          fecha = DateTime.Now,
                                                                          id_usuario = _user.id,
                                                                          descripcion = "Se cierra cuenta por pagar.",
                                                                          saldo = lastBalance.efectivo_caja - monto,
                                                                          credito = monto
                                                                      });
                            lastBalance.efectivo_caja -= monto;
                            break;
                    }

                    _entities.AddTosubmayor_cuentas_por_pagar(new submayor_cuentas_por_pagar
                                                                  {
                                                                      fecha = DateTime.Now,
                                                                      id_usuario = _user.id,
                                                                      descripcion = "Se cierra cuenta por pagar.",
                                                                      saldo = lastBalance.cuentas_por_pagar - monto,
                                                                      debito = monto
                                                                  });
                    lastBalance.cuentas_por_pagar -= monto;

                    //_entities.AddTosubmayor_gasto(new submayor_gasto
                    //                                  {
                    //                                      fecha = DateTime.Now,
                    //                                      id_usuario = _user.id,
                    //                                      descripcion = "Se cierra cuenta por pagar.",
                    //                                      saldo = lastBalance.gasto + monto,
                    //                                      debito = monto
                    //                                  });
                    //lastBalance.gasto += monto;

                    var bit = new bitacora
                                  {
                                      id_usuario = _user.id,
                                      nombre_usuario = _user.login_nombre,
                                      fecha = DateTime.Now,
                                      accion_realizada =
                                          "Cerrada la cuenta por pagar registrada el: " +
                                          cuentaPorpagar.fecha.ToString("dd/MM/yyyy") + " con un monto de " +
                                          cuentaPorpagar.monto
                                  };
                    _entities.AddTobitacora(bit);
                }

                _entities.SaveChanges();

                ListarCuentasPorCobrar();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Cierre de cuentas por pagar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableLV.Title = "Cuentas por pagar";
            else
            {
                this.printableLV.Title = "Cuentas por pagar desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") +
                                         " hasta el " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            }
            printableLV.PrintPreview();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableLV.Title = "Cuentas por pagar";
            else
            {
                this.printableLV.Title = "Cuentas por pagar desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") +
                                         " hasta el " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            }
            printableLV.Print();
        }
    }
}
