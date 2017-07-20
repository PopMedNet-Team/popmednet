namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAclTables3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AclRequests",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        RequestID = c.Guid(nullable: false),
                        Allowed = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.RequestID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.Requests", t => t.RequestID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.RequestID);
            
            CreateTable(
                "dbo.DataModels",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        Description = c.String(),
                        RequiresConfiguration = c.Boolean(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Name);
            
            CreateTable(
                "dbo.RequestTypeModels",
                c => new
                    {
                        RequestTypeID = c.Guid(nullable: false),
                        DataModelID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.RequestTypeID, t.DataModelID })
                .ForeignKey("dbo.RequestTypes", t => t.RequestTypeID, cascadeDelete: true)
                .ForeignKey("dbo.DataModels", t => t.DataModelID, cascadeDelete: true)
                .Index(t => t.RequestTypeID)
                .Index(t => t.DataModelID);
            
            CreateTable(
                "dbo.RequestTypes",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        ProcessorID = c.Guid(),
                        MetaData = c.Boolean(nullable: false),
                        PostProcess = c.Boolean(nullable: false),
                        AddFiles = c.Boolean(nullable: false),
                        RequiresProcessing = c.Boolean(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AclRequestTypes",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        RequestTypeID = c.Guid(nullable: false),
                        Allowed = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.RequestTypeID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.RequestTypes", t => t.RequestTypeID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.RequestTypeID);

            //Insert data
            Sql(
                @"INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'9e22d68a-7dc3-4ad5-b38a-03ea5f72c654', N'Metadata: Organization Search', N'Search for organizations', N'9d0cd143-7dca-4953-8209-224bdd3af718', 1, 0, 1, 0)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'9d0cd143-7dca-4953-8209-224bdd3af718', N'Metadata Search', NULL, NULL, 0, 0, 1, 0)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'f985dbd9-da7e-41b4-8fbd-2a73b7fcf6dd', N'Sample Processor', NULL, NULL, 0, 0, 1, 0)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'55c48a42-b800-4a55-8134-309cc9954d4c', N'Web Service', NULL, NULL, 0, 0, 1, 0)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'c8bc0bd9-a50d-4b9c-9a25-472827c8640a', N'File Distribution', NULL, NULL, 0, 0, 1, 0)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'd5da7aca-7179-4ea5-bd9c-534d47b6c6c4', N'Data Checker: Diagnosis', NULL, N'5de1cf20-1ce0-49a2-8767-d8bc5d16d36f', 0, 0, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'1bd526d9-46d8-4f66-9191-5731cb8189ee', N'ESP Query', NULL, NULL, 0, 0, 1, 0)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'4ee29758-dcff-4d2a-a7a8-626c81fba367', N'Data Checker: Ethnicity', NULL, N'5de1cf20-1ce0-49a2-8767-d8bc5d16d36f', 0, 0, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'5ca5940a-cf8b-48cc-836c-66b2eb97afb3', N'Data Checker: Race', NULL, N'5de1cf20-1ce0-49a2-8767-d8bc5d16d36f', 0, 0, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'0c330f69-5927-43c8-9036-68cc9d6186c7', N'Metadata: DataMart Search', N'Search for datamarts', N'9d0cd143-7dca-4953-8209-224bdd3af718', 1, 0, 1, 0)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'5d630771-8619-41f7-9407-696302e48237', N'SAS Distribution', NULL, NULL, 0, 0, 1, 0)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', N'Summary Query', NULL, NULL, 0, 0, 1, 0)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'39f8e764-bdd8-4d75-ae50-809c59c28e43', N'Data Checker: Procedure', NULL, N'5de1cf20-1ce0-49a2-8767-d8bc5d16d36f', 0, 0, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'a3044773-8387-4c1b-8139-92b281d0467c', N'Query Composer', N'Allows the composition of complex queries to submit to data sources and analyse the results.', N'ae0da7b0-0f73-4d06-b70b-922032b7f0eb', 0, 0, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'0ffc0001-a09d-423c-b47a-a22200f72944', N'Prev: Pharmacy Dispensings by Generic Name', N'Stratified by age group, sex, and period (either year or quarter).  Results include members, dispensing, and days supplied', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'c97b0001-e7ac-483d-acfa-a22200f7577d', N'Prev: Pharmacy Dispensings by Drug Class', N'Stratified by age group, sex, and period (either year or quarter). Results include members, dispensing, and days supplied.', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'3b200001-591e-4a38-be42-a22200f77798', N'Prev: ICD-9 Diagnoses (3 digit codes)', N'Three-digit ICD-9-CM diagnoses codes stratified by age group, sex, year, and location of service.', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'2b3e0001-4d8b-4f9d-b07e-a22200f79b6e', N'Prev: ICD-9 Procedures (3 digit codes)', N'Three-digit ICD-9-CM procedure codes stratified by age group, sex, year, and location of service.', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'57530001-68d5-4a8d-bdd2-a22200f7cfb9', N'Prev: HCPCS Procedures', N'Five-digit HCPCS (CPT-4) procedure codes stratified by age group, sex, year, and location of service.', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'c8f50001-7893-43c4-a9a0-a22200f7ea52', N'Prev: Enrollment', N'Enrollment figures stratified by age group, sex, and eligibility type.', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'3d2b0001-37ad-4294-bc9a-a22200f80cba', N'Prev: ICD-9 Diagnoses (4 digit codes)', N'Four-digit ICD-9-CM diagnosis codes stratified by age group, sex, year, and location of service.', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'98f50001-c222-4444-83bd-a22200f82a09', N'Prev: ICD-9 Diagnoses (5 digit codes)', N'Five-digit ICD-9-CM diagnosis codes stratified by age group, sex, year, and location of service.', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'19420001-59ca-4968-8b5a-a22200f8429b', N'Prev: ICD-9 Procedures (4 digit codes)', N'Four-digit ICD-9-CM procedure codes stratified by age group, sex, year, and location of service.', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'58190001-7de2-4cc0-9d68-a22200f85baf', N'Prev Metadata: Refresh Dates', N'Period in which DataMart has data for a Query type', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 1, 0, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'bb4e0001-b88e-422d-91ec-a22200f88ab7', N'Inci: Pharmacy Dispensings by Generic Name', N'Stratified by age group, sex, and period (either year or quarter).  Results include members, dispensing, and days supplied.', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'50b60001-c065-4803-bd3f-a22200f8a07d', N'Inci: Pharmacy Dispensings by Drug Class', N'Stratified by age group, sex, and period (either year or quarter). Results include members, dispensing, and days supplied.', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'826d0001-42fd-4b7e-923b-a22200f8b54c', N'Inci: ICD-9 Diagnoses (3 digit codes)', N'Three-digit ICD-9-CM diagnoses codes stratified by age group, sex, year, and location of service.', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'55290001-a764-482b-a4ab-a22200f8d293', N'Inci Metadata: Refresh Dates', N'Period in which DataMart has data for a Query type', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 1, 0, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'33f80001-0e03-473e-ae7a-a22200f8f0e0', N'MFU: Pharmacy Dispensings by Generic Name', N'Stratified by age group, sex, and period (either year or quarter).  Results include members, dispensing, and days supplied.', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'df4a0001-cd75-43f8-9771-a22200f908f4', N'MFU: Pharmacy Dispensings by Drug Class', N'Stratified by age group, sex, and period (either year or quarter).  Results include members, dispensing, and days supplied.', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'd44c0001-7e49-4808-899a-a22200f91e84', N'MFU: ICD-9 Diagnoses (3 digit codes)', N'Three-digit ICD-9-CM diagnoses codes stratified by age group, sex, year, and location of service.', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'b4b60001-c6da-4ae1-a794-a22200f93498', N'MFU: ICD-9 Procedures (3 digit codes)', N'Three-digit ICD-9-CM diagnoses codes stratified by age group, sex, year, and location of service.', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'151e0001-0dc7-43ba-bef0-a22200f94a81', N'MFU: HCPCS Procedures', N'Five-digit HCPCS (CPT-4) procedure codes stratified by age group, sex, year, and location of service.', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'cd9f0001-6cda-4107-875e-a22200f9606e', N'MFU: ICD-9 Diagnoses (4 digit codes)', N'Four-digit ICD-9-CM diagnosis codes stratified by age group, sex, year, and location of service.', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'27b00001-f8ee-4d6a-b714-a22200f9768d', N'MFU: ICD-9 Diagnoses (5 digit codes)', N'Five-digit ICD-9-CM diagnosis codes stratified by age group, sex, year, and location of service.', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'e7b10001-96d6-4896-846f-a22200f98d02', N'MFU: ICD-9 Procedures (4 digit codes)', N'Four-digit ICD-9-CM procedure codes stratified by age group, sex, year, and location of service.', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 0, 1, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'70dd0001-abfc-4d71-91f4-a22200f9a30f', N'MFU Metadata: Refresh Dates', N'Period in which DataMart has data for a Query type', N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', 1, 0, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'd87f0001-b2e6-4c33-8e9d-a22200fb514e', N'SPAN Request', N'SPAN Request to Datamarts', NULL, 0, 0, 1, 0)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'27a90001-d8ef-4b7e-9bc9-a22200fb82f3', N'SAS Distribution', N'Distribute SAS files to DataMarts', N'5d630771-8619-41f7-9407-696302e48237', 0, 0, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'bbb00001-16e2-4c53-8aeb-a22200fbae28', N'Modular Program', N'Modular Program Submission to DataMarts', NULL, 0, 0, 1, 0)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'2c880001-5e3d-4032-9ada-a22200fbc595', N'Ad Hoc', N'Ad Hoc Program Submission to DataMarts', NULL, 0, 0, 1, 0)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'ec1a0001-c467-4f03-a2f7-a22200fbde89', N'Testing', N'Testing Program Submission to DataMarts', NULL, 0, 0, 1, 0)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'a4850001-b3a7-4596-80bc-a22200fc06e9', N'I2B2 (Embedded)', N'Compose i2b2 queries', NULL, 0, 0, 1, 0)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'b01c0001-8b6e-49e9-9a4b-a22200fc3147', N'File Distribution', NULL, N'c8bc0bd9-a50d-4b9c-9a25-472827c8640a', 0, 0, 1, 0)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'6a900001-cfc3-439c-9978-a22200fc5253', N'ICD-9 Diagnosis', N'Compose queries that target populations using 3, 4, and 5 digit ICD-9 diagnosis codes that produce counts stratified by code age, race, sex, and period.', N'1bd526d9-46d8-4f66-9191-5731cb8189ee', 0, 0, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'84cf0001-475c-46ab-998b-a22200fc6439', N'Reportable Disease', N'Compose a request to create a report on reportable disease containing counts stratified by age, race, sex, and period.', N'1bd526d9-46d8-4f66-9191-5731cb8189ee', 0, 0, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'15830001-6dff-47e9-b2fd-a22200fc77c3', N'Query Composer', N'Compose a complex query to create a report of counts stratified by age, race, sex, and period.', N'1bd526d9-46d8-4f66-9191-5731cb8189ee', 0, 0, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'a6ed0001-f6ae-4d25-bef3-a22200fcbabc', N'Sql Distribution', N'Distribute Sql to DataMarts', N'ae85d3e6-93f8-4cb5-bd45-d2f84ab85d83', 0, 0, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'c5d10001-b1fe-4292-a744-a22200fce11b', N'Request Search', N'Search for submitted requests using metadata', N'9d0cd143-7dca-4953-8209-224bdd3af718', 0, 0, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'0f1ea011-b588-4775-9e16-cb6dbe12f8be', N'Data Checker: NDC', NULL, N'5de1cf20-1ce0-49a2-8767-d8bc5d16d36f', 0, 0, 1, 1)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'ae85d3e6-93f8-4cb5-bd45-d2f84ab85d83', N'ESP SQL Distribution', NULL, NULL, 0, 0, 1, 0)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'5de1cf20-1ce0-49a2-8767-d8bc5d16d36f', N'Data Checker', NULL, NULL, 0, 0, 1, 0)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'2ca2379e-40d6-4e59-bd41-fc116d304a43', N'Metadata: Registry Search', N'Search for registries', N'9d0cd143-7dca-4953-8209-224bdd3af718', 1, 0, 1, 0)

INSERT [dbo].[RequestTypes] ([ID], [Name], [Description], [ProcessorID], [MetaData], [PostProcess], [AddFiles], [RequiresProcessing]) VALUES (N'b8f2b52e-cbf9-4ee8-94eb-fc226e2426b6', N'Sample', N'Sample', N'f985dbd9-da7e-41b4-8fbd-2a73b7fcf6dd', 1, 0, 1, 0)

INSERT [dbo].[DataModels] ([ID], [Name], [Description], [RequiresConfiguration]) VALUES (N'575b465f-9c14-4d21-8cec-4f8a87fbf34b', N'SPAN Query Builder', NULL, 0)

INSERT [dbo].[DataModels] ([ID], [Name], [Description], [RequiresConfiguration]) VALUES (N'1b0ffd4c-3eef-479d-a5c4-69d8ba0d0154', N'Modular Program', NULL, 0)

INSERT [dbo].[DataModels] ([ID], [Name], [Description], [RequiresConfiguration]) VALUES (N'82daeca8-634d-4590-9bd3-77a2324f68d4', N'SAS Distribution', NULL, 0)

INSERT [dbo].[DataModels] ([ID], [Name], [Description], [RequiresConfiguration]) VALUES (N'805df412-4acc-4ba0-b0ad-7c4c24fd9f67', N'Summary: Most Frequently Used Queries', NULL, 0)

INSERT [dbo].[DataModels] ([ID], [Name], [Description], [RequiresConfiguration]) VALUES (N'4f364773-20a0-4036-800b-841421cb3209', N'Summary: Incidence Queries', NULL, 0)

INSERT [dbo].[DataModels] ([ID], [Name], [Description], [RequiresConfiguration]) VALUES (N'ce347ef9-3f60-4099-a221-85084f940ede', N'Data Checker', NULL, 0)

INSERT [dbo].[DataModels] ([ID], [Name], [Description], [RequiresConfiguration]) VALUES (N'f4ee0d81-de74-4b3f-adac-8e75301407fd', N'Web Service', NULL, 0)

INSERT [dbo].[DataModels] ([ID], [Name], [Description], [RequiresConfiguration]) VALUES (N'7c69584a-5602-4fc0-9f3f-a27f329b1113', N'ESP Request', NULL, 0)

INSERT [dbo].[DataModels] ([ID], [Name], [Description], [RequiresConfiguration]) VALUES (N'c59f449c-230c-4a6d-b37f-ab62c60ed471', N'Sample Processor', NULL, 0)

INSERT [dbo].[DataModels] ([ID], [Name], [Description], [RequiresConfiguration]) VALUES (N'a8ef75f2-0dc1-4cb1-8fdf-ab7065192352', N'I2B2 Query Builder (Embedded)', NULL, 0)

INSERT [dbo].[DataModels] ([ID], [Name], [Description], [RequiresConfiguration]) VALUES (N'8584f9cd-846e-4024-bd5c-c2a2dd48a5d3', N'MetaData Search', NULL, 0)

INSERT [dbo].[DataModels] ([ID], [Name], [Description], [RequiresConfiguration]) VALUES (N'00bf515f-6539-405b-a617-ca9f8aa12970', N'File Distribution', NULL, 0)

INSERT [dbo].[DataModels] ([ID], [Name], [Description], [RequiresConfiguration]) VALUES (N'3178367a-65ba-4dae-9070-cd786e925635', N'SQL Distribution', NULL, 0)

INSERT [dbo].[DataModels] ([ID], [Name], [Description], [RequiresConfiguration]) VALUES (N'455c772a-df9b-4c6b-a6b0-d4fd4dd98488', N'QueryComposer', NULL, 0)

INSERT [dbo].[DataModels] ([ID], [Name], [Description], [RequiresConfiguration]) VALUES (N'4c99fa21-cdea-4b09-b95b-eebdda05adea', N'Summary: Prevalence Queries', NULL, 0)

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'f985dbd9-da7e-41b4-8fbd-2a73b7fcf6dd', N'c59f449c-230c-4a6d-b37f-ab62c60ed471')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'5de1cf20-1ce0-49a2-8767-d8bc5d16d36f', N'ce347ef9-3f60-4099-a221-85084f940ede')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'ae85d3e6-93f8-4cb5-bd45-d2f84ab85d83', N'3178367a-65ba-4dae-9070-cd786e925635')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'55c48a42-b800-4a55-8134-309cc9954d4c', N'f4ee0d81-de74-4b3f-adac-8e75301407fd')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb', N'4c99fa21-cdea-4b09-b95b-eebdda05adea')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'9d0cd143-7dca-4953-8209-224bdd3af718', N'8584f9cd-846e-4024-bd5c-c2a2dd48a5d3')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'1bd526d9-46d8-4f66-9191-5731cb8189ee', N'7c69584a-5602-4fc0-9f3f-a27f329b1113')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'c8bc0bd9-a50d-4b9c-9a25-472827c8640a', N'00bf515f-6539-405b-a617-ca9f8aa12970')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'5d630771-8619-41f7-9407-696302e48237', N'82daeca8-634d-4590-9bd3-77a2324f68d4')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'd5da7aca-7179-4ea5-bd9c-534d47b6c6c4', N'ce347ef9-3f60-4099-a221-85084f940ede')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'c5d10001-b1fe-4292-a744-a22200fce11b', N'8584f9cd-846e-4024-bd5c-c2a2dd48a5d3')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'3b200001-591e-4a38-be42-a22200f77798', N'4c99fa21-cdea-4b09-b95b-eebdda05adea')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'2b3e0001-4d8b-4f9d-b07e-a22200f79b6e', N'4c99fa21-cdea-4b09-b95b-eebdda05adea')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'0f1ea011-b588-4775-9e16-cb6dbe12f8be', N'ce347ef9-3f60-4099-a221-85084f940ede')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'bb4e0001-b88e-422d-91ec-a22200f88ab7', N'4f364773-20a0-4036-800b-841421cb3209')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'6a900001-cfc3-439c-9978-a22200fc5253', N'7c69584a-5602-4fc0-9f3f-a27f329b1113')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'39f8e764-bdd8-4d75-ae50-809c59c28e43', N'ce347ef9-3f60-4099-a221-85084f940ede')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'19420001-59ca-4968-8b5a-a22200f8429b', N'4c99fa21-cdea-4b09-b95b-eebdda05adea')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'c8f50001-7893-43c4-a9a0-a22200f7ea52', N'4c99fa21-cdea-4b09-b95b-eebdda05adea')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'df4a0001-cd75-43f8-9771-a22200f908f4', N'805df412-4acc-4ba0-b0ad-7c4c24fd9f67')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'a6ed0001-f6ae-4d25-bef3-a22200fcbabc', N'3178367a-65ba-4dae-9070-cd786e925635')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'84cf0001-475c-46ab-998b-a22200fc6439', N'7c69584a-5602-4fc0-9f3f-a27f329b1113')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'27b00001-f8ee-4d6a-b714-a22200f9768d', N'805df412-4acc-4ba0-b0ad-7c4c24fd9f67')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'3d2b0001-37ad-4294-bc9a-a22200f80cba', N'4c99fa21-cdea-4b09-b95b-eebdda05adea')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'c97b0001-e7ac-483d-acfa-a22200f7577d', N'4c99fa21-cdea-4b09-b95b-eebdda05adea')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'57530001-68d5-4a8d-bdd2-a22200f7cfb9', N'4c99fa21-cdea-4b09-b95b-eebdda05adea')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'5ca5940a-cf8b-48cc-836c-66b2eb97afb3', N'ce347ef9-3f60-4099-a221-85084f940ede')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'55290001-a764-482b-a4ab-a22200f8d293', N'4f364773-20a0-4036-800b-841421cb3209')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'50b60001-c065-4803-bd3f-a22200f8a07d', N'4f364773-20a0-4036-800b-841421cb3209')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'b4b60001-c6da-4ae1-a794-a22200f93498', N'805df412-4acc-4ba0-b0ad-7c4c24fd9f67')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'151e0001-0dc7-43ba-bef0-a22200f94a81', N'805df412-4acc-4ba0-b0ad-7c4c24fd9f67')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'0ffc0001-a09d-423c-b47a-a22200f72944', N'4c99fa21-cdea-4b09-b95b-eebdda05adea')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'4ee29758-dcff-4d2a-a7a8-626c81fba367', N'ce347ef9-3f60-4099-a221-85084f940ede')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'33f80001-0e03-473e-ae7a-a22200f8f0e0', N'805df412-4acc-4ba0-b0ad-7c4c24fd9f67')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'27a90001-d8ef-4b7e-9bc9-a22200fb82f3', N'82daeca8-634d-4590-9bd3-77a2324f68d4')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'd44c0001-7e49-4808-899a-a22200f91e84', N'805df412-4acc-4ba0-b0ad-7c4c24fd9f67')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'826d0001-42fd-4b7e-923b-a22200f8b54c', N'4f364773-20a0-4036-800b-841421cb3209')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'cd9f0001-6cda-4107-875e-a22200f9606e', N'805df412-4acc-4ba0-b0ad-7c4c24fd9f67')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'b01c0001-8b6e-49e9-9a4b-a22200fc3147', N'00bf515f-6539-405b-a617-ca9f8aa12970')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'58190001-7de2-4cc0-9d68-a22200f85baf', N'4c99fa21-cdea-4b09-b95b-eebdda05adea')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'70dd0001-abfc-4d71-91f4-a22200f9a30f', N'805df412-4acc-4ba0-b0ad-7c4c24fd9f67')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'15830001-6dff-47e9-b2fd-a22200fc77c3', N'7c69584a-5602-4fc0-9f3f-a27f329b1113')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'98f50001-c222-4444-83bd-a22200f82a09', N'4c99fa21-cdea-4b09-b95b-eebdda05adea')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'a4850001-b3a7-4596-80bc-a22200fc06e9', N'a8ef75f2-0dc1-4cb1-8fdf-ab7065192352')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'd87f0001-b2e6-4c33-8e9d-a22200fb514e', N'575b465f-9c14-4d21-8cec-4f8a87fbf34b')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'2c880001-5e3d-4032-9ada-a22200fbc595', N'1b0ffd4c-3eef-479d-a5c4-69d8ba0d0154')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'e7b10001-96d6-4896-846f-a22200f98d02', N'805df412-4acc-4ba0-b0ad-7c4c24fd9f67')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'bbb00001-16e2-4c53-8aeb-a22200fbae28', N'1b0ffd4c-3eef-479d-a5c4-69d8ba0d0154')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'a3044773-8387-4c1b-8139-92b281d0467c', N'455c772a-df9b-4c6b-a6b0-d4fd4dd98488')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'9e22d68a-7dc3-4ad5-b38a-03ea5f72c654', N'8584f9cd-846e-4024-bd5c-c2a2dd48a5d3')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'0c330f69-5927-43c8-9036-68cc9d6186c7', N'8584f9cd-846e-4024-bd5c-c2a2dd48a5d3')

INSERT [dbo].[RequestTypeModels] ([RequestTypeID], [DataModelID]) VALUES (N'2ca2379e-40d6-4e59-bd41-fc116d304a43', N'8584f9cd-846e-4024-bd5c-c2a2dd48a5d3')

");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequestTypeModels", "DataModelID", "dbo.DataModels");
            DropForeignKey("dbo.AclRequestTypes", "RequestTypeID", "dbo.RequestTypes");
            DropForeignKey("dbo.AclRequestTypes", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclRequestTypes", "PermissionIdentifiers", "dbo.Permissions");
            DropForeignKey("dbo.RequestTypeModels", "RequestTypeID", "dbo.RequestTypes");
            DropForeignKey("dbo.AclRequests", "RequestID", "dbo.Requests");
            DropForeignKey("dbo.AclRequests", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclRequests", "PermissionIdentifiers", "dbo.Permissions");
            DropIndex("dbo.AclRequestTypes", new[] { "RequestTypeID" });
            DropIndex("dbo.AclRequestTypes", new[] { "PermissionIdentifiers" });
            DropIndex("dbo.AclRequestTypes", new[] { "SecurityGroupID" });
            DropIndex("dbo.RequestTypeModels", new[] { "DataModelID" });
            DropIndex("dbo.RequestTypeModels", new[] { "RequestTypeID" });
            DropIndex("dbo.DataModels", new[] { "Name" });
            DropIndex("dbo.AclRequests", new[] { "RequestID" });
            DropIndex("dbo.AclRequests", new[] { "PermissionIdentifiers" });
            DropIndex("dbo.AclRequests", new[] { "SecurityGroupID" });
            DropTable("dbo.AclRequestTypes");
            DropTable("dbo.RequestTypes");
            DropTable("dbo.RequestTypeModels");
            DropTable("dbo.DataModels");
            DropTable("dbo.AclRequests");
        }
    }
}
