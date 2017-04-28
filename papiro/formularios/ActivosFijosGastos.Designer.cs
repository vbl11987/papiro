namespace papiro.formularios
{
    partial class ActivosFijosGastos
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
            this.productosDataGridView = new System.Windows.Forms.DataGridView();
            this.CodigoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NombreColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TipoProductoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CantidadColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnidadMedidaColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrecioColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImporteColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CantidadUtilizarColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CostoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GastoTotalLabel = new System.Windows.Forms.Label();
            this.CancelarButton = new System.Windows.Forms.Button();
            this.AceptarButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.productosDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // productosDataGridView
            // 
            this.productosDataGridView.AllowUserToAddRows = false;
            this.productosDataGridView.AllowUserToDeleteRows = false;
            this.productosDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.productosDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CodigoColumn,
            this.NombreColumn,
            this.TipoProductoColumn,
            this.CantidadColumn,
            this.UnidadMedidaColumn,
            this.PrecioColumn,
            this.ImporteColumn,
            this.CantidadUtilizarColumn,
            this.CostoColumn});
            this.productosDataGridView.Dock = System.Windows.Forms.DockStyle.Top;
            this.productosDataGridView.Location = new System.Drawing.Point(0, 0);
            this.productosDataGridView.Name = "productosDataGridView";
            this.productosDataGridView.Size = new System.Drawing.Size(945, 440);
            this.productosDataGridView.TabIndex = 9;
            this.productosDataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ProductosDataGridViewCellEndEdit);
            this.productosDataGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.ProductosDataGridViewCellValidating);
            // 
            // CodigoColumn
            // 
            this.CodigoColumn.HeaderText = "Código";
            this.CodigoColumn.Name = "CodigoColumn";
            this.CodigoColumn.ReadOnly = true;
            // 
            // NombreColumn
            // 
            this.NombreColumn.HeaderText = "Nombre";
            this.NombreColumn.MaxInputLength = 100;
            this.NombreColumn.Name = "NombreColumn";
            this.NombreColumn.ReadOnly = true;
            // 
            // TipoProductoColumn
            // 
            this.TipoProductoColumn.HeaderText = "Tipo de activo fijo";
            this.TipoProductoColumn.Name = "TipoProductoColumn";
            this.TipoProductoColumn.ReadOnly = true;
            // 
            // CantidadColumn
            // 
            this.CantidadColumn.HeaderText = "Cantidad";
            this.CantidadColumn.Name = "CantidadColumn";
            this.CantidadColumn.ReadOnly = true;
            // 
            // UnidadMedidaColumn
            // 
            this.UnidadMedidaColumn.HeaderText = "Unidad de medida";
            this.UnidadMedidaColumn.Name = "UnidadMedidaColumn";
            this.UnidadMedidaColumn.ReadOnly = true;
            this.UnidadMedidaColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.UnidadMedidaColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PrecioColumn
            // 
            this.PrecioColumn.HeaderText = "Precio";
            this.PrecioColumn.Name = "PrecioColumn";
            this.PrecioColumn.ReadOnly = true;
            // 
            // ImporteColumn
            // 
            this.ImporteColumn.HeaderText = "Importe";
            this.ImporteColumn.Name = "ImporteColumn";
            this.ImporteColumn.ReadOnly = true;
            // 
            // CantidadUtilizarColumn
            // 
            this.CantidadUtilizarColumn.HeaderText = "Cantidad a utilizar";
            this.CantidadUtilizarColumn.Name = "CantidadUtilizarColumn";
            // 
            // CostoColumn
            // 
            this.CostoColumn.HeaderText = "Costo";
            this.CostoColumn.Name = "CostoColumn";
            this.CostoColumn.ReadOnly = true;
            // 
            // GastoTotalLabel
            // 
            this.GastoTotalLabel.AutoSize = true;
            this.GastoTotalLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GastoTotalLabel.Location = new System.Drawing.Point(760, 443);
            this.GastoTotalLabel.Name = "GastoTotalLabel";
            this.GastoTotalLabel.Size = new System.Drawing.Size(78, 16);
            this.GastoTotalLabel.TabIndex = 17;
            this.GastoTotalLabel.Text = "Gasto total: ";
            // 
            // CancelarButton
            // 
            this.CancelarButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelarButton.Image = global::papiro.Properties.Resources.exit;
            this.CancelarButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CancelarButton.Location = new System.Drawing.Point(748, 490);
            this.CancelarButton.Name = "CancelarButton";
            this.CancelarButton.Size = new System.Drawing.Size(90, 23);
            this.CancelarButton.TabIndex = 19;
            this.CancelarButton.Text = "Cancelar";
            this.CancelarButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CancelarButton.UseVisualStyleBackColor = true;
            this.CancelarButton.Click += new System.EventHandler(this.CancelarButton_Click);
            // 
            // AceptarButton
            // 
            this.AceptarButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AceptarButton.Image = global::papiro.Properties.Resources.ok;
            this.AceptarButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AceptarButton.Location = new System.Drawing.Point(636, 490);
            this.AceptarButton.Name = "AceptarButton";
            this.AceptarButton.Size = new System.Drawing.Size(90, 23);
            this.AceptarButton.TabIndex = 18;
            this.AceptarButton.Text = "Aceptar";
            this.AceptarButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AceptarButton.UseVisualStyleBackColor = true;
            this.AceptarButton.Click += new System.EventHandler(this.AceptarButton_Click);
            // 
            // ActivosFijosGastos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 525);
            this.Controls.Add(this.CancelarButton);
            this.Controls.Add(this.AceptarButton);
            this.Controls.Add(this.GastoTotalLabel);
            this.Controls.Add(this.productosDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ActivosFijosGastos";
            this.Text = "Activos fijos-Gastos";
            this.Load += new System.EventHandler(this.ActivosFijosGastos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.productosDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView productosDataGridView;
        private System.Windows.Forms.Button CancelarButton;
        private System.Windows.Forms.Button AceptarButton;
        private System.Windows.Forms.Label GastoTotalLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn CodigoColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NombreColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TipoProductoColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CantidadColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnidadMedidaColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrecioColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ImporteColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CantidadUtilizarColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CostoColumn;
    }
}