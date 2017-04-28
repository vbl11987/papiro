using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class CerrarCuentaPorCobrar : Form
    {
        private readonly usuarios _user;

        private readonly papiro_finalEntities _entities;

        private readonly List<int> _cuentasId;

        private readonly List<int> _clientesId;

        private readonly List<int> _facturasId;

        private decimal _total;

        public CerrarCuentaPorCobrar(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _entities = new papiro_finalEntities();
            _cuentasId = new List<int>();
            _clientesId = new List<int> {-1};
            _facturasId = new List<int> { -1 };
            _total = 0;
        }

        private void ListarCuentasPorCobrar()
        {
            // Filtrar de acuerdo a las opciones de filtro seleccionadas.

            _total = 0;
            var objectQuery = new List<cuentas_por_cobrar>();
            _cuentasId.Clear();
            foreach (var cuentasPorCobrar in _entities.cuentas_por_cobrar)
            {
                if (cuentasPorCobrar.operaciones.pendiente == 0 && cuentasPorCobrar.ejecutada == 1) continue;
                
                if (filtroDesde.Checked)
                    if (cuentasPorCobrar.operaciones.fecha.Date < filtroDesde.Value.Date) continue;

                if (filtroHasta.Checked)
                    if (cuentasPorCobrar.operaciones.fecha.Date > filtroHasta.Value.Date) continue;

                if (clientcomboBox.SelectedIndex > 0 && 
                    _clientesId[clientcomboBox.SelectedIndex] != cuentasPorCobrar.id_cliente)
                    continue;

                if (facturacomboBox.SelectedIndex > 0 && 
                    _facturasId[facturacomboBox.SelectedIndex] != cuentasPorCobrar.operaciones.id_factura)
                        continue;

                _cuentasId.Add(cuentasPorCobrar.id);

                objectQuery.Add(cuentasPorCobrar);
                _total += cuentasPorCobrar.operaciones.monto;
            }

            // Mostrarlos en el listview.

            printableLV.Items.Clear();

            var invariantCulture = CultureInfo.InvariantCulture;

            // Acutalizat label del total
            lblTotal.Text = "";
            lblTotal.Text = "Total: " + Math.Round(_total, 2).ToString(invariantCulture);

            foreach (var item in objectQuery.Select(
                                        cuenta =>
                                            new ListViewItem(
                                                new[]
                                                    {
                                                        cuenta.cliente.nombre, 
                                                        cuenta.operaciones.fecha.ToString("dd/MM/yyyy", 
                                                            CultureInfo.GetCultureInfo("es")),
                                                        Math.Round(cuenta.operaciones.monto, 2).ToString(invariantCulture),
                                                        cuenta.operaciones.id_factura != null && 
                                                            cuenta.operaciones.id_factura != 0 
                                                                ? cuenta.operaciones.factura.dir_fich_factura 
                                                                : "" 
                                                    })))
            {
                printableLV.Items.Add(item);
            }
        }

        private void GestionarCuentasCobrarLoad(object sender, EventArgs e)
        {
            try
            {
                foreach (var cliente in _entities.cliente)
                {
                    if (cliente.nombre == "Cliente casual") continue;
                    _clientesId.Add(cliente.id);
                    clientcomboBox.Items.Add(cliente.nombre);
                }

                foreach(var fact in _entities.factura)
                {
                    _facturasId.Add(fact.id);
                    facturacomboBox.Items.Add(fact.dir_fich_factura);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Cierre de cuentas por cobrar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            ListarCuentasPorCobrar();
        }

        private void FiltrarClick(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date > filtroHasta.Value.Date)
            {
                MessageBox.Show("La fecha de inicio debe ser menor que la fecha final", "Error en la entrada de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    "Debe especificar si el cobro se va a guardar en el efectivo en banco o en el efectivo en caja",
                    @"Cierre de cuentas por cobrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var checkedListViewItemCollection = printableLV.CheckedItems;

                foreach (ListViewItem item in checkedListViewItemCollection)
                {
                    var cuentaPorCobrar =
                        (cuentas_por_cobrar)
                        _entities.GetObjectByKey(new EntityKey("papiro_finalEntities.cuentas_por_cobrar", "id",
                                                               _cuentasId[item.Index]));
                    
                    cuentaPorCobrar.operaciones.pendiente = 0;

                    // Obtener último balance para actualizar.
                    balance balance = _entities.balance.ToList().Last();

                    decimal monto = cuentaPorCobrar.operaciones.monto;

                    // Registrar ingreso.
                    //_entities.AddTosubmayor_ingreso(new submayor_ingreso
                    //                                    {
                    //                                        fecha = DateTime.Now,
                    //                                        id_usuario = _user.id,
                    //                                        descripcion =
                    //                                            "Se realiza cierre de cuenta por cobrar para el cliente " +
                    //                                            cuentaPorCobrar.cliente.nombre,
                    //                                        saldo = balance.ingreso + monto,
                    //                                        credito = monto
                    //                                    });
                    //balance.ingreso += monto;

                    switch (efectivoEncomboBox.SelectedIndex)
                    {
                        case 1:
                            _entities.AddTosubmayor_efectivo_banco(new submayor_efectivo_banco
                                                                       {
                                                                           fecha = DateTime.Now,
                                                                           id_usuario = _user.id,
                                                                           descripcion =
                                                                               "Se cierra cuenta por cobrar para el cliente " +
                                                                               cuentaPorCobrar.cliente.nombre,
                                                                           saldo =
                                                                               balance.efectivo_banco + monto,
                                                                           debito = monto
                                                                       });
                            balance.efectivo_banco += monto;
                            break;

                        case 2:
                            _entities.AddTosubmayor_efectivo_caja(new submayor_efectivo_caja
                                                                      {
                                                                          fecha = DateTime.Now,
                                                                          id_usuario = _user.id,
                                                                          descripcion =
                                                                              "Se cierra cuenta por cobrar para el cliente " +
                                                                              cuentaPorCobrar.cliente.nombre,
                                                                          saldo = balance.efectivo_caja + monto,
                                                                          debito = monto
                                                                      });
                            balance.efectivo_caja += monto;
                            break;
                    }

                    _entities.AddTosubmayor_cuentas_cobrar(new submayor_cuentas_cobrar
                                                               {
                                                                   fecha = DateTime.Now,
                                                                   id_usuario = _user.id,
                                                                   descripcion =
                                                                       "Se cierra cuenta por cobrar para el cliente " +
                                                                       cuentaPorCobrar.cliente.nombre,
                                                                   saldo = balance.cuentas_por_cobrar - monto,
                                                                   credito = monto
                                                               });
                    balance.cuentas_por_cobrar -= monto;

                    cuentaPorCobrar.ejecutada = 1;

                    var bit = new bitacora
                                  {
                                      id_usuario = _user.id,
                                      nombre_usuario = _user.login_nombre,
                                      fecha = DateTime.Now,
                                      accion_realizada =
                                          "Cerrada la cuenta por cobrar con Id: " + cuentaPorCobrar.id + " Cliente: " +
                                          cuentaPorCobrar.cliente.nombre,
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
                    @"Cierre de cuentas por cobrar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableLV.Title = "Cuentas por cobrar";
            else
            {
                this.printableLV.Title = "Cuentas por cobrar desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") +
                                         " hasta el " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            }
            printableLV.PrintPreview();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
                this.printableLV.Title = "Cuentas por cobrar";
            else
            {
                this.printableLV.Title = "Cuentas por cobrar desde el " + filtroDesde.Value.Date.ToString("dd/MM/yyyy") +
                                         " hasta el " + filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            }
            printableLV.Print();
        }
    }
}
