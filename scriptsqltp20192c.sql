DROP TABLE IF EXISTS [TP-20192C];
GO
CREATE DATABASE [TP-20192C]
GO
USE [TP-20192C]
GO
CREATE TABLE [dbo].Motivo(
	[IdMotivo] [int] IDENTITY(1,1) NOT NULL  PRIMARY KEY,
	[Descripcion] [nvarchar](30) NOT NULL)
GO
CREATE TABLE [dbo].[Denuncias](
	[IdDenuncia] [int] IDENTITY(1,1) NOT NULL,
	[IdPropuesta] [int] NOT NULL,
	[IdMotivo] [int] NOT NULL,
	[Comentarios] [nvarchar](300) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[Estado] [int] NOT NULL,
	 CONSTRAINT [FK_Motivo] FOREIGN KEY (IdMotivo) REFERENCES Motivo (IdMotivo),
 CONSTRAINT [PK_Denuncias] PRIMARY KEY CLUSTERED 
(
	[IdDenuncia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonacionesHorasTrabajo](
	[IdDonacionHorasTrabajo] [int] IDENTITY(1,1) NOT NULL,
	[IdPropuestaDonacionHorasTrabajo] [int] NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[Cantidad] [int] NOT NULL,
 CONSTRAINT [PK_DonacionesHorasTrabajo] PRIMARY KEY CLUSTERED 
(
	[IdDonacionHorasTrabajo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonacionesInsumos](
	[IdDonacionInsumo] [int] IDENTITY(1,1) NOT NULL,
	[IdPropuestaDonacionInsumo] [int] NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[Cantidad] [int] NOT NULL,
 CONSTRAINT [PK_DonacionesInsumos] PRIMARY KEY CLUSTERED 
(
	[IdDonacionInsumo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonacionesMonetarias](
	[IdDonacionMonetaria] [int] IDENTITY(1,1) NOT NULL,
	[IdPropuestaDonacionMonetaria] [int] NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[Dinero] [decimal](18, 2) NOT NULL,
	[ArchivoTransferencia] [nvarchar](200) NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
 CONSTRAINT [PK_DonacionesMonetarias] PRIMARY KEY CLUSTERED 
(
	[IdDonacionMonetaria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Propuestas](
	[IdPropuesta] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Descripcion] [text] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[FechaFin] [datetime] NOT NULL,
	[TelefonoContacto] [nvarchar](30) NOT NULL,
	[TipoDonacion] [int] NOT NULL,
	[Foto] [nvarchar](100) NOT NULL,
	[IdUsuarioCreador] [int] NOT NULL,
	[Estado] [int] NOT NULL,
	[Valoracion] [decimal](18, 0) NULL,
 CONSTRAINT [PK_Propuestas] PRIMARY KEY CLUSTERED 
(
	[IdPropuesta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropuestasDonacionesHorasTrabajo](
	[IdPropuestaDonacionHorasTrabajo] [int] IDENTITY(1,1) NOT NULL,
	[IdPropuesta] [int] NOT NULL,
	[CantidadHoras] [int] NOT NULL,
	[Profesion] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_PropuestasDonacionesHorasTrabajo] PRIMARY KEY CLUSTERED 
(
	[IdPropuestaDonacionHorasTrabajo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropuestasDonacionesInsumos](
	[IdPropuestaDonacionInsumo] [int] IDENTITY(1,1) NOT NULL,
	[IdPropuesta] [int] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Cantidad] [int] NOT NULL,
 CONSTRAINT [PK_PropuestasDonacionesInsumos] PRIMARY KEY CLUSTERED 
(
	[IdPropuestaDonacionInsumo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropuestasDonacionesMonetarias](
	[IdPropuestaDonacionMonetaria] [int] IDENTITY(1,1) NOT NULL,
	[IdPropuesta] [int] NOT NULL,
	[Dinero] [decimal](18, 2) NOT NULL,
	[CBU] [nvarchar](80) NOT NULL,
 CONSTRAINT [PK_PropuestasDonacionesMonetarias] PRIMARY KEY CLUSTERED 
(
	[IdPropuestaDonacionMonetaria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropuestasReferencias](
	[IdReferencia] [int] IDENTITY(1,1) NOT NULL,
	[IdPropuesta] [int] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Telefono] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_PropuestasReferencias] PRIMARY KEY CLUSTERED 
(
	[IdReferencia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropuestasValoraciones](
	[IdValoracion] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdPropuesta] [int] NOT NULL,
	[Valoracion] [bit] NOT NULL,
 CONSTRAINT [PK_PropuestasValoraciones] PRIMARY KEY CLUSTERED 
(
	[IdValoracion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NULL,
	[Apellido] [nvarchar](50) NULL,
	[FechaNacimiento] [datetime] NOT NULL,
	[UserName] [nvarchar](20) NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](20) NOT NULL,
	[Foto] [nvarchar](100) NULL,
	[TipoUsuario] [int] NOT NULL,
	[FechaCracion] [datetime] NOT NULL,
	[Activo] [bit] NOT NULL,
	[Token] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Propuestas] ADD  DEFAULT ((0)) FOR [Valoracion]
GO
ALTER TABLE [dbo].[Denuncias]  WITH CHECK ADD  CONSTRAINT [FK_Denuncias_Propuestas] FOREIGN KEY([IdPropuesta])
REFERENCES [dbo].[Propuestas] ([IdPropuesta])
GO
ALTER TABLE [dbo].[Denuncias] CHECK CONSTRAINT [FK_Denuncias_Propuestas]
GO
ALTER TABLE [dbo].[Denuncias]  WITH CHECK ADD  CONSTRAINT [FK_Denuncias_Usuarios] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[Denuncias] CHECK CONSTRAINT [FK_Denuncias_Usuarios]
GO
ALTER TABLE [dbo].[DonacionesHorasTrabajo]  WITH CHECK ADD  CONSTRAINT [FK_DonacionesHorasTrabajo_PropuestasDonacionesHorasTrabajo] FOREIGN KEY([IdPropuestaDonacionHorasTrabajo])
REFERENCES [dbo].[PropuestasDonacionesHorasTrabajo] ([IdPropuestaDonacionHorasTrabajo])
GO
ALTER TABLE [dbo].[DonacionesHorasTrabajo] CHECK CONSTRAINT [FK_DonacionesHorasTrabajo_PropuestasDonacionesHorasTrabajo]
GO
ALTER TABLE [dbo].[DonacionesHorasTrabajo]  WITH CHECK ADD  CONSTRAINT [FK_DonacionesHorasTrabajo_Usuarios] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[DonacionesHorasTrabajo] CHECK CONSTRAINT [FK_DonacionesHorasTrabajo_Usuarios]
GO
ALTER TABLE [dbo].[DonacionesInsumos]  WITH CHECK ADD  CONSTRAINT [FK_DonacionesInsumos_PropuestasDonacionesInsumos] FOREIGN KEY([IdPropuestaDonacionInsumo])
REFERENCES [dbo].[PropuestasDonacionesInsumos] ([IdPropuestaDonacionInsumo])
GO
ALTER TABLE [dbo].[DonacionesInsumos] CHECK CONSTRAINT [FK_DonacionesInsumos_PropuestasDonacionesInsumos]
GO
ALTER TABLE [dbo].[DonacionesInsumos]  WITH CHECK ADD  CONSTRAINT [FK_DonacionesInsumos_Usuarios] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[DonacionesInsumos] CHECK CONSTRAINT [FK_DonacionesInsumos_Usuarios]
GO
ALTER TABLE [dbo].[DonacionesMonetarias]  WITH CHECK ADD  CONSTRAINT [FK_DonacionesMonetarias_PropuestasDonacionesMonetarias] FOREIGN KEY([IdPropuestaDonacionMonetaria])
REFERENCES [dbo].[PropuestasDonacionesMonetarias] ([IdPropuestaDonacionMonetaria])
GO
ALTER TABLE [dbo].[DonacionesMonetarias] CHECK CONSTRAINT [FK_DonacionesMonetarias_PropuestasDonacionesMonetarias]
GO
ALTER TABLE [dbo].[DonacionesMonetarias]  WITH CHECK ADD  CONSTRAINT [FK_DonacionesMonetarias_Usuarios] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[DonacionesMonetarias] CHECK CONSTRAINT [FK_DonacionesMonetarias_Usuarios]
GO
ALTER TABLE [dbo].[Propuestas]  WITH CHECK ADD  CONSTRAINT [FK_Propuestas_Usuarios] FOREIGN KEY([IdUsuarioCreador])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[Propuestas] CHECK CONSTRAINT [FK_Propuestas_Usuarios]
GO
ALTER TABLE [dbo].[PropuestasDonacionesHorasTrabajo]  WITH CHECK ADD  CONSTRAINT [FK_PropuestasDonacionesHorasTrabajo_Propuestas] FOREIGN KEY([IdPropuesta])
REFERENCES [dbo].[Propuestas] ([IdPropuesta])
GO
ALTER TABLE [dbo].[PropuestasDonacionesHorasTrabajo] CHECK CONSTRAINT [FK_PropuestasDonacionesHorasTrabajo_Propuestas]
GO
ALTER TABLE [dbo].[PropuestasDonacionesInsumos]  WITH CHECK ADD  CONSTRAINT [FK_PropuestasDonacionesInsumos_Propuestas] FOREIGN KEY([IdPropuesta])
REFERENCES [dbo].[Propuestas] ([IdPropuesta])
GO
ALTER TABLE [dbo].[PropuestasDonacionesInsumos] CHECK CONSTRAINT [FK_PropuestasDonacionesInsumos_Propuestas]
GO
ALTER TABLE [dbo].[PropuestasDonacionesMonetarias]  WITH CHECK ADD  CONSTRAINT [FK_PropuestasDonacionesMonetarias_Propuestas] FOREIGN KEY([IdPropuesta])
REFERENCES [dbo].[Propuestas] ([IdPropuesta])
GO
ALTER TABLE [dbo].[PropuestasDonacionesMonetarias] CHECK CONSTRAINT [FK_PropuestasDonacionesMonetarias_Propuestas]
GO
ALTER TABLE [dbo].[PropuestasReferencias]  WITH CHECK ADD  CONSTRAINT [FK_PropuestasReferencias_Propuestas] FOREIGN KEY([IdPropuesta])
REFERENCES [dbo].[Propuestas] ([IdPropuesta])
GO
ALTER TABLE [dbo].[PropuestasReferencias] CHECK CONSTRAINT [FK_PropuestasReferencias_Propuestas]
GO
ALTER TABLE [dbo].[PropuestasValoraciones]  WITH CHECK ADD  CONSTRAINT [FK_PropuestasValoraciones_Propuestas] FOREIGN KEY([IdPropuesta])
REFERENCES [dbo].[Propuestas] ([IdPropuesta])
GO
ALTER TABLE [dbo].[PropuestasValoraciones] CHECK CONSTRAINT [FK_PropuestasValoraciones_Propuestas]
GO
ALTER TABLE [dbo].[PropuestasValoraciones]  WITH CHECK ADD  CONSTRAINT [FK_PropuestasValoraciones_Usuarios] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[PropuestasValoraciones] CHECK CONSTRAINT [FK_PropuestasValoraciones_Usuarios]
GO
INSERT INTO [dbo].[Motivo]([Descripcion])
VALUES('Fraude')
GO
INSERT INTO [dbo].[Motivo]([Descripcion])
VALUES('Indebida')
GO
INSERT INTO [dbo].[Motivo]([Descripcion])
VALUES('Violación de derechos')
GO
INSERT INTO [dbo].[Motivo]([Descripcion])
VALUES('Contiene Información política')
GO
INSERT INTO [dbo].[Usuarios]
           ([Nombre]
		   ,[Apellido]
           ,[FechaNacimiento]
           ,[UserName]
           ,[Email]
           ,[Password]
           ,[Foto]
           ,[TipoUsuario]
           ,[FechaCracion]
           ,[Activo]
           ,[Token])
     VALUES
           ('Nicolás'
           ,'Morales'
           ,'01/02/1978'
           ,null
           ,'UsuarioDe@Prueba.com'
           ,'UsuarioDePrueba1'
           ,null
           ,1
           ,getdate()
           ,1
           ,'')
GO
INSERT INTO [dbo].[Usuarios]
           ([Nombre]
           ,[Apellido]
           ,[FechaNacimiento]
           ,[UserName]
           ,[Email]
           ,[Password]
           ,[Foto]
           ,[TipoUsuario]
           ,[FechaCracion]
           ,[Activo]
           ,[Token])
     VALUES
           ('Jorge'
           ,'DePrueba'
           ,'01/02/1978'
           ,null
           ,'UsuarioDe2@Prueba.com'
           ,'UsuarioDePrueba1'
           ,null
           ,2
           ,getdate()
           ,1
           ,'')
GO
alter table DonacionesInsumos
add FechaCreacion datetime;
GO
alter table DonacionesHorasTrabajo
add FechaCreacion datetime;
GO
INSERT INTO [dbo].[Propuestas]([Nombre],[Descripcion],[FechaCreacion],[FechaFin],[TelefonoContacto],[TipoDonacion],[Foto],[IdUsuarioCreador],[Estado],[Valoracion])
     VALUES
           ('Juntada para el merendero'
			,'Se necesitan alimentos no perecederos'
			,'2020-11-28 00:00:00.000'
			,'2030-10-20 00:00:00.000'
			,'1149408340'
			,2           
			,'merendero-rayito-de-sol-63egns5ju1u0.jpg'
			,2
			,0
			,50
			)
GO
INSERT INTO [dbo].[PropuestasDonacionesInsumos]([IdPropuesta],[Nombre],[Cantidad])
VALUES (1,'Fideo',20)
GO
INSERT INTO [dbo].[PropuestasDonacionesInsumos]([IdPropuesta],[Nombre],[Cantidad])
VALUES (1,'Arroz',50)
GO
INSERT INTO [dbo].[PropuestasDonacionesInsumos]([IdPropuesta],[Nombre],[Cantidad])
VALUES (1,'Aceite',10)
GO
INSERT INTO [dbo].[Propuestas]([Nombre],[Descripcion],[FechaCreacion],[FechaFin],[TelefonoContacto],[TipoDonacion],[Foto],[IdUsuarioCreador],[Estado],[Valoracion])
     VALUES
           ('Comida comunitaria'
			,'Se realizara merienda y sena para personas en situacion de calle'
			,'2020-11-28 00:00:00.000'
			,'2030-10-20 00:00:00.000'
			,'1149408340'
			,2           
			,'p3.jpg'
			,2
			,0
			,30
			)
GO
INSERT INTO [dbo].[PropuestasDonacionesInsumos]([IdPropuesta],[Nombre],[Cantidad])
VALUES (2,'Verduras',110)
GO
INSERT INTO [dbo].[PropuestasDonacionesInsumos]([IdPropuesta],[Nombre],[Cantidad])
VALUES (2,'Mermeladas',50)
GO
INSERT INTO [dbo].[PropuestasDonacionesInsumos]([IdPropuesta],[Nombre],[Cantidad])
VALUES (2,'Vinagre',2)
GO
INSERT INTO [dbo].[Propuestas]([Nombre],[Descripcion],[FechaCreacion],[FechaFin],[TelefonoContacto],[TipoDonacion],[Foto],[IdUsuarioCreador],[Estado],[Valoracion])
     VALUES
           ('Ayuda monetaria'
			,'Se pretende llegar a una cifra monetaria para poder ayudar a gente de la calle '
			,'2020-11-28 00:00:00.000'
			,'2030-10-20 00:00:00.000'
			,'1149408340'
			,1           
			,'p2.png'
			,2
			,0
			,60
			)
GO
INSERT INTO [dbo].[PropuestasDonacionesMonetarias]([IdPropuesta],[Dinero],[CBU])
     VALUES (3,150000,25325984865236)
GO
INSERT INTO [dbo].[Propuestas]([Nombre],[Descripcion],[FechaCreacion],[FechaFin],[TelefonoContacto],[TipoDonacion],[Foto],[IdUsuarioCreador],[Estado],[Valoracion])
     VALUES
           ('Arreglo de sistema de agua'
			,'Pretendemos juntar dinero para poder arreglar el sistema de agua'
			,'2020-11-27 00:00:00.000'
			,'2020-11-27 00:00:00.000'
			,'1149408340'
			,1           
			,'p1.jpg'
			,1
			,0
			,80
			)
GO
INSERT INTO [dbo].[PropuestasDonacionesMonetarias]([IdPropuesta],[Dinero],[CBU])
     VALUES (4,50000,25325984865236)

GO
INSERT INTO [dbo].[Propuestas]([Nombre],[Descripcion],[FechaCreacion],[FechaFin],[TelefonoContacto],[TipoDonacion],[Foto],[IdUsuarioCreador],[Estado],[Valoracion])
     VALUES
           ('Clases solidarias'
			,'Organizamos clases para chicos de entre 13 y 17 años'
			,'2020-11-28 00:00:00.000'
			,'2030-10-20 00:00:00.000'
			,'1149408340'
			,3           
			,'images.jpg'
			,1
			,0
			,20
			)
GO
INSERT INTO [dbo].[PropuestasDonacionesHorasTrabajo]([IdPropuesta],[CantidadHoras],[Profesion])
VALUES (5,80,'Profesor de Matematica')
GO
INSERT INTO [dbo].[Propuestas]([Nombre],[Descripcion],[FechaCreacion],[FechaFin],[TelefonoContacto],[TipoDonacion],[Foto],[IdUsuarioCreador],[Estado],[Valoracion])
     VALUES
           ('Clases solidarias a Primaria'
			,'Organizamos clases para chicos de entre 7 y 10 años'
			,'2020-11-27 00:00:00.000'
			,'2020-11-27 00:00:00.000'
			,'1149408340'
			,3           
			,'images (1).jpg'
			,1
			,0
			,20
			)
GO
INSERT INTO [dbo].[PropuestasDonacionesHorasTrabajo]([IdPropuesta],[CantidadHoras],[Profesion])
VALUES (6,50,'Profesor de Primaria')
GO
INSERT INTO [dbo].[Denuncias]([IdPropuesta],[IdMotivo],[Comentarios],[IdUsuario],[FechaCreacion],[Estado])
VALUES (4,1,'Contenido inapropiado',2,'2021-10-20 00:00:00.000',1)
GO
INSERT INTO [dbo].[Denuncias]([IdPropuesta],[IdMotivo],[Comentarios],[IdUsuario],[FechaCreacion],[Estado])
VALUES (4,2,'No me parece que tengan que hacer una publicacion por esto',2,'2021-10-20 00:00:00.000',1)
GO
INSERT INTO [dbo].[Denuncias]([IdPropuesta],[IdMotivo],[Comentarios],[IdUsuario],[FechaCreacion],[Estado])
VALUES (4,3,'Estas personas son estafadores',2,'2021-10-20 00:00:00.000',1)
GO
INSERT INTO [dbo].[Denuncias]([IdPropuesta],[IdMotivo],[Comentarios],[IdUsuario],[FechaCreacion],[Estado])
VALUES (4,4,'Mensaje de prueba',2,'2021-10-20 00:00:00.000',1)
GO
INSERT INTO [dbo].[Denuncias]([IdPropuesta],[IdMotivo],[Comentarios],[IdUsuario],[FechaCreacion],[Estado])
VALUES (4,2,'Una vez que acepte alguna denuncia se eliminara del inicio',2,'2021-10-20 00:00:00.000',1)
GO
INSERT INTO [dbo].[Denuncias]([IdPropuesta],[IdMotivo],[Comentarios],[IdUsuario],[FechaCreacion],[Estado])
VALUES (4,3,'Mensaje para rellenar',2,'2021-10-20 00:00:00.000',1)
GO
INSERT INTO [dbo].[DonacionesMonetarias]([IdPropuestaDonacionMonetaria],[IdUsuario],[Dinero],[ArchivoTransferencia],[FechaCreacion])
VALUES (1,1,3000,'215489563254871','2021-10-20 00:00:00.000')
GO
INSERT INTO [dbo].[DonacionesMonetarias]([IdPropuestaDonacionMonetaria],[IdUsuario],[Dinero],[ArchivoTransferencia],[FechaCreacion])
VALUES (2,1,2000,'Mensaje para rellenar','2021-10-20 00:00:00.000')
GO
INSERT INTO [dbo].[DonacionesMonetarias]([IdPropuestaDonacionMonetaria],[IdUsuario],[Dinero],[ArchivoTransferencia],[FechaCreacion])
VALUES (1,1,3000,'215489563254871','2021-10-20 00:00:00.000')
GO
INSERT INTO [dbo].[DonacionesMonetarias]([IdPropuestaDonacionMonetaria],[IdUsuario],[Dinero],[ArchivoTransferencia],[FechaCreacion])
VALUES (2,1,2000,'Mensaje para rellenar','2021-10-20 00:00:00.000')
GO