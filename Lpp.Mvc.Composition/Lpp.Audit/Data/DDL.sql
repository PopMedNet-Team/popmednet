create table AuditEvents
(
	Id int identity primary key,
	KindId uniqueidentifier not null,
	[Time] datetime not null default getdate(),
	TargetId1 uniqueidentifier not null default '00000000-0000-0000-0000-000000000000',
	TargetId2 uniqueidentifier not null default '00000000-0000-0000-0000-000000000000',
	TargetId3 uniqueidentifier not null default '00000000-0000-0000-0000-000000000000',
	TargetId4 uniqueidentifier not null default '00000000-0000-0000-0000-000000000000',
	TargetId5 uniqueidentifier not null default '00000000-0000-0000-0000-000000000000',
	TargetId6 uniqueidentifier not null default '00000000-0000-0000-0000-000000000000',
	TargetId7 uniqueidentifier not null default '00000000-0000-0000-0000-000000000000',
	TargetId8 uniqueidentifier not null default '00000000-0000-0000-0000-000000000000',
	TargetId9 uniqueidentifier not null default '00000000-0000-0000-0000-000000000000',
	TargetId10 uniqueidentifier not null default '00000000-0000-0000-0000-000000000000'
)
go

create table AuditPropertyValues
(
	Id int identity primary key,
	EventId int not null references AuditEvents on delete cascade,
	PropertyId uniqueidentifier not null,
	IntValue int,
	StringValue nvarchar(max),
	DoubleValue float,
	DateTimeValue datetime,
	GuidValue uniqueidentifier
)
go

create index IX_EventId on AuditPropertyValues(EventId)
create index IX_Time on AuditEvents([Time])
go
