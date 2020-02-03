SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT * FROM Information_Schema.Tables WHERE TABLE_SCHEMA = 'dbo' AND  Table_Name = 'RegistryItemDefinitions')
BEGIN

CREATE TABLE [dbo].[RegistryItemDefinitions](
	[Id] [int] NOT NULL,
	[Category] [nvarchar](80) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_RegistryItemDefinitions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END
GO

IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 1)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (1, 'Classification', 'Disease/Disorder/Condition') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 2)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (2, 'Classification', 'Pregnancy') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 3)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (3, 'Classification', 'Product, Biologic') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 4)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (4, 'Classification', 'Product, Device') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 5)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (5, 'Classification', 'Product, Drug') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 6)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (6, 'Classification', 'Service, Encounter') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 7)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (7, 'Classification', 'Service, Hospitalization') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 8)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (8, 'Classification', 'Service, Procedure') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 9)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (9, 'Classification', 'Transplant') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 10)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (10, 'Classification', 'Tumor') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 11)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (11, 'Classification', 'Vaccine') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 12)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (12, 'Purpose', 'Clinical Practice Assessment') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 13)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (13, 'Purpose', 'Effectiveness') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 14)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (14, 'Purpose', 'Natural History of Disease') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 15)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (15, 'Purpose', 'Payment/Certification') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 16)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (16, 'Purpose', 'Post Marketing Commitment') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 17)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (17, 'Purpose', 'Public Health Surveillance') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 18)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (18, 'Purpose', 'Quality Improvement') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 19)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (19, 'Purpose', 'Safety or Harm') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 20)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (20, 'Condition of Interest', 'Bacterial and Fungal Diseases') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 21)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (21, 'Condition of Interest', 'Behaviors and Mental Disorders') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 22)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (22, 'Condition of Interest', 'Blood and Lymph Conditions') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 23)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (23, 'Condition of Interest', 'Cancers and Other Neoplasms') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 24)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (24, 'Condition of Interest', 'Digestive System Diseases') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 25)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (25, 'Condition of Interest', 'Diseases or Abnormalities at or Before Birth') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 26)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (26, 'Condition of Interest', 'Ear, Nose, and Throat Diseases') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 27)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (27, 'Condition of Interest', 'Eye Diseases') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 28)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (28, 'Condition of Interest', 'Gland and Hormone Related Diseases') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 29)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (29, 'Condition of Interest', 'Heart and Blood Diseases') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 30)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (30, 'Condition of Interest', 'Immune System Diseases') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 31)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (31, 'Condition of Interest', 'Mouth and Tooth Diseases') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 32)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (32, 'Condition of Interest', 'Muscle, Bone, and Cartilage Diseases') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 33)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (33, 'Condition of Interest', 'Nervous System Diseases') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 34)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (34, 'Condition of Interest', 'Nutritional and Metabolic Diseases') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 35)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (35, 'Condition of Interest', 'Occupational Diseases') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 36)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (36, 'Condition of Interest', 'Parasitic Diseases') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 37)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (37, 'Condition of Interest', 'Respiratory Tract Diseases') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 38)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (38, 'Condition of Interest', 'Skin and Connective Tissue Diseases') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 39)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (39, 'Condition of Interest', 'Substance Related Disorders') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 40)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (40, 'Condition of Interest', 'Symptoms and General Pathology') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 41)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (41, 'Condition of Interest', 'Urinary Tract, Sexual Organs, and Pregnancy Conditions') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 42)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (42, 'Condition of Interest', 'Viral Diseases') END
IF NOT EXISTS(SELECT NULL FROM RegistryItemDefinitions WHERE Id = 43)
    BEGIN INSERT INTO [RegistryItemDefinitions](Id, Category, Title) VALUES (43, 'Condition of Interest', 'Wounds and Injuries') END

GO

IF NOT EXISTS(SELECT * FROM Information_Schema.Tables WHERE TABLE_SCHEMA = 'dbo' AND  Table_Name = 'Registries')
BEGIN
CREATE TABLE [dbo].[Registries](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[Type] [tinyint] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[RoPRUrl] [nvarchar](500) NULL,
 CONSTRAINT [PK_Registries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[Registries] ADD  CONSTRAINT [DF_Registries_Type]  DEFAULT ((0)) FOR [Type]
ALTER TABLE [dbo].[Registries] ADD  CONSTRAINT [DF_Registries_Id]  DEFAULT ([dbo].[NewSqlGuid]()) FOR [Id]
ALTER TABLE [dbo].[Registries] ADD  CONSTRAINT [DF_Registries_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 = Registry, 1 = DataSet' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Registries', @level2type=N'COLUMN',@level2name=N'Type'

END
GO


IF NOT EXISTS(SELECT * FROM Information_Schema.Tables WHERE TABLE_SCHEMA = 'dbo' AND  Table_Name = 'Registries_RegistryItemDefinitions')
BEGIN
CREATE TABLE [dbo].[Registries_RegistryItemDefinitions](
	[Registry_Id] [uniqueidentifier] NOT NULL,
	[RegistryItemDefinition_Id] [int] NOT NULL,
 CONSTRAINT [PK_RegistryItems] PRIMARY KEY CLUSTERED 
(
	[Registry_Id] ASC,
	[RegistryItemDefinition_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[Registries_RegistryItemDefinitions]  WITH CHECK ADD  CONSTRAINT [FK_RegistryItems_Registries] FOREIGN KEY([Registry_Id])
REFERENCES [dbo].[Registries] ([Id])

ALTER TABLE [dbo].[Registries_RegistryItemDefinitions] CHECK CONSTRAINT [FK_RegistryItems_Registries]

ALTER TABLE [dbo].[Registries_RegistryItemDefinitions]  WITH CHECK ADD  CONSTRAINT [FK_RegistryItems_RegistryItemDefinitions] FOREIGN KEY([RegistryItemDefinition_Id])
REFERENCES [dbo].[RegistryItemDefinitions] ([Id])

ALTER TABLE [dbo].[Registries_RegistryItemDefinitions] CHECK CONSTRAINT [FK_RegistryItems_RegistryItemDefinitions]

END
GO

IF NOT EXISTS(SELECT * FROM Information_Schema.Tables WHERE TABLE_SCHEMA = 'dbo' AND  Table_Name = 'OrganizationsRegistries')
BEGIN
CREATE TABLE [dbo].[OrganizationsRegistries](
	[OrganizationID] [int] NOT NULL,
	[RegistryID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_OrganizationsRegistries] PRIMARY KEY CLUSTERED 
(
	[OrganizationID] ASC,
	[RegistryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[OrganizationsRegistries]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationsRegistries_Organizations] FOREIGN KEY([OrganizationID])
REFERENCES [dbo].[Organizations] ([OrganizationId])

ALTER TABLE [dbo].[OrganizationsRegistries] CHECK CONSTRAINT [FK_OrganizationsRegistries_Organizations]

ALTER TABLE [dbo].[OrganizationsRegistries]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationsRegistries_Registries] FOREIGN KEY([RegistryID])
REFERENCES [dbo].[Registries] ([Id])

ALTER TABLE [dbo].[OrganizationsRegistries] CHECK CONSTRAINT [FK_OrganizationsRegistries_Registries]

END
GO

INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '077.2013.09.24')
GO

