/*** Add metadata Data Models to Organization ***/

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [Name] = N'DataModelMSCDM' AND Object_ID = Object_ID(N'Organizations'))    
BEGIN
    ALTER TABLE Organizations ADD DataModelMSCDM bit NOT NULL CONSTRAINT DF_Organizations_DataModelMSCDM DEFAULT 0
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [Name] = N'DataModelHMORNVDW' AND Object_ID = Object_ID(N'Organizations'))    
BEGIN
    ALTER TABLE Organizations ADD DataModelHMORNVDW bit NOT NULL CONSTRAINT DF_Organizations_DataModelHMORNVDW DEFAULT 0
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [Name] = N'DataModelESP' AND Object_ID = Object_ID(N'Organizations'))    
BEGIN
    ALTER TABLE Organizations ADD DataModelESP bit NOT NULL CONSTRAINT DF_Organizations_DataModelESP DEFAULT 0
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [Name] = N'DataModelI2B2' AND Object_ID = Object_ID(N'Organizations'))    
BEGIN
    ALTER TABLE Organizations ADD DataModelI2B2 bit NOT NULL CONSTRAINT DF_Organizations_DataModelI2B2 DEFAULT 0
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [Name] = N'DataModelOther' AND Object_ID = Object_ID(N'Organizations'))    
BEGIN
    ALTER TABLE Organizations ADD DataModelOther nvarchar(80) NULL
END
GO

IF EXISTS(SELECT * FROM sys.views WHERE name = 'DNS3_Organizations')
	DROP VIEW [dbo].[DNS3_Organizations]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[DNS3_Organizations]
AS
SELECT     OrganizationId AS Id, OrganizationName AS Name, IsDeleted, IsApprovalRequired, OrganizationAcronym AS Acronym, ParentId, [SID],ContactEmail,ContactFirstName, 
			ContactLastName,ContactPhone,SpecialRequirements, UsageRestrictions, HealthPlanDescription,ObservationClinicalExperience, 
			InpatientEHRApplication, OtherInpatientEHRApplication, OutpatientEHRApplication, OtherOutpatientEHRApplication, 
			InpatientClaims, OutpatientClaims, OutpatientPharmacyClaims, Registeries, ObservationalParticipation, ProspectiveTrials, 
			EnrollmentClaims, DemographicsClaims, LaboratoryResultsClaims, VitalSignsClaims, OtherClaims, EnableClaimsAndBilling, EnableEHRA,
			DataModelMSCDM, DataModelHMORNVDW, DataModelESP, DataModelI2B2, DataModelOther
FROM         dbo.Organizations
GO

INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '087.2013.11.05')
GO