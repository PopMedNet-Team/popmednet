declare cur cursor for select ( 'drop database ' + name ) from sysdatabases where name like 'Test_%';
open cur;

declare @sql nvarchar(200);
fetch next from cur into @sql;
while @@fetch_status = 0
begin
	print @sql
	execute sp_executesql @sql;
	fetch next from cur into @sql;
end

close cur;
deallocate cur;