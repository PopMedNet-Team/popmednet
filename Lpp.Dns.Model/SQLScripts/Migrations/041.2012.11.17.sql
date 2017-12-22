-- This migration is tied to the introduction of projects.
-- Before projects, the "Submit" permission was applied to the triple of (Organization,DataMart,RequestType).
-- With projects, however, it now must apply to the quad of (Project,Organization,DataMart,RequestType).
--
-- This migration updates the ACLs by replacing every such triple with a quad, where the first component is "All Projects".

declare @acl table( id1 uniqueidentifier, id2 uniqueidentifier, id3 uniqueidentifier, subj uniqueidentifier, priv uniqueidentifier )
declare @submit uniqueidentifier = 'BA5C26D1-448B-4D7D-B237-0E0F04F406E3'
declare @allProjects uniqueidentifier = '6A690001-7579-4C74-ADE1-A2210107FA29'

insert into @acl(id1,id2,id3,subj,priv)
select objectid1, objectid2, objectid3, subjectid, privilegeid
from aclentries e
inner join securitytargets t on e.targetid = t.id
where arity = 3 and e.privilegeid = @submit

insert into securitytargets(objectid1, objectid2, objectid3, objectid4, arity)
select distinct @allProjects, id1, id2, id3, 4 from @acl
where not exists( select * from securitytargets where objectid1 = @allProjects and objectid2 = id1 and objectid3 = id2 and objectid4 = id3 )

update e set targetid = nt.Id
from aclentries e
inner join securitytargets t on e.targetid = t.Id
inner join securitytargets nt on nt.objectid1 = @allProjects and nt.objectid2 = t.objectid1 and nt.objectid3 = t.objectid2 and nt.objectid4 = t.objectid3
where t.arity = 3 and nt.arity = 4 and e.privilegeid = @submit