namespace papiro.formularios
{
    partial class OperacionesPendientes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OperacionesPendientes));
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.clienteToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tipoOptoolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.tipProdtoolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.filtrar = new System.Windows.Forms.ToolStripButton();
            this.ejecutaroperacionbutton = new System.Windows.Forms.Button();
            this.salirbutton = new System.Windows.Forms.Button();
            this.cancelOpbutton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.printableLV = new PrintableListView.PrintableListView();
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button3 = new System.Windows.Forms.Button();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripSeparator2,
            this.clienteToolStripComboBox,
            this.toolStripSeparator4,
            this.tipoOptoolStripComboBox,
            this.tipProdtoolStripComboBox,
            this.filtrar});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(750, 25);
            this.toolStrip2.TabIndex = 3;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(107, 22);
            this.toolStripLabel1.Text = "Opciones de filtro: ";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // clienteToolStripComboBox
            // 
            this.clienteToolStripComboBox.Items.AddRange(new object[] {
            "(Cliente)"});
            this.clienteToolStripComboBox.Name = "clienteToolStripComboBox";
            this.clienteToolStripComboBox.Size = new System.Drawing.Size(121, 25);
            this.clienteToolStripComboBox.Text = "(Cliente)";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tipoOptoolStripComboBox
            // 
            this.tipoOptoolStripComboBox.Items.AddRange(new object[] {
            "(Tipo de operación)"});
            this.tipoOptoolStripComboBox.Name = "tipoOptoolStripComboBox";
            this.tipoOptoolStripComboBox.Size = new System.Drawing.Size(121, 25);
            this.tipoOptoolStripComboBox.Text = "(Tipo de operación)";
            // 
            // tipProdtoolStripComboBox
            // 
            this.tipProdtoolStripComboBox.Items.AddRange(new object[] {
            "(Tipo de productos)"});
            this.tipProdtoolStripComboBox.Name = "tipProdtoolStripComboBox";
            this.tipProdtoolStripComboBox.Size = new System.Drawing.Size(121, 25);
            this.tipProdtoolStripComboBox.Text = "(Tipo de producto)";
            // 
            // filtrar
            // 
            this.filtrar.Image = global::papiro.Properties.Resources.viewmag;
            this.filtrar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.filtrar.Name = "filtrar";
            this.filtrar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.filtrar.Size = new System.Drawing.Size(57, 22);
            this.filtrar.Text = "Filtrar";
            this.filtrar.ToolTipText = "Filtrar de acuerdo a las opciones de filtro seleccionadas";
            this.filtrar.Click += new System.EventHandler(this.FiltrarClick);
            // 
            // ejecutaroperacionbutton
            // 
            this.ejecutaroperacionbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ejecutaroperacionbutton.Image = global::papiro.Properties.Resources.ok;
            this.ejecutaroperacionbutton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ejecutaroperacionbutton.Location = new System.Drawing.Point(22, 436);
            this.ejecutaroperacionbutton.Name = "ejecutaroperacionbutton";
            this.ejecutaroperacionbutton.Size = new System.Drawing.Size(144, 23);
            this.ejecutaroperacionbutton.TabIndex = 4;
            this.ejecutaroperacionbutton.Text = "Ejecutar operación";
            this.ejecutaroperacionbutton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ejecutaroperacionbutton.UseVisualStyleBackColor = true;
            this.ejecutaroperacionbutton.Click += new System.EventHandler(this.EjecutaroperacionbuttonClick);
            // 
            // salirbutton
            // 
            this.salirbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.salirbutton.Image = global::papiro.Properties.Resources.exit;
            this.salirbutton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.salirbutton.Location = new System.Drawing.Point(347, 436);
            this.salirbutton.Name = "salirbutton";
            this.salirbutton.Size = new System.Drawing.Size(106, 23);
            this.salirbutton.TabIndex = 5;
            this.salirbutton.Text = "Salir";
            this.salirbutton.UseVisualStyleBackColor = true;
            this.salirbutton.Click += new System.EventHandler(this.SalirbuttonClick);
            // 
            // cancelOpbutton
            // 
            this.cancelOpbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelOpbutton.Image = global::papiro.Properties.Resources.cancel;
            this.cancelOpbutton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cancelOpbutton.Location = new System.Drawing.Point(183, 436);
            this.cancelOpbutton.Name = "cancelOpbutton";
            this.cancelOpbutton.Size = new System.Drawing.Size(146, 23);
            this.cancelOpbutton.TabIndex = 6;
            this.cancelOpbutton.Text = "Cancelar operación";
            this.cancelOpbutton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cancelOpbutton.UseVisualStyleBackColor = true;
            this.cancelOpbutton.Click += new System.EventHandler(this.CancelOpbuttonClick);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(616, 436);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Imprimir";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(472, 436);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(132, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Vista Previa";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // printableLV
            // 
            this.printableLV.CheckBoxes = true;
            this.printableLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14});
            this.printableLV.FitToPage = false;
            this.printableLV.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printableLV.FullRowSelect = true;
            this.printableLV.GridLines = true;
            this.printableLV.Location = new System.Drawing.Point(-1, 28);
            this.printableLV.Name = "printableLV";
            this.printableLV.Size = new System.Drawing.Size(752, 377);
            this.printableLV.TabIndex = 9;
            this.printableLV.Title = "Operaciones pendientes";
            this.printableLV.UseCompatibleStateImageBehavior = false;
            this.printableLV.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Cliente";
            this.columnHeader8.Width = 104;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Contrato";
            this.columnHeader9.Width = 128;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Tipo de operación";
            this.columnHeader10.Width = 104;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Tipo de producto";
            this.columnHeader11.Width = 94;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Fecha";
            this.columnHeader12.Width = 73;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Descripción";
            this.columnHeader13.Width = 167;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "Monto";
            this.columnHeader14.Width = 72;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Image = global::papiro.Properties.Resources.ok;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(22, 407);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(144, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "Cobro Anticipado";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // OperacionesPendientes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 471);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.printableLV);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cancelOpbutton);
            this.Controls.Add(this.salirbutton);
            this.Controls.Add(this.ejecutaroperacionbutton);
            this.Controls.Add(this.toolStrip2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OperacionesPendientes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Operaciones pendientes";
            this.Load += new System.EventHandler(this.OperacionesPendientesLoad);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripComboBox clienteToolStripComboBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripComboBox tipoOptoolStripComboBox;
        private System.Windows.Forms.ToolStripComboBox tipProdtoolStripComboBox;
        private System.Windows.Forms.ToolStripButton filtrar;
        private System.Windows.Forms.Button ejecutaroperacionbutton;
        private System.Windows.Forms.Button salirbutton;
        private System.Windows.Forms.Button cancelOpbutton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private PrintableListView.PrintableListView printableLV;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.Button button3;
    }
}