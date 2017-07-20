/**

This script deletes the WorkplanTypes, and Activities added by PMNDEV-3835.
REMEMBER TO SET THE PROJECT ID VARIABLE.

**/

BEGIN TRANSACTION

	/** delete workplans **/
	DECLARE @NetworkID uniqueidentifier = (SELECT TOP 1 ID FROM Networks)
	UPDATE Requests SET WorkplanTypeID = NULL WHERE EXISTS(SELECT NULL FROM WorkplanTypes WHERE [Name] = 'Initial Basic Query' AND ID = Requests.WorkplanTypeID AND NetworkID = @NetworkID)
	DELETE FROM WorkplanTypes WHERE [Name] = 'Initial Basic Query' AND NetworkID = @NetworkID
	UPDATE Requests SET WorkplanTypeID = NULL WHERE EXISTS(SELECT NULL FROM WorkplanTypes WHERE [Name] = 'Quality Assurance' AND ID = Requests.WorkplanTypeID AND NetworkID = @NetworkID)
	DELETE FROM WorkplanTypes WHERE [Name] = 'Quality Assurance' AND NetworkID = @NetworkID
	UPDATE Requests SET WorkplanTypeID = NULL WHERE EXISTS(SELECT NULL FROM WorkplanTypes WHERE [Name] = 'Quality Assurance Request' AND ID = Requests.WorkplanTypeID AND NetworkID = @NetworkID)
	DELETE FROM WorkplanTypes WHERE [Name] = 'Quality Assurance Request' AND NetworkID = @NetworkID

	/** delete activities **/
	DECLARE @ProjectID uniqueidentifier = '{PROJECT ID}'

	DECLARE @TO_ID uniqueidentifier = (SELECT TOP 1 ID FROM Activities WHERE ParentActivityID IS NULL AND TaskLevel = 1 AND [Name] = 'P01')
	IF @TO_ID IS NOT NULL
	BEGIN
		DELETE FROM Activities WHERE TaskLevel = 3 AND EXISTS(SELECT NULL FROM Activities a WHERE a.TaskLevel = 2 AND a.ParentActivityID = @TO_ID AND Activities.ParentActivityID = a.ID)
		DELETE FROM Activities WHERE ParentActivityID = @TO_ID
		DELETE FROM Activities WHERE ID = @TO_ID
	END

	SET @TO_ID = (SELECT TOP 1 ID FROM Activities WHERE ParentActivityID IS NULL AND TaskLevel = 1 AND [Name] = 'P02')
	IF @TO_ID IS NOT NULL
	BEGIN
		DELETE FROM Activities WHERE TaskLevel = 3 AND EXISTS(SELECT NULL FROM Activities a WHERE a.TaskLevel = 2 AND a.ParentActivityID = @TO_ID AND Activities.ParentActivityID = a.ID)
		DELETE FROM Activities WHERE ParentActivityID = @TO_ID
		DELETE FROM Activities WHERE ID = @TO_ID
	END

	SET @TO_ID = (SELECT TOP 1 ID FROM Activities WHERE ParentActivityID IS NULL AND TaskLevel = 1 AND [Name] = 'P03')
	IF @TO_ID IS NOT NULL
	BEGIN
		DELETE FROM Activities WHERE TaskLevel = 3 AND EXISTS(SELECT NULL FROM Activities a WHERE a.TaskLevel = 2 AND a.ParentActivityID = @TO_ID AND Activities.ParentActivityID = a.ID)
		DELETE FROM Activities WHERE ParentActivityID = @TO_ID
		DELETE FROM Activities WHERE ID = @TO_ID
	END

	SET @TO_ID = (SELECT TOP 1 ID FROM Activities WHERE ParentActivityID IS NULL AND TaskLevel = 1 AND [Name] = 'P04')
	IF @TO_ID IS NOT NULL
	BEGIN
		DELETE FROM Activities WHERE TaskLevel = 3 AND EXISTS(SELECT NULL FROM Activities a WHERE a.TaskLevel = 2 AND a.ParentActivityID = @TO_ID AND Activities.ParentActivityID = a.ID)
		DELETE FROM Activities WHERE ParentActivityID = @TO_ID
		DELETE FROM Activities WHERE ID = @TO_ID
	END

COMMIT TRANSACTION