if not exists( select * from sys.columns where name = 'FailedLoginCount' and object_id = object_id('Users') ) begin
	alter table Users add FailedLoginCount int default 0
	exec sp_sqlexec 'update Users set FailedLoginCount = 0'
	alter table Users alter column FailedLoginCount int not null
end
go

DROP VIEW [dbo].[vwUsers]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

create view [dbo].[vwUsers] as
	select u.*, cast( case when isnull(u.isdeleted,0) = 1 or isnull(o.isdeleted,0) = 1 then 1 else 0 end as bit ) as EffectiveIsDeleted
	from users u inner join organizations o on u.organizationid = o.organizationid


GO