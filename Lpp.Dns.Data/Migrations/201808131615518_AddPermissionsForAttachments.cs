namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPermissionsForAttachments : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Permissions(ID, Name, Description) VALUES('D59FA0D4-15FA-4088-9A98-35CDD7902EC1', 'Modify Attachments', 'Allows the user to add or delete Attachments.')");
            Sql("INSERT INTO PermissionLocations(PermissionID, Type) VALUES('D59FA0D4-15FA-4088-9A98-35CDD7902EC1', 4)");
            Sql("INSERT INTO PermissionLocations(PermissionID, Type) VALUES('D59FA0D4-15FA-4088-9A98-35CDD7902EC1', 24)");
            Sql("INSERT INTO PermissionLocations(PermissionID, Type) VALUES('D59FA0D4-15FA-4088-9A98-35CDD7902EC1', 11)");
            Sql("INSERT INTO PermissionLocations(PermissionID, Type) VALUES('D59FA0D4-15FA-4088-9A98-35CDD7902EC1', 20)");


            Sql("INSERT INTO Permissions(ID, Name, Description) VALUES('50157D72-8EED-45E4-B6F4-2A935191F57F', 'View Attachments', 'Allows the user to download Attachments.')");
            Sql("INSERT INTO PermissionLocations(PermissionID, Type) VALUES('50157D72-8EED-45E4-B6F4-2A935191F57F', 4)");
            Sql("INSERT INTO PermissionLocations(PermissionID, Type) VALUES('50157D72-8EED-45E4-B6F4-2A935191F57F', 24)");
            Sql("INSERT INTO PermissionLocations(PermissionID, Type) VALUES('50157D72-8EED-45E4-B6F4-2A935191F57F', 11)");
            Sql("INSERT INTO PermissionLocations(PermissionID, Type) VALUES('50157D72-8EED-45E4-B6F4-2A935191F57F', 20)");
        }
        
        public override void Down()
        {
            Sql("delete from Permissions where ID = 'D59FA0D4-15FA-4088-9A98-35CDD7902EC1'");
            Sql("delete from Permissions where ID = '50157D72-8EED-45E4-B6F4-2A935191F57F'");

            Sql("delete from PermissionLocations where PermissionID = 'D59FA0D4-15FA-4088-9A98-35CDD7902EC1'");
            Sql("delete from PermissionLocations where PermissionID = '50157D72-8EED-45E4-B6F4-2A935191F57F'");
        }
    }
}
