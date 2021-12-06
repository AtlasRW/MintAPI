/*'	=====================================================================
=========================================================================

	<MINT_DASHBOARD_BUILDER>

	====================
	DESCRIPTION

	Script de création des tables du projet
	====================

	====================
	LISTE DES ELEMENTS DU FICHIER

	TABLE	[MESSAGE]
	TABLE	[CATEGORY]
	TABLE	[ACTION]
	TABLE	[ICON]
	TABLE	[MESSAGE_CATEGORY]
	====================
			
=========================================================================
====================================================================== */

/*'	---------------------------------------------------------------------
'*	TABLE	[CATEGORY]
'*	AUTHOR	RAPHAEL
'* ------------------------------------------------------------------ */

USE [MINT_DASHBOARD_BUILDER]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS
(
	SELECT * FROM dbo.sysobjects
	WHERE id = object_id(N'[dbo].[CATEGORY]')
	AND OBJECTPROPERTY(id, N'IsUserTable') = 1
)
DROP TABLE [dbo].[CATEGORY]
GO

CREATE TABLE [dbo].[CATEGORY]
-- =============================================
-- 1.00 / 2021-11-18 / RAPHAEL => Creation
-- =============================================
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](50) NOT NULL,
	CONSTRAINT [PK_CATEGORY]
	PRIMARY KEY CLUSTERED ([id] ASC)
	WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
) ON [PRIMARY]
GO

/*'	---------------------------------------------------------------------
'*	TABLE	[ICON]
'*	AUTHOR	RAPHAEL
'* ------------------------------------------------------------------ */

USE [MINT_DASHBOARD_BUILDER]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS
(
	SELECT * FROM dbo.sysobjects
	WHERE id = object_id(N'[dbo].[ICON]')
	AND OBJECTPROPERTY(id, N'IsUserTable') = 1
)
DROP TABLE [dbo].[ICON]
GO

CREATE TABLE [dbo].[ICON]
-- =============================================
-- 1.00 / 2021-11-18 / RAPHAEL => Creation
-- =============================================
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[data] [varbinary](MAX) NOT NULL,
	CONSTRAINT [PK_ICON]
	PRIMARY KEY CLUSTERED ([id] ASC)
	WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

/*'	---------------------------------------------------------------------
'*	TABLE	[ACTION]
'*	AUTHOR	RAPHAEL
'* ------------------------------------------------------------------ */

USE [MINT_DASHBOARD_BUILDER]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS
(
	SELECT * FROM dbo.sysobjects
	WHERE id = object_id(N'[dbo].[ACTION]')
	AND OBJECTPROPERTY(id, N'IsUserTable') = 1
)
DROP TABLE [dbo].[ACTION]
GO

CREATE TABLE [dbo].[ACTION]
-- =============================================
-- 1.00 / 2021-11-18 / RAPHAEL => Creation
-- =============================================
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](50) NOT NULL,
	[link] [nvarchar](MAX) NOT NULL,
	CONSTRAINT [PK_ACTION]
	PRIMARY KEY CLUSTERED ([id] ASC)
	WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

/*'	---------------------------------------------------------------------
'*	TABLE	[MESSAGE]
'*	AUTHOR	RAPHAEL
'* ------------------------------------------------------------------ */

USE [MINT_DASHBOARD_BUILDER]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS
(
	SELECT * FROM dbo.sysobjects
	WHERE id = object_id(N'[dbo].[MESSAGE]')
	AND OBJECTPROPERTY(id, N'IsUserTable') = 1
)
DROP TABLE [dbo].[MESSAGE]
GO

CREATE TABLE [dbo].[MESSAGE]
-- =============================================
-- 1.00 / 2021-11-18 / RAPHAEL => Creation
-- =============================================
(
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](30) NOT NULL,
	[is_draft] [bit] NOT NULL,
	[header] [nvarchar](200) NULL,
	[body] [nvarchar](200) NULL,
	[background] [nchar](6) NULL,
	[start_date] [date] NULL,
	[end_date] [date] NULL,
	[icon_id] [int] NULL,
	[action_id] [int] NULL
	CONSTRAINT [PK_MESSAGE]
	PRIMARY KEY CLUSTERED ([id] ASC)
	WITH
	(
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MESSAGE]
ADD CONSTRAINT [DF_Table_1_to_display]
DEFAULT ((1)) FOR [is_draft]
GO

ALTER TABLE [dbo].[MESSAGE]
WITH NOCHECK
ADD CONSTRAINT [FK_MESSAGE_ACTION]
FOREIGN KEY([action_id])
REFERENCES [dbo].[ACTION] ([id])
GO

ALTER TABLE [dbo].[MESSAGE]
CHECK CONSTRAINT [FK_MESSAGE_ACTION]
GO

ALTER TABLE [dbo].[MESSAGE]
WITH NOCHECK
ADD CONSTRAINT [FK_MESSAGE_ICON]
FOREIGN KEY([icon_id])
REFERENCES [dbo].[ICON] ([id])
GO

ALTER TABLE [dbo].[MESSAGE]
CHECK CONSTRAINT [FK_MESSAGE_ICON]
GO

/*'	---------------------------------------------------------------------
'*	TABLE	[MESSAGE_CATEGORY]
'*	AUTHOR	RAPHAEL
'* ------------------------------------------------------------------ */

USE [MINT_DASHBOARD_BUILDER]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS
(
	SELECT * FROM dbo.sysobjects
	WHERE id = object_id(N'[dbo].[MESSAGE_CATEGORY]')
	AND OBJECTPROPERTY(id, N'IsUserTable') = 1
)
DROP TABLE [dbo].[MESSAGE_CATEGORY]
GO

CREATE TABLE [dbo].[MESSAGE_CATEGORY]
-- =============================================
-- 1.00 / 2021-11-18 / RAPHAEL => Creation
-- =============================================
(
	[message_id] [int] NOT NULL,
	[category_id] [int] NOT NULL,
	CONSTRAINT [PK_MESSAGE_CATEGORY]
	PRIMARY KEY CLUSTERED 
	(
		[message_id] ASC,
		[category_id] ASC
	)
	WITH
	(
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON
	) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MESSAGE_CATEGORY]
WITH CHECK
ADD CONSTRAINT [FK_MESSAGE_CATEGORY_MESSAGE]
FOREIGN KEY([message_id])
REFERENCES [dbo].[MESSAGE] ([id])
GO

ALTER TABLE [dbo].[MESSAGE_CATEGORY]
CHECK CONSTRAINT [FK_MESSAGE_CATEGORY_MESSAGE]
GO

ALTER TABLE [dbo].[MESSAGE_CATEGORY]
WITH CHECK
ADD CONSTRAINT [FK_MESSAGE_CATEGORY_CATEGORY]
FOREIGN KEY([category_id])
REFERENCES [dbo].[CATEGORY] ([id])
GO

ALTER TABLE [dbo].[MESSAGE_CATEGORY]
CHECK CONSTRAINT [FK_MESSAGE_CATEGORY_CATEGORY]
GO