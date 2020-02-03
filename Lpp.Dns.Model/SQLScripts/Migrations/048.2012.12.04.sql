if exists( select * from sys.triggers where parent_id = object_id('SecurityMembership') and name='SecurityMembership_Delete' )
	drop trigger SecurityMembership_Delete
if exists( select * from sys.triggers where parent_id = object_id('SecurityMembership') and name='SecurityMembership_Insert' )
	drop trigger SecurityMembership_Insert
if exists( select * from sys.triggers where parent_id = object_id('SecurityInheritance') and name='SecurityInheritance_Insert' )
	drop trigger SecurityInheritance_Insert
if exists( select * from sys.triggers where parent_id = object_id('SecurityInheritance') and name='SecurityInheritance_Delete' )
	drop trigger SecurityInheritance_Delete
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
			on @s = bf.[End] and @e = af.[Start] and bf.[Start] <> bf.[End] and af.[Start] <> af.[End]
    
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
	and one.[Start] <> one.[End] and two.[Start] <> two.[End]
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
			on @s = bf.[End] and @e = af.[Start] and bf.[Start] <> bf.[End] and af.[Start] <> af.[End]
    
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
	and one.[Start] <> one.[End] and two.[Start] <> two.[End]
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

-- This little snippet is "CLEANUP". It clears DAG closure tables, then repopulates them from the original tables, thus bringing them back into sync.
-- This script is here because a bug was found in the "delete" trigger. That bug would cause the closure tables to come out of sync with the
-- original tables. The fixed versions of the triggers are above.

declare @t table( [Start] uniqueidentifier, [End] uniqueidentifier )

delete @t
insert into @t([Start], [End]) select [Start], [End] from SecurityMembership
delete SecurityMembership
delete SecurityMembershipClosure
insert into SecurityMembership([Start], [End]) select [Start], [End] from @t

delete @t
insert into @t([Start], [End]) select [Start], [End] from SecurityInheritance
delete SecurityInheritance
delete SecurityInheritanceClosure
insert into SecurityInheritance([Start], [End]) select [Start], [End] from @t