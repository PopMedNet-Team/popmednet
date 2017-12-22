if exists( select * from sys.indexes where name = 'securitytargets_unique' and object_id = object_id('securitytargets') )
	alter table SecurityTargets drop constraint SecurityTargets_Unique
go

alter table SecurityTargets 
	add constraint SecurityTargets_Unique
	unique ( Arity, ObjectId1, ObjectId2, ObjectId3, ObjectId4, ObjectId5, ObjectId6, ObjectId7, ObjectId8, ObjectId9, ObjectId10 )