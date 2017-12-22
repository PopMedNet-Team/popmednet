if not exists( select * from sys.columns where name = 'ProjectId' and object_id = object_id('PluginSessions') ) begin
	alter table PluginSessions add ProjectId uniqueidentifier
end
go