-- This migration is tied to the introduction of projects.
-- Before projects, all datamart permissions were applied to the pair of (Organization,DataMart).
-- With projects, some of them must apply to the triple of (Project,Organization,DataMart),
-- namely: 
--		See Request Queue
--		Upload Results
--		Hold Request
--		Reject Request
--		Approve Responses
--		Skip Response Approval
--		Group Responses
--
-- This migration updates the ACLs accordingly

declare @acl table( id1 uniqueidentifier, id2 uniqueidentifier, subj uniqueidentifier, priv uniqueidentifier )
declare @allProjects uniqueidentifier = '6A690001-7579-4C74-ADE1-A2210107FA29'
declare @SeeRequests uniqueidentifier = '5D6DD388-7842-40A1-A27A-B9782A445E20'
declare @UploadResults uniqueidentifier = '0AC48BA6-4680-40E5-AE7A-F3436B0852A0'
declare @HoldRequest uniqueidentifier = '894619BE-9A73-4DA9-A43A-10BCC563031C'
declare @RejectRequest uniqueidentifier = '0CABF382-93D3-4DAC-AA80-2DE500A5F945'
declare @ApproveResponses uniqueidentifier = 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6'
declare @SkipResponseApproval uniqueidentifier = 'A0F5B621-277A-417C-A862-801D7B9030A2'
declare @GroupResponses uniqueidentifier = 'F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE'

insert into @acl(id1,id2,subj,priv)
select objectid1, objectid2, subjectid, privilegeid
from aclentries e
inner join securitytargets t on e.targetid = t.id
where arity = 2 and e.privilegeid in ( @SeeRequests, @UploadResults, @HoldRequest, @RejectRequest, @ApproveResponses, @SkipResponseApproval, @GroupResponses )

insert into securitytargets(objectid1, objectid2, objectid3, arity)
select distinct @allProjects, id1, id2, 4 from @acl
where not exists( select * from securitytargets where objectid1 = @allProjects and objectid2 = id1 and objectid3 = id2 )

update e set targetid = nt.Id
from aclentries e
inner join securitytargets t on e.targetid = t.Id
inner join securitytargets nt on nt.objectid1 = @allProjects and nt.objectid2 = t.objectid1 and nt.objectid3 = t.objectid2
where t.arity = 2 and nt.arity = 3 and e.privilegeid in ( @SeeRequests, @UploadResults, @HoldRequest, @RejectRequest, @ApproveResponses, @SkipResponseApproval, @GroupResponses )