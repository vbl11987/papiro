using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Linq;

namespace papiro.formularios
{
    public partial class NominaForm : Form
    {
        private readonly usuarios _user;

        private readonly List<int> _usersId;

        public NominaForm(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _usersId = new List<int>();
        }

        private void Reload()
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    productosDataGridView.Rows.Clear();
                    _usersId.Clear();
                    foreach (var user in entities.usuarios.Where(u => u.nombre != "Sistema" && u.id != 1))
                    {
                        _usersId.Add(user.id);
                        if(user.a_pagar != null && user.salario_extra_operaciones != null)
                        productosDataGridView.Rows.Add(new object[]
                                                           {
                                                               user.nombre, Math.Round(user.salario_fijo, 2),
                                                               Math.Round(user.salario_extra_operaciones.Value, 2),
                                                               0m,
                                                               Math.Round(
                                                                   user.a_pagar.Value + user.salario_extra_operaciones.Value, 2)
                                                           });
                        else if(user.salario_extra_operaciones == null && user.a_pagar != null)
                        {
                            productosDataGridView.Rows.Add(new object[]
                                                           {
                                                               user.nombre, Math.Round(user.salario_fijo, 2),
                                                               0m,
                                                               0m,
                                                               Math.Round(
                                                                   user.a_pagar.Value, 2)
                                                           });
                        }
                        else if(user.a_pagar == null)
                        {
                            MessageBox.Show(
                                "El usuario " + user.nombre +
                                " tiene 0 pesos a pagar, esto puede darse si no tiene asignado un salrio fijo.",
                                "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Nómina", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EntradaProductosFormLoad(object sender, EventArgs e)
        {
            Font font = productosDataGridView.Font;
            productosDataGridView.Font = new Font(font.Name, 10, font.Style, font.Unit);
            productosDataGridView.AutoGenerateColumns = false;

            Reload();
        }

        private void ProductosDataGridViewCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string value = e.FormattedValue == null ? "" : e.FormattedValue.ToString().Trim();
            
            // Validar que el plus sea un número positivo.
            if (productosDataGridView.Columns[e.ColumnIndex].Name == "plusColumn")
            {
                decimal plus;
                if (!decimal.TryParse(value, out plus) || plus < 0)
                {
                    productosDataGridView.Rows[e.RowIndex].ErrorText =
                        "La plus debe ser un número positivo.";
                    e.Cancel = true;
                }
            }
        }

        private void ProductosDataGridViewCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Limpiar la fila de error en caso de que el usuario presione esc.
            productosDataGridView.Rows[e.RowIndex].ErrorText = String.Empty;

            if (productosDataGridView.Columns[e.ColumnIndex].Name == "plusColumn")
            {
                productosDataGridView.Rows[e.RowIndex].Cells["TotalColumn"].Value =
                    Math.Round(
                        Convert.ToDecimal(productosDataGridView.Rows[e.RowIndex].Cells["plusColumn"].Value,
                                          CultureInfo.InvariantCulture), 2) +
                        Convert.ToDecimal(productosDataGridView.Rows[e.RowIndex].Cells["TotalColumn"].Value,
                                          CultureInfo.InvariantCulture);
            }
        }

        private void SalirbuttonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void PagarButtonClick(object sender, EventArgs e)
        {
            if (efectivoEncomboBox.SelectedIndex < 1)
            {
                MessageBox.Show(
                    "Debe especificar si se va a pagar con el efectivo en banco o con el efectivo en caja",
                    @"Nómina", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (MessageBox.Show("Esta acción emitirá el pago. ¿Estás seguro de querer realizarla?",
                                @"Nómina", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (var entities = new papiro_finalEntities())
                    {
                        decimal total = 0;
                        int index = 0;

                        // Obtener último balance para actualizar.
                        balance lastBalance = entities.balance.ToList().Last();

                        foreach (var user in entities.usuarios.Where(u => u.nombre != "Sistema" && u.id != 1))
                        {
                            total += Convert.ToDecimal(productosDataGridView.Rows[index].Cells["TotalColumn"].Value,
                                                       CultureInfo.InvariantCulture);
                            if (Convert.ToDecimal(productosDataGridView.Rows[index].Cells["plusColumn"].Value, CultureInfo.InvariantCulture) != 0)
                            {
                                entities.AddTosubmayor_gasto(new submayor_gasto
                                                                 {
                                                                     fecha = DateTime.Now,
                                                                     id_usuario = _user.id,
                                                                     descripcion =
                                                                         "Se registra el plus del salario.",
                                                                     saldo =
                                                                         lastBalance.gasto +
                                                                         Convert.ToDecimal(
                                                                             productosDataGridView.Rows[index].Cells[
                                                                                 "plusColumn"].Value,
                                                                             CultureInfo.InvariantCulture),
                                                                     debito =
                                                                         Convert.ToDecimal(
                                                                             productosDataGridView.Rows[index].Cells[
                                                                                 "plusColumn"].Value,
                                                                             CultureInfo.InvariantCulture),
                                                                     id_usuario_salario = user.id
                                                                 });
                                lastBalance.gasto +=
                                    Convert.ToDecimal(productosDataGridView.Rows[index].Cells["plusColumn"].Value,
                                                      CultureInfo.InvariantCulture);
                                entities.AddTosubmayor_nomina(new submayor_nomina
                                                                  {
                                                                      fecha = DateTime.Now,
                                                                      id_usuario = _user.id,
                                                                      descripcion = "Se registra el plus del salario.",
                                                                      saldo = lastBalance.nominas_pagar + Convert.ToDecimal(
                                                                             productosDataGridView.Rows[index].Cells[
                                                                                 "plusColumn"].Value,
                                                                             CultureInfo.InvariantCulture),
                                                                      credito = Convert.ToDecimal(
                                                                             productosDataGridView.Rows[index].Cells[
                                                                                 "plusColumn"].Value,
                                                                             CultureInfo.InvariantCulture)
                                                                  });
                                lastBalance.nominas_pagar += Convert.ToDecimal(productosDataGridView.Rows[index].Cells["plusColumn"].Value,
                                                      CultureInfo.InvariantCulture);
                            }
                            if (user.salario_extra_operaciones != 0 && user.salario_extra_operaciones != null)
                            {
                                entities.AddTosubmayor_gasto(new submayor_gasto
                                                                 {
                                                                     fecha = DateTime.Now,
                                                                     id_usuario = _user.id,
                                                                     descripcion =
                                                                         "Se registra gastos de usuarios.",
                                                                     saldo =
                                                                         lastBalance.gasto + user.salario_extra_operaciones,
                                                                     debito = user.salario_extra_operaciones,
                                                                     id_usuario_salario = user.id 
                                                                 });
                                lastBalance.gasto += user.salario_extra_operaciones.Value;

                                entities.AddTosubmayor_nomina(new submayor_nomina
                                                                  {
                                                                      fecha = DateTime.Now,
                                                                      id_usuario = _user.id,
                                                                      descripcion = "Se registra gastos de usuarios.",
                                                                      saldo = lastBalance.nominas_pagar + user.salario_extra_operaciones,
                                                                      credito = user.salario_extra_operaciones
                                                                  });
                                lastBalance.nominas_pagar += user.salario_extra_operaciones;
                            }
                            user.salario_extra_operaciones = 0;
                            user.a_pagar = 0;
                            index++;
                        }

                        switch (efectivoEncomboBox.SelectedIndex)
                        {
                            case 1:
                                if (lastBalance.efectivo_banco - total < 0)
                                {
                                    MessageBox.Show(
                                        "No hay dinero suficiente en el banco para efectuar el pago",
                                        @"Nómina", MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    return;
                                }
                                entities.AddTosubmayor_efectivo_banco(new submayor_efectivo_banco
                                                                          {
                                                                              fecha = DateTime.Now,
                                                                              id_usuario = _user.id,
                                                                              descripcion =
                                                                                  "Se efectúa pago.",
                                                                              saldo =
                                                                                  lastBalance.efectivo_banco - total,
                                                                              credito = total
                                                                          });
                                lastBalance.efectivo_banco -= total;
                                break;

                            case 2:
                                if (lastBalance.efectivo_caja - total < 0)
                                {
                                    MessageBox.Show(
                                        "No hay dinero suficiente en la caja para efectuar el pago",
                                        @"Nómina", MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    return;
                                }
                                entities.AddTosubmayor_efectivo_caja(new submayor_efectivo_caja
                                                                         {
                                                                             fecha = DateTime.Now,
                                                                             id_usuario = _user.id,
                                                                             descripcion = "Se efectúa pago.",
                                                                             saldo =
                                                                                 lastBalance.efectivo_caja - total,
                                                                             credito = total
                                                                         });
                                lastBalance.efectivo_caja -= total;
                                break;
                        }
                        entities.AddTosubmayor_nomina(new submayor_nomina
                                                          {
                                                              fecha = DateTime.Now,
                                                              id_usuario = _user.id,
                                                              descripcion = "Se efectúa pago.",
                                                              saldo = lastBalance.nominas_pagar - total,
                                                              debito = total,
                                                              ultimo_pago = 1
                                                          });
                        lastBalance.nominas_pagar -= total;
                        if (lastBalance.nominas_pagar < 0)
                            lastBalance.nominas_pagar = 0;
                        if (lastBalance.nominas_pagar > 0 && lastBalance.nominas_pagar < 1)
                            lastBalance.nominas_pagar = 0;

                        //se guarda en bitacora
                        var b = new bitacora
                                    {
                                        id_usuario = _user.id,
                                        nombre_usuario = _user.login_nombre,
                                        fecha = DateTime.Now,
                                        accion_realizada =
                                            "Se efectuó un pago de " +
                                            Math.Round(total, 2).ToString(CultureInfo.InvariantCulture)
                                    };

                        entities.AddTobitacora(b);

                        entities.SaveChanges();

                        for (int i = 0; i < productosDataGridView.RowCount; i++ )
                        {
                            productosDataGridView.Rows[i].Cells["comisionColumn"].Value = 0m;
                            productosDataGridView.Rows[i].Cells["plusColumn"].Value = 0m;
                            productosDataGridView.Rows[i].Cells["TotalColumn"].Value =
                            productosDataGridView.Rows[i].Cells["salarioFijoColumn"].Value;
                        }

                        productosDataGridView.Enabled = false;
                        pagarButton.Enabled = false;
                        efectivoEncomboBox.Enabled = false;
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                        exception.Message +
                        (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                        @"Nómina", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }       
            }
        }

        private void ProductosDataGridViewCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1) return;
            
            if (productosDataGridView.Columns[e.ColumnIndex].Name == "operacionesColumn")
            {
                try
                {
                    using (var entities = new papiro_finalEntities())
                    {
                        var user =
                            (usuarios)
                            entities.GetObjectByKey(new EntityKey("papiro_finalEntities.usuarios", "id",
                                                                  _usersId[e.RowIndex]));
                        var detalles = new DetallesOperacionesUsuario(user);
                        detalles.ShowDialog();
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                        exception.Message +
                        (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                        @"Nómina", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }   
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Reload();
        }
    }
}
