using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class TransferirCaja : Form
    {
        private readonly usuarios _user;

        public TransferirCaja(usuarios user)
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
            // Transferir del banco a la caja.

            if (valortransferirnumericUpDown.Value == 0)
            {
                MessageBox.Show(
                    "Debe especificar una cantidad válida para realizar la transferencia.",
                    @"Transferir a la caja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    var balance = entities.balance.ToList().Last();

                    if (balance.efectivo_banco < valortransferirnumericUpDown.Value)
                    {
                        MessageBox.Show(
                            "La cantidad que desea transferir no está disponible en el banco.",
                            @"Transferir a la caja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    entities.AddTosubmayor_efectivo_banco(new submayor_efectivo_banco
                                                              {
                                                                  fecha = DateTime.Now,
                                                                  id_usuario = _user.id,
                                                                  descripcion = "Transferencia del banco a la caja.",
                                                                  saldo =
                                                                      balance.efectivo_banco - valortransferirnumericUpDown.Value,
                                                                  credito = valortransferirnumericUpDown.Value
                                                              });
                    balance.efectivo_banco -= valortransferirnumericUpDown.Value;

                    entities.AddTosubmayor_efectivo_caja(new submayor_efectivo_caja
                                                             {
                                                                 fecha = DateTime.Now,
                                                                 id_usuario = _user.id,
                                                                 descripcion = "Transferencia del banco a la caja.",
                                                                 saldo =
                                                                     balance.efectivo_caja + valortransferirnumericUpDown.Value,
                                                                 debito = valortransferirnumericUpDown.Value
                                                             });
                    balance.efectivo_caja += valortransferirnumericUpDown.Value;

                    entities.AddTobitacora(new bitacora
                                               {
                                                   id_usuario = _user.id,
                                                   nombre_usuario = _user.login_nombre,
                                                   fecha = DateTime.Now,
                                                   accion_realizada =
                                                       "Transferido del banco a la caja: " +
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
                    @"Transferir a la caja", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                              Math.Round(balance.efectivo_banco, 2).ToString(
                                                  CultureInfo.InvariantCulture);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Transferir a la caja", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
