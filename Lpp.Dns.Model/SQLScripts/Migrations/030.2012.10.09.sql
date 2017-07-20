if not exists( select * from sys.columns where name='UnattendedMode' and object_id = object_id('datamarts') )
	alter table datamarts add UnattendedMode int
go

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[DNS3_DataMarts]'))
    DROP VIEW [dbo].[DNS3_DataMarts]
go

CREATE VIEW [dbo].[DNS3_DataMarts]
AS
    SELECT     d.DataMartId AS Id, d.DataMartName AS Name, d.Url, d.RequiresApproval, d.DataMartTypeId, d.AvailablePeriod, d.ContactEmail, d.ContactFirstName, d.ContactLastName, 
               d.ContactPhone, d.SpecialRequirements, d.UsageRestrictions, d.isDeleted, d.HealthPlanDescription, d.OrganizationId, d.IsGroupDataMart, d.UnattendedMode,
			   d.[SID],
		   cast( case when isnull(d.isdeleted,0) = 1 or isnull(o.isdeleted,0) = 1 then 1 else 0 end as bit) as EffectiveIsDeleted
    FROM         dbo.DataMarts d
      inner join organizations o on d.organizationid = o.organizationid
go
