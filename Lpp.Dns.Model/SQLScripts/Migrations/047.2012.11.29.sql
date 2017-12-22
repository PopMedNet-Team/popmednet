-- 1) "Create Projects" and "List Projects" permissions are now applied to "Org Group", not to "Portal" as before
-- 2) "List Requests" is now applied to "Project", not to "Portal" as before

declare @allGroups uniqueidentifier = '6C380001-FD30-4A47-BC64-A22100FE22EF'
declare @allProjects uniqueidentifier = '6A690001-7579-4C74-ADE1-A2210107FA29'
declare @portal uniqueidentifier = 'BBBA0001-2BC2-4E12-A5B4-A22100FDBAFD'
declare @CreateProjects uniqueidentifier = '93623C60-6425-40A0-91A0-01FA34920913'
declare @ListProjects uniqueidentifier = '8C5E44DC-284E-45D8-A014-A0CD815883AE'
declare @ListRequests uniqueidentifier = '8DCA22F0-EA18-4353-BA45-CC2692C7A844'

if not exists( select * from SecurityTargets where arity = 1 and objectid1 = @allGroups )
	insert into SecurityTargets( objectid1, arity ) values( @allGroups, 1 )

if not exists( select * from SecurityTargets where arity = 1 and objectid1 = @allProjects )
	insert into SecurityTargets( objectid1, arity ) values( @allProjects, 1 )

update e set targetid = ( select [id] from SecurityTargets where arity = 1 and objectid1 = @allGroups )
from aclentries e
inner join securitytargets t on e.targetid = t.Id
where t.arity = 1 and objectid1 = @portal and e.privilegeid in ( @CreateProjects, @ListProjects )

update e set targetid = ( select [id] from SecurityTargets where arity = 1 and objectid1 = @allProjects )
from aclentries e
inner join securitytargets t on e.targetid = t.Id
where t.arity = 1 and objectid1 = @portal and e.privilegeid = @ListRequests