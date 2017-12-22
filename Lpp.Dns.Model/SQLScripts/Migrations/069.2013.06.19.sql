declare 
	@allOrgs uniqueidentifier = 'F3AB0001-DEF9-43D1-B862-A22100FE1882',
	@allUsers uniqueidentifier = '1D3A0001-4717-40A3-98A1-A22100FDE0ED',
	@allProjects uniqueidentifier = '6A690001-7579-4C74-ADE1-A2210107FA29',
	@viewResults uniqueidentifier = 'BDC57049-27BA-41DF-B9F9-A15ABF19B120'

insert into SecurityTargets( Arity, ObjectId1, ObjectId2, ObjectId3 )
select 3, @allProjects, @allOrgs, u.[SID]
from Users u
where not exists( select * from SecurityTargets t where ObjectId1 = @allProjects and ObjectId2 = @allOrgs and ObjectId3 = u.[SID] )

insert into AclEntries( TargetId, Allow, PrivilegeId, SubjectId, [Order] )
select t.Id, 1, @viewResults, u.[sid], 0
from Users u
inner join SecurityTargets t on ObjectId1 = @allProjects and ObjectId2 = @allOrgs and ObjectId3 = u.[SID]
where not exists( select * from AclEntries e where TargetId = t.Id and PrivilegeId = @viewResults and SubjectId = u.[SID] )