using System;
using System.Linq;
using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void Button2Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Button1Click(object sender, EventArgs e)
        {
            //creo el contexto de trabajo con las entidades generadas desde la base de datos
            var entidad = new papiro_finalEntities();
            
            try
            {
                //busco si el usuario y pass existen en la base de datos para confirmar la autenticacion
                usuarios loginUser =
                    entidad.usuarios.Where(
                        u => u.activado && u.login_nombre == user.Text.Trim() && u.pass == pass.Text.Trim()).
                        SingleOrDefault();
                
                //compruebo que el usuario no sea null, de ser distinto entonces guardo en bitacora la accion 
                // de entrada al sistema por ese usuario)
                if (loginUser != null && loginUser.login_nombre != "Sistema")
                {
                    //se guarda en bitacora
                    var b = new bitacora
                                {
                                    id_usuario = loginUser.id,
                                    nombre_usuario = loginUser.login_nombre,
                                    fecha = DateTime.Now,
                                    accion_realizada = "El usuario se ha autenticado en el sistema"
                                };

                    entidad.AddTobitacora(b);
                    entidad.SaveChanges();

                    pass.Text = "";

                    // Escondo el formulario de autenticación.
                    Hide();

                    // Muestra el formulario principal.
                    var formularioPrincipal = new FormularioPrincipal(loginUser, this);
                    formularioPrincipal.Show();
                }
                else
                {
                    //creo el contexto de trabajo con las entidades generadas desde la base de datos
                    var entity = new papiro_finalEntities();

                    var sistId = (from u in entity.usuarios
                                  where u.login_nombre == "Sistema"
                                  select u.id).SingleOrDefault();

                    //se guarda en bitacora
                    var b = new bitacora
                                {
                                    id_usuario = sistId,
                                    nombre_usuario = "Sistema",
                                    fecha = DateTime.Now,
                                    accion_realizada =
                                        "El usuario '" + user.Text.Trim() + "' o la contraseña son incorrectos"
                                };
                    entidad.AddTobitacora(b);
                    entidad.SaveChanges();

                    MessageBox.Show(@"El usuario o la contraseña son incorrectos", @"Autenticación incorrecta", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch
            {
                MessageBox.Show(@"Error en el sistema.", @"Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
