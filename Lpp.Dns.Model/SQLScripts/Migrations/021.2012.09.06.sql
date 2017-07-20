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

	-------------------------------------------------------------------------------
	-- SecurityObjects1_p - an exact duplicate of the SecurityObjects table
	-------------------------------------------------------------------------------
	create table SecurityObjects1_p( Id uniqueidentifier not null, LeftIndex int, RightIndex int, TreeTag uniqueidentifier )
	go

	create clustered index securityobjects1_p_TreeTag on securityobjects1_p(TreeTag, LeftIndex desc)
	create unique index securityobjects1_p_PK_IX on securityobjects1_p(Id)
	go

	alter table SecurityObjects1_p add constraint SecurityObjects1_p_PK primary key(Id)
	go

	insert into SecurityObjects1_p( Id, LeftIndex, RightIndex, TreeTag ) select Id, LeftIndex, RightIndex, TreeTag from SecurityObjects
	go
	-------------------------------------------------------------------------------
	-- SecurityObjects2_p - an exact duplicate of the SecurityObjects table
	-------------------------------------------------------------------------------
	create table SecurityObjects2_p( Id uniqueidentifier not null, LeftIndex int, RightIndex int, TreeTag uniqueidentifier )
	go

	create clustered index securityobjects2_p_TreeTag on securityobjects2_p(TreeTag, LeftIndex desc)
	create unique index securityobjects2_p_PK_IX on securityobjects2_p(Id)
	go

	alter table SecurityObjects2_p add constraint SecurityObjects2_p_PK primary key(Id)
	go

	insert into SecurityObjects2_p( Id, LeftIndex, RightIndex, TreeTag ) select Id, LeftIndex, RightIndex, TreeTag from SecurityObjects
	go
	-------------------------------------------------------------------------------
	-- SecurityObjects2_c - an exact duplicate of the SecurityObjects table
	-------------------------------------------------------------------------------
	create table SecurityObjects2_c( Id uniqueidentifier not null, LeftIndex int, RightIndex int, TreeTag uniqueidentifier )
	go

	create clustered index securityobjects2_c_TreeTag on securityobjects2_c(TreeTag, LeftIndex desc)
	create unique index securityobjects2_c_PK_IX on securityobjects2_c(Id)
	go

	alter table SecurityObjects2_c add constraint SecurityObjects2_c_PK primary key(Id)
	go

	insert into SecurityObjects2_c( Id, LeftIndex, RightIndex, TreeTag ) select Id, LeftIndex, RightIndex, TreeTag from SecurityObjects
	go
	-------------------------------------------------------------------------------
	-- SecurityObjects3_p - an exact duplicate of the SecurityObjects table
	-------------------------------------------------------------------------------
	create table SecurityObjects3_p( Id uniqueidentifier not null, LeftIndex int, RightIndex int, TreeTag uniqueidentifier )
	go

	create clustered index securityobjects3_p_TreeTag on securityobjects3_p(TreeTag, LeftIndex desc)
	create unique index securityobjects3_p_PK_IX on securityobjects3_p(Id)
	go

	alter table SecurityObjects3_p add constraint SecurityObjects3_p_PK primary key(Id)
	go

	insert into SecurityObjects3_p( Id, LeftIndex, RightIndex, TreeTag ) select Id, LeftIndex, RightIndex, TreeTag from SecurityObjects
	go
	-------------------------------------------------------------------------------
	-- SecurityObjects3_c - an exact duplicate of the SecurityObjects table
	-------------------------------------------------------------------------------
	create table SecurityObjects3_c( Id uniqueidentifier not null, LeftIndex int, RightIndex int, TreeTag uniqueidentifier )
	go

	create clustered index securityobjects3_c_TreeTag on securityobjects3_c(TreeTag, LeftIndex desc)
	create unique index securityobjects3_c_PK_IX on securityobjects3_c(Id)
	go

	alter table SecurityObjects3_c add constraint SecurityObjects3_c_PK primary key(Id)
	go

	insert into SecurityObjects3_c( Id, LeftIndex, RightIndex, TreeTag ) select Id, LeftIndex, RightIndex, TreeTag from SecurityObjects
	go
	-------------------------------------------------------------------------------
	-- SecurityObjects4_p - an exact duplicate of the SecurityObjects table
	-------------------------------------------------------------------------------
	create table SecurityObjects4_p( Id uniqueidentifier not null, LeftIndex int, RightIndex int, TreeTag uniqueidentifier )
	go

	create clustered index securityobjects4_p_TreeTag on securityobjects4_p(TreeTag, LeftIndex desc)
	create unique index securityobjects4_p_PK_IX on securityobjects4_p(Id)
	go

	alter table SecurityObjects4_p add constraint SecurityObjects4_p_PK primary key(Id)
	go

	insert into SecurityObjects4_p( Id, LeftIndex, RightIndex, TreeTag ) select Id, LeftIndex, RightIndex, TreeTag from SecurityObjects
	go
	-------------------------------------------------------------------------------
	-- SecurityObjects4_c - an exact duplicate of the SecurityObjects table
	-------------------------------------------------------------------------------
	create table SecurityObjects4_c( Id uniqueidentifier not null, LeftIndex int, RightIndex int, TreeTag uniqueidentifier )
	go

	create clustered index securityobjects4_c_TreeTag on securityobjects4_c(TreeTag, LeftIndex desc)
	create unique index securityobjects4_c_PK_IX on securityobjects4_c(Id)
	go

	alter table SecurityObjects4_c add constraint SecurityObjects4_c_PK primary key(Id)
	go

	insert into SecurityObjects4_c( Id, LeftIndex, RightIndex, TreeTag ) select Id, LeftIndex, RightIndex, TreeTag from SecurityObjects
	go
	-------------------------------------------------------------------------------
	-- SecurityObjects5_p - an exact duplicate of the SecurityObjects table
	-------------------------------------------------------------------------------
	create table SecurityObjects5_p( Id uniqueidentifier not null, LeftIndex int, RightIndex int, TreeTag uniqueidentifier )
	go

	create clustered index securityobjects5_p_TreeTag on securityobjects5_p(TreeTag, LeftIndex desc)
	create unique index securityobjects5_p_PK_IX on securityobjects5_p(Id)
	go

	alter table SecurityObjects5_p add constraint SecurityObjects5_p_PK primary key(Id)
	go

	insert into SecurityObjects5_p( Id, LeftIndex, RightIndex, TreeTag ) select Id, LeftIndex, RightIndex, TreeTag from SecurityObjects
	go
	-------------------------------------------------------------------------------
	-- SecurityObjects5_c - an exact duplicate of the SecurityObjects table
	-------------------------------------------------------------------------------
	create table SecurityObjects5_c( Id uniqueidentifier not null, LeftIndex int, RightIndex int, TreeTag uniqueidentifier )
	go

	create clustered index securityobjects5_c_TreeTag on securityobjects5_c(TreeTag, LeftIndex desc)
	create unique index securityobjects5_c_PK_IX on securityobjects5_c(Id)
	go

	alter table SecurityObjects5_c add constraint SecurityObjects5_c_PK primary key(Id)
	go

	insert into SecurityObjects5_c( Id, LeftIndex, RightIndex, TreeTag ) select Id, LeftIndex, RightIndex, TreeTag from SecurityObjects
	go

create trigger SecurityObjects_Copy_Insert on SecurityObjects after insert as
			insert into SecurityObjects1_p( Id, LeftIndex, RightIndex, TreeTag ) select o.Id, o.LeftIndex, o.RightIndex, o.TreeTag 
		from securityobjects o inner join inserted i on o.Id = i.Id
			insert into SecurityObjects2_p( Id, LeftIndex, RightIndex, TreeTag ) select o.Id, o.LeftIndex, o.RightIndex, o.TreeTag 
		from securityobjects o inner join inserted i on o.Id = i.Id
			insert into SecurityObjects2_c( Id, LeftIndex, RightIndex, TreeTag ) select o.Id, o.LeftIndex, o.RightIndex, o.TreeTag 
		from securityobjects o inner join inserted i on o.Id = i.Id
			insert into SecurityObjects3_p( Id, LeftIndex, RightIndex, TreeTag ) select o.Id, o.LeftIndex, o.RightIndex, o.TreeTag 
		from securityobjects o inner join inserted i on o.Id = i.Id
			insert into SecurityObjects3_c( Id, LeftIndex, RightIndex, TreeTag ) select o.Id, o.LeftIndex, o.RightIndex, o.TreeTag 
		from securityobjects o inner join inserted i on o.Id = i.Id
			insert into SecurityObjects4_p( Id, LeftIndex, RightIndex, TreeTag ) select o.Id, o.LeftIndex, o.RightIndex, o.TreeTag 
		from securityobjects o inner join inserted i on o.Id = i.Id
			insert into SecurityObjects4_c( Id, LeftIndex, RightIndex, TreeTag ) select o.Id, o.LeftIndex, o.RightIndex, o.TreeTag 
		from securityobjects o inner join inserted i on o.Id = i.Id
			insert into SecurityObjects5_p( Id, LeftIndex, RightIndex, TreeTag ) select o.Id, o.LeftIndex, o.RightIndex, o.TreeTag 
		from securityobjects o inner join inserted i on o.Id = i.Id
			insert into SecurityObjects5_c( Id, LeftIndex, RightIndex, TreeTag ) select o.Id, o.LeftIndex, o.RightIndex, o.TreeTag 
		from securityobjects o inner join inserted i on o.Id = i.Id
	go
create trigger SecurityObjects_Copy_Delete on SecurityObjects after delete as
			delete from SecurityObjects1_p where Id in ( select id from deleted )
			delete from SecurityObjects2_p where Id in ( select id from deleted )
			delete from SecurityObjects2_c where Id in ( select id from deleted )
			delete from SecurityObjects3_p where Id in ( select id from deleted )
			delete from SecurityObjects3_c where Id in ( select id from deleted )
			delete from SecurityObjects4_p where Id in ( select id from deleted )
			delete from SecurityObjects4_c where Id in ( select id from deleted )
			delete from SecurityObjects5_p where Id in ( select id from deleted )
			delete from SecurityObjects5_c where Id in ( select id from deleted )
	go
create trigger SecurityObjects_Copy_Update on SecurityObjects after update as
			update SecurityObjects1_p set LeftIndex = o.LeftIndex, RightIndex = o.RightIndex, TreeTag = o.TreeTag 
		from SecurityObjects1_p o2 
		inner join SecurityObjects o on o2.id = o.id
		inner join inserted i on i.Id = o.Id
			update SecurityObjects2_p set LeftIndex = o.LeftIndex, RightIndex = o.RightIndex, TreeTag = o.TreeTag 
		from SecurityObjects2_p o2 
		inner join SecurityObjects o on o2.id = o.id
		inner join inserted i on i.Id = o.Id
			update SecurityObjects2_c set LeftIndex = o.LeftIndex, RightIndex = o.RightIndex, TreeTag = o.TreeTag 
		from SecurityObjects2_c o2 
		inner join SecurityObjects o on o2.id = o.id
		inner join inserted i on i.Id = o.Id
			update SecurityObjects3_p set LeftIndex = o.LeftIndex, RightIndex = o.RightIndex, TreeTag = o.TreeTag 
		from SecurityObjects3_p o2 
		inner join SecurityObjects o on o2.id = o.id
		inner join inserted i on i.Id = o.Id
			update SecurityObjects3_c set LeftIndex = o.LeftIndex, RightIndex = o.RightIndex, TreeTag = o.TreeTag 
		from SecurityObjects3_c o2 
		inner join SecurityObjects o on o2.id = o.id
		inner join inserted i on i.Id = o.Id
			update SecurityObjects4_p set LeftIndex = o.LeftIndex, RightIndex = o.RightIndex, TreeTag = o.TreeTag 
		from SecurityObjects4_p o2 
		inner join SecurityObjects o on o2.id = o.id
		inner join inserted i on i.Id = o.Id
			update SecurityObjects4_c set LeftIndex = o.LeftIndex, RightIndex = o.RightIndex, TreeTag = o.TreeTag 
		from SecurityObjects4_c o2 
		inner join SecurityObjects o on o2.id = o.id
		inner join inserted i on i.Id = o.Id
			update SecurityObjects5_p set LeftIndex = o.LeftIndex, RightIndex = o.RightIndex, TreeTag = o.TreeTag 
		from SecurityObjects5_p o2 
		inner join SecurityObjects o on o2.id = o.id
		inner join inserted i on i.Id = o.Id
			update SecurityObjects5_c set LeftIndex = o.LeftIndex, RightIndex = o.RightIndex, TreeTag = o.TreeTag 
		from SecurityObjects5_c o2 
		inner join SecurityObjects o on o2.id = o.id
		inner join inserted i on i.Id = o.Id
	go


create view _Security_Tuple1
with schemabinding
as
	select 
		e.Id as EntryId,
		o1.Id as Id1, 
		t.ObjectId1 as ParentId1,
		p1.LeftIndex as ParentLeftIndex1,
		e.PrivilegeId, e.SubjectId, e.Allow, [Order]
	from dbo.AclEntries e
	inner join dbo.SecurityTargets t on e.TargetId = t.Id
	inner join dbo.SecurityObjects1_p p1 on p1.Id = t.ObjectId1  
inner join dbo.SecurityObjects o1 on o1.LeftIndex between p1.LeftIndex and p1.RightIndex and o1.TreeTag = p1.TreeTag
	where t.Arity = 1
go

create unique clustered index _Security_Tuple1_PK on _Security_Tuple1(
	Id1, 
	PrivilegeId, SubjectId,
	ParentLeftIndex1 desc,
	[Order], EntryId
)
go

create view Security_Tuple1
as
select 
	Id1, ParentId1,
	SubjectId, PrivilegeId, Allow
from
(
	select 
		Id1, 
		ParentId1,
		PrivilegeId, SubjectId, Allow,
		row_number() over( 
			partition by Id1, PrivilegeId, SubjectId 
			order by ParentLeftIndex1 desc, [Order]) 
		as SortedIndex
	from _Security_Tuple1 with(noexpand)
) x
where SortedIndex = 1
go


create view _Security_Tuple2
with schemabinding
as
	select 
		e.Id as EntryId,
		o1.Id as Id1, o2.Id as Id2, 
		t.ObjectId1 as ParentId1, t.ObjectId2 as ParentId2,
		p1.LeftIndex as ParentLeftIndex1, p2.LeftIndex as ParentLeftIndex2,
		e.PrivilegeId, e.SubjectId, e.Allow, [Order]
	from dbo.AclEntries e
	inner join dbo.SecurityTargets t on e.TargetId = t.Id
	inner join dbo.SecurityObjects1_p p1 on p1.Id = t.ObjectId1  
inner join dbo.SecurityObjects o1 on o1.LeftIndex between p1.LeftIndex and p1.RightIndex and o1.TreeTag = p1.TreeTag
inner join dbo.SecurityObjects2_p p2 on p2.Id = t.ObjectId2  
inner join dbo.SecurityObjects2_c o2 on o2.LeftIndex between p2.LeftIndex and p2.RightIndex and o2.TreeTag = p2.TreeTag
	where t.Arity = 2
go

create unique clustered index _Security_Tuple2_PK on _Security_Tuple2(
	Id1, Id2, 
	PrivilegeId, SubjectId,
	ParentLeftIndex1 desc, ParentLeftIndex2 desc,
	[Order], EntryId
)
go

create view Security_Tuple2
as
select 
	Id1, Id2, ParentId1, ParentId2,
	SubjectId, PrivilegeId, Allow
from
(
	select 
		Id1, Id2, 
		ParentId1, ParentId2,
		PrivilegeId, SubjectId, Allow,
		row_number() over( 
			partition by Id1, Id2, PrivilegeId, SubjectId 
			order by ParentLeftIndex1 desc, ParentLeftIndex2 desc, [Order]) 
		as SortedIndex
	from _Security_Tuple2 with(noexpand)
) x
where SortedIndex = 1
go


create view _Security_Tuple3
with schemabinding
as
	select 
		e.Id as EntryId,
		o1.Id as Id1, o2.Id as Id2, o3.Id as Id3, 
		t.ObjectId1 as ParentId1, t.ObjectId2 as ParentId2, t.ObjectId3 as ParentId3,
		p1.LeftIndex as ParentLeftIndex1, p2.LeftIndex as ParentLeftIndex2, p3.LeftIndex as ParentLeftIndex3,
		e.PrivilegeId, e.SubjectId, e.Allow, [Order]
	from dbo.AclEntries e
	inner join dbo.SecurityTargets t on e.TargetId = t.Id
	inner join dbo.SecurityObjects1_p p1 on p1.Id = t.ObjectId1  
inner join dbo.SecurityObjects o1 on o1.LeftIndex between p1.LeftIndex and p1.RightIndex and o1.TreeTag = p1.TreeTag
inner join dbo.SecurityObjects2_p p2 on p2.Id = t.ObjectId2  
inner join dbo.SecurityObjects2_c o2 on o2.LeftIndex between p2.LeftIndex and p2.RightIndex and o2.TreeTag = p2.TreeTag
inner join dbo.SecurityObjects3_p p3 on p3.Id = t.ObjectId3  
inner join dbo.SecurityObjects3_c o3 on o3.LeftIndex between p3.LeftIndex and p3.RightIndex and o3.TreeTag = p3.TreeTag
	where t.Arity = 3
go

create unique clustered index _Security_Tuple3_PK on _Security_Tuple3(
	Id1, Id2, Id3, 
	PrivilegeId, SubjectId,
	ParentLeftIndex1 desc, ParentLeftIndex2 desc, ParentLeftIndex3 desc,
	[Order], EntryId
)
go

create view Security_Tuple3
as
select 
	Id1, Id2, Id3, ParentId1, ParentId2, ParentId3,
	SubjectId, PrivilegeId, Allow
from
(
	select 
		Id1, Id2, Id3, 
		ParentId1, ParentId2, ParentId3,
		PrivilegeId, SubjectId, Allow,
		row_number() over( 
			partition by Id1, Id2, Id3, PrivilegeId, SubjectId 
			order by ParentLeftIndex1 desc, ParentLeftIndex2 desc, ParentLeftIndex3 desc, [Order]) 
		as SortedIndex
	from _Security_Tuple3 with(noexpand)
) x
where SortedIndex = 1
go


create view _Security_Tuple4
with schemabinding
as
	select 
		e.Id as EntryId,
		o1.Id as Id1, o2.Id as Id2, o3.Id as Id3, o4.Id as Id4, 
		t.ObjectId1 as ParentId1, t.ObjectId2 as ParentId2, t.ObjectId3 as ParentId3, t.ObjectId4 as ParentId4,
		p1.LeftIndex as ParentLeftIndex1, p2.LeftIndex as ParentLeftIndex2, p3.LeftIndex as ParentLeftIndex3, p4.LeftIndex as ParentLeftIndex4,
		e.PrivilegeId, e.SubjectId, e.Allow, [Order]
	from dbo.AclEntries e
	inner join dbo.SecurityTargets t on e.TargetId = t.Id
	inner join dbo.SecurityObjects1_p p1 on p1.Id = t.ObjectId1  
inner join dbo.SecurityObjects o1 on o1.LeftIndex between p1.LeftIndex and p1.RightIndex and o1.TreeTag = p1.TreeTag
inner join dbo.SecurityObjects2_p p2 on p2.Id = t.ObjectId2  
inner join dbo.SecurityObjects2_c o2 on o2.LeftIndex between p2.LeftIndex and p2.RightIndex and o2.TreeTag = p2.TreeTag
inner join dbo.SecurityObjects3_p p3 on p3.Id = t.ObjectId3  
inner join dbo.SecurityObjects3_c o3 on o3.LeftIndex between p3.LeftIndex and p3.RightIndex and o3.TreeTag = p3.TreeTag
inner join dbo.SecurityObjects4_p p4 on p4.Id = t.ObjectId4  
inner join dbo.SecurityObjects4_c o4 on o4.LeftIndex between p4.LeftIndex and p4.RightIndex and o4.TreeTag = p4.TreeTag
	where t.Arity = 4
go

create unique clustered index _Security_Tuple4_PK on _Security_Tuple4(
	Id1, Id2, Id3, Id4, 
	PrivilegeId, SubjectId,
	ParentLeftIndex1 desc, ParentLeftIndex2 desc, ParentLeftIndex3 desc, ParentLeftIndex4 desc,
	[Order], EntryId
)
go

create view Security_Tuple4
as
select 
	Id1, Id2, Id3, Id4, ParentId1, ParentId2, ParentId3, ParentId4,
	SubjectId, PrivilegeId, Allow
from
(
	select 
		Id1, Id2, Id3, Id4, 
		ParentId1, ParentId2, ParentId3, ParentId4,
		PrivilegeId, SubjectId, Allow,
		row_number() over( 
			partition by Id1, Id2, Id3, Id4, PrivilegeId, SubjectId 
			order by ParentLeftIndex1 desc, ParentLeftIndex2 desc, ParentLeftIndex3 desc, ParentLeftIndex4 desc, [Order]) 
		as SortedIndex
	from _Security_Tuple4 with(noexpand)
) x
where SortedIndex = 1
go


create view _Security_Tuple5
with schemabinding
as
	select 
		e.Id as EntryId,
		o1.Id as Id1, o2.Id as Id2, o3.Id as Id3, o4.Id as Id4, o5.Id as Id5, 
		t.ObjectId1 as ParentId1, t.ObjectId2 as ParentId2, t.ObjectId3 as ParentId3, t.ObjectId4 as ParentId4, t.ObjectId5 as ParentId5,
		p1.LeftIndex as ParentLeftIndex1, p2.LeftIndex as ParentLeftIndex2, p3.LeftIndex as ParentLeftIndex3, p4.LeftIndex as ParentLeftIndex4, p5.LeftIndex as ParentLeftIndex5,
		e.PrivilegeId, e.SubjectId, e.Allow, [Order]
	from dbo.AclEntries e
	inner join dbo.SecurityTargets t on e.TargetId = t.Id
	inner join dbo.SecurityObjects1_p p1 on p1.Id = t.ObjectId1  
inner join dbo.SecurityObjects o1 on o1.LeftIndex between p1.LeftIndex and p1.RightIndex and o1.TreeTag = p1.TreeTag
inner join dbo.SecurityObjects2_p p2 on p2.Id = t.ObjectId2  
inner join dbo.SecurityObjects2_c o2 on o2.LeftIndex between p2.LeftIndex and p2.RightIndex and o2.TreeTag = p2.TreeTag
inner join dbo.SecurityObjects3_p p3 on p3.Id = t.ObjectId3  
inner join dbo.SecurityObjects3_c o3 on o3.LeftIndex between p3.LeftIndex and p3.RightIndex and o3.TreeTag = p3.TreeTag
inner join dbo.SecurityObjects4_p p4 on p4.Id = t.ObjectId4  
inner join dbo.SecurityObjects4_c o4 on o4.LeftIndex between p4.LeftIndex and p4.RightIndex and o4.TreeTag = p4.TreeTag
inner join dbo.SecurityObjects5_p p5 on p5.Id = t.ObjectId5  
inner join dbo.SecurityObjects5_c o5 on o5.LeftIndex between p5.LeftIndex and p5.RightIndex and o5.TreeTag = p5.TreeTag
	where t.Arity = 5
go

create unique clustered index _Security_Tuple5_PK on _Security_Tuple5(
	Id1, Id2, Id3, Id4, Id5, 
	PrivilegeId, SubjectId,
	ParentLeftIndex1 desc, ParentLeftIndex2 desc, ParentLeftIndex3 desc, ParentLeftIndex4 desc, ParentLeftIndex5 desc,
	[Order], EntryId
)
go

create view Security_Tuple5
as
select 
	Id1, Id2, Id3, Id4, Id5, ParentId1, ParentId2, ParentId3, ParentId4, ParentId5,
	SubjectId, PrivilegeId, Allow
from
(
	select 
		Id1, Id2, Id3, Id4, Id5, 
		ParentId1, ParentId2, ParentId3, ParentId4, ParentId5,
		PrivilegeId, SubjectId, Allow,
		row_number() over( 
			partition by Id1, Id2, Id3, Id4, Id5, PrivilegeId, SubjectId 
			order by ParentLeftIndex1 desc, ParentLeftIndex2 desc, ParentLeftIndex3 desc, ParentLeftIndex4 desc, ParentLeftIndex5 desc, [Order]) 
		as SortedIndex
	from _Security_Tuple5 with(noexpand)
) x
where SortedIndex = 1
go


create trigger SecurityTargets_MakeSureObjectsExist on SecurityTargets after insert, update
as
	declare @empty uniqueidentifier
	set @empty = '00000000-0000-0000-0000-000000000000'

			insert into SecurityObjects(Id,LeftIndex,RightIndex) 
		select distinct ObjectId1, 0, 0 from inserted where ObjectId1 <> @empty and ObjectId1 not in (select Id from SecurityObjects)
			insert into SecurityObjects(Id,LeftIndex,RightIndex) 
		select distinct ObjectId2, 0, 0 from inserted where ObjectId2 <> @empty and ObjectId2 not in (select Id from SecurityObjects)
			insert into SecurityObjects(Id,LeftIndex,RightIndex) 
		select distinct ObjectId3, 0, 0 from inserted where ObjectId3 <> @empty and ObjectId3 not in (select Id from SecurityObjects)
			insert into SecurityObjects(Id,LeftIndex,RightIndex) 
		select distinct ObjectId4, 0, 0 from inserted where ObjectId4 <> @empty and ObjectId4 not in (select Id from SecurityObjects)
			insert into SecurityObjects(Id,LeftIndex,RightIndex) 
		select distinct ObjectId5, 0, 0 from inserted where ObjectId5 <> @empty and ObjectId5 not in (select Id from SecurityObjects)
	go