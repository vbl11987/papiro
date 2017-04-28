namespace papiro.formularios
{
    partial class Gastos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Gastos));
            this.gastosTreeView = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nombreCuentatextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numCuentanumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.existCuentacomboBox = new System.Windows.Forms.ComboBox();
            this.nuevaCuentaradioButton = new System.Windows.Forms.RadioButton();
            this.existCuentaRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nombrePartidatextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numeroPartidanumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.existPartidadcomboBox = new System.Windows.Forms.ComboBox();
            this.nuevaPartidaradioButton = new System.Windows.Forms.RadioButton();
            this.existPartidaradioButton = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nombreElementtextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.existElementcomboBox = new System.Windows.Forms.ComboBox();
            this.nuevoElementradioButton = new System.Windows.Forms.RadioButton();
            this.existElementradioButton = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.efectivoEncomboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.descripciontextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.fechadateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.gastonumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numeroElemnetonumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.nuevogastobutton = new System.Windows.Forms.Button();
            this.salirbutton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCuentanumericUpDown)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numeroPartidanumericUpDown)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gastonumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeroElemnetonumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // gastosTreeView
            // 
            this.gastosTreeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gastosTreeView.FullRowSelect = true;
            this.gastosTreeView.Location = new System.Drawing.Point(3, 4);
            this.gastosTreeView.Name = "gastosTreeView";
            this.gastosTreeView.Size = new System.Drawing.Size(455, 534);
            this.gastosTreeView.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nombreCuentatextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numCuentanumericUpDown);
            this.groupBox1.Controls.Add(this.existCuentacomboBox);
            this.groupBox1.Controls.Add(this.nuevaCuentaradioButton);
            this.groupBox1.Controls.Add(this.existCuentaRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(474, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(331, 134);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cuenta";
            // 
            // nombreCuentatextBox
            // 
            this.nombreCuentatextBox.Location = new System.Drawing.Point(77, 100);
            this.nombreCuentatextBox.MaxLength = 30;
            this.nombreCuentatextBox.Name = "nombreCuentatextBox";
            this.nombreCuentatextBox.Size = new System.Drawing.Size(242, 20);
            this.nombreCuentatextBox.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Nombre:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Número:";
            // 
            // numCuentanumericUpDown
            // 
            this.numCuentanumericUpDown.Location = new System.Drawing.Point(77, 71);
            this.numCuentanumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numCuentanumericUpDown.Name = "numCuentanumericUpDown";
            this.numCuentanumericUpDown.Size = new System.Drawing.Size(121, 20);
            this.numCuentanumericUpDown.TabIndex = 3;
            // 
            // existCuentacomboBox
            // 
            this.existCuentacomboBox.FormattingEnabled = true;
            this.existCuentacomboBox.Location = new System.Drawing.Point(77, 19);
            this.existCuentacomboBox.Name = "existCuentacomboBox";
            this.existCuentacomboBox.Size = new System.Drawing.Size(242, 21);
            this.existCuentacomboBox.TabIndex = 2;
            this.existCuentacomboBox.Text = "(Seleccione)";
            this.existCuentacomboBox.SelectedIndexChanged += new System.EventHandler(this.ExistCuentacomboBoxSelectedIndexChanged);
            // 
            // nuevaCuentaradioButton
            // 
            this.nuevaCuentaradioButton.AutoSize = true;
            this.nuevaCuentaradioButton.Location = new System.Drawing.Point(10, 48);
            this.nuevaCuentaradioButton.Name = "nuevaCuentaradioButton";
            this.nuevaCuentaradioButton.Size = new System.Drawing.Size(57, 17);
            this.nuevaCuentaradioButton.TabIndex = 1;
            this.nuevaCuentaradioButton.Text = "Nueva";
            this.nuevaCuentaradioButton.UseVisualStyleBackColor = true;
            this.nuevaCuentaradioButton.CheckedChanged += new System.EventHandler(this.NuevaCuentaradioButtonCheckedChanged);
            // 
            // existCuentaRadioButton
            // 
            this.existCuentaRadioButton.AutoSize = true;
            this.existCuentaRadioButton.Location = new System.Drawing.Point(10, 20);
            this.existCuentaRadioButton.Name = "existCuentaRadioButton";
            this.existCuentaRadioButton.Size = new System.Drawing.Size(68, 17);
            this.existCuentaRadioButton.TabIndex = 0;
            this.existCuentaRadioButton.Text = "Existente";
            this.existCuentaRadioButton.UseVisualStyleBackColor = true;
            this.existCuentaRadioButton.CheckedChanged += new System.EventHandler(this.ExistCuentaRadioButtonCheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nombrePartidatextBox);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.numeroPartidanumericUpDown);
            this.groupBox2.Controls.Add(this.existPartidadcomboBox);
            this.groupBox2.Controls.Add(this.nuevaPartidaradioButton);
            this.groupBox2.Controls.Add(this.existPartidaradioButton);
            this.groupBox2.Location = new System.Drawing.Point(474, 137);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(331, 134);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Partida";
            // 
            // nombrePartidatextBox
            // 
            this.nombrePartidatextBox.Location = new System.Drawing.Point(77, 100);
            this.nombrePartidatextBox.MaxLength = 30;
            this.nombrePartidatextBox.Name = "nombrePartidatextBox";
            this.nombrePartidatextBox.Size = new System.Drawing.Size(242, 20);
            this.nombrePartidatextBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Nombre:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Número:";
            // 
            // numeroPartidanumericUpDown
            // 
            this.numeroPartidanumericUpDown.Location = new System.Drawing.Point(77, 71);
            this.numeroPartidanumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numeroPartidanumericUpDown.Name = "numeroPartidanumericUpDown";
            this.numeroPartidanumericUpDown.Size = new System.Drawing.Size(121, 20);
            this.numeroPartidanumericUpDown.TabIndex = 3;
            // 
            // existPartidadcomboBox
            // 
            this.existPartidadcomboBox.FormattingEnabled = true;
            this.existPartidadcomboBox.Location = new System.Drawing.Point(77, 19);
            this.existPartidadcomboBox.Name = "existPartidadcomboBox";
            this.existPartidadcomboBox.Size = new System.Drawing.Size(242, 21);
            this.existPartidadcomboBox.TabIndex = 2;
            this.existPartidadcomboBox.Text = "(Seleccione)";
            this.existPartidadcomboBox.SelectedIndexChanged += new System.EventHandler(this.ExistPartidadcomboBoxSelectedIndexChanged);
            // 
            // nuevaPartidaradioButton
            // 
            this.nuevaPartidaradioButton.AutoSize = true;
            this.nuevaPartidaradioButton.Location = new System.Drawing.Point(10, 48);
            this.nuevaPartidaradioButton.Name = "nuevaPartidaradioButton";
            this.nuevaPartidaradioButton.Size = new System.Drawing.Size(57, 17);
            this.nuevaPartidaradioButton.TabIndex = 1;
            this.nuevaPartidaradioButton.Text = "Nueva";
            this.nuevaPartidaradioButton.UseVisualStyleBackColor = true;
            this.nuevaPartidaradioButton.CheckedChanged += new System.EventHandler(this.NuevaPartidaradioButtonCheckedChanged);
            // 
            // existPartidaradioButton
            // 
            this.existPartidaradioButton.AutoSize = true;
            this.existPartidaradioButton.Location = new System.Drawing.Point(10, 20);
            this.existPartidaradioButton.Name = "existPartidaradioButton";
            this.existPartidaradioButton.Size = new System.Drawing.Size(68, 17);
            this.existPartidaradioButton.TabIndex = 0;
            this.existPartidaradioButton.Text = "Existente";
            this.existPartidaradioButton.UseVisualStyleBackColor = true;
            this.existPartidaradioButton.CheckedChanged += new System.EventHandler(this.ExistPartidaradioButtonCheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nombreElementtextBox);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.existElementcomboBox);
            this.groupBox3.Controls.Add(this.nuevoElementradioButton);
            this.groupBox3.Controls.Add(this.existElementradioButton);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.efectivoEncomboBox);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.descripciontextBox);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.fechadateTimePicker);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.gastonumericUpDown);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.numeroElemnetonumericUpDown);
            this.groupBox3.Location = new System.Drawing.Point(474, 274);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(331, 235);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Elemento";
            // 
            // nombreElementtextBox
            // 
            this.nombreElementtextBox.Location = new System.Drawing.Point(119, 44);
            this.nombreElementtextBox.MaxLength = 30;
            this.nombreElementtextBox.Name = "nombreElementtextBox";
            this.nombreElementtextBox.Size = new System.Drawing.Size(200, 20);
            this.nombreElementtextBox.TabIndex = 54;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(74, 46);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 13);
            this.label10.TabIndex = 53;
            this.label10.Text = "Nombre:";
            // 
            // existElementcomboBox
            // 
            this.existElementcomboBox.FormattingEnabled = true;
            this.existElementcomboBox.Location = new System.Drawing.Point(77, 15);
            this.existElementcomboBox.Name = "existElementcomboBox";
            this.existElementcomboBox.Size = new System.Drawing.Size(242, 21);
            this.existElementcomboBox.TabIndex = 52;
            this.existElementcomboBox.Text = "(Seleccione)";
            this.existElementcomboBox.SelectedIndexChanged += new System.EventHandler(this.ExistElementcomboBoxSelectedIndexChanged);
            // 
            // nuevoElementradioButton
            // 
            this.nuevoElementradioButton.AutoSize = true;
            this.nuevoElementradioButton.Location = new System.Drawing.Point(9, 44);
            this.nuevoElementradioButton.Name = "nuevoElementradioButton";
            this.nuevoElementradioButton.Size = new System.Drawing.Size(57, 17);
            this.nuevoElementradioButton.TabIndex = 51;
            this.nuevoElementradioButton.Text = "Nuevo";
            this.nuevoElementradioButton.UseVisualStyleBackColor = true;
            this.nuevoElementradioButton.CheckedChanged += new System.EventHandler(this.NuevoElementradioButtonCheckedChanged);
            // 
            // existElementradioButton
            // 
            this.existElementradioButton.AutoSize = true;
            this.existElementradioButton.Location = new System.Drawing.Point(9, 17);
            this.existElementradioButton.Name = "existElementradioButton";
            this.existElementradioButton.Size = new System.Drawing.Size(68, 17);
            this.existElementradioButton.TabIndex = 50;
            this.existElementradioButton.Text = "Existente";
            this.existElementradioButton.UseVisualStyleBackColor = true;
            this.existElementradioButton.CheckedChanged += new System.EventHandler(this.ExistElementradioButtonCheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(194, 106);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 13);
            this.label9.TabIndex = 49;
            this.label9.Text = "Efectivo en";
            // 
            // efectivoEncomboBox
            // 
            this.efectivoEncomboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.efectivoEncomboBox.FormattingEnabled = true;
            this.efectivoEncomboBox.Items.AddRange(new object[] {
            "(Seleccione)",
            "Banco",
            "Caja"});
            this.efectivoEncomboBox.Location = new System.Drawing.Point(197, 121);
            this.efectivoEncomboBox.Name = "efectivoEncomboBox";
            this.efectivoEncomboBox.Size = new System.Drawing.Size(121, 21);
            this.efectivoEncomboBox.TabIndex = 48;
            this.efectivoEncomboBox.Text = "(Seleccione)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 144);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Descripción:";
            // 
            // descripciontextBox
            // 
            this.descripciontextBox.Location = new System.Drawing.Point(10, 160);
            this.descripciontextBox.MaxLength = 255;
            this.descripciontextBox.Multiline = true;
            this.descripciontextBox.Name = "descripciontextBox";
            this.descripciontextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.descripciontextBox.Size = new System.Drawing.Size(308, 65);
            this.descripciontextBox.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 106);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Fecha:";
            // 
            // fechadateTimePicker
            // 
            this.fechadateTimePicker.CustomFormat = "dd/MM/yyyy";
            this.fechadateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.fechadateTimePicker.Location = new System.Drawing.Point(34, 121);
            this.fechadateTimePicker.Name = "fechadateTimePicker";
            this.fechadateTimePicker.Size = new System.Drawing.Size(120, 20);
            this.fechadateTimePicker.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(194, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Gasto:";
            // 
            // gastonumericUpDown
            // 
            this.gastonumericUpDown.DecimalPlaces = 2;
            this.gastonumericUpDown.Location = new System.Drawing.Point(197, 82);
            this.gastonumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.gastonumericUpDown.Name = "gastonumericUpDown";
            this.gastonumericUpDown.Size = new System.Drawing.Size(121, 20);
            this.gastonumericUpDown.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Número:";
            // 
            // numeroElemnetonumericUpDown
            // 
            this.numeroElemnetonumericUpDown.Location = new System.Drawing.Point(33, 82);
            this.numeroElemnetonumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numeroElemnetonumericUpDown.Name = "numeroElemnetonumericUpDown";
            this.numeroElemnetonumericUpDown.Size = new System.Drawing.Size(121, 20);
            this.numeroElemnetonumericUpDown.TabIndex = 3;
            // 
            // nuevogastobutton
            // 
            this.nuevogastobutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nuevogastobutton.Image = global::papiro.Properties.Resources.save_all;
            this.nuevogastobutton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.nuevogastobutton.Location = new System.Drawing.Point(474, 515);
            this.nuevogastobutton.Name = "nuevogastobutton";
            this.nuevogastobutton.Size = new System.Drawing.Size(164, 23);
            this.nuevogastobutton.TabIndex = 4;
            this.nuevogastobutton.Text = "Guardar nuevo gasto";
            this.nuevogastobutton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nuevogastobutton.UseVisualStyleBackColor = true;
            this.nuevogastobutton.Click += new System.EventHandler(this.NuevogastobuttonClick);
            // 
            // salirbutton
            // 
            this.salirbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.salirbutton.Image = global::papiro.Properties.Resources.exit;
            this.salirbutton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.salirbutton.Location = new System.Drawing.Point(714, 515);
            this.salirbutton.Name = "salirbutton";
            this.salirbutton.Size = new System.Drawing.Size(91, 23);
            this.salirbutton.TabIndex = 5;
            this.salirbutton.Text = "Salir";
            this.salirbutton.UseVisualStyleBackColor = true;
            this.salirbutton.Click += new System.EventHandler(this.SalirbuttonClick);
            // 
            // Gastos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 547);
            this.Controls.Add(this.salirbutton);
            this.Controls.Add(this.nuevogastobutton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gastosTreeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Gastos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gastos";
            this.Load += new System.EventHandler(this.GastosLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCuentanumericUpDown)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numeroPartidanumericUpDown)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gastonumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeroElemnetonumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView gastosTreeView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox existCuentacomboBox;
        private System.Windows.Forms.RadioButton nuevaCuentaradioButton;
        private System.Windows.Forms.RadioButton existCuentaRadioButton;
        private System.Windows.Forms.TextBox nombreCuentatextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox nombrePartidatextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numeroPartidanumericUpDown;
        private System.Windows.Forms.ComboBox existPartidadcomboBox;
        private System.Windows.Forms.RadioButton nuevaPartidaradioButton;
        private System.Windows.Forms.RadioButton existPartidaradioButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numeroElemnetonumericUpDown;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox descripciontextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker fechadateTimePicker;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown gastonumericUpDown;
        private System.Windows.Forms.Button nuevogastobutton;
        private System.Windows.Forms.Button salirbutton;
        private System.Windows.Forms.NumericUpDown numCuentanumericUpDown;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox efectivoEncomboBox;
        private System.Windows.Forms.ComboBox existElementcomboBox;
        private System.Windows.Forms.RadioButton nuevoElementradioButton;
        private System.Windows.Forms.RadioButton existElementradioButton;
        private System.Windows.Forms.TextBox nombreElementtextBox;
        private System.Windows.Forms.Label label10;
    }
}