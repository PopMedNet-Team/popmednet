declare 
	@allOrgs uniqueidentifier = 'F3AB0001-DEF9-43D1-B862-A22100FE1882',
	@allUsers uniqueidentifier = '1D3A0001-4717-40A3-98A1-A22100FDE0ED',
	@allProjects uniqueidentifier = '6A690001-7579-4C74-ADE1-A2210107FA29',
	@allRequests uniqueidentifier = 'EC260001-2AD7-4EC9-B492-A221011E5AF8',
	@unexaminedResultsReminder uniqueidentifier = 'E39A0001-A4CA-46B8-B7EF-A22200E72B08',
	@observe uniqueidentifier = 'E1A20001-2BE1-48EA-8FC6-A22200E7A7F9'

insert into SecurityTargets( Arity, ObjectId1, ObjectId2, ObjectId3, ObjectId4, ObjectId5 )
select 5, @allProjects, @allOrgs, u.[SID], @allRequests, @unexaminedResultsReminder
from Users u
where not exists( select * from SecurityTargets t where ObjectId1 = @allProjects and ObjectId2 = @allOrgs and ObjectId3 = u.[SID] and ObjectId4 = @allRequests and ObjectId5 = @unexaminedResultsReminder )

insert into AclEntries( TargetId, Allow, PrivilegeId, SubjectId, [Order] )
select t.Id, 1, @observe, u.[sid], 0
from Users u
inner join SecurityTargets t on ObjectId1 = @allProjects and ObjectId2 = @allOrgs and ObjectId3 = u.[SID] and ObjectId4 = @allRequests and ObjectId5 = @unexaminedResultsReminder
where not exists( select * from AclEntries e where TargetId = t.Id and PrivilegeId = @observe and SubjectId = u.[SID] )