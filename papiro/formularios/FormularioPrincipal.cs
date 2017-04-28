using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using Application = System.Windows.Forms.Application;
using papiro.Reportes;

namespace papiro.formularios
{
    public partial class FormularioPrincipal : Form
    {
        private readonly usuarios _user;
        private readonly LoginForm _loginForm;
        private readonly CargandoForm _cargandoForm;
        private bool _cambiarUsuario;
        private readonly List<int> _avisosId;
        private readonly papiro_finalEntities _entities;

        public FormularioPrincipal(usuarios user, LoginForm loginForm)
        {
            InitializeComponent();
            _user = user;
            _loginForm = loginForm;
            _cargandoForm = new CargandoForm();
            _cambiarUsuario = false;
            _avisosId = new List<int>();
            _entities = new papiro_finalEntities();
        }

        private void ListarAvisos()
        {
            _avisosId.Clear();
            avisosdataGridView.Rows.Clear();

            foreach (var aviso in from avi in _entities.avisos
                                  where avi.id_usuario == null || avi.id_usuario == _user.id
                                  orderby avi.fecha descending
                                  select avi)
            {
                _avisosId.Add(aviso.id);
                avisosdataGridView.Rows.Add(new[]
                                                {
                                                    aviso.fecha.ToString("dd/MM/yyyy h:mm:ss tt"), aviso.descripcion,
                                                    aviso.id_contrato == null ? "Eliminar" : "Facturar y Eliminar"
                                                });
            }
        }

        private void FormularioPrincipalLoad(object sender, EventArgs e)
        {
            if (_user == null)
            {
                Application.Exit();
                return;
            }

            // Oculto el formulario principal.
            Hide();

            _cargandoForm.Reset();
            _cargandoForm.Show();

            user_status.Text = @"Usuario: " + _user.login_nombre;
            fecha_status.Text = DateTime.Now.ToString("dd/MM/yyyy");

            try
            {
                //if (_entities.balance.Count() == 1)
                //{
                //    _entities.AddTobalance(new balance
                //                               {
                //                                   efectivo_caja = 0,
                //                                   efectivo_banco = 0,
                //                                   cuentas_por_cobrar = 0,
                //                                   inventario = 0,
                //                                   activos_fijo_tangible = 0,
                //                                   gasto = 0,
                //                                   costo = 0,
                //                                   cuentas_por_pagar = 0,
                //                                   papiro_capital = 0,
                //                                   ingreso = 0,
                //                                   fecha = DateTime.Now
                //                               });
                //    _entities.SaveChanges();
                //}

                _cargandoForm.SetValue(5);

                _cargandoForm.SetMessage("Recalculando prioridades de clientes...");

                //decimal ingresoTotal = _entities.balance.ToList().Last().ingreso;

                foreach (var cliente in _entities.cliente)
                {
                    // Si es casual continuar.
                    if (cliente.nombre == "Cliente casual") continue;

                    var fecha = (from prio in _entities.prioridad
                                 where prio.id_cliente == cliente.id
                                 orderby prio.fecha descending
                                 select prio.fecha).FirstOrDefault();

                    bool calcular = false;
                    if (fecha == null) // No tiene prioridad, calcular
                    {
                        calcular = true;
                    }
                    else
                    {
                        if (fecha.Value.Year == DateTime.Now.Year)
                        {
                            if (fecha.Value.Month < DateTime.Now.Month)
                                calcular = true;
                        }
                        else
                            calcular = true;
                    }

                    if (!calcular) continue;

                    // Porciento de cantidad de operaciones y cantidad de ingresos.
                    //var porcientoIngreso = ingresoTotal == 0
                    //                           ? 0
                    //                           : _entities.operaciones.Where(
                    //                               op => op.id_cliente == cliente.id && op.pendiente == 0).Sum(
                    //                                   ingreso => ingreso.monto)*100/ingresoTotal;

                    //var porcientoCantOperac = _entities.operaciones.Count() == 0
                    //          ? 0
                    //          : cliente.operaciones.Where(op => op.pendiente == 0).Count() * 100 /
                    //            _entities.operaciones.Count();


                    var cant_operaciones =
                        _entities.operaciones.Where(op => op.id_cliente == cliente.id && op.pendiente == 0).Count();
                    if (cant_operaciones == 0) continue;
                    var ingresocliente = _entities.operaciones.Where(
                        op => op.id_cliente == cliente.id && op.pendiente == 0).Sum(
                            ingreso => ingreso.monto);
                    var ingresoTotal = _entities.operaciones.Sum(op => op.monto);


                    var porcientoIngreso = (ingresocliente*100)/ingresoTotal;
                    var porcientoCantOperac = (cant_operaciones*100)/
                                              _entities.operaciones.Where(op => op.pendiente == 0).Count();

                    // Nuevo valor de prioridad para el cliente.
                    cliente.prioridad.Add(new prioridad
                                              {
                                                  valor_por_ingreso = (int) porcientoIngreso,
                                                  valor_cant_operaciones = porcientoCantOperac,
                                                  valor_ambos = (int) ((porcientoIngreso + porcientoCantOperac)/2),
                                                  fecha = DateTime.Now
                                              });
                }

                _cargandoForm.SetValue(25);

                _cargandoForm.SetMessage("Verificando períodos de tiempo de los contratos...");

                foreach (var contrato in _entities.contrato)
                {
                    if (contrato.terminado == 1) continue;

                    string aviso = "";

                    if (contrato.fecha_fin.Date < DateTime.Now.Date)
                    {
                        aviso = "Para el contrato '" + contrato.dir_fich_contrato + "' firmado por el cliente: " +
                                contrato.cliente.nombre + " , el " +
                                contrato.fecha_inic.ToString("dd/MM/yyyy") +
                                " ha finalizado su tiempo de validez.";

                        foreach (var user in _entities.usuarios.Where(u => u.rol.Any(r => r.id == 1 || r.id == 5)))
                        {
                            user.avisos.Add(new avisos
                                                {
                                                    descripcion = aviso,
                                                    fecha = DateTime.Now,
                                                    id_contrato = contrato.id
                                                });
                        }
                        contrato.terminado = 1;
                    }
                    else
                    {
                        int days = (contrato.fecha_fin.Date - DateTime.Now.Date).Days;
                        if (days <= 5)
                        {
                            aviso = "Al contrato '" + contrato.dir_fich_contrato + "' firmado por el cliente: " +
                                    contrato.cliente.nombre + " , el " +
                                    contrato.fecha_inic.ToString("dd/MM/yyyy") +
                                    " le quedan " + days + " días para finalizar su tiempo de validez.";

                            foreach (var user in _entities.usuarios.Where(u => u.rol.Any(r => r.id == 1 || r.id == 5)))
                            {
                                user.avisos.Add(new avisos
                                                    {
                                                        descripcion = aviso,
                                                        fecha = DateTime.Now,
                                                    });
                            }
                        }
                    }
                }

                _cargandoForm.SetValue(55);

                _cargandoForm.SetMessage("Chequeando cuentas por cobrar...");

                foreach (var cuentasPorCobrar in _entities.cuentas_por_cobrar)
                {
                    if (cuentasPorCobrar.operaciones.pendiente == 0 && cuentasPorCobrar.ejecutada == 1) continue;

                    string aviso = "";

                    int daysCobrar = (cuentasPorCobrar.operaciones.fecha.Date - DateTime.Now.Date).Days;

                    if (daysCobrar >= 30)
                        aviso = "La cuenta por cobrar generada a partir de la operación realizada por el cliente " +
                                cuentasPorCobrar.cliente.nombre + " el " +
                                cuentasPorCobrar.operaciones.fecha.ToString("dd/MM/yyyy") +
                                ", lleva " + daysCobrar + " días sin cobrarse.";

                    if (aviso == "") continue;

                    foreach (var user in _entities.usuarios.Where(u => u.rol.Any(r => r.id == 1 || r.id == 5)))
                    {
                        user.avisos.Add(new avisos
                                            {
                                                descripcion = aviso,
                                                fecha = DateTime.Now,
                                            });
                    }
                }

                _cargandoForm.SetValue(65);

                _cargandoForm.SetMessage("Chequeando cuentas por pagar...");

                foreach (var cuentaPorPagar in _entities.cuentas_por_pagar)
                {
                    if (cuentaPorPagar.ejecutada == 1) continue;

                    string aviso = "";

                    int daysPagar = (cuentaPorPagar.fecha.Date - DateTime.Now.Date).Days;

                    if (daysPagar >= 7)
                        aviso = "La cuenta por pagar con monto " + cuentaPorPagar.monto + " creada el " +
                                cuentaPorPagar.fecha.ToString("dd/MM/yyyy") +
                                " lleva " + daysPagar + " días sin ser pagada.";

                    if (aviso == "") continue;

                    foreach (var user in _entities.usuarios.Where(u => u.rol.Any(r => r.id == 1 || r.id == 5)))
                    {
                        user.avisos.Add(new avisos
                                            {
                                                descripcion = aviso,
                                                fecha = DateTime.Now,
                                            });
                    }
                }

                //Aqui comienzo ha realizar las operaciones para realizar los cierres contables automaticos.
                _cargandoForm.SetValue(70);

                _cargandoForm.SetMessage("Cargando datos del último cierre...");

                //cargo la ultima fila del balance
                balance last_balance = _entities.balance.ToList().Last();
                if(last_balance.fecha != null)
                {
                    if (last_balance.fecha.Value.Date > DateTime.Now.Date)
                    {
                        almacénToolStripMenuItem.Enabled = false;
                        gestiónDeUsuariosToolStripMenuItem.Enabled = false;
                        gestiónDeClientesToolStripMenuItem.Enabled = false;
                        gestiónDeUnidadesDeMedidaToolStripMenuItem.Enabled = false;
                        impresiónToolStripMenuItem.Enabled = false;
                        gestiónDeContratosToolStripMenuItem.Enabled = false;
                        gestiónDeTiposDeOperaciónToolStripMenuItem.Enabled = false;
                        gestiónDeTiposDeProductoToolStripMenuItem.Enabled = false;
                        diseñoToolStripMenuItem.Enabled = false;
                        pendientesToolStripMenuItem.Enabled = false;
                        finanzasToolStripMenuItem.Enabled = false;
                        contabilidadToolStripMenuItem.Enabled = false;
                        balanceToolStripMenuItem.Enabled = false;
                        transfererirAlBancoToolStripMenuItem.Enabled = false;
                        transferirALaCajaToolStripMenuItem.Enabled = false;
                        gestiónDeTiposDeOperaciónDe3erosToolStripMenuItem.Enabled = false;
                        gestiónDeProveedoresToolStripMenuItem.Enabled = false;
                        reportesToolStripMenuItem.Enabled = false;
                        eliminarOperaciónToolStripMenuItem.Enabled = false;
                        gestionarToolStripMenuItem.Enabled = false;
                        gestiónDeTiposDeÚtilesYHerramientasToolStripMenuItem.Enabled = false;
                        reporteDeCobroAnticipadoToolStripMenuItem.Enabled = false;
                        cobrosAnticipadosToolStripMenuItem.Enabled = false;
                        operaciónDeCobroAnticipadoToolStripMenuItem.Enabled = false;

                        MessageBox.Show(
                            "La fecha de la máquina es menor que la última fecha guardada en la Base de Datos, ajuste la fecha",
                            "Error de fechas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    var aux = _entities.balance.ToList();
                    if(aux.Count == 1)
                    {
                        aux.ElementAt(0).fecha = DateTime.Now;
                    }
                    else
                    {
                        balance aux1 = aux.ElementAt(aux.Count - 2);
                    if (aux1.fecha != null)
                    {
                        if (aux1.fecha.Value.Date > DateTime.Now.Date)
                        {
                            almacénToolStripMenuItem.Enabled = false;
                            gestiónDeUsuariosToolStripMenuItem.Enabled = false;
                            gestiónDeClientesToolStripMenuItem.Enabled = false;
                            gestiónDeUnidadesDeMedidaToolStripMenuItem.Enabled = false;
                            impresiónToolStripMenuItem.Enabled = false;
                            gestiónDeContratosToolStripMenuItem.Enabled = false;
                            gestiónDeTiposDeOperaciónToolStripMenuItem.Enabled = false;
                            gestiónDeTiposDeProductoToolStripMenuItem.Enabled = false;
                            diseñoToolStripMenuItem.Enabled = false;
                            pendientesToolStripMenuItem.Enabled = false;
                            finanzasToolStripMenuItem.Enabled = false;
                            contabilidadToolStripMenuItem.Enabled = false;
                            balanceToolStripMenuItem.Enabled = false;
                            transfererirAlBancoToolStripMenuItem.Enabled = false;
                            transferirALaCajaToolStripMenuItem.Enabled = false;
                            gestiónDeTiposDeOperaciónDe3erosToolStripMenuItem.Enabled = false;
                            gestiónDeProveedoresToolStripMenuItem.Enabled = false;
                            reportesToolStripMenuItem.Enabled = false;
                            eliminarOperaciónToolStripMenuItem.Enabled = false;
                            gestionarToolStripMenuItem.Enabled = false;
                            gestiónDeTiposDeÚtilesYHerramientasToolStripMenuItem.Enabled = false;
                            reporteDeCobroAnticipadoToolStripMenuItem.Enabled = false;
                            cobrosAnticipadosToolStripMenuItem.Enabled = false;
                            operaciónDeCobroAnticipadoToolStripMenuItem.Enabled = false;

                            MessageBox.Show(
                                "La fecha de la máquina es menor que la última fecha guardada en la Base de Datos, ajuste la fecha",
                                "Error de fechas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    }
                    
                }
                if(_entities.balance.Count() != 1)
                {
                    if(last_balance.fecha == null)
                    {
                        var aux = _entities.balance.ToList();
                        balance aux1 = aux.ElementAt(aux.Count - 2);
                        if(aux1.fecha != null)
                        {
                            if(aux1.fecha.Value.Year == DateTime.Now.Year && aux1.fecha.Value.Month == DateTime.Now.Month && aux1.fecha.Value.Day == DateTime.Now.Day)
                            {
                                _entities.SaveChanges(); 
                            }
                            else
                            {
                                last_balance.fecha = DateTime.Now;
                                _entities.SaveChanges();
                            }
                            
                        }
                        
                    }
                    else if (last_balance.fecha.Value.Year == DateTime.Now.Year &&
                             last_balance.fecha.Value.Month == DateTime.Now.Month &&
                             last_balance.fecha.Value.Day < DateTime.Now.Day
                             ||
                             last_balance.fecha.Value.Year == DateTime.Now.Year &&
                             (last_balance.fecha.Value.Month + 1) == DateTime.Now.Month
                             || last_balance.fecha.Value.Year + 1 == DateTime.Now.Year)
                    {


                        //creo una nueva entidad para el balance
                        balance nuevo_balance = new balance();

                        //se calcula la utilidad y se le suma a la cuenta de papiro
                        //last_balance.papiro_capital += last_balance.ingreso - last_balance.costo - last_balance.gasto;
                        //var num = last_balance.ingreso - last_balance.costo - last_balance.gasto;

                        ////se vacian las cuentas de ingresos, gastos y costos
                        //last_balance.ingreso = 0;
                        //last_balance.gasto = 0;
                        //last_balance.costo = 0;

                        //Le paso todos los valores al nuevo balance
                        nuevo_balance.ingreso = last_balance.ingreso;
                        nuevo_balance.costo = last_balance.costo;
                        nuevo_balance.gasto = last_balance.gasto;
                        nuevo_balance.fecha = DateTime.Now;
                        nuevo_balance.activos_fijo_tangible = last_balance.activos_fijo_tangible;
                        nuevo_balance.efectivo_caja = last_balance.efectivo_caja;
                        nuevo_balance.efectivo_banco = last_balance.efectivo_banco;
                        nuevo_balance.cuentas_por_cobrar = last_balance.cuentas_por_cobrar;
                        nuevo_balance.cuentas_por_pagar = last_balance.cuentas_por_pagar;
                        nuevo_balance.inventario = last_balance.inventario;
                        nuevo_balance.utiles_herramientas = last_balance.utiles_herramientas;
                        nuevo_balance.activos_fijo_tangible = last_balance.activos_fijo_tangible;
                        nuevo_balance.nominas_pagar = last_balance.nominas_pagar;
                        nuevo_balance.papiro_capital = last_balance.papiro_capital;
                        nuevo_balance.cobro_anticipado = last_balance.cobro_anticipado;
                        
                        //gastos los salarioos de los usuarios
                        foreach (var usuario in _entities.usuarios.Where(u => u.id != 1 && u.id != 16))
                        {
                            decimal total_user = 0;
                            total_user += usuario.salario_fijo;
                            if (usuario.salario_extra_operaciones != null)
                                total_user += usuario.salario_extra_operaciones.Value;
                            _entities.AddTosubmayor_gasto(new submayor_gasto
                                                              {
                                                                  fecha = DateTime.Now,
                                                                  id_usuario = _user.id,
                                                                  descripcion = "Se registra gastos de usuarios.",
                                                                  saldo = nuevo_balance.gasto + total_user,
                                                                  debito = total_user,
                                                                  id_usuario_salario = usuario.id
                                                              });
                            _entities.AddTosubmayor_nomina(new submayor_nomina
                                                               {
                                                                   fecha =  DateTime.Now,
                                                                   id_usuario = _user.id,
                                                                   descripcion = "Se registra gastos de usuarios.",
                                                                   saldo = nuevo_balance.nominas_pagar + total_user,
                                                                   credito = total_user,
                                                                   ultimo_pago = usuario.id
                                                               });
                            usuario.salario_extra_operaciones = 0;
                            usuario.a_pagar += total_user;
                            nuevo_balance.gasto += total_user;
                            nuevo_balance.nominas_pagar += total_user;
                        }
                        _entities.SaveChanges();
                        
                        _entities.AddTobalance(nuevo_balance);

                    }
                 }


                _cargandoForm.SetValue(90);

                _cargandoForm.SetMessage("Cargando avisos...");
                ListarAvisos();

                _cargandoForm.SetValue(95);

                _cargandoForm.SetMessage("Verificando permisos...");

                /* Roles
                 * 1 Administrador
                 * 2 Económico
                 * 3 Impresion
                 * 4 Diseño
                 * 5 Comercial
                 * 6 Operador
                 */
                
                almacénToolStripMenuItem.Enabled = false;
                gestiónDeUsuariosToolStripMenuItem.Enabled = false;
                gestiónDeClientesToolStripMenuItem.Enabled = false;
                gestiónDeUnidadesDeMedidaToolStripMenuItem.Enabled = false;
                impresiónToolStripMenuItem.Enabled = false;
                gestiónDeContratosToolStripMenuItem.Enabled = false;
                gestiónDeTiposDeOperaciónToolStripMenuItem.Enabled = false;
                gestiónDeTiposDeProductoToolStripMenuItem.Enabled = false;
                diseñoToolStripMenuItem.Enabled = false;
                pendientesToolStripMenuItem.Enabled = false;
                finanzasToolStripMenuItem.Enabled = false;
                contabilidadToolStripMenuItem.Enabled = false;
                balanceToolStripMenuItem.Enabled = false;
                transfererirAlBancoToolStripMenuItem.Enabled = false;
                transferirALaCajaToolStripMenuItem.Enabled = false;
                gestiónDeTiposDeOperaciónDe3erosToolStripMenuItem.Enabled = false;
                gestiónDeProveedoresToolStripMenuItem.Enabled = false;
                reportesToolStripMenuItem.Enabled = false;
                eliminarOperaciónToolStripMenuItem.Enabled = false;
                gestionarToolStripMenuItem.Enabled = false;
                gestiónDeTiposDeÚtilesYHerramientasToolStripMenuItem.Enabled = false;
                reporteDeCobroAnticipadoToolStripMenuItem.Enabled = false;
                cobrosAnticipadosToolStripMenuItem.Enabled = false;
                operaciónDeCobroAnticipadoToolStripMenuItem.Enabled = false;


                foreach (var rol in _user.rol)
                {
                    int rolId = rol.id;
                    if (rolId == 1 || rolId == 2)
                    {
                        almacénToolStripMenuItem.Enabled = true;
                        gestiónDeTiposDeProductoToolStripMenuItem.Enabled = true;
                        finanzasToolStripMenuItem.Enabled = true;
                        contabilidadToolStripMenuItem.Enabled = true;
                        transfererirAlBancoToolStripMenuItem.Enabled = true;
                        transferirALaCajaToolStripMenuItem.Enabled = true;
                        gestiónDeProveedoresToolStripMenuItem.Enabled = true;
                        gestiónDeContratosToolStripMenuItem.Enabled = true;
                        gestiónDeClientesToolStripMenuItem.Enabled = true;
                        gestiónDeUnidadesDeMedidaToolStripMenuItem.Enabled = true;
                        gestiónDeTiposDeOperaciónToolStripMenuItem.Enabled = true;
                        gestiónDeTiposDeOperaciónDe3erosToolStripMenuItem.Enabled = true;
                        balanceToolStripMenuItem.Enabled = true;
                        reportesToolStripMenuItem.Enabled = true;
                        gestionarToolStripMenuItem.Enabled = true;
                        gestiónDeTiposDeÚtilesYHerramientasToolStripMenuItem.Enabled = true;
                        eliminarOperaciónToolStripMenuItem.Enabled = true;
                        reporteDeCobroAnticipadoToolStripMenuItem.Enabled = true;
                        cobrosAnticipadosToolStripMenuItem.Enabled = true;


                    }
                    if (rolId == 1)
                    {
                        gestiónDeUsuariosToolStripMenuItem.Enabled = true;
                        gestiónDeClientesToolStripMenuItem.Enabled = true;
                        gestiónDeUnidadesDeMedidaToolStripMenuItem.Enabled = true;
                        gestiónDeTiposDeOperaciónToolStripMenuItem.Enabled = true;
                        gestiónDeTiposDeOperaciónDe3erosToolStripMenuItem.Enabled = true;
                        balanceToolStripMenuItem.Enabled = true;
                        eliminarOperaciónToolStripMenuItem.Enabled = true;
                        //if (_entities.balance.ToList().Count > 1)
                        //    balanceToolStripMenuItem.Visible = false;
                        //else
                        //    balanceToolStripMenuItem.Enabled = true;
                    }
                    if (rolId == 1 || rolId == 3)
                    {
                        impresiónToolStripMenuItem.Enabled = true;
                        pendientesToolStripMenuItem.Enabled = true;
                        eliminarOperaciónToolStripMenuItem.Enabled = true;
                        operaciónDeCobroAnticipadoToolStripMenuItem.Enabled = true;
                    }
                    if (rolId == 1 || rolId == 4)
                        diseñoToolStripMenuItem.Enabled = true;
                    if (rolId == 1 || rolId == 5)
                        gestiónDeContratosToolStripMenuItem.Enabled = true;
                }
                
                _cargandoForm.SetValue(100);

                _entities.SaveChanges();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Inicializando la aplicación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _cargandoForm.Hide();

            Show();
        }

        private void FormularioPrincipalFormClosed(object sender, FormClosedEventArgs e)
        {
            if (!_cambiarUsuario) _loginForm.Close();
        }

        private void BitácoraDelSistemaToolStripMenuItemClick(object sender, EventArgs e)
        {
            var bitForm = new ListarBitacora(_user);
            bitForm.ShowDialog();
        }

        private void CambiarUserButtonClick(object sender, EventArgs e)
        {
            _cambiarUsuario = true;
            _loginForm.Show();
            Close();
        }

        private void EntradaToolStripMenuItemClick(object sender, EventArgs e)
        {
            
        }

        private void CostoToolStripMenuItemClick(object sender, EventArgs e)
        {
            var costoForm = new InventarioCostoForm(_user);
            costoForm.ShowDialog();
        }

        private void GastoToolStripMenuItemClick(object sender, EventArgs e)
        {
            
        }

        private void GestiónDeUsuariosToolStripMenuItemClick(object sender, EventArgs e)
        {
            var usersForm = new GestionarUsuarios(_user);
            usersForm.ShowDialog();
        }

        private void GestiónDeClientesToolStripMenuItemClick(object sender, EventArgs e)
        {
            var clientesForm = new GestionarClientes(_user);
            clientesForm.ShowDialog();
        }

        private void GestiónDeUnidadesDeMedidaToolStripMenuItemClick(object sender, EventArgs e)
        {
            var uniMedForm = new GestionarUnidadMedida(_user);
            uniMedForm.ShowDialog();
        }

        private void ImpresiónToolStripMenuItemClick(object sender, EventArgs e)
        {
            var opImp = new OperacionImpresion(_user);
            opImp.ShowDialog();
        }

        private void GestiónDeContratosToolStripMenuItemClick(object sender, EventArgs e)
        {
            var contractForm = new GestionarContratos(_user);
            contractForm.ShowDialog();
        }

        private void GestiónDeTiposDeOperaciónToolStripMenuItemClick(object sender, EventArgs e)
        {
            var gestTipOp = new GestionarTipoOperacion(_user);
            gestTipOp.ShowDialog();
        }

        private void GestiónDeTiposDeProductoToolStripMenuItemClick(object sender, EventArgs e)
        {
            var gestTipProd = new GestionarTipoProducto(_user);
            gestTipProd.ShowDialog();
        }

        private void DiseñoToolStripMenuItemClick(object sender, EventArgs e)
        {
            var operacionDisenno = new OperacionDisenno(_user);
            operacionDisenno.ShowDialog();
        }

        private void PendientesToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Operaciones pendientes.
            var opPend = new OperacionesPendientes(_user);
            opPend.ShowDialog();
        }

        private void CobrosToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Cuentas por cobrar.

            var cobros = new CerrarCuentaPorCobrar(_user);
            cobros.ShowDialog();
        }

        private void PagosToolStripMenuItemClick(object sender, EventArgs e)
        {
            var cuentasPorPagar = new CerrarCuentaPorPagar(_user);
            cuentasPorPagar.ShowDialog();
        }

        private void NóminaToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Nómina.

            var nominaForm = new NominaForm(_user);
            nominaForm.ShowDialog();
        }

        private void GastosToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Contabilidad - Gastos 

            var gastos = new Gastos(_user);
            gastos.ShowDialog();
        }

        private void BalanceToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Gestionar actualizar los datos de la tupla que representa el balance.

            var balance = new Balance(_user);
            balance.ShowDialog();
        }

        private void TransfererirAlBancoToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Transferir cantidad de dinero de la caja al banco.

            var tranferirbanco = new TransferirBanco(_user);
            tranferirbanco.ShowDialog();
        }

        private void ExtracciónDeLaCajaToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Extraer dinero de la caja o el banco.

            var extraccion = new Extraccion(_user);
            extraccion.ShowDialog();
        }

        private void GestiónDeTiposDeOperaciónDe3ErosToolStripMenuItemClick(object sender, EventArgs e)
        {
            var tipOp3Eros = new GestionarTipoOperacion3Ero(_user);
            tipOp3Eros.ShowDialog();
        }

        private void TransferirALaCajaToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Transferencia banco caja.

            var transferCaja = new TransferirCaja(_user);
            transferCaja.ShowDialog();
        }

        private void CostoPorOperacionesToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Reporte Costo x Operaciones

            var costoOp = new CostoPorOperaciones();
            costoOp.ShowDialog();
        }

        private void AvisosdataGridViewCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1) return;

            if (avisosdataGridView.Columns[e.ColumnIndex].Name == "CerrarColumn")
            {
                Microsoft.Office.Interop.Excel.Application application = null;
                _Workbook book = null;
                object missing = Type.Missing;
                
                try
                {
                    using (var entities = new papiro_finalEntities())
                    {
                        var aviso =
                            (avisos)
                            entities.GetObjectByKey(new EntityKey("papiro_finalEntities.avisos", "id",
                                                                  _avisosId[e.RowIndex]));
                        
                        if (aviso.id_contrato != null && aviso.id_contrato != 0)
                        {
                            // Generar factura

                            string path = Assembly.GetExecutingAssembly().CodeBase.Remove(0, 8);
                            path = path.Substring(0, path.LastIndexOf('/') + 1).Replace('/', '\\');

                            if (!File.Exists(path + "PLANTILLA_FACTURA.xlsx"))
                                throw new Exception(
                                    "No existe la plantilla a partir de la cual se crea la factura.");

                            application = new Microsoft.Office.Interop.Excel.Application();

                            book = application.Workbooks.Open(path + "PLANTILLA_FACTURA.xlsx", missing, missing,
                                                              missing, missing, missing, missing, missing,
                                                              missing, missing, missing, missing, missing, missing,
                                                              missing);
                            _Worksheet sheet = book.Worksheets[1];

                            // llenar el sheet.
                            
                            var contract =
                                (contrato)
                                entities.GetObjectByKey(new EntityKey("papiro_finalEntities.contrato", "id",
                                                                      aviso.id_contrato));
                            contract.terminado = 1;

                            int max = _entities.factura.Count() == 0
                              ? 0
                              : Enumerable.Max(_entities.factura,
                                               f => Convert.ToInt32(f.dir_fich_factura.Substring(0, 3)));
                            string num = (max + 1).ToString(CultureInfo.InvariantCulture);
                            if (num.Length == 1)
                                num = "00" + num;
                            else if (num.Length == 2)
                                num = "0" + num;
                            string id = num + DateTime.Today.ToString("MMyy", CultureInfo.InvariantCulture);

                            sheet.Cells[7, "C"] = id;
                            sheet.Cells[8, "C"] = contract.cliente.nombre ?? "";
                            sheet.Cells[9, "C"] = DateTime.Now.ToString("dd/MM/yyyy");

                            // Tipo de contrato.
                            switch (contract.id_tipo_contrato)
                            {
                                case 2: // Puntual
                                    sheet.Cells[11, "D"] = "X";
                                    break;
                                case 1: // Lineal
                                    sheet.Cells[11, "F"] = "X";
                                    break;
                            }

                            // Tipo de pago.
                            switch (contract.id_tipo_pago)
                            {
                                case 1: sheet.Cells[12, "D"] = "X";  // puntual
                                    break;
                                case 3: sheet.Cells[13, "D"] = "X";  // anticipado
                                    break;
                                case 2: sheet.Cells[14, "D"] = "X";  // a credito
                                    break;
                            }

                            // Operaciones
                            int pos = 17;
                            foreach (var op in contract.operaciones)
                            {
                                if (op.facturada) continue;
                                
                                // Obtener el producto por su tipo.
                                if (op.id_tipo_producto == null || op.id_tipo_producto == 0)
                                    continue;
                                int idTipProd = op.id_tipo_producto.Value;
                                producto producto =
                                    _entities.producto.Where(prod => prod.id_tipo_producto == idTipProd).SingleOrDefault
                                        ();
                                
                                sheet.Cells[pos, "B"] = op.tipo_operacion.valor;
                                sheet.Cells[pos, "C"] = op.cantidad;
                                sheet.Cells[pos, "D"] = producto.unidad_medida.siglas;
                                sheet.Cells[pos, "E"] = op.valor_tipo_operacion;
                                pos++;
                            }

                            if (!Directory.Exists(path + "Facturas"))
                                Directory.CreateDirectory(path + "Facturas");

                            book.SaveAs(path + "Facturas" + "\\" + id);

                            var fact = new factura
                                           {
                                               id_contrato = aviso.id_contrato.Value,
                                               fecha = DateTime.Now,
                                               dir_fich_factura = id
                                           };

                            entities.AddTofactura(fact);

                            entities.SaveChanges();

                            foreach (var op in contract.operaciones)
                            {
                                op.id_factura = fact.id;
                                op.facturada = true;
                            }
                        }

                        entities.avisos.DeleteObject(aviso);
                        
                        entities.SaveChanges();

                        if (application != null) application.Visible = true;

                        ListarAvisos();
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                        exception.Message +
                        (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                        @"Cerrar aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (book != null) book.Close(missing, missing, missing);
                    if (application != null) application.Application.Quit();
                }   
            }
        }

        private void EstadoDeResultadosToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Reporte del estado de resultados.

            var estadoResultados = new EstadoResultados();
            estadoResultados.ShowDialog();
        }

        private void balanceDeComprobaciónToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var comprobacion = new BalanceComprobacion();
            comprobacion.ShowDialog();
        }

        private void balancePorOperacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var bo = new CostoPorOperaciones();
            bo.ShowDialog();
        }

        private void estadoDeResultadosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Reporte del estado de resultados.

            var estadoResultados = new EstadoResultados();
            estadoResultados.ShowDialog();
        }

        private void GestiónDeProveedoresToolStripMenuItemClick(object sender, EventArgs e)
        {
            var prov = new GestionarProveedor(_user);
            prov.ShowDialog();
        }

        private void facToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // facturas por cliente.

            var fact = new MostrarFacturasPorCliente();
            fact.ShowDialog();
        }

        private void cerrarPeríodoToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void EliminarOperaciónToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Eliminar operación.

            var eliminarOperaciones = new EliminarOperaciones(_user);
            eliminarOperaciones.ShowDialog();
        }

        private void reporteDeClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reporteCliente rc = new reporteCliente();
            rc.ShowDialog();
        }

        private void reporteDeProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reporteProductos rp = new reporteProductos();
            rp.ShowDialog();
        }

        private void operacionesPorClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            operacionesXClientes opc = new operacionesXClientes();
            opc.ShowDialog();
        }

        private void operacionesPorTipoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            oepracionesXTipo opt = new oepracionesXTipo();
            opt.ShowDialog();
        }

        private void gestionarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionarTipoActivosFijos gaf = new GestionarTipoActivosFijos(this._user);
            gaf.ShowDialog();
        }

        private void gestiónDeTiposDeÚtilesYHerramientasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionarTipoUtiles gtu = new GestionarTipoUtiles(this._user);
            gtu.ShowDialog();
        }

        private void entradaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Entrada de productos en el sistema

            var entradaProductosForm = new EntradaProductosForm(_user);
            entradaProductosForm.ShowDialog();
        }

        private void salidaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var gastoForm = new InventarioGastoForm(_user);
            gastoForm.ShowDialog();
        }

        private void entradaToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var entradaUtiles = new EntradaUtiles(this._user);
            entradaUtiles.ShowDialog();
        }

        private void salidaToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var utilesgasto = new UtilesHerramGasto(this._user);
            utilesgasto.ShowDialog();
        }

        private void entradaToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            var activosF = new EntradaActivos(this._user);
            activosF.ShowDialog();
        }

        private void salidaToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            var activosfgasto = new ActivosFijosGastos(this._user);
            activosfgasto.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Está seguro que desea eliminar todos los avisos?", "Eliminar todos los avisos",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (var entities = new papiro_finalEntities())
                {
                    foreach (var aviso in entities.avisos.Where(a => a.id_usuario == _user.id))
                    {
                        entities.DeleteObject(aviso);
                    }
                    entities.SaveChanges();
                }
            }
            else
                return;
        }

        private void útilesYHerramientasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ReporteUtiles ru = new ReporteUtiles();
            ru.ShowDialog();
        }

        private void activosFijosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReporteActivosFijos ra = new ReporteActivosFijos();
            ra.ShowDialog();
        }

        private void cierreDeCuentasNominalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CierreCuentasNominales ccn = new CierreCuentasNominales(this._user);
            ccn.ShowDialog();
        }

        private void cobrosAnticipadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CobrosAnticipados ca = new CobrosAnticipados(this._user);
            ca.ShowDialog();
        }

        private void operaciónDeCobroAnticipadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OperacionCobroAnticipado oca = new OperacionCobroAnticipado(this._user);
            oca.ShowDialog();
        }

        private void reporteDeCobroAnticipadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReporteCobrosAnticipados rca = new ReporteCobrosAnticipados();
            rca.ShowDialog();
        }
    }
}
