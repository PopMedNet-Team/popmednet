namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectPermissionLocationForEditMetadata : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE [Permissions] SET [Name] = 'Edit Metadata After Submission', [Description] = 'Allows the user the ability to edit the request metadata after a request has been submitted.' WHERE ID = '51A43BE0-290A-49D4-8278-ADE36706A80D'");
            Sql("IF NOT EXISTS(SELECT NULL FROM PermissionLocations WHERE PermissionID = '51A43BE0-290A-49D4-8278-ADE36706A80D' AND [Type] = 4) INSERT INTO PermissionLocations (PermissionID, [Type]) VALUES ('51A43BE0-290A-49D4-8278-ADE36706A80D', 4)");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM AclProjects WHERE PermissionID = '51A43BE0-290A-49D4-8278-ADE36706A80D'");
            Sql("DELETE FROM PermissionLocations WHERE PermissionID = '51A43BE0-290A-49D4-8278-ADE36706A80D' AND [Type] = 4");
            Sql("UPDATE [Permissions] SET [Name] = 'Edit Request Metadata', [Description] = 'Allows the user to edit the request metadata.' WHERE ID = '51A43BE0-290A-49D4-8278-ADE36706A80D'");
        }
    }
}
