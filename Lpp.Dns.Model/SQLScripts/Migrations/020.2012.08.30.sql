if exists( select * from sys.triggers where name = 'SecurityObjects_Insert' and parent_id = object_id( 'SecurityObjects' ) )
	drop trigger SecurityObjects_Insert
if exists( select * from sys.triggers where name = 'SecurityObjects_Update' and parent_id = object_id( 'SecurityObjects' ) )
	drop trigger SecurityObjects_Update
if exists( select * from sys.triggers where name = 'SecurityObjects_Delete' and parent_id = object_id( 'SecurityObjects' ) )
	drop trigger SecurityObjects_Delete
if exists( select * from sys.triggers where name = 'SecurityObjects_Copy_Insert' and parent_id = object_id( 'SecurityObjects' ) )
	drop trigger SecurityObjects_Copy_Insert
if exists( select * from sys.triggers where name = 'SecurityObjects_Copy_Update' and parent_id = object_id( 'SecurityObjects' ) )
	drop trigger SecurityObjects_Copy_Update
if exists( select * from sys.triggers where name = 'SecurityObjects_Copy_Delete' and parent_id = object_id( 'SecurityObjects' ) )
	drop trigger SecurityObjects_Copy_Delete
if exists( select * from sys.procedures where name = 'SecurityObjects_InitializeIndexesForInserted' )
	drop proc SecurityObjects_InitializeIndexesForInserted
if exists( select * from sys.procedures where name = 'SecurityObjects_NullOutHive' )
	drop proc SecurityObjects_NullOutHive
if exists( select * from sys.procedures where name = 'SecurityObjects_ShiftIndexesAfterHiveDeletion' )
	drop proc SecurityObjects_ShiftIndexesAfterHiveDeletion
go

CREATE procedure [SecurityObjects_InitializeIndexesForInserted] as
	-- Repeat the following until all inserted nodes are assigned a tree tag and indexes
	while 1 = 1 begin

		-- Get one object that has a child which is not assigned a tree yet
		declare @parentId uniqueidentifier, @parentTree uniqueidentifier, @parentRightIndex int; set @parentId = null
		select top 1 @parentId = p.Id, @parentTree = p.TreeTag, @parentRightIndex = p.RightIndex
		from SecurityObjects i inner join SecurityObjects p on i.ParentId = p.Id
		where i.TreeTag is null and p.TreeTag is not null

		if @parentId is null return -- Didn't find any such objects => the end

		-- "shift" right indexes of all nodes to the right of the current parent (including the parent itself)
		declare @childrenCount int
		select @childrenCount = COUNT(*) from SecurityObjects where ParentId = @parentId and TreeTag is null
		update SecurityObjects set 
			RightIndex = RightIndex + @childrenCount*2,
			LeftIndex = case when LeftIndex >= @parentRightIndex then LeftIndex + @childrenCount*2 else LeftIndex end
		where RightIndex >= @parentRightIndex and TreeTag = @parentTree

		-- Set indexes and tree tag for all just-inserted children of the current parent
		update SecurityObjects
		set LeftIndex = @parentRightIndex + [index]*2, RightIndex = @parentRightIndex + [index]*2 + 1, TreeTag = @parentTree
		from (
			select Id, (row_number() over (order by Id)) - 1 as [index]
			from SecurityObjects
			where ParentId = @parentId and TreeTag is null
		) i
		where i.Id = SecurityObjects.Id
	end
GO

create procedure [SecurityObjects_NullOutHive]
	@tree uniqueidentifier,
	@li int, @ri int
as
	update SecurityObjects set TreeTag = null
	where TreeTag = @tree and LeftIndex between @li and @ri
GO

create procedure [SecurityObjects_ShiftIndexesAfterHiveDeletion] 
	@tree uniqueidentifier,
	@li int, @ri int
as
	-- "Shift" indexes of all nodes to the right of this deleted node
	update SecurityObjects 
	set 
		RightIndex = RightIndex - (@ri-@li+1),
		LeftIndex = case when LeftIndex < @li then LeftIndex else LeftIndex - (@ri-@li+1) end
	where RightIndex > @ri and TreeTag = @tree
GO

CREATE trigger [SecurityObjects_Update] on [SecurityObjects] after update as
	if not update(ParentId) return -- This should prevent recursion
	declare @dummy uniqueidentifier; set @dummy = [dbo].[NewSqlGuid]()

	declare c cursor for 
		select i.Id, i.ParentId, d.ParentId, d.LeftIndex, d.RightIndex, d.TreeTag
		from inserted i inner join deleted d on i.Id = d.Id
		where isnull( i.ParentId, @dummy ) <> isnull( d.ParentId, @dummy )
	open c

	declare @id uniqueidentifier, @newParentId uniqueidentifier, @oldParentId uniqueidentifier, @li int, @ri int, @tree uniqueidentifier
	fetch next from c into @id, @newParentId, @oldParentId, @li, @ri, @tree
	while @@fetch_status = 0 begin

		declare @oldTree uniqueidentifier; select @oldTree = TreeTag from SecurityObjects where Id = @oldParentId
		declare @newTree uniqueidentifier, @newParentRi int
		select @newTree = TreeTag, @newParentRi = RightIndex from SecurityObjects where Id = @newParentId

		if @newParentId is null -- The object has been moved out of a tree to become its own tree
		begin
			set @newTree = [dbo].[NewSqlGuid]()
			update SecurityObjects set TreeTag = @newTree, LeftIndex = LeftIndex - @li, RightIndex = RightIndex - @li
			where TreeTag = @tree and LeftIndex between @li and @ri

			update SecurityObjects set 
				LeftIndex = case when LeftIndex >= @ri then LeftIndex - (@ri-@li+1) else LeftIndex end,
				RightIndex = RightIndex - (@ri-@li+1)
			where RightIndex > @ri and TreeTag = @tree

		end else begin 
			-- The object has been moved into a tree OR
		    -- The object has been moved between trees OR
		    -- The object has been moved to a new location within the same tree
			exec SecurityObjects_NullOutHive @tree, @li, @ri
			if @oldParentId is not null exec SecurityObjects_ShiftIndexesAfterHiveDeletion @oldTree, @li, @ri
			exec SecurityObjects_InitializeIndexesForInserted

		end

		fetch next from c into @id, @newParentId, @oldParentId, @li, @ri, @tree
	end
	
	close c
	deallocate c
GO

create trigger [SecurityObjects_Insert] on [SecurityObjects] after insert as

	update SecurityObjects set LeftIndex = 0, RightIndex = 1, TreeTag = [dbo].[NewSqlGuid]()
	where Id in (select Id from inserted) and ParentId is null

	update SecurityObjects set TreeTag = null
	where Id in (select Id from inserted) and ParentId is not null

	exec SecurityObjects_InitializeIndexesForInserted
GO

create trigger [SecurityObjects_Delete] on [SecurityObjects] after delete as
	declare c cursor for select Id, RightIndex, LeftIndex, TreeTag from deleted
	open c

	declare @id uniqueidentifier, @ri int, @li int, @tree uniqueidentifier
	fetch next from c into @id, @ri, @li, @tree
	while @@fetch_status = 0 begin

		exec SecurityObjects_ShiftIndexesAfterHiveDeletion @tree, @li, @ri

		-- In case the deleted node had any children, make each of them a new hive with new tag
		-- and shift their indexes to make them start at zero
		update SecurityObjects 
		set TreeTag = newTree, LeftIndex = LeftIndex - c.diff, RightIndex = RightIndex - c.diff
		from (
			select c.Id, p.newTree, p.LeftIndex-1 as diff
			from SecurityObjects c
			inner join ( select *, [dbo].[NewSqlGuid]() as newTree from SecurityObjects p where p.ParentId = @id ) p
			on c.TreeTag = @tree and c.LeftIndex between p.LeftIndex and p.RightIndex
		) c
		where c.Id = SecurityObjects.Id

		fetch next from c into @id, @ri, @li, @tree
	end

	close c
	deallocate c
GO

create trigger SecurityObjects_Copy_Insert on SecurityObjects after insert as
	insert into SecurityObjects2( Id, LeftIndex, RightIndex, TreeTag ) select o.Id, o.LeftIndex, o.RightIndex, o.TreeTag 
	from securityobjects o inner join inserted i on o.Id = i.Id
go
create trigger SecurityObjects_Copy_Delete on SecurityObjects after delete as
	delete from SecurityObjects2 where Id in ( select id from deleted )
go
create trigger SecurityObjects_Copy_Update on SecurityObjects after update as
	update SecurityObjects2 set LeftIndex = o.LeftIndex, RightIndex = o.RightIndex, TreeTag = o.TreeTag 
	from SecurityObjects2 o2 
	inner join SecurityObjects o on o2.id = o.id
	inner join inserted i on i.Id = o.Id
go