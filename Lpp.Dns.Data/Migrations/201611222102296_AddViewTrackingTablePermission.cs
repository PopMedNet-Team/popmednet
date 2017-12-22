namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddViewTrackingTablePermission : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF NOT EXISTS(SELECT NULL FROM dbo.[Permissions] WHERE ID = '97850001-E880-40FB-AC98-A6C601592C15')
	INSERT INTO dbo.[Permissions](ID, [Name], [Description]) VALUES ('97850001-E880-40FB-AC98-A6C601592C15', 'View Tracking Table', 'Specifies if the tracking table summary will be visible for the security group when the specified workflow activity is active.')");
            Sql(@"IF NOT EXISTS(SELECT NULL FROM dbo.PermissionLocations WHERE PermissionID = '97850001-E880-40FB-AC98-A6C601592C15' AND [Type] = 24)
	INSERT INTO dbo.PermissionLocations (PermissionID, [Type]) VALUES ('97850001-E880-40FB-AC98-A6C601592C15', 24)");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM dbo.PermissionLocations WHERE PermissionID = '97850001-E880-40FB-AC98-A6C601592C15' AND [Type] = 24");
            Sql("DELETE FROM dbo.[Permissions] WHERE ID = '97850001-E880-40FB-AC98-A6C601592C15'");
        }
    }
}
