create table Files
(
	Id int primary key identity,
	[FileName] nvarchar(max) not null,
	[MimeType] nvarchar(max) not null,
	[Created] datetime2 not null default getdate(),

	[SegmentSize] int not null,
	[NumberOfSegments] int not null,
	[LastSegmentFill] int not null
)

create table FileSegments
(
	FileId int not null references Files on delete cascade,
	[Index] int not null default 0,
	[Data] varbinary(max)
)