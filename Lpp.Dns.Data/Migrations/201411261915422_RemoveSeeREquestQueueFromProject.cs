namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSeeREquestQueueFromProject : DbMigration
    {
        public override void Up()
        {
            Sql(@"DELETE FROM AclProjects WHERE PermissionID = '5D6DD388-7842-40A1-A27A-B9782A445E20'");
            Sql(@"DELETE FROM PermissionLocations WHERE PermissionID = '5D6DD388-7842-40A1-A27A-B9782A445E20' AND Type = 4");
            Sql(@"UPDATE Permissions SET Name = 'See DataMart Request Queue' WHERE ID = '5D6DD388-7842-40A1-A27A-B9782A445E20'");
        }
        
        public override void Down()
        {
        }
    }
}
