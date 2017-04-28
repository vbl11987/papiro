using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class CobrosAnticipados : Form
    {
        private readonly papiro_finalEntities _entities;
        private readonly usuarios _user;
        private List<int> _idClientes;
        private List<int> _forma_pago;
        public CobrosAnticipados(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _entities = new papiro_finalEntities();
            _forma_pago = new List<int>();
            _idClientes = new List<int>();
        }

        private void CobrosAnticipados_Load(object sender, EventArgs e)
        {
            this.Reload();
        }
        private void Reload()
        {
            try
            {
                nCantidad.Value = 0;
                tbDescripcion.Text = "";


                pagoEfectivoComboBox.Items.Clear();
                pagoEfectivoComboBox.Items.Add("<Seleccione>");
                _forma_pago.Add(0);
                foreach (var fp in _entities.forma_pago_efectivo)
                {
                    pagoEfectivoComboBox.Items.Add(fp.descripcion);
                    _forma_pago.Add(fp.id);
                }
                pagoEfectivoComboBox.SelectedIndex = 0;

                cb_cliente.Items.Clear();
                cb_cliente.Items.Add("<Seleccione>");
                _idClientes.Add(0);
                foreach (var c in _entities.cliente.Where(cl => cl.id != 13))
                {
                    cb_cliente.Items.Add(c.nombre);
                    _idClientes.Add(c.id);
                }
                cb_cliente.SelectedIndex = 0;
                rBParticular.Checked = false;
                rBEstatal.Checked = false;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void rBEstatal_CheckedChanged(object sender, EventArgs e)
        {
            cb_cliente.Items.Clear();
            cb_cliente.Items.Add("<Seleccione>");
            _idClientes.Add(0);
            foreach (var c in _entities.cliente.Where(cl => cl.id != 13 && cl.privado == 0))
            {
                cb_cliente.Items.Add(c.nombre);
                _idClientes.Add(c.id);
            }
            cb_cliente.SelectedIndex = 0;
        }

        private void rBParticular_CheckedChanged(object sender, EventArgs e)
        {
            cb_cliente.Items.Clear();
            cb_cliente.Items.Add("<Seleccione>");
            _idClientes.Add(0);
            foreach (var c in _entities.cliente.Where(cl => cl.id != 13 && cl.privado == 1))
            {
                cb_cliente.Items.Add(c.nombre);
                _idClientes.Add(c.id);
            }
            cb_cliente.SelectedIndex = 0;
        }

        private void limpiar_Click(object sender, EventArgs e)
        {
            this.Reload();
        }

        private void cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aceptar_Click(object sender, EventArgs e)
        {
            try
            {
                var validacion = ValidarDatos();
                if(!validacion.Equals(""))
                {
                    MessageBox.Show(validacion, "Error en la entrada de datos", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    var aux = _idClientes[cb_cliente.SelectedIndex];
                    if(_entities.cobro_anticipado.Any(ca => ca.id_cliente == aux))
                    {
                        balance ultimo_balan = _entities.balance.ToList().Last();
                        decimal valor = nCantidad.Value;
                        if (_forma_pago[pagoEfectivoComboBox.SelectedIndex] == 1)
                        {

                            _entities.AddTosubmayor_cobro_anticipado(new submayor_cobro_anticipado
                            {
                                fecha = DateTime.Now,
                                id_usuario = _user.id,
                                descripcion = "Se agrega cobro anticipado",
                                saldo = ultimo_balan.cobro_anticipado + valor,
                                credito = valor
                            });
                            ultimo_balan.cobro_anticipado += valor;
                            _entities.AddTosubmayor_efectivo_caja(new submayor_efectivo_caja
                            {
                                fecha = DateTime.Now,
                                id_usuario = _user.id,
                                descripcion = "Se agrega cobro anticipado",
                                saldo = ultimo_balan.efectivo_caja + valor,
                                debito = valor
                            });
                            ultimo_balan.efectivo_caja += valor;
                            var cobro = _entities.cobro_anticipado.Where(can => can.id_cliente == aux).Single();
                            cobro.cantidad += valor;
                            _entities.AddTobitacora(new bitacora
                            {
                                id_usuario = _user.id,
                                accion_realizada = "Se agrega un cobro anticipado",
                                fecha = DateTime.Now,
                                nombre_usuario = _user.nombre
                            });
                            ultimo_balan.cobro_anticipado += valor;
                            _entities.SaveChanges();
                            this.Reload();
                        }
                        else
                        {
                            _entities.AddTosubmayor_cobro_anticipado(new submayor_cobro_anticipado
                            {
                                fecha = DateTime.Now,
                                id_usuario = _user.id,
                                descripcion = "Se agrega cobro anticipado",
                                saldo = ultimo_balan.cobro_anticipado + valor,
                                credito = valor
                            });
                            ultimo_balan.cobro_anticipado += valor;
                            _entities.AddTosubmayor_efectivo_banco(new submayor_efectivo_banco()
                            {
                                fecha = DateTime.Now,
                                id_usuario = _user.id,
                                descripcion = "Se agrega cobro anticipado",
                                saldo = ultimo_balan.efectivo_caja + valor,
                                debito = valor
                            });
                            ultimo_balan.efectivo_banco += valor;
                            ultimo_balan.efectivo_caja += valor;
                            var cobro = _entities.cobro_anticipado.Where(can => can.id_cliente == aux).Single();
                            _entities.AddTobitacora(new bitacora
                            {
                                id_usuario = _user.id,
                                accion_realizada = "Se agrega cobro anticipado",
                                fecha = DateTime.Now,
                                nombre_usuario = _user.nombre
                            });
                            ultimo_balan.cobro_anticipado += valor;
                            _entities.SaveChanges();
                            this.Reload();
                        }
                    }
                    else
                    {
                        balance ultimo_balan = _entities.balance.ToList().Last();
                        decimal valor = nCantidad.Value;
                        if(_forma_pago[pagoEfectivoComboBox.SelectedIndex] == 1)
                        {
                        
                            _entities.AddTosubmayor_cobro_anticipado(new submayor_cobro_anticipado
                                                                         {
                                                                             fecha = DateTime.Now,
                                                                             id_usuario = _user.id,
                                                                             descripcion = "Se registra cobro anticipado",
                                                                             saldo = ultimo_balan.cobro_anticipado + valor,
                                                                             credito = valor
                                                                         });
                            ultimo_balan.cobro_anticipado += valor;
                            _entities.AddTosubmayor_efectivo_caja(new submayor_efectivo_caja
                                                                      {
                                                                          fecha = DateTime.Now,
                                                                          id_usuario = _user.id,
                                                                          descripcion = "Se registra cobro anticipado",
                                                                          saldo = ultimo_balan.efectivo_caja + valor,
                                                                          debito = valor
                                                                      });
                            ultimo_balan.efectivo_caja += valor;
                            _entities.AddTocobro_anticipado(new cobro_anticipado
                                                                {
                                                                    cantidad = valor,
                                                                    descripcion = tbDescripcion.Text,
                                                                    id_cliente = _idClientes[cb_cliente.SelectedIndex],
                                                                    tipo_cobro = "Efectivo en caja",
                                                                    fecha = DateTime.Now
                                                                });
                            _entities.AddTobitacora(new bitacora
                                                        {
                                                            id_usuario = _user.id,
                                                            accion_realizada = "Se registra cobro anticipado",
                                                            fecha = DateTime.Now,
                                                            nombre_usuario = _user.nombre
                                                        });
                            ultimo_balan.cobro_anticipado += valor;
                            _entities.SaveChanges();
                            this.Reload();
                        }
                        else
                        {
                            _entities.AddTosubmayor_cobro_anticipado(new submayor_cobro_anticipado
                            {
                                fecha = DateTime.Now,
                                id_usuario = _user.id,
                                descripcion = "Se registra cobro anticipado",
                                saldo = ultimo_balan.cobro_anticipado + valor,
                                credito = valor
                            });
                            ultimo_balan.cobro_anticipado += valor;
                            _entities.AddTosubmayor_efectivo_banco(new submayor_efectivo_banco()
                            {
                                fecha = DateTime.Now,
                                id_usuario = _user.id,
                                descripcion = "Se registra cobro anticipado",
                                saldo = ultimo_balan.efectivo_caja + valor,
                                debito = valor
                            });
                            ultimo_balan.efectivo_banco += valor;
                            _entities.AddTocobro_anticipado(new cobro_anticipado
                            {
                                cantidad = valor,
                                descripcion = tbDescripcion.Text,
                                id_cliente = _idClientes[cb_cliente.SelectedIndex],
                                tipo_cobro = "Efectivo en banco",
                                fecha = DateTime.Now
                            });
                            _entities.AddTobitacora(new bitacora
                            {
                                id_usuario = _user.id,
                                accion_realizada = "Se registra cobro anticipado",
                                fecha = DateTime.Now,
                                nombre_usuario = _user.nombre
                            });
                            ultimo_balan.cobro_anticipado += valor;
                            _entities.SaveChanges();
                            this.Reload();
                        }
                    }
                    
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        private string ValidarDatos()
        {
            string message = "";
            if (cb_cliente.SelectedIndex == 0)
                message = "Debe seleccionar un cliente";
            if (pagoEfectivoComboBox.SelectedIndex == 0)
                message += "\nDebe escoger el destino donde depositar el efectivo";
            if (nCantidad.Value == 0)
                message += "\nDebe insertar la cantidad de efectivo a depositar";
            return message;
        }
    }
}
