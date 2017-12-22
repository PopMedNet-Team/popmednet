namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrectActivityProjectsForTaskOrder8 : DbMigration
    {
        public override void Up()
        {
            Sql(@" UPDATE [Activities] SET [ParentId] ='8002'  WHERE [Id]= 8002001  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' ");
            Sql(@" UPDATE [Activities] SET [ParentId] ='8002'  WHERE [Id]= 8002002  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' ");
            Sql(@" UPDATE [Activities] SET [ParentId] ='8003'  WHERE [Id]= 8003001  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' ");
            Sql(@" UPDATE [Activities] SET [ParentId] ='8003'  WHERE [Id]= 8003002  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' ");
            Sql(@" UPDATE [Activities] SET [ParentId] ='8003'  WHERE [Id]= 8003003  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' ");
            Sql(@" UPDATE [Activities] SET [ParentId] ='8004'  WHERE [Id]= 8004001  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' ");
            Sql(@" UPDATE [Activities] SET [ParentId] ='8004'  WHERE [Id]= 8004002  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' ");
            Sql(@" UPDATE [Activities] SET [ParentId] ='8005'  WHERE [Id]= 8005001  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' ");
            Sql(@" UPDATE [Activities] SET [ParentId] ='8005'  WHERE [Id]= 8005002  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' ");
            Sql(@" UPDATE [Activities] SET [ParentId] ='8005'  WHERE [Id]= 8005003  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' ");
            Sql(@" UPDATE [Activities] SET [ParentId] ='8005'  WHERE [Id]= 8005004  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' ");
            Sql(@" UPDATE [Activities] SET [ParentId] ='8006'  WHERE [Id]= 8006001  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' ");
            Sql(@" UPDATE [Activities] SET [ParentId] ='8006'  WHERE [Id]= 8006002  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' ");
            Sql(@" UPDATE [Activities] SET [ParentId] ='8006'  WHERE [Id]= 8006003  and [ProjectId]='06C20001-1C79-4260-915E-A22201477C58' ");
        }
        
        public override void Down()
        {
        }
    }
}
