/*'	=====================================================================
=========================================================================

	<DASHBOARD_MESSAGE>

	====================
	DESCRIPTION

	Script de création des procédures stockées du projet DASHBOARD_MESSAGE
	====================

	====================
	LISTE DES ELEMENTS DU FICHIER
	TYPE	[ID_list]
	PS		[MESSAGE_getAll]
	PS		[MESSAGE_getAllCurrent]
	PS		[MESSAGE_getById]
	PS		[MESSAGE_updateOrInsert]
	PS		[CATEGORY_getAll]
	PS		[CATEGORY_insert]
	PS		[ACTION_getAll]
	PS		[ACTION_insert]
	PS		[ICON_getAll]
	PS		[ICON_insert]
	====================

=========================================================================
====================================================================== */

/*'	---------------------------------------------------------------------
'*	BASE			: [MINT_DASHBOARD_BUILDER]
'*	TYPE			: TABLE(int)
'*	AUTHOR			: RAPHAEL
'*	COMMENT			: Liste de catégories de Messages
'* ------------------------------------------------------------------ */

USE [MINT_DASHBOARD_BUILDER]
GO

IF NOT EXISTS
(
	SELECT *
	FROM sys.types
	WHERE is_table_type = 1
	AND name = 'ID_list'
)
CREATE TYPE [ID_list]
AS TABLE ([ID] int NOT NULL);
GO

/*'	---------------------------------------------------------------------
'*	BASE			: [MINT_DASHBOARD_BUILDER]
'*	TABLE			: [MESSAGE]
'*	PS				: [MESSAGE_getAll]
'*	USED BY			: C# 
'*	AUTHOR			: RAPHAEL
'*	UPDATE			: 
'*	PARAM			: tableau ID Catégories, booléen En Cours, booléen Brouillon
'*	COMMENT			: Récupère tous les message correspondant aux critères
'*	TEST			:	
'*
'*	RETURN			: 
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
	WHERE id = object_id(N'[dbo].[MESSAGE_getAll]')
	AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
DROP PROCEDURE [dbo].[MESSAGE_getAll]
GO

CREATE PROCEDURE [dbo].[MESSAGE_getAll]
-- ================================================================
-- 1.00 / 2021-11-18 / RAPHAEL : Creation
-- ================================================================
	@pCategories AS [ID_list] READONLY,
	@pIsCurrent bit = NULL,
	@pIsDraft bit = NULL
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	DECLARE @today date;
	SET @today = CAST(GETDATE() AS date);

	SELECT DISTINCT
		[MESSAGE].[id] AS [MESSAGE_id],
		[MESSAGE].[is_draft] AS [MESSAGE_is_draft],
		[MESSAGE].[title] AS [MESSAGE_title],
		[MESSAGE].[header] AS [MESSAGE_header],
		[MESSAGE].[body] AS [MESSAGE_body],
		[MESSAGE].[background] AS [MESSAGE_background],
		[MESSAGE].[start_date] AS [MESSAGE_start_date],
		[MESSAGE].[end_date] AS [MESSAGE_end_date],
		[MESSAGE].[icon_id] AS [MESSAGE_icon],
		[MESSAGE].[action_id] AS [MESSAGE_action],
		STUFF((
			SELECT ',' + CAST([MESSAGE_CATEGORY].[category_id] AS varchar)
			FROM [MESSAGE_CATEGORY]
			WHERE [MESSAGE_CATEGORY].[message_id] = [MESSAGE].[id]
			FOR XML PATH('')
        ), 1, 1, '') AS [MESSAGE_categories]
	FROM [MESSAGE]
	WHERE
	(
		NOT EXISTS(SELECT 1 FROM @pCategories)
		OR ([CATEGORY].[id] IN (SELECT [MESSAGE_category_id] FROM @pCategories))
	)
	AND
	(
		@pIsDraft IS NULL
		OR [MESSAGE].[is_draft] = @pIsDraft
	)
	AND
	(
		@pIsCurrent IS NULL
		OR @today BETWEEN [MESSAGE].[start_date] AND [MESSAGE].[end_date]
	)
	ORDER BY
		[MESSAGE_is_draft]

END
GO

/*'	---------------------------------------------------------------------
'*	BASE			: [MINT_DASHBOARD_BUILDER]
'*	TABLE			: [MESSAGE]
'*	PS				: [MESSAGE_getAllCurrent]
'*	USED BY			: C# 
'*	AUTHOR			: RAPHAEL
'*	UPDATE			: 
'*	PARAM			: tableau ID Catégories, booléen En Cours, booléen Brouillon
'*	COMMENT			: Récupère tous les message EN COURS correspondant aux critères
'*	TEST			:	
'*
'*	RETURN			: 
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
	WHERE id = object_id(N'[dbo].[MESSAGE_getAllCurrent]')
	AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
DROP PROCEDURE [dbo].[MESSAGE_getAllCurrent]
GO

CREATE PROCEDURE [dbo].[MESSAGE_getAllCurrent]
-- ================================================================
-- 1.00 / 2021-11-18 / RAPHAEL : Creation
-- ================================================================
	-- @pProfiles AS [ID_list] READONLY
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	DECLARE @today date;
	SET @today = CAST(GETDATE() AS date);

	SELECT DISTINCT
		[MESSAGE].[id] AS [MESSAGE_id],
		[MESSAGE].[is_draft] AS [MESSAGE_is_draft],
		[MESSAGE].[title] AS [MESSAGE_title],
		[MESSAGE].[header] AS [MESSAGE_header],
		[MESSAGE].[body] AS [MESSAGE_body],
		[MESSAGE].[background] AS [MESSAGE_background],
		[MESSAGE].[start_date] AS [MESSAGE_start_date],
		[MESSAGE].[end_date] AS [MESSAGE_end_date],
		[MESSAGE].[icon_id] AS [MESSAGE_icon],
		[MESSAGE].[action_id] AS [MESSAGE_action],
		STUFF((
			SELECT ',' + CAST([MESSAGE_CATEGORY].[category_id] AS varchar)
			FROM [MESSAGE_CATEGORY]
			WHERE [MESSAGE_CATEGORY].[message_id] = [MESSAGE].[id]
			FOR XML PATH('')
        ), 1, 1, '') AS [MESSAGE_categories]
	FROM [MESSAGE]
	WHERE (@today BETWEEN [MESSAGE].[start_date] AND [MESSAGE].[end_date])
	AND ([MESSAGE].[is_draft] = 0)
	ORDER BY [MESSAGE_start_date]

END
GO

/*'	---------------------------------------------------------------------
'*	BASE			: [MINT_DASHBOARD_BUILDER]
'*	TABLE			: [MESSAGE]
'*	PS				: [MESSAGE_getById]
'*	USED BY			: C# 
'*	AUTHOR			: RAPHAEL
'*	UPDATE			: 
'*	PARAM			: ID Message
'*	COMMENT			: Récupère le Message correspondant à l'ID
'*	TEST			:	
'*
'*	RETURN			: 
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
	WHERE id = object_id(N'[dbo].[MESSAGE_getById]')
	AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
DROP PROCEDURE [dbo].[MESSAGE_getById]
GO

CREATE PROCEDURE [dbo].[MESSAGE_getById]
-- =============================================
-- 1.00 / 2021-11-18 / RAPHAEL : Creation
-- =============================================
	@pMessageID int = NULL
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	SELECT
		[MESSAGE].[id] AS [MESSAGE_id],
		[MESSAGE].[is_draft] AS [MESSAGE_is_draft],
		[MESSAGE].[title] AS [MESSAGE_title],
		[MESSAGE].[header] AS [MESSAGE_header],
		[MESSAGE].[body] AS [MESSAGE_body],
		[MESSAGE].[background] AS [MESSAGE_background],
		[MESSAGE].[start_date] AS [MESSAGE_start_date],
		[MESSAGE].[end_date] AS [MESSAGE_end_date],
		[MESSAGE].[icon_id] AS [MESSAGE_icon],
		[MESSAGE].[action_id] AS [MESSAGE_action],
		STUFF((
			SELECT ',' + CAST([MESSAGE_CATEGORY].[category_id] AS varchar)
			FROM [MESSAGE_CATEGORY]
			WHERE [MESSAGE_CATEGORY].[message_id] = [MESSAGE].[id]
			FOR XML PATH('')
        ), 1, 1, '') AS [MESSAGE_categories]
	FROM [MESSAGE]
	WHERE [MESSAGE].[id] = @pMessageID

END
GO

/*'	---------------------------------------------------------------------
'*	BASE			: [MINT_DASHBOARD_BUILDER]
'*	TABLE			: [MESSAGE]
'*	PS				: [MESSAGE_updateOrInsert]
'*	USED BY			: C# 
'*	AUTHOR			: RAPHAEL
'*	UPDATE			: 
'*	PARAM			: ID, Titre, booléen Brouillon, Header, Body, Dates Début & Fin, ID Icone, ID ACtion
'*	COMMENT			: Ajoute un nouveau Message ou le modifie s'il existe déjà
'*	TEST			:	
'*
'*	RETURN			: 0 => OK / -1 => ERREUR
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
	WHERE id = object_id(N'[dbo].[MESSAGE_updateOrInsert]')
	AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
DROP PROCEDURE [dbo].[MESSAGE_updateOrInsert]
GO

CREATE PROCEDURE [dbo].[MESSAGE_updateOrInsert]
-- =============================================
-- 1.00 / 2021-11-18 / RAPHAEL => Creation
-- =============================================
	@pMessageID int = NULL,
	@pMessageTitle nvarchar(30),
	@pMessageIsDraft bit,
	@pMessageHeader nvarchar(200),
	@pMessageBody nvarchar(200),
	@pMessageBackground char(6),
	@pMessageStartDate date,
	@pMessageEndDate date,
	@pMessageIconId int,
	@pMessageActionId int,
	@pMessageCategories AS [ID_list] READONLY
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	IF @pMessageID IS NULL

		BEGIN

			INSERT INTO [MESSAGE]
			(
				[title],
				[is_draft],
				[header],
				[body],
				[background],
				[start_date],
				[end_date],
				[icon_id],
				[action_id]
			)
			VALUES
			(
				@pMessageTitle,
				@pMessageIsDraft,
				@pMessageHeader,
				@pMessageBody,
				@pMessageBackground,
				@pMessageStartDate,
				@pMessageEndDate,
				@pMessageIconId,
				@pMessageActionId
			);

			SET @pMessageID = SCOPE_IDENTITY();

		END;

	ELSE

		UPDATE [MESSAGE]
		SET
			[title] = @pMessageTitle,
			[is_draft] = @pMessageIsDraft,
			[header] = @pMessageHeader,
			[body] = @pMessageBody,
			[background] = @pMessageBackground,
			[start_date] = @pMessageStartDate,
			[end_date] = @pMessageEndDate,
			[icon_id] = @pMessageIconId,
			[action_id] = @pMessageActionId
		WHERE [id] = @pMessageID;

	IF @@ROWCOUNT = 0
		RETURN -1;

	DELETE FROM [MESSAGE_CATEGORY]
	WHERE [message_id] = @pMessageID;

	INSERT INTO [MESSAGE_CATEGORY] ([message_id], [category_id])
	SELECT @pMessageID, [category_id] FROM @pMessageCategories;

	SELECT
		[MESSAGE].[id] AS [MESSAGE_id],
        [MESSAGE].[is_draft] AS [MESSAGE_is_draft],
		[MESSAGE].[title] AS [MESSAGE_title],
		[MESSAGE].[header] AS [MESSAGE_header],
		[MESSAGE].[body] AS [MESSAGE_body],
		[MESSAGE].[background] AS [MESSAGE_background],
		[MESSAGE].[start_date] AS [MESSAGE_start_date],
		[MESSAGE].[end_date] AS [MESSAGE_end_date],
		[MESSAGE].[icon_id] AS [MESSAGE_icon],
		[MESSAGE].[action_id] AS [MESSAGE_action],
		STUFF((
			SELECT ',' + CAST([MESSAGE_CATEGORY].[category_id] AS varchar)
			FROM [MESSAGE_CATEGORY]
			WHERE [MESSAGE_CATEGORY].[message_id] = [MESSAGE].[id]
			FOR XML PATH('')
        ), 1, 1, '') AS [MESSAGE_categories]
	FROM [MESSAGE]
	WHERE [MESSAGE].[id] = @pMessageID;

	RETURN 0;

END
GO

/*'	---------------------------------------------------------------------
'*	BASE			: [MINT_DASHBOARD_BUILDER]
'*	TABLE			: [CATEGORY]
'*	PS				: [CATEGORY_getAll]
'*	USED BY			: C# 
'*	AUTHOR			: RAPHAEL
'*	UPDATE			: 
'*	PARAM			: 
'*	COMMENT			: Récupérer liste des catégories
'*	TEST			:	
'*
'*	RETURN			: 
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
	WHERE id = object_id(N'[dbo].[CATEGORY_getAll]')
	AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
DROP PROCEDURE [dbo].[CATEGORY_getAll]
GO

CREATE PROCEDURE [dbo].[CATEGORY_getAll]
-- =============================================
-- 1.00 / 2021-11-18 / RAPHAEL => Creation
-- =============================================
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	SELECT DISTINCT
		[CATEGORY].[id] AS [CATEGORY_id],
		[CATEGORY].[title] AS [CATEGORY_title]
	FROM [CATEGORY]

END
GO

/*'	---------------------------------------------------------------------
'*	BASE			: [MINT_DASHBOARD_BUILDER]
'*	TABLE			: [CATEGORY]
'*	PS				: [CATEGORY_insert]
'*	USED BY			: C# 
'*	AUTHOR			: RAPHAEL
'*	UPDATE			: 
'*	PARAM			: Titre
'*	COMMENT			: Ajout nouvelle Catégorie
'*	TEST			:	
'*
'*	RETURN			: 0 => OK / -1 => ERREUR
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
	WHERE id = object_id(N'[dbo].[CATEGORY_insert]')
	AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
DROP PROCEDURE [dbo].[CATEGORY_insert]
GO

CREATE PROCEDURE [dbo].[CATEGORY_insert]
-- =============================================
-- 1.00 / 2021-11-18 / RAPHAEL => Creation
-- =============================================
	@pCategoryTitle nvarchar(50) = NULL
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	IF (@pCategoryTitle IS NULL)
    OR (
        EXISTS(
            SELECT 1
            FROM [CATEGORY]
            WHERE [title]
            COLLATE Latin1_General_CS_AS = @pCategoryTitle
        )
    )
        RETURN -1

    ELSE

		BEGIN

			INSERT INTO [CATEGORY] ([title])
			VALUES (@pCategoryTitle);

            IF @@ROWCOUNT = 0
		        RETURN -1;

            SELECT
                [CATEGORY].[id] AS [CATEGORY_id],
                [CATEGORY].[title] AS [CATEGORY_title]
            FROM [CATEGORY]
            WHERE [CATEGORY].[id] = SCOPE_IDENTITY();

            RETURN 0;

		END;

END
GO

/*'	---------------------------------------------------------------------
'*	BASE			: [MINT_DASHBOARD_BUILDER]
'*	TABLE			: [ACTION]
'*	PS				: [ACTION_getAll]
'*	USED BY			: C# 
'*	AUTHOR			: RAPHAEL
'*	UPDATE			: 
'*	PARAM			: 
'*	COMMENT			: Récupérer liste des actions
'*	TEST			:	
'*
'*	RETURN			: 
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
	WHERE id = object_id(N'[dbo].[ACTION_getAll]')
	AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
DROP PROCEDURE [dbo].[ACTION_getAll]
GO

CREATE PROCEDURE [dbo].[ACTION_getAll]
-- =============================================
-- 1.00 / 2021-11-18 / RAPHAEL => Creation
-- =============================================
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	SELECT DISTINCT
		[ACTION].[id] AS [ACTION_id],
		[ACTION].[title] AS [ACTION_title],
		[ACTION].[link] AS [ACTION_link]
	FROM [ACTION];

END
GO

/*'	---------------------------------------------------------------------
'*	BASE			: [MINT_DASHBOARD_BUILDER]
'*	TABLE			: [ACTION]
'*	PS				: [ACTION_insert]
'*	USED BY			: C# 
'*	AUTHOR			: RAPHAEL
'*	UPDATE			: 
'*	PARAM			: Titre, Lien
'*	COMMENT			: Ajout nouvelle Action
'*	TEST			:	
'*
'*	RETURN			: 0 => OK / -1 => ERREUR
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
	WHERE id = object_id(N'[dbo].[ACTION_insert]')
	AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
DROP PROCEDURE [dbo].[ACTION_insert]
GO

CREATE PROCEDURE [dbo].[ACTION_insert]
-- =============================================
-- 1.00 / 2021-11-18 / RAPHAEL => Creation
-- =============================================
	@pActionTitle nvarchar(50) = NULL,
	@pActionLink nvarchar(MAX) = NULL
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	IF (@pActionTitle IS NULL)
    OR (@pActionLink IS NULL)
    OR (
        EXISTS(
            SELECT 1
            FROM [ACTION]
            WHERE [title]
            COLLATE Latin1_General_CS_AS = @pActionTitle
        )
    )
        RETURN -1;

    ELSE

		BEGIN

			INSERT INTO [ACTION]
			(
				[title],
                [link]
			)
			VALUES
			(
				@pActionTitle,
				@pActionLink
			);

            IF @@ROWCOUNT = 0
		        RETURN -1;

            SELECT
                [ACTION].[id] AS [ACTION_id],
                [ACTION].[title] AS [ACTION_title],
                [ACTION].[link] AS [ACTION_link]
            FROM [ACTION]
            WHERE [ACTION].[id] = SCOPE_IDENTITY();

            RETURN 0;

		END;

END
GO
/*'	---------------------------------------------------------------------
'*	BASE			: [MINT_DASHBOARD_BUILDER]
'*	TABLE			: [ICON]
'*	PS				: [ICON_getAll]
'*	USED BY			: C# 
'*	AUTHOR			: RAPHAEL
'*	UPDATE			: 
'*	PARAM			: 
'*	COMMENT			: Récupérer liste des icônes
'*	TEST			:	
'*
'*	RETURN			: 
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
	WHERE id = object_id(N'[dbo].[ICON_getAll]')
	AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
DROP PROCEDURE [dbo].[ICON_getAll]
GO

CREATE PROCEDURE [dbo].[ICON_getAll]
-- =============================================
-- 1.00 / 2021-11-23 / RAPHAEL => Creation
-- =============================================
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	SELECT DISTINCT
		[ICON].[id] AS [ICON_id],
		[ICON].[data] AS [ICON_data]
	FROM [ICON];

END
GO

/*'	---------------------------------------------------------------------
'*	BASE			: [MINT_DASHBOARD_BUILDER]
'*	TABLE			: [ICON]
'*	PS				: [ICON_insert]
'*	USED BY			: C# 
'*	AUTHOR			: RAPHAEL
'*	UPDATE			: 
'*	PARAM			: Fichier (binaire)
'*	COMMENT			: Ajout nouvelle Icône
'*	TEST			:	
'*
'*	RETURN			: 0 => OK / -1 => ERREUR
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
	WHERE id = object_id(N'[dbo].[ICON_insert]')
	AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
DROP PROCEDURE [dbo].[ICON_insert]
GO

CREATE PROCEDURE [dbo].[ICON_insert]
-- =============================================
-- 1.00 / 2021-11-23 / RAPHAEL => Creation
-- =============================================
	@pIconData varbinary(MAX) = NULL
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	IF (@pIconData IS NULL)

        RETURN -1;

    ELSE

		BEGIN

			INSERT INTO [ICON] ([data])
			VALUES (@pIconData);

            IF @@ROWCOUNT = 0
		        RETURN -1;

			SELECT
				[ICON].[id] AS [ICON_id],
				[ICON].[data] AS [ICON_data]
			FROM [ICON]
            WHERE [ICON].[id] = SCOPE_IDENTITY();

            RETURN 0;

		END;

END
GO