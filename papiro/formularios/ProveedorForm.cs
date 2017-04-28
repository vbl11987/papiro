using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class ProveedorForm : Form
    {
        private readonly usuarios _user;
        private int _idProveedor;
        private readonly List<int> _proveedorId;

        public ProveedorForm(usuarios user)
        {
            InitializeComponent();
            _user = user;
            _proveedorId = new List<int>();
        }

        public int IdProveedor
        {
            get { return _idProveedor; }
        }

        private void CancelarButtonClick(object sender, EventArgs e)
        {
            _idProveedor = 0;
            Close();
        }

        private void AceptarButtonClick(object sender, EventArgs e)
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    if (existenteRadioButton.Checked)
                    {
                        if (proveedorComboBox.SelectedIndex < 1)
                        {
                            MessageBox.Show("Debe seleccionar un proveedor.", "Proveedor",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        _idProveedor = _proveedorId[proveedorComboBox.SelectedIndex];
                    }
                    else
                    {
                        // validar nombre.
                        string nombre = nombretextBox.Text.Trim();
                        if (nombre == "" || entities.proveedor.Any(prov => prov.nombre == nombre))
                        {
                            MessageBox.Show("Ya existe un proveedor con el nombre especificado", "Proveedor",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }   
 
                        // Crear el proveedor.
                        var proveedor = new proveedor
                                            {
                                                nombre = nombre,
                                            };
                        entities.AddToproveedor(proveedor);
                        entities.SaveChanges();

                        // Guardar en bitácora.
                        entities.AddTobitacora(new bitacora
                                                   {
                                                       id_usuario = _user.id,
                                                       nombre_usuario = _user.login_nombre,
                                                       fecha = DateTime.Now,
                                                       accion_realizada =
                                                           "Se ha registrado un nuevo Proveedor con Id: " + proveedor.id +
                                                           ", Nombre: " + proveedor.nombre
                                                   });
                        entities.SaveChanges();

                        _idProveedor = proveedor.id;
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Entrada de productos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            Close();
        }

        private void Reload()
        {
            _idProveedor = 0;

            _proveedorId.Clear();
            _proveedorId.Add(-1);
            proveedorComboBox.Items.Clear();
            proveedorComboBox.Items.Add("(Seleccione)");
            proveedorComboBox.SelectedIndex = 0;
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    foreach (var proveedor in entities.proveedor)
                    {
                        _proveedorId.Add(proveedor.id);
                        proveedorComboBox.Items.Add(proveedor.nombre);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Entrada de productos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProveedorFormLoad(object sender, EventArgs e)
        {
            Reload();    
        }

        private void ExistenteRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            if (!existenteRadioButton.Checked) return;

            proveedorComboBox.Enabled = true;
            nuevoRadioButton.Checked = false;
            nombretextBox.Enabled = false;
        }

        private void NuevoRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            if (!nuevoRadioButton.Checked) return;

            nuevoRadioButton.Checked = true;
            nombretextBox.Enabled = true;
            proveedorComboBox.Enabled = false;
        }
    }
}
