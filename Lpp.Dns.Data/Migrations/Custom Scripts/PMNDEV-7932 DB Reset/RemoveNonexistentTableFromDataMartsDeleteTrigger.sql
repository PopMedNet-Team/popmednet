-- This is used to alter a trigger for DataMart that tries to delete a table that no longer exists.
USE [QATrunk]
GO

/****** Object:  Trigger [dbo].[DataMartsDelete]    Script Date: 9/1/2023 12:51:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[DataMartsDelete] 
		ON  [dbo].[DataMarts]
		AFTER DELETE
	AS 
	BEGIN
		DELETE FROM RequestDataMartResponseSearchResults WHERE ItemID IN (SELECT ID FROM deleted)
	END
GO


