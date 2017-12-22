if exists( select * from sys.views where object_id = object_id( 'Security_Tuple1' ) ) drop view Security_Tuple1
if exists( select * from sys.views where object_id = object_id( '_Security_Tuple1' ) ) drop view _Security_Tuple1
if exists( select * from sys.views where object_id = object_id( 'Security_Tuple2' ) ) drop view Security_Tuple2
if exists( select * from sys.views where object_id = object_id( '_Security_Tuple2' ) ) drop view _Security_Tuple2
if exists( select * from sys.views where object_id = object_id( 'Security_Tuple3' ) ) drop view Security_Tuple3
if exists( select * from sys.views where object_id = object_id( '_Security_Tuple3' ) ) drop view _Security_Tuple3
if exists( select * from sys.views where object_id = object_id( 'Security_Tuple4' ) ) drop view Security_Tuple4
if exists( select * from sys.views where object_id = object_id( '_Security_Tuple4' ) ) drop view _Security_Tuple4
if exists( select * from sys.views where object_id = object_id( 'Security_Tuple5' ) ) drop view Security_Tuple5
if exists( select * from sys.views where object_id = object_id( '_Security_Tuple5' ) ) drop view _Security_Tuple5
if exists( select * from sys.tables where object_id = object_id( 'SecurityObjects1_p' ) ) drop table SecurityObjects1_p
if exists( select * from sys.tables where object_id = object_id( 'SecurityObjects2_p' ) ) drop table SecurityObjects2_p
if exists( select * from sys.tables where object_id = object_id( 'SecurityObjects2_c' ) ) drop table SecurityObjects2_c
if exists( select * from sys.tables where object_id = object_id( 'SecurityObjects3_p' ) ) drop table SecurityObjects3_p
if exists( select * from sys.tables where object_id = object_id( 'SecurityObjects3_c' ) ) drop table SecurityObjects3_c
if exists( select * from sys.tables where object_id = object_id( 'SecurityObjects4_p' ) ) drop table SecurityObjects4_p
if exists( select * from sys.tables where object_id = object_id( 'SecurityObjects4_c' ) ) drop table SecurityObjects4_c
if exists( select * from sys.tables where object_id = object_id( 'SecurityObjects5_p' ) ) drop table SecurityObjects5_p
if exists( select * from sys.tables where object_id = object_id( 'SecurityObjects5_c' ) ) drop table SecurityObjects5_c

if exists( select * from sys.triggers where name = 'SecurityObjects_Copy_Insert' and parent_id = object_id( 'SecurityObjects' ) )
	drop trigger SecurityObjects_Copy_Insert
if exists( select * from sys.triggers where name = 'SecurityObjects_Copy_Update' and parent_id = object_id( 'SecurityObjects' ) )
	drop trigger SecurityObjects_Copy_Update
if exists( select * from sys.triggers where name = 'SecurityObjects_Copy_Delete' and parent_id = object_id( 'SecurityObjects' ) )
	drop trigger SecurityObjects_Copy_Delete
if exists( select * from sys.triggers where name = 'SecurityTargets_MakeSureObjectsExist' and parent_id = object_id( 'SecurityTargets' ) )
	drop trigger SecurityTargets_MakeSureObjectsExist
go

if exists( select * from sys.triggers where name = 'SecurityInheritanceClosure_Copy_Insert' and parent_id = object_id( 'SecurityInheritanceClosure' ) )
	drop trigger SecurityInheritanceClosure_Copy_Insert
if exists( select * from sys.triggers where name = 'SecurityInheritanceClosure_Copy_Update' and parent_id = object_id( 'SecurityInheritanceClosure' ) )
	drop trigger SecurityInheritanceClosure_Copy_Update
if exists( select * from sys.triggers where name = 'SecurityInheritanceClosure_Copy_Delete' and parent_id = object_id( 'SecurityInheritanceClosure' ) )
	drop trigger SecurityInheritanceClosure_Copy_Delete

if exists( select * from sys.tables where object_id = object_id( 'SecurityMembershipClosure' ) ) drop table SecurityMembershipClosure
go

if exists( select * from sys.tables where object_id = object_id( 'SecurityInheritanceClosure' ) ) drop table SecurityInheritanceClosure
if exists( select * from sys.tables where object_id = object_id( 'SecurityInheritanceClosure2' ) ) drop table SecurityInheritanceClosure2
if exists( select * from sys.tables where object_id = object_id( 'SecurityInheritanceClosure3' ) ) drop table SecurityInheritanceClosure3
if exists( select * from sys.tables where object_id = object_id( 'SecurityInheritanceClosure4' ) ) drop table SecurityInheritanceClosure4
if exists( select * from sys.tables where object_id = object_id( 'SecurityInheritanceClosure5' ) ) drop table SecurityInheritanceClosure5
go

if exists( select * from sys.views where object_id = object_id( '_vwSecurityObjectsWithParents' ) ) drop view _vwSecurityObjectsWithParents
if exists( select * from sys.views where object_id = object_id( '_vwAclEntries' ) ) drop view _vwAclEntries
go

create table SecurityMembershipClosure
(
	[Start] uniqueidentifier not null,
	[End] uniqueidentifier not null,
	[Distance] int not null default 0,
	constraint SecurityMembershipClosure_PK primary key clustered ([Start], [End])
)
go

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

	declare c cursor local forward_only read_only for select [Start], [End] from inserted
	open c
	declare @s uniqueidentifier, @e uniqueidentifier
	fetch next from c into @s, @e

	while @@fetch_status = 0 begin
		declare @incomingClosures table( [Start] uniqueidentifier not null, [End] uniqueidentifier not null, [Distance] int not null )
	
		insert into @incomingClosures([Start],[End],[Distance]) values( @s, @e, case when @s = @e then 0 else 1 end )

		if @s <> @e begin
			insert into @incomingClosures([Start],[End],[Distance])
			select bf.[Start], af.[End], bf.[Distance] + af.[Distance] + 1
			from SecurityMembershipClosure bf
			inner join SecurityMembershipClosure af 
			on @e = bf.[End] and @e = af.[Start] and bf.[Start] <> bf.[End] and af.[Start] <> af.[End]
    
			insert into @incomingClosures([Start],[End],[Distance])
			select @s, af.[End], af.[Distance] + 1
			from SecurityMembershipClosure af where @e = af.[Start] and af.[Start] <> af.[End]

			insert into @incomingClosures([Start],[End],[Distance])
			select bf.[Start], @e, bf.[Distance] + 1
			from SecurityMembershipClosure bf where @s = bf.[End] and bf.[Start] <> bf.[End]
		end

		update SecurityMembershipClosure set [Distance] = i.[Distance]
		from SecurityMembershipClosure c
		inner join @incomingClosures i on c.[Start] = i.[Start] and c.[End] = i.[End] and c.[Distance] > i.[Distance]

		insert into SecurityMembershipClosure([Start], [End], [Distance])
		select i.[Start], i.[End], i.[Distance] from @incomingClosures i
		left join SecurityMembershipClosure ex on ex.[Start] = i.[Start] and ex.[End] = i.[End]
		where ex.[Start] is null

		fetch next from c into @s, @e
	end

	close c
	deallocate c
go

create trigger SecurityMembership_Delete on SecurityMembership after delete as
	-- Simply delete empty loops
	delete from SecurityMembershipClosure
	where [Start] = [End] and [Start] in (select [Start] from deleted where [Start] = [End])

	-- For all non-loop edges, find alternative routes
	declare @alternativeRoutes table( [Start] uniqueidentifier not null, [End] uniqueidentifier not null, [Distance] int null )
	insert into @alternativeRoutes( [Start], [End], [Distance] )
	select d.[Start], d.[End], max( one.[Distance] + two.[Distance] )
	from deleted d
	left join SecurityMembershipClosure one on d.[Start] = one.[Start] and d.[Start] <> d.[End]
	left join SecurityMembershipClosure two on d.[End] = two.[End] and one.[End] = two.[Start]
	group by d.[Start], d.[End]

	delete from c
	from @alternativeRoutes a
	-- This trick is here in order to consider four possible paths: bf+a+af, a+af, bf+a, a
	-- for all possible combinations of "bf" and "af".
	-- Simply doing "left join" will not give us the latter three options unless there are no possible values for "bf" or "af".
	-- Therefore, we artificially fabricate a case when there are no possible values for "bf" by including value 1 for bb.x
	-- and a case with no values for "af" by including value 1 for aa.x
	cross apply ( select 0 as x union select 1 ) bb   
	cross apply ( select 0 as x union select 1 ) aa   
	left join SecurityMembershipClosure bf on bf.[End] = a.[Start] and bb.x = 0
	left join SecurityMembershipClosure af on af.[Start] = a.[End] and aa.x = 0
	inner join SecurityMembershipClosure c on c.[Start] = isnull( bf.[Start], a.[Start] ) and c.[End] = isnull( af.[End], a.[End] ) and a.Distance is null

	update c set [Distance] = c.[Distance] + a.[Distance] - 1
	from @alternativeRoutes a
	cross apply ( select 0 as x union select 1 ) bb
	cross apply ( select 0 as x union select 1 ) aa
	left join SecurityMembershipClosure bf on bf.[End] = a.[Start] and bb.x = 0
	left join SecurityMembershipClosure af on af.[Start] = a.[End] and aa.x = 0
	inner join SecurityMembershipClosure c on c.[Start] = isnull( bf.[Start], a.[Start] ) and c.[End] = isnull( af.[End], a.[End] ) and a.Distance is not null
go

if not exists( select * from sys.tables where object_id = object_id( 'SecurityInheritance' ) )
	create table SecurityInheritance
	(
		[Start] uniqueidentifier not null,
		[End] uniqueidentifier not null,
		constraint SecurityInheritance_PK primary key clustered ([Start], [End])
	)
go

create table SecurityInheritanceClosure
(
	[Start] uniqueidentifier not null,
	[End] uniqueidentifier not null,
	[Distance] int not null default 0,
	constraint SecurityInheritanceClosure_PK primary key clustered ([Start], [End])
)
go

if exists( select * from sys.triggers where parent_id = object_id('SecurityInheritance') and name='SecurityInheritance_Insert' )
	drop trigger SecurityInheritance_Insert
if exists( select * from sys.triggers where parent_id = object_id('SecurityInheritance') and name='SecurityInheritance_Delete' )
	drop trigger SecurityInheritance_Delete
if exists( select * from sys.triggers where parent_id = object_id('SecurityInheritance') and name='SecurityInheritance_Update' )
	drop trigger SecurityInheritance_Update
if exists( select * from sys.triggers where parent_id = object_id('AclEntries') and name='Security_Membership_EnsureAllSubjectsExist' )
	drop trigger Security_Membership_EnsureAllSubjectsExist
go

create trigger SecurityInheritance_Update on SecurityInheritance instead of update as
	raiserror('Updates on the SecurityInheritance table are not supported. Use delete then insert.', 16, 1)
go

create trigger SecurityInheritance_Insert on SecurityInheritance after insert as
	if exists(select * from SecurityInheritanceClosure e 
              inner join inserted i 
              on e.[Start] = i.[End] and e.[End] = i.[Start] and i.[Start] <> i.[End])
    begin
		raiserror('Attempt to create a cycle', 16, 1)
		rollback tran
		return
    end

	declare c cursor local forward_only read_only for select [Start], [End] from inserted
	open c
	declare @s uniqueidentifier, @e uniqueidentifier
	fetch next from c into @s, @e

	while @@fetch_status = 0 begin
		declare @incomingClosures table( [Start] uniqueidentifier not null, [End] uniqueidentifier not null, [Distance] int not null )
	
		insert into @incomingClosures([Start],[End],[Distance]) values( @s, @e, case when @s = @e then 0 else 1 end )

		if @s <> @e begin
			insert into @incomingClosures([Start],[End],[Distance])
			select bf.[Start], af.[End], bf.[Distance] + af.[Distance] + 1
			from SecurityInheritanceClosure bf
			inner join SecurityInheritanceClosure af 
			on @e = bf.[End] and @e = af.[Start] and bf.[Start] <> bf.[End] and af.[Start] <> af.[End]
    
			insert into @incomingClosures([Start],[End],[Distance])
			select @s, af.[End], af.[Distance] + 1
			from SecurityInheritanceClosure af where @e = af.[Start] and af.[Start] <> af.[End]

			insert into @incomingClosures([Start],[End],[Distance])
			select bf.[Start], @e, bf.[Distance] + 1
			from SecurityInheritanceClosure bf where @s = bf.[End] and bf.[Start] <> bf.[End]
		end

		update SecurityInheritanceClosure set [Distance] = i.[Distance]
		from SecurityInheritanceClosure c
		inner join @incomingClosures i on c.[Start] = i.[Start] and c.[End] = i.[End] and c.[Distance] > i.[Distance]

		insert into SecurityInheritanceClosure([Start], [End], [Distance])
		select i.[Start], i.[End], i.[Distance] from @incomingClosures i
		left join SecurityInheritanceClosure ex on ex.[Start] = i.[Start] and ex.[End] = i.[End]
		where ex.[Start] is null

		fetch next from c into @s, @e
	end

	close c
	deallocate c
go

create trigger SecurityInheritance_Delete on SecurityInheritance after delete as
	-- Simply delete empty loops
	delete from SecurityInheritanceClosure
	where [Start] = [End] and [Start] in (select [Start] from deleted where [Start] = [End])

	-- For all non-loop edges, find alternative routes
	declare @alternativeRoutes table( [Start] uniqueidentifier not null, [End] uniqueidentifier not null, [Distance] int null )
	insert into @alternativeRoutes( [Start], [End], [Distance] )
	select d.[Start], d.[End], max( one.[Distance] + two.[Distance] )
	from deleted d
	left join SecurityInheritanceClosure one on d.[Start] = one.[Start] and d.[Start] <> d.[End]
	left join SecurityInheritanceClosure two on d.[End] = two.[End] and one.[End] = two.[Start]
	group by d.[Start], d.[End]

	delete from c
	from @alternativeRoutes a
	-- This trick is here in order to consider four possible paths: bf+a+af, a+af, bf+a, a
	-- for all possible combinations of "bf" and "af".
	-- Simply doing "left join" will not give us the latter three options unless there are no possible values for "bf" or "af".
	-- Therefore, we artificially fabricate a case when there are no possible values for "bf" by including value 1 for bb.x
	-- and a case with no values for "af" by including value 1 for aa.x
	cross apply ( select 0 as x union select 1 ) bb   
	cross apply ( select 0 as x union select 1 ) aa   
	left join SecurityInheritanceClosure bf on bf.[End] = a.[Start] and bb.x = 0
	left join SecurityInheritanceClosure af on af.[Start] = a.[End] and aa.x = 0
	inner join SecurityInheritanceClosure c on c.[Start] = isnull( bf.[Start], a.[Start] ) and c.[End] = isnull( af.[End], a.[End] ) and a.Distance is null

	update c set [Distance] = c.[Distance] + a.[Distance] - 1
	from @alternativeRoutes a
	cross apply ( select 0 as x union select 1 ) bb
	cross apply ( select 0 as x union select 1 ) aa
	left join SecurityInheritanceClosure bf on bf.[End] = a.[Start] and bb.x = 0
	left join SecurityInheritanceClosure af on af.[Start] = a.[End] and aa.x = 0
	inner join SecurityInheritanceClosure c on c.[Start] = isnull( bf.[Start], a.[Start] ) and c.[End] = isnull( af.[End], a.[End] ) and a.Distance is not null
go

declare @t table( s uniqueidentifier, e uniqueidentifier )

delete from SecurityMembershipClosure
insert into @t(s,e) select [Start], [End] from SecurityMembership
delete from SecurityMembership
insert into SecurityMembership([Start], [End]) select s, e from @t
delete from @t

delete from SecurityInheritanceClosure
insert into @t(s,e) select [Start], [End] from SecurityInheritance
delete from SecurityInheritance
insert into SecurityInheritance([Start], [End]) select s, e from @t
delete from @t
go

if exists( select * from sys.tables where object_id = object_id( 'SecurityObjects' ) ) begin
	delete from SecurityInheritance
	insert into SecurityInheritance([Start],[End]) select distinct [Id], [ParentId] from SecurityObjects where ParentId is not null
	insert into SecurityInheritance([Start],[End]) select distinct [Id], [Id] from SecurityObjects
	drop table SecurityObjects
end
go

	-------------------------------------------------------------------------------
	-- SecurityInheritanceClosure2 - an exact duplicate of the SecurityInheritanceClosure table
	-------------------------------------------------------------------------------
	create table SecurityInheritanceClosure2( [Start] uniqueidentifier not null, [End] uniqueidentifier not null, [Distance] int not null default 0 )
	go

	alter table SecurityInheritanceClosure2 add constraint SecurityInheritanceClosure2_PK primary key([Start],[End])
	go

	insert into SecurityInheritanceClosure2( [Start],[End],[Distance] ) select [Start],[End],[Distance] from SecurityInheritanceClosure
	go
	-------------------------------------------------------------------------------
	-- SecurityInheritanceClosure3 - an exact duplicate of the SecurityInheritanceClosure table
	-------------------------------------------------------------------------------
	create table SecurityInheritanceClosure3( [Start] uniqueidentifier not null, [End] uniqueidentifier not null, [Distance] int not null default 0 )
	go

	alter table SecurityInheritanceClosure3 add constraint SecurityInheritanceClosure3_PK primary key([Start],[End])
	go

	insert into SecurityInheritanceClosure3( [Start],[End],[Distance] ) select [Start],[End],[Distance] from SecurityInheritanceClosure
	go
	-------------------------------------------------------------------------------
	-- SecurityInheritanceClosure4 - an exact duplicate of the SecurityInheritanceClosure table
	-------------------------------------------------------------------------------
	create table SecurityInheritanceClosure4( [Start] uniqueidentifier not null, [End] uniqueidentifier not null, [Distance] int not null default 0 )
	go

	alter table SecurityInheritanceClosure4 add constraint SecurityInheritanceClosure4_PK primary key([Start],[End])
	go

	insert into SecurityInheritanceClosure4( [Start],[End],[Distance] ) select [Start],[End],[Distance] from SecurityInheritanceClosure
	go
	-------------------------------------------------------------------------------
	-- SecurityInheritanceClosure5 - an exact duplicate of the SecurityInheritanceClosure table
	-------------------------------------------------------------------------------
	create table SecurityInheritanceClosure5( [Start] uniqueidentifier not null, [End] uniqueidentifier not null, [Distance] int not null default 0 )
	go

	alter table SecurityInheritanceClosure5 add constraint SecurityInheritanceClosure5_PK primary key([Start],[End])
	go

	insert into SecurityInheritanceClosure5( [Start],[End],[Distance] ) select [Start],[End],[Distance] from SecurityInheritanceClosure
	go

create trigger SecurityInheritanceClosure_Copy_Insert on SecurityInheritanceClosure after insert as
			insert into SecurityInheritanceClosure2( [Start],[End],[Distance] ) select [Start], [End], [Distance] from inserted
			insert into SecurityInheritanceClosure3( [Start],[End],[Distance] ) select [Start], [End], [Distance] from inserted
			insert into SecurityInheritanceClosure4( [Start],[End],[Distance] ) select [Start], [End], [Distance] from inserted
			insert into SecurityInheritanceClosure5( [Start],[End],[Distance] ) select [Start], [End], [Distance] from inserted
	go
create trigger SecurityInheritanceClosure_Copy_Delete on SecurityInheritanceClosure after delete as
			delete from SecurityInheritanceClosure2 
		from SecurityInheritanceClosure2 c 
		inner join deleted d on c.[Start] = d.[Start] and c.[End] = d.[End]
			delete from SecurityInheritanceClosure3 
		from SecurityInheritanceClosure3 c 
		inner join deleted d on c.[Start] = d.[Start] and c.[End] = d.[End]
			delete from SecurityInheritanceClosure4 
		from SecurityInheritanceClosure4 c 
		inner join deleted d on c.[Start] = d.[Start] and c.[End] = d.[End]
			delete from SecurityInheritanceClosure5 
		from SecurityInheritanceClosure5 c 
		inner join deleted d on c.[Start] = d.[Start] and c.[End] = d.[End]
	go
create trigger SecurityInheritanceClosure_Copy_Update on SecurityInheritanceClosure after update as
			update SecurityInheritanceClosure2 set [Distance] = i.[Distance]
		from SecurityInheritanceClosure2 c 
		inner join inserted i on c.[Start] = i.[Start] and c.[End] = i.[End]
			update SecurityInheritanceClosure3 set [Distance] = i.[Distance]
		from SecurityInheritanceClosure3 c 
		inner join inserted i on c.[Start] = i.[Start] and c.[End] = i.[End]
			update SecurityInheritanceClosure4 set [Distance] = i.[Distance]
		from SecurityInheritanceClosure4 c 
		inner join inserted i on c.[Start] = i.[Start] and c.[End] = i.[End]
			update SecurityInheritanceClosure5 set [Distance] = i.[Distance]
		from SecurityInheritanceClosure5 c 
		inner join inserted i on c.[Start] = i.[Start] and c.[End] = i.[End]
	go


create view _Security_Tuple1
with schemabinding
as
	select 
		ih1.[Start] as Id1, 
		t.ObjectId1 as ParentId1,
		right( '00000' + convert( nvarchar(5), isnull( ih1.Distance, 0 ) ), 5 ) as DistancesJoined,
		e.PrivilegeId, m.[Start] as SubjectId,
		count_big(*) as TotalEntries, sum(1-e.Allow) as DeniedEntries,
		sum( case when m.[End] = m.[Start] and e.Allow = 0 then 1 else 0 end ) as ExplicitDeniedEntries,
		sum( case when m.[End] = m.[Start] and e.Allow = 1 then 1 else 0 end ) as ExplicitAllowedEntries,
		sum( case when m.[End] = m.[Start] then 1 else 0 end ) as NotViaMembership
	from dbo.AclEntries e
	inner join dbo.SecurityTargets t on e.TargetId = t.Id
	inner join dbo.SecurityMembershipClosure m on e.SubjectId = m.[End]
	inner join dbo.SecurityInheritanceClosure ih1 on ih1.[End] = t.ObjectId1
	where t.Arity = 1
	group by 
		ih1.[Start], 
		t.ObjectId1,
		right( '00000' + convert( nvarchar(5), isnull( ih1.Distance, 0 ) ), 5 ),
		e.PrivilegeId, m.[Start]
go

create unique clustered index _Security_Tuple1_PK on _Security_Tuple1(
	Id1, SubjectId, PrivilegeId,
	DistancesJoined
)
go

create view Security_Tuple1
as
select 
	Id1, ParentId1,
	SubjectId, PrivilegeId, 1-NotViaMembership as ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries
from _Security_Tuple1 x with(noexpand)
where x.DistancesJoined = (
	select min(y.DistancesJoined) from _Security_Tuple1 y with(noexpand)
	where x.SubjectId = y.SubjectId and x.PrivilegeId = y.PrivilegeId and x.Id1 = y.Id1
)
go


create view _Security_Tuple2
with schemabinding
as
	select 
		ih1.[Start] as Id1, ih2.[Start] as Id2, 
		t.ObjectId1 as ParentId1, t.ObjectId2 as ParentId2,
		right( '00000' + convert( nvarchar(5), isnull( ih1.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih2.Distance, 0 ) ), 5 ) as DistancesJoined,
		e.PrivilegeId, m.[Start] as SubjectId,
		count_big(*) as TotalEntries, sum(1-e.Allow) as DeniedEntries,
		sum( case when m.[End] = m.[Start] and e.Allow = 0 then 1 else 0 end ) as ExplicitDeniedEntries,
		sum( case when m.[End] = m.[Start] and e.Allow = 1 then 1 else 0 end ) as ExplicitAllowedEntries,
		sum( case when m.[End] = m.[Start] then 1 else 0 end ) as NotViaMembership
	from dbo.AclEntries e
	inner join dbo.SecurityTargets t on e.TargetId = t.Id
	inner join dbo.SecurityMembershipClosure m on e.SubjectId = m.[End]
	inner join dbo.SecurityInheritanceClosure ih1 on ih1.[End] = t.ObjectId1
inner join dbo.SecurityInheritanceClosure2 ih2 on ih2.[End] = t.ObjectId2
	where t.Arity = 2
	group by 
		ih1.[Start], ih2.[Start], 
		t.ObjectId1, t.ObjectId2,
		right( '00000' + convert( nvarchar(5), isnull( ih1.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih2.Distance, 0 ) ), 5 ),
		e.PrivilegeId, m.[Start]
go

create unique clustered index _Security_Tuple2_PK on _Security_Tuple2(
	Id1, Id2, SubjectId, PrivilegeId,
	DistancesJoined
)
go

create view Security_Tuple2
as
select 
	Id1, Id2, ParentId1, ParentId2,
	SubjectId, PrivilegeId, 1-NotViaMembership as ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries
from _Security_Tuple2 x with(noexpand)
where x.DistancesJoined = (
	select min(y.DistancesJoined) from _Security_Tuple2 y with(noexpand)
	where x.SubjectId = y.SubjectId and x.PrivilegeId = y.PrivilegeId and x.Id1 = y.Id1 and x.Id2 = y.Id2
)
go


create view _Security_Tuple3
with schemabinding
as
	select 
		ih1.[Start] as Id1, ih2.[Start] as Id2, ih3.[Start] as Id3, 
		t.ObjectId1 as ParentId1, t.ObjectId2 as ParentId2, t.ObjectId3 as ParentId3,
		right( '00000' + convert( nvarchar(5), isnull( ih1.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih2.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih3.Distance, 0 ) ), 5 ) as DistancesJoined,
		e.PrivilegeId, m.[Start] as SubjectId,
		count_big(*) as TotalEntries, sum(1-e.Allow) as DeniedEntries,
		sum( case when m.[End] = m.[Start] and e.Allow = 0 then 1 else 0 end ) as ExplicitDeniedEntries,
		sum( case when m.[End] = m.[Start] and e.Allow = 1 then 1 else 0 end ) as ExplicitAllowedEntries,
		sum( case when m.[End] = m.[Start] then 1 else 0 end ) as NotViaMembership
	from dbo.AclEntries e
	inner join dbo.SecurityTargets t on e.TargetId = t.Id
	inner join dbo.SecurityMembershipClosure m on e.SubjectId = m.[End]
	inner join dbo.SecurityInheritanceClosure ih1 on ih1.[End] = t.ObjectId1
inner join dbo.SecurityInheritanceClosure2 ih2 on ih2.[End] = t.ObjectId2
inner join dbo.SecurityInheritanceClosure3 ih3 on ih3.[End] = t.ObjectId3
	where t.Arity = 3
	group by 
		ih1.[Start], ih2.[Start], ih3.[Start], 
		t.ObjectId1, t.ObjectId2, t.ObjectId3,
		right( '00000' + convert( nvarchar(5), isnull( ih1.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih2.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih3.Distance, 0 ) ), 5 ),
		e.PrivilegeId, m.[Start]
go

create unique clustered index _Security_Tuple3_PK on _Security_Tuple3(
	Id1, Id2, Id3, SubjectId, PrivilegeId,
	DistancesJoined
)
go

create view Security_Tuple3
as
select 
	Id1, Id2, Id3, ParentId1, ParentId2, ParentId3,
	SubjectId, PrivilegeId, 1-NotViaMembership as ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries
from _Security_Tuple3 x with(noexpand)
where x.DistancesJoined = (
	select min(y.DistancesJoined) from _Security_Tuple3 y with(noexpand)
	where x.SubjectId = y.SubjectId and x.PrivilegeId = y.PrivilegeId and x.Id1 = y.Id1 and x.Id2 = y.Id2 and x.Id3 = y.Id3
)
go


create view _Security_Tuple4
with schemabinding
as
	select 
		ih1.[Start] as Id1, ih2.[Start] as Id2, ih3.[Start] as Id3, ih4.[Start] as Id4, 
		t.ObjectId1 as ParentId1, t.ObjectId2 as ParentId2, t.ObjectId3 as ParentId3, t.ObjectId4 as ParentId4,
		right( '00000' + convert( nvarchar(5), isnull( ih1.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih2.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih3.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih4.Distance, 0 ) ), 5 ) as DistancesJoined,
		e.PrivilegeId, m.[Start] as SubjectId,
		count_big(*) as TotalEntries, sum(1-e.Allow) as DeniedEntries,
		sum( case when m.[End] = m.[Start] and e.Allow = 0 then 1 else 0 end ) as ExplicitDeniedEntries,
		sum( case when m.[End] = m.[Start] and e.Allow = 1 then 1 else 0 end ) as ExplicitAllowedEntries,
		sum( case when m.[End] = m.[Start] then 1 else 0 end ) as NotViaMembership
	from dbo.AclEntries e
	inner join dbo.SecurityTargets t on e.TargetId = t.Id
	inner join dbo.SecurityMembershipClosure m on e.SubjectId = m.[End]
	inner join dbo.SecurityInheritanceClosure ih1 on ih1.[End] = t.ObjectId1
inner join dbo.SecurityInheritanceClosure2 ih2 on ih2.[End] = t.ObjectId2
inner join dbo.SecurityInheritanceClosure3 ih3 on ih3.[End] = t.ObjectId3
inner join dbo.SecurityInheritanceClosure4 ih4 on ih4.[End] = t.ObjectId4
	where t.Arity = 4
	group by 
		ih1.[Start], ih2.[Start], ih3.[Start], ih4.[Start], 
		t.ObjectId1, t.ObjectId2, t.ObjectId3, t.ObjectId4,
		right( '00000' + convert( nvarchar(5), isnull( ih1.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih2.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih3.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih4.Distance, 0 ) ), 5 ),
		e.PrivilegeId, m.[Start]
go

create unique clustered index _Security_Tuple4_PK on _Security_Tuple4(
	Id1, Id2, Id3, Id4, SubjectId, PrivilegeId,
	DistancesJoined
)
go

create view Security_Tuple4
as
select 
	Id1, Id2, Id3, Id4, ParentId1, ParentId2, ParentId3, ParentId4,
	SubjectId, PrivilegeId, 1-NotViaMembership as ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries
from _Security_Tuple4 x with(noexpand)
where x.DistancesJoined = (
	select min(y.DistancesJoined) from _Security_Tuple4 y with(noexpand)
	where x.SubjectId = y.SubjectId and x.PrivilegeId = y.PrivilegeId and x.Id1 = y.Id1 and x.Id2 = y.Id2 and x.Id3 = y.Id3 and x.Id4 = y.Id4
)
go


create view _Security_Tuple5
with schemabinding
as
	select 
		ih1.[Start] as Id1, ih2.[Start] as Id2, ih3.[Start] as Id3, ih4.[Start] as Id4, ih5.[Start] as Id5, 
		t.ObjectId1 as ParentId1, t.ObjectId2 as ParentId2, t.ObjectId3 as ParentId3, t.ObjectId4 as ParentId4, t.ObjectId5 as ParentId5,
		right( '00000' + convert( nvarchar(5), isnull( ih1.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih2.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih3.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih4.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih5.Distance, 0 ) ), 5 ) as DistancesJoined,
		e.PrivilegeId, m.[Start] as SubjectId,
		count_big(*) as TotalEntries, sum(1-e.Allow) as DeniedEntries,
		sum( case when m.[End] = m.[Start] and e.Allow = 0 then 1 else 0 end ) as ExplicitDeniedEntries,
		sum( case when m.[End] = m.[Start] and e.Allow = 1 then 1 else 0 end ) as ExplicitAllowedEntries,
		sum( case when m.[End] = m.[Start] then 1 else 0 end ) as NotViaMembership
	from dbo.AclEntries e
	inner join dbo.SecurityTargets t on e.TargetId = t.Id
	inner join dbo.SecurityMembershipClosure m on e.SubjectId = m.[End]
	inner join dbo.SecurityInheritanceClosure ih1 on ih1.[End] = t.ObjectId1
inner join dbo.SecurityInheritanceClosure2 ih2 on ih2.[End] = t.ObjectId2
inner join dbo.SecurityInheritanceClosure3 ih3 on ih3.[End] = t.ObjectId3
inner join dbo.SecurityInheritanceClosure4 ih4 on ih4.[End] = t.ObjectId4
inner join dbo.SecurityInheritanceClosure5 ih5 on ih5.[End] = t.ObjectId5
	where t.Arity = 5
	group by 
		ih1.[Start], ih2.[Start], ih3.[Start], ih4.[Start], ih5.[Start], 
		t.ObjectId1, t.ObjectId2, t.ObjectId3, t.ObjectId4, t.ObjectId5,
		right( '00000' + convert( nvarchar(5), isnull( ih1.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih2.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih3.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih4.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih5.Distance, 0 ) ), 5 ),
		e.PrivilegeId, m.[Start]
go

create unique clustered index _Security_Tuple5_PK on _Security_Tuple5(
	Id1, Id2, Id3, Id4, Id5, SubjectId, PrivilegeId,
	DistancesJoined
)
go

create view Security_Tuple5
as
select 
	Id1, Id2, Id3, Id4, Id5, ParentId1, ParentId2, ParentId3, ParentId4, ParentId5,
	SubjectId, PrivilegeId, 1-NotViaMembership as ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries
from _Security_Tuple5 x with(noexpand)
where x.DistancesJoined = (
	select min(y.DistancesJoined) from _Security_Tuple5 y with(noexpand)
	where x.SubjectId = y.SubjectId and x.PrivilegeId = y.PrivilegeId and x.Id1 = y.Id1 and x.Id2 = y.Id2 and x.Id3 = y.Id3 and x.Id4 = y.Id4 and x.Id5 = y.Id5
)
go


create trigger SecurityTargets_MakeSureObjectsExist on SecurityTargets after insert, update
as
	declare @empty uniqueidentifier
	set @empty = '00000000-0000-0000-0000-000000000000'

			insert into SecurityInheritance(Start,[End]) 
		select distinct ObjectId1, ObjectId1 from inserted 
		left join SecurityInheritance ih on [Start] = ObjectId1 and [End] = ObjectId1
		where ih.[Start] is null and ObjectId1 <> @empty
			insert into SecurityInheritance(Start,[End]) 
		select distinct ObjectId2, ObjectId2 from inserted 
		left join SecurityInheritance ih on [Start] = ObjectId2 and [End] = ObjectId2
		where ih.[Start] is null and ObjectId2 <> @empty
			insert into SecurityInheritance(Start,[End]) 
		select distinct ObjectId3, ObjectId3 from inserted 
		left join SecurityInheritance ih on [Start] = ObjectId3 and [End] = ObjectId3
		where ih.[Start] is null and ObjectId3 <> @empty
			insert into SecurityInheritance(Start,[End]) 
		select distinct ObjectId4, ObjectId4 from inserted 
		left join SecurityInheritance ih on [Start] = ObjectId4 and [End] = ObjectId4
		where ih.[Start] is null and ObjectId4 <> @empty
			insert into SecurityInheritance(Start,[End]) 
		select distinct ObjectId5, ObjectId5 from inserted 
		left join SecurityInheritance ih on [Start] = ObjectId5 and [End] = ObjectId5
		where ih.[Start] is null and ObjectId5 <> @empty
	go