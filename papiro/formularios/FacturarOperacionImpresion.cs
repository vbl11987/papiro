using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using Font = System.Drawing.Font;

namespace papiro.formularios
{
    public partial class FacturarOperacionImpresion : Form
    {
        private readonly contrato _contract;
        private readonly string _tipOp;
        private readonly int _cant;
        private readonly string _um;
        private readonly decimal _valorOp;
        private readonly List<int> _operacionesId;
        private bool _facturadaActual;
        private int _idFactura;

        public FacturarOperacionImpresion(contrato contract, string tipOp, int cant, string um, decimal valorOp)
        {
            InitializeComponent();
            _contract = contract;
            _tipOp = tipOp;
            _cant = cant;
            _um = um;
            _valorOp = valorOp;
            _operacionesId = new List<int>();
            _facturadaActual = false;
            _idFactura = 0;
        }

        public bool FacturadaActual
        {
            get { return _facturadaActual; }
        }

        public int IdFactura
        {
            get { return _idFactura; }
        }

        private void Reload()
        {
            opDataGridView.Rows.Clear();
            
            if (opActualradioButton.Checked) return;
            
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    foreach (var op in entities.operaciones)
                    {
                        if (op.id_contrato != _contract.id || op.facturada) continue;

                        opDataGridView.Rows.Add(new object[]
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
                    @"Facturar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EntradaProductosFormLoad(object sender, EventArgs e)
        {
            Font font = opDataGridView.Font;
            opDataGridView.Font = new Font(font.Name, 10, font.Style, font.Unit);
            opDataGridView.AutoGenerateColumns = false;
        }

        private void AceptarbuttonClick(object sender, EventArgs e)
        {
            // Facturar.
            
            Microsoft.Office.Interop.Excel.Application application = null;
            _Workbook book = null;
            object missing = Type.Missing;
            
            try
            {
                using (var entities = new papiro_finalEntities())
                {
                    string path = Assembly.GetExecutingAssembly().CodeBase.Remove(0, 8);
                    path = path.Substring(0, path.LastIndexOf('/') + 1).Replace('/', '\\');

                    if (!File.Exists(path + "PLANTILLA_FACTURA.xlsx"))
                        throw new Exception(
                            "No existe la plantilla a partir de la cual se crea la factura.");

                    application = new Microsoft.Office.Interop.Excel.Application();

                    book = application.Workbooks.Open(path + "PLANTILLA_FACTURA.xlsx", missing, missing,
                                                      missing, missing, missing, missing, missing,
                                                      missing, missing, missing, missing, missing, missing,
                                                      missing);
                    _Worksheet sheet = book.Worksheets[1];

                    int max = entities.factura.Count() == 0
                              ? 0
                              : Enumerable.Max(entities.factura,
                                               f => Convert.ToInt32(f.dir_fich_factura.Substring(0, 3)));
                    string num = (max + 1).ToString(CultureInfo.InvariantCulture);
                    if (num.Length == 1)
                        num = "00" + num;
                    else if (num.Length == 2)
                        num = "0" + num;
                    string fich = num + DateTime.Today.ToString("MMyy", CultureInfo.InvariantCulture);

                    // llenar el sheet.

                    sheet.Cells[7, "C"] = fich;
                    sheet.Cells[8, "C"] = _contract.cliente.nombre ?? "";
                    sheet.Cells[9, "C"] = DateTime.Now.ToString("dd/MM/yyyy");

                    // Tipo de contrato.
                    switch (_contract.id_tipo_contrato)
                    {
                        case 2: // Puntual
                            sheet.Cells[11, "D"] = "X";
                            break;
                        case 1: // Lineal
                            sheet.Cells[11, "F"] = "X";
                            break;
                    }

                    // Tipo de pago.
                    switch (_contract.id_tipo_pago)
                    {
                        case 1:
                            sheet.Cells[12, "D"] = "X"; // puntual
                            break;
                        case 3:
                            sheet.Cells[13, "D"] = "X"; // anticipado
                            break;
                        case 2:
                            sheet.Cells[14, "D"] = "X"; // a credito
                            break;
                    }

                    // Datos de la operación actual.
                    sheet.Cells[17, "B"] = _tipOp;
                    sheet.Cells[17, "C"] = _cant;
                    sheet.Cells[17, "D"] = _um;
                    sheet.Cells[17, "E"] = Math.Round(_valorOp, 2);

                    if (selectlistradioButton.Checked)
                    {
                        var opSeleccionadas = new List<int>();
                        for (int index = 0; index < opDataGridView.RowCount; index++)
                        {
                            if ((bool)opDataGridView.Rows[index].Cells["SeleccioneColumn"].Value)
                                opSeleccionadas.Add(_operacionesId[index]);
                        }

                        if (opSeleccionadas.Count > 0)
                        {
                            int pos = 18;
                            foreach (var op in _contract.operaciones)
                            {
                                if (!opSeleccionadas.Contains(op.id)) continue;

                                op.facturada = true;

                                // Obtener el producto por su tipo.
                                if (op.id_tipo_producto == null || op.id_tipo_producto == 0)
                                    continue;
                                int idTipProd = op.id_tipo_producto.Value;
                                producto producto =
                                    entities.producto.Where(prod => prod.id_tipo_producto == idTipProd).SingleOrDefault();

                                sheet.Cells[pos, "B"] = op.tipo_operacion.valor;
                                sheet.Cells[pos, "C"] = op.cantidad;
                                sheet.Cells[pos, "D"] = producto.unidad_medida.siglas;
                                sheet.Cells[pos, "E"] = Math.Round(op.valor_tipo_operacion.Value, 2);
                                pos++;
                            }
                        }
                    }

                    if (!Directory.Exists(path + "Facturas"))
                        Directory.CreateDirectory(path + "Facturas");

                    book.SaveAs(path + "Facturas" + "\\" + fich);

                    var fact = new factura
                                   {
                                       id_contrato = _contract.id,
                                       fecha = DateTime.Now,
                                       dir_fich_factura = fich
                                   };

                    entities.AddTofactura(fact);

                    entities.SaveChanges();

                    application.Visible = true;

                    _facturadaActual = true;

                    _idFactura = fact.id;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("No se ha podido llevar a cabo la operación porque ha ocurrido un error en el sistema.\n"
                                + @"Excepción: " + exception.Message,
                                "Facturar",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (book != null) book.Close(missing, missing, missing);
                if (application != null) application.Application.Quit();
            }

            Close();
        }

        private void Cancelbutton1Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OpActualradioButtonCheckedChanged(object sender, EventArgs e)
        {
            Reload();
            
            if (!opActualradioButton.Checked) return;

            selectlistradioButton.Checked = false;
            opDataGridView.Enabled = false;
        }

        private void SelectlistradioButtonCheckedChanged(object sender, EventArgs e)
        {
            Reload();
            
            if (!selectlistradioButton.Checked) return;

            opActualradioButton.Checked = false;
            opDataGridView.Enabled = true;
        }
    }
}
