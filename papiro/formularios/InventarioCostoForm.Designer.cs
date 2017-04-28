using System;

namespace papiro.formularios
{
    partial class InventarioCostoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventarioCostoForm));
            this.productosDataGridView = new System.Windows.Forms.DataGridView();
            this.CodigoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NombreColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TipoProdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CantidadColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnidadMedidaColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrecioColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImporteColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.importeTotalLabel = new System.Windows.Forms.Label();
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
            this.TipoProdColumn,
            this.CantidadColumn,
            this.UnidadMedidaColumn,
            this.PrecioColumn,
            this.ImporteColumn});
            this.productosDataGridView.Dock = System.Windows.Forms.DockStyle.Top;
            this.productosDataGridView.Location = new System.Drawing.Point(0, 0);
            this.productosDataGridView.Name = "productosDataGridView";
            this.productosDataGridView.ReadOnly = true;
            this.productosDataGridView.Size = new System.Drawing.Size(744, 362);
            this.productosDataGridView.TabIndex = 8;
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
            // TipoProdColumn
            // 
            this.TipoProdColumn.HeaderText = "Tipo de producto";
            this.TipoProdColumn.Name = "TipoProdColumn";
            this.TipoProdColumn.ReadOnly = true;
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
            // importeTotalLabel
            // 
            this.importeTotalLabel.AutoSize = true;
            this.importeTotalLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importeTotalLabel.Location = new System.Drawing.Point(479, 372);
            this.importeTotalLabel.Name = "importeTotalLabel";
            this.importeTotalLabel.Size = new System.Drawing.Size(87, 16);
            this.importeTotalLabel.TabIndex = 14;
            this.importeTotalLabel.Text = "Importe total: ";
            // 
            // InventarioCostoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 399);
            this.Controls.Add(this.importeTotalLabel);
            this.Controls.Add(this.productosDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InventarioCostoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Costo";
            this.Load += new System.EventHandler(this.EntradaProductosFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.productosDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView productosDataGridView;
        private System.Windows.Forms.Label importeTotalLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn CodigoColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NombreColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TipoProdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CantidadColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnidadMedidaColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrecioColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ImporteColumn;
    }
}