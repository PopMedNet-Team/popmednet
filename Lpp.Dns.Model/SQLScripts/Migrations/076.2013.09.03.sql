IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE [Name] = N'EnrollmentClaims' AND Object_ID = Object_ID(N'Organizations'))    
BEGIN
    ALTER TABLE Organizations ADD EnrollmentClaims bit NULL
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE [Name] = N'DemographicsClaims' AND Object_ID = Object_ID(N'Organizations'))    
BEGIN
    ALTER TABLE Organizations ADD DemographicsClaims bit NULL
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE [Name] = N'LaboratoryResultsClaims' AND Object_ID = Object_ID(N'Organizations'))    
BEGIN
    ALTER TABLE Organizations ADD LaboratoryResultsClaims bit NULL
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE [Name] = N'VitalSignsClaims' AND Object_ID = Object_ID(N'Organizations'))    
BEGIN
    ALTER TABLE Organizations ADD VitalSignsClaims bit NULL
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE [Name] = N'OtherClaims' AND Object_ID = Object_ID(N'Organizations'))    
BEGIN
    ALTER TABLE Organizations ADD OtherClaims nvarchar(80) NULL
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE [Name] = N'EnableClaimsAndBilling' AND Object_ID = Object_ID(N'Organizations'))    
BEGIN
    ALTER TABLE Organizations ADD EnableClaimsAndBilling bit NOT NULL CONSTRAINT DF_Organizations_EnableClaimsAndBilling DEFAULT 0
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE [Name] = N'EnableClaimsAndBilling' AND Object_ID = Object_ID(N'Organizations'))    
BEGIN
    ALTER TABLE Organizations ADD EnableClaimsAndBilling bit NOT NULL CONSTRAINT DF_Organizations_EnableClaimsAndBilling DEFAULT 0
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE [Name] = N'EnableEHRA' AND Object_ID = Object_ID(N'Organizations'))    
BEGIN
    ALTER TABLE Organizations ADD EnableEHRA bit NOT NULL CONSTRAINT DF_Organizations_EnableEHRA DEFAULT 0
END
GO

/* Set the defaults for the existing organizations */
UPDATE Organizations SET EnrollmentClaims = 0, DemographicsClaims = 0, LaboratoryResultsClaims = 0, VitalSignsClaims = 0, EnableClaimsAndBilling = 1, EnableEHRA = 1

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
			ContactLastName,ContactPhone,SpecialRequirements, UsageRestrictions, HealthPlanDescription,ObservationClinicalExperience, InpatientEHRApplication, OtherInpatientEHRApplication, OutpatientEHRApplication, OtherOutpatientEHRApplication, 
			InpatientClaims, OutpatientClaims, OutpatientPharmacyClaims, Registeries, ObservationalParticipation, ProspectiveTrials, EnrollmentClaims, DemographicsClaims, LaboratoryResultsClaims, VitalSignsClaims, OtherClaims, EnableClaimsAndBilling, EnableEHRA
FROM         dbo.Organizations
GO

INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '076.2013.09.03')
GO

