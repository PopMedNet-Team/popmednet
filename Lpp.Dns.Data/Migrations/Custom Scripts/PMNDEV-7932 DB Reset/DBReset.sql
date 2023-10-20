use pmnuat;

-- Number of years of request to retain from today
declare @retainYears int;
set @retainYears=5;

declare @requests table (id uniqueidentifier);
insert into @requests select id from requests where identifier in (48301, 48302, 47893, 47845, 47855);

declare @datamarts table (name varchar(100));
insert into @datamarts values 
('PCORnet Replica DataMart 1a'), 
('PCORnet Replica DataMart 1b'), 
('Demo DataMart 2 DRA Data Partner'), 
('Demo DataMart 1 DRA Analysis Center'), 
('.UAT Data Checker DataMart - QE'), 
('.UAT Org A Modular Program DataMart'),
('Test Auto DataMart 1'),
('Test Auto DataMart 2');

declare @users table (name varchar(100));
insert into @users values 
('SsoSite'), -- DO NOT DELETE
('NoteStat'), -- DO NOT DELETE
('Lakshmi'),
('QAEnhancedInvestigator'),
('QAAutomation_EnhancedInvestigator'),
('QAAutomation_Admin'),
('QAAutomation_DMAdmin'),
('QAResponseReviewer'),
('QaResponseReviewer_DMlevel'),
('QARequestReviewer'),
('QAobserver'),
('QAdmadmin'),
('SystemAdministrator'),
('orglevelobserver'),
('QAinvestigator_requestapproval'),
('Cmartin'),
('Cpurington'),
('Tclemmons'),
('Detectifyuser'),
('nKim')
;

declare @organizations table (name varchar(100));
insert into @organizations values 
('Root'), 
('Operations Center'),
('.UAT OrgA'),
('Harvard Pilgrim Healthcare'),
('PCORnet Replica Org'),
('PCPCORnet Replica Org 1b'),
('Demo Organization');

declare @groups table (name varchar(100));
insert into @groups values 
('QA'), ('FDA');

declare @projects table (name varchar(100));
insert into @projects values 
('PCORnet Replica Project'), 
('Automation Project'), 
('Distributed Regression'), 
('.UAT Project');

declare @requesttypes table (name varchar(100));
insert into @requesttypes values 
('MDQ Query Composer / Default Workflow'), 
('QE File Distribution'),
('WF: Summary Query: Prevalence - Enrollment'), 
('WF - Summary Query: MFU Codes'), 
('Simplified MP Request Type'), 
('Simple MP workflow'), 
('QE summary Prevalence request (Default workflow)'), 
('QE summary MFU request (summary workflow)'), 
('QE Summary MFU (Default workflow)'), 
('QE summary incidence request (summary workflow)'), 
('QE Summary Incidence (Default workflow)'), 
('QE Summary Inci ICD-9 Diagnosis (3 digit) (summary workflow)'), 
('QE MFU HCPCS procedures (summary workflow)'), 
('QA WF - Prevalence Pharmacy Dispensings By Drug Name'), 
('Lakshmi - Data checker Age distribution Request type'),
('Distributed Regression Workflow'), 
('Data Checker_Age Distribution'), 
('Menu-Driven Query ADAPTABLE'), 
('QE File Distribution (summary workflow)');

begin transaction;

delete LogsNewRequestDraftSubmitted where requestid in (select requestid from requests where requestid not in (select id from @requests) and createdon <= dateadd(year, -@retainYears, getdate()));
delete LogsRequestDataMartMetadataChange where requestid not in (select id from @requests) and requestid in (select requestid from requests where createdon <= dateadd(year, -@retainYears, getdate()));
delete LogsRoutingStatusChange where ResponseID in (select rdmr.id from RequestDataMartResponses rdmr 
	join requestdatamarts rdm on rdm.id=rdmr.RequestDataMartID
	join requests r on rdm.requestid=r.id where r.id not in (select id from @requests) and r.createdon <= dateadd(year, -@retainYears, getdate()));
delete RequestDocuments where ResponseID in (select rdmr.id from RequestDataMartResponses rdmr 
	join requestdatamarts rdm on rdm.id=rdmr.RequestDataMartID
	join requests r on rdm.requestid=r.id where r.id not in (select id from @requests) and r.createdon <= dateadd(year, -@retainYears, getdate()));
delete RequestDataMartResponses where ID in (select rdmr.id from RequestDataMartResponses rdmr 
	join requestdatamarts rdm on rdm.id=rdmr.RequestDataMartID
	join requests r on rdm.requestid=r.id where r.id not in (select id from @requests) and r.createdon <= dateadd(year, -@retainYears, getdate()));
delete RequestDataMarts where ID in (select rdm.id from RequestDataMarts rdm
	join requests r on rdm.requestid=r.id where r.id not in (select id from @requests) and r.createdon <= dateadd(year, -@retainYears, getdate()));
delete requests where id not in (select id from @requests) and createdon <= dateadd(year, -@retainYears, getdate());

--select * from requests;

delete DataMartAvailabilityPeriods where datamartid in (select id from datamarts where name not in (select name from @datamarts));
delete DataMartAvailabilityPeriods_v2 where datamartid in (select id from datamarts where name not in (select name from @datamarts));
delete datamarts where id in (select id from datamarts where name not in (select name from @datamarts) 
and id not in (select datamartid from requestdatamarts));
--and id not in (select datamartid from datamartavailabilityperiods));
select name from datamarts;

delete documents where UploadedByID in (select id  from users where username not in (select name from @users));

delete LogsUserPasswordChange where userid in (select id  from users where username not in (select name from @users));

delete users where username not in (select name from @users) 
--  and (isdeleted=1 or isactive=0)
  and id not in (select respondedbyid from requestdatamartresponses where respondedbyid is not null)
  and id not in (select submittedbyid from requests where submittedbyid is not null)
  and id not in (select createdbyid from requests where createdbyid is not null)
  and id not in (select updatedbyid from requests where updatedbyid is not null)
  and id not in (select rejectedbyid from requests where rejectedbyid is not null)
  and id not in (select userid from logsuserpasswordchange)
  and id not in (select userchangedid from logsuserpasswordchange);
select username from users u join @users u2 on u.username=u2.name;

delete organizations where name not in (select name from @organizations) and id not in (select organizationid from users) 
and id not in (select organizationid from ProjectOrganizations where projectid in (select id from projects where name in (select name from @projects)));
select name from organizations;
commit;

begin transaction;

delete activities where projectid not in (select id from projects where name in (select name from @projects));
--select distinct p.name from activities a join projects p on p.id=a.projectid;
delete projects where name not in (select name from @projects);
select * from projects;

delete groups where name not in (select name from @groups) and id not in (select groupid from projects);
select * from groups;

commit;

begin transaction;

--delete  where projectid not in (select id from projects where name in (select name from @projects));
--select distinct p.name from activities a join projects p on p.id=a.projectid;
delete requesttypes where name not in (select name from @requesttypes) and id not in (select requesttypeid from requests);
select * from requesttypes rt join @requesttypes rt2 on rt2.name = rt.name;

commit;

--select * from organizations where name not in ('Root', 'Operations Center');
--select count(rdmr.id) from RequestDataMartResponses rdmr 
--join requestdatamarts rdm on rdm.id=rdmr.RequestDataMartID
--join requests r on rdm.requestid=r.id where r.createdon <= dateadd(year, -1, getdate());