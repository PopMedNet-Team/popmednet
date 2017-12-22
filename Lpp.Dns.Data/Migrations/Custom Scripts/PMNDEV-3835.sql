/**

This script adds new WorkplanTypes, and Activities specified by PMNDEV-3835.
REMEMBER TO SET THE PROJECT ID VARIABLE.

**/

BEGIN TRANSACTION

	DECLARE @NetworkID uniqueidentifier = (SELECT TOP 1 ID FROM Networks)

	IF NOT EXISTS(SELECT NULL FROM WorkplanTypes WHERE [Name] = 'Initial Basic Query')
		INSERT INTO WorkplanTypes (ID, WorkplanTypeID, [Name], Acronym, NetworkID) VALUES (dbo.NewSqlGuid(), (SELECT ISNULL(MAX(WorkplanTypeID), 0) FROM WorkplanTypes) + 1, 'Initial Basic Query', 'IBQ', @NetworkID)
	IF NOT EXISTS(SELECT NULL FROM WorkplanTypes WHERE [Name] = 'Quality Assurance')
		INSERT INTO WorkplanTypes (ID, WorkplanTypeID, [Name], Acronym, NetworkID) VALUES (dbo.NewSqlGuid(), (SELECT ISNULL(MAX(WorkplanTypeID), 0) FROM WorkplanTypes) + 1, 'Quality Assurance', 'QAD', @NetworkID)
	IF NOT EXISTS(SELECT NULL FROM WorkplanTypes WHERE [Name] = 'Quality Assurance Request')
		INSERT INTO WorkplanTypes (ID, WorkplanTypeID, [Name], Acronym, NetworkID) VALUES (dbo.NewSqlGuid(), (SELECT ISNULL(MAX(WorkplanTypeID), 0) FROM WorkplanTypes) + 1, 'Quality Assurance Request', 'QAR', @NetworkID)

	DECLARE @ProjectID uniqueidentifier = '{PROJECT ID}'

	/** Add task order P01 **/
	DECLARE @TO_ID uniqueidentifier = (SELECT TOP 1 ID FROM Activities WHERE ParentActivityID IS NULL AND TaskLevel = 1 AND [Name] = 'P01' AND ProjectId = @ProjectID)
	IF @TO_ID IS NULL
	BEGIN
		SET @TO_ID = dbo.NewSqlGuid()
		INSERT INTO Activities (ID, ProjectId, [Name], [Description], TaskLevel, DisplayOrder) VALUES (@TO_ID, @ProjectID, 'P01', '', 1, 0)
	END

	/** P01 Activities **/
	DECLARE @ActivityID uniqueidentifier = (SELECT TOP 1 ID FROM Activities WHERE ParentActivityID = @TO_ID AND TaskLevel = 2 AND [Name] = 'Prepare environment for the PCORnet Distributed Query Tool and Test Functionality')
	IF @ActivityID IS NULL
	BEGIN
		SET @ActivityID = dbo.NewSqlGuid()
		INSERT INTO Activities (ProjectId, ParentActivityID, ID, [Name], [Description], Acronym, TaskLevel, DisplayOrder) VALUES (@ProjectID, @TO_ID, @ActivityID, 'Prepare environment for the PCORnet Distributed Query Tool and Test Functionality', '', 'PREP', 2, 0)
	END

	SET @ActivityID = (SELECT TOP 1 ID FROM Activities WHERE ParentActivityID = @TO_ID AND TaskLevel = 2 AND [Name] = 'Establish Capability to Respond to PCORnet Queries and Support PCORnet CC Data QA Activities')
	IF @ActivityID IS NULL
	BEGIN
		SET @ActivityID = dbo.NewSqlGuid()
		INSERT INTO Activities (ProjectId, ParentActivityID, ID, [Name], [Description], Acronym, TaskLevel, DisplayOrder) VALUES (@ProjectID, @TO_ID, @ActivityID, 'Establish Capability to Respond to PCORnet Queries and Support PCORnet CC Data QA Activities', '', 'CAP', 2, 0)
	END

	SET @ActivityID = (SELECT TOP 1 ID FROM Activities WHERE ParentActivityID = @TO_ID AND TaskLevel = 2 AND [Name] = 'Program Development and Testing')
	IF @ActivityID IS NULL
	BEGIN
		SET @ActivityID = dbo.NewSqlGuid()
		INSERT INTO Activities (ProjectId, ParentActivityID, ID, [Name], [Description], Acronym, TaskLevel, DisplayOrder) VALUES (@ProjectID, @TO_ID, @ActivityID, 'Program Development and Testing', '', 'DEV', 2, 0)
	END

	IF NOT EXISTS(SELECT NULL FROM Activities WHERE ParentActivityID = @ActivityID AND TaskLevel = 3 AND [Name] = 'Obesity Cohort Algorithms')
		INSERT INTO Activities (ProjectId, ParentActivityID, ID, [Name], [Description], Acronym, TaskLevel, DisplayOrder) VALUES (@ProjectID, @ActivityID, dbo.NewSqlGuid(), 'Obesity Cohort Algorithms', '', 'OBALG', 3, 0)

	/** Add task order P02 **/
	SET @TO_ID = (SELECT TOP 1 ID FROM Activities WHERE ParentActivityID IS NULL AND TaskLevel = 1 AND [Name] = 'P02' AND ProjectId = @ProjectID)
	IF @TO_ID IS NULL
	BEGIN
		SET @TO_ID = dbo.NewSqlGuid()
		INSERT INTO Activities (ID, ProjectId, [Name], [Description], TaskLevel, DisplayOrder) VALUES (@TO_ID, @ProjectID, 'P02', '', 1, 0)
	END

	/** P02 Activities **/
	SET @ActivityID = (SELECT TOP 1 ID FROM Activities WHERE ParentActivityID = @TO_ID AND TaskLevel = 2 AND [Name] = 'Prepare environment for the PCORnet Distributed Query Tool and Test Functionality')
	IF @ActivityID IS NULL
	BEGIN
		SET @ActivityID = dbo.NewSqlGuid()
		INSERT INTO Activities (ProjectId, ParentActivityID, ID, [Name], [Description], Acronym, TaskLevel, DisplayOrder) VALUES (@ProjectID, @TO_ID, @ActivityID, 'Prepare environment for the PCORnet Distributed Query Tool and Test Functionality', '', 'PREP', 2, 0)
	END

	SET @ActivityID = (SELECT TOP 1 ID FROM Activities WHERE ParentActivityID = @TO_ID AND TaskLevel = 2 AND [Name] = 'Establish Capability to Respond to PCORnet Queries and Support PCORnet CC Data QA Activities')
	IF @ActivityID IS NULL
	BEGIN
		SET @ActivityID = dbo.NewSqlGuid()
		INSERT INTO Activities (ProjectId, ParentActivityID, ID, [Name], [Description], Acronym, TaskLevel, DisplayOrder) VALUES (@ProjectID, @TO_ID, @ActivityID, 'Establish Capability to Respond to PCORnet Queries and Support PCORnet CC Data QA Activities', '', 'CAP', 2, 0)
	END

	SET @ActivityID = (SELECT TOP 1 ID FROM Activities WHERE ParentActivityID = @TO_ID AND TaskLevel = 2 AND [Name] = 'Program Development and Testing')
	IF @ActivityID IS NULL
	BEGIN
		SET @ActivityID = dbo.NewSqlGuid()
		INSERT INTO Activities (ProjectId, ParentActivityID, ID, [Name], [Description], Acronym, TaskLevel, DisplayOrder) VALUES (@ProjectID, @TO_ID, @ActivityID, 'Program Development and Testing', '', 'DEV', 2, 0)
	END


	/** Add task order P03 **/
	SET @TO_ID = (SELECT TOP 1 ID FROM Activities WHERE ParentActivityID IS NULL AND TaskLevel = 1 AND [Name] = 'P03' AND ProjectId = @ProjectID)
	IF @TO_ID IS NULL
	BEGIN
		SET @TO_ID = dbo.NewSqlGuid()
		INSERT INTO Activities (ID, ProjectId, [Name], [Description], TaskLevel, DisplayOrder) VALUES (@TO_ID, @ProjectID, 'P03', '', 1, 0)
	END

	/** P03 Activities **/
	SET @ActivityID = (SELECT TOP 1 ID FROM Activities WHERE ParentActivityID = @TO_ID AND TaskLevel = 2 AND [Name] = 'Establish Capability to Respond to PCORnet Queries and Support PCORnet CC Data QA Activities')
	IF @ActivityID IS NULL
	BEGIN
		SET @ActivityID = dbo.NewSqlGuid()
		INSERT INTO Activities (ProjectId, ParentActivityID, ID, [Name], [Description], Acronym, TaskLevel, DisplayOrder) VALUES (@ProjectID, @TO_ID, @ActivityID, 'Establish Capability to Respond to PCORnet Queries and Support PCORnet CC Data QA Activities', '', 'CAP', 2, 0)
	END

	SET @ActivityID = (SELECT TOP 1 ID FROM Activities WHERE ParentActivityID = @TO_ID AND TaskLevel = 2 AND [Name] = 'Program Development and Testing ')
	IF @ActivityID IS NULL
	BEGIN
		SET @ActivityID = dbo.NewSqlGuid()
		INSERT INTO Activities (ProjectId, ParentActivityID, ID, [Name], [Description], Acronym, TaskLevel, DisplayOrder) VALUES (@ProjectID, @TO_ID, @ActivityID, 'Program Development and Testing ', '', 'DEV', 2, 0)
	END


	/** Add task order P04 **/
	SET @TO_ID = (SELECT TOP 1 ID FROM Activities WHERE ParentActivityID IS NULL AND TaskLevel = 1 AND [Name] = 'P04' AND ProjectId = @ProjectID)
	IF @TO_ID IS NULL
	BEGIN
		SET @TO_ID = dbo.NewSqlGuid()
		INSERT INTO Activities (ID, ProjectId, [Name], [Description], TaskLevel, DisplayOrder) VALUES (@TO_ID, @ProjectID, 'P04', '', 1, 0)
	END

	/** P04 Activities **/
	SET @ActivityID = (SELECT TOP 1 ID FROM Activities WHERE ParentActivityID = @TO_ID AND TaskLevel = 2 AND [Name] = 'Establish Capability to Respond to PCORnet Queries and Support PCORnet CC Data QA Activities')
	IF @ActivityID IS NULL
	BEGIN
		SET @ActivityID = dbo.NewSqlGuid()
		INSERT INTO Activities (ProjectId, ParentActivityID, ID, [Name], [Description], Acronym, TaskLevel, DisplayOrder) VALUES (@ProjectID, @TO_ID, @ActivityID, 'Establish Capability to Respond to PCORnet Queries and Support PCORnet CC Data QA Activities', '', 'CAP', 2, 0)
	END

	SET @ActivityID = (SELECT TOP 1 ID FROM Activities WHERE ParentActivityID = @TO_ID AND TaskLevel = 2 AND [Name] = 'Program Development and Testing ')
	IF @ActivityID IS NULL
	BEGIN
		SET @ActivityID = dbo.NewSqlGuid()
		INSERT INTO Activities (ProjectId, ParentActivityID, ID, [Name], [Description], Acronym, TaskLevel, DisplayOrder) VALUES (@ProjectID, @TO_ID, @ActivityID, 'Program Development and Testing ', '', 'DEV', 2, 0)
	END

COMMIT TRANSACTION