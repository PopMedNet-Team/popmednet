namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreateRequestTypesSecurityPermission : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Permissions (ID, Name, Description) VALUES ('AE341111-020E-4E32-9E9F-A3B00134A862', 'Create Request Types', 'Allows the security group to create Request Types for the site')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('AE341111-020E-4E32-9E9F-A3B00134A862', 0)");
            
            Sql("INSERT INTO Permissions (ID, Name, Description) VALUES ('E8C11111-C030-424A-87A1-A3B00134A1C5', 'List Request Types', 'Allows the security group to create Request Types for the site')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('E8C11111-C030-424A-87A1-A3B00134A1C5', 0)");

            Sql("INSERT INTO Permissions (ID, Name, Description) VALUES ('25BD1111-4739-41D8-BC74-A2AF01733B64', 'Manage Request Types', 'Allows the security group to manage request types for the current project')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('25BD1111-4739-41D8-BC74-A2AF01733B64', 0)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('25BD1111-4739-41D8-BC74-A2AF01733B64', 4)");
        }
        
        public override void Down()
        {
        }
    }
}
