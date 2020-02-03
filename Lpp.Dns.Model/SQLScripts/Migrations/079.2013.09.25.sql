IF EXISTS(SELECT * FROM [Projects] WHERE Name = 'Mini-Sentinel' )
BEGIN

DECLARE @SID VARCHAR(max)
SET @SID=(SELECT TOP 1 SID FROM [Projects] WHERE Name='Mini-Sentinel')

IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 3)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (3, '03','03' , @SID , null)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 3001)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (3001, '2. A) New Molecular Entities (NMEs)','',@SID, 3)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 3002)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (3002, '2. B) Drugs on the Market for >2 years','',@SID, 3)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 3003)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (3003, '2. C) Evaluation of Effects of FDA’s Regulatory Actions','',@SID, 3)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 3004)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (3004, '2. D) Drug Use Studies - Comparison to Nationally Projected Databases','',@SID, 3)

IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 4)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (4, '04','04' , @SID , null)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 4001)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (4001, '2. A) HOI Validation/Adjudication','',@SID, 4)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 4002)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (4002, '2. B) Statistical Methods Development ','',@SID, 4)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 4003)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (4003, '2. C) Linking of Distributed Database Environments ','',@SID, 4)


IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 6)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (6, '06','06' , @SID , null)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 6001)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (6001, 'Task 1: Define the surveillance population and create the distributed database','',@SID, 6)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 6002)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (6002, 'Task 2: Characterize the Blood-SCAN distributed database','',@SID, 6)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 6003)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (6003, 'Task 3: Conduct an evaluation for thromboembolic events (TEE) after immunoglobulin administration, ','',@SID, 6)


IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 7)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (7, '07','07' , @SID , null)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 7001)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (7001, '2. A) New Molecular Entities (NMEs)','',@SID, 7)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 7002)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (7002, '2. B) Drugs on the Market for >2 years','',@SID, 7)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 7003)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (7003, '2. D) Drug Use Studies - Comparison to Nationally Projected Databases','',@SID, 7)


IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (8, '08','08' , @SID , null)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8001)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (8001, '2. A) HOI Validation/Adjudication','',@SID, 8)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8002)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (8002, '2. B) Alternative Approaches for HOI Validation','',@SID, 8)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8003)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (8003, '2. C) Surveillance Preparedness','',@SID, 8)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8004)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (8004, '2. D) Extension of Methods Taxonomy','',@SID, 8)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8005)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (8005, '2. E) Methods Development','',@SID, 8)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8006)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (8006, '2. F) Routine Surveillance','',@SID, 8)


IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 9)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (9, '09','09' , @SID , null)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 9001)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (9001, '1. Update the MSCDM','',@SID, 9)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 9002)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (9002, '1a. Update the MSCDM','',@SID, 9)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 9003)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (9003, '2a. Prepare Summary Tables for the Query Tool','',@SID, 9)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 9004)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (9004, '2b. Maintain Operation of the Mini-Sentinel Query Tool ','',@SID, 9)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 9005)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (9005, '2c. Program Development and Testing ','',@SID, 9)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 9006)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (9006, '3a. Establish Capacity for Timely Response to MSOC Queries','',@SID, 9)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 9007)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (9007, '3b. Respond to Ad Hoc MSOC Questions and Requests','',@SID, 9)



IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 10)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (10, '10','10' , @SID , null)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 10001)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (10001, '4. Task 1 - Development of Modular Programs Modified with  ','',@SID, 10)



IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 11)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (11, '11','11' , @SID , null)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 11001)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (11001, 'Evaluating Blood-SCAN Findings through Chart Review','',@SID, 11)


IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 12)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (12, '12','12' , @SID , null)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 12001)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (12001, '3. Expansion of Routine Surveillance Capabilities','',@SID, 12)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 12002)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (12002, '5. Create Capacity for Implementation of Routine Surveillance ','',@SID, 12)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 12003)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (12003, '6. Developing Approaches for Randomization within MSDD','',@SID, 12)
IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 12004)
	INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (12004, '7. Implementing Efficiencies to Improve the Query Fulfillment ','',@SID, 12)

END
GO

IF NOT EXISTS(SELECT NULL FROM MigrationScript WHERE ScriptRun = '079.2013.09.25')
BEGIN
	INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '079.2013.09.25')
END
GO
