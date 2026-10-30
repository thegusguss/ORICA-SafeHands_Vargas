namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    partial class FrmConfiguracion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConfiguracion));
            this.flpMenu = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
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
            this.pnlCabeceraConfiguracion = new System.Windows.Forms.Panel();
            this.btnParadaSimulada = new System.Windows.Forms.Button();
            this.lblEstadoDatos = new System.Windows.Forms.Label();
            this.lblConexionESP32 = new System.Windows.Forms.Label();
            this.lblDiagEstadoApp = new System.Windows.Forms.Label();
            this.lblSubtituloConfiguracion = new System.Windows.Forms.Label();
            this.lblTituloConfiguracion = new System.Windows.Forms.Label();
            this.pnlConfiguracionVoz = new System.Windows.Forms.Panel();
            this.chkFeedbackVoz = new System.Windows.Forms.CheckBox();
            this.chkPTTObligatorio = new System.Windows.Forms.CheckBox();
            this.chkGramaticaCerrada = new System.Windows.Forms.CheckBox();
            this.nudTimeoutConfirmacion = new System.Windows.Forms.NumericUpDown();
            this.trkUmbralConfianza = new System.Windows.Forms.TrackBar();
            this.lblTimeoutConfirmacion = new System.Windows.Forms.Label();
            this.lblUmbralConfianza = new System.Windows.Forms.Label();
            this.lblTituloConfiguracionVoz = new System.Windows.Forms.Label();
            this.pnlConfiguracionEnlace = new System.Windows.Forms.Panel();
            this.btnSimularEnlace = new System.Windows.Forms.Button();
            this.chkValidarSecuencia = new System.Windows.Forms.CheckBox();
            this.chkCRCObligatorio = new System.Windows.Forms.CheckBox();
            this.nudMaxHeartbeatPerdidos = new System.Windows.Forms.NumericUpDown();
            this.nudIntervaloHeartbeat = new System.Windows.Forms.NumericUpDown();
            this.cbxBaudRate = new System.Windows.Forms.ComboBox();
            this.cbxPuertoESP32 = new System.Windows.Forms.ComboBox();
            this.lblBaudRate = new System.Windows.Forms.Label();
            this.lblIntervaloHeartbeat = new System.Windows.Forms.Label();
            this.lblMaxHeartbeatPerdidos = new System.Windows.Forms.Label();
            this.lblPuertoESP32 = new System.Windows.Forms.Label();
            this.lblTituloConfiguracionEnlace = new System.Windows.Forms.Label();
            this.pnlConfiguracionPersistencia = new System.Windows.Forms.Panel();
            this.chkSincronizarSQLServer = new System.Windows.Forms.CheckBox();
            this.chkModoOfflineFirst = new System.Windows.Forms.CheckBox();
            this.btnSimularRedSQLConfig = new System.Windows.Forms.Button();
            this.lblTituloConfiguracionPersistencia = new System.Windows.Forms.Label();
            this.pnlConfiguracionSeguridad = new System.Windows.Forms.Panel();
            this.btnGuardarConfiguracion = new System.Windows.Forms.Button();
            this.chkRearmeExplicito = new System.Windows.Forms.CheckBox();
            this.chkWatchdogESP32 = new System.Windows.Forms.CheckBox();
            this.chkEstadoSeguroError = new System.Windows.Forms.CheckBox();
            this.chkParadaPrioritaria = new System.Windows.Forms.CheckBox();
            this.lblModoSeguridad = new System.Windows.Forms.Label();
            this.lblTituloConfiguracionSeguridad = new System.Windows.Forms.Label();
            this.flpMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlMarca.SuspendLayout();
            this.pnlAcceso.SuspendLayout();
            this.pnlCabeceraConfiguracion.SuspendLayout();
            this.pnlConfiguracionVoz.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimeoutConfirmacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkUmbralConfianza)).BeginInit();
            this.pnlConfiguracionEnlace.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxHeartbeatPerdidos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudIntervaloHeartbeat)).BeginInit();
            this.pnlConfiguracionPersistencia.SuspendLayout();
            this.pnlConfiguracionSeguridad.SuspendLayout();
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
            this.flpMenu.Location = new System.Drawing.Point(2, 3);
            this.flpMenu.Name = "flpMenu";
            this.flpMenu.Padding = new System.Windows.Forms.Padding(10, 20, 0, 0);
            this.flpMenu.Size = new System.Drawing.Size(230, 880);
            this.flpMenu.TabIndex = 8;
            this.flpMenu.WrapContents = false;
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
            // pnlCabeceraConfiguracion
            // 
            this.pnlCabeceraConfiguracion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(27)))), ((int)(((byte)(36)))));
            this.pnlCabeceraConfiguracion.Controls.Add(this.btnParadaSimulada);
            this.pnlCabeceraConfiguracion.Controls.Add(this.lblEstadoDatos);
            this.pnlCabeceraConfiguracion.Controls.Add(this.lblConexionESP32);
            this.pnlCabeceraConfiguracion.Controls.Add(this.lblDiagEstadoApp);
            this.pnlCabeceraConfiguracion.Controls.Add(this.lblSubtituloConfiguracion);
            this.pnlCabeceraConfiguracion.Controls.Add(this.lblTituloConfiguracion);
            this.pnlCabeceraConfiguracion.Location = new System.Drawing.Point(253, 21);
            this.pnlCabeceraConfiguracion.Name = "pnlCabeceraConfiguracion";
            this.pnlCabeceraConfiguracion.Size = new System.Drawing.Size(973, 65);
            this.pnlCabeceraConfiguracion.TabIndex = 9;
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
            // lblSubtituloConfiguracion
            // 
            this.lblSubtituloConfiguracion.AutoSize = true;
            this.lblSubtituloConfiguracion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtituloConfiguracion.ForeColor = System.Drawing.Color.Silver;
            this.lblSubtituloConfiguracion.Location = new System.Drawing.Point(16, 39);
            this.lblSubtituloConfiguracion.Name = "lblSubtituloConfiguracion";
            this.lblSubtituloConfiguracion.Size = new System.Drawing.Size(290, 15);
            this.lblSubtituloConfiguracion.TabIndex = 2;
            this.lblSubtituloConfiguracion.Text = "Parámetros de voz, enlace, persistencia y seguridad";
            // 
            // lblTituloConfiguracion
            // 
            this.lblTituloConfiguracion.AutoSize = true;
            this.lblTituloConfiguracion.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloConfiguracion.ForeColor = System.Drawing.Color.White;
            this.lblTituloConfiguracion.Location = new System.Drawing.Point(16, 11);
            this.lblTituloConfiguracion.Name = "lblTituloConfiguracion";
            this.lblTituloConfiguracion.Size = new System.Drawing.Size(113, 18);
            this.lblTituloConfiguracion.TabIndex = 1;
            this.lblTituloConfiguracion.Text = "Configuración";
            // 
            // pnlConfiguracionVoz
            // 
            this.pnlConfiguracionVoz.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlConfiguracionVoz.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(29)))), ((int)(((byte)(37)))));
            this.pnlConfiguracionVoz.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlConfiguracionVoz.Controls.Add(this.chkFeedbackVoz);
            this.pnlConfiguracionVoz.Controls.Add(this.chkPTTObligatorio);
            this.pnlConfiguracionVoz.Controls.Add(this.chkGramaticaCerrada);
            this.pnlConfiguracionVoz.Controls.Add(this.nudTimeoutConfirmacion);
            this.pnlConfiguracionVoz.Controls.Add(this.trkUmbralConfianza);
            this.pnlConfiguracionVoz.Controls.Add(this.lblTimeoutConfirmacion);
            this.pnlConfiguracionVoz.Controls.Add(this.lblUmbralConfianza);
            this.pnlConfiguracionVoz.Controls.Add(this.lblTituloConfiguracionVoz);
            this.pnlConfiguracionVoz.Location = new System.Drawing.Point(253, 115);
            this.pnlConfiguracionVoz.Name = "pnlConfiguracionVoz";
            this.pnlConfiguracionVoz.Size = new System.Drawing.Size(481, 324);
            this.pnlConfiguracionVoz.TabIndex = 10;
            // 
            // chkFeedbackVoz
            // 
            this.chkFeedbackVoz.AutoSize = true;
            this.chkFeedbackVoz.BackColor = System.Drawing.Color.Transparent;
            this.chkFeedbackVoz.ForeColor = System.Drawing.Color.White;
            this.chkFeedbackVoz.Location = new System.Drawing.Point(24, 282);
            this.chkFeedbackVoz.Name = "chkFeedbackVoz";
            this.chkFeedbackVoz.Size = new System.Drawing.Size(149, 17);
            this.chkFeedbackVoz.TabIndex = 16;
            this.chkFeedbackVoz.Text = "Retroalimentación por voz";
            this.chkFeedbackVoz.UseVisualStyleBackColor = false;
            // 
            // chkPTTObligatorio
            // 
            this.chkPTTObligatorio.AutoSize = true;
            this.chkPTTObligatorio.BackColor = System.Drawing.Color.Transparent;
            this.chkPTTObligatorio.ForeColor = System.Drawing.Color.White;
            this.chkPTTObligatorio.Location = new System.Drawing.Point(24, 248);
            this.chkPTTObligatorio.Name = "chkPTTObligatorio";
            this.chkPTTObligatorio.Size = new System.Drawing.Size(188, 17);
            this.chkPTTObligatorio.TabIndex = 15;
            this.chkPTTObligatorio.Text = "PTT obligatorio: pulsar para hablar";
            this.chkPTTObligatorio.UseVisualStyleBackColor = false;
            // 
            // chkGramaticaCerrada
            // 
            this.chkGramaticaCerrada.AutoSize = true;
            this.chkGramaticaCerrada.BackColor = System.Drawing.Color.Transparent;
            this.chkGramaticaCerrada.ForeColor = System.Drawing.Color.White;
            this.chkGramaticaCerrada.Location = new System.Drawing.Point(24, 211);
            this.chkGramaticaCerrada.Name = "chkGramaticaCerrada";
            this.chkGramaticaCerrada.Size = new System.Drawing.Size(226, 17);
            this.chkGramaticaCerrada.TabIndex = 14;
            this.chkGramaticaCerrada.Text = "Gramática cerrada: solo frases autorizadas";
            this.chkGramaticaCerrada.UseVisualStyleBackColor = false;
            // 
            // nudTimeoutConfirmacion
            // 
            this.nudTimeoutConfirmacion.Location = new System.Drawing.Point(20, 164);
            this.nudTimeoutConfirmacion.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudTimeoutConfirmacion.Name = "nudTimeoutConfirmacion";
            this.nudTimeoutConfirmacion.Size = new System.Drawing.Size(436, 20);
            this.nudTimeoutConfirmacion.TabIndex = 13;
            // 
            // trkUmbralConfianza
            // 
            this.trkUmbralConfianza.Location = new System.Drawing.Point(20, 84);
            this.trkUmbralConfianza.Maximum = 100;
            this.trkUmbralConfianza.Name = "trkUmbralConfianza";
            this.trkUmbralConfianza.Size = new System.Drawing.Size(436, 45);
            this.trkUmbralConfianza.TabIndex = 12;
            this.trkUmbralConfianza.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trkUmbralConfianza.Value = 73;
            this.trkUmbralConfianza.ValueChanged += new System.EventHandler(this.trkUmbralConfianza_ValueChanged);
            // 
            // lblTimeoutConfirmacion
            // 
            this.lblTimeoutConfirmacion.AutoSize = true;
            this.lblTimeoutConfirmacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeoutConfirmacion.ForeColor = System.Drawing.Color.Silver;
            this.lblTimeoutConfirmacion.Location = new System.Drawing.Point(17, 130);
            this.lblTimeoutConfirmacion.Name = "lblTimeoutConfirmacion";
            this.lblTimeoutConfirmacion.Size = new System.Drawing.Size(208, 15);
            this.lblTimeoutConfirmacion.TabIndex = 11;
            this.lblTimeoutConfirmacion.Text = "Timeout de confirmación (segundos)";
            // 
            // lblUmbralConfianza
            // 
            this.lblUmbralConfianza.AutoSize = true;
            this.lblUmbralConfianza.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUmbralConfianza.ForeColor = System.Drawing.Color.Silver;
            this.lblUmbralConfianza.Location = new System.Drawing.Point(17, 55);
            this.lblUmbralConfianza.Name = "lblUmbralConfianza";
            this.lblUmbralConfianza.Size = new System.Drawing.Size(134, 15);
            this.lblUmbralConfianza.TabIndex = 7;
            this.lblUmbralConfianza.Text = "Umbral de confianza: —";
            // 
            // lblTituloConfiguracionVoz
            // 
            this.lblTituloConfiguracionVoz.AutoSize = true;
            this.lblTituloConfiguracionVoz.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloConfiguracionVoz.ForeColor = System.Drawing.Color.White;
            this.lblTituloConfiguracionVoz.Location = new System.Drawing.Point(16, 24);
            this.lblTituloConfiguracionVoz.Name = "lblTituloConfiguracionVoz";
            this.lblTituloConfiguracionVoz.Size = new System.Drawing.Size(186, 18);
            this.lblTituloConfiguracionVoz.TabIndex = 10;
            this.lblTituloConfiguracionVoz.Text = "Reconocimiento de voz";
            // 
            // pnlConfiguracionEnlace
            // 
            this.pnlConfiguracionEnlace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlConfiguracionEnlace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(29)))), ((int)(((byte)(37)))));
            this.pnlConfiguracionEnlace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlConfiguracionEnlace.Controls.Add(this.btnSimularEnlace);
            this.pnlConfiguracionEnlace.Controls.Add(this.chkValidarSecuencia);
            this.pnlConfiguracionEnlace.Controls.Add(this.chkCRCObligatorio);
            this.pnlConfiguracionEnlace.Controls.Add(this.nudMaxHeartbeatPerdidos);
            this.pnlConfiguracionEnlace.Controls.Add(this.nudIntervaloHeartbeat);
            this.pnlConfiguracionEnlace.Controls.Add(this.cbxBaudRate);
            this.pnlConfiguracionEnlace.Controls.Add(this.cbxPuertoESP32);
            this.pnlConfiguracionEnlace.Controls.Add(this.lblBaudRate);
            this.pnlConfiguracionEnlace.Controls.Add(this.lblIntervaloHeartbeat);
            this.pnlConfiguracionEnlace.Controls.Add(this.lblMaxHeartbeatPerdidos);
            this.pnlConfiguracionEnlace.Controls.Add(this.lblPuertoESP32);
            this.pnlConfiguracionEnlace.Controls.Add(this.lblTituloConfiguracionEnlace);
            this.pnlConfiguracionEnlace.Location = new System.Drawing.Point(746, 115);
            this.pnlConfiguracionEnlace.Name = "pnlConfiguracionEnlace";
            this.pnlConfiguracionEnlace.Size = new System.Drawing.Size(481, 324);
            this.pnlConfiguracionEnlace.TabIndex = 11;
            // 
            // btnSimularEnlace
            // 
            this.btnSimularEnlace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSimularEnlace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(43)))), ((int)(((byte)(105)))));
            this.btnSimularEnlace.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSimularEnlace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimularEnlace.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSimularEnlace.ForeColor = System.Drawing.Color.White;
            this.btnSimularEnlace.Location = new System.Drawing.Point(19, 270);
            this.btnSimularEnlace.Name = "btnSimularEnlace";
            this.btnSimularEnlace.Size = new System.Drawing.Size(440, 38);
            this.btnSimularEnlace.TabIndex = 26;
            this.btnSimularEnlace.Text = "Actualizar";
            this.btnSimularEnlace.UseVisualStyleBackColor = false;
            // 
            // chkValidarSecuencia
            // 
            this.chkValidarSecuencia.AutoSize = true;
            this.chkValidarSecuencia.BackColor = System.Drawing.Color.Transparent;
            this.chkValidarSecuencia.ForeColor = System.Drawing.Color.White;
            this.chkValidarSecuencia.Location = new System.Drawing.Point(22, 234);
            this.chkValidarSecuencia.Name = "chkValidarSecuencia";
            this.chkValidarSecuencia.Size = new System.Drawing.Size(172, 17);
            this.chkValidarSecuencia.TabIndex = 25;
            this.chkValidarSecuencia.Text = "Validar secuencia de mensajes";
            this.chkValidarSecuencia.UseVisualStyleBackColor = false;
            // 
            // chkCRCObligatorio
            // 
            this.chkCRCObligatorio.AutoSize = true;
            this.chkCRCObligatorio.BackColor = System.Drawing.Color.Transparent;
            this.chkCRCObligatorio.ForeColor = System.Drawing.Color.White;
            this.chkCRCObligatorio.Location = new System.Drawing.Point(22, 211);
            this.chkCRCObligatorio.Name = "chkCRCObligatorio";
            this.chkCRCObligatorio.Size = new System.Drawing.Size(99, 17);
            this.chkCRCObligatorio.TabIndex = 24;
            this.chkCRCObligatorio.Text = "CRC obligatorio";
            this.chkCRCObligatorio.UseVisualStyleBackColor = false;
            // 
            // nudMaxHeartbeatPerdidos
            // 
            this.nudMaxHeartbeatPerdidos.Location = new System.Drawing.Point(249, 164);
            this.nudMaxHeartbeatPerdidos.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudMaxHeartbeatPerdidos.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMaxHeartbeatPerdidos.Name = "nudMaxHeartbeatPerdidos";
            this.nudMaxHeartbeatPerdidos.Size = new System.Drawing.Size(204, 20);
            this.nudMaxHeartbeatPerdidos.TabIndex = 23;
            this.nudMaxHeartbeatPerdidos.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudIntervaloHeartbeat
            // 
            this.nudIntervaloHeartbeat.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudIntervaloHeartbeat.Location = new System.Drawing.Point(22, 164);
            this.nudIntervaloHeartbeat.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudIntervaloHeartbeat.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudIntervaloHeartbeat.Name = "nudIntervaloHeartbeat";
            this.nudIntervaloHeartbeat.Size = new System.Drawing.Size(205, 20);
            this.nudIntervaloHeartbeat.TabIndex = 17;
            this.nudIntervaloHeartbeat.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // cbxBaudRate
            // 
            this.cbxBaudRate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(19)))), ((int)(((byte)(26)))));
            this.cbxBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBaudRate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxBaudRate.ForeColor = System.Drawing.Color.White;
            this.cbxBaudRate.FormattingEnabled = true;
            this.cbxBaudRate.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cbxBaudRate.Location = new System.Drawing.Point(249, 84);
            this.cbxBaudRate.Name = "cbxBaudRate";
            this.cbxBaudRate.Size = new System.Drawing.Size(204, 21);
            this.cbxBaudRate.TabIndex = 22;
            // 
            // cbxPuertoESP32
            // 
            this.cbxPuertoESP32.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(19)))), ((int)(((byte)(26)))));
            this.cbxPuertoESP32.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPuertoESP32.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxPuertoESP32.ForeColor = System.Drawing.Color.White;
            this.cbxPuertoESP32.FormattingEnabled = true;
            this.cbxPuertoESP32.Items.AddRange(new object[] {
            "Ejecutado ",
            "Rechazado",
            "Cancelado"});
            this.cbxPuertoESP32.Location = new System.Drawing.Point(22, 84);
            this.cbxPuertoESP32.Name = "cbxPuertoESP32";
            this.cbxPuertoESP32.Size = new System.Drawing.Size(205, 21);
            this.cbxPuertoESP32.TabIndex = 21;
            // 
            // lblBaudRate
            // 
            this.lblBaudRate.AutoSize = true;
            this.lblBaudRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBaudRate.ForeColor = System.Drawing.Color.Silver;
            this.lblBaudRate.Location = new System.Drawing.Point(246, 55);
            this.lblBaudRate.Name = "lblBaudRate";
            this.lblBaudRate.Size = new System.Drawing.Size(60, 15);
            this.lblBaudRate.TabIndex = 20;
            this.lblBaudRate.Text = "Baud rate";
            // 
            // lblIntervaloHeartbeat
            // 
            this.lblIntervaloHeartbeat.AutoSize = true;
            this.lblIntervaloHeartbeat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIntervaloHeartbeat.ForeColor = System.Drawing.Color.Silver;
            this.lblIntervaloHeartbeat.Location = new System.Drawing.Point(19, 130);
            this.lblIntervaloHeartbeat.Name = "lblIntervaloHeartbeat";
            this.lblIntervaloHeartbeat.Size = new System.Drawing.Size(89, 15);
            this.lblIntervaloHeartbeat.TabIndex = 19;
            this.lblIntervaloHeartbeat.Text = "Heartbeat (ms)";
            // 
            // lblMaxHeartbeatPerdidos
            // 
            this.lblMaxHeartbeatPerdidos.AutoSize = true;
            this.lblMaxHeartbeatPerdidos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxHeartbeatPerdidos.ForeColor = System.Drawing.Color.Silver;
            this.lblMaxHeartbeatPerdidos.Location = new System.Drawing.Point(246, 130);
            this.lblMaxHeartbeatPerdidos.Name = "lblMaxHeartbeatPerdidos";
            this.lblMaxHeartbeatPerdidos.Size = new System.Drawing.Size(140, 15);
            this.lblMaxHeartbeatPerdidos.TabIndex = 18;
            this.lblMaxHeartbeatPerdidos.Text = "Máx. heartbeat perdidos";
            // 
            // lblPuertoESP32
            // 
            this.lblPuertoESP32.AutoSize = true;
            this.lblPuertoESP32.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPuertoESP32.ForeColor = System.Drawing.Color.Silver;
            this.lblPuertoESP32.Location = new System.Drawing.Point(19, 55);
            this.lblPuertoESP32.Name = "lblPuertoESP32";
            this.lblPuertoESP32.Size = new System.Drawing.Size(43, 15);
            this.lblPuertoESP32.TabIndex = 17;
            this.lblPuertoESP32.Text = "Puerto";
            // 
            // lblTituloConfiguracionEnlace
            // 
            this.lblTituloConfiguracionEnlace.AutoSize = true;
            this.lblTituloConfiguracionEnlace.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloConfiguracionEnlace.ForeColor = System.Drawing.Color.White;
            this.lblTituloConfiguracionEnlace.Location = new System.Drawing.Point(16, 24);
            this.lblTituloConfiguracionEnlace.Name = "lblTituloConfiguracionEnlace";
            this.lblTituloConfiguracionEnlace.Size = new System.Drawing.Size(221, 18);
            this.lblTituloConfiguracionEnlace.TabIndex = 10;
            this.lblTituloConfiguracionEnlace.Text = "Comunicación PC ↔ ESP32";
            // 
            // pnlConfiguracionPersistencia
            // 
            this.pnlConfiguracionPersistencia.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlConfiguracionPersistencia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(29)))), ((int)(((byte)(37)))));
            this.pnlConfiguracionPersistencia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlConfiguracionPersistencia.Controls.Add(this.chkSincronizarSQLServer);
            this.pnlConfiguracionPersistencia.Controls.Add(this.chkModoOfflineFirst);
            this.pnlConfiguracionPersistencia.Controls.Add(this.btnSimularRedSQLConfig);
            this.pnlConfiguracionPersistencia.Controls.Add(this.lblTituloConfiguracionPersistencia);
            this.pnlConfiguracionPersistencia.Location = new System.Drawing.Point(253, 459);
            this.pnlConfiguracionPersistencia.Name = "pnlConfiguracionPersistencia";
            this.pnlConfiguracionPersistencia.Size = new System.Drawing.Size(481, 299);
            this.pnlConfiguracionPersistencia.TabIndex = 11;
            // 
            // chkSincronizarSQLServer
            // 
            this.chkSincronizarSQLServer.AutoSize = true;
            this.chkSincronizarSQLServer.BackColor = System.Drawing.Color.Transparent;
            this.chkSincronizarSQLServer.ForeColor = System.Drawing.Color.White;
            this.chkSincronizarSQLServer.Location = new System.Drawing.Point(19, 99);
            this.chkSincronizarSQLServer.Name = "chkSincronizarSQLServer";
            this.chkSincronizarSQLServer.Size = new System.Drawing.Size(272, 17);
            this.chkSincronizarSQLServer.TabIndex = 18;
            this.chkSincronizarSQLServer.Text = "Sincronizar con SQL Server cuando exista conexión";
            this.chkSincronizarSQLServer.UseVisualStyleBackColor = false;
            // 
            // chkModoOfflineFirst
            // 
            this.chkModoOfflineFirst.AutoSize = true;
            this.chkModoOfflineFirst.BackColor = System.Drawing.Color.Transparent;
            this.chkModoOfflineFirst.ForeColor = System.Drawing.Color.White;
            this.chkModoOfflineFirst.Location = new System.Drawing.Point(19, 62);
            this.chkModoOfflineFirst.Name = "chkModoOfflineFirst";
            this.chkModoOfflineFirst.Size = new System.Drawing.Size(176, 17);
            this.chkModoOfflineFirst.TabIndex = 17;
            this.chkModoOfflineFirst.Text = "Guardar primero en SQLite local";
            this.chkModoOfflineFirst.UseVisualStyleBackColor = false;
            // 
            // btnSimularRedSQLConfig
            // 
            this.btnSimularRedSQLConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSimularRedSQLConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(43)))), ((int)(((byte)(105)))));
            this.btnSimularRedSQLConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSimularRedSQLConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimularRedSQLConfig.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSimularRedSQLConfig.ForeColor = System.Drawing.Color.White;
            this.btnSimularRedSQLConfig.Location = new System.Drawing.Point(19, 161);
            this.btnSimularRedSQLConfig.Name = "btnSimularRedSQLConfig";
            this.btnSimularRedSQLConfig.Size = new System.Drawing.Size(442, 38);
            this.btnSimularRedSQLConfig.TabIndex = 27;
            this.btnSimularRedSQLConfig.Text = "Simular red SQL Server";
            this.btnSimularRedSQLConfig.UseVisualStyleBackColor = false;
            // 
            // lblTituloConfiguracionPersistencia
            // 
            this.lblTituloConfiguracionPersistencia.AutoSize = true;
            this.lblTituloConfiguracionPersistencia.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloConfiguracionPersistencia.ForeColor = System.Drawing.Color.White;
            this.lblTituloConfiguracionPersistencia.Location = new System.Drawing.Point(16, 24);
            this.lblTituloConfiguracionPersistencia.Name = "lblTituloConfiguracionPersistencia";
            this.lblTituloConfiguracionPersistencia.Size = new System.Drawing.Size(229, 18);
            this.lblTituloConfiguracionPersistencia.TabIndex = 10;
            this.lblTituloConfiguracionPersistencia.Text = "Persistencia y sincronización";
            // 
            // pnlConfiguracionSeguridad
            // 
            this.pnlConfiguracionSeguridad.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlConfiguracionSeguridad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(29)))), ((int)(((byte)(37)))));
            this.pnlConfiguracionSeguridad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlConfiguracionSeguridad.Controls.Add(this.btnGuardarConfiguracion);
            this.pnlConfiguracionSeguridad.Controls.Add(this.chkRearmeExplicito);
            this.pnlConfiguracionSeguridad.Controls.Add(this.chkWatchdogESP32);
            this.pnlConfiguracionSeguridad.Controls.Add(this.chkEstadoSeguroError);
            this.pnlConfiguracionSeguridad.Controls.Add(this.chkParadaPrioritaria);
            this.pnlConfiguracionSeguridad.Controls.Add(this.lblModoSeguridad);
            this.pnlConfiguracionSeguridad.Controls.Add(this.lblTituloConfiguracionSeguridad);
            this.pnlConfiguracionSeguridad.Location = new System.Drawing.Point(745, 459);
            this.pnlConfiguracionSeguridad.Name = "pnlConfiguracionSeguridad";
            this.pnlConfiguracionSeguridad.Size = new System.Drawing.Size(481, 299);
            this.pnlConfiguracionSeguridad.TabIndex = 11;
            // 
            // btnGuardarConfiguracion
            // 
            this.btnGuardarConfiguracion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGuardarConfiguracion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(43)))), ((int)(((byte)(105)))));
            this.btnGuardarConfiguracion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGuardarConfiguracion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardarConfiguracion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardarConfiguracion.ForeColor = System.Drawing.Color.White;
            this.btnGuardarConfiguracion.Location = new System.Drawing.Point(23, 237);
            this.btnGuardarConfiguracion.Name = "btnGuardarConfiguracion";
            this.btnGuardarConfiguracion.Size = new System.Drawing.Size(437, 38);
            this.btnGuardarConfiguracion.TabIndex = 28;
            this.btnGuardarConfiguracion.Text = "Guardar configuración";
            this.btnGuardarConfiguracion.UseVisualStyleBackColor = false;
            // 
            // chkRearmeExplicito
            // 
            this.chkRearmeExplicito.AutoSize = true;
            this.chkRearmeExplicito.BackColor = System.Drawing.Color.Transparent;
            this.chkRearmeExplicito.ForeColor = System.Drawing.Color.White;
            this.chkRearmeExplicito.Location = new System.Drawing.Point(23, 196);
            this.chkRearmeExplicito.Name = "chkRearmeExplicito";
            this.chkRearmeExplicito.Size = new System.Drawing.Size(187, 17);
            this.chkRearmeExplicito.TabIndex = 31;
            this.chkRearmeExplicito.Text = "Rearme mediante acción explícita";
            this.chkRearmeExplicito.UseVisualStyleBackColor = false;
            // 
            // chkWatchdogESP32
            // 
            this.chkWatchdogESP32.AutoSize = true;
            this.chkWatchdogESP32.BackColor = System.Drawing.Color.Transparent;
            this.chkWatchdogESP32.ForeColor = System.Drawing.Color.White;
            this.chkWatchdogESP32.Location = new System.Drawing.Point(23, 161);
            this.chkWatchdogESP32.Name = "chkWatchdogESP32";
            this.chkWatchdogESP32.Size = new System.Drawing.Size(222, 17);
            this.chkWatchdogESP32.TabIndex = 30;
            this.chkWatchdogESP32.Text = "Supervisión del enlace: watchdog ESP32";
            this.chkWatchdogESP32.UseVisualStyleBackColor = false;
            // 
            // chkEstadoSeguroError
            // 
            this.chkEstadoSeguroError.AutoSize = true;
            this.chkEstadoSeguroError.BackColor = System.Drawing.Color.Transparent;
            this.chkEstadoSeguroError.ForeColor = System.Drawing.Color.White;
            this.chkEstadoSeguroError.Location = new System.Drawing.Point(23, 127);
            this.chkEstadoSeguroError.Name = "chkEstadoSeguroError";
            this.chkEstadoSeguroError.Size = new System.Drawing.Size(219, 17);
            this.chkEstadoSeguroError.TabIndex = 29;
            this.chkEstadoSeguroError.Text = "Estado seguro ante error: motor apagado";
            this.chkEstadoSeguroError.UseVisualStyleBackColor = false;
            // 
            // chkParadaPrioritaria
            // 
            this.chkParadaPrioritaria.AutoSize = true;
            this.chkParadaPrioritaria.BackColor = System.Drawing.Color.Transparent;
            this.chkParadaPrioritaria.ForeColor = System.Drawing.Color.White;
            this.chkParadaPrioritaria.Location = new System.Drawing.Point(23, 90);
            this.chkParadaPrioritaria.Name = "chkParadaPrioritaria";
            this.chkParadaPrioritaria.Size = new System.Drawing.Size(105, 17);
            this.chkParadaPrioritaria.TabIndex = 28;
            this.chkParadaPrioritaria.Text = "Parada prioritaria";
            this.chkParadaPrioritaria.UseVisualStyleBackColor = false;
            // 
            // lblModoSeguridad
            // 
            this.lblModoSeguridad.AutoSize = true;
            this.lblModoSeguridad.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModoSeguridad.ForeColor = System.Drawing.Color.Silver;
            this.lblModoSeguridad.Location = new System.Drawing.Point(20, 61);
            this.lblModoSeguridad.Name = "lblModoSeguridad";
            this.lblModoSeguridad.Size = new System.Drawing.Size(117, 15);
            this.lblModoSeguridad.TabIndex = 27;
            this.lblModoSeguridad.Text = "Modo demostración";
            // 
            // lblTituloConfiguracionSeguridad
            // 
            this.lblTituloConfiguracionSeguridad.AutoSize = true;
            this.lblTituloConfiguracionSeguridad.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloConfiguracionSeguridad.ForeColor = System.Drawing.Color.White;
            this.lblTituloConfiguracionSeguridad.Location = new System.Drawing.Point(17, 24);
            this.lblTituloConfiguracionSeguridad.Name = "lblTituloConfiguracionSeguridad";
            this.lblTituloConfiguracionSeguridad.Size = new System.Drawing.Size(183, 18);
            this.lblTituloConfiguracionSeguridad.TabIndex = 10;
            this.lblTituloConfiguracionSeguridad.Text = "Seguridad del prototipo";
            // 
            // FrmConfiguracion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(19)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(1252, 885);
            this.Controls.Add(this.pnlConfiguracionSeguridad);
            this.Controls.Add(this.pnlConfiguracionPersistencia);
            this.Controls.Add(this.pnlConfiguracionEnlace);
            this.Controls.Add(this.pnlConfiguracionVoz);
            this.Controls.Add(this.flpMenu);
            this.Controls.Add(this.pnlCabeceraConfiguracion);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmConfiguracion";
            this.Text = "Configuración";
            this.flpMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlMarca.ResumeLayout(false);
            this.pnlMarca.PerformLayout();
            this.pnlAcceso.ResumeLayout(false);
            this.pnlAcceso.PerformLayout();
            this.pnlCabeceraConfiguracion.ResumeLayout(false);
            this.pnlCabeceraConfiguracion.PerformLayout();
            this.pnlConfiguracionVoz.ResumeLayout(false);
            this.pnlConfiguracionVoz.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimeoutConfirmacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkUmbralConfianza)).EndInit();
            this.pnlConfiguracionEnlace.ResumeLayout(false);
            this.pnlConfiguracionEnlace.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxHeartbeatPerdidos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudIntervaloHeartbeat)).EndInit();
            this.pnlConfiguracionPersistencia.ResumeLayout(false);
            this.pnlConfiguracionPersistencia.PerformLayout();
            this.pnlConfiguracionSeguridad.ResumeLayout(false);
            this.pnlConfiguracionSeguridad.PerformLayout();
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
        private System.Windows.Forms.Panel pnlCabeceraConfiguracion;
        private System.Windows.Forms.Button btnParadaSimulada;
        private System.Windows.Forms.Label lblEstadoDatos;
        private System.Windows.Forms.Label lblConexionESP32;
        private System.Windows.Forms.Label lblDiagEstadoApp;
        private System.Windows.Forms.Label lblSubtituloConfiguracion;
        private System.Windows.Forms.Label lblTituloConfiguracion;
        private System.Windows.Forms.Panel pnlConfiguracionVoz;
        private System.Windows.Forms.TrackBar trkUmbralConfianza;
        private System.Windows.Forms.Label lblTimeoutConfirmacion;
        private System.Windows.Forms.Label lblUmbralConfianza;
        private System.Windows.Forms.Label lblTituloConfiguracionVoz;
        private System.Windows.Forms.Panel pnlConfiguracionEnlace;
        private System.Windows.Forms.Label lblTituloConfiguracionEnlace;
        private System.Windows.Forms.Panel pnlConfiguracionPersistencia;
        private System.Windows.Forms.Label lblTituloConfiguracionPersistencia;
        private System.Windows.Forms.Panel pnlConfiguracionSeguridad;
        private System.Windows.Forms.Label lblTituloConfiguracionSeguridad;
        private System.Windows.Forms.NumericUpDown nudTimeoutConfirmacion;
        private System.Windows.Forms.CheckBox chkFeedbackVoz;
        private System.Windows.Forms.CheckBox chkPTTObligatorio;
        private System.Windows.Forms.CheckBox chkGramaticaCerrada;
        private System.Windows.Forms.Label lblBaudRate;
        private System.Windows.Forms.Label lblIntervaloHeartbeat;
        private System.Windows.Forms.Label lblMaxHeartbeatPerdidos;
        private System.Windows.Forms.Label lblPuertoESP32;
        private System.Windows.Forms.CheckBox chkValidarSecuencia;
        private System.Windows.Forms.CheckBox chkCRCObligatorio;
        private System.Windows.Forms.NumericUpDown nudMaxHeartbeatPerdidos;
        private System.Windows.Forms.NumericUpDown nudIntervaloHeartbeat;
        private System.Windows.Forms.ComboBox cbxBaudRate;
        private System.Windows.Forms.ComboBox cbxPuertoESP32;
        private System.Windows.Forms.Button btnSimularEnlace;
        private System.Windows.Forms.Button btnSimularRedSQLConfig;
        private System.Windows.Forms.CheckBox chkSincronizarSQLServer;
        private System.Windows.Forms.CheckBox chkModoOfflineFirst;
        private System.Windows.Forms.Button btnGuardarConfiguracion;
        private System.Windows.Forms.CheckBox chkRearmeExplicito;
        private System.Windows.Forms.CheckBox chkWatchdogESP32;
        private System.Windows.Forms.CheckBox chkEstadoSeguroError;
        private System.Windows.Forms.CheckBox chkParadaPrioritaria;
        private System.Windows.Forms.Label lblModoSeguridad;
    }
}