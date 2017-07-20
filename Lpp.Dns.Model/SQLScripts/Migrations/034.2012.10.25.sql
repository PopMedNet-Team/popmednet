if exists( select * from sys.triggers where name = 'Security_Membership_EnsureAllSubjectsExist' and parent_id = object_id('AclEntries') )
	drop trigger Security_Membership_EnsureAllSubjectsExist 
go

create trigger Security_Membership_EnsureAllSubjectsExist on AclEntries after update, insert as
	insert into SecurityMembership( [Start], [End] ) 
	select distinct [SubjectId], [SubjectId] from inserted i
	left join SecurityMembership m on [Start] = i.[SubjectId] and [End] = i.[SubjectId]
	where m.[Start] is null
go