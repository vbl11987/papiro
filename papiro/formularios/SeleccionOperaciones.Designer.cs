using System;

namespace papiro.formularios
{
    partial class SeleccionOperaciones
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SeleccionOperaciones));
            this.productosDataGridView = new System.Windows.Forms.DataGridView();
            this.SeleccioneColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DescripcionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TipOpColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClienteColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MontoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FechaColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aceptarbutton = new System.Windows.Forms.Button();
            this.cancelbutton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.productosDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // productosDataGridView
            // 
            this.productosDataGridView.AllowUserToAddRows = false;
            this.productosDataGridView.AllowUserToDeleteRows = false;
            this.productosDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.productosDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SeleccioneColumn,
            this.DescripcionColumn,
            this.TipOpColumn,
            this.ClienteColumn,
            this.MontoColumn,
            this.FechaColumn});
            this.productosDataGridView.Dock = System.Windows.Forms.DockStyle.Top;
            this.productosDataGridView.Location = new System.Drawing.Point(0, 0);
            this.productosDataGridView.Name = "productosDataGridView";
            this.productosDataGridView.Size = new System.Drawing.Size(644, 342);
            this.productosDataGridView.TabIndex = 8;
            // 
            // SeleccioneColumn
            // 
            this.SeleccioneColumn.HeaderText = "Seleccione";
            this.SeleccioneColumn.Name = "SeleccioneColumn";
            // 
            // DescripcionColumn
            // 
            this.DescripcionColumn.HeaderText = "Descripción";
            this.DescripcionColumn.Name = "DescripcionColumn";
            this.DescripcionColumn.ReadOnly = true;
            // 
            // TipOpColumn
            // 
            this.TipOpColumn.HeaderText = "Tipo de operación";
            this.TipOpColumn.Name = "TipOpColumn";
            this.TipOpColumn.ReadOnly = true;
            // 
            // ClienteColumn
            // 
            this.ClienteColumn.HeaderText = "Cliente";
            this.ClienteColumn.Name = "ClienteColumn";
            this.ClienteColumn.ReadOnly = true;
            // 
            // MontoColumn
            // 
            this.MontoColumn.HeaderText = "Monto";
            this.MontoColumn.Name = "MontoColumn";
            this.MontoColumn.ReadOnly = true;
            // 
            // FechaColumn
            // 
            this.FechaColumn.HeaderText = "Fecha";
            this.FechaColumn.Name = "FechaColumn";
            this.FechaColumn.ReadOnly = true;
            // 
            // aceptarbutton
            // 
            this.aceptarbutton.Image = global::papiro.Properties.Resources.ok;
            this.aceptarbutton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.aceptarbutton.Location = new System.Drawing.Point(445, 350);
            this.aceptarbutton.Name = "aceptarbutton";
            this.aceptarbutton.Size = new System.Drawing.Size(86, 23);
            this.aceptarbutton.TabIndex = 9;
            this.aceptarbutton.Text = "Aceptar";
            this.aceptarbutton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.aceptarbutton.UseVisualStyleBackColor = true;
            this.aceptarbutton.Click += new System.EventHandler(this.AceptarbuttonClick);
            // 
            // cancelbutton
            // 
            this.cancelbutton.Image = global::papiro.Properties.Resources.exit;
            this.cancelbutton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cancelbutton.Location = new System.Drawing.Point(547, 350);
            this.cancelbutton.Name = "cancelbutton";
            this.cancelbutton.Size = new System.Drawing.Size(86, 23);
            this.cancelbutton.TabIndex = 10;
            this.cancelbutton.Text = "Cancelar";
            this.cancelbutton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cancelbutton.UseVisualStyleBackColor = true;
            this.cancelbutton.Click += new System.EventHandler(this.Cancelbutton1Click);
            // 
            // SeleccionOperaciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 378);
            this.Controls.Add(this.cancelbutton);
            this.Controls.Add(this.aceptarbutton);
            this.Controls.Add(this.productosDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SeleccionOperaciones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Operaciones del contrato";
            this.Load += new System.EventHandler(this.EntradaProductosFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.productosDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView productosDataGridView;
        private System.Windows.Forms.Button aceptarbutton;
        private System.Windows.Forms.Button cancelbutton;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SeleccioneColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DescripcionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TipOpColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClienteColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MontoColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FechaColumn;
    }
}