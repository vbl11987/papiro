using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace papiro.formularios
{
    public partial class SeleccionOperaciones : Form
    {
        private readonly contrato _contract;
        private readonly List<int> _operacionesId;
        private readonly List<int> _opSeleccionadas;

        public SeleccionOperaciones(contrato contract)
        {
            InitializeComponent();
            _contract = contract;
            _opSeleccionadas = new List<int>();
            _operacionesId = new List<int>();
        }

        public List<int> OpSeleccionadas
        {
            get { return _opSeleccionadas; }
        }

        private void Reload()
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    productosDataGridView.Rows.Clear();
                    foreach (var op in entities.operaciones)
                    {
                        if (op.id_contrato != _contract.id || op.facturada) continue;

                        productosDataGridView.Rows.Add(new object[]
                                                           {
                                                               true,
                                                               op.descripcion,
                                                               op.tipo_operacion != null ? op.tipo_operacion.valor : "",
                                                               op.cliente != null ? op.cliente.nombre : "",
                                                               Math.Round(op.monto, 2),
                                                               op.fecha.ToString("dd/MM/yyyy")
                                                           });
                        _operacionesId.Add(op.id);
                    }
                    
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Selección de operaciones", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EntradaProductosFormLoad(object sender, EventArgs e)
        {
            Font font = productosDataGridView.Font;
            productosDataGridView.Font = new Font(font.Name, 10, font.Style, font.Unit);
            productosDataGridView.AutoGenerateColumns = false;

            Reload();
        }

        private void AceptarbuttonClick(object sender, EventArgs e)
        {
            if (!productosDataGridView.Rows.Cast<DataGridViewRow>().Any(
                r => (bool) r.Cells["SeleccioneColumn"].Value))
            {
                MessageBox.Show(
                    "Debe seleccionar al menos una operación.", @"Selección de operaciones", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            for (int index = 0; index < productosDataGridView.RowCount; index++)
            {
                if ((bool) productosDataGridView.Rows[index].Cells["SeleccioneColumn"].Value)
                    _opSeleccionadas.Add(_operacionesId[index]);
            }

            Close();
        }

        private void Cancelbutton1Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
