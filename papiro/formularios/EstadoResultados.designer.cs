namespace papiro.formularios
{
    partial class EstadoResultados
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EstadoResultados));
            this.label1 = new System.Windows.Forms.Label();
            this.filtroDesde = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.filtroHasta = new System.Windows.Forms.DateTimePicker();
            this.printableLV = new PrintableListView.PrintableListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.verdetalles = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.filtrar = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
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
            this.filtroDesde.Location = new System.Drawing.Point(176, 7);
            this.filtroDesde.Name = "filtroDesde";
            this.filtroDesde.Size = new System.Drawing.Size(103, 21);
            this.filtroDesde.TabIndex = 39;
            this.filtroDesde.Value = new System.DateTime(2012, 2, 24, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(281, 9);
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
            this.filtroHasta.Location = new System.Drawing.Point(327, 7);
            this.filtroHasta.Name = "filtroHasta";
            this.filtroHasta.Size = new System.Drawing.Size(102, 21);
            this.filtroHasta.TabIndex = 41;
            this.filtroHasta.Value = new System.DateTime(2012, 2, 24, 0, 0, 0, 0);
            // 
            // printableLV
            // 
            this.printableLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.printableLV.FitToPage = false;
            this.printableLV.GridLines = true;
            this.printableLV.Location = new System.Drawing.Point(2, 34);
            this.printableLV.Name = "printableLV";
            this.printableLV.Size = new System.Drawing.Size(384, 212);
            this.printableLV.TabIndex = 51;
            this.printableLV.Title = "";
            this.printableLV.UseCompatibleStateImageBehavior = false;
            this.printableLV.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 285;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Valor";
            this.columnHeader2.Width = 93;
            // 
            // button5
            // 
            this.button5.Image = global::papiro.Properties.Resources.viewmag;
            this.button5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button5.Location = new System.Drawing.Point(392, 94);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(68, 19);
            this.button5.TabIndex = 56;
            this.button5.Text = "Ingresos";
            this.button5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Image = global::papiro.Properties.Resources.viewmag;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.Location = new System.Drawing.Point(392, 76);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(68, 19);
            this.button4.TabIndex = 55;
            this.button4.Text = "Gastos";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // verdetalles
            // 
            this.verdetalles.Image = global::papiro.Properties.Resources.viewmag;
            this.verdetalles.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.verdetalles.Location = new System.Drawing.Point(392, 58);
            this.verdetalles.Name = "verdetalles";
            this.verdetalles.Size = new System.Drawing.Size(68, 19);
            this.verdetalles.TabIndex = 54;
            this.verdetalles.Text = "Costos";
            this.verdetalles.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.verdetalles.UseVisualStyleBackColor = true;
            this.verdetalles.Click += new System.EventHandler(this.verdetalles_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(298, 252);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(88, 23);
            this.button3.TabIndex = 53;
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
            this.button2.Location = new System.Drawing.Point(183, 252);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(109, 23);
            this.button2.TabIndex = 52;
            this.button2.Text = "Vista Previa";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Image = global::papiro.Properties.Resources.exit;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(398, 252);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 23);
            this.button1.TabIndex = 49;
            this.button1.Text = "Salir";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // filtrar
            // 
            this.filtrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filtrar.Image = global::papiro.Properties.Resources.viewmag;
            this.filtrar.Location = new System.Drawing.Point(435, 5);
            this.filtrar.Name = "filtrar";
            this.filtrar.Size = new System.Drawing.Size(25, 25);
            this.filtrar.TabIndex = 42;
            this.filtrar.UseVisualStyleBackColor = true;
            this.filtrar.Click += new System.EventHandler(this.FiltrarClick);
            // 
            // button6
            // 
            this.button6.Image = global::papiro.Properties.Resources.viewmag;
            this.button6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button6.Location = new System.Drawing.Point(392, 112);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(68, 19);
            this.button6.TabIndex = 57;
            this.button6.Text = "Utilidad";
            this.button6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // EstadoResultados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 287);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.verdetalles);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.printableLV);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.filtrar);
            this.Controls.Add(this.filtroHasta);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.filtroDesde);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EstadoResultados";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Estado de resultados";
            this.Load += new System.EventHandler(this.EstadoResultadosLoad);
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
        private PrintableListView.PrintableListView printableLV;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button verdetalles;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
    }
}