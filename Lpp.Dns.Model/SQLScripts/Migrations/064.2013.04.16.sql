if exists( select * from sys.views where object_id = object_id('_RoutingCounts') )
	drop view dbo._RoutingCounts
GO

create view [dbo].[_RoutingCounts]
with schemabinding
as
select QueryId,
	Sum(case when QueryStatusTypeId = 2 then 1 else 0 end) as Submitted,
	Sum(case when QueryStatusTypeId = 3 OR QueryStatusTypeId = 14 then 1 else 0 end) as Completed,
	Sum(case when QueryStatusTypeId = 4 then 1 else 0 end) as AwaitingRequestApproval,
	Sum(case when QueryStatusTypeId = 10 then 1 else 0 end) as AwaitingResponseApproval,
	Sum(case when QueryStatusTypeId = 5 then 1 else 0 end) as RejectedRequest,
	Sum(case when QueryStatusTypeId = 12 then 1 else 0 end) as RejectedBeforeUploadResults,
	Sum(case when QueryStatusTypeId = 13 then 1 else 0 end) as RejectedAfterUploadResults,
	COUNT_BIG(*) as Total
from
	dbo.QueriesDataMarts
group by QueryId
GO

create unique clustered index PK_RequestCounts on _RoutingCounts( QueryId )
GO