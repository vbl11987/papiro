namespace papiro.formularios
{
    partial class CerrarCuentaPorCobrar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CerrarCuentaPorCobrar));
            this.label1 = new System.Windows.Forms.Label();
            this.filtroDesde = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.filtroHasta = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.efectivoEncomboBox = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.filtrar = new System.Windows.Forms.Button();
            this.clientcomboBox = new System.Windows.Forms.ComboBox();
            this.facturacomboBox = new System.Windows.Forms.ComboBox();
            this.printableLV = new PrintableListView.PrintableListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 16);
            this.label1.TabIndex = 38;
            this.label1.Text = "Opciones de filtro:  Desde";
            // 
            // filtroDesde
            // 
            this.filtroDesde.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filtroDesde.Checked = false;
            this.filtroDesde.CustomFormat = "dd/MM/yyyy";
            this.filtroDesde.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filtroDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.filtroDesde.Location = new System.Drawing.Point(164, 10);
            this.filtroDesde.Name = "filtroDesde";
            this.filtroDesde.ShowCheckBox = true;
            this.filtroDesde.Size = new System.Drawing.Size(103, 21);
            this.filtroDesde.TabIndex = 39;
            this.filtroDesde.Value = new System.DateTime(2012, 2, 24, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(269, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 16);
            this.label2.TabIndex = 40;
            this.label2.Text = "Hasta";
            // 
            // filtroHasta
            // 
            this.filtroHasta.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filtroHasta.Checked = false;
            this.filtroHasta.CustomFormat = "dd/MM/yyyy";
            this.filtroHasta.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filtroHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.filtroHasta.Location = new System.Drawing.Point(315, 10);
            this.filtroHasta.Name = "filtroHasta";
            this.filtroHasta.ShowCheckBox = true;
            this.filtroHasta.Size = new System.Drawing.Size(102, 21);
            this.filtroHasta.TabIndex = 41;
            this.filtroHasta.Value = new System.DateTime(2012, 2, 24, 0, 0, 0, 0);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(138, 409);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 16);
            this.label3.TabIndex = 48;
            this.label3.Text = "Guardar en efectivo en";
            // 
            // efectivoEncomboBox
            // 
            this.efectivoEncomboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.efectivoEncomboBox.FormattingEnabled = true;
            this.efectivoEncomboBox.Items.AddRange(new object[] {
            "(Seleccione)",
            "Banco",
            "Caja"});
            this.efectivoEncomboBox.Location = new System.Drawing.Point(280, 406);
            this.efectivoEncomboBox.Name = "efectivoEncomboBox";
            this.efectivoEncomboBox.Size = new System.Drawing.Size(121, 24);
            this.efectivoEncomboBox.TabIndex = 47;
            this.efectivoEncomboBox.Text = "(Seleccione)";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Image = global::papiro.Properties.Resources.locked;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(417, 406);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(119, 23);
            this.button2.TabIndex = 44;
            this.button2.Text = "Cerrar cuenta";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Image = global::papiro.Properties.Resources.exit;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(553, 406);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 43;
            this.button1.Text = "Salir";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // filtrar
            // 
            this.filtrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filtrar.Image = global::papiro.Properties.Resources.viewmag;
            this.filtrar.Location = new System.Drawing.Point(831, 7);
            this.filtrar.Name = "filtrar";
            this.filtrar.Size = new System.Drawing.Size(25, 25);
            this.filtrar.TabIndex = 42;
            this.filtrar.UseVisualStyleBackColor = true;
            this.filtrar.Click += new System.EventHandler(this.FiltrarClick);
            // 
            // clientcomboBox
            // 
            this.clientcomboBox.FormattingEnabled = true;
            this.clientcomboBox.Items.AddRange(new object[] {
            "(Cliente)"});
            this.clientcomboBox.Location = new System.Drawing.Point(423, 10);
            this.clientcomboBox.Name = "clientcomboBox";
            this.clientcomboBox.Size = new System.Drawing.Size(280, 21);
            this.clientcomboBox.TabIndex = 49;
            this.clientcomboBox.Text = "(Cliente)";
            // 
            // facturacomboBox
            // 
            this.facturacomboBox.FormattingEnabled = true;
            this.facturacomboBox.Items.AddRange(new object[] {
            "(Factura)"});
            this.facturacomboBox.Location = new System.Drawing.Point(709, 10);
            this.facturacomboBox.Name = "facturacomboBox";
            this.facturacomboBox.Size = new System.Drawing.Size(116, 21);
            this.facturacomboBox.TabIndex = 50;
            this.facturacomboBox.Text = "(Factura)";
            // 
            // printableLV
            // 
            this.printableLV.CheckBoxes = true;
            this.printableLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader7,
            this.columnHeader9});
            this.printableLV.FitToPage = false;
            this.printableLV.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printableLV.GridLines = true;
            this.printableLV.HideSelection = false;
            this.printableLV.Location = new System.Drawing.Point(0, 37);
            this.printableLV.Name = "printableLV";
            this.printableLV.Size = new System.Drawing.Size(868, 335);
            this.printableLV.TabIndex = 51;
            this.printableLV.Title = "Cuentas por pagar";
            this.printableLV.UseCompatibleStateImageBehavior = false;
            this.printableLV.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Cliente";
            this.columnHeader2.Width = 397;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Fecha";
            this.columnHeader3.Width = 175;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Monto";
            this.columnHeader7.Width = 168;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Factura";
            this.columnHeader9.Width = 124;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(760, 406);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(89, 23);
            this.button3.TabIndex = 52;
            this.button3.Text = "Imprimir";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Image = ((System.Drawing.Image)(resources.GetObject("button4.Image")));
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.Location = new System.Drawing.Point(637, 406);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(111, 23);
            this.button4.TabIndex = 53;
            this.button4.Text = "Vista previa";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(711, 383);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(35, 13);
            this.lblTotal.TabIndex = 54;
            this.lblTotal.Text = "label4";
            // 
            // CerrarCuentaPorCobrar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 441);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.printableLV);
            this.Controls.Add(this.facturacomboBox);
            this.Controls.Add(this.efectivoEncomboBox);
            this.Controls.Add(this.clientcomboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.filtrar);
            this.Controls.Add(this.filtroHasta);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.filtroDesde);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CerrarCuentaPorCobrar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cerrar cuentas por cobrar";
            this.Load += new System.EventHandler(this.GestionarCuentasCobrarLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker filtroDesde;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker filtroHasta;
        private System.Windows.Forms.Button filtrar;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox efectivoEncomboBox;
        private System.Windows.Forms.ComboBox clientcomboBox;
        private System.Windows.Forms.ComboBox facturacomboBox;
        private PrintableListView.PrintableListView printableLV;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label lblTotal;
    }
}