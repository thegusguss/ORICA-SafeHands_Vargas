namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    partial class FrmAlertas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAlertas));
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
            this.pnlCabeceraAlertas = new System.Windows.Forms.Panel();
            this.btnParadaSimulada = new System.Windows.Forms.Button();
            this.lblEstadoDatos = new System.Windows.Forms.Label();
            this.lblConexionESP32 = new System.Windows.Forms.Label();
            this.lblDiagEstadoApp = new System.Windows.Forms.Label();
            this.lblSubtituloAlertas = new System.Windows.Forms.Label();
            this.lblTituloAlertas = new System.Windows.Forms.Label();
            this.pnlAlertasActivas = new System.Windows.Forms.Panel();
            this.lblDetalleAlertasActivas = new System.Windows.Forms.Label();
            this.lblCantidadAlertasActivas = new System.Windows.Forms.Label();
            this.lblTituloAlertasActivas = new System.Windows.Forms.Label();
            this.pnlRechazosConfianza = new System.Windows.Forms.Panel();
            this.lblDetalleRechazosConfianza = new System.Windows.Forms.Label();
            this.lblCantidadRechazosConfianza = new System.Windows.Forms.Label();
            this.lblTituloRechazosConfianza = new System.Windows.Forms.Label();
            this.pnlFallasEnlace = new System.Windows.Forms.Panel();
            this.lblDetalleFallasEnlace = new System.Windows.Forms.Label();
            this.lblCantidadFallasEnlace = new System.Windows.Forms.Label();
            this.lblTituloFallasEnlace = new System.Windows.Forms.Label();
            this.pnlRegistroAlertas = new System.Windows.Forms.Panel();
            this.lblTituloRegistroAlertas = new System.Windows.Forms.Label();
            this.dgvAlertas = new System.Windows.Forms.DataGridView();
            this.colHoraAlerta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTipoAlerta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNivelAlerta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescripcionAlerta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstadoAlerta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReconocerAlerta = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colCerrarAlerta = new System.Windows.Forms.DataGridViewButtonColumn();
            this.flpMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlMarca.SuspendLayout();
            this.pnlAcceso.SuspendLayout();
            this.pnlCabeceraAlertas.SuspendLayout();
            this.pnlAlertasActivas.SuspendLayout();
            this.pnlRechazosConfianza.SuspendLayout();
            this.pnlFallasEnlace.SuspendLayout();
            this.pnlRegistroAlertas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlertas)).BeginInit();
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
            this.flpMenu.Location = new System.Drawing.Point(0, 1);
            this.flpMenu.Name = "flpMenu";
            this.flpMenu.Padding = new System.Windows.Forms.Padding(10, 20, 0, 0);
            this.flpMenu.Size = new System.Drawing.Size(230, 880);
            this.flpMenu.TabIndex = 2;
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
            // pnlCabeceraAlertas
            // 
            this.pnlCabeceraAlertas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(27)))), ((int)(((byte)(36)))));
            this.pnlCabeceraAlertas.Controls.Add(this.btnParadaSimulada);
            this.pnlCabeceraAlertas.Controls.Add(this.lblEstadoDatos);
            this.pnlCabeceraAlertas.Controls.Add(this.lblConexionESP32);
            this.pnlCabeceraAlertas.Controls.Add(this.lblDiagEstadoApp);
            this.pnlCabeceraAlertas.Controls.Add(this.lblSubtituloAlertas);
            this.pnlCabeceraAlertas.Controls.Add(this.lblTituloAlertas);
            this.pnlCabeceraAlertas.Location = new System.Drawing.Point(248, 12);
            this.pnlCabeceraAlertas.Name = "pnlCabeceraAlertas";
            this.pnlCabeceraAlertas.Size = new System.Drawing.Size(973, 65);
            this.pnlCabeceraAlertas.TabIndex = 3;
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
            // lblSubtituloAlertas
            // 
            this.lblSubtituloAlertas.AutoSize = true;
            this.lblSubtituloAlertas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtituloAlertas.ForeColor = System.Drawing.Color.Silver;
            this.lblSubtituloAlertas.Location = new System.Drawing.Point(16, 39);
            this.lblSubtituloAlertas.Name = "lblSubtituloAlertas";
            this.lblSubtituloAlertas.Size = new System.Drawing.Size(222, 15);
            this.lblSubtituloAlertas.TabIndex = 2;
            this.lblSubtituloAlertas.Text = "Incidentes, rechazos y fallas detectadas";
            // 
            // lblTituloAlertas
            // 
            this.lblTituloAlertas.AutoSize = true;
            this.lblTituloAlertas.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloAlertas.ForeColor = System.Drawing.Color.White;
            this.lblTituloAlertas.Location = new System.Drawing.Point(16, 11);
            this.lblTituloAlertas.Name = "lblTituloAlertas";
            this.lblTituloAlertas.Size = new System.Drawing.Size(60, 18);
            this.lblTituloAlertas.TabIndex = 1;
            this.lblTituloAlertas.Text = "Alertas";
            // 
            // pnlAlertasActivas
            // 
            this.pnlAlertasActivas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(29)))), ((int)(((byte)(37)))));
            this.pnlAlertasActivas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAlertasActivas.Controls.Add(this.lblDetalleAlertasActivas);
            this.pnlAlertasActivas.Controls.Add(this.lblCantidadAlertasActivas);
            this.pnlAlertasActivas.Controls.Add(this.lblTituloAlertasActivas);
            this.pnlAlertasActivas.Location = new System.Drawing.Point(248, 100);
            this.pnlAlertasActivas.Name = "pnlAlertasActivas";
            this.pnlAlertasActivas.Size = new System.Drawing.Size(974, 110);
            this.pnlAlertasActivas.TabIndex = 4;
            // 
            // lblDetalleAlertasActivas
            // 
            this.lblDetalleAlertasActivas.AutoSize = true;
            this.lblDetalleAlertasActivas.ForeColor = System.Drawing.Color.Silver;
            this.lblDetalleAlertasActivas.Location = new System.Drawing.Point(15, 73);
            this.lblDetalleAlertasActivas.Name = "lblDetalleAlertasActivas";
            this.lblDetalleAlertasActivas.Size = new System.Drawing.Size(55, 13);
            this.lblDetalleAlertasActivas.TabIndex = 5;
            this.lblDetalleAlertasActivas.Text = "Sin cargar";
            // 
            // lblCantidadAlertasActivas
            // 
            this.lblCantidadAlertasActivas.AutoSize = true;
            this.lblCantidadAlertasActivas.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCantidadAlertasActivas.Location = new System.Drawing.Point(14, 38);
            this.lblCantidadAlertasActivas.Name = "lblCantidadAlertasActivas";
            this.lblCantidadAlertasActivas.Size = new System.Drawing.Size(25, 24);
            this.lblCantidadAlertasActivas.TabIndex = 4;
            this.lblCantidadAlertasActivas.Text = "—";
            // 
            // lblTituloAlertasActivas
            // 
            this.lblTituloAlertasActivas.AutoSize = true;
            this.lblTituloAlertasActivas.ForeColor = System.Drawing.Color.Silver;
            this.lblTituloAlertasActivas.Location = new System.Drawing.Point(15, 15);
            this.lblTituloAlertasActivas.Name = "lblTituloAlertasActivas";
            this.lblTituloAlertasActivas.Size = new System.Drawing.Size(104, 13);
            this.lblTituloAlertasActivas.TabIndex = 3;
            this.lblTituloAlertasActivas.Text = "ALERTAS ACTIVAS";
            // 
            // pnlRechazosConfianza
            // 
            this.pnlRechazosConfianza.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(29)))), ((int)(((byte)(37)))));
            this.pnlRechazosConfianza.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRechazosConfianza.Controls.Add(this.lblDetalleRechazosConfianza);
            this.pnlRechazosConfianza.Controls.Add(this.lblCantidadRechazosConfianza);
            this.pnlRechazosConfianza.Controls.Add(this.lblTituloRechazosConfianza);
            this.pnlRechazosConfianza.Location = new System.Drawing.Point(248, 226);
            this.pnlRechazosConfianza.Name = "pnlRechazosConfianza";
            this.pnlRechazosConfianza.Size = new System.Drawing.Size(974, 110);
            this.pnlRechazosConfianza.TabIndex = 6;
            // 
            // lblDetalleRechazosConfianza
            // 
            this.lblDetalleRechazosConfianza.AutoSize = true;
            this.lblDetalleRechazosConfianza.ForeColor = System.Drawing.Color.Silver;
            this.lblDetalleRechazosConfianza.Location = new System.Drawing.Point(15, 73);
            this.lblDetalleRechazosConfianza.Name = "lblDetalleRechazosConfianza";
            this.lblDetalleRechazosConfianza.Size = new System.Drawing.Size(71, 13);
            this.lblDetalleRechazosConfianza.TabIndex = 5;
            this.lblDetalleRechazosConfianza.Text = "Sesión actual";
            // 
            // lblCantidadRechazosConfianza
            // 
            this.lblCantidadRechazosConfianza.AutoSize = true;
            this.lblCantidadRechazosConfianza.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCantidadRechazosConfianza.Location = new System.Drawing.Point(14, 38);
            this.lblCantidadRechazosConfianza.Name = "lblCantidadRechazosConfianza";
            this.lblCantidadRechazosConfianza.Size = new System.Drawing.Size(25, 24);
            this.lblCantidadRechazosConfianza.TabIndex = 4;
            this.lblCantidadRechazosConfianza.Text = "—";
            // 
            // lblTituloRechazosConfianza
            // 
            this.lblTituloRechazosConfianza.AutoSize = true;
            this.lblTituloRechazosConfianza.ForeColor = System.Drawing.Color.Silver;
            this.lblTituloRechazosConfianza.Location = new System.Drawing.Point(15, 15);
            this.lblTituloRechazosConfianza.Name = "lblTituloRechazosConfianza";
            this.lblTituloRechazosConfianza.Size = new System.Drawing.Size(156, 13);
            this.lblTituloRechazosConfianza.TabIndex = 3;
            this.lblTituloRechazosConfianza.Text = "RECHAZOS POR CONFIANZA";
            // 
            // pnlFallasEnlace
            // 
            this.pnlFallasEnlace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(29)))), ((int)(((byte)(37)))));
            this.pnlFallasEnlace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFallasEnlace.Controls.Add(this.lblDetalleFallasEnlace);
            this.pnlFallasEnlace.Controls.Add(this.lblCantidadFallasEnlace);
            this.pnlFallasEnlace.Controls.Add(this.lblTituloFallasEnlace);
            this.pnlFallasEnlace.Location = new System.Drawing.Point(248, 352);
            this.pnlFallasEnlace.Name = "pnlFallasEnlace";
            this.pnlFallasEnlace.Size = new System.Drawing.Size(974, 110);
            this.pnlFallasEnlace.TabIndex = 7;
            // 
            // lblDetalleFallasEnlace
            // 
            this.lblDetalleFallasEnlace.AutoSize = true;
            this.lblDetalleFallasEnlace.ForeColor = System.Drawing.Color.Silver;
            this.lblDetalleFallasEnlace.Location = new System.Drawing.Point(15, 73);
            this.lblDetalleFallasEnlace.Name = "lblDetalleFallasEnlace";
            this.lblDetalleFallasEnlace.Size = new System.Drawing.Size(75, 13);
            this.lblDetalleFallasEnlace.TabIndex = 5;
            this.lblDetalleFallasEnlace.Text = "Sin comprobar";
            // 
            // lblCantidadFallasEnlace
            // 
            this.lblCantidadFallasEnlace.AutoSize = true;
            this.lblCantidadFallasEnlace.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCantidadFallasEnlace.Location = new System.Drawing.Point(14, 38);
            this.lblCantidadFallasEnlace.Name = "lblCantidadFallasEnlace";
            this.lblCantidadFallasEnlace.Size = new System.Drawing.Size(25, 24);
            this.lblCantidadFallasEnlace.TabIndex = 4;
            this.lblCantidadFallasEnlace.Text = "—";
            // 
            // lblTituloFallasEnlace
            // 
            this.lblTituloFallasEnlace.AutoSize = true;
            this.lblTituloFallasEnlace.ForeColor = System.Drawing.Color.Silver;
            this.lblTituloFallasEnlace.Location = new System.Drawing.Point(15, 15);
            this.lblTituloFallasEnlace.Name = "lblTituloFallasEnlace";
            this.lblTituloFallasEnlace.Size = new System.Drawing.Size(109, 13);
            this.lblTituloFallasEnlace.TabIndex = 3;
            this.lblTituloFallasEnlace.Text = "FALLAS DE ENLACE";
            // 
            // pnlRegistroAlertas
            // 
            this.pnlRegistroAlertas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(29)))), ((int)(((byte)(37)))));
            this.pnlRegistroAlertas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRegistroAlertas.Controls.Add(this.dgvAlertas);
            this.pnlRegistroAlertas.Controls.Add(this.lblTituloRegistroAlertas);
            this.pnlRegistroAlertas.Location = new System.Drawing.Point(248, 495);
            this.pnlRegistroAlertas.Name = "pnlRegistroAlertas";
            this.pnlRegistroAlertas.Size = new System.Drawing.Size(974, 270);
            this.pnlRegistroAlertas.TabIndex = 8;
            // 
            // lblTituloRegistroAlertas
            // 
            this.lblTituloRegistroAlertas.AutoSize = true;
            this.lblTituloRegistroAlertas.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloRegistroAlertas.ForeColor = System.Drawing.Color.White;
            this.lblTituloRegistroAlertas.Location = new System.Drawing.Point(15, 20);
            this.lblTituloRegistroAlertas.Name = "lblTituloRegistroAlertas";
            this.lblTituloRegistroAlertas.Size = new System.Drawing.Size(151, 18);
            this.lblTituloRegistroAlertas.TabIndex = 7;
            this.lblTituloRegistroAlertas.Text = "Registro de alertas";
            // 
            // dgvAlertas
            // 
            this.dgvAlertas.AllowUserToAddRows = false;
            this.dgvAlertas.AllowUserToDeleteRows = false;
            this.dgvAlertas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAlertas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAlertas.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(29)))), ((int)(((byte)(37)))));
            this.dgvAlertas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(27)))), ((int)(((byte)(36)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAlertas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAlertas.ColumnHeadersHeight = 35;
            this.dgvAlertas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvAlertas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colHoraAlerta,
            this.colTipoAlerta,
            this.colNivelAlerta,
            this.colDescripcionAlerta,
            this.colEstadoAlerta,
            this.colReconocerAlerta,
            this.colCerrarAlerta});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(29)))), ((int)(((byte)(37)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(43)))), ((int)(((byte)(105)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAlertas.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvAlertas.EnableHeadersVisualStyles = false;
            this.dgvAlertas.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(65)))));
            this.dgvAlertas.Location = new System.Drawing.Point(18, 57);
            this.dgvAlertas.MultiSelect = false;
            this.dgvAlertas.Name = "dgvAlertas";
            this.dgvAlertas.ReadOnly = true;
            this.dgvAlertas.RowHeadersVisible = false;
            this.dgvAlertas.RowTemplate.Height = 45;
            this.dgvAlertas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAlertas.Size = new System.Drawing.Size(944, 200);
            this.dgvAlertas.TabIndex = 8;
            // 
            // colHoraAlerta
            // 
            this.colHoraAlerta.FillWeight = 65F;
            this.colHoraAlerta.HeaderText = "HORA";
            this.colHoraAlerta.Name = "colHoraAlerta";
            this.colHoraAlerta.ReadOnly = true;
            // 
            // colTipoAlerta
            // 
            this.colTipoAlerta.FillWeight = 110F;
            this.colTipoAlerta.HeaderText = "TIPO";
            this.colTipoAlerta.Name = "colTipoAlerta";
            this.colTipoAlerta.ReadOnly = true;
            // 
            // colNivelAlerta
            // 
            this.colNivelAlerta.FillWeight = 65F;
            this.colNivelAlerta.HeaderText = "NIVEL";
            this.colNivelAlerta.Name = "colNivelAlerta";
            this.colNivelAlerta.ReadOnly = true;
            // 
            // colDescripcionAlerta
            // 
            this.colDescripcionAlerta.FillWeight = 250F;
            this.colDescripcionAlerta.HeaderText = "DESCRIPCIÓN";
            this.colDescripcionAlerta.Name = "colDescripcionAlerta";
            this.colDescripcionAlerta.ReadOnly = true;
            // 
            // colEstadoAlerta
            // 
            this.colEstadoAlerta.FillWeight = 80F;
            this.colEstadoAlerta.HeaderText = "ESTADO";
            this.colEstadoAlerta.Name = "colEstadoAlerta";
            this.colEstadoAlerta.ReadOnly = true;
            // 
            // colReconocerAlerta
            // 
            this.colReconocerAlerta.FillWeight = 85F;
            this.colReconocerAlerta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colReconocerAlerta.HeaderText = "RECONOCER";
            this.colReconocerAlerta.Name = "colReconocerAlerta";
            this.colReconocerAlerta.ReadOnly = true;
            this.colReconocerAlerta.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colReconocerAlerta.UseColumnTextForButtonValue = true;
            // 
            // colCerrarAlerta
            // 
            this.colCerrarAlerta.FillWeight = 65F;
            this.colCerrarAlerta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colCerrarAlerta.HeaderText = "CERRAR";
            this.colCerrarAlerta.Name = "colCerrarAlerta";
            this.colCerrarAlerta.ReadOnly = true;
            this.colCerrarAlerta.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colCerrarAlerta.UseColumnTextForButtonValue = true;
            // 
            // FrmAlertas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(19)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(1248, 881);
            this.Controls.Add(this.pnlRegistroAlertas);
            this.Controls.Add(this.pnlFallasEnlace);
            this.Controls.Add(this.pnlRechazosConfianza);
            this.Controls.Add(this.pnlAlertasActivas);
            this.Controls.Add(this.pnlCabeceraAlertas);
            this.Controls.Add(this.flpMenu);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmAlertas";
            this.Text = "Alertas";
            this.flpMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlMarca.ResumeLayout(false);
            this.pnlMarca.PerformLayout();
            this.pnlAcceso.ResumeLayout(false);
            this.pnlAcceso.PerformLayout();
            this.pnlCabeceraAlertas.ResumeLayout(false);
            this.pnlCabeceraAlertas.PerformLayout();
            this.pnlAlertasActivas.ResumeLayout(false);
            this.pnlAlertasActivas.PerformLayout();
            this.pnlRechazosConfianza.ResumeLayout(false);
            this.pnlRechazosConfianza.PerformLayout();
            this.pnlFallasEnlace.ResumeLayout(false);
            this.pnlFallasEnlace.PerformLayout();
            this.pnlRegistroAlertas.ResumeLayout(false);
            this.pnlRegistroAlertas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlertas)).EndInit();
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
        private System.Windows.Forms.Panel pnlCabeceraAlertas;
        private System.Windows.Forms.Button btnParadaSimulada;
        private System.Windows.Forms.Label lblEstadoDatos;
        private System.Windows.Forms.Label lblConexionESP32;
        private System.Windows.Forms.Label lblDiagEstadoApp;
        private System.Windows.Forms.Label lblSubtituloAlertas;
        private System.Windows.Forms.Label lblTituloAlertas;
        private System.Windows.Forms.Panel pnlAlertasActivas;
        private System.Windows.Forms.Label lblDetalleAlertasActivas;
        private System.Windows.Forms.Label lblCantidadAlertasActivas;
        private System.Windows.Forms.Label lblTituloAlertasActivas;
        private System.Windows.Forms.Panel pnlRechazosConfianza;
        private System.Windows.Forms.Label lblDetalleRechazosConfianza;
        private System.Windows.Forms.Label lblCantidadRechazosConfianza;
        private System.Windows.Forms.Label lblTituloRechazosConfianza;
        private System.Windows.Forms.Panel pnlFallasEnlace;
        private System.Windows.Forms.Label lblDetalleFallasEnlace;
        private System.Windows.Forms.Label lblCantidadFallasEnlace;
        private System.Windows.Forms.Label lblTituloFallasEnlace;
        private System.Windows.Forms.Panel pnlRegistroAlertas;
        private System.Windows.Forms.DataGridView dgvAlertas;
        private System.Windows.Forms.Label lblTituloRegistroAlertas;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHoraAlerta;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTipoAlerta;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNivelAlerta;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescripcionAlerta;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstadoAlerta;
        private System.Windows.Forms.DataGridViewButtonColumn colReconocerAlerta;
        private System.Windows.Forms.DataGridViewButtonColumn colCerrarAlerta;
    }
}