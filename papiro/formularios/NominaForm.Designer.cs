using System;

namespace papiro.formularios
{
    partial class NominaForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NominaForm));
            this.productosDataGridView = new System.Windows.Forms.DataGridView();
            this.nombreColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.salarioFijoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comisionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.plusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.operacionesColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.efectivoEncomboBox = new System.Windows.Forms.ComboBox();
            this.pagarButton = new System.Windows.Forms.Button();
            this.salirbutton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.productosDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // productosDataGridView
            // 
            this.productosDataGridView.AllowUserToAddRows = false;
            this.productosDataGridView.AllowUserToDeleteRows = false;
            this.productosDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.productosDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nombreColumn,
            this.salarioFijoColumn,
            this.comisionColumn,
            this.plusColumn,
            this.TotalColumn,
            this.operacionesColumn});
            this.productosDataGridView.Dock = System.Windows.Forms.DockStyle.Top;
            this.productosDataGridView.Location = new System.Drawing.Point(0, 0);
            this.productosDataGridView.Name = "productosDataGridView";
            this.productosDataGridView.Size = new System.Drawing.Size(744, 362);
            this.productosDataGridView.TabIndex = 8;
            this.productosDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ProductosDataGridViewCellClick);
            this.productosDataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.ProductosDataGridViewCellEndEdit);
            this.productosDataGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.ProductosDataGridViewCellValidating);
            // 
            // nombreColumn
            // 
            this.nombreColumn.HeaderText = "Nombre";
            this.nombreColumn.Name = "nombreColumn";
            this.nombreColumn.ReadOnly = true;
            this.nombreColumn.Width = 200;
            // 
            // salarioFijoColumn
            // 
            this.salarioFijoColumn.HeaderText = "Salario fijo";
            this.salarioFijoColumn.Name = "salarioFijoColumn";
            this.salarioFijoColumn.ReadOnly = true;
            // 
            // comisionColumn
            // 
            this.comisionColumn.HeaderText = "Comisión";
            this.comisionColumn.Name = "comisionColumn";
            this.comisionColumn.ReadOnly = true;
            // 
            // plusColumn
            // 
            this.plusColumn.HeaderText = "Plus";
            this.plusColumn.Name = "plusColumn";
            // 
            // TotalColumn
            // 
            this.TotalColumn.HeaderText = "Total";
            this.TotalColumn.Name = "TotalColumn";
            this.TotalColumn.ReadOnly = true;
            // 
            // operacionesColumn
            // 
            this.operacionesColumn.HeaderText = "Operaciones";
            this.operacionesColumn.Name = "operacionesColumn";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(322, 375);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 16);
            this.label3.TabIndex = 48;
            this.label3.Text = "Efectivo en";
            // 
            // efectivoEncomboBox
            // 
            this.efectivoEncomboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.efectivoEncomboBox.FormattingEnabled = true;
            this.efectivoEncomboBox.Items.AddRange(new object[] {
            "(Seleccione)",
            "Banco",
            "Caja"});
            this.efectivoEncomboBox.Location = new System.Drawing.Point(402, 372);
            this.efectivoEncomboBox.Name = "efectivoEncomboBox";
            this.efectivoEncomboBox.Size = new System.Drawing.Size(121, 24);
            this.efectivoEncomboBox.TabIndex = 47;
            this.efectivoEncomboBox.Text = "(Seleccione)";
            // 
            // pagarButton
            // 
            this.pagarButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pagarButton.Image = global::papiro.Properties.Resources.business;
            this.pagarButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.pagarButton.Location = new System.Drawing.Point(565, 372);
            this.pagarButton.Name = "pagarButton";
            this.pagarButton.Size = new System.Drawing.Size(75, 23);
            this.pagarButton.TabIndex = 10;
            this.pagarButton.Text = "Pagar";
            this.pagarButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.pagarButton.UseVisualStyleBackColor = true;
            this.pagarButton.Click += new System.EventHandler(this.PagarButtonClick);
            // 
            // salirbutton
            // 
            this.salirbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.salirbutton.Image = global::papiro.Properties.Resources.exit;
            this.salirbutton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.salirbutton.Location = new System.Drawing.Point(657, 372);
            this.salirbutton.Name = "salirbutton";
            this.salirbutton.Size = new System.Drawing.Size(75, 23);
            this.salirbutton.TabIndex = 9;
            this.salirbutton.Text = "Salir";
            this.salirbutton.UseVisualStyleBackColor = true;
            this.salirbutton.Click += new System.EventHandler(this.SalirbuttonClick);
            // 
            // button1
            // 
            this.button1.Image = global::papiro.Properties.Resources.agt_add_to_autorun;
            this.button1.Location = new System.Drawing.Point(13, 367);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(29, 24);
            this.button1.TabIndex = 49;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // NominaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 403);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.efectivoEncomboBox);
            this.Controls.Add(this.pagarButton);
            this.Controls.Add(this.salirbutton);
            this.Controls.Add(this.productosDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NominaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nómina";
            this.Load += new System.EventHandler(this.EntradaProductosFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.productosDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView productosDataGridView;
        private System.Windows.Forms.Button salirbutton;
        private System.Windows.Forms.Button pagarButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox efectivoEncomboBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombreColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn salarioFijoColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn comisionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn plusColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalColumn;
        private System.Windows.Forms.DataGridViewButtonColumn operacionesColumn;
        private System.Windows.Forms.Button button1;
    }
}