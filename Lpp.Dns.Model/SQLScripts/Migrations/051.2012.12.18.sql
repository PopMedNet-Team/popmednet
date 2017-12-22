if not exists( select * from sys.columns where object_id = object_id( 'Activities' ) and name = 'Description' ) begin
	alter table Activities add [Description] nvarchar(max)
end

if not exists( select * from sys.columns where object_id = object_id( 'Activities' ) and name = 'ProjectId' ) begin
	alter table Activities add ProjectId uniqueidentifier references Projects
end