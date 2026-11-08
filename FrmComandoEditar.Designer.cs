namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    partial class FrmComandoEditar
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
            this.lblCodigo = new System.Windows.Forms.Label();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblFrase = new System.Windows.Forms.Label();
            this.txtFrase = new System.Windows.Forms.TextBox();
            this.lblCriticidad = new System.Windows.Forms.Label();
            this.cbxCriticidad = new System.Windows.Forms.ComboBox();
            this.lblTipoAccion = new System.Windows.Forms.Label();
            this.cbxTipoAccion = new System.Windows.Forms.ComboBox();
            this.lblConfianza = new System.Windows.Forms.Label();
            this.numConfianza = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numConfianza)).BeginInit();
            this.chkPTT = new System.Windows.Forms.CheckBox();
            this.lblEquipos = new System.Windows.Forms.Label();
            this.clbEquipos = new System.Windows.Forms.CheckedListBox();
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
            this.lblTitulo.Text = "Nuevo comando";
            //
            // lblCodigo
            //
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.ForeColor = System.Drawing.Color.Silver;
            this.lblCodigo.Location = new System.Drawing.Point(24, 60);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Text = "Código *";
            //
            // txtCodigo
            //
            this.txtCodigo.BackColor = System.Drawing.Color.FromArgb(14, 19, 26);
            this.txtCodigo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCodigo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.txtCodigo.ForeColor = System.Drawing.Color.White;
            this.txtCodigo.Location = new System.Drawing.Point(24, 78);
            this.txtCodigo.MaxLength = 20;
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(392, 22);
            this.txtCodigo.TabIndex = 1;
            //
            // lblNombre
            //
            this.lblNombre.AutoSize = true;
            this.lblNombre.ForeColor = System.Drawing.Color.Silver;
            this.lblNombre.Location = new System.Drawing.Point(24, 112);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Text = "Nombre *";
            //
            // txtNombre
            //
            this.txtNombre.BackColor = System.Drawing.Color.FromArgb(14, 19, 26);
            this.txtNombre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.txtNombre.ForeColor = System.Drawing.Color.White;
            this.txtNombre.Location = new System.Drawing.Point(24, 130);
            this.txtNombre.MaxLength = 100;
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(392, 22);
            this.txtNombre.TabIndex = 2;
            //
            // lblFrase
            //
            this.lblFrase.AutoSize = true;
            this.lblFrase.ForeColor = System.Drawing.Color.Silver;
            this.lblFrase.Location = new System.Drawing.Point(24, 164);
            this.lblFrase.Name = "lblFrase";
            this.lblFrase.Text = "Frase autorizada * (lo que dirá el operador)";
            //
            // txtFrase
            //
            this.txtFrase.BackColor = System.Drawing.Color.FromArgb(14, 19, 26);
            this.txtFrase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFrase.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.txtFrase.ForeColor = System.Drawing.Color.White;
            this.txtFrase.Location = new System.Drawing.Point(24, 182);
            this.txtFrase.MaxLength = 180;
            this.txtFrase.Name = "txtFrase";
            this.txtFrase.Size = new System.Drawing.Size(392, 22);
            this.txtFrase.TabIndex = 3;
            //
            // lblCriticidad
            //
            this.lblCriticidad.AutoSize = true;
            this.lblCriticidad.ForeColor = System.Drawing.Color.Silver;
            this.lblCriticidad.Location = new System.Drawing.Point(24, 216);
            this.lblCriticidad.Name = "lblCriticidad";
            this.lblCriticidad.Text = "Nivel de criticidad *";
            //
            // cbxCriticidad
            //
            this.cbxCriticidad.BackColor = System.Drawing.Color.FromArgb(14, 19, 26);
            this.cbxCriticidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCriticidad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxCriticidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cbxCriticidad.ForeColor = System.Drawing.Color.White;
            this.cbxCriticidad.Location = new System.Drawing.Point(24, 234);
            this.cbxCriticidad.Name = "cbxCriticidad";
            this.cbxCriticidad.Size = new System.Drawing.Size(392, 24);
            this.cbxCriticidad.TabIndex = 4;
            //
            // lblTipoAccion
            //
            this.lblTipoAccion.AutoSize = true;
            this.lblTipoAccion.ForeColor = System.Drawing.Color.Silver;
            this.lblTipoAccion.Location = new System.Drawing.Point(24, 268);
            this.lblTipoAccion.Name = "lblTipoAccion";
            this.lblTipoAccion.Text = "Tipo de acción *";
            //
            // cbxTipoAccion
            //
            this.cbxTipoAccion.BackColor = System.Drawing.Color.FromArgb(14, 19, 26);
            this.cbxTipoAccion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTipoAccion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxTipoAccion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.cbxTipoAccion.ForeColor = System.Drawing.Color.White;
            this.cbxTipoAccion.Location = new System.Drawing.Point(24, 286);
            this.cbxTipoAccion.Name = "cbxTipoAccion";
            this.cbxTipoAccion.Size = new System.Drawing.Size(392, 24);
            this.cbxTipoAccion.TabIndex = 5;
            //
            // lblConfianza
            //
            this.lblConfianza.AutoSize = true;
            this.lblConfianza.ForeColor = System.Drawing.Color.Silver;
            this.lblConfianza.Location = new System.Drawing.Point(24, 320);
            this.lblConfianza.Name = "lblConfianza";
            this.lblConfianza.Text = "Confianza mínima del reconocimiento (%)";
            //
            // numConfianza
            //
            this.numConfianza.BackColor = System.Drawing.Color.FromArgb(14, 19, 26);
            this.numConfianza.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numConfianza.DecimalPlaces = 0;
            this.numConfianza.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.numConfianza.ForeColor = System.Drawing.Color.White;
            this.numConfianza.Location = new System.Drawing.Point(24, 338);
            this.numConfianza.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.numConfianza.Minimum = new decimal(new int[] { 50, 0, 0, 0 });
            this.numConfianza.Name = "numConfianza";
            this.numConfianza.Size = new System.Drawing.Size(392, 22);
            this.numConfianza.TabIndex = 6;
            //
            // chkPTT
            //
            this.chkPTT.AutoSize = true;
            this.chkPTT.ForeColor = System.Drawing.Color.Silver;
            this.chkPTT.Location = new System.Drawing.Point(24, 372);
            this.chkPTT.Name = "chkPTT";
            this.chkPTT.TabIndex = 7;
            this.chkPTT.Text = "Requiere mantener pulsado el botón PTT";
            this.chkPTT.UseVisualStyleBackColor = true;
            //
            // lblEquipos
            //
            this.lblEquipos.AutoSize = true;
            this.lblEquipos.ForeColor = System.Drawing.Color.Silver;
            this.lblEquipos.Location = new System.Drawing.Point(24, 406);
            this.lblEquipos.Name = "lblEquipos";
            this.lblEquipos.Text = "Equipos donde se puede usar *";
            //
            // clbEquipos
            //
            this.clbEquipos.BackColor = System.Drawing.Color.FromArgb(14, 19, 26);
            this.clbEquipos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clbEquipos.CheckOnClick = true;
            this.clbEquipos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.clbEquipos.ForeColor = System.Drawing.Color.White;
            this.clbEquipos.IntegralHeight = false;
            this.clbEquipos.Location = new System.Drawing.Point(24, 424);
            this.clbEquipos.Name = "clbEquipos";
            this.clbEquipos.Size = new System.Drawing.Size(392, 110);
            this.clbEquipos.TabIndex = 8;
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
            this.btnCancelar.Location = new System.Drawing.Point(24, 556);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(190, 38);
            this.btnCancelar.TabIndex = 10;
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
            this.btnGuardar.Location = new System.Drawing.Point(226, 556);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(190, 38);
            this.btnGuardar.TabIndex = 9;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            //
            // FrmComandoEditar
            //
            this.AcceptButton = this.btnGuardar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(22, 29, 37);
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(440, 616);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblCodigo);
            this.Controls.Add(this.txtCodigo);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblFrase);
            this.Controls.Add(this.txtFrase);
            this.Controls.Add(this.lblCriticidad);
            this.Controls.Add(this.cbxCriticidad);
            this.Controls.Add(this.lblTipoAccion);
            this.Controls.Add(this.cbxTipoAccion);
            this.Controls.Add(this.lblConfianza);
            this.Controls.Add(this.numConfianza);
            this.Controls.Add(this.chkPTT);
            this.Controls.Add(this.lblEquipos);
            this.Controls.Add(this.clbEquipos);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmComandoEditar";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Comando";
            this.Load += new System.EventHandler(this.FrmComandoEditar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numConfianza)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblCodigo;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblFrase;
        private System.Windows.Forms.TextBox txtFrase;
        private System.Windows.Forms.Label lblCriticidad;
        private System.Windows.Forms.ComboBox cbxCriticidad;
        private System.Windows.Forms.Label lblTipoAccion;
        private System.Windows.Forms.ComboBox cbxTipoAccion;
        private System.Windows.Forms.Label lblConfianza;
        private System.Windows.Forms.NumericUpDown numConfianza;
        private System.Windows.Forms.CheckBox chkPTT;
        private System.Windows.Forms.Label lblEquipos;
        private System.Windows.Forms.CheckedListBox clbEquipos;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
