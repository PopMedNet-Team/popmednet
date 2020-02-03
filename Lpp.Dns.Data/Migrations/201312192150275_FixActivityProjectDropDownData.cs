namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixActivityProjectDropDownData : DbMigration
    {
        public override void Up()
        {
            //INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (6001, 'Task 1: Define the surveillance population and create the distributed database','','06C20001-1C79-4260-915E-A22201477C58', 6)
            Sql(@" IF EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 6001002 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' )
		            UPDATE [Activities] SET [Name] ='Phase 2'  WHERE [Id]= 6001002  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58'");
            Sql(@" IF EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 6001003 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' )
		            UPDATE [Activities] SET [Name] ='Phase 3'  WHERE [Id]= 6001003  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58'");
            //INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (8, '08','08' , '06C20001-1C79-4260-915E-A22201477C58' , null)
            //INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (8001, '2. A) HOI Validation/Adjudication','','06C20001-1C79-4260-915E-A22201477C58', 8)
                Sql(@" IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8001001 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8001001, 'Acute Kidney Injury','','06C20001-1C79-4260-915E-A22201477C58', 8001,3) ");
                Sql(@" IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8001002 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8001002, 'Thromboembolism','','06C20001-1C79-4260-915E-A22201477C58', 8001,3) ");
                Sql(@" IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8001003 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8001003, 'Atrial Fibrillation','','06C20001-1C79-4260-915E-A22201477C58', 8001,3) ");
                Sql(@" IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8001004 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8001004, 'Hemolytic Anemia','','06C20001-1C79-4260-915E-A22201477C58', 8001,3) ");
            //INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (8002, '2. B) Alternative Approaches for HOI Validation','','06C20001-1C79-4260-915E-A22201477C58', 8)
                Sql(@" IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8002001 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8002001, 'Alternative Approaches','','06C20001-1C79-4260-915E-A22201477C58', 8002,3) ");
            //INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (8003, '2. C) Surveillance Preparedness','','06C20001-1C79-4260-915E-A22201477C58', 8)
                Sql(@" IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8003001 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8003001, '15 HOIs','','06C20001-1C79-4260-915E-A22201477C58', 8003,3) ");
                Sql(@" IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8003002 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8003002, '15 Cohorts','','06C20001-1C79-4260-915E-A22201477C58', 8003,3) ");
            //INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (8004, '2. D) Extension of Methods Taxonomy','','06C20001-1C79-4260-915E-A22201477C58', 8)
                Sql(@" IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8004001 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8004001, 'Taxonomy','','06C20001-1C79-4260-915E-A22201477C58', 8004,3) ");
            //INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (8005, '2. E) Methods Development','','06C20001-1C79-4260-915E-A22201477C58', 8)
                Sql(@" IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8005001 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8005001, 'Confounder Adjustment','','06C20001-1C79-4260-915E-A22201477C58', 8005,3) ");
                Sql(@" IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8005002 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8005002, 'Supplemental Info','','06C20001-1C79-4260-915E-A22201477C58', 8005,3) ");
                Sql(@" IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8005003 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8005003, 'PROMPT 3/Sequential Methods','','06C20001-1C79-4260-915E-A22201477C58', 8005,3) ");
            //INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId]) VALUES (8006, '2. F) Routine Surveillance','','06C20001-1C79-4260-915E-A22201477C58', 8)
                Sql(@" IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8006001 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8006001, 'PROMPT 1/Self-ControlDesign','','06C20001-1C79-4260-915E-A22201477C58', 8006,3) ");
                Sql(@" IF NOT EXISTS (SELECT 1 FROM [Activities] WHERE [Id]= 8006002 and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58')
		            INSERT INTO [Activities] ([Id] ,[Name] ,[Description] ,[ProjectId] ,[ParentId],[TaskLevel]) VALUES (8006002, 'PROMPT 2/HDPS','','06C20001-1C79-4260-915E-A22201477C58', 8006,3) ");

        }
        
        public override void Down()
        {
        }
    }
}
