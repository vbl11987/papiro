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
    public partial class CierreCuentasNominales : Form
    {
        private usuarios _user;
        public CierreCuentasNominales(usuarios u)
        {
            InitializeComponent();
            this._user = u;
        }

        private void CierreCuentasNominales_Load(object sender, EventArgs e)
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    balance balan = entities.balance.ToList().Last();
                    label1.Text = Math.Round(balan.gasto, 2).ToString(CultureInfo.InvariantCulture);
                    label2.Text = Math.Round(balan.costo, 2).ToString(CultureInfo.InvariantCulture);
                    label3.Text = Math.Round(balan.ingreso, 2).ToString(CultureInfo.InvariantCulture);
                    label4.Text = Math.Round((balan.ingreso - (balan.gasto + balan.costo)), 2).ToString(CultureInfo.InvariantCulture);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void filtrar_Click(object sender, EventArgs e)
        {
            EstadoResultadosVerDetallesGastos gastos = new EstadoResultadosVerDetallesGastos(DateTime.Now, DateTime.Now);
            gastos.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EstadoResultadosVerDetalles x = new EstadoResultadosVerDetalles(DateTime.Now, DateTime.Now);
            x.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EstadoResultadosVerDetallesIngresos i = new EstadoResultadosVerDetallesIngresos(DateTime.Now, DateTime.Now);
            i.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    balance balan = entities.balance.ToList().Last();

                    if(balan.nominas_pagar != 0 || !(balan.nominas_pagar > 0 && balan.nominas_pagar < 1))
                    {
                        MessageBox.Show(
                            "Debe realizar cerrar las nóminas pendientes a pago antes de poder realizar el cierre de las cuentas nominales",
                            "Cierre de cuentas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        //creo un nuevo balance
                        balance aux = new balance();
                        aux.efectivo_caja = balan.efectivo_caja;
                        aux.efectivo_banco = balan.efectivo_banco;
                        aux.cuentas_por_cobrar = balan.cuentas_por_cobrar;
                        aux.inventario = balan.inventario;
                        aux.activos_fijo_tangible = balan.activos_fijo_tangible;
                        aux.costo = 0;
                        aux.gasto = 0;
                        aux.cuentas_por_pagar = balan.cuentas_por_pagar;
                        aux.papiro_capital = balan.papiro_capital;
                        aux.ingreso = 0;
                        aux.fecha = null;
                        aux.utiles_herramientas = balan.utiles_herramientas;
                        aux.nominas_pagar = 0;
                        aux.cobro_anticipado = balan.cobro_anticipado;

                        //luego le sumo o resto la utilidad
                        decimal x = balan.gasto + balan.costo;
                        decimal y = balan.ingreso;
                       
                        y -= x;
                        aux.papiro_capital += y;

                        //Registro el gasto para cerrar la cuenta
                        entities.AddTosubmayor_gasto(new submayor_gasto
                        {
                            fecha = DateTime.Now,
                            id_usuario = _user.id,
                            descripcion = "Cierre de cuentas nominales",
                            saldo = 0,
                            credito = balan.gasto
                        });
                        //Registro el costo para cerrar la cuenta
                        entities.AddTosubmayor_costo(new submayor_costo
                                                         {
                                                             fecha =  DateTime.Now,
                                                             id_usuario = _user.id,
                                                             descripcion = "Cierre de cuentas nominales",
                                                             saldo = 0,
                                                             credito = balan.costo
                                                         });
                        //Registro el ingreso para cerrar la cuenta
                        entities.AddTosubmayor_ingreso(new submayor_ingreso
                                                           {
                                                               fecha = DateTime.Now,
                                                               id_usuario = _user.id,
                                                               descripcion = "Cierre de cuentas nominales",
                                                               saldo = 0,
                                                               debito = balan.ingreso
                                                           });
                        //Registro la utilidad en papiro capital
                        if(y > 0)
                        {
                            entities.AddTosubmayor_papiro_capital(new submayor_papiro_capital
                            {
                                fecha = DateTime.Now,
                                id_usuario = _user.id,
                                descripcion = "Cierre de cuentas nominales",
                                saldo = balan.papiro_capital + y,
                                debito = y
                            });
                        }
                        else
                        {
                            y = -y;
                            entities.AddTosubmayor_papiro_capital(new submayor_papiro_capital
                            {
                                fecha = DateTime.Now,
                                id_usuario = _user.id,
                                descripcion = "Cierre de cuentas nominales",
                                saldo = balan.papiro_capital + y,
                                credito = y
                            });
                        }
                        

                        //balan.papiro_capital = aux.papiro_capital;
                        //balan.ingreso = 0;
                        //balan.gasto = 0;
                        //balan.costo = 0;
                        entities.AddTobalance(aux);
                        entities.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            this.Close();
        }
    }
}
