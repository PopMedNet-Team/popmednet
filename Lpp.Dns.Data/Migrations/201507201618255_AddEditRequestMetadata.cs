namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEditRequestMetadata : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO Permissions (ID, Name, Description) VALUES ('51A43BE0-290A-49D4-8278-ADE36706A80D', 'Edit Request Metadata', 'Allows the user to edit the request metadata.')");

            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('51A43BE0-290A-49D4-8278-ADE36706A80D', 24)");
        }
        
        public override void Down()
        {
            Sql(@"Delete from PermissionLocations Where PermissionID '51A43BE0-290A-49D4-8278-ADE36706A80D'");
            Sql(@"Delete from Permissions Where ID = '51A43BE0-290A-49D4-8278-ADE36706A80D'");
        }
    }
}
