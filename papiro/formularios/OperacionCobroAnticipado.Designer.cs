namespace papiro.formularios
{
    partial class OperacionCobroAnticipado
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OperacionCobroAnticipado));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rBEstatal = new System.Windows.Forms.RadioButton();
            this.rBParticular = new System.Windows.Forms.RadioButton();
            this.cantidadLabel = new System.Windows.Forms.Label();
            this.cb_cliente = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.facturarbutton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.valorOpnumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.contratotextBox = new System.Windows.Forms.TextBox();
            this.apagar3eronumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.op3erosTotallabel = new System.Windows.Forms.Label();
            this.operaciones3erosdataGridView = new System.Windows.Forms.DataGridView();
            this.ProveedorColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.TipoOperacion3eroColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.PrecioColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CantidadColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImporteColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descripciontextBox = new System.Windows.Forms.TextBox();
            this.salirbutton = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.operadorcomboBox = new System.Windows.Forms.ComboBox();
            this.comercialcomboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cantidad = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cb_tipo_producto = new System.Windows.Forms.ComboBox();
            this.S = new System.Windows.Forms.Button();
            this.cb_tipo_operacion = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.montoTotal = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valorOpnumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.apagar3eronumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.operaciones3erosdataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cantidad)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rBEstatal);
            this.groupBox1.Controls.Add(this.rBParticular);
            this.groupBox1.Controls.Add(this.cantidadLabel);
            this.groupBox1.Controls.Add(this.cb_cliente);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(770, 87);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cliente";
            // 
            // rBEstatal
            // 
            this.rBEstatal.AutoSize = true;
            this.rBEstatal.Location = new System.Drawing.Point(18, 61);
            this.rBEstatal.Name = "rBEstatal";
            this.rBEstatal.Size = new System.Drawing.Size(67, 20);
            this.rBEstatal.TabIndex = 37;
            this.rBEstatal.TabStop = true;
            this.rBEstatal.Text = "Estatal";
            this.rBEstatal.UseVisualStyleBackColor = true;
            this.rBEstatal.CheckedChanged += new System.EventHandler(this.rBEstatal_CheckedChanged_1);
            // 
            // rBParticular
            // 
            this.rBParticular.AutoSize = true;
            this.rBParticular.Location = new System.Drawing.Point(91, 61);
            this.rBParticular.Name = "rBParticular";
            this.rBParticular.Size = new System.Drawing.Size(82, 20);
            this.rBParticular.TabIndex = 36;
            this.rBParticular.TabStop = true;
            this.rBParticular.Text = "Particular";
            this.rBParticular.UseVisualStyleBackColor = true;
            this.rBParticular.CheckedChanged += new System.EventHandler(this.rBParticular_CheckedChanged_1);
            // 
            // cantidadLabel
            // 
            this.cantidadLabel.AutoSize = true;
            this.cantidadLabel.Location = new System.Drawing.Point(528, 28);
            this.cantidadLabel.Name = "cantidadLabel";
            this.cantidadLabel.Size = new System.Drawing.Size(111, 16);
            this.cantidadLabel.TabIndex = 3;
            this.cantidadLabel.Text = "cantidad restante";
            // 
            // cb_cliente
            // 
            this.cb_cliente.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_cliente.FormattingEnabled = true;
            this.cb_cliente.Location = new System.Drawing.Point(66, 25);
            this.cb_cliente.Name = "cb_cliente";
            this.cb_cliente.Size = new System.Drawing.Size(456, 24);
            this.cb_cliente.TabIndex = 0;
            this.cb_cliente.SelectedIndexChanged += new System.EventHandler(this.cb_cliente_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nombre:";
            // 
            // facturarbutton
            // 
            this.facturarbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.facturarbutton.Image = global::papiro.Properties.Resources.todo;
            this.facturarbutton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.facturarbutton.Location = new System.Drawing.Point(496, 464);
            this.facturarbutton.Name = "facturarbutton";
            this.facturarbutton.Size = new System.Drawing.Size(140, 24);
            this.facturarbutton.TabIndex = 74;
            this.facturarbutton.Text = "&Facturar";
            this.facturarbutton.UseVisualStyleBackColor = true;
            this.facturarbutton.Click += new System.EventHandler(this.facturarbutton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(561, 191);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 25);
            this.label7.TabIndex = 73;
            this.label7.Text = "=";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(371, 195);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 25);
            this.label6.TabIndex = 72;
            this.label6.Text = "*";
            // 
            // valorOpnumericUpDown
            // 
            this.valorOpnumericUpDown.DecimalPlaces = 2;
            this.valorOpnumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valorOpnumericUpDown.Location = new System.Drawing.Point(242, 192);
            this.valorOpnumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.valorOpnumericUpDown.Name = "valorOpnumericUpDown";
            this.valorOpnumericUpDown.Size = new System.Drawing.Size(99, 22);
            this.valorOpnumericUpDown.TabIndex = 71;
            // 
            // contratotextBox
            // 
            this.contratotextBox.Location = new System.Drawing.Point(24, 124);
            this.contratotextBox.Multiline = true;
            this.contratotextBox.Name = "contratotextBox";
            this.contratotextBox.ReadOnly = true;
            this.contratotextBox.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.contratotextBox.Size = new System.Drawing.Size(711, 27);
            this.contratotextBox.TabIndex = 69;
            // 
            // apagar3eronumericUpDown
            // 
            this.apagar3eronumericUpDown.DecimalPlaces = 2;
            this.apagar3eronumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.apagar3eronumericUpDown.Location = new System.Drawing.Point(474, 302);
            this.apagar3eronumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.apagar3eronumericUpDown.Name = "apagar3eronumericUpDown";
            this.apagar3eronumericUpDown.Size = new System.Drawing.Size(116, 22);
            this.apagar3eronumericUpDown.TabIndex = 68;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(412, 304);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(62, 16);
            this.label14.TabIndex = 67;
            this.label14.Text = "A cobrar:";
            // 
            // op3erosTotallabel
            // 
            this.op3erosTotallabel.AutoSize = true;
            this.op3erosTotallabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.op3erosTotallabel.Location = new System.Drawing.Point(21, 304);
            this.op3erosTotallabel.Name = "op3erosTotallabel";
            this.op3erosTotallabel.Size = new System.Drawing.Size(187, 16);
            this.op3erosTotallabel.TabIndex = 66;
            this.op3erosTotallabel.Text = "Operaciones de terceros: 0.0$";
            // 
            // operaciones3erosdataGridView
            // 
            this.operaciones3erosdataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.operaciones3erosdataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProveedorColumn,
            this.TipoOperacion3eroColumn,
            this.PrecioColumn,
            this.CantidadColumn,
            this.ImporteColumn});
            this.operaciones3erosdataGridView.Location = new System.Drawing.Point(21, 328);
            this.operaciones3erosdataGridView.Name = "operaciones3erosdataGridView";
            this.operaciones3erosdataGridView.Size = new System.Drawing.Size(771, 107);
            this.operaciones3erosdataGridView.TabIndex = 65;
            this.operaciones3erosdataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.operaciones3erosdataGridView_CellClick);
            this.operaciones3erosdataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.operaciones3erosdataGridView_CellEndEdit);
            this.operaciones3erosdataGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.operaciones3erosdataGridView_CellValidating);
            this.operaciones3erosdataGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.operaciones3erosdataGridView_RowsAdded);
            this.operaciones3erosdataGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.operaciones3erosdataGridView_RowsRemoved);
            // 
            // ProveedorColumn
            // 
            this.ProveedorColumn.HeaderText = "Proveedor";
            this.ProveedorColumn.Name = "ProveedorColumn";
            this.ProveedorColumn.Width = 200;
            // 
            // TipoOperacion3eroColumn
            // 
            this.TipoOperacion3eroColumn.HeaderText = "Tipo de operación";
            this.TipoOperacion3eroColumn.Name = "TipoOperacion3eroColumn";
            this.TipoOperacion3eroColumn.Width = 200;
            // 
            // PrecioColumn
            // 
            this.PrecioColumn.HeaderText = "Precio";
            this.PrecioColumn.Name = "PrecioColumn";
            this.PrecioColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.PrecioColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CantidadColumn
            // 
            this.CantidadColumn.HeaderText = "Cantidad";
            this.CantidadColumn.Name = "CantidadColumn";
            this.CantidadColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CantidadColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ImporteColumn
            // 
            this.ImporteColumn.HeaderText = "Importe";
            this.ImporteColumn.Name = "ImporteColumn";
            this.ImporteColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // descripciontextBox
            // 
            this.descripciontextBox.Location = new System.Drawing.Point(21, 464);
            this.descripciontextBox.MaxLength = 255;
            this.descripciontextBox.Multiline = true;
            this.descripciontextBox.Name = "descripciontextBox";
            this.descripciontextBox.Size = new System.Drawing.Size(441, 72);
            this.descripciontextBox.TabIndex = 64;
            // 
            // salirbutton
            // 
            this.salirbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.salirbutton.Image = global::papiro.Properties.Resources.exit;
            this.salirbutton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.salirbutton.Location = new System.Drawing.Point(657, 510);
            this.salirbutton.Name = "salirbutton";
            this.salirbutton.Size = new System.Drawing.Size(135, 26);
            this.salirbutton.TabIndex = 63;
            this.salirbutton.Text = "Cancelar";
            this.salirbutton.UseVisualStyleBackColor = true;
            this.salirbutton.Click += new System.EventHandler(this.salirbutton_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(608, 235);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(170, 16);
            this.label12.TabIndex = 61;
            this.label12.Text = "Participación del operador:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(373, 235);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(173, 16);
            this.label11.TabIndex = 60;
            this.label11.Text = "Participación del comercial:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(18, 239);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 16);
            this.label4.TabIndex = 59;
            this.label4.Text = "Tipo de producto:";
            // 
            // operadorcomboBox
            // 
            this.operadorcomboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.operadorcomboBox.FormattingEnabled = true;
            this.operadorcomboBox.Items.AddRange(new object[] {
            "(Seleccione)"});
            this.operadorcomboBox.Location = new System.Drawing.Point(611, 258);
            this.operadorcomboBox.Name = "operadorcomboBox";
            this.operadorcomboBox.Size = new System.Drawing.Size(172, 24);
            this.operadorcomboBox.TabIndex = 51;
            this.operadorcomboBox.Text = "(Seleccione)";
            // 
            // comercialcomboBox
            // 
            this.comercialcomboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comercialcomboBox.FormattingEnabled = true;
            this.comercialcomboBox.Items.AddRange(new object[] {
            "(Seleccione)"});
            this.comercialcomboBox.Location = new System.Drawing.Point(376, 258);
            this.comercialcomboBox.Name = "comercialcomboBox";
            this.comercialcomboBox.Size = new System.Drawing.Size(172, 24);
            this.comercialcomboBox.TabIndex = 50;
            this.comercialcomboBox.Text = "(Seleccione)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(22, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 16);
            this.label3.TabIndex = 54;
            this.label3.Text = "Contrato:";
            // 
            // cantidad
            // 
            this.cantidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cantidad.Location = new System.Drawing.Point(421, 192);
            this.cantidad.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.cantidad.Name = "cantidad";
            this.cantidad.Size = new System.Drawing.Size(99, 22);
            this.cantidad.TabIndex = 49;
            this.cantidad.ValueChanged += new System.EventHandler(this.cantidad_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(618, 195);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 16);
            this.label9.TabIndex = 58;
            this.label9.Text = "Monto Total:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(418, 170);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 16);
            this.label8.TabIndex = 57;
            this.label8.Text = "Cantidad:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(239, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 16);
            this.label5.TabIndex = 56;
            this.label5.Text = "Precio:";
            // 
            // cb_tipo_producto
            // 
            this.cb_tipo_producto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_tipo_producto.FormattingEnabled = true;
            this.cb_tipo_producto.Items.AddRange(new object[] {
            "(Seleccione)"});
            this.cb_tipo_producto.Location = new System.Drawing.Point(21, 258);
            this.cb_tipo_producto.Name = "cb_tipo_producto";
            this.cb_tipo_producto.Size = new System.Drawing.Size(197, 24);
            this.cb_tipo_producto.TabIndex = 52;
            this.cb_tipo_producto.Text = "(Seleccione)";
            // 
            // S
            // 
            this.S.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.S.Image = global::papiro.Properties.Resources.ok;
            this.S.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.S.Location = new System.Drawing.Point(496, 510);
            this.S.Name = "S";
            this.S.Size = new System.Drawing.Size(152, 26);
            this.S.TabIndex = 53;
            this.S.Text = "Registrar operación";
            this.S.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.S.UseVisualStyleBackColor = true;
            this.S.Click += new System.EventHandler(this.S_Click);
            // 
            // cb_tipo_operacion
            // 
            this.cb_tipo_operacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_tipo_operacion.FormattingEnabled = true;
            this.cb_tipo_operacion.Items.AddRange(new object[] {
            "(Seleccione)"});
            this.cb_tipo_operacion.Location = new System.Drawing.Point(24, 192);
            this.cb_tipo_operacion.Name = "cb_tipo_operacion";
            this.cb_tipo_operacion.Size = new System.Drawing.Size(161, 24);
            this.cb_tipo_operacion.TabIndex = 48;
            this.cb_tipo_operacion.Text = "(Seleccione)";
            this.cb_tipo_operacion.SelectedIndexChanged += new System.EventHandler(this.cb_tipo_operacion_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(21, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 16);
            this.label2.TabIndex = 55;
            this.label2.Text = "Tipo de Operación:";
            // 
            // montoTotal
            // 
            this.montoTotal.AutoSize = true;
            this.montoTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.montoTotal.Location = new System.Drawing.Point(706, 195);
            this.montoTotal.Name = "montoTotal";
            this.montoTotal.Size = new System.Drawing.Size(15, 16);
            this.montoTotal.TabIndex = 75;
            this.montoTotal.Text = "0";
            // 
            // OperacionCobroAnticipado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 548);
            this.Controls.Add(this.montoTotal);
            this.Controls.Add(this.facturarbutton);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.valorOpnumericUpDown);
            this.Controls.Add(this.contratotextBox);
            this.Controls.Add(this.apagar3eronumericUpDown);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.op3erosTotallabel);
            this.Controls.Add(this.operaciones3erosdataGridView);
            this.Controls.Add(this.descripciontextBox);
            this.Controls.Add(this.salirbutton);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.operadorcomboBox);
            this.Controls.Add(this.comercialcomboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cantidad);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cb_tipo_producto);
            this.Controls.Add(this.S);
            this.Controls.Add(this.cb_tipo_operacion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OperacionCobroAnticipado";
            this.Text = "Operación de Cobro Anticipado";
            this.Load += new System.EventHandler(this.OperacionCobroAnticipado_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valorOpnumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.apagar3eronumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.operaciones3erosdataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cantidad)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rBEstatal;
        private System.Windows.Forms.RadioButton rBParticular;
        private System.Windows.Forms.Label cantidadLabel;
        private System.Windows.Forms.ComboBox cb_cliente;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button facturarbutton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown valorOpnumericUpDown;
        private System.Windows.Forms.TextBox contratotextBox;
        private System.Windows.Forms.NumericUpDown apagar3eronumericUpDown;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label op3erosTotallabel;
        private System.Windows.Forms.DataGridView operaciones3erosdataGridView;
        private System.Windows.Forms.DataGridViewButtonColumn ProveedorColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn TipoOperacion3eroColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrecioColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CantidadColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ImporteColumn;
        private System.Windows.Forms.TextBox descripciontextBox;
        private System.Windows.Forms.Button salirbutton;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox operadorcomboBox;
        private System.Windows.Forms.ComboBox comercialcomboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown cantidad;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cb_tipo_producto;
        private System.Windows.Forms.Button S;
        private System.Windows.Forms.ComboBox cb_tipo_operacion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label montoTotal;
    }
}