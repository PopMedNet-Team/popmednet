if not exists( select * from sys.columns where name = 'ClientSettingsXml' and object_id = object_id( 'Users' ) )
	alter table Users add ClientSettingsXml nvarchar(max)

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwUsers]'))
DROP VIEW [dbo].[vwUsers]
GO

create view vwUsers as
select u.*, cast( case when isnull(u.isdeleted,0) = 1 or isnull(o.isdeleted,0) = 1 then 1 else 0 end as bit ) as EffectiveIsDeleted
from users u inner join organizations o on u.organizationid = o.organizationid
go

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[UsedRequestTypes]'))  drop view dbo.UsedRequestTypes
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwUsedRequestTypes]'))  drop view dbo.vwUsedRequestTypes
go

set ansi_padding on
go

create view vwUsedRequestTypes
with schemabinding
as select count_big(*) as [count], requesttypeid from dbo.queries group by requesttypeid
go

create unique clustered index ix on vwusedrequesttypes(requesttypeid)
go

create view UsedRequestTypes
as select RequestTypeId from vwUsedRequestTypes with(noexpand)
go