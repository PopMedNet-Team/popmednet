-- This is just cleanup: it removes duplicate SecurityTargets

update AclEntries set TargetId = minT
from AclEntries e inner join
(select e.Id as iid, (select MIN(it.id) from SecurityTargets it where 
	it.arity = t.arity 
	and it.objectid1 = t.objectid1
	and (it.objectid2 = t.objectid2 or t.arity < 2)
	and (it.objectid3 = t.objectid3 or t.arity < 3)
	and (it.objectid4 = t.objectid4 or t.arity < 4)
	and (it.objectid5 = t.objectid5 or t.arity < 5)
	and (it.objectid6 = t.objectid6 or t.arity < 6)
	and (it.objectid7 = t.objectid7 or t.arity < 7)
	and (it.objectid8 = t.objectid8 or t.arity < 8)
	and (it.objectid9 = t.objectid9 or t.arity < 9)
	and (it.objectid10 = t.objectid10 or t.arity < 10 ) )
	as minT
from AclEntries e
inner join SecurityTargets t on e.TargetId = t.Id
where (select count(it.id) from SecurityTargets it where 
	it.arity = t.arity 
	and it.objectid1 = t.objectid1
	and (it.objectid2 = t.objectid2 or t.arity < 2)
	and (it.objectid3 = t.objectid3 or t.arity < 3)
	and (it.objectid4 = t.objectid4 or t.arity < 4)
	and (it.objectid5 = t.objectid5 or t.arity < 5)
	and (it.objectid6 = t.objectid6 or t.arity < 6)
	and (it.objectid7 = t.objectid7 or t.arity < 7)
	and (it.objectid8 = t.objectid8 or t.arity < 8)
	and (it.objectid9 = t.objectid9 or t.arity < 9)
	and (it.objectid10 = t.objectid10 or t.arity < 10 ) )
	> 1
) ie
on e.Id = ie.iid

delete from SecurityTargets where not exists (select * from AclEntries where TargetId = SecurityTargets.Id)