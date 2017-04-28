using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace papiro.Reportes
{
    public partial class reporteCliente : Form
    {
        public reporteCliente()
        {
            InitializeComponent();
        }

        private void reporteCliente_Load(object sender, EventArgs e)
        {
            cbEstatal.Checked = true;
            cbPrivado.Checked = true;
            printableLVClientes.Items.Clear();
            actualizarLista();
        }
        private void actualizarLista()
        {
            try
            {
                using (var _entities = new papiro_finalEntities())
                {
                    string telef;
                    //listo todos los clientes
                    if (cbEstatal.Checked && cbPrivado.Checked || !cbEstatal.Checked && !cbPrivado.Checked)
                    {
                        foreach (var client in _entities.cliente)
                        {
                            if (client.id == 13) continue;
                            telef = "";
                            //busco los telefonos del cliente
                            foreach (var userTelefcliente in client.user_telef_cliente)
                                telef += userTelefcliente.telefonos.telefono + " ";
                            //busco la fecha de vencimineto del contrato del cliente
                            if (_entities.contrato.Where(cont => cont.cliente.id == client.id && cont.terminado == 0).Count() > 0)
                            {
                                contrato c = _entities.contrato.Where(cont => cont.cliente.id == client.id && cont.terminado == 0).Single();
                                //Imprimo en la lista el resultado
                                printableLVClientes.Items.Add(new ListViewItem(new[]
                                                {
                                                   client.nombre,
                                                   client.nombre_contacto,
                                                   client.cargo,
                                                   telef,
                                                   c.fecha_fin.Date.ToString("dd/MM/yyyy")                                                   
                                                }
                                               ));
                            }
                            else
                            {
                                printableLVClientes.Items.Add(new ListViewItem(new[]
                                                {
                                                   client.nombre,
                                                   client.nombre_contacto,
                                                   client.cargo,
                                                   telef,
                                                   "Sin contrato"                                                   
                                                }
                                               ));
                            }
                        }
                    }
                    else if (cbEstatal.Checked && !cbPrivado.Checked)
                    {
                        foreach (var client in _entities.cliente.Where(c => c.privado == 0))
                        {
                            if (client.id == 13) continue;
                            telef = "";
                            //busco los telefonos del cliente
                            foreach (var userTelefcliente in client.user_telef_cliente)
                                telef += userTelefcliente.telefonos.telefono + " ";
                            //busco la fecha de vencimineto del contrato del cliente
                            if (_entities.contrato.Where(cont => cont.cliente.id == client.id && cont.terminado == 0).Count() > 0)
                            {
                                contrato c = _entities.contrato.Where(cont => cont.cliente.id == client.id && cont.terminado == 0).Single();
                                //Imprimo en la lista el resultado
                                printableLVClientes.Items.Add(new ListViewItem(new[]
                                                {
                                                   client.nombre,
                                                   client.nombre_contacto,
                                                   client.cargo,
                                                   telef,
                                                   c.fecha_fin.Date.ToString("dd/MM/yyyy")                                                   
                                                }
                                               ));
                            }
                            else
                            {
                                printableLVClientes.Items.Add(new ListViewItem(new[]
                                                {
                                                   client.nombre,
                                                   client.nombre_contacto,
                                                   client.cargo,
                                                   telef,
                                                   "Sin contrato"                                                   
                                                }
                                               ));
                            }
                        }
                    }
                    else
                    {
                        foreach (var client in _entities.cliente.Where(c => c.privado == 1))
                        {
                            if (client.id == 13) continue;
                            telef = "";
                            //busco los telefonos del cliente
                            foreach (var userTelefcliente in client.user_telef_cliente)
                                telef += userTelefcliente.telefonos.telefono + " ";
                            //busco la fecha de vencimineto del contrato del cliente
                            if (_entities.contrato.Where(cont => cont.cliente.id == client.id && cont.terminado == 0).Count() > 0)
                            {
                                contrato c = _entities.contrato.Where(cont => cont.cliente.id == client.id && cont.terminado == 0).Single();
                                //Imprimo en la lista el resultado
                                printableLVClientes.Items.Add(new ListViewItem(new[]
                                                {
                                                   client.nombre,
                                                   client.nombre_contacto,
                                                   client.cargo,
                                                   telef,
                                                   c.fecha_fin.Date.ToString("dd/MM/yyyy")                                                   
                                                }
                                               ));
                            }
                            else
                            {
                                printableLVClientes.Items.Add(new ListViewItem(new[]
                                                {
                                                   client.nombre,
                                                   client.nombre_contacto,
                                                   client.cargo,
                                                   telef,
                                                   "Sin contrato"                                                   
                                                }
                                               ));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ha ocurrido un error " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void filtrar_Click(object sender, EventArgs e)
        {
            printableLVClientes.Items.Clear();
            actualizarLista();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cbEstatal.Checked && !cbPrivado.Checked)
                printableLVClientes.Title = "Reporte de Clientes Estatales del " + DateTime.Now.ToString("dd/MM/yyyy");
            else if (cbPrivado.Checked && !cbEstatal.Checked)
                printableLVClientes.Title = "Reporte de CLientes Privados del " + DateTime.Now.ToString("dd/MM/yyyy");
            else
                printableLVClientes.Title = "Reporte de Clientes del " + DateTime.Now.ToString("dd/MM/yyyy");
            printableLVClientes.PrintPreview();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (cbEstatal.Checked && !cbPrivado.Checked)
                printableLVClientes.Title = "Reporte de Clientes Estatales del " + DateTime.Now.ToString("dd/MM/yyyy");
            else if (cbPrivado.Checked && !cbEstatal.Checked)
                printableLVClientes.Title = "Reporte de CLientes Privados del " + DateTime.Now.ToString("dd/MM/yyyy");
            else
                printableLVClientes.Title = "Reporte de Clientes del " + DateTime.Now.ToString("dd/MM/yyyy");
            printableLVClientes.Print();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
