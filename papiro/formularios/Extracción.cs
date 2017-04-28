using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class Extraccion : Form
    {
        private readonly usuarios _user;

        public Extraccion(usuarios user)
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
            // Extraer dinero de la caja o el banco.

            if (efectivoEncomboBox.SelectedIndex < 1)
            {
                MessageBox.Show(
                    "Debe especificar si la extracción se va a realizar del efectivo en banco o del efectivo en caja",
                    @"Extracción", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (valortransferirnumericUpDown.Value == 0)
            {
                MessageBox.Show(
                    "Debe especificar una cantidad válida para realizar la extracción.",
                    @"Extracción", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    var balance = entities.balance.ToList().Last();

                    switch (efectivoEncomboBox.SelectedIndex)
                    {
                        case 1: // banco
                            if (balance.efectivo_banco < valortransferirnumericUpDown.Value)
                            {
                                MessageBox.Show(
                                    "La cantidad que desea extraer no está disponible en el banco.",
                                    @"Extracción", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            entities.AddTosubmayor_efectivo_banco(new submayor_efectivo_banco
                                                              {
                                                                  fecha = DateTime.Now,
                                                                  id_usuario = _user.id,
                                                                  descripcion = "Extracción de dinero.",
                                                                  saldo =
                                                                      balance.efectivo_banco - valortransferirnumericUpDown.Value,
                                                                  credito = valortransferirnumericUpDown.Value
                                                              });
                            balance.efectivo_banco -= valortransferirnumericUpDown.Value;
                            balance.papiro_capital -= valortransferirnumericUpDown.Value;
                            break;

                        case 2: // caja
                            if (balance.efectivo_caja < valortransferirnumericUpDown.Value)
                            {
                                MessageBox.Show(
                                    "La cantidad que desea extraer no está disponible en la caja.",
                                    @"Extracción", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            entities.AddTosubmayor_efectivo_caja(new submayor_efectivo_caja
                                                             {
                                                                 fecha = DateTime.Now,
                                                                 id_usuario = _user.id,
                                                                 descripcion = "Extracción de dinero.",
                                                                 saldo =
                                                                     balance.efectivo_caja - valortransferirnumericUpDown.Value,
                                                                 credito = valortransferirnumericUpDown.Value
                                                             });
                            balance.efectivo_caja -= valortransferirnumericUpDown.Value;
                            balance.papiro_capital -= valortransferirnumericUpDown.Value;
                            break;
                    }
                    //Agregado porque cuando se extrae dinero hay que disminiurlo del capital de la empresa
                    entities.AddTosubmayor_papiro_capital(new submayor_papiro_capital
                                                            {
                                                                fecha = DateTime.Now,
                                                                id_usuario = _user.id,
                                                                descripcion = "Extracción de dinero.",
                                                                saldo = balance.papiro_capital - valortransferirnumericUpDown.Value,
                                                                credito = valortransferirnumericUpDown.Value
                                                            });

                    entities.AddTobitacora(new bitacora
                                               {
                                                   id_usuario = _user.id,
                                                   nombre_usuario = _user.login_nombre,
                                                   fecha = DateTime.Now,
                                                   accion_realizada =
                                                       (efectivoEncomboBox.SelectedIndex == 1
                                                            ? "Extraído del banco "
                                                            : "Extraído de la caja ") +
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
                    @"Extracción", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Close();
        }

        private void TransferirBancoLoad(object sender, EventArgs e)
        {
            efectivoenlabel.Text = @": 0.0";
        }

        private void EfectivoEncomboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    var balance = entities.balance.ToList().Last();

                    switch (efectivoEncomboBox.SelectedIndex)
                    {
                        case 1: // banco
                            efectivoenlabel.Text = @": " +
                                                   Math.Round(balance.efectivo_banco, 2).ToString(
                                                       CultureInfo.InvariantCulture);
                            break;

                        case 2: // caja
                            efectivoenlabel.Text = @": " +
                                                   Math.Round(balance.efectivo_caja, 2).ToString(
                                                       CultureInfo.InvariantCulture);
                            break;
                        default:
                            efectivoenlabel.Text = @": 0.0";
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Extracción", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
