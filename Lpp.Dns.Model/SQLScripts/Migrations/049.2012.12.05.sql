update Groups set IsDeleted = 0 where IsDeleted is null
go

alter table Groups alter column IsDeleted bit not null
go