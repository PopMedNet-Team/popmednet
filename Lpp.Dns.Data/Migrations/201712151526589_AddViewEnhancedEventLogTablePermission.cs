namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddViewEnhancedEventLogTablePermission : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF NOT EXISTS(SELECT NULL FROM dbo.[Permissions] WHERE ID = '75160001-97C3-4619-A197-A84A00FD2918')
	INSERT INTO dbo.[Permissions](ID, [Name], [Description]) VALUES ('75160001-97C3-4619-A197-A84A00FD2918', 'View Enhanced Event Log', 'Specifies if the enhanced event log will be visible for the security group when the specified workflow activity is active.')");
            Sql(@"IF NOT EXISTS(SELECT NULL FROM dbo.PermissionLocations WHERE PermissionID = '75160001-97C3-4619-A197-A84A00FD2918' AND [Type] = 24)
	INSERT INTO dbo.PermissionLocations (PermissionID, [Type]) VALUES ('75160001-97C3-4619-A197-A84A00FD2918', 24)");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM dbo.PermissionLocations WHERE PermissionID = '75160001-97C3-4619-A197-A84A00FD2918' AND [Type] = 24");
            Sql("DELETE FROM dbo.[Permissions] WHERE ID = '75160001-97C3-4619-A197-A84A00FD2918'");
        }
    }
}
