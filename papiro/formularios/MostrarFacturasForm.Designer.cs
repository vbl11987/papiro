using System;

namespace papiro.formularios
{
    partial class MostrarFacturasForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MostrarFacturasForm));
            this.facturasDataGridView = new System.Windows.Forms.DataGridView();
            this.CodigoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FechaColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.operacionesColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.salirbutton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.facturasDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // facturasDataGridView
            // 
            this.facturasDataGridView.AllowUserToAddRows = false;
            this.facturasDataGridView.AllowUserToDeleteRows = false;
            this.facturasDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.facturasDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CodigoColumn,
            this.FechaColumn,
            this.operacionesColumn});
            this.facturasDataGridView.Dock = System.Windows.Forms.DockStyle.Top;
            this.facturasDataGridView.Location = new System.Drawing.Point(0, 0);
            this.facturasDataGridView.Name = "facturasDataGridView";
            this.facturasDataGridView.ReadOnly = true;
            this.facturasDataGridView.Size = new System.Drawing.Size(397, 362);
            this.facturasDataGridView.TabIndex = 8;
            this.facturasDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ProductosDataGridViewCellClick);
            // 
            // CodigoColumn
            // 
            this.CodigoColumn.HeaderText = "Código";
            this.CodigoColumn.Name = "CodigoColumn";
            this.CodigoColumn.ReadOnly = true;
            // 
            // FechaColumn
            // 
            this.FechaColumn.HeaderText = "Fecha";
            this.FechaColumn.Name = "FechaColumn";
            this.FechaColumn.ReadOnly = true;
            this.FechaColumn.Width = 150;
            // 
            // operacionesColumn
            // 
            this.operacionesColumn.HeaderText = "Abrir";
            this.operacionesColumn.Name = "operacionesColumn";
            this.operacionesColumn.ReadOnly = true;
            // 
            // salirbutton
            // 
            this.salirbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.salirbutton.Image = global::papiro.Properties.Resources.exit;
            this.salirbutton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.salirbutton.Location = new System.Drawing.Point(288, 368);
            this.salirbutton.Name = "salirbutton";
            this.salirbutton.Size = new System.Drawing.Size(103, 23);
            this.salirbutton.TabIndex = 9;
            this.salirbutton.Text = "Salir";
            this.salirbutton.UseVisualStyleBackColor = true;
            this.salirbutton.Click += new System.EventHandler(this.SalirbuttonClick);
            // 
            // MostrarFacturasForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 397);
            this.Controls.Add(this.salirbutton);
            this.Controls.Add(this.facturasDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MostrarFacturasForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Facturas asociadas al contrato";
            this.Load += new System.EventHandler(this.EntradaProductosFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.facturasDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView facturasDataGridView;
        private System.Windows.Forms.Button salirbutton;
        private System.Windows.Forms.DataGridViewTextBoxColumn CodigoColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FechaColumn;
        private System.Windows.Forms.DataGridViewButtonColumn operacionesColumn;
    }
}