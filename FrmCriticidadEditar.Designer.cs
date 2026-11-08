namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    partial class FrmCriticidadEditar
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblConfirmaciones = new System.Windows.Forms.Label();
            this.numConfirmaciones = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numConfirmaciones)).BeginInit();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.numTimeout = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(24, 18);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Nivel";
            //
            // lblConfirmaciones
            //
            this.lblConfirmaciones.AutoSize = true;
            this.lblConfirmaciones.ForeColor = System.Drawing.Color.Silver;
            this.lblConfirmaciones.Location = new System.Drawing.Point(24, 60);
            this.lblConfirmaciones.Name = "lblConfirmaciones";
            this.lblConfirmaciones.Text = "Número de confirmaciones (0 a 2)";
            //
            // numConfirmaciones
            //
            this.numConfirmaciones.BackColor = System.Drawing.Color.FromArgb(14, 19, 26);
            this.numConfirmaciones.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numConfirmaciones.DecimalPlaces = 0;
            this.numConfirmaciones.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.numConfirmaciones.ForeColor = System.Drawing.Color.White;
            this.numConfirmaciones.Location = new System.Drawing.Point(24, 78);
            this.numConfirmaciones.Maximum = new decimal(new int[] { 2, 0, 0, 0 });
            this.numConfirmaciones.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            this.numConfirmaciones.Name = "numConfirmaciones";
            this.numConfirmaciones.Size = new System.Drawing.Size(352, 22);
            this.numConfirmaciones.TabIndex = 1;
            //
            // lblTimeout
            //
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.ForeColor = System.Drawing.Color.Silver;
            this.lblTimeout.Location = new System.Drawing.Point(24, 112);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Text = "Tiempo para responder cada confirmación (segundos)";
            //
            // numTimeout
            //
            this.numTimeout.BackColor = System.Drawing.Color.FromArgb(14, 19, 26);
            this.numTimeout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numTimeout.DecimalPlaces = 0;
            this.numTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.numTimeout.ForeColor = System.Drawing.Color.White;
            this.numTimeout.Location = new System.Drawing.Point(24, 130);
            this.numTimeout.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            this.numTimeout.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            this.numTimeout.Name = "numTimeout";
            this.numTimeout.Size = new System.Drawing.Size(352, 22);
            this.numTimeout.TabIndex = 2;
            //
            // lblDescripcion
            //
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.ForeColor = System.Drawing.Color.Silver;
            this.lblDescripcion.Location = new System.Drawing.Point(24, 164);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Text = "Descripción";
            //
            // txtDescripcion
            //
            this.txtDescripcion.BackColor = System.Drawing.Color.FromArgb(14, 19, 26);
            this.txtDescripcion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescripcion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.txtDescripcion.ForeColor = System.Drawing.Color.White;
            this.txtDescripcion.Location = new System.Drawing.Point(24, 182);
            this.txtDescripcion.MaxLength = 250;
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(352, 60);
            this.txtDescripcion.TabIndex = 3;
            //
            // btnCancelar
            //
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(32, 41, 56);
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(24, 264);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(170, 38);
            this.btnCancelar.TabIndex = 5;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            //
            // btnGuardar
            //
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(45, 43, 105);
            this.btnGuardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGuardar.FlatAppearance.BorderSize = 0;
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(206, 264);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(170, 38);
            this.btnGuardar.TabIndex = 4;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            //
            // FrmCriticidadEditar
            //
            this.AcceptButton = this.btnGuardar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(22, 29, 37);
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(400, 324);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblConfirmaciones);
            this.Controls.Add(this.numConfirmaciones);
            this.Controls.Add(this.lblTimeout);
            this.Controls.Add(this.numTimeout);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCriticidadEditar";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Criticidad";
            this.Load += new System.EventHandler(this.FrmCriticidadEditar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numConfirmaciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblConfirmaciones;
        private System.Windows.Forms.NumericUpDown numConfirmaciones;
        private System.Windows.Forms.Label lblTimeout;
        private System.Windows.Forms.NumericUpDown numTimeout;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
