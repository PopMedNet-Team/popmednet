declare @allOrgs uniqueidentifier = 'F3AB0001-DEF9-43D1-B862-A22100FE1882'
declare @allUsers uniqueidentifier = '1D3A0001-4717-40A3-98A1-A22100FDE0ED'
declare @allProjects uniqueidentifier = '6A690001-7579-4C74-ADE1-A2210107FA29'
declare @allRequests uniqueidentifier = 'EC260001-2AD7-4EC9-B492-A221011E5AF8'

update SecurityTargets set Arity=Arity+1, ObjectId2 = @allOrgs, ObjectId3 = @allUsers, ObjectId4 = ObjectId3
where ObjectId2 = @allRequests and (Arity = 2 or Arity = 3)
	
declare @targetsToFix table( Id int, orgsid uniqueidentifier, usersid uniqueidentifier)
insert into @targetsToFix
select s.Id, o.[SID], u.[SID]
from SecurityTargets s
inner join Queries r on s.ObjectId2 = r.[SID]
inner join Organizations o on r.OrganizationId = o.OrganizationId
inner join Users u on r.CreatedByUserId = u.UserId
where s.Arity = 2 or s.Arity = 3

update e set targetid = (select MIN(Id) from @targetsToFix m where m.orgsid = t.orgsid and m.usersid = t.usersid)
from AclEntries e
inner join @targetsToFix t on e.TargetId = t.Id

delete e
from AclEntries e where 
	exists( select * from AclEntries i where 
		e.PrivilegeId = i.PrivilegeId and e.SubjectId = i.SubjectId and e.TargetId = i.TargetId
		and e.Allow = i.Allow and e.Id > i.Id )
		
delete f
from @targetsToFix f
where not exists( select * from AclEntries where TargetId = f.Id )

delete f
from SecurityTargets f
where not exists( select * from AclEntries where TargetId = f.Id )
		
update t set ObjectId2 = orgsid, ObjectId3 = usersid, ObjectId4 = t.ObjectId3, Arity = t.Arity+1
from SecurityTargets t
inner join @targetsToFix f on t.Id = f.Id


declare @reqPrivs table( id uniqueidentifier )
insert into @reqPrivs(id) values('4CCB0EC2-006D-4345-895E-5DD2C6C8C791') -- Read
insert into @reqPrivs(id) values('1B42D2D7-F7A7-4119-9CC5-22991DC12AD3') -- Edit
insert into @reqPrivs(id) values('D4494B80-966A-473D-A1B3-4B18BBEF1F34') -- View Status
insert into @reqPrivs(id) values('FDEE0BA5-AC09-4580-BAA4-496362985BF7') -- Change Routings

insert into SecurityTargets( Arity, ObjectId1, ObjectId2, ObjectId3 )
select 3, @allProjects, @allOrgs, u.[SID]
from Users u
where not exists( select * from SecurityTargets t where ObjectId1 = @allProjects and ObjectId2 = @allOrgs and ObjectId3 = u.[SID] )

insert into AclEntries( TargetId, Allow, PrivilegeId, SubjectId, [Order] )
select t.Id, 1, p.id, u.[sid], 0
from Users u
inner join SecurityTargets t on ObjectId1 = @allProjects and ObjectId2 = @allOrgs and ObjectId3 = u.[SID]
outer apply @reqPrivs p
where not exists( select * from AclEntries e where TargetId = t.Id and PrivilegeId = p.id and SubjectId = u.[SID] )



declare @evts table( id uniqueidentifier )
insert into @evts(id) values('0A850001-FC8A-4DE2-9AA5-A22200E82398') -- Request Status Change
insert into @evts(id) values('E39A0001-A4CA-46B8-B7EF-A22200E72B08') -- Results Reminder

declare @observe uniqueidentifier = 'E1A20001-2BE1-48EA-8FC6-A22200E7A7F9'

insert into SecurityTargets( Arity, ObjectId1, ObjectId2, ObjectId3, ObjectId4 )
select 4, @allProjects, @allOrgs, u.[SID], e.id
from Users u
outer apply @evts e
where not exists( select * from SecurityTargets t where ObjectId1 = @allProjects and ObjectId2 = @allOrgs and ObjectId3 = u.[SID] )

insert into AclEntries( TargetId, Allow, PrivilegeId, SubjectId, [Order] )
select t.Id, 1, @observe, u.[sid], 0
from Users u
inner join SecurityTargets t on ObjectId1 = @allProjects and ObjectId2 = @allOrgs and ObjectId3 = u.[SID]
outer apply @evts e
where not exists( select * from AclEntries where TargetId = t.Id and PrivilegeId = @observe and SubjectId = u.[SID] )