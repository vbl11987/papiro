using System;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class ListarBitacora : Form
    {
        private readonly usuarios _user;
        
        public ListarBitacora(usuarios user)
        {
            InitializeComponent();
            _user = user;
        }

        private void ListarBitDatos()
        {
            // Filtrar de acuerdo a las opciones de filtro seleccionadas.

            using (var entities = new papiro_finalEntities())
            {
                string queryString = "SELECT VALUE b FROM papiro_finalEntities.bitacora AS b ";

                string where = "";

                bool isNotAdmin = !_user.rol.Any(rol => rol.id == 1);

                if (isNotAdmin)
                    where += " and b.id_usuario = @idUser ";

                string accion = filtroAccion.Text.Trim();

                if (accion != "")
                    where += " and SqlServer.UPPER(b.accion_realizada) LIKE @accion ";

                if (where != "")
                    queryString += " WHERE " + where.Remove(0, 4);

                queryString += " order by b.fecha desc";

                ObjectQuery<bitacora> objectQuery = entities.CreateQuery<bitacora>(queryString);

                if (isNotAdmin)
                    objectQuery.Parameters.Add(new ObjectParameter("idUser", _user.id));
                
                if (accion != "")
                    objectQuery.Parameters.Add(
                        new ObjectParameter("accion", "%" + accion.Trim().ToUpper() + "%"));

                // Mostrarlos en el listview. 

                printableLV.Items.Clear();

                foreach (var item in objectQuery.ToList().Select(
                    bit =>
                        new ListViewItem(
                                        new[]
                                            {
                                                bit.nombre_usuario.Trim(),
                                                bit.accion_realizada.Trim(),
                                                bit.fecha.ToString("dd/MM/yyyy h:mm tt"),
                                            })))
                {
                    printableLV.Items.Add(item);
                }
            }
        }

        private void ListarBitacoraLoad(object sender, EventArgs e)
        {
            ListarBitDatos();
        }

        private void FiltrarClick(object sender, EventArgs e)
        {
            ListarBitDatos();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printableLV.Title = "Bitácora del sistema";
            printableLV.PrintPreview();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            printableLV.Title = "Bitácora del sistema";
            printableLV.Print();
        }
    }
}
