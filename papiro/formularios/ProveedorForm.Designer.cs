namespace papiro.formularios
{
    partial class ProveedorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProveedorForm));
            this.existenteRadioButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.proveedorComboBox = new System.Windows.Forms.ComboBox();
            this.nuevoRadioButton = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.nombretextBox = new System.Windows.Forms.TextBox();
            this.aceptarButton = new System.Windows.Forms.Button();
            this.CancelarButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // existenteRadioButton
            // 
            this.existenteRadioButton.AutoSize = true;
            this.existenteRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.existenteRadioButton.Location = new System.Drawing.Point(22, 42);
            this.existenteRadioButton.Name = "existenteRadioButton";
            this.existenteRadioButton.Size = new System.Drawing.Size(80, 20);
            this.existenteRadioButton.TabIndex = 0;
            this.existenteRadioButton.Text = "Existente";
            this.existenteRadioButton.UseVisualStyleBackColor = true;
            this.existenteRadioButton.CheckedChanged += new System.EventHandler(this.ExistenteRadioButtonCheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(234, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Seleccione el proveedor del producto";
            // 
            // proveedorComboBox
            // 
            this.proveedorComboBox.Enabled = false;
            this.proveedorComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.proveedorComboBox.FormattingEnabled = true;
            this.proveedorComboBox.Location = new System.Drawing.Point(104, 41);
            this.proveedorComboBox.Name = "proveedorComboBox";
            this.proveedorComboBox.Size = new System.Drawing.Size(282, 24);
            this.proveedorComboBox.TabIndex = 2;
            // 
            // nuevoRadioButton
            // 
            this.nuevoRadioButton.AutoSize = true;
            this.nuevoRadioButton.Checked = true;
            this.nuevoRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nuevoRadioButton.Location = new System.Drawing.Point(22, 87);
            this.nuevoRadioButton.Name = "nuevoRadioButton";
            this.nuevoRadioButton.Size = new System.Drawing.Size(66, 20);
            this.nuevoRadioButton.TabIndex = 3;
            this.nuevoRadioButton.TabStop = true;
            this.nuevoRadioButton.Text = "Nuevo";
            this.nuevoRadioButton.UseVisualStyleBackColor = true;
            this.nuevoRadioButton.CheckedChanged += new System.EventHandler(this.NuevoRadioButtonCheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(42, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Nombre:";
            // 
            // nombretextBox
            // 
            this.nombretextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nombretextBox.Location = new System.Drawing.Point(104, 116);
            this.nombretextBox.MaxLength = 255;
            this.nombretextBox.Name = "nombretextBox";
            this.nombretextBox.Size = new System.Drawing.Size(282, 22);
            this.nombretextBox.TabIndex = 6;
            // 
            // aceptarButton
            // 
            this.aceptarButton.Image = global::papiro.Properties.Resources.ok;
            this.aceptarButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.aceptarButton.Location = new System.Drawing.Point(157, 172);
            this.aceptarButton.Name = "aceptarButton";
            this.aceptarButton.Size = new System.Drawing.Size(101, 23);
            this.aceptarButton.TabIndex = 8;
            this.aceptarButton.Text = "Aceptar";
            this.aceptarButton.UseVisualStyleBackColor = true;
            this.aceptarButton.Click += new System.EventHandler(this.AceptarButtonClick);
            // 
            // CancelarButton
            // 
            this.CancelarButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelarButton.Image = global::papiro.Properties.Resources.exit;
            this.CancelarButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CancelarButton.Location = new System.Drawing.Point(285, 172);
            this.CancelarButton.Name = "CancelarButton";
            this.CancelarButton.Size = new System.Drawing.Size(101, 23);
            this.CancelarButton.TabIndex = 9;
            this.CancelarButton.Text = "Cancelar";
            this.CancelarButton.UseVisualStyleBackColor = true;
            this.CancelarButton.Click += new System.EventHandler(this.CancelarButtonClick);
            // 
            // ProveedorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelarButton;
            this.ClientSize = new System.Drawing.Size(409, 204);
            this.Controls.Add(this.CancelarButton);
            this.Controls.Add(this.aceptarButton);
            this.Controls.Add(this.nombretextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nuevoRadioButton);
            this.Controls.Add(this.proveedorComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.existenteRadioButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProveedorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Proveedor";
            this.Load += new System.EventHandler(this.ProveedorFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton existenteRadioButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox proveedorComboBox;
        private System.Windows.Forms.RadioButton nuevoRadioButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox nombretextBox;
        private System.Windows.Forms.Button aceptarButton;
        private System.Windows.Forms.Button CancelarButton;
    }
}