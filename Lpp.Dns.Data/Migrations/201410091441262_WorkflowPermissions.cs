namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkflowPermissions : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Permissions (ID, Name, Description) VALUES ('DD20EE1B-C433-49F8-8A91-76AD10DB1BEC', 'View Task', 'Allows the security group to view tasks associated with the workflow activity')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('DD20EE1B-C433-49F8-8A91-76AD10DB1BEC', 24)");

            Sql("INSERT INTO Permissions (ID, Name, Description) VALUES ('75FC4DEA-220C-486D-9E8C-AC2B6F6F8415', 'Edit Task', 'Allows the security group to make changes to tasks associated wit hthe workflow activity')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('75FC4DEA-220C-486D-9E8C-AC2B6F6F8415', 24)");

            Sql("INSERT INTO Permissions (ID, Name, Description) VALUES ('7025F490-9635-4540-B682-3A4F152E73EF', 'View Comments', 'Allows the security group to view comments made against the specified workflow activity')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('7025F490-9635-4540-B682-3A4F152E73EF', 24)");

            Sql("INSERT INTO Permissions (ID, Name, Description) VALUES ('B03BDDE0-CD76-47C3-BB7D-C39A28B232B4', 'Add Comments', 'Allows the security group to add comments to tasks associated with the specified workflow activity')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('B03BDDE0-CD76-47C3-BB7D-C39A28B232B4', 24)");

            Sql("INSERT INTO Permissions (ID, Name, Description) VALUES ('FAE8FC24-362D-4382-AF31-0933AF95FDE9', 'View Documents', 'Allows the security group to view documents on tasks associated with the specified workflow activity')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('FAE8FC24-362D-4382-AF31-0933AF95FDE9', 24)");

            Sql("INSERT INTO Permissions (ID, Name, Description) VALUES ('A593C7EC-61F3-42F8-8D26-8A4BACC8BC17', 'Add Documents', 'Allows the security group to add documents to tasks associated with the specified workflow activity')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('A593C7EC-61F3-42F8-8D26-8A4BACC8BC17', 24)");

            Sql("INSERT INTO Permissions (ID, Name, Description) VALUES ('0312B7F3-FFBC-4FBF-B3BD-5CB69AEAA045', 'Revise Documents', 'Allows the security group to revise documents on tasks associated with the specified workflow activity')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0312B7F3-FFBC-4FBF-B3BD-5CB69AEAA045', 24)");

            Sql("INSERT INTO Permissions (ID, Name, Description) VALUES ('32DC49AE-E845-4EA9-80CD-CC0281559443', 'Complete/Close Task', 'Allows the security group to close or complete a task associated wit hthe specified workflow activity')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('32DC49AE-E845-4EA9-80CD-CC0281559443', 24)");

            Sql("INSERT INTO Permissions (ID, Name, Description) VALUES ('FFADFDE8-2ADA-488E-90AA-0AD29874A61B', 'View Request Overview', 'Specifies if the request overview will be visible for the security group when the specified workflow activity is active')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('FFADFDE8-2ADA-488E-90AA-0AD29874A61B', 24)");

            Sql("INSERT INTO Permissions (ID, Name, Description) VALUES ('712B3B5D-5115-40C0-AB5C-73132965902A', 'Terminate Workflow', 'Specifies that the security group will be allowed to terminate the workflow at the given stage')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('712B3B5D-5115-40C0-AB5C-73132965902A', 24)");
        }
        
        public override void Down()
        {
        }
    }
}
