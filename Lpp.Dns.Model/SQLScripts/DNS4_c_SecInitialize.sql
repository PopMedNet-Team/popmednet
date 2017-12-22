delete from aclentries
delete from securitymembership
delete from securityinheritance
delete from securitytargets
delete from securitygroups
go

declare @allUsers uniqueidentifier, @allDms uniqueidentifier, @allOrgs uniqueidentifier, @portal uniqueidentifier, @allGroups uniqueidentifier
declare @manageAcl uniqueidentifier, @login uniqueidentifier
set @allUsers = '1D3A0001-4717-40A3-98A1-A22100FDE0ED'
set @portal = 'BBBA0001-2BC2-4E12-A5B4-A22100FDBAFD'
set @allDms = '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4'
set @allOrgs = 'F3AB0001-DEF9-43D1-B862-A22100FE1882'
set @allGroups = '6C380001-FD30-4A47-BC64-A22100FE22EF'
set @manageAcl = 'D68E7007-E95F-435C-8FAF-0B9FBC9CA997'
set @login = '5FBA8EF3-F9A3-4ACC-A3D0-09905FA16E8E'

insert into securityinheritance([start], [end]) select [sid], @allUsers from [users]
insert into securityinheritance([start], [end]) select [sid], @allDms from [datamarts]
insert into securityinheritance([start], [end]) select [sid], @allOrgs from [organizations]
insert into securityinheritance([start], [end]) select [sid], @allGroups from [groups]

insert into securitygroups([sid], displayname, organizationid, kind) select [dbo].[NewSqlGuid](), 'Everyone', [id], 1 from dns3_organizations
insert into securitygroups([sid], displayname, organizationid, kind) select [dbo].[NewSqlGuid](), 'Administrators', [id], 2 from dns3_organizations
insert into securitygroups([sid], displayname, organizationid, kind) select [dbo].[NewSqlGuid](), 'Investigators', [id], 3 from dns3_organizations
insert into securitygroups([sid], displayname, organizationid, kind) select [dbo].[NewSqlGuid](), 'EnhancedInvestigators', [id], 4 from dns3_organizations
insert into securitygroups([sid], displayname, organizationid, kind) select [dbo].[NewSqlGuid](), 'QueryAdministrators', [id], 5 from dns3_organizations
insert into securitygroups([sid], displayname, organizationid, kind) select [dbo].[NewSqlGuid](), 'DataMartAdministrators', [id], 6 from dns3_organizations
insert into securitygroups([sid], displayname, organizationid, kind) select [dbo].[NewSqlGuid](), 'Observers', [id], 7 from dns3_organizations

insert into securitymembership([start],[end])
	select [u].[sid], [g].[sid]
	from users u
	inner join dns3_organizations o on u.organizationid = o.[id]
	inner join securitygroups g on g.organizationid = o.[id]
	where ( g.kind = 2 and u.userid=1 ) or g.kind = 1

insert into securitytargets(arity,objectid1) values(1,@portal)
insert into aclentries(targetid,subjectid,privilegeid,[order],allow) 
	select t.[id], g.[sid], @manageAcl, 0, 1
	from securitytargets t, [securitygroups] g 
	where g.kind = 2 --Admins
insert into aclentries(targetid,subjectid,privilegeid,[order],allow) 
	select t.[id], g.[sid], @login, 1, 1
	from securitytargets t, [securitygroups] g 
	where g.kind = 1 --Everyone

go