namespace PROYECTO_ORICA_SAFEHANDS_VARGAS
{
    partial class FrmMiHistorial
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMiHistorial));
            this.pnlMenuOperador = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlMarca = new System.Windows.Forms.Panel();
            this.lblNombreSistema = new System.Windows.Forms.Label();
            this.lblSeccionOperador = new System.Windows.Forms.Label();
            this.btnInicioOperador = new System.Windows.Forms.Button();
            this.btnControlVozOperador = new System.Windows.Forms.Button();
            this.btnHistorialOperador = new System.Windows.Forms.Button();
            this.pnlSesionOperador = new System.Windows.Forms.Panel();
            this.lblNombreOperador = new System.Windows.Forms.Label();
            this.lblRolOperador = new System.Windows.Forms.Label();
            this.btnCerrarSesionOperador = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblSubtituloHistorial = new System.Windows.Forms.Label();
            this.lblRutaHistorial = new System.Windows.Forms.Label();
            this.lblTituloInicio = new System.Windows.Forms.Label();
            this.pnlAccionesHistorial = new System.Windows.Forms.Panel();
            this.lblTituloAccionesHistorial = new System.Windows.Forms.Label();
            this.lblAyudaHistorial = new System.Windows.Forms.Label();
            this.lblCantidadRegistrosHistorial = new System.Windows.Forms.Label();
            this.lblBuscarHistorial = new System.Windows.Forms.Label();
            this.txtBuscarHistorial = new System.Windows.Forms.TextBox();
            this.lblResultadoHistorial = new System.Windows.Forms.Label();
            this.cbxResultadoHistorial = new System.Windows.Forms.ComboBox();
            this.dgvMiHistorial = new System.Windows.Forms.DataGridView();
            this.colHoraHistorial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEquipoHistorial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colComandoHistorial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResultadoHistorial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetalleHistorial = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnlMenuOperador.SuspendLayout();
            this.pnlMarca.SuspendLayout();
            this.pnlSesionOperador.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlAccionesHistorial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMiHistorial)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMenuOperador
            // 
            this.pnlMenuOperador.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pnlMenuOperador.AutoScroll = true;
            this.pnlMenuOperador.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(43)))), ((int)(((byte)(105)))));
            this.pnlMenuOperador.Controls.Add(this.pictureBox1);
            this.pnlMenuOperador.Controls.Add(this.pnlMarca);
            this.pnlMenuOperador.Controls.Add(this.btnInicioOperador);
            this.pnlMenuOperador.Controls.Add(this.btnControlVozOperador);
            this.pnlMenuOperador.Controls.Add(this.btnHistorialOperador);
            this.pnlMenuOperador.Controls.Add(this.pnlSesionOperador);
            this.pnlMenuOperador.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlMenuOperador.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnlMenuOperador.Location = new System.Drawing.Point(2, 2);
            this.pnlMenuOperador.Name = "pnlMenuOperador";
            this.pnlMenuOperador.Padding = new System.Windows.Forms.Padding(10, 20, 0, 0);
            this.pnlMenuOperador.Size = new System.Drawing.Size(230, 880);
            this.pnlMenuOperador.TabIndex = 11;
            this.pnlMenuOperador.WrapContents = false;
            // 
            // pnlMarca
            // 
            this.pnlMarca.Controls.Add(this.lblNombreSistema);
            this.pnlMarca.Controls.Add(this.lblSeccionOperador);
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
            // lblSeccionOperador
            // 
            this.lblSeccionOperador.AutoSize = true;
            this.lblSeccionOperador.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSeccionOperador.ForeColor = System.Drawing.Color.Silver;
            this.lblSeccionOperador.Location = new System.Drawing.Point(21, 35);
            this.lblSeccionOperador.Name = "lblSeccionOperador";
            this.lblSeccionOperador.Size = new System.Drawing.Size(161, 13);
            this.lblSeccionOperador.TabIndex = 16;
            this.lblSeccionOperador.Text = "ESPACIO DEL OPERADOR";
            this.lblSeccionOperador.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnInicioOperador
            // 
            this.btnInicioOperador.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInicioOperador.FlatAppearance.BorderSize = 0;
            this.btnInicioOperador.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInicioOperador.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInicioOperador.ForeColor = System.Drawing.Color.White;
            this.btnInicioOperador.Location = new System.Drawing.Point(13, 168);
            this.btnInicioOperador.Name = "btnInicioOperador";
            this.btnInicioOperador.Size = new System.Drawing.Size(200, 42);
            this.btnInicioOperador.TabIndex = 0;
            this.btnInicioOperador.Text = "Inicio";
            this.btnInicioOperador.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInicioOperador.UseVisualStyleBackColor = true;
            // 
            // btnControlVozOperador
            // 
            this.btnControlVozOperador.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnControlVozOperador.FlatAppearance.BorderSize = 0;
            this.btnControlVozOperador.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnControlVozOperador.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnControlVozOperador.ForeColor = System.Drawing.Color.White;
            this.btnControlVozOperador.Location = new System.Drawing.Point(13, 216);
            this.btnControlVozOperador.Name = "btnControlVozOperador";
            this.btnControlVozOperador.Size = new System.Drawing.Size(200, 42);
            this.btnControlVozOperador.TabIndex = 19;
            this.btnControlVozOperador.Text = "Control por voz";
            this.btnControlVozOperador.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnControlVozOperador.UseVisualStyleBackColor = true;
            // 
            // btnHistorialOperador
            // 
            this.btnHistorialOperador.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHistorialOperador.FlatAppearance.BorderSize = 0;
            this.btnHistorialOperador.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistorialOperador.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHistorialOperador.ForeColor = System.Drawing.Color.White;
            this.btnHistorialOperador.Location = new System.Drawing.Point(13, 264);
            this.btnHistorialOperador.Name = "btnHistorialOperador";
            this.btnHistorialOperador.Size = new System.Drawing.Size(200, 42);
            this.btnHistorialOperador.TabIndex = 4;
            this.btnHistorialOperador.Text = "Mi historial";
            this.btnHistorialOperador.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHistorialOperador.UseVisualStyleBackColor = true;
            // 
            // pnlSesionOperador
            // 
            this.pnlSesionOperador.Controls.Add(this.lblNombreOperador);
            this.pnlSesionOperador.Controls.Add(this.lblRolOperador);
            this.pnlSesionOperador.Controls.Add(this.btnCerrarSesionOperador);
            this.pnlSesionOperador.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSesionOperador.Location = new System.Drawing.Point(13, 312);
            this.pnlSesionOperador.Name = "pnlSesionOperador";
            this.pnlSesionOperador.Size = new System.Drawing.Size(200, 122);
            this.pnlSesionOperador.TabIndex = 18;
            // 
            // lblNombreOperador
            // 
            this.lblNombreOperador.AutoSize = true;
            this.lblNombreOperador.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombreOperador.ForeColor = System.Drawing.Color.White;
            this.lblNombreOperador.Location = new System.Drawing.Point(12, 15);
            this.lblNombreOperador.Name = "lblNombreOperador";
            this.lblNombreOperador.Size = new System.Drawing.Size(144, 17);
            this.lblNombreOperador.TabIndex = 22;
            this.lblNombreOperador.Text = "Sin sesión iniciada";
            // 
            // lblRolOperador
            // 
            this.lblRolOperador.AutoSize = true;
            this.lblRolOperador.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRolOperador.ForeColor = System.Drawing.Color.White;
            this.lblRolOperador.Location = new System.Drawing.Point(12, 48);
            this.lblRolOperador.Name = "lblRolOperador";
            this.lblRolOperador.Size = new System.Drawing.Size(65, 16);
            this.lblRolOperador.TabIndex = 21;
            this.lblRolOperador.Text = "Operador";
            // 
            // btnCerrarSesionOperador
            // 
            this.btnCerrarSesionOperador.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrarSesionOperador.FlatAppearance.BorderSize = 0;
            this.btnCerrarSesionOperador.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarSesionOperador.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrarSesionOperador.ForeColor = System.Drawing.Color.White;
            this.btnCerrarSesionOperador.Location = new System.Drawing.Point(3, 74);
            this.btnCerrarSesionOperador.Name = "btnCerrarSesionOperador";
            this.btnCerrarSesionOperador.Size = new System.Drawing.Size(194, 40);
            this.btnCerrarSesionOperador.TabIndex = 20;
            this.btnCerrarSesionOperador.Text = "Cerrar sesión";
            this.btnCerrarSesionOperador.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCerrarSesionOperador.UseVisualStyleBackColor = true;
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
            // lblSubtituloHistorial
            // 
            this.lblSubtituloHistorial.AutoSize = true;
            this.lblSubtituloHistorial.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtituloHistorial.ForeColor = System.Drawing.Color.Silver;
            this.lblSubtituloHistorial.Location = new System.Drawing.Point(263, 106);
            this.lblSubtituloHistorial.Name = "lblSubtituloHistorial";
            this.lblSubtituloHistorial.Size = new System.Drawing.Size(323, 17);
            this.lblSubtituloHistorial.TabIndex = 15;
            this.lblSubtituloHistorial.Text = "Consulta las acciones de tu sesión y su resultado.";
            // 
            // lblRutaHistorial
            // 
            this.lblRutaHistorial.AutoSize = true;
            this.lblRutaHistorial.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRutaHistorial.ForeColor = System.Drawing.Color.Silver;
            this.lblRutaHistorial.Location = new System.Drawing.Point(263, 25);
            this.lblRutaHistorial.Name = "lblRutaHistorial";
            this.lblRutaHistorial.Size = new System.Drawing.Size(128, 13);
            this.lblRutaHistorial.TabIndex = 13;
            this.lblRutaHistorial.Text = "ORICA / OPERADOR";
            // 
            // lblTituloInicio
            // 
            this.lblTituloInicio.AutoSize = true;
            this.lblTituloInicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloInicio.ForeColor = System.Drawing.Color.White;
            this.lblTituloInicio.Location = new System.Drawing.Point(259, 58);
            this.lblTituloInicio.Name = "lblTituloInicio";
            this.lblTituloInicio.Size = new System.Drawing.Size(170, 37);
            this.lblTituloInicio.TabIndex = 14;
            this.lblTituloInicio.Text = "Mi historial";
            // 
            // pnlAccionesHistorial
            // 
            this.pnlAccionesHistorial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(30)))), ((int)(((byte)(41)))));
            this.pnlAccionesHistorial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAccionesHistorial.Controls.Add(this.dgvMiHistorial);
            this.pnlAccionesHistorial.Controls.Add(this.cbxResultadoHistorial);
            this.pnlAccionesHistorial.Controls.Add(this.lblResultadoHistorial);
            this.pnlAccionesHistorial.Controls.Add(this.txtBuscarHistorial);
            this.pnlAccionesHistorial.Controls.Add(this.lblBuscarHistorial);
            this.pnlAccionesHistorial.Controls.Add(this.lblCantidadRegistrosHistorial);
            this.pnlAccionesHistorial.Controls.Add(this.lblTituloAccionesHistorial);
            this.pnlAccionesHistorial.Controls.Add(this.lblAyudaHistorial);
            this.pnlAccionesHistorial.Location = new System.Drawing.Point(254, 149);
            this.pnlAccionesHistorial.Name = "pnlAccionesHistorial";
            this.pnlAccionesHistorial.Size = new System.Drawing.Size(973, 537);
            this.pnlAccionesHistorial.TabIndex = 16;
            // 
            // lblTituloAccionesHistorial
            // 
            this.lblTituloAccionesHistorial.AutoSize = true;
            this.lblTituloAccionesHistorial.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloAccionesHistorial.ForeColor = System.Drawing.Color.White;
            this.lblTituloAccionesHistorial.Location = new System.Drawing.Point(22, 16);
            this.lblTituloAccionesHistorial.Name = "lblTituloAccionesHistorial";
            this.lblTituloAccionesHistorial.Size = new System.Drawing.Size(204, 20);
            this.lblTituloAccionesHistorial.TabIndex = 3;
            this.lblTituloAccionesHistorial.Text = "Acciones de esta sesión";
            // 
            // lblAyudaHistorial
            // 
            this.lblAyudaHistorial.AutoSize = true;
            this.lblAyudaHistorial.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAyudaHistorial.ForeColor = System.Drawing.Color.Silver;
            this.lblAyudaHistorial.Location = new System.Drawing.Point(26, 53);
            this.lblAyudaHistorial.Name = "lblAyudaHistorial";
            this.lblAyudaHistorial.Size = new System.Drawing.Size(334, 17);
            this.lblAyudaHistorial.TabIndex = 4;
            this.lblAyudaHistorial.Text = "Selecciona “Ver detalle” para consultar qué ocurrió.";
            // 
            // lblCantidadRegistrosHistorial
            // 
            this.lblCantidadRegistrosHistorial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCantidadRegistrosHistorial.AutoSize = true;
            this.lblCantidadRegistrosHistorial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(41)))), ((int)(((byte)(56)))));
            this.lblCantidadRegistrosHistorial.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCantidadRegistrosHistorial.ForeColor = System.Drawing.Color.Silver;
            this.lblCantidadRegistrosHistorial.Location = new System.Drawing.Point(870, 33);
            this.lblCantidadRegistrosHistorial.Name = "lblCantidadRegistrosHistorial";
            this.lblCantidadRegistrosHistorial.Size = new System.Drawing.Size(75, 17);
            this.lblCantidadRegistrosHistorial.TabIndex = 40;
            this.lblCantidadRegistrosHistorial.Text = "0 registros";
            this.lblCantidadRegistrosHistorial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBuscarHistorial
            // 
            this.lblBuscarHistorial.AutoSize = true;
            this.lblBuscarHistorial.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBuscarHistorial.ForeColor = System.Drawing.Color.White;
            this.lblBuscarHistorial.Location = new System.Drawing.Point(25, 105);
            this.lblBuscarHistorial.Name = "lblBuscarHistorial";
            this.lblBuscarHistorial.Size = new System.Drawing.Size(196, 17);
            this.lblBuscarHistorial.TabIndex = 41;
            this.lblBuscarHistorial.Text = "Buscar comando o equipo";
            // 
            // txtBuscarHistorial
            // 
            this.txtBuscarHistorial.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBuscarHistorial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(41)))), ((int)(((byte)(56)))));
            this.txtBuscarHistorial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBuscarHistorial.ForeColor = System.Drawing.Color.White;
            this.txtBuscarHistorial.Location = new System.Drawing.Point(26, 138);
            this.txtBuscarHistorial.Name = "txtBuscarHistorial";
            this.txtBuscarHistorial.Size = new System.Drawing.Size(429, 20);
            this.txtBuscarHistorial.TabIndex = 42;
            // 
            // lblResultadoHistorial
            // 
            this.lblResultadoHistorial.AutoSize = true;
            this.lblResultadoHistorial.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResultadoHistorial.ForeColor = System.Drawing.Color.White;
            this.lblResultadoHistorial.Location = new System.Drawing.Point(491, 105);
            this.lblResultadoHistorial.Name = "lblResultadoHistorial";
            this.lblResultadoHistorial.Size = new System.Drawing.Size(81, 17);
            this.lblResultadoHistorial.TabIndex = 43;
            this.lblResultadoHistorial.Text = "Resultado";
            // 
            // cbxResultadoHistorial
            // 
            this.cbxResultadoHistorial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(19)))), ((int)(((byte)(26)))));
            this.cbxResultadoHistorial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxResultadoHistorial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxResultadoHistorial.ForeColor = System.Drawing.Color.White;
            this.cbxResultadoHistorial.FormattingEnabled = true;
            this.cbxResultadoHistorial.Items.AddRange(new object[] {
            "Todos los resultados",
            "Confirmado",
            "Cancelado",
            "Consultado",
            "Rechazado",
            "Sin respuesta"});
            this.cbxResultadoHistorial.Location = new System.Drawing.Point(494, 137);
            this.cbxResultadoHistorial.Name = "cbxResultadoHistorial";
            this.cbxResultadoHistorial.Size = new System.Drawing.Size(429, 21);
            this.cbxResultadoHistorial.TabIndex = 45;
            // 
            // dgvMiHistorial
            // 
            this.dgvMiHistorial.AllowUserToAddRows = false;
            this.dgvMiHistorial.AllowUserToDeleteRows = false;
            this.dgvMiHistorial.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMiHistorial.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMiHistorial.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(29)))), ((int)(((byte)(37)))));
            this.dgvMiHistorial.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(27)))), ((int)(((byte)(36)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMiHistorial.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMiHistorial.ColumnHeadersHeight = 35;
            this.dgvMiHistorial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvMiHistorial.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colHoraHistorial,
            this.colEquipoHistorial,
            this.colComandoHistorial,
            this.colResultadoHistorial,
            this.colDetalleHistorial});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(29)))), ((int)(((byte)(37)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(43)))), ((int)(((byte)(105)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMiHistorial.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvMiHistorial.EnableHeadersVisualStyles = false;
            this.dgvMiHistorial.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(30)))), ((int)(((byte)(41)))));
            this.dgvMiHistorial.Location = new System.Drawing.Point(26, 200);
            this.dgvMiHistorial.MultiSelect = false;
            this.dgvMiHistorial.Name = "dgvMiHistorial";
            this.dgvMiHistorial.ReadOnly = true;
            this.dgvMiHistorial.RowHeadersVisible = false;
            this.dgvMiHistorial.RowTemplate.Height = 45;
            this.dgvMiHistorial.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMiHistorial.Size = new System.Drawing.Size(897, 294);
            this.dgvMiHistorial.TabIndex = 46;
            // 
            // colHoraHistorial
            // 
            this.colHoraHistorial.FillWeight = 80F;
            this.colHoraHistorial.HeaderText = "Hora";
            this.colHoraHistorial.Name = "colHoraHistorial";
            this.colHoraHistorial.ReadOnly = true;
            // 
            // colEquipoHistorial
            // 
            this.colEquipoHistorial.FillWeight = 150F;
            this.colEquipoHistorial.HeaderText = "Equipo";
            this.colEquipoHistorial.Name = "colEquipoHistorial";
            this.colEquipoHistorial.ReadOnly = true;
            // 
            // colComandoHistorial
            // 
            this.colComandoHistorial.FillWeight = 110F;
            this.colComandoHistorial.HeaderText = "Comando";
            this.colComandoHistorial.Name = "colComandoHistorial";
            this.colComandoHistorial.ReadOnly = true;
            // 
            // colResultadoHistorial
            // 
            this.colResultadoHistorial.FillWeight = 140F;
            this.colResultadoHistorial.HeaderText = "Resultado";
            this.colResultadoHistorial.Name = "colResultadoHistorial";
            this.colResultadoHistorial.ReadOnly = true;
            // 
            // colDetalleHistorial
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(41)))), ((int)(((byte)(56)))));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.colDetalleHistorial.DefaultCellStyle = dataGridViewCellStyle2;
            this.colDetalleHistorial.FillWeight = 85F;
            this.colDetalleHistorial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colDetalleHistorial.HeaderText = "Detalle";
            this.colDetalleHistorial.Name = "colDetalleHistorial";
            this.colDetalleHistorial.ReadOnly = true;
            this.colDetalleHistorial.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDetalleHistorial.Text = "Ver detalle";
            this.colDetalleHistorial.UseColumnTextForButtonValue = true;
            // 
            // FrmMiHistorial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(19)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(1252, 885);
            this.Controls.Add(this.pnlAccionesHistorial);
            this.Controls.Add(this.lblSubtituloHistorial);
            this.Controls.Add(this.lblRutaHistorial);
            this.Controls.Add(this.lblTituloInicio);
            this.Controls.Add(this.pnlMenuOperador);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMiHistorial";
            this.Text = "Mi historial";
            this.pnlMenuOperador.ResumeLayout(false);
            this.pnlMarca.ResumeLayout(false);
            this.pnlMarca.PerformLayout();
            this.pnlSesionOperador.ResumeLayout(false);
            this.pnlSesionOperador.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlAccionesHistorial.ResumeLayout(false);
            this.pnlAccionesHistorial.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMiHistorial)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnlMenuOperador;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlMarca;
        private System.Windows.Forms.Label lblNombreSistema;
        private System.Windows.Forms.Label lblSeccionOperador;
        private System.Windows.Forms.Button btnInicioOperador;
        private System.Windows.Forms.Button btnControlVozOperador;
        private System.Windows.Forms.Button btnHistorialOperador;
        private System.Windows.Forms.Panel pnlSesionOperador;
        private System.Windows.Forms.Label lblNombreOperador;
        private System.Windows.Forms.Label lblRolOperador;
        private System.Windows.Forms.Button btnCerrarSesionOperador;
        private System.Windows.Forms.Label lblSubtituloHistorial;
        private System.Windows.Forms.Label lblRutaHistorial;
        private System.Windows.Forms.Label lblTituloInicio;
        private System.Windows.Forms.Panel pnlAccionesHistorial;
        private System.Windows.Forms.Label lblBuscarHistorial;
        private System.Windows.Forms.Label lblCantidadRegistrosHistorial;
        private System.Windows.Forms.Label lblTituloAccionesHistorial;
        private System.Windows.Forms.Label lblAyudaHistorial;
        private System.Windows.Forms.TextBox txtBuscarHistorial;
        private System.Windows.Forms.Label lblResultadoHistorial;
        private System.Windows.Forms.ComboBox cbxResultadoHistorial;
        private System.Windows.Forms.DataGridView dgvMiHistorial;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHoraHistorial;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEquipoHistorial;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComandoHistorial;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResultadoHistorial;
        private System.Windows.Forms.DataGridViewButtonColumn colDetalleHistorial;
    }
}