if not exists( select * from sys.columns where name='isActive' and object_id = object_id('users') )
begin
	alter table users add isActive bit not null default 0
	alter table users add SignupDate DateTime
	alter table users add ActiveDate DateTime
	exec sp_executesql "update users set isActive = 1"
end
go

if exists(select * from sys.views where object_id = object_id(N'[dbo].[vwUsers]'))
	drop view [dbo].[vwUsers]
go

create view vwUsers as
	select u.*, cast( case when isnull(u.isdeleted,0) = 1 or isnull(o.isdeleted,0) = 1 then 1 else 0 end as bit ) as EffectiveIsDeleted
	from users u inner join organizations o on u.organizationid = o.organizationid
go


