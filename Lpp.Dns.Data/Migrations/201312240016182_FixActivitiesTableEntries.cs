namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixActivitiesTableEntries : DbMigration
    {
        public override void Up()
        {
            //INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (6001, 'Task 1: Define the surveillance population and create the distributed database','','06C20001-1C79-4260-915E-A22201477C58', 6)
            Sql(@" 
            IF EXISTS(SELECT * FROM [Projects] WHERE SID = '06C20001-1C79-4260-915E-A22201477C58' )
            BEGIN
                --Script to Fix the Names of Task 06 projects.

                IF EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 6001002 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' )
		            UPDATE [Activities] SET [Name] ='Phase 2'  WHERE [Id]= 6001002  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58'
                IF EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 6001003 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' )
		            UPDATE [Activities] SET [Name] ='Phase 3'  WHERE [Id]= 6001003  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58'

                --Script to Insert Entries for Task 8 if not exists.

                IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8001001 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8001001, 'Acute Kidney Injury','','06C20001-1C79-4260-915E-A22201477C58', 8001,3) 
                IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8001002 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8001002, 'Thromboembolism','','06C20001-1C79-4260-915E-A22201477C58', 8001,3) 
                IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8001003 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8001003, 'Atrial Fibrillation','','06C20001-1C79-4260-915E-A22201477C58', 8001,3) 
                IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8001004 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8001004, 'Hemolytic Anemia','','06C20001-1C79-4260-915E-A22201477C58', 8001,3) 
                IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8002001 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8002001, 'Alternative Approaches','','06C20001-1C79-4260-915E-A22201477C58', 8002,3) 
                IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8003001 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8003001, '15 HOIs','','06C20001-1C79-4260-915E-A22201477C58', 8003,3) 
                IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8003002 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8003002, '15 Cohorts','','06C20001-1C79-4260-915E-A22201477C58', 8003,3) 
                IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8004001 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8004001, 'Taxonomy','','06C20001-1C79-4260-915E-A22201477C58', 8004,3) 
                IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8005001 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8005001, 'Confounder Adjustment','','06C20001-1C79-4260-915E-A22201477C58', 8005,3) 
                IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8005002 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8005002, 'Supplemental Info','','06C20001-1C79-4260-915E-A22201477C58', 8005,3) 
                IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8005003 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8005003, 'PROMPT 3/Sequential Methods','','06C20001-1C79-4260-915E-A22201477C58', 8005,3) 
                IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8006001 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8006001, 'PROMPT 1/Self-ControlDesign','','06C20001-1C79-4260-915E-A22201477C58', 8006,3) 
                IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8006002 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8006002, 'PROMPT 2/HDPS','','06C20001-1C79-4260-915E-A22201477C58', 8006,3) 
                
                --Script to Fix the Parent of problematic entries.

                UPDATE [Activities] SET [ParentId] ='8002'  WHERE [Id]= 8002001  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' 
                UPDATE [Activities] SET [ParentId] ='8002'  WHERE [Id]= 8002002  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' 
                UPDATE [Activities] SET [ParentId] ='8003'  WHERE [Id]= 8003001  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' 
                UPDATE [Activities] SET [ParentId] ='8003'  WHERE [Id]= 8003002  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' 
                UPDATE [Activities] SET [ParentId] ='8003'  WHERE [Id]= 8003003  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' 
                UPDATE [Activities] SET [ParentId] ='8004'  WHERE [Id]= 8004001  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' 
                UPDATE [Activities] SET [ParentId] ='8004'  WHERE [Id]= 8004002  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' 
                UPDATE [Activities] SET [ParentId] ='8005'  WHERE [Id]= 8005001  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' 
                UPDATE [Activities] SET [ParentId] ='8005'  WHERE [Id]= 8005002  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' 
                UPDATE [Activities] SET [ParentId] ='8005'  WHERE [Id]= 8005003  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' 
                UPDATE [Activities] SET [ParentId] ='8005'  WHERE [Id]= 8005004  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' 
                UPDATE [Activities] SET [ParentId] ='8006'  WHERE [Id]= 8006001  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' 
                UPDATE [Activities] SET [ParentId] ='8006'  WHERE [Id]= 8006002  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' 
                UPDATE [Activities] SET [ParentId] ='8006'  WHERE [Id]= 8006003  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' 

                --Script to Fix the Task Levels and add Not Selected fields wherever it does not exist.

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
