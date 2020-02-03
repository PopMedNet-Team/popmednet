-- This migration is tied to the introduction of projects.
-- Before projects, all request permissions (such as Read, Delete, View Results, etc.) were applied to the
-- pair of (Organization,Request).
-- With projects, however, it now must apply to the pair of (Project,Request).
--
-- This migration updates the ACLs by replacing every orgs with projects for all requests.

declare @allProjects uniqueidentifier, @allOrgs uniqueidentifier
set @allProjects = '6A690001-7579-4C74-ADE1-A2210107FA29'
set @allOrgs = 'F3AB0001-DEF9-43D1-B862-A22100FE1882'

update t set objectid1 = case when t.objectid1 = @allOrgs then @allProjects else isnull( q.projectid, @allProjects ) end
from securitytargets t
inner join queries q on q.[sid] = t.objectid2
inner join organizations o on q.organizationid = o.organizationid and (t.objectid1 = o.[sid] or t.ObjectId1 = @allOrgs)
where t.arity = 2