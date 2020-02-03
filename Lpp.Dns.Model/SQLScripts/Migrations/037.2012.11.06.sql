if not exists( select * from sys.tables where object_id = object_id('projects_datamarts') )
begin

CREATE TABLE [dbo].[Projects_DataMarts](
	[ProjectId] [uniqueidentifier] NOT NULL,
	[DataMartId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[DataMartId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[Projects_DataMarts]  WITH CHECK ADD FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([SID])
ON DELETE CASCADE

ALTER TABLE [dbo].[Projects_DataMarts]  WITH CHECK ADD FOREIGN KEY([DataMartId])
REFERENCES [dbo].[DataMarts] ([DataMartId])
ON DELETE CASCADE

end

declare @projectid uniqueidentifier
set @projectid = (select sid from projects where name='Default')

if not exists( select * from projects_datamarts where projectid = @projectid)
begin
insert into projects_datamarts (projectid, datamartid) select @projectid, datamartid from datamarts
end
go
