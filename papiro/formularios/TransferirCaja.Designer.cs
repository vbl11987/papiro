namespace papiro.formularios
{
    partial class TransferirCaja
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransferirCaja));
            this.guardarButton = new System.Windows.Forms.Button();
            this.salirButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.valortransferirnumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.EfectivoCajalabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.valortransferirnumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // guardarButton
            // 
            this.guardarButton.Image = global::papiro.Properties.Resources.forward;
            this.guardarButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.guardarButton.Location = new System.Drawing.Point(70, 85);
            this.guardarButton.Name = "guardarButton";
            this.guardarButton.Size = new System.Drawing.Size(75, 23);
            this.guardarButton.TabIndex = 10;
            this.guardarButton.Text = "Transferir";
            this.guardarButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.guardarButton.UseVisualStyleBackColor = true;
            this.guardarButton.Click += new System.EventHandler(this.GuardarButtonClick);
            // 
            // salirButton
            // 
            this.salirButton.Image = global::papiro.Properties.Resources.exit;
            this.salirButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.salirButton.Location = new System.Drawing.Point(167, 85);
            this.salirButton.Name = "salirButton";
            this.salirButton.Size = new System.Drawing.Size(75, 23);
            this.salirButton.TabIndex = 11;
            this.salirButton.Text = "Cancelar";
            this.salirButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.salirButton.UseVisualStyleBackColor = true;
            this.salirButton.Click += new System.EventHandler(this.SalirButtonClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Cantidad a transferir:";
            // 
            // valortransferirnumericUpDown
            // 
            this.valortransferirnumericUpDown.DecimalPlaces = 2;
            this.valortransferirnumericUpDown.Location = new System.Drawing.Point(122, 48);
            this.valortransferirnumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.valortransferirnumericUpDown.Name = "valortransferirnumericUpDown";
            this.valortransferirnumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.valortransferirnumericUpDown.TabIndex = 0;
            // 
            // EfectivoCajalabel
            // 
            this.EfectivoCajalabel.AutoSize = true;
            this.EfectivoCajalabel.Location = new System.Drawing.Point(12, 20);
            this.EfectivoCajalabel.Name = "EfectivoCajalabel";
            this.EfectivoCajalabel.Size = new System.Drawing.Size(97, 13);
            this.EfectivoCajalabel.TabIndex = 13;
            this.EfectivoCajalabel.Text = "Efectivo en banco:";
            // 
            // TransferirCaja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 120);
            this.Controls.Add(this.EfectivoCajalabel);
            this.Controls.Add(this.valortransferirnumericUpDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.salirButton);
            this.Controls.Add(this.guardarButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TransferirCaja";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Transferencia a la caja";
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
    }
}