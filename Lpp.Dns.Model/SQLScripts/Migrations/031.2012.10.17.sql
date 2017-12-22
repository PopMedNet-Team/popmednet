if not exists( select * from sys.tables where object_id = object_id('SecurityMembership') ) begin
	create table SecurityMembership
	(
		[Start] uniqueidentifier not null,
		[End] uniqueidentifier not null,
		constraint SecurityMembership_PK primary key clustered ([Start], [End])
	)
end

if not exists( select * from sys.tables where object_id = object_id('SecurityMembershipClosure') ) begin
	create table SecurityMembershipClosure
	(
		[Start] uniqueidentifier not null,
		[End] uniqueidentifier not null,
		constraint SecurityMembershipClosure_PK primary key clustered ([Start], [End])
	)
end

if exists( select * from sys.triggers where parent_id = object_id('SecurityMembership') and name='SecurityMembership_Insert' )
	drop trigger SecurityMembership_Insert
if exists( select * from sys.triggers where parent_id = object_id('SecurityMembership') and name='SecurityMembership_Delete' )
	drop trigger SecurityMembership_Delete
if exists( select * from sys.triggers where parent_id = object_id('SecurityMembership') and name='SecurityMembership_Update' )
	drop trigger SecurityMembership_Update
if exists( select * from sys.triggers where parent_id = object_id('AclEntries') and name='Security_Membership_EnsureAllSubjectsExist' )
	drop trigger Security_Membership_EnsureAllSubjectsExist
go

create trigger SecurityMembership_Update on SecurityMembership instead of update as
	raiserror('Updates on the SecurityMembership table are not supported. Use delete then insert.', 16, 1)
go

create trigger SecurityMembership_Insert on SecurityMembership after insert as
	if exists(select * from SecurityMembershipClosure e 
              inner join inserted i 
              on e.[Start] = i.[End] and e.[End] = i.[Start] and i.[Start] <> i.[End])
    begin
		raiserror('Attempt to create a cycle', 16, 1)
		rollback tran
		return
    end
    
    insert into SecurityMembershipClosure([Start], [End])
    select distinct bf.[Start], af.[End] 
    from inserted i
    inner join SecurityMembershipClosure bf on i.[Start] = bf.[End]
    inner join SecurityMembershipClosure af on i.[End] = af.[Start]
	left join SecurityMembershipClosure ex on ex.[Start] = bf.[Start] and ex.[End] = af.[End]
    where ex.[Start] is null and bf.[Start] <> bf.[End] and af.[Start] <> af.[End] and i.[Start] <> i.[End]

    insert into SecurityMembershipClosure([Start], [End])
    select distinct i.[Start], af.[End] 
    from inserted i
    inner join SecurityMembershipClosure af on i.[End] = af.[Start]
	left join SecurityMembershipClosure ex on ex.[Start] = i.[Start] and ex.[End] = af.[End]
    where ex.[Start] is null and af.[Start] <> af.[End] and i.[Start] <> i.[End]

    insert into SecurityMembershipClosure([Start], [End])
    select distinct bf.[Start], i.[End] 
    from inserted i
    inner join SecurityMembershipClosure bf on i.[Start] = bf.[End]
	left join SecurityMembershipClosure ex on ex.[Start] = bf.[Start] and ex.[End] = i.[End]
    where ex.[Start] is null and bf.[Start] <> bf.[End] and i.[Start] <> i.[End]

    insert into SecurityMembershipClosure([Start], [End])
    select i.[Start], i.[End] from inserted i
	left join SecurityMembershipClosure ex on ex.[Start] = i.[Start] and ex.[End] = i.[End]
    where ex.[Start] is null
go

create trigger SecurityMembership_Delete on SecurityMembership after delete as
	delete from SecurityMembershipClosure where 
		not exists( select * from SecurityMembership m where m.[Start] = SecurityMembershipClosure.[Start] and m.[End] = SecurityMembershipClosure.[End] )
		and
		not exists( select * 
			from SecurityMembershipClosure one
			
			inner join SecurityMembershipClosure two 
			on one.[Start] = SecurityMembershipClosure.[Start] and one.[End] = two.[Start] and two.[End] = SecurityMembershipClosure.[End]

			left join deleted d 
			on ( one.[Start] = d.[Start] and one.[End] = d.[End] ) or ( two.[Start] = d.[Start] and two.[End] = d.[End] )

			where d.[Start] is null
		)
go

create trigger Security_Membership_EnsureAllSubjectsExist on AclEntries after update, insert as
	insert into SecurityMembership( [Start], [End] ) 
	select distinct [SubjectId], [SubjectId] from inserted i
	left join SecurityMembership m on [Start] = i.[SubjectId] and [End] = i.[SubjectId]
	where m.[Start] is null
go

if exists( select * from sys.tables where object_id = object_id('SecurityGroupsUsers') ) begin
	
	insert into SecurityMembership( [Start], [End] )
	select [SID], [SID] from Users

	insert into SecurityMembership( [Start], [End] )
	select [SID], [SID] from SecurityGroups

	insert into SecurityMembership( [Start], [End] )
	select u.[SID], gu.[GroupId] from Users u
	inner join SecurityGroupsUsers gu on u.UserId = gu.UserId
	
    insert into SecurityMembership( [Start], [End] )
	select [ParentId], [ChildId] -- This is not a typo. [ChildId] and [ParentId] should really be reversed. The reason? They were mistakenly reversed in the previous version of the schema, so now I have to reverse them back.
	from SecurityGroupsSecurityGroups

	drop table SecurityGroupsUsers
	drop table SecurityGroupsSecurityGroups

end