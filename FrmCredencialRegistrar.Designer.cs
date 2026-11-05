namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    partial class FrmCredencialRegistrar
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
            this.lblOperador = new System.Windows.Forms.Label();
            this.cbxOperador = new System.Windows.Forms.ComboBox();
            this.lblTipo = new System.Windows.Forms.Label();
            this.cbxTipo = new System.Windows.Forms.ComboBox();
            this.lblValor = new System.Windows.Forms.Label();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.lblConfirmar = new System.Windows.Forms.Label();
            this.txtConfirmar = new System.Windows.Forms.TextBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
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
            this.lblTitulo.Text = "Registrar credencial";
            //
            // lblOperador
            //
            this.lblOperador.AutoSize = true;
            this.lblOperador.ForeColor = System.Drawing.Color.Silver;
            this.lblOperador.Location = new System.Drawing.Point(24, 60);
            this.lblOperador.Name = "lblOperador";
            this.lblOperador.Text = "Operador *";
            //
            // cbxOperador
            //
            this.cbxOperador.BackColor = System.Drawing.Color.FromArgb(14, 19, 26);
            this.cbxOperador.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxOperador.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxOperador.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cbxOperador.ForeColor = System.Drawing.Color.White;
            this.cbxOperador.Location = new System.Drawing.Point(24, 78);
            this.cbxOperador.Name = "cbxOperador";
            this.cbxOperador.Size = new System.Drawing.Size(352, 24);
            this.cbxOperador.TabIndex = 1;
            //
            // lblTipo
            //
            this.lblTipo.AutoSize = true;
            this.lblTipo.ForeColor = System.Drawing.Color.Silver;
            this.lblTipo.Location = new System.Drawing.Point(24, 116);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Text = "Tipo de credencial *";
            //
            // cbxTipo
            //
            this.cbxTipo.BackColor = System.Drawing.Color.FromArgb(14, 19, 26);
            this.cbxTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTipo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxTipo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cbxTipo.ForeColor = System.Drawing.Color.White;
            this.cbxTipo.Location = new System.Drawing.Point(24, 134);
            this.cbxTipo.Name = "cbxTipo";
            this.cbxTipo.Size = new System.Drawing.Size(352, 24);
            this.cbxTipo.TabIndex = 2;
            //
            // lblValor
            //
            this.lblValor.AutoSize = true;
            this.lblValor.ForeColor = System.Drawing.Color.Silver;
            this.lblValor.Location = new System.Drawing.Point(24, 172);
            this.lblValor.Name = "lblValor";
            this.lblValor.Text = "PIN (4 números) *";
            //
            // txtValor
            //
            this.txtValor.BackColor = System.Drawing.Color.FromArgb(14, 19, 26);
            this.txtValor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.txtValor.ForeColor = System.Drawing.Color.White;
            this.txtValor.Location = new System.Drawing.Point(24, 190);
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(352, 22);
            this.txtValor.TabIndex = 3;
            //
            // lblConfirmar
            //
            this.lblConfirmar.AutoSize = true;
            this.lblConfirmar.ForeColor = System.Drawing.Color.Silver;
            this.lblConfirmar.Location = new System.Drawing.Point(24, 224);
            this.lblConfirmar.Name = "lblConfirmar";
            this.lblConfirmar.Text = "Confirmar PIN *";
            //
            // txtConfirmar
            //
            this.txtConfirmar.BackColor = System.Drawing.Color.FromArgb(14, 19, 26);
            this.txtConfirmar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConfirmar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.txtConfirmar.ForeColor = System.Drawing.Color.White;
            this.txtConfirmar.Location = new System.Drawing.Point(24, 242);
            this.txtConfirmar.MaxLength = 4;
            this.txtConfirmar.Name = "txtConfirmar";
            this.txtConfirmar.Size = new System.Drawing.Size(352, 22);
            this.txtConfirmar.TabIndex = 4;
            this.txtConfirmar.UseSystemPasswordChar = true;
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
            this.btnCancelar.Location = new System.Drawing.Point(24, 296);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(170, 38);
            this.btnCancelar.TabIndex = 6;
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
            this.btnGuardar.Location = new System.Drawing.Point(206, 296);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(170, 38);
            this.btnGuardar.TabIndex = 5;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            //
            // FrmCredencialRegistrar
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(22, 29, 37);
            this.ClientSize = new System.Drawing.Size(400, 356);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblOperador);
            this.Controls.Add(this.cbxOperador);
            this.Controls.Add(this.lblTipo);
            this.Controls.Add(this.cbxTipo);
            this.Controls.Add(this.lblValor);
            this.Controls.Add(this.txtValor);
            this.Controls.Add(this.lblConfirmar);
            this.Controls.Add(this.txtConfirmar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCredencialRegistrar";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Credencial";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblOperador;
        private System.Windows.Forms.ComboBox cbxOperador;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.ComboBox cbxTipo;
        private System.Windows.Forms.Label lblValor;
        private System.Windows.Forms.TextBox txtValor;
        private System.Windows.Forms.Label lblConfirmar;
        private System.Windows.Forms.TextBox txtConfirmar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnGuardar;
    }
}
