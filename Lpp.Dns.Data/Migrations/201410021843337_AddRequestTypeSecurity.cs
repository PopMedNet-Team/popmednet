namespace Lpp.Dns.Data.Migrations
{
    using Lpp.Dns.DTO.Security;
    using Lpp.Utilities;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestTypeSecurity : DbMigration
    {
        public override void Up()
        {
            Sql("DROP TRIGGER AclRequestTypes_InsertItem");
            Sql("DROP TRIGGER AclRequestTypes_UpdateItem");
            Sql("DROP TRIGGER AclRequestTypes_DeleteItem");
            Sql(MigrationHelpers.AddAclInsertScript("AclRequestTypes"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclRequestTypes"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclRequestTypes"));

            Sql("INSERT INTO Permissions (ID, [Name], [Description]) VALUES ('" + PermissionIdentifiers.RequestTypes.Delete.ID.ToString("D") + "', 'Delete Request Type', 'Allows the selected security group to delete the selected Request Type.')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('" + PermissionIdentifiers.RequestTypes.Delete.ID.ToString("D") + "', 0)");//Global
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('" + PermissionIdentifiers.RequestTypes.Delete.ID.ToString("D") + "', 23)");//Templates

            Sql("INSERT INTO Permissions (ID, [Name], [Description]) VALUES ('" + PermissionIdentifiers.RequestTypes.Edit.ID.ToString("D") + "', 'Edit Request Type', 'Allows the selected security group to edit the selected Request Type.')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('" + PermissionIdentifiers.RequestTypes.Edit.ID.ToString("D") + "', 0)");//Global
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('" + PermissionIdentifiers.RequestTypes.Edit.ID.ToString("D") + "', 23)");//Templates

            Sql("INSERT INTO Permissions (ID, [Name], [Description]) VALUES ('" + PermissionIdentifiers.RequestTypes.ManageSecurity.ID.ToString("D") + "', 'Manage Request Type Security', 'Allows the selected security group to manage security of the selected Request Type.')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('" + PermissionIdentifiers.RequestTypes.ManageSecurity.ID.ToString("D") + "', 0)");//Global
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('" + PermissionIdentifiers.RequestTypes.ManageSecurity.ID.ToString("D") + "', 23)");//Templates

            Sql("INSERT INTO Permissions (ID, [Name], [Description]) VALUES ('" + PermissionIdentifiers.RequestTypes.View.ID.ToString("D") + "', 'View Request Type', 'Allows the selected security group to view the selected Request Type.')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('" + PermissionIdentifiers.RequestTypes.View.ID.ToString("D") + "', 0)");//Global
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('" + PermissionIdentifiers.RequestTypes.View.ID.ToString("D") + "', 23)");//Templates

        }
        
        public override void Down()
        {
        }
    }
}
