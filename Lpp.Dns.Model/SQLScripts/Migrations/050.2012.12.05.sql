create index _Security_Tuple1_Reverse_IX on _Security_Tuple1(
	SubjectId, PrivilegeId, Id1,
	DistancesJoined
)
go

create index _Security_Tuple2_Reverse_IX on _Security_Tuple2(
	SubjectId, PrivilegeId, Id1, Id2,
	DistancesJoined
)
go

create index _Security_Tuple3_Reverse_IX on _Security_Tuple3(
	SubjectId, PrivilegeId, Id1, Id2, Id3,
	DistancesJoined
)
go

create index _Security_Tuple4_Reverse_IX on _Security_Tuple4(
	SubjectId, PrivilegeId, Id1, Id2, Id3, Id4,
	DistancesJoined
)
go

create index _Security_Tuple5_Reverse_IX on _Security_Tuple5(
	SubjectId, PrivilegeId, Id1, Id2, Id3, Id4, Id5,
	DistancesJoined
)
go