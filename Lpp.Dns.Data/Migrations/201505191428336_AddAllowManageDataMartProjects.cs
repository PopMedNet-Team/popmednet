namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAllowManageDataMartProjects : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO Permissions (ID, Name, Description) VALUES ('6B42D2D8-F7A7-4119-9CC5-22991DC12AD3', 'Manage DataMart''s Projects', 'Allows the user to remove a DataMart from a Project directly from the DataMart instead of having to have permissions on the Project to do so.')");

            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('6B42D2D8-F7A7-4119-9CC5-22991DC12AD3', 0)");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('6B42D2D8-F7A7-4119-9CC5-22991DC12AD3', 1)");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('6B42D2D8-F7A7-4119-9CC5-22991DC12AD3', 3)");
        }
        
        public override void Down()
        {
        }
    }
}
