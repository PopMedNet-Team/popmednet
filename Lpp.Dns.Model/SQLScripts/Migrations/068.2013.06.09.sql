IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[_Security_Tuple1]') AND name = N'_Security_Tuple1_Reverse_IX')
DROP INDEX [_Security_Tuple1_Reverse_IX] ON [dbo].[_Security_Tuple1] WITH ( ONLINE = OFF )

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[_Security_Tuple2]') AND name = N'_Security_Tuple2_Reverse_IX')
DROP INDEX [_Security_Tuple2_Reverse_IX] ON [dbo].[_Security_Tuple2] WITH ( ONLINE = OFF )

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[_Security_Tuple3]') AND name = N'_Security_Tuple3_Reverse_IX')
DROP INDEX [_Security_Tuple3_Reverse_IX] ON [dbo].[_Security_Tuple3] WITH ( ONLINE = OFF )

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[_Security_Tuple4]') AND name = N'_Security_Tuple4_Reverse_IX')
DROP INDEX [_Security_Tuple4_Reverse_IX] ON [dbo].[_Security_Tuple4] WITH ( ONLINE = OFF )

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[_Security_Tuple5]') AND name = N'_Security_Tuple5_Reverse_IX')
DROP INDEX [_Security_Tuple5_Reverse_IX] ON [dbo].[_Security_Tuple5] WITH ( ONLINE = OFF )

GO

create index _Security_Tuple1_Reverse_IX on _Security_Tuple1(
	SubjectId, PrivilegeId, Id1,
	DistancesJoined, DeniedEntries
)

create index _Security_Tuple2_Reverse_IX on _Security_Tuple2(
	SubjectId, PrivilegeId, Id1, Id2,
	DistancesJoined, DeniedEntries
)

create index _Security_Tuple3_Reverse_IX on _Security_Tuple3(
	SubjectId, PrivilegeId, Id1, Id2, Id3,
	DistancesJoined, DeniedEntries
)

create index _Security_Tuple4_Reverse_IX on _Security_Tuple4(
	SubjectId, PrivilegeId, Id1, Id2, Id3, Id4,
	DistancesJoined, DeniedEntries
)

create index _Security_Tuple5_Reverse_IX on _Security_Tuple5(
	SubjectId, PrivilegeId, Id1, Id2, Id3, Id4, Id5,
	DistancesJoined, DeniedEntries
)

GO