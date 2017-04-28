using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using Font = System.Drawing.Font;

namespace papiro.formularios
{
    public partial class MostrarFacturasForm : Form
    {
        private readonly contrato _contract;
        
        public MostrarFacturasForm(contrato contract)
        {
            InitializeComponent();
            _contract = contract;
        }

        private void Reload()
        {
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    facturasDataGridView.Rows.Clear();
                    foreach (var fact in entities.factura)
                    {
                        if (fact.id_contrato != _contract.id) continue;

                        facturasDataGridView.Rows.Add(new object[]
                                                          {
                                                              fact.dir_fich_factura,
                                                              fact.fecha.ToString("dd/MM/yyyy")
                                                          });
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                    exception.Message +
                    (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                    @"Mostrar facturas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EntradaProductosFormLoad(object sender, EventArgs e)
        {
            Font font = facturasDataGridView.Font;
            facturasDataGridView.Font = new Font(font.Name, 10, font.Style, font.Unit);
            facturasDataGridView.AutoGenerateColumns = false;

            Text += @" '" + _contract.dir_fich_contrato + @"'";

            Reload();
        }

        private void SalirbuttonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void ProductosDataGridViewCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 || e.RowIndex == -1) return;
            
            if (facturasDataGridView.Columns[e.ColumnIndex].Name == "operacionesColumn")
            {
                Microsoft.Office.Interop.Excel.Application application = null;
                _Workbook book = null;
                object missing = Type.Missing;
                
                try
                {
                    var fich = (string)facturasDataGridView.Rows[e.RowIndex].Cells["CodigoColumn"].Value + ".xlsx";

                    string path = Assembly.GetExecutingAssembly().CodeBase.Remove(0, 8);
                    path = path.Substring(0, path.LastIndexOf('/') + 1).Replace('/', '\\');

                    if (!File.Exists(path + "Facturas" + "\\" + fich))
                    {
                        MessageBox.Show("No existe el documento de factura solicitado.\n", "Abrir documento de factura",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                    application = new Microsoft.Office.Interop.Excel.Application();

                    book = application.Workbooks.Open(path + "Facturas" + "\\" + fich, missing, missing,
                                                      missing, missing, missing, missing, missing,
                                                      missing, missing, missing, missing, missing, missing,
                                                      missing);
                    application.Visible = true;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(
                        "Ha ocurrido un error en el sistema. Consulte al administrador.\nExcepción: " +
                        exception.Message +
                        (exception.InnerException != null ? "-->" + exception.InnerException.Message : ""),
                        @"Mostrar facturas", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (book != null) book.Close(missing, missing, missing);
                    if (application != null) application.Application.Quit();
                }   
            }
        }
    }
}
