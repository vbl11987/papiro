﻿namespace papiro.formularios
{
    partial class IngresoPorTipoOperacion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IngresoPorTipoOperacion));
            this.filtrar = new System.Windows.Forms.Button();
            this.filtroHasta = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.filtroDesde = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.printableLV = new PrintableListView.PrintableListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // filtrar
            // 
            this.filtrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filtrar.Image = global::papiro.Properties.Resources.viewmag;
            this.filtrar.Location = new System.Drawing.Point(425, 10);
            this.filtrar.Name = "filtrar";
            this.filtrar.Size = new System.Drawing.Size(25, 25);
            this.filtrar.TabIndex = 57;
            this.filtrar.UseVisualStyleBackColor = true;
            this.filtrar.Click += new System.EventHandler(this.filtrar_Click);
            // 
            // filtroHasta
            // 
            this.filtroHasta.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filtroHasta.Checked = false;
            this.filtroHasta.CustomFormat = "dd/MM/yyyy";
            this.filtroHasta.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filtroHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.filtroHasta.Location = new System.Drawing.Point(317, 12);
            this.filtroHasta.Name = "filtroHasta";
            this.filtroHasta.Size = new System.Drawing.Size(102, 21);
            this.filtroHasta.TabIndex = 56;
            this.filtroHasta.Value = new System.DateTime(2012, 2, 24, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(271, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 16);
            this.label2.TabIndex = 55;
            this.label2.Text = "Hasta";
            // 
            // filtroDesde
            // 
            this.filtroDesde.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filtroDesde.Checked = false;
            this.filtroDesde.CustomFormat = "dd/MM/yyyy";
            this.filtroDesde.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filtroDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.filtroDesde.Location = new System.Drawing.Point(170, 12);
            this.filtroDesde.Name = "filtroDesde";
            this.filtroDesde.Size = new System.Drawing.Size(103, 21);
            this.filtroDesde.TabIndex = 54;
            this.filtroDesde.Value = new System.DateTime(2012, 2, 24, 0, 0, 0, 0);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(2, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 16);
            this.label1.TabIndex = 53;
            this.label1.Text = "Opciones de filtro:  Desde";
            // 
            // printableLV
            // 
            this.printableLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.printableLV.FitToPage = false;
            this.printableLV.GridLines = true;
            this.printableLV.Location = new System.Drawing.Point(5, 41);
            this.printableLV.Name = "printableLV";
            this.printableLV.Size = new System.Drawing.Size(540, 313);
            this.printableLV.TabIndex = 58;
            this.printableLV.Title = "";
            this.printableLV.UseCompatibleStateImageBehavior = false;
            this.printableLV.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Operación";
            this.columnHeader1.Width = 207;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Tipo de operación";
            this.columnHeader4.Width = 134;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Ingresos";
            this.columnHeader5.Width = 99;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Fecha";
            this.columnHeader6.Width = 95;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(389, 364);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(88, 23);
            this.button3.TabIndex = 61;
            this.button3.Text = "Imprimir";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(274, 364);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(109, 23);
            this.button2.TabIndex = 60;
            this.button2.Text = "Vista Previa";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Image = global::papiro.Properties.Resources.exit;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(483, 364);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(63, 23);
            this.button1.TabIndex = 59;
            this.button1.Text = "Salir";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // IngresoPorTipoOperacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 399);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.printableLV);
            this.Controls.Add(this.filtrar);
            this.Controls.Add(this.filtroHasta);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.filtroDesde);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "IngresoPorTipoOperacion";
            this.Text = "Ingresos por tipo de operación";
            this.Load += new System.EventHandler(this.IngresoPorTipoOperacion_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button filtrar;
        private System.Windows.Forms.DateTimePicker filtroHasta;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker filtroDesde;
        private System.Windows.Forms.Label label1;
        private PrintableListView.PrintableListView printableLV;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}