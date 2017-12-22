----------------------------------------------------------------------
/*
In this script we do the following.
Add the fileds, DisplayOrder int, TaskLevel int if they do not exist.
Add Not Selected entries one for each level of each project.
Change the DisplayOrder of the Not Selected entries to -1. In the view, we order by DisplayOrder so that Not Selected displays first.
Compute and Update TaskLevel field upto Task Project level.
*/
----------------------------------------------------------------------
--Update Table Structure.
----------------------------------------------------------------------
IF NOT EXISTS(select * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Activities' and COLUMN_NAME = 'DisplayOrder')
	ALTER TABLE [Activities] ADD DisplayOrder int Not NULL Default(0)

GO

IF NOT EXISTS(select * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Activities' and COLUMN_NAME = 'TaskLevel')
	ALTER TABLE [Activities] ADD TaskLevel int Not NULL Default(1)
GO	
----------------------------------------------------------------------


IF EXISTS(SELECT * FROM [Projects] WHERE Name = 'Mini-Sentinel' )
BEGIN

Declare
	 @noselect varchar(255)
	,@project  varchar(255)
	,@id int
	,@name varchar(255)
	,@desc varchar(255)
	,@projectid varchar(255)
	,@parentid int
	,@activityid int
	,@activityname varchar(255)
	,@activitydesc varchar(255)
	,@activityprojectid varchar(255)
	,@activityparentid int
	,@curparent int
	,@curid int
	,@tasklevel int
	
	
SELECT @project = (SELECT TOP 1 SID FROM [Projects] WHERE Name='Mini-Sentinel') , @noselect = 'Not Selected',@tasklevel =1
----------------------------------------------------------------------
--INSERT BLANK TASK ORDER
----------------------------------------------------------------------
If EXISTS(Select 1 FROM [Activities] WHERE [ParentId] is null and ISNULL([Name],'') = @noselect)
	Select @curparent=[Id] FROM [Activities] WHERE [ParentId] is null and ISNULL([Name],'') = @noselect
Else
	Begin
		Select @curid=MAX([Id])+1 FROM [Activities] WHERE [ParentId] is null
		INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (@curid, @noselect,@noselect , @project , null)
		Select @curparent = SCOPE_IDENTITY()
	End
--Update TaskLevel
Update [Activities]	 set TaskLevel=@tasklevel where ParentId is null

Declare TasOrderCur Cursor for 
	Select [Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId] 
	From [Activities] 
	Where ParentId is null

Open TasOrderCur

FETCH NEXT FROM TasOrderCur INTO @id,@name,@desc,@projectid,@parentid
WHILE @@FETCH_STATUS = 0
BEGIN
	----------------------------------------------------------------------
	--INSERT BLANK TASK ACTIVITY
	----------------------------------------------------------------------
	If EXISTS(Select 1 FROM [Activities] WHERE [ParentId] = @id and ISNULL([Name],'') = @noselect)
		Select @curparent=[Id] FROM [Activities] WHERE [ParentId] = @id and ISNULL([Name],'') = @noselect
	Else
		Begin
			If EXISTS(Select 1 FROM [Activities] WHERE [ParentId] = @id)
				Select @curid=MAX([Id]) + 1 FROM [Activities] WHERE [ParentId] = @id
			else
				Select @curid=  CAST((CAST(@id as varchar) + '001') as int)
			print @curid
			print @id
			INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (@curid, @noselect,@noselect , @project , @id)
			Select @curparent = SCOPE_IDENTITY()
		End
	--Update TaskLevel
	SELECT @tasklevel = 2
	Update [Activities]	 set TaskLevel=@tasklevel where ParentId = @id

	Declare TasActivityCur Cursor for 
		Select [Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId] 
		From [Activities] 
		Where ParentId = @id

	Open TasActivityCur

	FETCH NEXT FROM TasActivityCur INTO @activityid,@activityname,@activitydesc,@activityprojectid,@activityparentid
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-------------------------------------------------------------
		--INSERT BLANK TASK PROJECT
		-------------------------------------------------------------
		If EXISTS(Select 1 FROM [Activities] WHERE [ParentId] = @activityid and ISNULL([Name],'') = @noselect)
			Select @curparent=[Id] FROM [Activities] WHERE [ParentId] = @activityid and ISNULL([Name],'') = @noselect
		Else
			Begin
				If EXISTS(Select 1 FROM [Activities] WHERE [ParentId] = @activityid)
					Select @curid=MAX([Id]) + 1 FROM [Activities] WHERE [ParentId] = @activityid
				else
					Select @curid=  CAST((CAST(@activityid as varchar) + '001') as int)
				print @curid
				print @activityid
				INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (@curid, @noselect,@noselect , @project , @activityid)
				Select @curparent = SCOPE_IDENTITY()
			End
		--Update TaskLevel
		SELECT @tasklevel = 3
		Update [Activities]	 set TaskLevel=@tasklevel where ParentId = @activityid
		
		FETCH NEXT FROM TasActivityCur INTO @activityid,@activityname,@activitydesc,@activityprojectid,@activityparentid
	END

	Close TasActivityCur
	Deallocate TasActivityCur
	----------------------------------------------------------------------
	FETCH NEXT FROM TasOrderCur INTO @id,@name,@desc,@projectid,@parentid
	
END

Close TasOrderCur
Deallocate TasOrderCur

--Update the display order to -1 so that they get first in the order.
UPDATE [Activities] set DisplayOrder = -1 where [Name] = @noselect
UPDATE [Activities] set DisplayOrder = 0 where [Name] <> @noselect



END
GO

--Restore constraint removed in 078.
ALTER TABLE [dbo].[Queries]  WITH CHECK ADD CONSTRAINT [FK_Queries_Activities] FOREIGN KEY([ActivityId]) 
REFERENCES [dbo].[Activities] ([Id])
GO

IF NOT EXISTS(SELECT NULL FROM MigrationScript WHERE ScriptRun = '085.2013.10.25')
BEGIN
	INSERT [MigrationScript]([Date],[ScriptRun]) VALUES (GetDate(), '085.2013.10.25')
END
GO