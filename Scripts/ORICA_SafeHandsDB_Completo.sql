/* ============================================================
   ORICA SafeHands - Script completo de ORICA_SafeHandsDB
   Generado el 2026-11-03 desde la base de datos de desarrollo.
   Contiene la estructura (tablas, restricciones, índices, vistas,
   procedimientos) y los datos base. No incluye las filas de
   dbo.SesionOperador (registros de uso).

   Uso en otra computadora:
     1. Abrir en SSMS conectado a (localdb)\MSSQLLocalDB.
     2. Ejecutar (F5). Crea la base de datos si no existe.
   Si la base ya existe, se detiene sin modificar nada.
   ============================================================ */
SET NOCOUNT ON;
GO

-- Protección: si la base ya existe se corta la conexión y no se ejecuta nada más
IF DB_ID('ORICA_SafeHandsDB') IS NOT NULL
    RAISERROR('La base ORICA_SafeHandsDB ya existe; no se modificó nada. Elimínela antes si desea recrearla.', 20, 1) WITH LOG;
GO

CREATE DATABASE ORICA_SafeHandsDB COLLATE SQL_Latin1_General_CP1_CI_AS;
GO

USE ORICA_SafeHandsDB;
GO

/* ===================== ESTRUCTURA ===================== */
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rol](
	[IdRol] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](20) NOT NULL,
	[Nombre] [varchar](40) NOT NULL,
	[Descripcion] [varchar](200) NULL,
	[Estado] [bit] NOT NULL,
	[FechaRegistro] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Rol] ADD  CONSTRAINT [DF_Rol_Estado]  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Rol] ADD  CONSTRAINT [DF_Rol_Fecha]  DEFAULT (sysdatetime()) FOR [FechaRegistro]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Usuario](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](15) NOT NULL,
	[Nombres] [varchar](80) NOT NULL,
	[Apellidos] [varchar](100) NOT NULL,
	[Documento] [varchar](20) NULL,
	[Telefono] [varchar](20) NULL,
	[Correo] [varchar](120) NULL,
	[IdRol] [int] NOT NULL,
	[Estado] [bit] NOT NULL,
	[FechaRegistro] [datetime2](0) NOT NULL,
	[FechaModificacion] [datetime2](0) NULL,
	[UltimoAcceso] [datetime2](0) NULL,
	[RowVersion] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_Usuario_IdRol_Estado] ON [dbo].[Usuario]
(
	[IdRol] ASC,
	[Estado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Usuario_Correo] ON [dbo].[Usuario]
(
	[Correo] ASC
)
WHERE ([Correo] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Usuario_Documento] ON [dbo].[Usuario]
(
	[Documento] ASC
)
WHERE ([Documento] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF_Usuario_Estado]  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF_Usuario_FechaRegistro]  DEFAULT (sysdatetime()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Rol] FOREIGN KEY([IdRol])
REFERENCES [dbo].[Rol] ([IdRol])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Rol]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SesionOperador](
	[IdSesion] [uniqueidentifier] NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[FechaInicio] [datetime2](0) NOT NULL,
	[FechaFin] [datetime2](0) NULL,
	[EstadoSesion] [varchar](20) NOT NULL,
	[MetodoAutenticacion] [varchar](20) NULL,
	[Estacion] [varchar](100) NULL,
	[Observacion] [varchar](300) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdSesion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[SesionOperador] ADD  CONSTRAINT [DF_Sesion_Id]  DEFAULT (newsequentialid()) FOR [IdSesion]
GO
ALTER TABLE [dbo].[SesionOperador] ADD  CONSTRAINT [DF_Sesion_Inicio]  DEFAULT (sysdatetime()) FOR [FechaInicio]
GO
ALTER TABLE [dbo].[SesionOperador] ADD  CONSTRAINT [DF_Sesion_Estado]  DEFAULT ('ABIERTA') FOR [EstadoSesion]
GO
ALTER TABLE [dbo].[SesionOperador]  WITH CHECK ADD  CONSTRAINT [FK_Sesion_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[SesionOperador] CHECK CONSTRAINT [FK_Sesion_Usuario]
GO
ALTER TABLE [dbo].[SesionOperador]  WITH CHECK ADD  CONSTRAINT [CK_Sesion_Estado] CHECK  (([EstadoSesion]='BLOQUEADA' OR [EstadoSesion]='CANCELADA' OR [EstadoSesion]='CERRADA' OR [EstadoSesion]='ABIERTA'))
GO
ALTER TABLE [dbo].[SesionOperador] CHECK CONSTRAINT [CK_Sesion_Estado]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TipoAccion](
	[IdTipoAccion] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](30) NOT NULL,
	[Nombre] [varchar](60) NOT NULL,
	[Descripcion] [varchar](250) NULL,
	[EsAccionSegura] [bit] NOT NULL,
	[PermiteConfirmacion] [bit] NOT NULL,
	[Estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdTipoAccion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[TipoAccion] ADD  CONSTRAINT [DF_TipoAccion_Segura]  DEFAULT ((0)) FOR [EsAccionSegura]
GO
ALTER TABLE [dbo].[TipoAccion] ADD  CONSTRAINT [DF_TipoAccion_Confirmacion]  DEFAULT ((1)) FOR [PermiteConfirmacion]
GO
ALTER TABLE [dbo].[TipoAccion] ADD  CONSTRAINT [DF_TipoAccion_Estado]  DEFAULT ((1)) FOR [Estado]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NivelCriticidad](
	[IdNivelCriticidad] [int] NOT NULL,
	[Codigo] [varchar](20) NOT NULL,
	[Nombre] [varchar](40) NOT NULL,
	[NumeroConfirmaciones] [int] NOT NULL,
	[TimeoutSegundos] [int] NOT NULL,
	[Descripcion] [varchar](250) NULL,
	[Estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdNivelCriticidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[NivelCriticidad] ADD  CONSTRAINT [DF_NivelCriticidad_Estado]  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[NivelCriticidad]  WITH CHECK ADD  CONSTRAINT [CK_NivelCriticidad_Confirmaciones] CHECK  (([NumeroConfirmaciones]>=(0) AND [NumeroConfirmaciones]<=(2)))
GO
ALTER TABLE [dbo].[NivelCriticidad] CHECK CONSTRAINT [CK_NivelCriticidad_Confirmaciones]
GO
ALTER TABLE [dbo].[NivelCriticidad]  WITH CHECK ADD  CONSTRAINT [CK_NivelCriticidad_Id] CHECK  (([IdNivelCriticidad]>=(1) AND [IdNivelCriticidad]<=(4)))
GO
ALTER TABLE [dbo].[NivelCriticidad] CHECK CONSTRAINT [CK_NivelCriticidad_Id]
GO
ALTER TABLE [dbo].[NivelCriticidad]  WITH CHECK ADD  CONSTRAINT [CK_NivelCriticidad_Timeout] CHECK  (([TimeoutSegundos]>=(0)))
GO
ALTER TABLE [dbo].[NivelCriticidad] CHECK CONSTRAINT [CK_NivelCriticidad_Timeout]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Comando](
	[IdComando] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](20) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Frase] [varchar](180) NOT NULL,
	[IdNivelCriticidad] [int] NOT NULL,
	[IdTipoAccion] [int] NOT NULL,
	[ConfianzaMinima] [decimal](5, 2) NOT NULL,
	[RequierePTT] [bit] NOT NULL,
	[Estado] [bit] NOT NULL,
	[FechaRegistro] [datetime2](0) NOT NULL,
	[FechaModificacion] [datetime2](0) NULL,
	[RowVersion] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdComando] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Frase] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Comando] ADD  CONSTRAINT [DF_Comando_PTT]  DEFAULT ((1)) FOR [RequierePTT]
GO
ALTER TABLE [dbo].[Comando] ADD  CONSTRAINT [DF_Comando_Estado]  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Comando] ADD  CONSTRAINT [DF_Comando_FechaRegistro]  DEFAULT (sysdatetime()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Comando]  WITH CHECK ADD  CONSTRAINT [FK_Comando_Criticidad] FOREIGN KEY([IdNivelCriticidad])
REFERENCES [dbo].[NivelCriticidad] ([IdNivelCriticidad])
GO
ALTER TABLE [dbo].[Comando] CHECK CONSTRAINT [FK_Comando_Criticidad]
GO
ALTER TABLE [dbo].[Comando]  WITH CHECK ADD  CONSTRAINT [FK_Comando_TipoAccion] FOREIGN KEY([IdTipoAccion])
REFERENCES [dbo].[TipoAccion] ([IdTipoAccion])
GO
ALTER TABLE [dbo].[Comando] CHECK CONSTRAINT [FK_Comando_TipoAccion]
GO
ALTER TABLE [dbo].[Comando]  WITH CHECK ADD  CONSTRAINT [CK_Comando_Confianza] CHECK  (([ConfianzaMinima]>=(0) AND [ConfianzaMinima]<=(100)))
GO
ALTER TABLE [dbo].[Comando] CHECK CONSTRAINT [CK_Comando_Confianza]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TipoEquipo](
	[IdTipoEquipo] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](20) NOT NULL,
	[Nombre] [varchar](60) NOT NULL,
	[Descripcion] [varchar](200) NULL,
	[Estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdTipoEquipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[TipoEquipo] ADD  CONSTRAINT [DF_TipoEquipo_Estado]  DEFAULT ((1)) FOR [Estado]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Equipo](
	[IdEquipo] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](20) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[IdTipoEquipo] [int] NOT NULL,
	[Descripcion] [varchar](250) NULL,
	[IdentificadorHardware] [varchar](80) NULL,
	[UbicacionReferencia] [varchar](120) NULL,
	[Estado] [bit] NOT NULL,
	[UltimoHeartbeat] [datetime2](0) NULL,
	[FechaRegistro] [datetime2](0) NOT NULL,
	[FechaModificacion] [datetime2](0) NULL,
	[RowVersion] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdEquipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_PADDING ON
GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Equipo_IdentificadorHardware] ON [dbo].[Equipo]
(
	[IdentificadorHardware] ASC
)
WHERE ([IdentificadorHardware] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Equipo] ADD  CONSTRAINT [DF_Equipo_Estado]  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Equipo] ADD  CONSTRAINT [DF_Equipo_FechaRegistro]  DEFAULT (sysdatetime()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Equipo]  WITH CHECK ADD  CONSTRAINT [FK_Equipo_Tipo] FOREIGN KEY([IdTipoEquipo])
REFERENCES [dbo].[TipoEquipo] ([IdTipoEquipo])
GO
ALTER TABLE [dbo].[Equipo] CHECK CONSTRAINT [FK_Equipo_Tipo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Evento](
	[IdEvento] [uniqueidentifier] NOT NULL,
	[IdSesion] [uniqueidentifier] NULL,
	[IdUsuario] [int] NULL,
	[IdEquipo] [int] NULL,
	[IdComando] [int] NULL,
	[IdNivelCriticidad] [int] NULL,
	[FechaHora] [datetime2](0) NOT NULL,
	[TipoEvento] [varchar](40) NOT NULL,
	[TextoDetectado] [varchar](250) NULL,
	[Confianza] [decimal](5, 2) NULL,
	[Resultado] [varchar](30) NOT NULL,
	[Secuencia] [bigint] NULL,
	[CRC] [varchar](30) NULL,
	[Confirmado] [bit] NULL,
	[Origen] [varchar](30) NOT NULL,
	[Detalle] [varchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdEvento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_Evento_FechaHora] ON [dbo].[Evento]
(
	[FechaHora] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Evento] ADD  CONSTRAINT [DF_Evento_Id]  DEFAULT (newsequentialid()) FOR [IdEvento]
GO
ALTER TABLE [dbo].[Evento] ADD  CONSTRAINT [DF_Evento_Fecha]  DEFAULT (sysdatetime()) FOR [FechaHora]
GO
ALTER TABLE [dbo].[Evento] ADD  CONSTRAINT [DF_Evento_Origen]  DEFAULT ('APLICACION') FOR [Origen]
GO
ALTER TABLE [dbo].[Evento]  WITH CHECK ADD  CONSTRAINT [FK_Evento_Comando] FOREIGN KEY([IdComando])
REFERENCES [dbo].[Comando] ([IdComando])
GO
ALTER TABLE [dbo].[Evento] CHECK CONSTRAINT [FK_Evento_Comando]
GO
ALTER TABLE [dbo].[Evento]  WITH CHECK ADD  CONSTRAINT [FK_Evento_Criticidad] FOREIGN KEY([IdNivelCriticidad])
REFERENCES [dbo].[NivelCriticidad] ([IdNivelCriticidad])
GO
ALTER TABLE [dbo].[Evento] CHECK CONSTRAINT [FK_Evento_Criticidad]
GO
ALTER TABLE [dbo].[Evento]  WITH CHECK ADD  CONSTRAINT [FK_Evento_Equipo] FOREIGN KEY([IdEquipo])
REFERENCES [dbo].[Equipo] ([IdEquipo])
GO
ALTER TABLE [dbo].[Evento] CHECK CONSTRAINT [FK_Evento_Equipo]
GO
ALTER TABLE [dbo].[Evento]  WITH CHECK ADD  CONSTRAINT [FK_Evento_Sesion] FOREIGN KEY([IdSesion])
REFERENCES [dbo].[SesionOperador] ([IdSesion])
GO
ALTER TABLE [dbo].[Evento] CHECK CONSTRAINT [FK_Evento_Sesion]
GO
ALTER TABLE [dbo].[Evento]  WITH CHECK ADD  CONSTRAINT [FK_Evento_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[Evento] CHECK CONSTRAINT [FK_Evento_Usuario]
GO
ALTER TABLE [dbo].[Evento]  WITH CHECK ADD  CONSTRAINT [CK_Evento_Confianza] CHECK  (([Confianza] IS NULL OR [Confianza]>=(0) AND [Confianza]<=(100)))
GO
ALTER TABLE [dbo].[Evento] CHECK CONSTRAINT [CK_Evento_Confianza]
GO
ALTER TABLE [dbo].[Evento]  WITH CHECK ADD  CONSTRAINT [CK_Evento_Resultado] CHECK  (([Resultado]='INFORMATIVO' OR [Resultado]='IGNORADO' OR [Resultado]='ERROR' OR [Resultado]='CANCELADO' OR [Resultado]='RECHAZADO' OR [Resultado]='EJECUTADO'))
GO
ALTER TABLE [dbo].[Evento] CHECK CONSTRAINT [CK_Evento_Resultado]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Alerta](
	[IdAlerta] [uniqueidentifier] NOT NULL,
	[IdEvento] [uniqueidentifier] NULL,
	[Codigo] [varchar](40) NOT NULL,
	[Tipo] [varchar](50) NOT NULL,
	[Severidad] [varchar](20) NOT NULL,
	[Descripcion] [varchar](500) NOT NULL,
	[EstadoAlerta] [varchar](20) NOT NULL,
	[FechaHora] [datetime2](0) NOT NULL,
	[FechaReconocida] [datetime2](0) NULL,
	[IdUsuarioReconoce] [int] NULL,
	[FechaCierre] [datetime2](0) NULL,
	[IdUsuarioCierra] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdAlerta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Alerta] ADD  CONSTRAINT [DF_Alerta_Id]  DEFAULT (newsequentialid()) FOR [IdAlerta]
GO
ALTER TABLE [dbo].[Alerta] ADD  CONSTRAINT [DF_Alerta_Estado]  DEFAULT ('ACTIVA') FOR [EstadoAlerta]
GO
ALTER TABLE [dbo].[Alerta] ADD  CONSTRAINT [DF_Alerta_Fecha]  DEFAULT (sysdatetime()) FOR [FechaHora]
GO
ALTER TABLE [dbo].[Alerta]  WITH CHECK ADD  CONSTRAINT [FK_Alerta_Cierra] FOREIGN KEY([IdUsuarioCierra])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[Alerta] CHECK CONSTRAINT [FK_Alerta_Cierra]
GO
ALTER TABLE [dbo].[Alerta]  WITH CHECK ADD  CONSTRAINT [FK_Alerta_Evento] FOREIGN KEY([IdEvento])
REFERENCES [dbo].[Evento] ([IdEvento])
GO
ALTER TABLE [dbo].[Alerta] CHECK CONSTRAINT [FK_Alerta_Evento]
GO
ALTER TABLE [dbo].[Alerta]  WITH CHECK ADD  CONSTRAINT [FK_Alerta_Reconoce] FOREIGN KEY([IdUsuarioReconoce])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[Alerta] CHECK CONSTRAINT [FK_Alerta_Reconoce]
GO
ALTER TABLE [dbo].[Alerta]  WITH CHECK ADD  CONSTRAINT [CK_Alerta_Estado] CHECK  (([EstadoAlerta]='CERRADA' OR [EstadoAlerta]='RECONOCIDA' OR [EstadoAlerta]='ACTIVA'))
GO
ALTER TABLE [dbo].[Alerta] CHECK CONSTRAINT [CK_Alerta_Estado]
GO
ALTER TABLE [dbo].[Alerta]  WITH CHECK ADD  CONSTRAINT [CK_Alerta_Severidad] CHECK  (([Severidad]='CRITICA' OR [Severidad]='ALTA' OR [Severidad]='MEDIA' OR [Severidad]='BAJA' OR [Severidad]='INFO'))
GO
ALTER TABLE [dbo].[Alerta] CHECK CONSTRAINT [CK_Alerta_Severidad]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Auditoria](
	[IdAuditoria] [bigint] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NULL,
	[Modulo] [varchar](60) NOT NULL,
	[Accion] [varchar](60) NOT NULL,
	[Entidad] [varchar](60) NULL,
	[IdRegistro] [varchar](80) NULL,
	[Descripcion] [varchar](500) NULL,
	[ValorAnterior] [nvarchar](max) NULL,
	[ValorNuevo] [nvarchar](max) NULL,
	[Origen] [varchar](30) NOT NULL,
	[NombreEquipoCliente] [varchar](100) NULL,
	[FechaHora] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdAuditoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_Auditoria_FechaHora] ON [dbo].[Auditoria]
(
	[FechaHora] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Auditoria] ADD  CONSTRAINT [DF_Auditoria_Origen]  DEFAULT ('APLICACION') FOR [Origen]
GO
ALTER TABLE [dbo].[Auditoria] ADD  CONSTRAINT [DF_Auditoria_Fecha]  DEFAULT (sysdatetime()) FOR [FechaHora]
GO
ALTER TABLE [dbo].[Auditoria]  WITH CHECK ADD  CONSTRAINT [FK_Auditoria_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[Auditoria] CHECK CONSTRAINT [FK_Auditoria_Usuario]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ComandoEquipo](
	[IdComandoEquipo] [int] IDENTITY(1,1) NOT NULL,
	[IdComando] [int] NOT NULL,
	[IdEquipo] [int] NOT NULL,
	[Estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdComandoEquipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_ComandoEquipo] UNIQUE NONCLUSTERED 
(
	[IdComando] ASC,
	[IdEquipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ComandoEquipo] ADD  CONSTRAINT [DF_ComandoEquipo_Estado]  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[ComandoEquipo]  WITH CHECK ADD  CONSTRAINT [FK_ComandoEquipo_Comando] FOREIGN KEY([IdComando])
REFERENCES [dbo].[Comando] ([IdComando])
GO
ALTER TABLE [dbo].[ComandoEquipo] CHECK CONSTRAINT [FK_ComandoEquipo_Comando]
GO
ALTER TABLE [dbo].[ComandoEquipo]  WITH CHECK ADD  CONSTRAINT [FK_ComandoEquipo_Equipo] FOREIGN KEY([IdEquipo])
REFERENCES [dbo].[Equipo] ([IdEquipo])
GO
ALTER TABLE [dbo].[ComandoEquipo] CHECK CONSTRAINT [FK_ComandoEquipo_Equipo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Configuracion](
	[IdConfiguracion] [int] IDENTITY(1,1) NOT NULL,
	[Categoria] [varchar](40) NOT NULL,
	[Clave] [varchar](80) NOT NULL,
	[Valor] [varchar](300) NOT NULL,
	[TipoDato] [varchar](20) NOT NULL,
	[Descripcion] [varchar](250) NULL,
	[EsEditable] [bit] NOT NULL,
	[Estado] [bit] NOT NULL,
	[FechaModificacion] [datetime2](0) NOT NULL,
	[IdUsuarioModifica] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdConfiguracion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Clave] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Configuracion] ADD  CONSTRAINT [DF_Configuracion_Editable]  DEFAULT ((1)) FOR [EsEditable]
GO
ALTER TABLE [dbo].[Configuracion] ADD  CONSTRAINT [DF_Configuracion_Estado]  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Configuracion] ADD  CONSTRAINT [DF_Configuracion_Fecha]  DEFAULT (sysdatetime()) FOR [FechaModificacion]
GO
ALTER TABLE [dbo].[Configuracion]  WITH CHECK ADD  CONSTRAINT [FK_Configuracion_Usuario] FOREIGN KEY([IdUsuarioModifica])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[Configuracion] CHECK CONSTRAINT [FK_Configuracion_Usuario]
GO
ALTER TABLE [dbo].[Configuracion]  WITH CHECK ADD  CONSTRAINT [CK_Configuracion_TipoDato] CHECK  (([TipoDato]='BOOLEAN' OR [TipoDato]='DECIMAL' OR [TipoDato]='INTEGER' OR [TipoDato]='STRING'))
GO
ALTER TABLE [dbo].[Configuracion] CHECK CONSTRAINT [CK_Configuracion_TipoDato]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Confirmacion](
	[IdConfirmacion] [bigint] IDENTITY(1,1) NOT NULL,
	[IdEvento] [uniqueidentifier] NOT NULL,
	[NumeroPaso] [int] NOT NULL,
	[Respuesta] [varchar](20) NOT NULL,
	[FechaHora] [datetime2](0) NOT NULL,
	[TiempoRespuestaMs] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdConfirmacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Confirmacion_Evento_Paso] UNIQUE NONCLUSTERED 
(
	[IdEvento] ASC,
	[NumeroPaso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Confirmacion] ADD  CONSTRAINT [DF_Confirmacion_Fecha]  DEFAULT (sysdatetime()) FOR [FechaHora]
GO
ALTER TABLE [dbo].[Confirmacion]  WITH CHECK ADD  CONSTRAINT [FK_Confirmacion_Evento] FOREIGN KEY([IdEvento])
REFERENCES [dbo].[Evento] ([IdEvento])
GO
ALTER TABLE [dbo].[Confirmacion] CHECK CONSTRAINT [FK_Confirmacion_Evento]
GO
ALTER TABLE [dbo].[Confirmacion]  WITH CHECK ADD  CONSTRAINT [CK_Confirmacion_Paso] CHECK  (([NumeroPaso]>=(1) AND [NumeroPaso]<=(2)))
GO
ALTER TABLE [dbo].[Confirmacion] CHECK CONSTRAINT [CK_Confirmacion_Paso]
GO
ALTER TABLE [dbo].[Confirmacion]  WITH CHECK ADD  CONSTRAINT [CK_Confirmacion_Respuesta] CHECK  (([Respuesta]='TIMEOUT' OR [Respuesta]='CANCELAR' OR [Respuesta]='CONFIRMAR'))
GO
ALTER TABLE [dbo].[Confirmacion] CHECK CONSTRAINT [CK_Confirmacion_Respuesta]
GO
ALTER TABLE [dbo].[Confirmacion]  WITH CHECK ADD  CONSTRAINT [CK_Confirmacion_Tiempo] CHECK  (([TiempoRespuestaMs] IS NULL OR [TiempoRespuestaMs]>=(0)))
GO
ALTER TABLE [dbo].[Confirmacion] CHECK CONSTRAINT [CK_Confirmacion_Tiempo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TipoCredencial](
	[IdTipoCredencial] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](20) NOT NULL,
	[Nombre] [varchar](40) NOT NULL,
	[Descripcion] [varchar](200) NULL,
	[RequiereSalt] [bit] NOT NULL,
	[Estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdTipoCredencial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Nombre] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[TipoCredencial] ADD  CONSTRAINT [DF_TipoCredencial_Salt]  DEFAULT ((0)) FOR [RequiereSalt]
GO
ALTER TABLE [dbo].[TipoCredencial] ADD  CONSTRAINT [DF_TipoCredencial_Estado]  DEFAULT ((1)) FOR [Estado]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Credencial](
	[IdCredencial] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](20) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdTipoCredencial] [int] NOT NULL,
	[HashCredencial] [varbinary](64) NOT NULL,
	[Salt] [varbinary](32) NULL,
	[ReferenciaVisible] [varchar](30) NULL,
	[Estado] [bit] NOT NULL,
	[FechaRegistro] [datetime2](0) NOT NULL,
	[FechaModificacion] [datetime2](0) NULL,
	[UltimoUso] [datetime2](0) NULL,
	[IntentosFallidos] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCredencial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_Credencial_Usuario_Estado] ON [dbo].[Credencial]
(
	[IdUsuario] ASC,
	[Estado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Credencial] ADD  CONSTRAINT [DF_Credencial_Estado]  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Credencial] ADD  CONSTRAINT [DF_Credencial_FechaRegistro]  DEFAULT (sysdatetime()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Credencial] ADD  CONSTRAINT [DF_Credencial_Intentos]  DEFAULT ((0)) FOR [IntentosFallidos]
GO
ALTER TABLE [dbo].[Credencial]  WITH CHECK ADD  CONSTRAINT [FK_Credencial_Tipo] FOREIGN KEY([IdTipoCredencial])
REFERENCES [dbo].[TipoCredencial] ([IdTipoCredencial])
GO
ALTER TABLE [dbo].[Credencial] CHECK CONSTRAINT [FK_Credencial_Tipo]
GO
ALTER TABLE [dbo].[Credencial]  WITH CHECK ADD  CONSTRAINT [FK_Credencial_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([IdUsuario])
GO
ALTER TABLE [dbo].[Credencial] CHECK CONSTRAINT [FK_Credencial_Usuario]
GO
ALTER TABLE [dbo].[Credencial]  WITH CHECK ADD  CONSTRAINT [CK_Credencial_Intentos] CHECK  (([IntentosFallidos]>=(0)))
GO
ALTER TABLE [dbo].[Credencial] CHECK CONSTRAINT [CK_Credencial_Intentos]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Dispositivo](
	[IdDispositivo] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](20) NOT NULL,
	[Tipo] [varchar](40) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[NumeroSerie] [varchar](80) NULL,
	[IdEquipo] [int] NULL,
	[Estado] [bit] NOT NULL,
	[FechaRegistro] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdDispositivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Dispositivo] ADD  CONSTRAINT [DF_Dispositivo_Estado]  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Dispositivo] ADD  CONSTRAINT [DF_Dispositivo_Fecha]  DEFAULT (sysdatetime()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Dispositivo]  WITH CHECK ADD  CONSTRAINT [FK_Dispositivo_Equipo] FOREIGN KEY([IdEquipo])
REFERENCES [dbo].[Equipo] ([IdEquipo])
GO
ALTER TABLE [dbo].[Dispositivo] CHECK CONSTRAINT [FK_Dispositivo_Equipo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EstadoDispositivo](
	[IdEstadoDispositivo] [bigint] IDENTITY(1,1) NOT NULL,
	[IdDispositivo] [int] NOT NULL,
	[Estado] [varchar](40) NOT NULL,
	[Valor] [varchar](100) NULL,
	[Detalle] [varchar](300) NULL,
	[FechaHora] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdEstadoDispositivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[EstadoDispositivo] ADD  CONSTRAINT [DF_EstadoDispositivo_Fecha]  DEFAULT (sysdatetime()) FOR [FechaHora]
GO
ALTER TABLE [dbo].[EstadoDispositivo]  WITH CHECK ADD  CONSTRAINT [FK_EstadoDispositivo_Dispositivo] FOREIGN KEY([IdDispositivo])
REFERENCES [dbo].[Dispositivo] ([IdDispositivo])
GO
ALTER TABLE [dbo].[EstadoDispositivo] CHECK CONSTRAINT [FK_EstadoDispositivo_Dispositivo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sincronizacion](
	[IdSincronizacion] [bigint] IDENTITY(1,1) NOT NULL,
	[IdLote] [uniqueidentifier] NOT NULL,
	[TipoEntidad] [varchar](50) NOT NULL,
	[IdRegistroOrigen] [varchar](100) NULL,
	[Accion] [varchar](20) NOT NULL,
	[EstadoSincronizacion] [varchar](20) NOT NULL,
	[Intentos] [int] NOT NULL,
	[FechaCreacion] [datetime2](0) NOT NULL,
	[FechaUltimoIntento] [datetime2](0) NULL,
	[FechaCompletado] [datetime2](0) NULL,
	[MensajeError] [varchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdSincronizacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Sincronizacion] ADD  CONSTRAINT [DF_Sync_Lote]  DEFAULT (newid()) FOR [IdLote]
GO
ALTER TABLE [dbo].[Sincronizacion] ADD  CONSTRAINT [DF_Sync_Estado]  DEFAULT ('PENDIENTE') FOR [EstadoSincronizacion]
GO
ALTER TABLE [dbo].[Sincronizacion] ADD  CONSTRAINT [DF_Sync_Intentos]  DEFAULT ((0)) FOR [Intentos]
GO
ALTER TABLE [dbo].[Sincronizacion] ADD  CONSTRAINT [DF_Sync_Fecha]  DEFAULT (sysdatetime()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Sincronizacion]  WITH CHECK ADD  CONSTRAINT [CK_Sync_Accion] CHECK  (([Accion]='DELETE' OR [Accion]='UPDATE' OR [Accion]='INSERT'))
GO
ALTER TABLE [dbo].[Sincronizacion] CHECK CONSTRAINT [CK_Sync_Accion]
GO
ALTER TABLE [dbo].[Sincronizacion]  WITH CHECK ADD  CONSTRAINT [CK_Sync_Estado] CHECK  (([EstadoSincronizacion]='ERROR' OR [EstadoSincronizacion]='COMPLETADO' OR [EstadoSincronizacion]='PROCESANDO' OR [EstadoSincronizacion]='PENDIENTE'))
GO
ALTER TABLE [dbo].[Sincronizacion] CHECK CONSTRAINT [CK_Sync_Estado]
GO
ALTER TABLE [dbo].[Sincronizacion]  WITH CHECK ADD  CONSTRAINT [CK_Sync_Intentos] CHECK  (([Intentos]>=(0)))
GO
ALTER TABLE [dbo].[Sincronizacion] CHECK CONSTRAINT [CK_Sync_Intentos]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VersionBaseDatos](
	[IdVersion] [int] IDENTITY(1,1) NOT NULL,
	[VersionBD] [varchar](20) NOT NULL,
	[Descripcion] [varchar](250) NOT NULL,
	[FechaAplicacion] [datetime2](0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdVersion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[VersionBD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[VersionBaseDatos] ADD  CONSTRAINT [DF_VersionBaseDatos_Fecha]  DEFAULT (sysdatetime()) FOR [FechaAplicacion]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   VIEW dbo.vw_ActividadRecienteAdministrador
AS
SELECT TOP(100)
       A.IdAuditoria,A.FechaHora,A.Modulo,A.Accion,A.Entidad,
       A.IdRegistro,A.Descripcion,U.Codigo CodigoUsuario,
       CONCAT(U.Nombres,' ',U.Apellidos) Usuario
FROM dbo.Auditoria A
LEFT JOIN dbo.Usuario U ON U.IdUsuario=A.IdUsuario
ORDER BY A.FechaHora DESC;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   VIEW dbo.vw_ComandosDetalle
AS
SELECT C.IdComando,C.Codigo,C.Nombre,C.Frase,
       C.IdNivelCriticidad,NC.Nombre Criticidad,
       NC.NumeroConfirmaciones,NC.TimeoutSegundos,
       C.ConfianzaMinima,C.RequierePTT,
       TA.IdTipoAccion,TA.Codigo CodigoTipoAccion,TA.Nombre TipoAccion,
       TA.EsAccionSegura,TA.PermiteConfirmacion,
       C.Estado,
       CASE WHEN C.Estado=1 THEN 'Activo' ELSE 'Inactivo' END EstadoTexto,
       C.FechaRegistro
FROM dbo.Comando C
INNER JOIN dbo.NivelCriticidad NC ON NC.IdNivelCriticidad=C.IdNivelCriticidad
INNER JOIN dbo.TipoAccion TA ON TA.IdTipoAccion=C.IdTipoAccion;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   VIEW dbo.vw_CredencialesDetalle
AS
SELECT C.IdCredencial,C.Codigo,C.IdUsuario,U.Codigo CodigoUsuario,
       CONCAT(U.Nombres,' ',U.Apellidos) Usuario,
       TC.Codigo CodigoTipoCredencial,TC.Nombre TipoCredencial,
       C.ReferenciaVisible,C.Estado,
       CASE WHEN C.Estado=1 THEN 'Activa' ELSE 'Inactiva' END EstadoTexto,
       C.FechaRegistro,C.UltimoUso,C.IntentosFallidos
FROM dbo.Credencial C
INNER JOIN dbo.Usuario U ON U.IdUsuario=C.IdUsuario
INNER JOIN dbo.TipoCredencial TC ON TC.IdTipoCredencial=C.IdTipoCredencial;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   VIEW dbo.vw_DashboardAdministrador
AS
SELECT
    (SELECT COUNT(*) FROM dbo.Usuario) TotalUsuarios,
    (SELECT COUNT(*) FROM dbo.Usuario WHERE Estado=1) UsuariosActivos,
    (SELECT COUNT(*) FROM dbo.Credencial) TotalCredenciales,
    (SELECT COUNT(*) FROM dbo.Credencial WHERE Estado=1) CredencialesActivas,
    (SELECT COUNT(*) FROM dbo.Equipo) TotalEquipos,
    (SELECT COUNT(*) FROM dbo.Equipo WHERE Estado=1) EquiposActivos,
    (SELECT COUNT(*) FROM dbo.Comando) TotalComandos,
    (SELECT COUNT(*) FROM dbo.Comando WHERE Estado=1) ComandosActivos,
    (SELECT COUNT(*) FROM dbo.Alerta WHERE EstadoAlerta='ACTIVA') AlertasActivas,
    (SELECT COUNT(*) FROM dbo.Sincronizacion WHERE EstadoSincronizacion='PENDIENTE') SincronizacionesPendientes;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   VIEW dbo.vw_EquiposDetalle
AS
SELECT E.IdEquipo,E.Codigo,E.Nombre,
       TE.Codigo CodigoTipoEquipo,TE.Nombre TipoEquipo,
       E.Descripcion,E.IdentificadorHardware,E.UbicacionReferencia,
       E.Estado,
       CASE WHEN E.Estado=1 THEN 'Activo' ELSE 'Inactivo' END EstadoTexto,
       E.UltimoHeartbeat,E.FechaRegistro
FROM dbo.Equipo E
INNER JOIN dbo.TipoEquipo TE ON TE.IdTipoEquipo=E.IdTipoEquipo;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* 12. Vistas del Administrador */
CREATE   VIEW dbo.vw_UsuariosDetalle
AS
SELECT U.IdUsuario,U.Codigo,U.Nombres,U.Apellidos,
       CONCAT(U.Nombres,' ',U.Apellidos) NombreCompleto,
       U.Documento,U.Telefono,U.Correo,
       R.IdRol,R.Codigo CodigoRol,R.Nombre Rol,
       U.Estado,
       CASE WHEN U.Estado=1 THEN 'Activo' ELSE 'Inactivo' END EstadoTexto,
       U.FechaRegistro,U.UltimoAcceso
FROM dbo.Usuario U
INNER JOIN dbo.Rol R ON R.IdRol=U.IdRol;
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE dbo.usp_DashboardAdministrador
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * FROM dbo.vw_DashboardAdministrador;

    SELECT TOP(10) *
    FROM dbo.vw_ActividadRecienteAdministrador
    ORDER BY FechaHora DESC;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE dbo.usp_Usuario_Buscar
    @Texto VARCHAR(120)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT *
    FROM dbo.vw_UsuariosDetalle
    WHERE Codigo LIKE '%'+@Texto+'%'
       OR Nombres LIKE '%'+@Texto+'%'
       OR Apellidos LIKE '%'+@Texto+'%'
       OR NombreCompleto LIKE '%'+@Texto+'%'
       OR Documento LIKE '%'+@Texto+'%'
       OR Rol LIKE '%'+@Texto+'%'
    ORDER BY Apellidos,Nombres;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* 13. Procedimientos de lectura inicial */
CREATE   PROCEDURE dbo.usp_Usuario_Listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.vw_UsuariosDetalle
    ORDER BY Apellidos,Nombres;
END
GO

/* ======================= DATOS ======================== */
SET ANSI_NULLS ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET ARITHABORT ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER ON;
SET NUMERIC_ROUNDABORT OFF;
GO

-- dbo.Rol
SET IDENTITY_INSERT [dbo].[Rol] ON
INSERT [dbo].[Rol] ([IdRol], [Codigo], [Nombre], [Descripcion], [Estado], [FechaRegistro]) VALUES (1, N'ADMIN', N'Administrador', N'Administración completa del sistema.', 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2))
INSERT [dbo].[Rol] ([IdRol], [Codigo], [Nombre], [Descripcion], [Estado], [FechaRegistro]) VALUES (2, N'SUP', N'Supervisor', N'Supervisión de eventos, alertas y reportes.', 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2))
INSERT [dbo].[Rol] ([IdRol], [Codigo], [Nombre], [Descripcion], [Estado], [FechaRegistro]) VALUES (3, N'OPE', N'Operador', N'Operación mediante comandos autorizados.', 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2))
INSERT [dbo].[Rol] ([IdRol], [Codigo], [Nombre], [Descripcion], [Estado], [FechaRegistro]) VALUES (4, N'MANT', N'Mantenimiento', N'Diagnóstico técnico, equipos y comunicaciones.', 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Rol] OFF
GO

-- dbo.Usuario
SET IDENTITY_INSERT [dbo].[Usuario] ON
INSERT [dbo].[Usuario] ([IdUsuario], [Codigo], [Nombres], [Apellidos], [Documento], [Telefono], [Correo], [IdRol], [Estado], [FechaRegistro], [FechaModificacion], [UltimoAcceso]) VALUES (1, N'AD-001', N'Marco', N'Salas', NULL, NULL, NULL, 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL, CAST(N'2026-11-03T00:10:09.0000000' AS DateTime2))
INSERT [dbo].[Usuario] ([IdUsuario], [Codigo], [Nombres], [Apellidos], [Documento], [Telefono], [Correo], [IdRol], [Estado], [FechaRegistro], [FechaModificacion], [UltimoAcceso]) VALUES (2, N'OP-001', N'Lucía', N'Ramos', NULL, NULL, NULL, 3, 1, CAST(N'2026-11-03T00:18:49.0000000' AS DateTime2), NULL, CAST(N'2026-11-03T00:23:27.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Usuario] OFF
GO

-- dbo.TipoAccion
SET IDENTITY_INSERT [dbo].[TipoAccion] ON
INSERT [dbo].[TipoAccion] ([IdTipoAccion], [Codigo], [Nombre], [Descripcion], [EsAccionSegura], [PermiteConfirmacion], [Estado]) VALUES (1, N'INFORMATIVA', N'Informativa', N'Consulta de información o estado.', 1, 0, 1)
INSERT [dbo].[TipoAccion] ([IdTipoAccion], [Codigo], [Nombre], [Descripcion], [EsAccionSegura], [PermiteConfirmacion], [Estado]) VALUES (2, N'INDICADOR', N'Indicador', N'Activa o modifica un indicador de demostración.', 0, 1, 1)
INSERT [dbo].[TipoAccion] ([IdTipoAccion], [Codigo], [Nombre], [Descripcion], [EsAccionSegura], [PermiteConfirmacion], [Estado]) VALUES (3, N'OPERAR', N'Operar', N'Inicia una operación simulada.', 0, 1, 1)
INSERT [dbo].[TipoAccion] ([IdTipoAccion], [Codigo], [Nombre], [Descripcion], [EsAccionSegura], [PermiteConfirmacion], [Estado]) VALUES (4, N'DETENER', N'Detener', N'Detiene o desenergiza una operación simulada.', 1, 0, 1)
INSERT [dbo].[TipoAccion] ([IdTipoAccion], [Codigo], [Nombre], [Descripcion], [EsAccionSegura], [PermiteConfirmacion], [Estado]) VALUES (5, N'CRITICA', N'Crítica', N'Operación crítica únicamente de demostración.', 0, 1, 1)
INSERT [dbo].[TipoAccion] ([IdTipoAccion], [Codigo], [Nombre], [Descripcion], [EsAccionSegura], [PermiteConfirmacion], [Estado]) VALUES (6, N'ESTADO_SEGURO', N'Estado seguro', N'Fuerza el prototipo a estado seguro.', 1, 0, 1)
SET IDENTITY_INSERT [dbo].[TipoAccion] OFF
GO

-- dbo.NivelCriticidad
INSERT [dbo].[NivelCriticidad] ([IdNivelCriticidad], [Codigo], [Nombre], [NumeroConfirmaciones], [TimeoutSegundos], [Descripcion], [Estado]) VALUES (1, N'N1', N'Informativo', 0, 0, N'Consulta o acción sin impacto operacional.', 1)
INSERT [dbo].[NivelCriticidad] ([IdNivelCriticidad], [Codigo], [Nombre], [NumeroConfirmaciones], [TimeoutSegundos], [Descripcion], [Estado]) VALUES (2, N'N2', N'Bajo', 1, 10, N'Acción reversible o de bajo impacto.', 1)
INSERT [dbo].[NivelCriticidad] ([IdNivelCriticidad], [Codigo], [Nombre], [NumeroConfirmaciones], [TimeoutSegundos], [Descripcion], [Estado]) VALUES (3, N'N3', N'Alto', 1, 8, N'Acción operacional que requiere confirmación explícita.', 1)
INSERT [dbo].[NivelCriticidad] ([IdNivelCriticidad], [Codigo], [Nombre], [NumeroConfirmaciones], [TimeoutSegundos], [Descripcion], [Estado]) VALUES (4, N'N4', N'Crítico', 2, 5, N'Acción crítica simulada con doble confirmación y timeout.', 1)
GO

-- dbo.Comando
SET IDENTITY_INSERT [dbo].[Comando] ON
INSERT [dbo].[Comando] ([IdComando], [Codigo], [Nombre], [Frase], [IdNivelCriticidad], [IdTipoAccion], [ConfianzaMinima], [RequierePTT], [Estado], [FechaRegistro], [FechaModificacion]) VALUES (1, N'CMD-001', N'Consultar estado', N'ESTADO DEL EQUIPO', 1, 1, CAST(85.00 AS Decimal(5, 2)), 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
INSERT [dbo].[Comando] ([IdComando], [Codigo], [Nombre], [Frase], [IdNivelCriticidad], [IdTipoAccion], [ConfianzaMinima], [RequierePTT], [Estado], [FechaRegistro], [FechaModificacion]) VALUES (2, N'CMD-002', N'Encender indicador', N'ENCENDER INDICADOR', 2, 2, CAST(85.00 AS Decimal(5, 2)), 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
INSERT [dbo].[Comando] ([IdComando], [Codigo], [Nombre], [Frase], [IdNivelCriticidad], [IdTipoAccion], [ConfianzaMinima], [RequierePTT], [Estado], [FechaRegistro], [FechaModificacion]) VALUES (3, N'CMD-003', N'Iniciar simulador', N'INICIAR SIMULADOR', 3, 3, CAST(90.00 AS Decimal(5, 2)), 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
INSERT [dbo].[Comando] ([IdComando], [Codigo], [Nombre], [Frase], [IdNivelCriticidad], [IdTipoAccion], [ConfianzaMinima], [RequierePTT], [Estado], [FechaRegistro], [FechaModificacion]) VALUES (4, N'CMD-004', N'Detener simulador', N'DETENER SIMULADOR', 2, 4, CAST(85.00 AS Decimal(5, 2)), 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
INSERT [dbo].[Comando] ([IdComando], [Codigo], [Nombre], [Frase], [IdNivelCriticidad], [IdTipoAccion], [ConfianzaMinima], [RequierePTT], [Estado], [FechaRegistro], [FechaModificacion]) VALUES (5, N'CMD-005', N'Operación crítica simulada', N'OPERACION CRITICA SIMULADA', 4, 5, CAST(95.00 AS Decimal(5, 2)), 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
SET IDENTITY_INSERT [dbo].[Comando] OFF
GO

-- dbo.TipoEquipo
SET IDENTITY_INSERT [dbo].[TipoEquipo] ON
INSERT [dbo].[TipoEquipo] ([IdTipoEquipo], [Codigo], [Nombre], [Descripcion], [Estado]) VALUES (1, N'CTRL', N'Controlador', N'Controlador electrónico del prototipo.', 1)
INSERT [dbo].[TipoEquipo] ([IdTipoEquipo], [Codigo], [Nombre], [Descripcion], [Estado]) VALUES (2, N'ACT', N'Actuador', N'Actuador físico de demostración.', 1)
INSERT [dbo].[TipoEquipo] ([IdTipoEquipo], [Codigo], [Nombre], [Descripcion], [Estado]) VALUES (3, N'IND', N'Indicador', N'Indicador luminoso o sonoro.', 1)
INSERT [dbo].[TipoEquipo] ([IdTipoEquipo], [Codigo], [Nombre], [Descripcion], [Estado]) VALUES (4, N'SIM', N'Simulador', N'Equipo que representa una operación industrial.', 1)
SET IDENTITY_INSERT [dbo].[TipoEquipo] OFF
GO

-- dbo.Equipo
SET IDENTITY_INSERT [dbo].[Equipo] ON
INSERT [dbo].[Equipo] ([IdEquipo], [Codigo], [Nombre], [IdTipoEquipo], [Descripcion], [IdentificadorHardware], [UbicacionReferencia], [Estado], [UltimoHeartbeat], [FechaRegistro], [FechaModificacion]) VALUES (1, N'EQ-001', N'Simulador principal', 4, N'Equipo académico de demostración.', N'SIM-001', NULL, 1, NULL, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
SET IDENTITY_INSERT [dbo].[Equipo] OFF
GO

-- dbo.ComandoEquipo
SET IDENTITY_INSERT [dbo].[ComandoEquipo] ON
INSERT [dbo].[ComandoEquipo] ([IdComandoEquipo], [IdComando], [IdEquipo], [Estado]) VALUES (1, 1, 1, 1)
INSERT [dbo].[ComandoEquipo] ([IdComandoEquipo], [IdComando], [IdEquipo], [Estado]) VALUES (2, 2, 1, 1)
INSERT [dbo].[ComandoEquipo] ([IdComandoEquipo], [IdComando], [IdEquipo], [Estado]) VALUES (3, 3, 1, 1)
INSERT [dbo].[ComandoEquipo] ([IdComandoEquipo], [IdComando], [IdEquipo], [Estado]) VALUES (4, 4, 1, 1)
INSERT [dbo].[ComandoEquipo] ([IdComandoEquipo], [IdComando], [IdEquipo], [Estado]) VALUES (5, 5, 1, 1)
SET IDENTITY_INSERT [dbo].[ComandoEquipo] OFF
GO

-- dbo.Configuracion
SET IDENTITY_INSERT [dbo].[Configuracion] ON
INSERT [dbo].[Configuracion] ([IdConfiguracion], [Categoria], [Clave], [Valor], [TipoDato], [Descripcion], [EsEditable], [Estado], [FechaModificacion], [IdUsuarioModifica]) VALUES (1, N'VOZ', N'UMBRAL_CONFIANZA', N'85', N'DECIMAL', N'Confianza mínima global.', 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
INSERT [dbo].[Configuracion] ([IdConfiguracion], [Categoria], [Clave], [Valor], [TipoDato], [Descripcion], [EsEditable], [Estado], [FechaModificacion], [IdUsuarioModifica]) VALUES (2, N'VOZ', N'TIMEOUT_CONFIRMACION', N'8', N'INTEGER', N'Timeout global de confirmación.', 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
INSERT [dbo].[Configuracion] ([IdConfiguracion], [Categoria], [Clave], [Valor], [TipoDato], [Descripcion], [EsEditable], [Estado], [FechaModificacion], [IdUsuarioModifica]) VALUES (3, N'VOZ', N'GRAMATICA_CERRADA', N'true', N'BOOLEAN', N'Restringe el reconocimiento a comandos autorizados.', 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
INSERT [dbo].[Configuracion] ([IdConfiguracion], [Categoria], [Clave], [Valor], [TipoDato], [Descripcion], [EsEditable], [Estado], [FechaModificacion], [IdUsuarioModifica]) VALUES (4, N'VOZ', N'PTT_OBLIGATORIO', N'true', N'BOOLEAN', N'PTT obligatorio.', 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
INSERT [dbo].[Configuracion] ([IdConfiguracion], [Categoria], [Clave], [Valor], [TipoDato], [Descripcion], [EsEditable], [Estado], [FechaModificacion], [IdUsuarioModifica]) VALUES (5, N'COMUNICACION', N'PUERTO_COM', N'COM3', N'STRING', N'Puerto serial del ESP32.', 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
INSERT [dbo].[Configuracion] ([IdConfiguracion], [Categoria], [Clave], [Valor], [TipoDato], [Descripcion], [EsEditable], [Estado], [FechaModificacion], [IdUsuarioModifica]) VALUES (6, N'COMUNICACION', N'BAUD_RATE', N'115200', N'INTEGER', N'Velocidad serial.', 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
INSERT [dbo].[Configuracion] ([IdConfiguracion], [Categoria], [Clave], [Valor], [TipoDato], [Descripcion], [EsEditable], [Estado], [FechaModificacion], [IdUsuarioModifica]) VALUES (7, N'COMUNICACION', N'HEARTBEAT_MS', N'250', N'INTEGER', N'Intervalo de heartbeat.', 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
INSERT [dbo].[Configuracion] ([IdConfiguracion], [Categoria], [Clave], [Valor], [TipoDato], [Descripcion], [EsEditable], [Estado], [FechaModificacion], [IdUsuarioModifica]) VALUES (8, N'COMUNICACION', N'MAX_HEARTBEAT_PERDIDOS', N'3', N'INTEGER', N'Máximo de heartbeat perdidos.', 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
INSERT [dbo].[Configuracion] ([IdConfiguracion], [Categoria], [Clave], [Valor], [TipoDato], [Descripcion], [EsEditable], [Estado], [FechaModificacion], [IdUsuarioModifica]) VALUES (9, N'COMUNICACION', N'CRC_OBLIGATORIO', N'true', N'BOOLEAN', N'CRC obligatorio.', 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
INSERT [dbo].[Configuracion] ([IdConfiguracion], [Categoria], [Clave], [Valor], [TipoDato], [Descripcion], [EsEditable], [Estado], [FechaModificacion], [IdUsuarioModifica]) VALUES (10, N'COMUNICACION', N'SECUENCIA_OBLIGATORIA', N'true', N'BOOLEAN', N'Secuencia obligatoria.', 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
INSERT [dbo].[Configuracion] ([IdConfiguracion], [Categoria], [Clave], [Valor], [TipoDato], [Descripcion], [EsEditable], [Estado], [FechaModificacion], [IdUsuarioModifica]) VALUES (11, N'SEGURIDAD', N'WATCHDOG_ACTIVO', N'true', N'BOOLEAN', N'Watchdog activo.', 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
INSERT [dbo].[Configuracion] ([IdConfiguracion], [Categoria], [Clave], [Valor], [TipoDato], [Descripcion], [EsEditable], [Estado], [FechaModificacion], [IdUsuarioModifica]) VALUES (12, N'SEGURIDAD', N'REARME_REQUERIDO', N'true', N'BOOLEAN', N'Rearme explícito después de parada.', 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
INSERT [dbo].[Configuracion] ([IdConfiguracion], [Categoria], [Clave], [Valor], [TipoDato], [Descripcion], [EsEditable], [Estado], [FechaModificacion], [IdUsuarioModifica]) VALUES (13, N'PERSISTENCIA', N'MODO_OFFLINE', N'true', N'BOOLEAN', N'Permite trabajo offline.', 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
INSERT [dbo].[Configuracion] ([IdConfiguracion], [Categoria], [Clave], [Valor], [TipoDato], [Descripcion], [EsEditable], [Estado], [FechaModificacion], [IdUsuarioModifica]) VALUES (14, N'PERSISTENCIA', N'SINCRONIZACION_AUTOMATICA', N'true', N'BOOLEAN', N'Sincronización automática.', 1, 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL)
SET IDENTITY_INSERT [dbo].[Configuracion] OFF
GO

-- dbo.TipoCredencial
SET IDENTITY_INSERT [dbo].[TipoCredencial] ON
INSERT [dbo].[TipoCredencial] ([IdTipoCredencial], [Codigo], [Nombre], [Descripcion], [RequiereSalt], [Estado]) VALUES (1, N'PIN', N'PIN', N'Código numérico de autenticación.', 1, 1)
INSERT [dbo].[TipoCredencial] ([IdTipoCredencial], [Codigo], [Nombre], [Descripcion], [RequiereSalt], [Estado]) VALUES (2, N'RFID', N'RFID', N'Credencial RFID asociada al usuario.', 0, 1)
INSERT [dbo].[TipoCredencial] ([IdTipoCredencial], [Codigo], [Nombre], [Descripcion], [RequiereSalt], [Estado]) VALUES (3, N'NFC', N'NFC', N'Credencial NFC asociada al usuario.', 0, 1)
SET IDENTITY_INSERT [dbo].[TipoCredencial] OFF
GO

-- dbo.Credencial
SET IDENTITY_INSERT [dbo].[Credencial] ON
INSERT [dbo].[Credencial] ([IdCredencial], [Codigo], [IdUsuario], [IdTipoCredencial], [HashCredencial], [Salt], [ReferenciaVisible], [Estado], [FechaRegistro], [FechaModificacion], [UltimoUso], [IntentosFallidos]) VALUES (1, N'CRD-AD-001-PIN', 1, 1, 0x03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4, NULL, N'PIN DEMO', 1, CAST(N'2026-10-26T14:26:35.0000000' AS DateTime2), NULL, CAST(N'2026-11-03T00:10:09.0000000' AS DateTime2), 0)
INSERT [dbo].[Credencial] ([IdCredencial], [Codigo], [IdUsuario], [IdTipoCredencial], [HashCredencial], [Salt], [ReferenciaVisible], [Estado], [FechaRegistro], [FechaModificacion], [UltimoUso], [IntentosFallidos]) VALUES (2, N'CRD-OP-001-PIN', 2, 1, 0xA1FB4E703A9EF1FA4936801721FF285A97AC85330856674412E054892AFE6972, NULL, N'PIN DEMO', 1, CAST(N'2026-11-03T00:18:49.0000000' AS DateTime2), NULL, CAST(N'2026-11-03T00:23:27.0000000' AS DateTime2), 0)
SET IDENTITY_INSERT [dbo].[Credencial] OFF
GO

-- dbo.VersionBaseDatos
SET IDENTITY_INSERT [dbo].[VersionBaseDatos] ON
INSERT [dbo].[VersionBaseDatos] ([IdVersion], [VersionBD], [Descripcion], [FechaAplicacion]) VALUES (1, N'0.1.0', N'Fase 0: núcleo administrativo y preparación para operación, voz, auditoría, sincronización y ESP32.', CAST(N'2026-10-26T14:26:34.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[VersionBaseDatos] OFF
GO

PRINT 'ORICA_SafeHandsDB creada correctamente.';
GO
