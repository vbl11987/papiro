using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace papiro.formularios
{
    public partial class DetallesOperacionesUsuario : Form
    {
        private readonly usuarios _user;

        public DetallesOperacionesUsuario(usuarios user)
        {
            InitializeComponent();
            _user = user;
        }

        private void Reload()
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    decimal comisionTtal = 0;
                    productosDataGridView.Rows.Clear();
                    foreach (var op in entities.operaciones)
                    {
                        decimal monto = op.monto;
                        if (op.operacion_3ero.Count() != 0)
                            monto -= op.operacion_3ero.Sum(g => g.cantidad * g.valor_operacion_3ero.Value);
                        
                        decimal comiAux = monto*(decimal) _user.por_ciento_operaciones/100;
                        decimal comision = 0;
                        if (op.id_usuario == _user.id)
                            comision += comiAux;
                        if (op.id_comercial == _user.id)
                            comision += comiAux;
                        if (op.id_disenador == _user.id)
                            comision += comiAux;
                        if (op.id_operador == _user.id)
                            comision += comiAux;

                        if (comision == 0) continue;

                        productosDataGridView.Rows.Add(new object[]
                                                           {
                                                               op.descripcion ?? "",
                                                               Math.Round(op.monto, 2),
                                                               Math.Round(comision, 2)
                                                           });
                        comisionTtal += comision;
                    }
                    productosDataGridView.Rows.Add(new object[]
                                                           {
                                                               "Total",
                                                               "",
                                                               Math.Round(comisionTtal, 2)
                                                           });
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Detalles de operaciones del usuario", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EntradaProductosFormLoad(object sender, EventArgs e)
        {
            Font font = productosDataGridView.Font;
            productosDataGridView.Font = new Font(font.Name, 10, font.Style, font.Unit);
            productosDataGridView.AutoGenerateColumns = false;

            Text += @" " + _user.nombre;

            Reload();
        }
    }
}
