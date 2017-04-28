using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class Gastos : Form
    {
        private readonly usuarios _user;

        private readonly List<string> _cuentas;

        private readonly List<string> _partidas;

        private readonly List<string> _elementos;

        public Gastos(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _cuentas = new List<string>{"(Seleccione)"};
            _partidas = new List<string>();
            _elementos = new List<string>();
        }

        private void ListarGastos()
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    gastosTreeView.Nodes.Clear();
                    existCuentacomboBox.Items.Clear();
                    existCuentacomboBox.Items.Add("(Seleccione)");
                    _cuentas.Clear();
                    _cuentas.Add("(Seleccione)");
                    
                    gastosTreeView.BeginUpdate();

                    foreach (var gasto in entities.gasto)
                    {
                        string[] elements = gasto.id.Split('\\');

                        TreeNode nodeCuenta = gastosTreeView.Nodes.ContainsKey(elements[0])
                                                  ? gastosTreeView.Nodes.Find(elements[0], false)[0]
                                                  : gastosTreeView.Nodes.Add(elements[0],
                                                                             "Cuenta: " + elements[0] + " - " +
                                                                             gasto.nombre_cuenta);

                        TreeNode nodePartida = nodeCuenta.Nodes.ContainsKey(elements[1])
                                                   ? nodeCuenta.Nodes.Find(elements[1], false)[0]
                                                   : nodeCuenta.Nodes.Add(elements[1],
                                                                          elements[1] + " - " +
                                                                          gasto.nombre_partida);

                        TreeNode nodeElement = nodePartida.Nodes.ContainsKey(elements[2])
                                                   ? nodePartida.Nodes.Find(elements[2], false)[0]
                                                   : nodePartida.Nodes.Add(elements[2],
                                                                           elements[2] + " - " + gasto.nombre_elemento +
                                                                           " de $0.0");

                        if (!nodeElement.Nodes.ContainsKey(elements[3]))
                        {
                            nodeElement.Nodes.Add(elements[3],
                                                  gasto.fecha.ToString("dd/MM/yyyy") + " de $" +
                                                  Math.Round(gasto.gasto1, 2).ToString(CultureInfo.InvariantCulture));

                            nodeElement.Text = elements[2] + @" - " + gasto.nombre_elemento + @" de $" +
                                               Math.Round(
                                                   Convert.ToDecimal(
                                                       nodeElement.Text.Substring(nodeElement.Text.LastIndexOf('$') + 1),
                                                       CultureInfo.InvariantCulture) + gasto.gasto1, 2).ToString(
                                                           CultureInfo.InvariantCulture);
                        }

                        if (_cuentas.Contains(elements[0])) continue;
                        _cuentas.Add(elements[0]);
                        existCuentacomboBox.Items.Add(elements[0] + " - " + gasto.nombre_cuenta);
                    }

                    existCuentacomboBox.SelectedIndex = 0;

                    gastosTreeView.EndUpdate();
                    gastosTreeView.ExpandAll();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Gastos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GastosLoad(object sender, EventArgs e)
        {
            ListarGastos();
            
            existCuentaRadioButton.Checked = true;
            existPartidaradioButton.Checked = true;
            nuevoElementradioButton.Checked = true;
        }

        private void SalirbuttonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void ExistCuentaRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            if (!existCuentaRadioButton.Checked) return;

            existCuentacomboBox.Enabled = true;
            numCuentanumericUpDown.Enabled = false;
            nombreCuentatextBox.Enabled = false;

            existPartidaradioButton.Enabled = true;
        }

        private void NuevaCuentaradioButtonCheckedChanged(object sender, EventArgs e)
        {
            if (!nuevaCuentaradioButton.Checked) return;

            existCuentacomboBox.Enabled = false;
            numCuentanumericUpDown.Enabled = true;
            nombreCuentatextBox.Enabled = true;

            nuevaPartidaradioButton.Checked = true;
            existPartidaradioButton.Enabled = false;
        }

        private void ExistPartidaradioButtonCheckedChanged(object sender, EventArgs e)
        {
            if (!existPartidaradioButton.Checked) return;

            existPartidadcomboBox.Enabled = true;
            numeroPartidanumericUpDown.Enabled = false;
            nombrePartidatextBox.Enabled = false;

            existElementradioButton.Enabled = true;
        }

        private void NuevaPartidaradioButtonCheckedChanged(object sender, EventArgs e)
        {
            if (!nuevaPartidaradioButton.Checked) return;

            existPartidadcomboBox.Enabled = false;
            numeroPartidanumericUpDown.Enabled = true;
            nombrePartidatextBox.Enabled = true;

            nuevoElementradioButton.Checked = true;
            existElementradioButton.Enabled = false;
        }

        private void NuevogastobuttonClick(object sender, EventArgs e)
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    // Validaciones
                    string validationMessage = "";

                    string cuenta = null, partida = null, elemento = null;

                    if (existCuentaRadioButton.Checked)
                    {
                        if (existCuentacomboBox.SelectedIndex < 1)
                            validationMessage += "\nDebe seleccionar la cuenta existente.";
                        else
                            cuenta = _cuentas[existCuentacomboBox.SelectedIndex];
                    }
                    else if (nuevaCuentaradioButton.Checked)
                    {
                        bool ok = true;

                        if (_cuentas.Any(c => c == numCuentanumericUpDown.Text.Trim()))
                        {
                            validationMessage += "\nYa existe el número de cuenta especificado.";
                            ok = false;
                        }

                        if (nombreCuentatextBox.Text.Trim() == "")
                        {
                            validationMessage += "\nDebe especificar el nombre de la nueva cuenta.";
                            ok = false;
                        }

                        if (ok) cuenta = numCuentanumericUpDown.Text.Trim();
                    }

                    if (cuenta != null)
                    {
                        if (existPartidaradioButton.Checked)
                        {
                            if (existPartidadcomboBox.SelectedIndex < 1)
                                validationMessage += "\nDebe seleccionar la partida existente.";
                            else
                                partida = _partidas[existPartidadcomboBox.SelectedIndex];
                        }
                        else if (nuevaPartidaradioButton.Checked)
                        {
                            bool ok = true;

                            if (_partidas.Any(p => p == numeroPartidanumericUpDown.Text.Trim()))
                            {
                                validationMessage += "\nYa existe el número de partida para la cuenta especificada.";
                                ok = false;
                            }

                            if (nombrePartidatextBox.Text.Trim() == "")
                            {
                                validationMessage += "\nDebe especificar el nombre de la nueva partida.";
                                ok = false;
                            }

                            if (ok) partida = numeroPartidanumericUpDown.Text.Trim();
                        }
                    }

                    if (cuenta != null && partida != null)
                    {
                        if (existElementradioButton.Checked)
                        {
                            if (existElementcomboBox.SelectedIndex < 1)
                                validationMessage += "\nDebe seleccionar el elemento a actualizar.";
                            else
                                elemento = _elementos[existElementcomboBox.SelectedIndex];
                        }
                        else if (nuevoElementradioButton.Checked)
                        {
                            elemento = numeroElemnetonumericUpDown.Text.Trim();

                            if (efectivoEncomboBox.SelectedIndex < 1)
                                validationMessage +=
                                    "\nDebe especificar si se va a registrar el gasto en el efectivo en banco o en el efectivo en caja";
                        }

                        if (Enumerable.Any(
                                entities.gasto,
                                g => 
                                    g.id == cuenta + "\\" + partida + "\\" + elemento + "\\" + 
                                            fechadateTimePicker.Value.ToString("ddMMyy")))
                        {
                            validationMessage += "\nYa existe el número de gasto '" +
                                                 numeroElemnetonumericUpDown.Text.Trim() + "' en la partida '" +
                                                 partida + "' de la cuenta '" + cuenta + "' en la fecha " +
                                                 fechadateTimePicker.Value.ToString("dd/MM/yyyy") + ".";
                        }

                        if (gastonumericUpDown.Value == 0)
                            validationMessage += "\nDebe especificar un gasto mayor que cero.";
                    }
                    
                    if (validationMessage != "")
                    {
                        MessageBox.Show(validationMessage, @"Gastos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var id = cuenta + "\\" + partida + "\\" + elemento + "\\" +
                             fechadateTimePicker.Value.ToString("ddMMyy");
                    string nombreCuenta = "", nombrePartida = "";

                    if (existCuentaRadioButton.Checked)
                    {
                        foreach (var gasto in entities.gasto)
                        {
                            if (gasto.id.Split('\\')[0] != cuenta) continue;
                            nombreCuenta = gasto.nombre_cuenta;
                            break;
                        }    
                    }
                    else
                    {
                        nombreCuenta = nombreCuentatextBox.Text.Trim();
                    }

                    if (existPartidaradioButton.Checked)
                    {
                        foreach (var gasto in entities.gasto)
                        {
                            string[] elements = gasto.id.Split('\\');
                            if (elements[0] != cuenta || elements[1] != partida) continue;
                            nombrePartida = gasto.nombre_partida;
                            break;
                        }
                    }
                    else
                    {
                        nombrePartida = nombrePartidatextBox.Text.Trim();
                    }

                    entities.AddTogasto(new gasto
                                            {
                                                id = id,
                                                gasto1 = gastonumericUpDown.Value,
                                                fecha = fechadateTimePicker.Value,
                                                descripcion = descripciontextBox.Text.Trim(),
                                                nombre_cuenta = nombreCuenta,
                                                nombre_partida = nombrePartida,
                                                nombre_elemento = nombreElementtextBox.Text.Trim(),
                                                efectivo_en = efectivoEncomboBox.SelectedIndex
                                            });
                    decimal gast = gastonumericUpDown.Value;

                    // Obtener último balance para actualizar.
                    balance lastBalance = entities.balance.ToList().Last();

                    // Rgistrar el gasto en el submayor.
                    switch (efectivoEncomboBox.SelectedIndex)
                    {
                        case 1:
                            if (lastBalance.efectivo_banco - gast < 0)
                            {
                                MessageBox.Show("No hay dinero suficiente en el banco para registrar el gasto",
                                                @"Gastos", MessageBoxButtons.OK,
                                                MessageBoxIcon.Warning);
                                return;
                            }
                            entities.AddTosubmayor_efectivo_banco(new submayor_efectivo_banco
                                                                      {
                                                                          fecha = fechadateTimePicker.Value,
                                                                          id_usuario = _user.id,
                                                                          descripcion =
                                                                              "Se registra gasto con id " + id + ".",
                                                                          saldo =
                                                                              lastBalance.efectivo_banco - gast,
                                                                          credito = gast
                                                                      });
                            lastBalance.efectivo_banco -= gast;
                            break;

                        case 2:
                            if (lastBalance.efectivo_caja - gast < 0)
                            {
                                MessageBox.Show("No hay dinero suficiente en la caja para registrar el gasto",
                                                @"Gastos", MessageBoxButtons.OK,
                                                MessageBoxIcon.Warning);
                                return;
                            }
                            entities.AddTosubmayor_efectivo_caja(new submayor_efectivo_caja
                                                                     {
                                                                         fecha = fechadateTimePicker.Value,
                                                                         id_usuario = _user.id,
                                                                         descripcion =
                                                                             "Se registra gasto con id " + id + ".",
                                                                         saldo = lastBalance.efectivo_caja - gast,
                                                                         credito = gast
                                                                     });
                            lastBalance.efectivo_caja -= gast;
                            break;
                    }

                    entities.AddTosubmayor_gasto(new submayor_gasto
                                                     {
                                                         fecha = fechadateTimePicker.Value,
                                                         id_usuario = _user.id,
                                                         descripcion = "Se registra gasto con id " + id + ".",
                                                         saldo = lastBalance.gasto + gast,
                                                         debito = gast
                                                     });
                    lastBalance.gasto += gast;

                    entities.SaveChanges();

                    //se guarda en bitacora
                    entities.AddTobitacora(new bitacora
                                               {
                                                   id_usuario = _user.id,
                                                   nombre_usuario = _user.login_nombre,
                                                   fecha = DateTime.Now,
                                                   accion_realizada =
                                                       "Registrado nuevo gasto de $" +
                                                       Math.Round(gast, 2).ToString(CultureInfo.InvariantCulture) +
                                                       " con Id: " + id
                                               });
                    entities.SaveChanges();

                    numCuentanumericUpDown.Value = 0;
                    nombreCuentatextBox.Text = "";
                    numeroPartidanumericUpDown.Text = "";
                    nombrePartidatextBox.Text = "";
                    numeroElemnetonumericUpDown.Value = 0;
                    gastonumericUpDown.Value = 0;
                    fechadateTimePicker.Value = DateTime.Now;
                    descripciontextBox.Text = "";
                    efectivoEncomboBox.SelectedIndex = 0;

                    ListarGastos();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Gastos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExistCuentacomboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    existPartidadcomboBox.Items.Clear();
                    existPartidadcomboBox.Items.Add("(Seleccione)");
                    _partidas.Clear();
                    _partidas.Add("(Seleccione)");
                    foreach (var gasto in entities.gasto)
                    {
                        var elements = gasto.id.Split('\\');
                        if (elements[0] != _cuentas[existCuentacomboBox.SelectedIndex]) continue;
                        if (_partidas.Contains(elements[1])) continue;
                        _partidas.Add(elements[1]);
                        existPartidadcomboBox.Items.Add(elements[1] + " - " + gasto.nombre_partida);
                    }
                    existPartidadcomboBox.SelectedIndex = 0;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Gastos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExistElementradioButtonCheckedChanged(object sender, EventArgs e)
        {
            if (!existElementradioButton.Checked) return;

            existElementcomboBox.Enabled = true;
            nombreElementtextBox.Enabled = false;
            numeroElemnetonumericUpDown.Enabled = false;
        }

        private void NuevoElementradioButtonCheckedChanged(object sender, EventArgs e)
        {
            if (!nuevoElementradioButton.Checked) return;

            existElementcomboBox.Enabled = false;
            nombreElementtextBox.Enabled = true;
            numeroElemnetonumericUpDown.Enabled = true;
        }

        private void ExistElementcomboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            gastonumericUpDown.Value = 0;
            
            if (existElementcomboBox.SelectedIndex == 0)
            {
                nombreElementtextBox.Text = "";
                numeroElemnetonumericUpDown.Value = 0;
                descripciontextBox.Text = "";
                fechadateTimePicker.Value = DateTime.Now;
                efectivoEncomboBox.SelectedIndex = 0;
                return;
            }

            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    foreach (var gasto in entities.gasto)
                    {
                        var elements = gasto.id.Split('\\');
                        if (elements[0] != _cuentas[existCuentacomboBox.SelectedIndex]) continue;
                        if (elements[1] != _partidas[existPartidadcomboBox.SelectedIndex]) continue;
                        if (elements[2] != _elementos[existElementcomboBox.SelectedIndex]) continue;
                        nombreElementtextBox.Text = gasto.nombre_elemento;
                        numeroElemnetonumericUpDown.Value = decimal.Parse(elements[2], CultureInfo.InvariantCulture);
                        descripciontextBox.Text = gasto.descripcion;
                        fechadateTimePicker.Value = gasto.fecha;
                        efectivoEncomboBox.SelectedIndex = gasto.efectivo_en;
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Gastos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExistPartidadcomboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    existElementcomboBox.Items.Clear();
                    existElementcomboBox.Items.Add("(Seleccione)");
                    _elementos.Clear();
                    _elementos.Add("(Seleccione)");
                    foreach (var gasto in entities.gasto)
                    {
                        var elements = gasto.id.Split('\\');
                        if (elements[0] != _cuentas[existCuentacomboBox.SelectedIndex]) continue;
                        if (elements[1] != _partidas[existPartidadcomboBox.SelectedIndex]) continue;
                        if (_elementos.Contains(elements[2])) continue;
                        _elementos.Add(elements[2]);
                        existElementcomboBox.Items.Add(elements[2] + " - " + gasto.nombre_elemento);
                    }
                    existElementcomboBox.SelectedIndex = 0;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Gastos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
