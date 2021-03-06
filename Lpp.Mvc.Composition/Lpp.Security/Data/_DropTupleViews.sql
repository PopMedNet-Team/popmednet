

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