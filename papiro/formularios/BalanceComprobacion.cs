using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class BalanceComprobacion : Form
    {
        public BalanceComprobacion()
        {
            InitializeComponent();
        }

        private void ActualizarReporte()
        {
            try
            {
                printableLV.Items.Clear();
                
                using (var entities = new papiro_finalEntities())
                {
                    var balance = new balance();

                    //Obtengo el balance anterior al actual
                    if (entities.balance.ToList().Count > 1)
                    {
                        //Si la fecha del filtro es igual al dia de hoy
                        if(filtroDesde.Value == DateTime.Now.Date)
                        {
                            if (entities.balance.ToList().Last().fecha != null)
                            {
                                balance = entities.balance.ToList().ElementAt(entities.balance.ToList().Count - 2);
                                if (balance.fecha != null)
                                    lbl_fecha.Text = balance.fecha.Value.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                balance = entities.balance.ToList().ElementAt(entities.balance.ToList().Count - 3);
                                if (balance.fecha != null)
                                    lbl_fecha.Text = balance.fecha.Value.ToString("dd/MM/yyyy");
                            }
                        }
                        else if(filtroDesde.Value <= DateTime.Now.Date && filtroHasta.Value <= DateTime.Now.Date
                            || filtroDesde.Value == filtroHasta.Value)
                        {
                            if(entities.balance.Where(b => b.fecha.Value.Day == filtroDesde.Value.Day
                                && b.fecha.Value.Month == filtroDesde.Value.Month 
                                && b.fecha.Value.Year == filtroDesde.Value.Year).Any())
                            {
                                if (entities.balance.Where(b => b.fecha.Value.Day == filtroDesde.Value.Day
                                && b.fecha.Value.Month == filtroDesde.Value.Month
                                && b.fecha.Value.Year == filtroDesde.Value.Year).Count() > 1)
                                {
                                    List<balance> aux = entities.balance.Where(b => b.fecha.Value.Day == filtroDesde.Value.Day
                                                                         && b.fecha.Value.Month == filtroDesde.Value.Month
                                                                         && b.fecha.Value.Year == filtroDesde.Value.Year).ToList();
                                    balance = aux.ElementAt(0);
                                }
                                else
                                {
                                    //if(filtroDesde.Value.Day == 1)
                                    //{
                                        List<balance> aux1 = entities.balance.ToList();

                                        foreach (var balance1 in aux1)
                                        {
                                            if(balance1.fecha.Value.Day == filtroDesde.Value.Day
                                                                      && balance1.fecha.Value.Month == filtroDesde.Value.Month
                                                                      && balance1.fecha.Value.Year == filtroDesde.Value.Year)
                                                break;
                                            balance = balance1;
                                        }
                                        if (balance.fecha != null)
                                            lbl_fecha.Text = balance.fecha.Value.ToString("dd/MM/yyyy");
                                    //}
                                    //else
                                    //{
                                    //    balance = entities.balance.Where(b => b.fecha.Value.Day == filtroDesde.Value.Day -1
                                    //                                  && b.fecha.Value.Month == filtroDesde.Value.Month
                                    //                                  && b.fecha.Value.Year == filtroDesde.Value.Year).
                                    //        Single();
                                    //            if (balance.fecha != null)
                                    //                lbl_fecha.Text = balance.fecha.Value.ToString("dd/MM/yyyy");
                                    //}
                                    //balance = entities.balance.Where(b => b.fecha.Value.Day == filtroDesde.Value.Day
                                    //                                  && b.fecha.Value.Month == filtroDesde.Value.Month
                                    //                                  && b.fecha.Value.Year == filtroDesde.Value.Year).
                                    //Single();
                                    //if (balance.fecha != null)
                                    //    lbl_fecha.Text = balance.fecha.Value.ToString("dd/MM/yyyy");
                                }
                                
                            }
                            else
                            {
                                MessageBox.Show(
                                    "No existe ningun cierre de cuentas para el dia solicitado, verifique si fue un dia trabajado",
                                    "Fecha de cierre inexistente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show(
                                    "Error en la selección de fechas para el filtro",
                                    "Error en la entrada de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        balance = entities.balance.ToList().Last();
                        lbl_fecha.Text = balance.fecha.Value.ToString("dd/MM/yyyy");
                    }

                    // Efectivo en caja
                    decimal debeEfectivoCaja = 0;
                    decimal haberEfectivoCaja = 0;
                    foreach (var submayorEfectivoCaja in entities.submayor_efectivo_caja)
                    {
                        if (submayorEfectivoCaja.fecha.Date < filtroDesde.Value.Date) continue;

                        if (submayorEfectivoCaja.fecha.Date > filtroHasta.Value.Date) continue;

                        if (submayorEfectivoCaja.debito != null)
                            debeEfectivoCaja += submayorEfectivoCaja.debito.Value;
                        else
                            haberEfectivoCaja += submayorEfectivoCaja.credito.Value;
                    }
                    decimal acumDebeEfectivoCaja = balance.efectivo_caja + debeEfectivoCaja - haberEfectivoCaja;
                    //if (acumDebeEfectivoCaja < 0)
                    //    acumDebeEfectivoCaja = -1 * acumDebeEfectivoCaja;
                    printableLV.Items.Add(
                        new ListViewItem(new[]
                                             {
                                                 "101", "Efectivo en Caja",
                                                 Math.Round(balance.efectivo_caja,2).ToString(CultureInfo.InvariantCulture), "",
                                                 Math.Round(debeEfectivoCaja, 2).ToString(CultureInfo.InvariantCulture),
                                                 Math.Round(haberEfectivoCaja, 2).ToString(CultureInfo.InvariantCulture)
                                                 ,
                                                 acumDebeEfectivoCaja > 0 ? Math.Round(acumDebeEfectivoCaja, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString(), 
                                                 acumDebeEfectivoCaja < 0 ? Math.Round(acumDebeEfectivoCaja * -1, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString()
                                             }));

                    // Efectivo en banco
                    decimal debeEfectivoBanco = 0;
                    decimal haberEfectivoBanco = 0;
                    foreach (var submayorEfectivoBanco in entities.submayor_efectivo_banco)
                    {
                        if (submayorEfectivoBanco.fecha.Date < filtroDesde.Value.Date) continue;

                        if (submayorEfectivoBanco.fecha.Date > filtroHasta.Value.Date) continue;

                        if (submayorEfectivoBanco.debito != null)
                            debeEfectivoBanco += submayorEfectivoBanco.debito.Value;
                        else
                            haberEfectivoBanco += submayorEfectivoBanco.credito.Value;
                    }
                    decimal acumDebeEfectivobanco = balance.efectivo_banco + debeEfectivoBanco - haberEfectivoBanco;
                    //if (acumDebeEfectivobanco < 0)
                    //    acumDebeEfectivobanco = -1 * acumDebeEfectivobanco;
                    printableLV.Items.Add(
                        new ListViewItem(new[]
                                             {
                                                 "110", "Efectivo en Banco",
                                                 Math.Round(balance.efectivo_banco, 2).ToString(CultureInfo.InvariantCulture), "",
                                                 Math.Round(debeEfectivoBanco, 2).ToString(CultureInfo.InvariantCulture)
                                                 ,
                                                 Math.Round(haberEfectivoBanco, 2).ToString(CultureInfo.InvariantCulture)
                                                 ,
                                                 acumDebeEfectivobanco > 0 ? Math.Round(acumDebeEfectivobanco, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString(), 
                                                 acumDebeEfectivobanco < 0 ? Math.Round(acumDebeEfectivobanco * -1, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString()
                                             }));

                    // Cuentas por cobrar.
                    decimal debeCuentasCobrar = 0;
                    decimal haberCuentasCobrar = 0;
                    foreach (var submayorCuentasCobrar in entities.submayor_cuentas_cobrar)
                    {
                        if (submayorCuentasCobrar.fecha.Date < filtroDesde.Value.Date) continue;

                        if (submayorCuentasCobrar.fecha.Date > filtroHasta.Value.Date) continue;

                        if (submayorCuentasCobrar.debito != null)
                            debeCuentasCobrar += submayorCuentasCobrar.debito.Value;
                        else
                            haberCuentasCobrar += submayorCuentasCobrar.credito.Value;
                    }
                    decimal acumCuentasCobrar = balance.cuentas_por_cobrar + debeCuentasCobrar - haberCuentasCobrar;
                    //if (acumCuentasCobrar < 0)
                    //    acumCuentasCobrar = -1 * acumCuentasCobrar;
                    printableLV.Items.Add(
                        new ListViewItem(new[]
                                             {
                                                 "135", "Cuentas por Cobrar",
                                                 Math.Round(balance.cuentas_por_cobrar, 2).ToString(CultureInfo.InvariantCulture), "",
                                                 Math.Round(debeCuentasCobrar, 2).ToString(CultureInfo.InvariantCulture)
                                                 ,
                                                 Math.Round(haberCuentasCobrar, 2).ToString(CultureInfo.InvariantCulture)
                                                 ,
                                                 acumCuentasCobrar > 0 ? Math.Round(acumCuentasCobrar, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString(), 
                                                 acumCuentasCobrar < 0 ? Math.Round(acumCuentasCobrar * -1, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString()
                                             }));

                    // Inventario.
                    decimal debeInventario = 0;
                    decimal haberInventario = 0;
                    foreach (var submayorInventario in entities.submayor_inventario)
                    {
                        if (submayorInventario.fecha.Date < filtroDesde.Value.Date) continue;

                        if (submayorInventario.fecha.Date > filtroHasta.Value.Date) continue;

                        if (submayorInventario.debito != null)
                            debeInventario += submayorInventario.debito.Value;
                        else
                            haberInventario += submayorInventario.credito.Value;
                    }
                    decimal acumInventario = balance.inventario + debeInventario - haberInventario;
                    //if (acumInventario < 0)
                    //    acumInventario = -1 * acumInventario;
                    printableLV.Items.Add(
                        new ListViewItem(new[]
                                             {
                                                 "183", "Inventario",
                                                 Math.Round(balance.inventario, 2).ToString(CultureInfo.InvariantCulture), "",
                                                 Math.Round(debeInventario, 2).ToString(CultureInfo.InvariantCulture)
                                                 ,
                                                 Math.Round(haberInventario, 2).ToString(CultureInfo.InvariantCulture)
                                                 ,
                                                 acumInventario > 0 ? Math.Round(acumInventario, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString(),
                                                 acumInventario < 0 ? Math.Round(acumInventario * -1, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString()
                                             }));

                    //Utiles y herramientas
                    decimal debeutiles = 0;
                    decimal haberutiles = 0;
                    foreach (var subutil in entities.submayor_utiles)
                    {
                        if (subutil.fecha.Date < filtroDesde.Value.Date) continue;

                        if (subutil.fecha.Date > filtroHasta.Value.Date) continue;

                        if (subutil.debito != null)
                            debeutiles += subutil.debito.Value;
                        else
                            haberutiles += subutil.credito.Value;
                    }
                    decimal acumUtiles = balance.utiles_herramientas + debeutiles - haberutiles;
                    //if(acumUtiles < 0)
                    //    acumUtiles = -1 * acumUtiles;
                    printableLV.Items.Add(
                        new ListViewItem(new[]
                                             {
                                                 "188", "Útiles y Herramientas",
                                                 Math.Round(balance.utiles_herramientas, 2).ToString(CultureInfo.InvariantCulture), "",
                                                 Math.Round(debeutiles, 2).ToString(CultureInfo.InvariantCulture)
                                                 ,
                                                 Math.Round(haberutiles, 2).ToString(CultureInfo.InvariantCulture)
                                                 ,
                                                 acumUtiles > 0 ? Math.Round(acumUtiles, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString(), 
                                                 acumUtiles < 0 ? Math.Round(acumUtiles * -1, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString()
                                             }));


                    // Activos fijos tangibles.
                    decimal debeActFijosTang = 0;
                    decimal haberActFijosTang = 0;
                    foreach (var submayorActFijosTang in entities.submayor_activos_fijo_tangible)
                    {
                        if (submayorActFijosTang.fecha.Date < filtroDesde.Value.Date) continue;

                        if (submayorActFijosTang.fecha.Date > filtroHasta.Value.Date) continue;

                        if (submayorActFijosTang.debito != null)
                            debeActFijosTang += submayorActFijosTang.debito.Value;
                        else
                            haberActFijosTang += submayorActFijosTang.credito.Value;
                    }
                    decimal acumActFijosTang = balance.activos_fijo_tangible + debeActFijosTang - haberActFijosTang;
                    //if (acumActFijosTang < 0)
                    //    acumActFijosTang = -1 * acumActFijosTang;
                    printableLV.Items.Add(
                        new ListViewItem(new[]
                                             {
                                                 "240", "Activos Fijos Tangibles",
                                                 Math.Round(balance.activos_fijo_tangible, 2).ToString(CultureInfo.InvariantCulture), "",
                                                 Math.Round(debeActFijosTang, 2).ToString(CultureInfo.InvariantCulture)
                                                 ,
                                                 Math.Round(haberActFijosTang, 2).ToString(CultureInfo.InvariantCulture)
                                                 ,
                                                 acumActFijosTang > 0 ? Math.Round(acumActFijosTang, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString(), 
                                                 acumActFijosTang < 0 ? Math.Round(acumActFijosTang * -1, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString()
                                             }));

                    
                    // Vacia
                    printableLV.Items.Add(new ListViewItem(new string[8]));

                    //Cobro Anticipado
                    decimal debeCobro = 0;
                    decimal haberCobro = 0;

                    foreach (var ca in entities.submayor_cobro_anticipado)
                    {
                        if(ca.fecha.Date < filtroDesde.Value.Date) continue;

                        if(ca.fecha.Date > filtroHasta.Value.Date) continue;

                        if (ca.debito != null)
                            debeCobro += ca.debito.Value;
                        if (ca.credito != null)
                            haberCobro += ca.credito.Value;
                    }
                    decimal acumuladoCobro = (balance.cobro_anticipado != null ? balance.cobro_anticipado.Value : 0) +
                                             haberCobro - debeCobro;
                    //if (acumuladoCobro < 0)
                    //    acumuladoCobro = -1 * acumuladoCobro;

                    printableLV.Items.Add(new ListViewItem(new[]
                                              {
                                                  "334", "Cobro anticipado", "",
                                                  Math.Round(balance.cobro_anticipado == null ? 0 : balance.cobro_anticipado.Value, 2).ToString(CultureInfo.InvariantCulture),
                                                  Math.Round(debeCobro, 2).ToString(CultureInfo.InvariantCulture),
                                                  Math.Round(haberCobro, 2).ToString(CultureInfo.InvariantCulture),
                                                  acumuladoCobro < 0 ? Math.Round(acumuladoCobro * -1, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString(),
                                                  acumuladoCobro > 0 ? Math.Round(acumuladoCobro, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString()
                                              }));

                    // Cuentas por pagar.
                    decimal debeCuentasPagar = 0;
                    decimal haberCuentasPagar = 0;
                    foreach (var submayorCuentasPagar in entities.submayor_cuentas_por_pagar)
                    {
                        if (submayorCuentasPagar.fecha.Date < filtroDesde.Value.Date) continue;

                        if (submayorCuentasPagar.fecha.Date > filtroHasta.Value.Date) continue;

                        if (submayorCuentasPagar.credito != null)
                            haberCuentasPagar += submayorCuentasPagar.credito.Value;
                        else
                            debeCuentasPagar += submayorCuentasPagar.debito.Value;
                    }
                    //debeCuentasPagar -= haberCuentasPagar;
                    decimal acumCuentasPagar = balance.cuentas_por_pagar + haberCuentasPagar - debeCuentasPagar;
                    //if (acumCuentasPagar < 0)
                    //    acumCuentasPagar = -1 * acumCuentasPagar;
                    printableLV.Items.Add(
                        new ListViewItem(new[]
                                             {
                                                 "405", "Cuentas por pagar", "",
                                                 Math.Round(balance.cuentas_por_pagar, 2).ToString(CultureInfo.InvariantCulture),
                                                 Math.Round(debeCuentasPagar, 2).ToString(CultureInfo.InvariantCulture)
                                                 ,
                                                 Math.Round(haberCuentasPagar, 2).ToString(CultureInfo.InvariantCulture)
                                                 , acumCuentasPagar < 0 ? Math.Round(acumCuentasPagar * -1, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString(),
                                                 acumCuentasPagar > 0 ? Math.Round(acumCuentasPagar, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString()
                                             }));




                    //Nominas por Pagar
                    decimal deberNominaGastos = 0;
                    decimal haberNominaGastos = 0;
                    decimal acumulado = 0;

                    foreach (var nom in entities.submayor_nomina)
                    {
                        if (nom.fecha.Date < filtroDesde.Value.Date) continue;

                        if (nom.fecha.Date > filtroHasta.Value.Date) continue;

                        if (nom.debito != null)
                            deberNominaGastos += nom.debito.Value;
                        if (nom.credito != null)
                            haberNominaGastos += nom.credito.Value;
                    }
                    //int x = 0;
                    //foreach (var nomina in from n in entities.submayor_nomina orderby n.fecha descending select n)
                    //{
                    //    if(nomina.ultimo_pago == 1)
                    //        x++;
                    //    if (nomina.credito != null)
                    //        haberNominaGastos += nomina.credito.Value;
                    //}
                    
                   
                    if (filtroHasta.Value.Date == DateTime.Now.Date)
                    {
                        foreach (var user in entities.usuarios)
                        {
                            if (user.salario_extra_operaciones != null)
                                acumulado += user.salario_extra_operaciones.Value;
                        }
                    }
                    haberNominaGastos += acumulado;
                    decimal acumNomina = (balance.nominas_pagar != null ? balance.nominas_pagar.Value : 0) + haberNominaGastos - deberNominaGastos;
                    //if (acumNomina < 0)
                    //    acumNomina = -1 * acumNomina;
                    printableLV.Items.Add(
                        new ListViewItem(new[]
                                             {
                                                 "455", "Nóminas por Pagar", "",
                                                 Math.Round(balance.nominas_pagar == null ? 0 : balance.nominas_pagar.Value, 2).ToString(CultureInfo.InvariantCulture),
                                                 Math.Round(deberNominaGastos, 2).ToString(CultureInfo.InvariantCulture)
                                                 ,
                                                 Math.Round(haberNominaGastos, 2).ToString(CultureInfo.InvariantCulture)
                                                 , acumNomina < 0 ? Math.Round(acumNomina * -1, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString(),
                                                 acumNomina > 0 ? Math.Round(acumNomina, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString()
                                             }));


                    // Vacia
                    printableLV.Items.Add(new ListViewItem(new string[8]));

                    // Cálculos de ingresos.
                    decimal haberIngresos = 0;
                    decimal debeIngreso = 0;
                    decimal acumIngresos = 0;
                    foreach (var submayorIngreso in entities.submayor_ingreso)
                    {
                        if (submayorIngreso.fecha.Date < filtroDesde.Value.Date) continue;

                        if (submayorIngreso.fecha.Date > filtroHasta.Value.Date) continue;

                        if (submayorIngreso.credito != null)
                            haberIngresos += submayorIngreso.credito.Value;
                        if (submayorIngreso.debito != null)
                            debeIngreso += submayorIngreso.debito.Value;
                    }
                    acumIngresos = balance.ingreso + haberIngresos - debeIngreso;
                    //if (acumIngresos < 0)
                    //    acumIngresos = -1 * acumIngresos;
                    // Cálculos de costos.
                    decimal deberCostos = 0;
                    decimal haberCostos = 0;
                    decimal acumCostos = 0;
                    foreach (var submayorCosto in entities.submayor_costo)
                    {
                        if (submayorCosto.fecha.Date < filtroDesde.Value.Date) continue;

                        if (submayorCosto.fecha.Date > filtroHasta.Value.Date) continue;

                        if (submayorCosto.debito != null)
                            deberCostos += submayorCosto.debito.Value;
                        if (submayorCosto.credito != null)
                            haberCostos += submayorCosto.credito.Value;
                    }
                    acumCostos = balance.costo + deberCostos - haberCostos;
                    //if (acumCostos < 0)
                    //    acumCostos = -1 * acumCostos;

                    // Cálculos de gastos.
                    decimal deberGastos = 0;
                    decimal haberGastos = 0;
                    decimal acumGastos = 0;
                    foreach (var submayorGastos in entities.submayor_gasto)
                    {
                        if (submayorGastos.fecha.Date < filtroDesde.Value.Date) continue;

                        if (submayorGastos.fecha.Date > filtroHasta.Value.Date) continue;

                        if (submayorGastos.debito != null)
                            deberGastos += submayorGastos.debito.Value;
                        if (submayorGastos.credito != null)
                            haberGastos += submayorGastos.credito.Value;
                    }
                    deberGastos += acumulado;
                    //deberGastos += gasto_acumulado;
                    acumGastos = balance.gasto + deberGastos - haberGastos;
                    //if(acumGastos < 0)
                    //    acumGastos = -1 * acumGastos;
                    // Papiro Capital
                    decimal debePapiroCapital = 0;
                    decimal haberPapiroCapital = haberIngresos - deberCostos - deberGastos;

                    //En haberPapiroCapital esta la utilidad de la empresa en el periodo
                    //por lo que actualizo el label del formulario con el valor
                    if (haberPapiroCapital > 0)
                    {
                        lblUtilidad.Text =
                            string.Format(
                                "Utilidad de: {0} Ultimo cierre: " +
                                (balance.fecha == null ? "" : balance.fecha.Value.ToString("dd/MM/yyyy")),
                                Math.Round(haberPapiroCapital, 2).ToString(CultureInfo.InvariantCulture));
                    }
                    else if (haberPapiroCapital == 0)
                        lblUtilidad.Visible = false;
                    else
                        lblUtilidad.Text =
                            string.Format(
                                "Pérdida de: {0} Ultimo cierre: " +
                                (balance.fecha == null ? "" : balance.fecha.Value.ToString("dd/MM/yyyy")),
                                Math.Round(haberPapiroCapital, 2).ToString(CultureInfo.InvariantCulture));

                    //foreach (var submayorPapiroCapital in entities.submayor_papiro_capital)
                    //{
                    //    if (submayorPapiroCapital.fecha.Date < filtroDesde.Value.Date) continue;

                    //    if (submayorPapiroCapital.fecha.Date > filtroHasta.Value.Date) continue;

                    //    if (submayorPapiroCapital.credito != null)
                    //        debePapiroCapital += submayorPapiroCapital.credito.Value;
                    //}
                    //decimal acumPapiroCapital = balance.papiro_capital - debePapiroCapital + haberPapiroCapital;
                    decimal papiro_acumulado = 0;
                    decimal haberpapiro = 0;
                    decimal acum_papiro = 0;

                    foreach (var p in entities.submayor_papiro_capital)
                    {
                        if (p.fecha.Date < filtroDesde.Value.Date) continue;

                        if (p.fecha.Date > filtroHasta.Value.Date) continue;

                        if (p.credito != 0 && p.credito != null)
                            papiro_acumulado += p.credito.Value;
                        if (p.debito != null)
                            haberpapiro += p.debito.Value;
                    }
                    acum_papiro = balance.papiro_capital + haberpapiro - papiro_acumulado;


                    printableLV.Items.Add(
                        new ListViewItem(new[]
                                             {
                                                 "600", "Papiro Capital", "",
                                                 Math.Round(balance.papiro_capital, 2).ToString(CultureInfo.InvariantCulture),
                                                 Math.Round(papiro_acumulado, 2).ToString(
                                                     CultureInfo.InvariantCulture)
                                                 ,
                                                 Math.Round(haberpapiro, 2).ToString(CultureInfo.InvariantCulture)
                                                 , acum_papiro < 0 ? Math.Round(acum_papiro * -1, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString(),
                                                 acum_papiro > 0 ? Math.Round(acum_papiro, 2).ToString(
                                                     CultureInfo.InvariantCulture) : 0.ToString()
                                             }));

                    // Costo de Ventas
                    printableLV.Items.Add(
                        new ListViewItem(new[]
                                             {
                                                 "800", "Costo de Ventas",
                                                 Math.Round(balance.costo, 2).ToString(CultureInfo.InvariantCulture), "",
                                                 Math.Round(deberCostos, 2).ToString(CultureInfo.InvariantCulture),
                                                 Math.Round(haberCostos, 2).ToString(CultureInfo.InvariantCulture)
                                                 , acumCostos > 0 ? Math.Round(acumCostos, 2).ToString(CultureInfo.InvariantCulture) : 0.ToString(), 
                                                 acumCostos < 0 ? Math.Round(acumCostos * -1, 2).ToString(CultureInfo.InvariantCulture) : 0.ToString()
                                             }));

                    // Gasto de Operaciones
                    printableLV.Items.Add(
                        new ListViewItem(new[]
                                             {
                                                 "825", "Gasto de Operaciones",
                                                 Math.Round(balance.gasto, 2).ToString(CultureInfo.InvariantCulture), "",
                                                 Math.Round(deberGastos, 2).ToString(CultureInfo.InvariantCulture),
                                                 Math.Round(haberGastos, 2).ToString(CultureInfo.InvariantCulture)
                                                 , acumGastos > 0 ? Math.Round(acumGastos, 2).ToString(CultureInfo.InvariantCulture) : 0.ToString(), 
                                                 acumGastos < 0 ? Math.Round(acumGastos * -1, 2).ToString(CultureInfo.InvariantCulture) : 0.ToString()
                                             }));

                    // Vacia
                    printableLV.Items.Add(new ListViewItem(new string[8]));


                    // Ingresos por Ventas
                    printableLV.Items.Add(
                        new ListViewItem(new[]
                                             {
                                                 "905", "Ingresos por Ventas", "", 
                                                 Math.Round(balance.ingreso, 2).ToString(CultureInfo.InvariantCulture),
                                                 Math.Round(debeIngreso, 2).ToString(CultureInfo.InvariantCulture),
                                                 Math.Round(haberIngresos, 2).ToString(CultureInfo.InvariantCulture),
                                                 acumIngresos < 0 ? Math.Round(acumIngresos * -1, 2).ToString(CultureInfo.InvariantCulture) : 0.ToString()
                                                 , acumIngresos > 0 ? Math.Round(acumIngresos, 2).ToString(CultureInfo.InvariantCulture) : 0.ToString()
                                             }));

                    // Vacia
                    printableLV.Items.Add(new ListViewItem(new string[8]));

                    // Totales
                    decimal totalA = balance.efectivo_caja + balance.efectivo_banco + balance.cuentas_por_cobrar + balance.inventario + balance.activos_fijo_tangible + balance.utiles_herramientas + balance.costo + balance.gasto;

                    decimal totalB = balance.cuentas_por_pagar + balance.papiro_capital + balance.ingreso + (balance.nominas_pagar == null ? 0 : balance.nominas_pagar.Value) + (balance.cobro_anticipado == null ? 0 : balance.cobro_anticipado.Value);

                    decimal total1 = debeEfectivoCaja + debeEfectivoBanco + debeCuentasCobrar + debeInventario + debeutiles +
                                     debeActFijosTang + debeCuentasPagar + deberCostos + deberGastos + deberNominaGastos + debeCobro + papiro_acumulado;
                    decimal total2 = haberEfectivoCaja + haberEfectivoBanco + haberCuentasCobrar + haberInventario + haberutiles +
                                     haberActFijosTang + haberCuentasPagar + haberIngresos + haberNominaGastos + haberCobro;
                    decimal total3 = (acumDebeEfectivoCaja < 0 ? 0 : acumDebeEfectivoCaja) + (acumDebeEfectivobanco < 0 ? 0 : acumDebeEfectivobanco) + (acumCuentasCobrar < 0 ? 0 : acumCuentasCobrar) + (acumInventario < 0 ? 0 : acumInventario) + (acumUtiles < 0 ? 0 : acumUtiles) +
                        (acumActFijosTang < 0 ? 0 : acumActFijosTang) + (acumCostos < 0 ? 0 : acumCostos) + (acumGastos < 0 ? 0 : acumGastos) + (acumCuentasPagar < 0 ? acumCuentasPagar * -1 : 0) + (acum_papiro < 0 ? acum_papiro * -1 : 0) + (acumIngresos < 0 ? acumIngresos * -1 : 0) + (acumNomina < 0 ? acumNomina * -1 : 0) + (acumuladoCobro < 0 ? acumuladoCobro * -1 : 0);
                    //decimal total4 = acumCuentasPagar + acumPapiroCapital + acumIngresos;
                    decimal total4 = (acumDebeEfectivoCaja < 0 ? acumDebeEfectivoCaja * -1 : 0) + (acumDebeEfectivobanco < 0 ? acumDebeEfectivobanco * -1 : 0) + (acumCuentasCobrar < 0 ? acumCuentasCobrar * -1 : 0) + (acumInventario < 0 ? acumInventario * -1 : 0) + (acumUtiles < 0 ? acumUtiles * -1 : 0) + (acumActFijosTang < 0 ? acumActFijosTang * -1 : 0) + (acumCostos < 0 ? acumCostos * -1 : 0) + (acumGastos < 0 ? acumGastos * -1 : 0)
                        + (acumCuentasPagar < 0 ? 0 : acumCuentasPagar) + (acum_papiro < 0 ? 0 : acum_papiro) + (acumIngresos < 0 ? 0 : acumIngresos) + (acumNomina < 0 ? 0 : acumNomina) + (acumuladoCobro < 0 ? 0 : acumuladoCobro);
                    printableLV.Items.Add(
                            new ListViewItem(new[]
                                                 {
                                                     "", "Totales",
                                                     Math.Round(totalA, 2).ToString(CultureInfo.InvariantCulture),
                                                     Math.Round(totalB, 2).ToString(CultureInfo.InvariantCulture),
                                                     Math.Round(total1, 2).ToString(CultureInfo.InvariantCulture),
                                                     Math.Round(total2, 2).ToString(CultureInfo.InvariantCulture),
                                                     Math.Round(total3, 2).ToString(CultureInfo.InvariantCulture),
                                                     Math.Round(total4, 2).ToString(CultureInfo.InvariantCulture)
                                                 }));
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Balance de comprobación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GestionarCuentasCobrarLoad(object sender, EventArgs e)
        {
            filtroDesde.Value = DateTime.Today;
            filtroHasta.Value = DateTime.Today;
            
            ActualizarReporte();
        }

        private void FiltrarClick(object sender, EventArgs e)
        {
            //if(filtroDesde.Value.Date == filtroHasta.Value.Date)
            //{
            //    MessageBox.Show("La fecha de inicio debe ser menor que la fecha final", "Error en la entrada de datos",
            //                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            ActualizarReporte();
        }

        private void btCerrar_Click(object sender, EventArgs e)
        {
            //esto ya no sirve

            //contexto de entidades
            var entities = new papiro_finalEntities();

            //cargo la ultima fila del balance
            balance last_balance = entities.balance.ToList().Last();

            if (last_balance.fecha.Value.Year == DateTime.Now.Year && last_balance.fecha.Value.Month == DateTime.Now.Month
                && last_balance.fecha.Value.Day == DateTime.Now.Day)
            {
                MessageBox.Show("No se puede realizar mas de un cierre de cuentas en el mismo dia.", "Cierre de cuentas", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if(last_balance.fecha.Value > filtroDesde.Value.Date || last_balance.fecha.Value > filtroHasta.Value.Date)
            {
                MessageBox.Show("Ya existe un cierre de cuentas para ese período.", "Cierre de cuentas", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {


                //creo una nueva entidad para el balance
                balance nuevo_balance = new balance();

                //lo lleno a mano para evitar errores de datos
                nuevo_balance.activos_fijo_tangible = last_balance.activos_fijo_tangible;
                nuevo_balance.cuentas_por_cobrar = last_balance.cuentas_por_cobrar;
                nuevo_balance.cuentas_por_pagar = last_balance.cuentas_por_pagar;
                nuevo_balance.efectivo_banco = last_balance.efectivo_banco;
                nuevo_balance.efectivo_caja = last_balance.efectivo_caja;
                nuevo_balance.fecha = DateTime.Now;
                nuevo_balance.costo = last_balance.costo;
                nuevo_balance.gasto = last_balance.gasto;
                nuevo_balance.inventario = last_balance.inventario;
                nuevo_balance.papiro_capital = last_balance.papiro_capital;
                nuevo_balance.papiro_capital += last_balance.ingreso - last_balance.costo - last_balance.gasto;

                entities.AddTobalance(nuevo_balance);
                entities.SaveChanges();

                var num = last_balance.ingreso - last_balance.costo - last_balance.gasto;
                if (num < 0)
                {
                    entities.AddTosubmayor_papiro_capital(new submayor_papiro_capital
                    {
                        fecha = DateTime.Now,
                        descripcion = "Se realiza el cierre de cuentas con una pérdida de: " + num.ToString(),
                        saldo = nuevo_balance.papiro_capital,
                        debito = num
                    });
                }
                else
                {
                    entities.AddTosubmayor_papiro_capital(new submayor_papiro_capital
                    {
                        fecha = DateTime.Now,
                        descripcion = "Se realiza el cierre de cuentas con una utilidad de: " + num.ToString(),
                        saldo = nuevo_balance.papiro_capital,
                        credito = num
                    });
                }

                
            }

            this.ActualizarReporte();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(filtroDesde.Value.Date == filtroHasta.Value.Date)
            {
                this.printableLV.Title = "Balance de Comprobación";
            }
            else
            {
                this.printableLV.Title = "Balance de Comprobación desde el " +
                                         filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta " +
                                         filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            }
            printableLV.PrintPreview();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (filtroDesde.Value.Date == filtroHasta.Value.Date)
            {
                this.printableLV.Title = "Balance de Comprobación";
            }
            else
            {
                this.printableLV.Title = "Balance de Comprobación desde el " +
                                         filtroDesde.Value.Date.ToString("dd/MM/yyyy") + " hasta " +
                                         filtroHasta.Value.Date.ToString("dd/MM/yyyy");
            }
            printableLV.Print();
        }
    }
}
