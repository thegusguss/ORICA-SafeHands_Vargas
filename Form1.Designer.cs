namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    partial class frmLogin
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.pnlBarraSuperior = new System.Windows.Forms.Panel();
            this.pcbIcon = new System.Windows.Forms.PictureBox();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.btnMinimizar = new System.Windows.Forms.Button();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblNombre = new System.Windows.Forms.Label();
            this.pcbIcon2 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblInicioSesion = new System.Windows.Forms.Label();
            this.lblIndicacion = new System.Windows.Forms.Label();
            this.lblOperador = new System.Windows.Forms.Label();
            this.cbxOperador = new System.Windows.Forms.ComboBox();
            this.lblPIN = new System.Windows.Forms.Label();
            this.txtPIN = new System.Windows.Forms.TextBox();
            this.btnIngresar = new System.Windows.Forms.Button();
            this.btnRFID = new System.Windows.Forms.Button();
            this.lblAyuda = new System.Windows.Forms.Label();
            this.lblDemo = new System.Windows.Forms.Label();
            this.pnlBarraSuperior.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbIcon)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbIcon2)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBarraSuperior
            // 
            this.pnlBarraSuperior.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(40)))));
            this.pnlBarraSuperior.Controls.Add(this.pcbIcon);
            this.pnlBarraSuperior.Controls.Add(this.btnCerrar);
            this.pnlBarraSuperior.Controls.Add(this.btnMinimizar);
            this.pnlBarraSuperior.Controls.Add(this.lblTitulo);
            this.pnlBarraSuperior.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBarraSuperior.Location = new System.Drawing.Point(0, 0);
            this.pnlBarraSuperior.Name = "pnlBarraSuperior";
            this.pnlBarraSuperior.Size = new System.Drawing.Size(936, 40);
            this.pnlBarraSuperior.TabIndex = 0;
            // 
            // pcbIcon
            // 
            this.pcbIcon.Image = global::PROYECTO_ORICA_SAFEHANDS_VARGAS.Properties.Resources.Logo_OS_blanco_sobre_fondo_transparente__1_;
            this.pcbIcon.Location = new System.Drawing.Point(12, 6);
            this.pcbIcon.Name = "pcbIcon";
            this.pcbIcon.Size = new System.Drawing.Size(33, 31);
            this.pcbIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbIcon.TabIndex = 1;
            this.pcbIcon.TabStop = false;
            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Location = new System.Drawing.Point(884, 5);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(40, 32);
            this.btnCerrar.TabIndex = 2;
            this.btnCerrar.Text = "X";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // btnMinimizar
            // 
            this.btnMinimizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimizar.FlatAppearance.BorderSize = 0;
            this.btnMinimizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimizar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimizar.ForeColor = System.Drawing.Color.White;
            this.btnMinimizar.Location = new System.Drawing.Point(838, 5);
            this.btnMinimizar.Name = "btnMinimizar";
            this.btnMinimizar.Size = new System.Drawing.Size(40, 32);
            this.btnMinimizar.TabIndex = 1;
            this.btnMinimizar.Text = "—";
            this.btnMinimizar.UseVisualStyleBackColor = true;
            this.btnMinimizar.Click += new System.EventHandler(this.btnMinimizar_Click);
            this.btnMinimizar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnMinimizar_MouseDown);
            this.btnMinimizar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnMinimizar_MouseMove);
            this.btnMinimizar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnMinimizar_MouseUp);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(51, 13);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(222, 17);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "ORICA SafeHands — Inicio de sesión";
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::PROYECTO_ORICA_SAFEHANDS_VARGAS.Properties.Resources.Fondo_azul_profundo_con_ondas_sutiles;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lblNombre);
            this.panel1.Controls.Add(this.pcbIcon2);
            this.panel1.Location = new System.Drawing.Point(-10, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(472, 643);
            this.panel1.TabIndex = 4;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.BackColor = System.Drawing.Color.Transparent;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombre.ForeColor = System.Drawing.Color.White;
            this.lblNombre.Location = new System.Drawing.Point(127, 103);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(188, 30);
            this.lblNombre.TabIndex = 5;
            this.lblNombre.Text = "ORICA SafeHands";
            // 
            // pcbIcon2
            // 
            this.pcbIcon2.BackColor = System.Drawing.Color.Transparent;
            this.pcbIcon2.Image = global::PROYECTO_ORICA_SAFEHANDS_VARGAS.Properties.Resources.Logo_OS_blanco_sobre_fondo_transparente__1_;
            this.pcbIcon2.Location = new System.Drawing.Point(54, 87);
            this.pcbIcon2.Name = "pcbIcon2";
            this.pcbIcon2.Size = new System.Drawing.Size(67, 65);
            this.pcbIcon2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbIcon2.TabIndex = 4;
            this.pcbIcon2.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(32, 201);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(361, 47);
            this.label2.TabIndex = 6;
            this.label2.Text = "Control manos libres";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(32, 248);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(422, 47);
            this.label3.TabIndex = 7;
            this.label3.Text = "con seguridad por capas";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(32, 286);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 47);
            this.label4.TabIndex = 8;
            this.label4.Text = "capas";
            // 
            // lblInicioSesion
            // 
            this.lblInicioSesion.AutoSize = true;
            this.lblInicioSesion.BackColor = System.Drawing.Color.Transparent;
            this.lblInicioSesion.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInicioSesion.ForeColor = System.Drawing.Color.White;
            this.lblInicioSesion.Location = new System.Drawing.Point(491, 135);
            this.lblInicioSesion.Name = "lblInicioSesion";
            this.lblInicioSesion.Size = new System.Drawing.Size(168, 30);
            this.lblInicioSesion.TabIndex = 6;
            this.lblInicioSesion.Text = "Inicio de Sesión";
            // 
            // lblIndicacion
            // 
            this.lblIndicacion.AutoSize = true;
            this.lblIndicacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIndicacion.ForeColor = System.Drawing.Color.White;
            this.lblIndicacion.Location = new System.Drawing.Point(492, 181);
            this.lblIndicacion.Name = "lblIndicacion";
            this.lblIndicacion.Size = new System.Drawing.Size(425, 20);
            this.lblIndicacion.TabIndex = 7;
            this.lblIndicacion.Text = "La voz permanece bloqueada hasta autenticar al operador.";
            // 
            // lblOperador
            // 
            this.lblOperador.AutoSize = true;
            this.lblOperador.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOperador.ForeColor = System.Drawing.Color.White;
            this.lblOperador.Location = new System.Drawing.Point(560, 246);
            this.lblOperador.Name = "lblOperador";
            this.lblOperador.Size = new System.Drawing.Size(76, 20);
            this.lblOperador.TabIndex = 8;
            this.lblOperador.Text = "Operador";
            // 
            // cbxOperador
            // 
            this.cbxOperador.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(32)))), ((int)(((byte)(45)))));
            this.cbxOperador.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxOperador.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cbxOperador.FormattingEnabled = true;
            this.cbxOperador.Items.AddRange(new object[] {
            "Supervisor",
            "Administrador",
            "Técnico de mantenimiento",
            "Jefe de planta",
            "Encargado de seguridad"});
            this.cbxOperador.Location = new System.Drawing.Point(564, 283);
            this.cbxOperador.Name = "cbxOperador";
            this.cbxOperador.Size = new System.Drawing.Size(259, 21);
            this.cbxOperador.TabIndex = 9;
            // 
            // lblPIN
            // 
            this.lblPIN.AutoSize = true;
            this.lblPIN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPIN.ForeColor = System.Drawing.Color.White;
            this.lblPIN.Location = new System.Drawing.Point(560, 343);
            this.lblPIN.Name = "lblPIN";
            this.lblPIN.Size = new System.Drawing.Size(35, 20);
            this.lblPIN.TabIndex = 10;
            this.lblPIN.Text = "PIN";
            // 
            // txtPIN
            // 
            this.txtPIN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(32)))), ((int)(((byte)(45)))));
            this.txtPIN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPIN.ForeColor = System.Drawing.Color.White;
            this.txtPIN.Location = new System.Drawing.Point(564, 385);
            this.txtPIN.Name = "txtPIN";
            this.txtPIN.Size = new System.Drawing.Size(259, 20);
            this.txtPIN.TabIndex = 11;
            this.txtPIN.UseSystemPasswordChar = true;
            // 
            // btnIngresar
            // 
            this.btnIngresar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(95)))), ((int)(((byte)(220)))));
            this.btnIngresar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIngresar.FlatAppearance.BorderSize = 0;
            this.btnIngresar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIngresar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIngresar.ForeColor = System.Drawing.Color.White;
            this.btnIngresar.Location = new System.Drawing.Point(548, 437);
            this.btnIngresar.Name = "btnIngresar";
            this.btnIngresar.Size = new System.Drawing.Size(134, 44);
            this.btnIngresar.TabIndex = 12;
            this.btnIngresar.Text = "Ingresar con PIN";
            this.btnIngresar.UseVisualStyleBackColor = false;
            this.btnIngresar.Click += new System.EventHandler(this.btnIngresar_Click);
            // 
            // btnRFID
            // 
            this.btnRFID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(32)))), ((int)(((byte)(45)))));
            this.btnRFID.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRFID.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(95)))), ((int)(((byte)(220)))));
            this.btnRFID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRFID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRFID.ForeColor = System.Drawing.Color.White;
            this.btnRFID.Location = new System.Drawing.Point(717, 437);
            this.btnRFID.Name = "btnRFID";
            this.btnRFID.Size = new System.Drawing.Size(129, 44);
            this.btnRFID.TabIndex = 13;
            this.btnRFID.Text = "Simular RFID / NFC";
            this.btnRFID.UseVisualStyleBackColor = false;
            this.btnRFID.Click += new System.EventHandler(this.btnRFID_Click);
            // 
            // lblAyuda
            // 
            this.lblAyuda.AutoSize = true;
            this.lblAyuda.BackColor = System.Drawing.Color.Transparent;
            this.lblAyuda.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAyuda.ForeColor = System.Drawing.Color.White;
            this.lblAyuda.Location = new System.Drawing.Point(544, 570);
            this.lblAyuda.Name = "lblAyuda";
            this.lblAyuda.Size = new System.Drawing.Size(302, 20);
            this.lblAyuda.TabIndex = 14;
            this.lblAyuda.Text = "También puede usar Simular RFID / NFC.";
            // 
            // lblDemo
            // 
            this.lblDemo.AutoSize = true;
            this.lblDemo.BackColor = System.Drawing.Color.Transparent;
            this.lblDemo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDemo.ForeColor = System.Drawing.Color.White;
            this.lblDemo.Location = new System.Drawing.Point(544, 538);
            this.lblDemo.Name = "lblDemo";
            this.lblDemo.Size = new System.Drawing.Size(130, 20);
            this.lblDemo.TabIndex = 15;
            this.lblDemo.Text = "Demo: PIN 1234.";
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(17)))), ((int)(((byte)(23)))));
            this.ClientSize = new System.Drawing.Size(936, 676);
            this.Controls.Add(this.lblDemo);
            this.Controls.Add(this.lblAyuda);
            this.Controls.Add(this.btnRFID);
            this.Controls.Add(this.btnIngresar);
            this.Controls.Add(this.txtPIN);
            this.Controls.Add(this.lblPIN);
            this.Controls.Add(this.cbxOperador);
            this.Controls.Add(this.lblOperador);
            this.Controls.Add(this.lblIndicacion);
            this.Controls.Add(this.lblInicioSesion);
            this.Controls.Add(this.pnlBarraSuperior);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ORICA SafeHands — Inicio de sesión";
            this.TransparencyKey = System.Drawing.Color.White;
            this.pnlBarraSuperior.ResumeLayout(false);
            this.pnlBarraSuperior.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbIcon)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbIcon2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBarraSuperior;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Button btnMinimizar;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.PictureBox pcbIcon;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pcbIcon2;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblInicioSesion;
        private System.Windows.Forms.Label lblIndicacion;
        private System.Windows.Forms.Label lblOperador;
        private System.Windows.Forms.ComboBox cbxOperador;
        private System.Windows.Forms.Label lblPIN;
        private System.Windows.Forms.TextBox txtPIN;
        private System.Windows.Forms.Button btnIngresar;
        private System.Windows.Forms.Button btnRFID;
        private System.Windows.Forms.Label lblAyuda;
        private System.Windows.Forms.Label lblDemo;
    }
}

