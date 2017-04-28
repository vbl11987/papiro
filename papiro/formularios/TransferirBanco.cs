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
    public partial class TransferirBanco : Form
    {
        private readonly usuarios _user;

        public TransferirBanco(usuarios user)
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
            // Transferir de la caja al banco.

            if (valortransferirnumericUpDown.Value == 0)
            {
                MessageBox.Show(
                    "Debe especificar una cantidad válida para realizar la transferencia.",
                    @"Transferir al banco", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    var balance = entities.balance.ToList().Last();

                    if (balance.efectivo_caja < valortransferirnumericUpDown.Value)
                    {
                        MessageBox.Show(
                            "La cantidad que desea transferir no está disponible en la caja.",
                            @"Transferir al banco", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    entities.AddTosubmayor_efectivo_caja(new submayor_efectivo_caja
                                                             {
                                                                 fecha = DateTime.Now,
                                                                 id_usuario = _user.id,
                                                                 descripcion = "Transferencia de la caja al banco.",
                                                                 saldo =
                                                                     balance.efectivo_caja - valortransferirnumericUpDown.Value,
                                                                 credito = valortransferirnumericUpDown.Value
                                                             });
                    balance.efectivo_caja -= valortransferirnumericUpDown.Value;

                    entities.AddTosubmayor_efectivo_banco(new submayor_efectivo_banco
                                                              {
                                                                  fecha = DateTime.Now,
                                                                  id_usuario = _user.id,
                                                                  descripcion = "Transferencia de la caja al banco.",
                                                                  saldo =
                                                                      balance.efectivo_banco + valortransferirnumericUpDown.Value,
                                                                  debito = valortransferirnumericUpDown.Value
                                                              });
                    balance.efectivo_banco += valortransferirnumericUpDown.Value;

                    entities.AddTobitacora(new bitacora
                                               {
                                                   id_usuario = _user.id,
                                                   nombre_usuario = _user.login_nombre,
                                                   fecha = DateTime.Now,
                                                   accion_realizada =
                                                       "Transferido de la caja al banco: " +
                                                       valortransferirnumericUpDown.Value.ToString(
                                                           CultureInfo.InvariantCulture) +
                                                       "$."
                                               });
                    entities.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Transferir al banco", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Close();
        }

        private void TransferirBancoLoad(object sender, EventArgs e)
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    var balance = entities.balance.ToList().Last();

                    EfectivoCajalabel.Text += @" " +
                                              Math.Round(balance.efectivo_caja, 2).ToString(CultureInfo.InvariantCulture);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Transferir al banco", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
