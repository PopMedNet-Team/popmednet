ALTER TABLE dbo.Organizations 
	ADD  
	 [ContactEmail] [varchar](510) NULL,
	 [ContactFirstName] [varchar](100) NULL,
	 [ContactLastName] [varchar](100) NULL,
	 [ContactPhone] [varchar](15) NULL,
	 [SpecialRequirements] [varchar](1000) NULL,
	 [UsageRestrictions] [varchar](1000) NULL,
	 [HealthPlanDescription] [nvarchar](1000) NULL,
	 [InpatientEHRApplication] [varchar] (512) NULL,
	 [OutpatientEHRApplication] [varchar] (512) NULL,
	 [OtherInpatientEHRApplication] [varchar] (512) NULL,
	 [OtherOutpatientEHRApplication] [varchar] (512) NULL,
	 [InpatientClaims] [bit] NULL,
	 [OutpatientClaims] [bit] NULL,
	 [OutpatientPharmacyClaims] [bit] NULL,
	 [Registeries] [varchar] (1000) NULL,
	 [ObservationalParticipation] [bit] NULL,
	 [ProspectiveTrials] [bit] NULL
GO

ALTER VIEW dbo.DNS3_Organizations
AS
SELECT     OrganizationId AS Id, OrganizationName AS Name, IsDeleted, IsApprovalRequired, OrganizationAcronym AS Acronym, ParentId, [SID],ContactEmail,ContactFirstName, 
			ContactLastName,ContactPhone,SpecialRequirements, UsageRestrictions, HealthPlanDescription, InpatientEHRApplication, OtherInpatientEHRApplication, OutpatientEHRApplication, OtherOutpatientEHRApplication, 
			InpatientClaims, OutpatientClaims, OutpatientPharmacyClaims, Registeries, ObservationalParticipation, ProspectiveTrials
FROM         dbo.Organizations
GO

UPDATE [dbo].[Organizations]
   SET [ContactEmail] = null
      ,[ContactFirstName] = null
      ,[ContactLastName] = null
      ,[ContactPhone] = null
      ,[SpecialRequirements] = null
      ,[UsageRestrictions] = null
      ,[HealthPlanDescription] = null
      ,[InpatientEHRApplication] = 'None'
      ,[OutpatientEHRApplication] = 'None'
      ,[OtherInpatientEHRApplication] = null
      ,[OtherOutpatientEHRApplication] = null
      ,[InpatientClaims] = 0
      ,[OutpatientClaims] = 0
      ,[OutpatientPharmacyClaims] = 0
      ,[Registeries] = null
      ,[ObservationalParticipation] = 0
      ,[ProspectiveTrials] = 0
GO


ALTER TABLE dbo.DataMarts 
	ADD  
	 [Description] [varchar](510) NULL,
	 [Acronym] [varchar] (100) NULL,
	 [StartDate] [datetime] NULL,
	 [EndDate] [datetime] NULL,
	 [DataUpdateFrequency] [varchar] (100) NULL,
	 [InpatientEHRApplication] [varchar] (512) NULL,
	 [OutpatientEHRApplication] [varchar] (512) NULL,
	 [InpatientClaims] [bit] NULL,
	 [OutpatientClaims] [bit] NULL,
	 [OutpatientPharmacyClaims] [bit] NULL,
	 [Registeries] [varchar] (1000) NULL,
     [OtherInpatientEHRApplication] [varchar] (512) NULL,
     [OtherOutpatientEHRApplication] [varchar] (512) NULL,
    [DataModel][varchar] (512) NULL,
	[OtherDataModel] [varchar] (512) NULL

GO

ALTER VIEW [dbo].[DNS3_DataMarts]
AS
    SELECT     d.DataMartId AS Id, d.DataMartName AS Name, d.Url, d.RequiresApproval, d.DataMartTypeId, d.AvailablePeriod, d.ContactEmail, d.ContactFirstName, d.ContactLastName, 
               d.ContactPhone, d.SpecialRequirements, d.UsageRestrictions, d.isDeleted, d.HealthPlanDescription, d.OrganizationId, d.IsGroupDataMart, d.UnattendedMode, d.[Description], d.Acronym, 
               d.StartDate, d.EndDate, d.DataUpdateFrequency, d.InpatientEHRApplication, d.OtherInpatientEHRApplication, d.OutpatientEHRApplication, d.OtherOutpatientEHRApplication, 
               d.InpatientClaims, d.OutpatientClaims, d.OutpatientPharmacyClaims, d.Registeries, d.DataModel, d.OtherDataModel,
			   d.[SID],
		   cast( case when isnull(d.isdeleted,0) = 1 or isnull(o.isdeleted,0) = 1 then 1 else 0 end as bit) as EffectiveIsDeleted
    FROM         dbo.DataMarts d
      inner join organizations o on d.organizationid = o.organizationid

GO

UPDATE [dbo].[DataMarts]
   SET [Description] = null
      ,[Acronym] = null
      ,[StartDate] = null
      ,[EndDate] = null
      ,[DataUpdateFrequency] = 'None'
      ,[InpatientEHRApplication] = 'None'
      ,[OutpatientEHRApplication] = 'None'
      ,[OtherInpatientEHRApplication] = null
      ,[OtherOutpatientEHRApplication] = null
      ,[InpatientClaims] = 0
      ,[OutpatientClaims] = 0
      ,[OutpatientPharmacyClaims] = 0
      ,[Registeries] = null
	  ,[DataModel] = 'None'
	  ,[OtherDataModel] = null      
GO