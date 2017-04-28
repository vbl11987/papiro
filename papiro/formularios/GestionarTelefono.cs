using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class GestionarTelefono : Form
    {
        private readonly List<int> _tipoTelId;

        private readonly bool _isEdicion;

        private telefonos _telefono;

        public GestionarTelefono(bool edicion = false, telefonos telefono = null)
        {
            InitializeComponent();
            _tipoTelId = new List<int> { -1 };
            _isEdicion = edicion;
            _telefono = telefono;
        }

        public telefonos TelefonoSeleccionado
        {
            get { return _telefono; }
        }

        private void GestionarTelefonoLoad(object sender, EventArgs e)
        {
            int idTipoTel = -1;
            
            if (_isEdicion)
            {
                Text = @"Modificar teléfono";
                telef.Text = _telefono.telefono;
                idTipoTel = _telefono.id_tipo_telefono;
            }
            else
                Text = @"Agregar teléfono";

            //se obtienen los campos para llenar el combo de los tipos de telefonos

            tipo_telef.Text = @"(Tipo de teléfono)";
            tipo_telef.Items.Add("(Tipo de teléfono)");

            using (var entities = new papiro_finalEntities())
            {
                foreach (var tipoTelefono in entities.tipo_telefono)
                {
                    _tipoTelId.Add(tipoTelefono.id);
                    tipo_telef.Items.Add(tipoTelefono.valor);
                    if (idTipoTel == tipoTelefono.id) tipo_telef.SelectedIndex = tipo_telef.Items.Count - 1;
                }
            }
        }

        private void AceptarClick(object sender, EventArgs e)
        {
            string validationMessage = "";

            if (telef.Text == "")
                validationMessage = "El número de teléfono no puede estar vacío.";

            if (tipo_telef.SelectedIndex < 1)
                validationMessage += "\nDebe seleccionar el tipo de teléfono.";

            if (validationMessage != "")
            {
                MessageBox.Show(validationMessage, @"Gestionar usuario", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            if (_isEdicion)
            {
                _telefono.telefono = telef.Text.Trim();
                _telefono.id_tipo_telefono = _tipoTelId[tipo_telef.SelectedIndex];
            }
            else
            {
                _telefono = new telefonos
                                {
                                    telefono = telef.Text.Trim(),
                                    id_tipo_telefono = _tipoTelId[tipo_telef.SelectedIndex],
                                };
            }

            Close();
        }
    }
}
