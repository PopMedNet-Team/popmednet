--------------------------------------------------------------------------------------------------------------
--PMN-786: Add a new field for organization metadata Observation and Clinical Experience
--------------------------------------------------------------------------------------------------------------
IF Not exists (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Organizations' and COLUMN_NAME = 'ObservationClinicalExperience')
	ALTER TABLE Organizations Add ObservationClinicalExperience varchar(1000) NULL
	
GO	

/****** Object:  View [dbo].[DNS3_Organizations]    Script Date: 07/24/2013 14:29:45 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[DNS3_Organizations]'))
DROP VIEW [dbo].[DNS3_Organizations]
GO

/****** Object:  View [dbo].[DNS3_Organizations]    Script Date: 07/24/2013 14:29:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[DNS3_Organizations]
AS
SELECT     OrganizationId AS Id, OrganizationName AS Name, IsDeleted, IsApprovalRequired, OrganizationAcronym AS Acronym, ParentId, [SID],ContactEmail,ContactFirstName, 
			ContactLastName,ContactPhone,SpecialRequirements, UsageRestrictions, HealthPlanDescription,ObservationClinicalExperience, InpatientEHRApplication, OtherInpatientEHRApplication, OutpatientEHRApplication, OtherOutpatientEHRApplication, 
			InpatientClaims, OutpatientClaims, OutpatientPharmacyClaims, Registeries, ObservationalParticipation, ProspectiveTrials
FROM         dbo.Organizations


GO
--------------------------------------------------------------------------------------------------------------
