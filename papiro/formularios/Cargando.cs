using System.Windows.Forms;

namespace papiro.formularios
{
    public partial class CargandoForm : Form
    {
        public CargandoForm()
        {
            InitializeComponent();
        }

        public void SetMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
                MessageLabel.Text = message;
        }

        public void SetValue(int value)
        {
            if (value < 0 || value > 100) return;
            progressBar.Value = value;
            Text = string.Format("Cargando [{0}%]", value);
        }

        public void Reset()
        {
            MessageLabel.Text = "";
            progressBar.Value = 0;
            Text = @"Cargando [0%]";
        }
    }
}
