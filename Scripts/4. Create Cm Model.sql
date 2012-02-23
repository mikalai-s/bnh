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
 CONSTRAINT [PK_Wall] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [cm].[Brick](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[WallId] [bigint] NOT NULL,
	[Type] [tinyint] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Width] [real] not null default 100,
	[Order] [tinyint] not null default 0,
	[Html] [ntext] NULL,
	[ImageListId] [bigint] NULL,
	[GpsLocation] [nvarchar](100) NULL,
	
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


