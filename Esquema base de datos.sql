/****** Object:  Table [dbo].[Asignaturas]    Script Date: 28/12/2023 13:18:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Asignaturas](
	[IdAsignatura] [int] IDENTITY(1,1) NOT NULL,
	[CodigoAsignatura] [nvarchar](10) NOT NULL,
	[NombreAsignatura] [nvarchar](100) NOT NULL,
	[CreditosAsignatura] [int] NOT NULL,
 CONSTRAINT [PK_Asignaturas] PRIMARY KEY CLUSTERED 
(
	[IdAsignatura] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 70, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_CodigoAsignatura] UNIQUE NONCLUSTERED 
(
	[CodigoAsignatura] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 70, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Carreras]    Script Date: 28/12/2023 13:18:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Carreras](
	[IdCarrera] [int] IDENTITY(1,1) NOT NULL,
	[CodigoCarrera] [nvarchar](6) NOT NULL,
	[NombreCarrera] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Carreras] PRIMARY KEY CLUSTERED 
(
	[IdCarrera] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_CodigoCarrera] UNIQUE NONCLUSTERED 
(
	[CodigoCarrera] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dias]    Script Date: 28/12/2023 13:18:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dias](
	[IdDia] [int] IDENTITY(7,1) NOT NULL,
	[NombreDia] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_Dia_IdDia] PRIMARY KEY CLUSTERED 
(
	[IdDia] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Horarios]    Script Date: 28/12/2023 13:18:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Horarios](
	[IdHora] [int] IDENTITY(1,1) NOT NULL,
	[Horas] [nvarchar](5) NULL,
	[IdDia] [int] NULL,
	[IdSeccion] [int] NOT NULL,
	[EstadoHora] [int] NOT NULL,
	[IdVersionTrimestral] [int] NOT NULL,
	[IdModalidad] [int] NULL,
 CONSTRAINT [PK_Horario_IdHora] PRIMARY KEY CLUSTERED 
(
	[IdHora] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 70, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HorariosSeleccionados]    Script Date: 28/12/2023 13:18:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HorariosSeleccionados](
	[IdHorarioSeleccion] [int] IDENTITY(1,1) NOT NULL,
	[HoraSeleccion] [nvarchar](5) NOT NULL,
	[IdProfesor] [int] NOT NULL,
	[IdDia] [int] NOT NULL,
	[IdModalidad] [int] NOT NULL,
 CONSTRAINT [PK_HorarioSeleccionado_IdHorarioSeleccion] PRIMARY KEY CLUSTERED 
(
	[IdHorarioSeleccion] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 70, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Modalidades]    Script Date: 28/12/2023 13:18:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modalidades](
	[IdModalidad] [int] IDENTITY(1,1) NOT NULL,
	[NombreModalidad] [nvarchar](15) NOT NULL,
 CONSTRAINT [PK_Modalidad_IdModalidad] PRIMARY KEY CLUSTERED 
(
	[IdModalidad] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pensums]    Script Date: 28/12/2023 13:18:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pensums](
	[IdPensum] [int] IDENTITY(213,1) NOT NULL,
	[IdVersionPensum] [int] NOT NULL,
	[IdCarrera] [int] NOT NULL,
	[IdAsignatura] [int] NOT NULL,
	[Trimestre] [int] NOT NULL,
 CONSTRAINT [PK_Pensum_IdPensum] PRIMARY KEY CLUSTERED 
(
	[IdPensum] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 70, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Profesores]    Script Date: 28/12/2023 13:18:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profesores](
	[IdProfesor] [int] IDENTITY(1234568,1) NOT NULL,
	[NombreProfesor] [nvarchar](100) NOT NULL,
	[PrioridadProfesor] [smallint] NOT NULL,
	[ContraseñaProfesor] [nvarchar](20) NOT NULL,
	[CoordinadorStatus] [smallint] NOT NULL,
 CONSTRAINT [PK_Profesor_IdProfesor] PRIMARY KEY CLUSTERED 
(
	[IdProfesor] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 70, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Secciones]    Script Date: 28/12/2023 13:18:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Secciones](
	[IdSeccion] [int] IDENTITY(1,1) NOT NULL,
	[CodigoSeccion] [int] NOT NULL,
	[IdProfesor] [int] NOT NULL,
	[IdAsignatura] [int] NOT NULL,
	[IdModalidad] [int] NOT NULL,
 CONSTRAINT [PK_Seccion_IdSeccion] PRIMARY KEY CLUSTERED 
(
	[IdSeccion] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 70, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VersionesPensums]    Script Date: 28/12/2023 13:18:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VersionesPensums](
	[IdVersionPensum] [int] IDENTITY(2,1) NOT NULL,
	[AñoPensum] [int] NOT NULL,
 CONSTRAINT [PK_VersionPensum_IdVersionPensum] PRIMARY KEY CLUSTERED 
(
	[IdVersionPensum] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VersionesTrimestrales]    Script Date: 28/12/2023 13:18:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VersionesTrimestrales](
	[IdVersionTrimestral] [int] IDENTITY(1,1) NOT NULL,
	[VersionTrimestral] [nvarchar](5) NOT NULL,
 CONSTRAINT [PK_VersionTrimestral] PRIMARY KEY CLUSTERED 
(
	[IdVersionTrimestral] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Horarios]  WITH CHECK ADD  CONSTRAINT [FK_Horario_Modalidad] FOREIGN KEY([IdModalidad])
REFERENCES [dbo].[Modalidades] ([IdModalidad])
GO
ALTER TABLE [dbo].[Horarios] CHECK CONSTRAINT [FK_Horario_Modalidad]
GO
ALTER TABLE [dbo].[Horarios]  WITH CHECK ADD  CONSTRAINT [FK_Horario_VersionTrimestral] FOREIGN KEY([IdVersionTrimestral])
REFERENCES [dbo].[VersionesTrimestrales] ([IdVersionTrimestral])
GO
ALTER TABLE [dbo].[Horarios] CHECK CONSTRAINT [FK_Horario_VersionTrimestral]
GO
ALTER TABLE [dbo].[Horarios]  WITH NOCHECK ADD  CONSTRAINT [Horario$fk_Hora_Dia1] FOREIGN KEY([IdDia])
REFERENCES [dbo].[Dias] ([IdDia])
GO
ALTER TABLE [dbo].[Horarios] CHECK CONSTRAINT [Horario$fk_Hora_Dia1]
GO
ALTER TABLE [dbo].[Horarios]  WITH NOCHECK ADD  CONSTRAINT [Horario$fk_Hora_Seccion1] FOREIGN KEY([IdSeccion])
REFERENCES [dbo].[Secciones] ([IdSeccion])
GO
ALTER TABLE [dbo].[Horarios] CHECK CONSTRAINT [Horario$fk_Hora_Seccion1]
GO
ALTER TABLE [dbo].[HorariosSeleccionados]  WITH NOCHECK ADD  CONSTRAINT [HorarioSeleccionado$fk_HorarioSeleccionado_Dia1] FOREIGN KEY([IdDia])
REFERENCES [dbo].[Dias] ([IdDia])
GO
ALTER TABLE [dbo].[HorariosSeleccionados] CHECK CONSTRAINT [HorarioSeleccionado$fk_HorarioSeleccionado_Dia1]
GO
ALTER TABLE [dbo].[HorariosSeleccionados]  WITH NOCHECK ADD  CONSTRAINT [HorarioSeleccionado$fk_HorarioSeleccionado_Modalidad1] FOREIGN KEY([IdModalidad])
REFERENCES [dbo].[Modalidades] ([IdModalidad])
GO
ALTER TABLE [dbo].[HorariosSeleccionados] CHECK CONSTRAINT [HorarioSeleccionado$fk_HorarioSeleccionado_Modalidad1]
GO
ALTER TABLE [dbo].[HorariosSeleccionados]  WITH NOCHECK ADD  CONSTRAINT [HorarioSeleccionado$fk_HorarioSeleccionado_Profesor1] FOREIGN KEY([IdProfesor])
REFERENCES [dbo].[Profesores] ([IdProfesor])
GO
ALTER TABLE [dbo].[HorariosSeleccionados] CHECK CONSTRAINT [HorarioSeleccionado$fk_HorarioSeleccionado_Profesor1]
GO
ALTER TABLE [dbo].[Pensums]  WITH CHECK ADD  CONSTRAINT [FK_Pensums_Asignaturas] FOREIGN KEY([IdAsignatura])
REFERENCES [dbo].[Asignaturas] ([IdAsignatura])
GO
ALTER TABLE [dbo].[Pensums] CHECK CONSTRAINT [FK_Pensums_Asignaturas]
GO
ALTER TABLE [dbo].[Pensums]  WITH CHECK ADD  CONSTRAINT [FK_Pensums_Carreras] FOREIGN KEY([IdCarrera])
REFERENCES [dbo].[Carreras] ([IdCarrera])
GO
ALTER TABLE [dbo].[Pensums] CHECK CONSTRAINT [FK_Pensums_Carreras]
GO
ALTER TABLE [dbo].[Pensums]  WITH NOCHECK ADD  CONSTRAINT [Pensum$fk_Pensum_VersionPensum1] FOREIGN KEY([IdVersionPensum])
REFERENCES [dbo].[VersionesPensums] ([IdVersionPensum])
GO
ALTER TABLE [dbo].[Pensums] CHECK CONSTRAINT [Pensum$fk_Pensum_VersionPensum1]
GO
ALTER TABLE [dbo].[Secciones]  WITH CHECK ADD  CONSTRAINT [FK_Secciones_Asignaturas] FOREIGN KEY([IdAsignatura])
REFERENCES [dbo].[Asignaturas] ([IdAsignatura])
GO
ALTER TABLE [dbo].[Secciones] CHECK CONSTRAINT [FK_Secciones_Asignaturas]
GO
ALTER TABLE [dbo].[Secciones]  WITH NOCHECK ADD  CONSTRAINT [Seccion$fk_Seccion_Modalidad1] FOREIGN KEY([IdModalidad])
REFERENCES [dbo].[Modalidades] ([IdModalidad])
GO
ALTER TABLE [dbo].[Secciones] CHECK CONSTRAINT [Seccion$fk_Seccion_Modalidad1]
GO
ALTER TABLE [dbo].[Secciones]  WITH NOCHECK ADD  CONSTRAINT [Seccion$fk_Seccion_Profesor1] FOREIGN KEY([IdProfesor])
REFERENCES [dbo].[Profesores] ([IdProfesor])
GO
ALTER TABLE [dbo].[Secciones] CHECK CONSTRAINT [Seccion$fk_Seccion_Profesor1]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'Carrera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Carreras'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'Dia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dias'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'Horario' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Horarios'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'HorarioSeleccionado' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HorariosSeleccionados'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'Modalidad' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Modalidades'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'Pensum' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pensums'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'Profesor' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Profesores'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'Seccion' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Secciones'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'VersionPensum' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'VersionesPensums'
GO
