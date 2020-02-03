declare @empty uniqueidentifier = '00000000-0000-0000-0000-000000000000'

insert into SecurityInheritance(Start,[End]) 
select distinct ObjectId1, ObjectId1 from SecurityTargets 
left join SecurityInheritance ih on [Start] = ObjectId1 and [End] = ObjectId1
where ih.[Start] is null and ObjectId1 <> @empty

insert into SecurityInheritance(Start,[End]) 
select distinct ObjectId2, ObjectId2 from SecurityTargets 
left join SecurityInheritance ih on [Start] = ObjectId2 and [End] = ObjectId2
where ih.[Start] is null and ObjectId2 <> @empty

insert into SecurityInheritance(Start,[End]) 
select distinct ObjectId3, ObjectId3 from SecurityTargets 
left join SecurityInheritance ih on [Start] = ObjectId3 and [End] = ObjectId3
where ih.[Start] is null and ObjectId3 <> @empty

insert into SecurityInheritance(Start,[End]) 
select distinct ObjectId4, ObjectId4 from SecurityTargets 
left join SecurityInheritance ih on [Start] = ObjectId4 and [End] = ObjectId4
where ih.[Start] is null and ObjectId4 <> @empty

insert into SecurityInheritance(Start,[End]) 
select distinct ObjectId5, ObjectId5 from SecurityTargets 
left join SecurityInheritance ih on [Start] = ObjectId5 and [End] = ObjectId5
where ih.[Start] is null and ObjectId5 <> @empty

declare @admin uniqueidentifier
select @admin = [sid] from [users] where userid=1

declare @allUsers uniqueidentifier = '1D3A0001-4717-40A3-98A1-A22100FDE0ED'
declare @portal uniqueidentifier = 'BBBA0001-2BC2-4E12-A5B4-A22100FDBAFD'
declare @allDms uniqueidentifier = '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4'
declare @allOrgs uniqueidentifier = 'F3AB0001-DEF9-43D1-B862-A22100FE1882'
declare @allGroups uniqueidentifier = '6C380001-FD30-4A47-BC64-A22100FE22EF'
declare @allProjects uniqueidentifier = '6A690001-7579-4C74-ADE1-A2210107FA29'
declare @manageAcl uniqueidentifier = 'D68E7007-E95F-435C-8FAF-0B9FBC9CA997'

declare @all table( id uniqueidentifier )
insert into @all(id) values(@allUsers)
insert into @all(id) values(@allDms)
insert into @all(id) values(@allOrgs)
insert into @all(id) values(@allGroups)
insert into @all(id) values(@allProjects)
insert into @all(id) values(@portal)

insert into securitytargets( objectid1, arity )
select id, 1 from @all a where not exists( select * from securitytargets t where t.objectid1 = a.id and t.arity = 1 )

delete e
from aclentries e
inner join securitytargets t on e.targetid = t.id
inner join @all a  on a.id = t.objectid1 and t.arity = 1
where e.subjectid = @admin and e.privilegeid = @manageAcl

insert into aclentries( targetid, subjectid, privilegeid, allow, [order] )
select t.id, @admin, @manageAcl, 1, 0
from @all a inner join securitytargets t on a.id = t.objectid1 and t.arity = 1

insert into securityinheritance([start], [end])
select [sid], @allOrgs from organizations o
where not exists( select * from securityinheritance i where o.[sid] = i.[start] and @allOrgs = i.[end] )
and o.parentid is null

insert into securityinheritance([start], [end])
select [sid], @alldms from datamarts o
where not exists( select * from securityinheritance i where o.[sid] = i.[start] and @allDms = i.[end] )

insert into securityinheritance([start], [end])
select [sid], @allUsers from [users] o
where not exists( select * from securityinheritance i where o.[sid] = i.[start] and @allUsers = i.[end] )

insert into securityinheritance([start], [end])
select [sid], @allGroups from groups o
where not exists( select * from securityinheritance i where o.[sid] = i.[start] and @allGroups = i.[end] )

insert into securityinheritance([start], [end])
select [sid], @allProjects from projects o
where not exists( select * from securityinheritance i where o.[sid] = i.[start] and @allProjects = i.[end] )