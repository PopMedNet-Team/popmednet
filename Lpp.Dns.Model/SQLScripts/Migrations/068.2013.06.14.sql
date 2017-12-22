if not exists( select * from sys.columns where name = 'IsLocal' and object_id = object_id( 'DataMarts' ) )
	alter table DataMarts add IsLocal bit not null default 0
GO

ALTER VIEW [dbo].[DNS3_DataMarts]
AS
    SELECT     d.DataMartId AS Id, d.DataMartName AS Name, d.Url, d.RequiresApproval, d.DataMartTypeId, d.AvailablePeriod, d.ContactEmail, d.ContactFirstName, d.ContactLastName, 
               d.ContactPhone, d.SpecialRequirements, d.UsageRestrictions, d.isDeleted, d.HealthPlanDescription, d.OrganizationId, d.IsGroupDataMart, d.UnattendedMode, d.[Description], d.Acronym, 
               d.StartDate, d.EndDate, d.DataUpdateFrequency, d.InpatientEHRApplication, d.OtherInpatientEHRApplication, d.OutpatientEHRApplication, d.OtherOutpatientEHRApplication, 
               d.InpatientClaims, d.OutpatientClaims, d.OutpatientPharmacyClaims, d.Registeries, d.DataModel, d.OtherDataModel,
			   d.[SID], d.IsLocal,
		   cast( case when isnull(d.isdeleted,0) = 1 or isnull(o.isdeleted,0) = 1 then 1 else 0 end as bit) as EffectiveIsDeleted
    FROM         dbo.DataMarts d
      inner join organizations o on d.organizationid = o.organizationid

GO