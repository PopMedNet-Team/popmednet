if not exists( select * from sys.tables where object_id = object_id('projects') )
begin

CREATE TABLE [dbo].[Projects](
	[SID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Acronym] [nvarchar](max) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,	
	[isDeleted] [bit] NOT NULL,
	[isActive] [bit] NOT NULL DEFAULT 0
PRIMARY KEY CLUSTERED 
(
	[SID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

insert into [dbo].[Projects] (SID, name, acronym, isDeleted, isActive) values([dbo].[newsqlguid](), 'Default', 'DEFAULT', 0, 1)

declare @allProjects uniqueidentifier
set @allProjects = '6A690001-7579-4C74-ADE1-A2210107FA29'

insert into securityinheritance([start], [end]) select [SID], @allProjects from [projects]

end
go


if not exists( select * from sys.tables where object_id = object_id('projects_users') )
begin

CREATE TABLE [dbo].[Projects_Users](
	[ProjectId] [uniqueidentifier] NOT NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[Projects_Users]  WITH CHECK ADD FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([SID])
ON DELETE CASCADE

ALTER TABLE [dbo].[Projects_Users]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE

end
go
