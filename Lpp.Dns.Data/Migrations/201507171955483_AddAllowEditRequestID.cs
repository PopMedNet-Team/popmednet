namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAllowEditRequestID : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO Permissions (ID, Name, Description) VALUES ('43BF0001-4735-4598-BBAD-A4D801478AAA', 'Edit Request ID', 'Allows the user to edit the request ID.')");

            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('43BF0001-4735-4598-BBAD-A4D801478AAA', 0)");
            Sql(@"INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('43BF0001-4735-4598-BBAD-A4D801478AAA', 4)");
        }
        
        public override void Down()
        {
            Sql(@"Delete from PermissionLocations Where PermissionID '43BF0001-4735-4598-BBAD-A4D801478AAA'");
            Sql(@"Delete from Permissions Where ID = '43BF0001-4735-4598-BBAD-A4D801478AAA'");
        }
    }
}
