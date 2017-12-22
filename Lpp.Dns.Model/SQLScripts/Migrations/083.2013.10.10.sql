IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE [Name] = N'RejectReason' AND Object_ID = Object_ID(N'Users'))    
BEGIN
    ALTER TABLE Users ADD RejectReason nvarchar(500) NULL 
END
GO


/****** Object:  View [dbo].[vwUsers]    Script Date: 10/14/2013 18:20:36 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwUsers]'))
DROP VIEW [dbo].[vwUsers]
GO

/****** Object:  View [dbo].[vwUsers]    Script Date: 10/14/2013 18:20:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE view [dbo].[vwUsers] as
	select u.*, cast( case when isnull(u.isdeleted,0) = 1 or isnull(o.isdeleted,0) = 1 then 1 else 0 end as bit ) as EffectiveIsDeleted
	from users u inner join organizations o on u.organizationid = o.organizationid

GO


IF NOT EXISTS(SELECT NULL FROM MigrationScript WHERE ScriptRun = '083.2013.10.10')
BEGIN
	INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '083.2013.10.10')
END
GO

