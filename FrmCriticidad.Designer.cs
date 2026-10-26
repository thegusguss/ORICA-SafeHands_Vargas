namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    partial class FrmCriticidad
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCriticidad));
            this.flpMenu = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlMarca = new System.Windows.Forms.Panel();
            this.lblNombreSistema = new System.Windows.Forms.Label();
            this.lblModoSistema = new System.Windows.Forms.Label();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.btnDiagnostico = new System.Windows.Forms.Button();
            this.btnAlertas = new System.Windows.Forms.Button();
            this.btnCriticidad = new System.Windows.Forms.Button();
            this.btnEventos = new System.Windows.Forms.Button();
            this.btnOperadores = new System.Windows.Forms.Button();
            this.btnCredenciales = new System.Windows.Forms.Button();
            this.btnEquipos = new System.Windows.Forms.Button();
            this.btnComandos = new System.Windows.Forms.Button();
            this.btnSincronizacion = new System.Windows.Forms.Button();
            this.btnReportes = new System.Windows.Forms.Button();
            this.btnConfiguracion = new System.Windows.Forms.Button();
            this.pnlAcceso = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlCabeceraCriticidad = new System.Windows.Forms.Panel();
            this.btnParadaSimulada = new System.Windows.Forms.Button();
            this.lblEstadoDatos = new System.Windows.Forms.Label();
            this.lblConexionESP32 = new System.Windows.Forms.Label();
            this.lblDiagEstadoApp = new System.Windows.Forms.Label();
            this.lblSubtituloCriticidad = new System.Windows.Forms.Label();
            this.lblTituloCriticidad = new System.Windows.Forms.Label();
            this.pnlNivelesCriticidad = new System.Windows.Forms.Panel();
            this.lblPoliticaSeguridad = new System.Windows.Forms.Label();
            this.lblTituloNivelesCriticidad = new System.Windows.Forms.Label();
            this.dgvCriticidad = new System.Windows.Forms.DataGridView();
            this.colNivelCriticidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombreCriticidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConfirmacionesCriticidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTimeoutCriticidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescripcionCriticidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEditarCriticidad = new System.Windows.Forms.DataGridViewButtonColumn();
            this.flpMenu.SuspendLayout();
            this.pnlMarca.SuspendLayout();
            this.pnlAcceso.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlCabeceraCriticidad.SuspendLayout();
            this.pnlNivelesCriticidad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCriticidad)).BeginInit();
            this.SuspendLayout();
            // 
            // flpMenu
            // 
            this.flpMenu.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.flpMenu.AutoScroll = true;
            this.flpMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(43)))), ((int)(((byte)(105)))));
            this.flpMenu.Controls.Add(this.pictureBox1);
            this.flpMenu.Controls.Add(this.pnlMarca);
            this.flpMenu.Controls.Add(this.btnDashboard);
            this.flpMenu.Controls.Add(this.btnDiagnostico);
            this.flpMenu.Controls.Add(this.btnAlertas);
            this.flpMenu.Controls.Add(this.btnCriticidad);
            this.flpMenu.Controls.Add(this.btnEventos);
            this.flpMenu.Controls.Add(this.btnOperadores);
            this.flpMenu.Controls.Add(this.btnCredenciales);
            this.flpMenu.Controls.Add(this.btnEquipos);
            this.flpMenu.Controls.Add(this.btnComandos);
            this.flpMenu.Controls.Add(this.btnSincronizacion);
            this.flpMenu.Controls.Add(this.btnReportes);
            this.flpMenu.Controls.Add(this.btnConfiguracion);
            this.flpMenu.Controls.Add(this.pnlAcceso);
            this.flpMenu.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpMenu.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.flpMenu.Location = new System.Drawing.Point(1, 2);
            this.flpMenu.Name = "flpMenu";
            this.flpMenu.Padding = new System.Windows.Forms.Padding(10, 20, 0, 0);
            this.flpMenu.Size = new System.Drawing.Size(230, 880);
            this.flpMenu.TabIndex = 3;
            this.flpMenu.WrapContents = false;
            // 
            // pnlMarca
            // 
            this.pnlMarca.Controls.Add(this.lblNombreSistema);
            this.pnlMarca.Controls.Add(this.lblModoSistema);
            this.pnlMarca.ForeColor = System.Drawing.Color.White;
            this.pnlMarca.Location = new System.Drawing.Point(13, 99);
            this.pnlMarca.Name = "pnlMarca";
            this.pnlMarca.Size = new System.Drawing.Size(200, 63);
            this.pnlMarca.TabIndex = 16;
            // 
            // lblNombreSistema
            // 
            this.lblNombreSistema.AutoSize = true;
            this.lblNombreSistema.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombreSistema.ForeColor = System.Drawing.Color.White;
            this.lblNombreSistema.Location = new System.Drawing.Point(51, 0);
            this.lblNombreSistema.Name = "lblNombreSistema";
            this.lblNombreSistema.Size = new System.Drawing.Size(99, 20);
            this.lblNombreSistema.TabIndex = 14;
            this.lblNombreSistema.Text = "SafeHands";
            this.lblNombreSistema.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblModoSistema
            // 
            this.lblModoSistema.AutoSize = true;
            this.lblModoSistema.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModoSistema.ForeColor = System.Drawing.Color.Silver;
            this.lblModoSistema.Location = new System.Drawing.Point(33, 37);
            this.lblModoSistema.Name = "lblModoSistema";
            this.lblModoSistema.Size = new System.Drawing.Size(134, 13);
            this.lblModoSistema.TabIndex = 16;
            this.lblModoSistema.Text = "PROTOTIPO OFFLINE";
            this.lblModoSistema.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDashboard
            // 
            this.btnDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDashboard.ForeColor = System.Drawing.Color.White;
            this.btnDashboard.Location = new System.Drawing.Point(13, 168);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(200, 42);
            this.btnDashboard.TabIndex = 0;
            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashboard.UseVisualStyleBackColor = true;
            // 
            // btnDiagnostico
            // 
            this.btnDiagnostico.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDiagnostico.FlatAppearance.BorderSize = 0;
            this.btnDiagnostico.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiagnostico.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDiagnostico.ForeColor = System.Drawing.Color.White;
            this.btnDiagnostico.Location = new System.Drawing.Point(13, 216);
            this.btnDiagnostico.Name = "btnDiagnostico";
            this.btnDiagnostico.Size = new System.Drawing.Size(200, 42);
            this.btnDiagnostico.TabIndex = 1;
            this.btnDiagnostico.Text = "Diagnóstico";
            this.btnDiagnostico.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDiagnostico.UseVisualStyleBackColor = true;
            // 
            // btnAlertas
            // 
            this.btnAlertas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAlertas.FlatAppearance.BorderSize = 0;
            this.btnAlertas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlertas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlertas.ForeColor = System.Drawing.Color.White;
            this.btnAlertas.Location = new System.Drawing.Point(13, 264);
            this.btnAlertas.Name = "btnAlertas";
            this.btnAlertas.Size = new System.Drawing.Size(200, 42);
            this.btnAlertas.TabIndex = 2;
            this.btnAlertas.Text = "Alertas";
            this.btnAlertas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAlertas.UseVisualStyleBackColor = true;
            // 
            // btnCriticidad
            // 
            this.btnCriticidad.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCriticidad.FlatAppearance.BorderSize = 0;
            this.btnCriticidad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCriticidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCriticidad.ForeColor = System.Drawing.Color.White;
            this.btnCriticidad.Location = new System.Drawing.Point(13, 312);
            this.btnCriticidad.Name = "btnCriticidad";
            this.btnCriticidad.Size = new System.Drawing.Size(200, 42);
            this.btnCriticidad.TabIndex = 3;
            this.btnCriticidad.Text = "Criticidad";
            this.btnCriticidad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCriticidad.UseVisualStyleBackColor = true;
            // 
            // btnEventos
            // 
            this.btnEventos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEventos.FlatAppearance.BorderSize = 0;
            this.btnEventos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEventos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEventos.ForeColor = System.Drawing.Color.White;
            this.btnEventos.Location = new System.Drawing.Point(13, 360);
            this.btnEventos.Name = "btnEventos";
            this.btnEventos.Size = new System.Drawing.Size(200, 42);
            this.btnEventos.TabIndex = 4;
            this.btnEventos.Text = "Eventos / Auditoría";
            this.btnEventos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEventos.UseVisualStyleBackColor = true;
            // 
            // btnOperadores
            // 
            this.btnOperadores.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOperadores.FlatAppearance.BorderSize = 0;
            this.btnOperadores.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOperadores.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOperadores.ForeColor = System.Drawing.Color.White;
            this.btnOperadores.Location = new System.Drawing.Point(13, 408);
            this.btnOperadores.Name = "btnOperadores";
            this.btnOperadores.Size = new System.Drawing.Size(200, 42);
            this.btnOperadores.TabIndex = 5;
            this.btnOperadores.Text = "Operadores";
            this.btnOperadores.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOperadores.UseVisualStyleBackColor = true;
            // 
            // btnCredenciales
            // 
            this.btnCredenciales.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCredenciales.FlatAppearance.BorderSize = 0;
            this.btnCredenciales.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCredenciales.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCredenciales.ForeColor = System.Drawing.Color.White;
            this.btnCredenciales.Location = new System.Drawing.Point(13, 456);
            this.btnCredenciales.Name = "btnCredenciales";
            this.btnCredenciales.Size = new System.Drawing.Size(200, 42);
            this.btnCredenciales.TabIndex = 6;
            this.btnCredenciales.Text = "Credenciales";
            this.btnCredenciales.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCredenciales.UseVisualStyleBackColor = true;
            // 
            // btnEquipos
            // 
            this.btnEquipos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEquipos.FlatAppearance.BorderSize = 0;
            this.btnEquipos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEquipos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEquipos.ForeColor = System.Drawing.Color.White;
            this.btnEquipos.Location = new System.Drawing.Point(13, 504);
            this.btnEquipos.Name = "btnEquipos";
            this.btnEquipos.Size = new System.Drawing.Size(200, 42);
            this.btnEquipos.TabIndex = 7;
            this.btnEquipos.Text = "Equipos";
            this.btnEquipos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEquipos.UseVisualStyleBackColor = true;
            // 
            // btnComandos
            // 
            this.btnComandos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnComandos.FlatAppearance.BorderSize = 0;
            this.btnComandos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComandos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnComandos.ForeColor = System.Drawing.Color.White;
            this.btnComandos.Location = new System.Drawing.Point(13, 552);
            this.btnComandos.Name = "btnComandos";
            this.btnComandos.Size = new System.Drawing.Size(200, 42);
            this.btnComandos.TabIndex = 8;
            this.btnComandos.Text = "Comandos";
            this.btnComandos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnComandos.UseVisualStyleBackColor = true;
            // 
            // btnSincronizacion
            // 
            this.btnSincronizacion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSincronizacion.FlatAppearance.BorderSize = 0;
            this.btnSincronizacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSincronizacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSincronizacion.ForeColor = System.Drawing.Color.White;
            this.btnSincronizacion.Location = new System.Drawing.Point(13, 600);
            this.btnSincronizacion.Name = "btnSincronizacion";
            this.btnSincronizacion.Size = new System.Drawing.Size(200, 42);
            this.btnSincronizacion.TabIndex = 9;
            this.btnSincronizacion.Text = "Sincronización";
            this.btnSincronizacion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSincronizacion.UseVisualStyleBackColor = true;
            // 
            // btnReportes
            // 
            this.btnReportes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReportes.FlatAppearance.BorderSize = 0;
            this.btnReportes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReportes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReportes.ForeColor = System.Drawing.Color.White;
            this.btnReportes.Location = new System.Drawing.Point(13, 648);
            this.btnReportes.Name = "btnReportes";
            this.btnReportes.Size = new System.Drawing.Size(200, 42);
            this.btnReportes.TabIndex = 10;
            this.btnReportes.Text = "Reportes";
            this.btnReportes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReportes.UseVisualStyleBackColor = true;
            // 
            // btnConfiguracion
            // 
            this.btnConfiguracion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfiguracion.FlatAppearance.BorderSize = 0;
            this.btnConfiguracion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfiguracion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfiguracion.ForeColor = System.Drawing.Color.White;
            this.btnConfiguracion.Location = new System.Drawing.Point(13, 696);
            this.btnConfiguracion.Name = "btnConfiguracion";
            this.btnConfiguracion.Size = new System.Drawing.Size(200, 42);
            this.btnConfiguracion.TabIndex = 11;
            this.btnConfiguracion.Text = "Configuración";
            this.btnConfiguracion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfiguracion.UseVisualStyleBackColor = true;
            // 
            // pnlAcceso
            // 
            this.pnlAcceso.Controls.Add(this.label1);
            this.pnlAcceso.Controls.Add(this.button1);
            this.pnlAcceso.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAcceso.Location = new System.Drawing.Point(13, 744);
            this.pnlAcceso.Name = "pnlAcceso";
            this.pnlAcceso.Size = new System.Drawing.Size(200, 100);
            this.pnlAcceso.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(7, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 16);
            this.label1.TabIndex = 21;
            this.label1.Text = "Sin sesión";
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(3, 48);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(194, 40);
            this.button1.TabIndex = 20;
            this.button1.Text = "Cerrar sesión";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PROYECTO_ORICA_SAFEHANDS_VARGAS.Properties.Resources.Placa_blanca_con_logo_ORICA_azul;
            this.pictureBox1.Location = new System.Drawing.Point(13, 23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 70);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // pnlCabeceraCriticidad
            // 
            this.pnlCabeceraCriticidad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(27)))), ((int)(((byte)(36)))));
            this.pnlCabeceraCriticidad.Controls.Add(this.btnParadaSimulada);
            this.pnlCabeceraCriticidad.Controls.Add(this.lblEstadoDatos);
            this.pnlCabeceraCriticidad.Controls.Add(this.lblConexionESP32);
            this.pnlCabeceraCriticidad.Controls.Add(this.lblDiagEstadoApp);
            this.pnlCabeceraCriticidad.Controls.Add(this.lblSubtituloCriticidad);
            this.pnlCabeceraCriticidad.Controls.Add(this.lblTituloCriticidad);
            this.pnlCabeceraCriticidad.Location = new System.Drawing.Point(250, 20);
            this.pnlCabeceraCriticidad.Name = "pnlCabeceraCriticidad";
            this.pnlCabeceraCriticidad.Size = new System.Drawing.Size(973, 65);
            this.pnlCabeceraCriticidad.TabIndex = 4;
            // 
            // btnParadaSimulada
            // 
            this.btnParadaSimulada.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.btnParadaSimulada.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnParadaSimulada.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnParadaSimulada.ForeColor = System.Drawing.Color.White;
            this.btnParadaSimulada.Location = new System.Drawing.Point(773, 18);
            this.btnParadaSimulada.Name = "btnParadaSimulada";
            this.btnParadaSimulada.Size = new System.Drawing.Size(180, 36);
            this.btnParadaSimulada.TabIndex = 6;
            this.btnParadaSimulada.Text = "■ PARADA SIMULADA";
            this.btnParadaSimulada.UseVisualStyleBackColor = false;
            // 
            // lblEstadoDatos
            // 
            this.lblEstadoDatos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEstadoDatos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(23)))), ((int)(((byte)(32)))));
            this.lblEstadoDatos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEstadoDatos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstadoDatos.ForeColor = System.Drawing.Color.Silver;
            this.lblEstadoDatos.Location = new System.Drawing.Point(633, 18);
            this.lblEstadoDatos.Name = "lblEstadoDatos";
            this.lblEstadoDatos.Size = new System.Drawing.Size(120, 32);
            this.lblEstadoDatos.TabIndex = 5;
            this.lblEstadoDatos.Text = "● Datos locales";
            this.lblEstadoDatos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblConexionESP32
            // 
            this.lblConexionESP32.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConexionESP32.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(23)))), ((int)(((byte)(32)))));
            this.lblConexionESP32.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblConexionESP32.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConexionESP32.ForeColor = System.Drawing.Color.Silver;
            this.lblConexionESP32.Location = new System.Drawing.Point(478, 18);
            this.lblConexionESP32.Name = "lblConexionESP32";
            this.lblConexionESP32.Size = new System.Drawing.Size(145, 32);
            this.lblConexionESP32.TabIndex = 4;
            this.lblConexionESP32.Text = "● ESP32 desconectado";
            this.lblConexionESP32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDiagEstadoApp
            // 
            this.lblDiagEstadoApp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDiagEstadoApp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(23)))), ((int)(((byte)(32)))));
            this.lblDiagEstadoApp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDiagEstadoApp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiagEstadoApp.ForeColor = System.Drawing.Color.Silver;
            this.lblDiagEstadoApp.Location = new System.Drawing.Point(347, 18);
            this.lblDiagEstadoApp.Name = "lblDiagEstadoApp";
            this.lblDiagEstadoApp.Size = new System.Drawing.Size(120, 32);
            this.lblDiagEstadoApp.TabIndex = 3;
            this.lblDiagEstadoApp.Text = "● App sin iniciar";
            this.lblDiagEstadoApp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSubtituloCriticidad
            // 
            this.lblSubtituloCriticidad.AutoSize = true;
            this.lblSubtituloCriticidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtituloCriticidad.ForeColor = System.Drawing.Color.Silver;
            this.lblSubtituloCriticidad.Location = new System.Drawing.Point(16, 39);
            this.lblSubtituloCriticidad.Name = "lblSubtituloCriticidad";
            this.lblSubtituloCriticidad.Size = new System.Drawing.Size(200, 15);
            this.lblSubtituloCriticidad.TabIndex = 2;
            this.lblSubtituloCriticidad.Text = "Política de niveles y confirmaciones";
            // 
            // lblTituloCriticidad
            // 
            this.lblTituloCriticidad.AutoSize = true;
            this.lblTituloCriticidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloCriticidad.ForeColor = System.Drawing.Color.White;
            this.lblTituloCriticidad.Location = new System.Drawing.Point(16, 11);
            this.lblTituloCriticidad.Name = "lblTituloCriticidad";
            this.lblTituloCriticidad.Size = new System.Drawing.Size(79, 18);
            this.lblTituloCriticidad.TabIndex = 1;
            this.lblTituloCriticidad.Text = "Criticidad";
            // 
            // pnlNivelesCriticidad
            // 
            this.pnlNivelesCriticidad.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlNivelesCriticidad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(29)))), ((int)(((byte)(37)))));
            this.pnlNivelesCriticidad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlNivelesCriticidad.Controls.Add(this.dgvCriticidad);
            this.pnlNivelesCriticidad.Controls.Add(this.lblPoliticaSeguridad);
            this.pnlNivelesCriticidad.Controls.Add(this.lblTituloNivelesCriticidad);
            this.pnlNivelesCriticidad.Location = new System.Drawing.Point(250, 101);
            this.pnlNivelesCriticidad.Name = "pnlNivelesCriticidad";
            this.pnlNivelesCriticidad.Size = new System.Drawing.Size(973, 543);
            this.pnlNivelesCriticidad.TabIndex = 5;
            // 
            // lblPoliticaSeguridad
            // 
            this.lblPoliticaSeguridad.AutoSize = true;
            this.lblPoliticaSeguridad.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPoliticaSeguridad.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblPoliticaSeguridad.Location = new System.Drawing.Point(16, 44);
            this.lblPoliticaSeguridad.Name = "lblPoliticaSeguridad";
            this.lblPoliticaSeguridad.Size = new System.Drawing.Size(122, 15);
            this.lblPoliticaSeguridad.TabIndex = 11;
            this.lblPoliticaSeguridad.Text = "Política de seguridad";
            // 
            // lblTituloNivelesCriticidad
            // 
            this.lblTituloNivelesCriticidad.AutoSize = true;
            this.lblTituloNivelesCriticidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloNivelesCriticidad.ForeColor = System.Drawing.Color.White;
            this.lblTituloNivelesCriticidad.Location = new System.Drawing.Point(16, 15);
            this.lblTituloNivelesCriticidad.Name = "lblTituloNivelesCriticidad";
            this.lblTituloNivelesCriticidad.Size = new System.Drawing.Size(159, 18);
            this.lblTituloNivelesCriticidad.TabIndex = 10;
            this.lblTituloNivelesCriticidad.Text = "Niveles de criticidad";
            // 
            // dgvCriticidad
            // 
            this.dgvCriticidad.AllowUserToAddRows = false;
            this.dgvCriticidad.AllowUserToDeleteRows = false;
            this.dgvCriticidad.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCriticidad.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCriticidad.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(29)))), ((int)(((byte)(37)))));
            this.dgvCriticidad.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(27)))), ((int)(((byte)(36)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCriticidad.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCriticidad.ColumnHeadersHeight = 35;
            this.dgvCriticidad.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCriticidad.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNivelCriticidad,
            this.colNombreCriticidad,
            this.colConfirmacionesCriticidad,
            this.colTimeoutCriticidad,
            this.colDescripcionCriticidad,
            this.colEditarCriticidad});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(29)))), ((int)(((byte)(37)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(43)))), ((int)(((byte)(105)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCriticidad.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCriticidad.EnableHeadersVisualStyles = false;
            this.dgvCriticidad.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(65)))));
            this.dgvCriticidad.Location = new System.Drawing.Point(19, 68);
            this.dgvCriticidad.MultiSelect = false;
            this.dgvCriticidad.Name = "dgvCriticidad";
            this.dgvCriticidad.ReadOnly = true;
            this.dgvCriticidad.RowHeadersVisible = false;
            this.dgvCriticidad.RowTemplate.Height = 45;
            this.dgvCriticidad.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCriticidad.Size = new System.Drawing.Size(934, 294);
            this.dgvCriticidad.TabIndex = 12;
            // 
            // colNivelCriticidad
            // 
            this.colNivelCriticidad.FillWeight = 70F;
            this.colNivelCriticidad.HeaderText = "NIVEL";
            this.colNivelCriticidad.Name = "colNivelCriticidad";
            this.colNivelCriticidad.ReadOnly = true;
            // 
            // colNombreCriticidad
            // 
            this.colNombreCriticidad.FillWeight = 90F;
            this.colNombreCriticidad.HeaderText = "NOMBRE";
            this.colNombreCriticidad.Name = "colNombreCriticidad";
            this.colNombreCriticidad.ReadOnly = true;
            // 
            // colConfirmacionesCriticidad
            // 
            this.colConfirmacionesCriticidad.FillWeight = 110F;
            this.colConfirmacionesCriticidad.HeaderText = "CONFIRMACIONES";
            this.colConfirmacionesCriticidad.Name = "colConfirmacionesCriticidad";
            this.colConfirmacionesCriticidad.ReadOnly = true;
            // 
            // colTimeoutCriticidad
            // 
            this.colTimeoutCriticidad.FillWeight = 70F;
            this.colTimeoutCriticidad.HeaderText = "TIMEOUT";
            this.colTimeoutCriticidad.Name = "colTimeoutCriticidad";
            this.colTimeoutCriticidad.ReadOnly = true;
            // 
            // colDescripcionCriticidad
            // 
            this.colDescripcionCriticidad.FillWeight = 240F;
            this.colDescripcionCriticidad.HeaderText = "DESCRIPCIÓN";
            this.colDescripcionCriticidad.Name = "colDescripcionCriticidad";
            this.colDescripcionCriticidad.ReadOnly = true;
            // 
            // colEditarCriticidad
            // 
            this.colEditarCriticidad.FillWeight = 65F;
            this.colEditarCriticidad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colEditarCriticidad.HeaderText = "ACCIÓN";
            this.colEditarCriticidad.Name = "colEditarCriticidad";
            this.colEditarCriticidad.ReadOnly = true;
            this.colEditarCriticidad.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colEditarCriticidad.UseColumnTextForButtonValue = true;
            // 
            // FrmCriticidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(19)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(1252, 885);
            this.Controls.Add(this.pnlNivelesCriticidad);
            this.Controls.Add(this.pnlCabeceraCriticidad);
            this.Controls.Add(this.flpMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmCriticidad";
            this.Text = "Criticidad";
            this.Load += new System.EventHandler(this.FrmCriticidad_Load);
            this.flpMenu.ResumeLayout(false);
            this.pnlMarca.ResumeLayout(false);
            this.pnlMarca.PerformLayout();
            this.pnlAcceso.ResumeLayout(false);
            this.pnlAcceso.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlCabeceraCriticidad.ResumeLayout(false);
            this.pnlCabeceraCriticidad.PerformLayout();
            this.pnlNivelesCriticidad.ResumeLayout(false);
            this.pnlNivelesCriticidad.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCriticidad)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpMenu;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlMarca;
        private System.Windows.Forms.Label lblNombreSistema;
        private System.Windows.Forms.Label lblModoSistema;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnDiagnostico;
        private System.Windows.Forms.Button btnAlertas;
        private System.Windows.Forms.Button btnCriticidad;
        private System.Windows.Forms.Button btnEventos;
        private System.Windows.Forms.Button btnOperadores;
        private System.Windows.Forms.Button btnCredenciales;
        private System.Windows.Forms.Button btnEquipos;
        private System.Windows.Forms.Button btnComandos;
        private System.Windows.Forms.Button btnSincronizacion;
        private System.Windows.Forms.Button btnReportes;
        private System.Windows.Forms.Button btnConfiguracion;
        private System.Windows.Forms.Panel pnlAcceso;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel pnlCabeceraCriticidad;
        private System.Windows.Forms.Button btnParadaSimulada;
        private System.Windows.Forms.Label lblEstadoDatos;
        private System.Windows.Forms.Label lblConexionESP32;
        private System.Windows.Forms.Label lblDiagEstadoApp;
        private System.Windows.Forms.Label lblSubtituloCriticidad;
        private System.Windows.Forms.Label lblTituloCriticidad;
        private System.Windows.Forms.Panel pnlNivelesCriticidad;
        private System.Windows.Forms.Label lblPoliticaSeguridad;
        private System.Windows.Forms.Label lblTituloNivelesCriticidad;
        private System.Windows.Forms.DataGridView dgvCriticidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNivelCriticidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombreCriticidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConfirmacionesCriticidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTimeoutCriticidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescripcionCriticidad;
        private System.Windows.Forms.DataGridViewButtonColumn colEditarCriticidad;
    }
}