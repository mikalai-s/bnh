USE [bnh]
GO

/****** Object:  Table [cm].[Wall]    Script Date: 02/21/2012 22:18:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [cm].[Wall](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Width] [real] NOT NULL,
	[Order] [tinyint] NOT NULL,
 CONSTRAINT [PK_Wall] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [cm].[Wall]  WITH NOCHECK ADD  CONSTRAINT [FK_Wall_Builder] FOREIGN KEY([OwnerId])
REFERENCES [bl].[Builder] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [cm].[Wall] NOCHECK CONSTRAINT [FK_Wall_Builder]
GO

ALTER TABLE [cm].[Wall]  WITH NOCHECK ADD  CONSTRAINT [FK_Wall_Community] FOREIGN KEY([OwnerId])
REFERENCES [bl].[Community] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [cm].[Wall] NOCHECK CONSTRAINT [FK_Wall_Community]
GO

ALTER TABLE [cm].[Wall] ADD  CONSTRAINT [DF_Wall_Width]  DEFAULT ((100)) FOR [Width]
GO

ALTER TABLE [cm].[Wall] ADD  CONSTRAINT [DF_Wall_Order]  DEFAULT ((0)) FOR [Order]
GO

CREATE TABLE [cm].[Brick](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[WallId] [bigint] NOT NULL,
	[Type] [tinyint] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Width] [real] NOT NULL,
	[Order] [tinyint] NOT NULL,
	[Html] [ntext] NULL,
	[ImageListId] [bigint] NULL,
	[GpsLocation] [nvarchar](100) NULL,
	[MapHeight] [int] NULL,
	[MapZoom] [int] NULL,
 CONSTRAINT [PK_Brick] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [cm].[Brick]  WITH CHECK ADD  CONSTRAINT [FK_Brick_Wall] FOREIGN KEY([WallId])
REFERENCES [cm].[Wall] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [cm].[Brick] CHECK CONSTRAINT [FK_Brick_Wall]
GO

ALTER TABLE [cm].[Brick] ADD  CONSTRAINT [DF_Brick_Type]  DEFAULT ((0)) FOR [Type]
GO

ALTER TABLE [cm].[Brick] ADD  DEFAULT ((100)) FOR [Width]
GO

ALTER TABLE [cm].[Brick] ADD  DEFAULT ((0)) FOR [Order]
GO

ALTER TABLE [cm].[Brick] ADD  CONSTRAINT [DF_Brick_MapHeight]  DEFAULT ((0)) FOR [MapHeight]
GO

ALTER TABLE [cm].[Brick] ADD  CONSTRAINT [DF_Brick_MapZoom]  DEFAULT ((0)) FOR [MapZoom]
GO


