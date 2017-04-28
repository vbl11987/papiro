namespace papiro.formularios
{
    partial class OperacionDisenno
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OperacionDisenno));
            this.label2 = new System.Windows.Forms.Label();
            this.cb_tipo_operacion = new System.Windows.Forms.ComboBox();
            this.S = new System.Windows.Forms.Button();
            this.cb_tipo_producto = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.addclientbutton = new System.Windows.Forms.Button();
            this.prioridadLabel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cb_cliente = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.addbutton = new System.Windows.Forms.Button();
            this.salirbutton = new System.Windows.Forms.Button();
            this.descripciontextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.propioRadioButton = new System.Windows.Forms.RadioButton();
            this.terceroRadioButton2 = new System.Windows.Forms.RadioButton();
            this.precionumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.contratotextBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.precionumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(42, 217);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 16);
            this.label2.TabIndex = 22;
            this.label2.Text = "Tipo de Operación:";
            // 
            // cb_tipo_operacion
            // 
            this.cb_tipo_operacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_tipo_operacion.FormattingEnabled = true;
            this.cb_tipo_operacion.Items.AddRange(new object[] {
            "(Seleccione)"});
            this.cb_tipo_operacion.Location = new System.Drawing.Point(45, 239);
            this.cb_tipo_operacion.Name = "cb_tipo_operacion";
            this.cb_tipo_operacion.Size = new System.Drawing.Size(179, 24);
            this.cb_tipo_operacion.TabIndex = 2;
            this.cb_tipo_operacion.Text = "(Seleccione)";
            // 
            // S
            // 
            this.S.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.S.Image = global::papiro.Properties.Resources.ok;
            this.S.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.S.Location = new System.Drawing.Point(88, 462);
            this.S.Name = "S";
            this.S.Size = new System.Drawing.Size(151, 26);
            this.S.TabIndex = 15;
            this.S.Text = "Registrar operación";
            this.S.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.S.UseVisualStyleBackColor = true;
            this.S.Click += new System.EventHandler(this.AceptarClick);
            // 
            // cb_tipo_producto
            // 
            this.cb_tipo_producto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_tipo_producto.FormattingEnabled = true;
            this.cb_tipo_producto.Items.AddRange(new object[] {
            "(Seleccione)"});
            this.cb_tipo_producto.Location = new System.Drawing.Point(263, 239);
            this.cb_tipo_producto.Name = "cb_tipo_producto";
            this.cb_tipo_producto.Size = new System.Drawing.Size(179, 24);
            this.cb_tipo_producto.TabIndex = 13;
            this.cb_tipo_producto.Text = "(Seleccione)";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.addclientbutton);
            this.groupBox1.Controls.Add(this.prioridadLabel);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.cb_cliente);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(441, 86);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cliente";
            // 
            // addclientbutton
            // 
            this.addclientbutton.Image = global::papiro.Properties.Resources.edit_add;
            this.addclientbutton.Location = new System.Drawing.Point(392, 44);
            this.addclientbutton.Name = "addclientbutton";
            this.addclientbutton.Size = new System.Drawing.Size(38, 36);
            this.addclientbutton.TabIndex = 34;
            this.addclientbutton.UseVisualStyleBackColor = true;
            this.addclientbutton.Click += new System.EventHandler(this.AddclientbuttonClick);
            // 
            // prioridadLabel
            // 
            this.prioridadLabel.AutoSize = true;
            this.prioridadLabel.Location = new System.Drawing.Point(68, 57);
            this.prioridadLabel.Name = "prioridadLabel";
            this.prioridadLabel.Size = new System.Drawing.Size(62, 16);
            this.prioridadLabel.TabIndex = 3;
            this.prioridadLabel.Text = "prioridad";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(6, 57);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 16);
            this.label10.TabIndex = 2;
            this.label10.Text = "Prioridad:";
            // 
            // cb_cliente
            // 
            this.cb_cliente.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_cliente.FormattingEnabled = true;
            this.cb_cliente.Items.AddRange(new object[] {
            "(Seleccione)"});
            this.cb_cliente.Location = new System.Drawing.Point(66, 19);
            this.cb_cliente.Name = "cb_cliente";
            this.cb_cliente.Size = new System.Drawing.Size(364, 24);
            this.cb_cliente.TabIndex = 0;
            this.cb_cliente.Text = "(Seleccione)";
            this.cb_cliente.SelectedIndexChanged += new System.EventHandler(this.CbClienteSelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nombre:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(42, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 16);
            this.label3.TabIndex = 20;
            this.label3.Text = "Contrato:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(260, 217);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 16);
            this.label4.TabIndex = 30;
            this.label4.Text = "Tipo de producto:";
            // 
            // addbutton
            // 
            this.addbutton.Image = global::papiro.Properties.Resources.edit_add;
            this.addbutton.Location = new System.Drawing.Point(404, 158);
            this.addbutton.Name = "addbutton";
            this.addbutton.Size = new System.Drawing.Size(38, 36);
            this.addbutton.TabIndex = 33;
            this.addbutton.UseVisualStyleBackColor = true;
            this.addbutton.Click += new System.EventHandler(this.AddbuttonClick);
            // 
            // salirbutton
            // 
            this.salirbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.salirbutton.Image = global::papiro.Properties.Resources.exit;
            this.salirbutton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.salirbutton.Location = new System.Drawing.Point(252, 462);
            this.salirbutton.Name = "salirbutton";
            this.salirbutton.Size = new System.Drawing.Size(135, 26);
            this.salirbutton.TabIndex = 34;
            this.salirbutton.Text = "Cancelar";
            this.salirbutton.UseVisualStyleBackColor = true;
            this.salirbutton.Click += new System.EventHandler(this.Button2Click);
            // 
            // descripciontextBox
            // 
            this.descripciontextBox.Location = new System.Drawing.Point(12, 384);
            this.descripciontextBox.MaxLength = 255;
            this.descripciontextBox.Multiline = true;
            this.descripciontextBox.Name = "descripciontextBox";
            this.descripciontextBox.Size = new System.Drawing.Size(441, 57);
            this.descripciontextBox.TabIndex = 35;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(9, 365);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(83, 16);
            this.label13.TabIndex = 36;
            this.label13.Text = "Descripción:";
            // 
            // propioRadioButton
            // 
            this.propioRadioButton.AutoSize = true;
            this.propioRadioButton.Checked = true;
            this.propioRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propioRadioButton.Location = new System.Drawing.Point(12, 108);
            this.propioRadioButton.Name = "propioRadioButton";
            this.propioRadioButton.Size = new System.Drawing.Size(66, 20);
            this.propioRadioButton.TabIndex = 37;
            this.propioRadioButton.TabStop = true;
            this.propioRadioButton.Text = "Propio";
            this.propioRadioButton.UseVisualStyleBackColor = true;
            this.propioRadioButton.CheckedChanged += new System.EventHandler(this.PropioRadioButtonCheckedChanged);
            // 
            // terceroRadioButton2
            // 
            this.terceroRadioButton2.AutoSize = true;
            this.terceroRadioButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.terceroRadioButton2.Location = new System.Drawing.Point(12, 279);
            this.terceroRadioButton2.Name = "terceroRadioButton2";
            this.terceroRadioButton2.Size = new System.Drawing.Size(69, 20);
            this.terceroRadioButton2.TabIndex = 38;
            this.terceroRadioButton2.Text = "Diseño";
            this.terceroRadioButton2.UseVisualStyleBackColor = true;
            this.terceroRadioButton2.CheckedChanged += new System.EventHandler(this.TerceroRadioButton2CheckedChanged);
            // 
            // precionumericUpDown
            // 
            this.precionumericUpDown.DecimalPlaces = 2;
            this.precionumericUpDown.Enabled = false;
            this.precionumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.precionumericUpDown.Location = new System.Drawing.Point(45, 323);
            this.precionumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.precionumericUpDown.Name = "precionumericUpDown";
            this.precionumericUpDown.Size = new System.Drawing.Size(179, 22);
            this.precionumericUpDown.TabIndex = 39;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(42, 304);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 16);
            this.label5.TabIndex = 40;
            this.label5.Text = "Precio de operación:";
            // 
            // contratotextBox
            // 
            this.contratotextBox.Location = new System.Drawing.Point(45, 150);
            this.contratotextBox.Multiline = true;
            this.contratotextBox.Name = "contratotextBox";
            this.contratotextBox.ReadOnly = true;
            this.contratotextBox.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.contratotextBox.Size = new System.Drawing.Size(353, 54);
            this.contratotextBox.TabIndex = 41;
            // 
            // OperacionDisenno
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 500);
            this.Controls.Add(this.contratotextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.precionumericUpDown);
            this.Controls.Add(this.terceroRadioButton2);
            this.Controls.Add(this.propioRadioButton);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.descripciontextBox);
            this.Controls.Add(this.salirbutton);
            this.Controls.Add(this.addbutton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cb_tipo_producto);
            this.Controls.Add(this.S);
            this.Controls.Add(this.cb_tipo_operacion);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "OperacionDisenno";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trabajo de diseño";
            this.Load += new System.EventHandler(this.OperacionLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.precionumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_tipo_operacion;
        private System.Windows.Forms.Button S;
        private System.Windows.Forms.ComboBox cb_tipo_producto;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label prioridadLabel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cb_cliente;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button addbutton;
        private System.Windows.Forms.Button addclientbutton;
        private System.Windows.Forms.Button salirbutton;
        private System.Windows.Forms.TextBox descripciontextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.RadioButton propioRadioButton;
        private System.Windows.Forms.RadioButton terceroRadioButton2;
        private System.Windows.Forms.NumericUpDown precionumericUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox contratotextBox;
    }
}