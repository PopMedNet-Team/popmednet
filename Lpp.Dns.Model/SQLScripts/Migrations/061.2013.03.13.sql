if exists( select * from sys.triggers where parent_id = object_id('SecurityMembership') and name='SecurityMembership_Delete' )
	drop trigger SecurityMembership_Delete
if exists( select * from sys.triggers where parent_id = object_id('SecurityInheritance') and name='SecurityInheritance_Delete' )
	drop trigger SecurityInheritance_Delete
go

create trigger SecurityMembership_Delete on SecurityMembership after delete as
	-- Simply delete empty loops
	delete from SecurityMembershipClosure
	where [Start] = [End] and [Start] in (select [Start] from deleted where [Start] = [End])

	-- For all non-loop edges, find alternative routes
	declare @alternativeRoutes table( [Start] uniqueidentifier not null, [End] uniqueidentifier not null, [OriginalDistance] int not null, [AlternativeDistance] int null )
	insert into @alternativeRoutes( [Start], [End], [OriginalDistance], [AlternativeDistance] )
	select d.[Start], d.[End], 1, max( one.[Distance] + two.[Distance] )
	from deleted d
	left join SecurityMembershipClosure one on d.[Start] = one.[Start] and d.[Start] <> d.[End]
	left join SecurityMembershipClosure two on d.[End] = two.[End] and one.[End] = two.[Start]
	and one.[Start] <> one.[End] and two.[Start] <> two.[End]
	group by d.[Start], d.[End]

	delete c
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
	inner join SecurityMembershipClosure c 
		on c.[Start] = isnull( bf.[Start], a.[Start] ) and c.[End] = isnull( af.[End], a.[End] ) 
		and c.Distance = a.OriginalDistance + isnull( bf.Distance, 0 ) + isnull( af.Distance, 0 )
		and a.AlternativeDistance is null

	update c set [Distance] = c.[Distance] + a.[AlternativeDistance] - 1
	from @alternativeRoutes a
	cross apply ( select 0 as x union select 1 ) bb
	cross apply ( select 0 as x union select 1 ) aa
	left join SecurityMembershipClosure bf on bf.[End] = a.[Start] and bb.x = 0
	left join SecurityMembershipClosure af on af.[Start] = a.[End] and aa.x = 0
	inner join SecurityMembershipClosure c 
		on c.[Start] = isnull( bf.[Start], a.[Start] ) and c.[End] = isnull( af.[End], a.[End] ) 
		and c.Distance = a.OriginalDistance + isnull( bf.Distance, 0 ) + isnull( af.Distance, 0 )
		and a.AlternativeDistance is not null
go

create trigger SecurityInheritance_Delete on SecurityInheritance after delete as
	-- Simply delete empty loops
	delete from SecurityInheritanceClosure
	where [Start] = [End] and [Start] in (select [Start] from deleted where [Start] = [End])

	-- For all non-loop edges, find alternative routes
	declare @alternativeRoutes table( [Start] uniqueidentifier not null, [End] uniqueidentifier not null, [OriginalDistance] int not null, [AlternativeDistance] int null )
	insert into @alternativeRoutes( [Start], [End], [OriginalDistance], [AlternativeDistance] )
	select d.[Start], d.[End], 1, max( one.[Distance] + two.[Distance] )
	from deleted d
	left join SecurityInheritanceClosure one on d.[Start] = one.[Start] and d.[Start] <> d.[End]
	left join SecurityInheritanceClosure two on d.[End] = two.[End] and one.[End] = two.[Start]
	and one.[Start] <> one.[End] and two.[Start] <> two.[End]
	group by d.[Start], d.[End]

	delete c
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
	inner join SecurityInheritanceClosure c 
		on c.[Start] = isnull( bf.[Start], a.[Start] ) and c.[End] = isnull( af.[End], a.[End] ) 
		and c.Distance = a.OriginalDistance + isnull( bf.Distance, 0 ) + isnull( af.Distance, 0 )
		and a.AlternativeDistance is null

	update c set [Distance] = c.[Distance] + a.[AlternativeDistance] - 1
	from @alternativeRoutes a
	cross apply ( select 0 as x union select 1 ) bb
	cross apply ( select 0 as x union select 1 ) aa
	left join SecurityInheritanceClosure bf on bf.[End] = a.[Start] and bb.x = 0
	left join SecurityInheritanceClosure af on af.[Start] = a.[End] and aa.x = 0
	inner join SecurityInheritanceClosure c 
		on c.[Start] = isnull( bf.[Start], a.[Start] ) and c.[End] = isnull( af.[End], a.[End] ) 
		and c.Distance = a.OriginalDistance + isnull( bf.Distance, 0 ) + isnull( af.Distance, 0 )
		and a.AlternativeDistance is not null
go