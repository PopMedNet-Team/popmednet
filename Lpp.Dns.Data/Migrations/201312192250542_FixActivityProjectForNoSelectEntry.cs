namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixActivityProjectForNoSelectEntry : DbMigration
    {
        public override void Up()
        {
            Sql(@"
            IF EXISTS(SELECT * FROM [Projects] WHERE SID = '06C20001-1C79-4260-915E-A22201477C58' )
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
	
	
            SELECT @project = '06C20001-1C79-4260-915E-A22201477C58' , @noselect = 'Not Selected',@tasklevel =1
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
             ");
        }
        
        public override void Down()
        {
        }
    }
}
