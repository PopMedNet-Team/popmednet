declare @sql nvarchar(max) = (select 'alter table filesegments drop constraint ' + name from sys.foreign_keys where parent_object_id = object_id('filesegments'))
exec sp_sqlexec @sql

alter table filesegments add constraint FK_FileSegments_Files foreign key (FileId) references Files on delete cascade