namespace papiro.formularios
{
    partial class Extraccion
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Extraccion));
            this.label1 = new System.Windows.Forms.Label();
            this.valortransferirnumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.EfectivoCajalabel = new System.Windows.Forms.Label();
            this.efectivoEncomboBox = new System.Windows.Forms.ComboBox();
            this.efectivoenlabel = new System.Windows.Forms.Label();
            this.salirButton = new System.Windows.Forms.Button();
            this.guardarButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.valortransferirnumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Cantidad a extraer:";
            // 
            // valortransferirnumericUpDown
            // 
            this.valortransferirnumericUpDown.DecimalPlaces = 2;
            this.valortransferirnumericUpDown.Location = new System.Drawing.Point(110, 60);
            this.valortransferirnumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.valortransferirnumericUpDown.Name = "valortransferirnumericUpDown";
            this.valortransferirnumericUpDown.Size = new System.Drawing.Size(132, 20);
            this.valortransferirnumericUpDown.TabIndex = 0;
            // 
            // EfectivoCajalabel
            // 
            this.EfectivoCajalabel.AutoSize = true;
            this.EfectivoCajalabel.Location = new System.Drawing.Point(12, 26);
            this.EfectivoCajalabel.Name = "EfectivoCajalabel";
            this.EfectivoCajalabel.Size = new System.Drawing.Size(61, 13);
            this.EfectivoCajalabel.TabIndex = 13;
            this.EfectivoCajalabel.Text = "Efectivo en";
            // 
            // efectivoEncomboBox
            // 
            this.efectivoEncomboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.efectivoEncomboBox.FormattingEnabled = true;
            this.efectivoEncomboBox.Items.AddRange(new object[] {
            "(Seleccione)",
            "Banco",
            "Caja"});
            this.efectivoEncomboBox.Location = new System.Drawing.Point(72, 23);
            this.efectivoEncomboBox.Name = "efectivoEncomboBox";
            this.efectivoEncomboBox.Size = new System.Drawing.Size(96, 21);
            this.efectivoEncomboBox.TabIndex = 48;
            this.efectivoEncomboBox.Text = "(Seleccione)";
            this.efectivoEncomboBox.SelectedIndexChanged += new System.EventHandler(this.EfectivoEncomboBoxSelectedIndexChanged);
            // 
            // efectivoenlabel
            // 
            this.efectivoenlabel.AutoSize = true;
            this.efectivoenlabel.Location = new System.Drawing.Point(167, 26);
            this.efectivoenlabel.Name = "efectivoenlabel";
            this.efectivoenlabel.Size = new System.Drawing.Size(10, 13);
            this.efectivoenlabel.TabIndex = 49;
            this.efectivoenlabel.Text = ":";
            // 
            // salirButton
            // 
            this.salirButton.Image = global::papiro.Properties.Resources.exit;
            this.salirButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.salirButton.Location = new System.Drawing.Point(167, 99);
            this.salirButton.Name = "salirButton";
            this.salirButton.Size = new System.Drawing.Size(75, 23);
            this.salirButton.TabIndex = 11;
            this.salirButton.Text = "Cancelar";
            this.salirButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.salirButton.UseVisualStyleBackColor = true;
            this.salirButton.Click += new System.EventHandler(this.SalirButtonClick);
            // 
            // guardarButton
            // 
            this.guardarButton.Image = global::papiro.Properties.Resources.agt_add_to_autorun;
            this.guardarButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.guardarButton.Location = new System.Drawing.Point(70, 99);
            this.guardarButton.Name = "guardarButton";
            this.guardarButton.Size = new System.Drawing.Size(75, 23);
            this.guardarButton.TabIndex = 10;
            this.guardarButton.Text = "Extraer";
            this.guardarButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.guardarButton.UseVisualStyleBackColor = true;
            this.guardarButton.Click += new System.EventHandler(this.GuardarButtonClick);
            // 
            // Extraccion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 133);
            this.Controls.Add(this.efectivoenlabel);
            this.Controls.Add(this.efectivoEncomboBox);
            this.Controls.Add(this.EfectivoCajalabel);
            this.Controls.Add(this.valortransferirnumericUpDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.salirButton);
            this.Controls.Add(this.guardarButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Extraccion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Extracción";
            this.Load += new System.EventHandler(this.TransferirBancoLoad);
            ((System.ComponentModel.ISupportInitialize)(this.valortransferirnumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button guardarButton;
        private System.Windows.Forms.Button salirButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown valortransferirnumericUpDown;
        private System.Windows.Forms.Label EfectivoCajalabel;
        private System.Windows.Forms.ComboBox efectivoEncomboBox;
        private System.Windows.Forms.Label efectivoenlabel;
    }
}