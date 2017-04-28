using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class Balance : Form
    {
        private readonly usuarios _user;

        public Balance(usuarios user)
        {
            InitializeComponent();
            _user = user;
        }

        private void SalirButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void GuardarButtonClick(object sender, EventArgs e)
        {
            // Actualizar los datos de balance.

            if (MessageBox.Show("Esta acción modificará los valores de las cuentas en la Base de Datos\n pudiendo traer un manl funcionamiento en el sistema. ¿Estás seguro de querer realizarla?",
                               @"Nómina", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                try
                {
                    using (var entities = new papiro_finalEntities())
                    {
                        //if (entities.balance.ToList().Count == 0 || entities.balance.ToList().Count > 1)
                        //{
                        //    MessageBox.Show("Ya se introdujo el estado de cuentas en la carga inicial", "Manipulación de datos sencibles", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        //    this.Close();
                        //}

                        var balance = entities.balance.ToList().Last();

                        if (balance.efectivo_caja != efectivo_cajanumericUpDown.Value)
                        {
                            //entities.AddTosubmayor_efectivo_caja(new submayor_efectivo_caja
                            //                                         {
                            //                                             id_usuario = _user.id,
                            //                                             fecha = DateTime.Now,
                            //                                             monto_antes_cambio = balance.efectivo_caja,
                            //                                             monto_cambio =
                            //                                                 efectivo_cajanumericUpDown.Value -
                            //                                                 balance.efectivo_caja,
                            //                                             descripcion = "Actualizando balance."
                            //                                         });
                            balance.efectivo_caja = efectivo_cajanumericUpDown.Value;
                        }

                        if (balance.efectivo_banco != efectivo_banconumericUpDown.Value)
                        {
                            //entities.AddTosubmayor_efectivo_banco(new submayor_efectivo_banco
                            //                                          {
                            //                                              id_usuario = _user.id,
                            //                                              fecha = DateTime.Now,
                            //                                              monto_antes_cambio = balance.efectivo_banco,
                            //                                              monto_cambio =
                            //                                                  efectivo_banconumericUpDown.Value -
                            //                                                  balance.efectivo_banco,
                            //                                              descripcion = "Actualizando balance."
                            //                                          });
                            balance.efectivo_banco = efectivo_banconumericUpDown.Value;
                        }

                        if (balance.cuentas_por_cobrar != cuentas_por_cobrarnumericUpDown.Value)
                        {
                            //entities.AddTosubmayor_cuentas_cobrar(new submayor_cuentas_cobrar
                            //                                          {
                            //                                              id_usuario = _user.id,
                            //                                              fecha = DateTime.Now,
                            //                                              monto_antes_cambio = balance.cuentas_por_cobrar,
                            //                                              monto_cambio =
                            //                                                  cuentas_por_cobrarnumericUpDown.Value -
                            //                                                  balance.cuentas_por_cobrar,
                            //                                              descripcion = "Actualizando balance."
                            //                                          });
                            balance.cuentas_por_cobrar = cuentas_por_cobrarnumericUpDown.Value;
                        }

                        if (balance.inventario != inventarionumericUpDown.Value)
                        {
                            //entities.AddTosubmayor_inventario(new submayor_inventario
                            //                                      {
                            //                                          id_usuario = _user.id,
                            //                                          fecha = DateTime.Now,
                            //                                          monto_antes_cambio = balance.inventario,
                            //                                          monto_cambio =
                            //                                              inventarionumericUpDown.Value -
                            //                                              balance.inventario,
                            //                                          descripcion = "Actualizando balance."
                            //                                      });
                            balance.inventario = inventarionumericUpDown.Value;
                        }

                        if (balance.activos_fijo_tangible != activos_fijo_tangiblenumericUpDown.Value)
                        {
                            //entities.AddTosubmayor_activos_fijo_tangible(new submayor_activos_fijo_tangible
                            //                                                 {
                            //                                                     id_usuario = _user.id,
                            //                                                     fecha = DateTime.Now,
                            //                                                     monto_antes_cambio =
                            //                                                         balance.activos_fijo_tangible,
                            //                                                     monto_cambio =
                            //                                                         activos_fijo_tangiblenumericUpDown.
                            //                                                             Value -
                            //                                                         balance.activos_fijo_tangible,
                            //                                                     descripcion = "Actualizando balance."
                            //                                                 });
                            balance.activos_fijo_tangible = activos_fijo_tangiblenumericUpDown.Value;
                        }

                        if (balance.gasto != gastonumericUpDown.Value)
                        {
                            //entities.AddTosubmayor_gasto(new submayor_gasto
                            //                                 {
                            //                                     id_usuario = _user.id,
                            //                                     fecha = DateTime.Now,
                            //                                     monto_antes_cambio =
                            //                                         balance.gasto,
                            //                                     monto_cambio =
                            //                                         gastonumericUpDown.Value -
                            //                                         balance.gasto,
                            //                                     descripcion = "Actualizando balance."
                            //                                 });
                            balance.gasto = gastonumericUpDown.Value;
                        }

                        if (balance.costo != costonumericUpDown.Value)
                        {
                            //entities.AddTosubmayor_costo(new submayor_costo
                            //                                 {
                            //                                     id_usuario = _user.id,
                            //                                     fecha = DateTime.Now,
                            //                                     monto_antes_cambio =
                            //                                         balance.costo,
                            //                                     monto_cambio =
                            //                                         costonumericUpDown.Value -
                            //                                         balance.costo,
                            //                                     descripcion = "Actualizando balance."
                            //                                 });
                            balance.costo = costonumericUpDown.Value;
                        }

                        if (balance.cuentas_por_pagar != cuentas_por_pagarnumericUpDown.Value)
                        {
                            //entities.AddTosubmayor_cuentas_por_pagar(new submayor_cuentas_por_pagar
                            //                                             {
                            //                                                 id_usuario = _user.id,
                            //                                                 fecha = DateTime.Now,
                            //                                                 monto_antes_cambio =
                            //                                                     balance.cuentas_por_pagar,
                            //                                                 monto_cambio =
                            //                                                     cuentas_por_pagarnumericUpDown.Value -
                            //                                                     balance.cuentas_por_pagar,
                            //                                                 descripcion = "Actualizando balance."
                            //                                             });
                            balance.cuentas_por_pagar = cuentas_por_pagarnumericUpDown.Value;
                        }

                        if (balance.papiro_capital != papiro_capitalnumericUpDown.Value)
                        {
                            //entities.AddTosubmayor_papiro_capital(new submayor_papiro_capital
                            //                                          {
                            //                                              id_usuario = _user.id,
                            //                                              fecha = DateTime.Now,
                            //                                              monto_antes_cambio =
                            //                                                  balance.papiro_capital,
                            //                                              monto_cambio =
                            //                                                  papiro_capitalnumericUpDown.Value -
                            //                                                  balance.papiro_capital,
                            //                                              descripcion = "Actualizando balance."
                            //                                          });
                            balance.papiro_capital = papiro_capitalnumericUpDown.Value;
                        }

                        if (balance.ingreso != ingresonumericUpDown.Value)
                        {
                            //entities.AddTosubmayor_ingreso(new submayor_ingreso
                            //                                   {
                            //                                       id_usuario = _user.id,
                            //                                       fecha = DateTime.Now,
                            //                                       monto_antes_cambio =
                            //                                           balance.ingreso,
                            //                                       monto_cambio =
                            //                                           ingresonumericUpDown.Value -
                            //                                           balance.ingreso,
                            //                                       descripcion = "Actualizando balance."
                            //                                   });
                            balance.ingreso = ingresonumericUpDown.Value;
                        }
                        balance.utiles_herramientas = 0;
                        balance.activos_fijo_tangible = 0;
                        balance.nominas_pagar = 0;
                        balance.fecha = DateTime.Now;

                        var invariantCulture = CultureInfo.InvariantCulture;

                        entities.AddTobitacora(new bitacora
                                                   {
                                                       id_usuario = _user.id,
                                                       nombre_usuario = _user.login_nombre,
                                                       fecha = DateTime.Now,
                                                       accion_realizada =
                                                           "Actualizado el balance con Efectivo en caja: " +
                                                           balance.efectivo_caja.ToString(invariantCulture) +
                                                           ", Efectivo en banco: " +
                                                           balance.efectivo_banco.ToString(invariantCulture) +
                                                           ", Cuentas por cobrar: " +
                                                           balance.cuentas_por_cobrar.ToString(invariantCulture) +
                                                           ", Cuentas por pagar: " +
                                                           balance.cuentas_por_pagar.ToString(invariantCulture) +
                                                           ", Inventario: " +
                                                           balance.inventario.ToString(invariantCulture) +
                                                           ", Activos fijo tangible: " +
                                                           balance.activos_fijo_tangible.ToString(invariantCulture) +
                                                           ", Gasto: " +
                                                           balance.gasto.ToString(invariantCulture) +
                                                           ", Costo: " +
                                                           balance.costo.ToString(invariantCulture) +
                                                           ", Papiro capital: " +
                                                           balance.papiro_capital.ToString(invariantCulture) +
                                                           ", Ingreso: " +
                                                           balance.ingreso.ToString(invariantCulture)
                                                   });

                        entities.SaveChanges();

                        //creo uno balance nuevo, pues en la bd cuando se carga la primera vez se deben crear dos tuplas
                        //para que funcione el reporte de balance de comprobacion

                        //balance last_balance = entities.balance.ToList().Last();

                        //creo una nueva entidad para el balance
                        balance nuevo_balance = new balance();

                        //lo lleno a mano para evitar errores de datos
                        nuevo_balance.activos_fijo_tangible = balance.activos_fijo_tangible;
                        nuevo_balance.cuentas_por_cobrar = balance.cuentas_por_cobrar;
                        nuevo_balance.cuentas_por_pagar = balance.cuentas_por_pagar;
                        nuevo_balance.efectivo_banco = balance.efectivo_banco;
                        nuevo_balance.efectivo_caja = balance.efectivo_caja;
                        nuevo_balance.fecha = DateTime.Now;
                        nuevo_balance.inventario = balance.inventario;
                        nuevo_balance.papiro_capital = balance.papiro_capital;
                        nuevo_balance.ingreso = balance.ingreso;
                        nuevo_balance.gasto = balance.gasto;
                        nuevo_balance.costo = balance.costo;
                        nuevo_balance.nominas_pagar = balance.nominas_pagar;
                        nuevo_balance.utiles_herramientas = balance.utiles_herramientas;
                        nuevo_balance.cobro_anticipado = balance.cobro_anticipado;


                        entities.AddTobalance(nuevo_balance);
                        entities.SaveChanges();
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                        exception.Message +
                        (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                        @"Balance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Close();
        }

        private void BalanceLoad(object sender, EventArgs e)
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    var balance = entities.balance.ToList().Last();

                    efectivo_cajanumericUpDown.Value = balance.efectivo_caja;
                    efectivo_banconumericUpDown.Value = balance.efectivo_banco;
                    cuentas_por_cobrarnumericUpDown.Value = balance.cuentas_por_cobrar;
                    inventarionumericUpDown.Value = balance.inventario;
                    activos_fijo_tangiblenumericUpDown.Value = balance.activos_fijo_tangible;
                    gastonumericUpDown.Value = balance.gasto;
                    costonumericUpDown.Value = balance.costo;
                    cuentas_por_pagarnumericUpDown.Value = balance.cuentas_por_pagar;
                    papiro_capitalnumericUpDown.Value = balance.papiro_capital;
                    ingresonumericUpDown.Value = balance.ingreso;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Balance", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
