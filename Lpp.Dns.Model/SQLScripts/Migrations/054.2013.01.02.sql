if not exists( select * from sys.columns where name = 'SubmitTime' and object_id = object_id('RequestRoutingInstances') ) begin
	alter table RequestRoutingInstances add SubmitTime datetime2
	alter table RequestRoutingInstances add SubmittedByUserId int references Users
	alter table RequestRoutingInstances add SubmitMessage nvarchar(max)
	alter table RequestRoutingInstances add ResponseMessage nvarchar(max)
	exec sp_sqlexec 'update r set 
			SubmitTime = isnull( rq.Submitted, rq.CreatedAt ), 
			SubmittedByUserId = rq.CreatedByUserId,
			SubmitMessage = r.Message
		from RequestRoutingInstances r
		inner join Queries rq on r.RequestId = rq.QueryId'
	alter table RequestRoutingInstances alter column SubmitTime datetime2 not null
	alter table RequestRoutingInstances alter column SubmittedByUserId int not null
	alter table RequestRoutingInstances drop column [Message]
end
go