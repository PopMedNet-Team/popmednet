namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSecurityPermissionsAndLocations2 : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE Permissions SET Name = 'Edit User', Description = 'Allows the selected group to edit the user' WHERE ID = '2B42D2D7-F7A7-4119-9CC5-22991DC12AD3'");
            Sql("UPDATE Permissions SET Name = 'Edit Group', Description = 'Allows the selected security group to edit the current group' WHERE ID = '3B42D2D7-F7A7-4119-9CC5-22991DC12AD3'");

            Sql("UPDATE Permissions SET Name = 'Edit Shared Folder', Description = 'Allows the selected group to edit the shared folder' WHERE ID = '5B42D2D7-F7A7-4119-9CC5-22991DC12AD3'");
            Sql("UPDATE Permissions SET Name = 'Edit DataMart', Description = 'Allows the selected group to edit the DataMart' WHERE ID = '6B42D2D7-F7A7-4119-9CC5-22991DC12AD3'");

            Sql("DELETE FROM Permissions WHERE ID = '7B42D2D7-F7A7-4119-9CC5-22991DC12AD3'"); //Removes dead right.


            Sql("UPDATE Permissions SET Name = 'View User', Description = 'Allows the selected group to view the user' WHERE ID = '2CCB0EC2-006D-4345-895E-5DD2C6C8C791'");
            Sql("UPDATE Permissions SET Name = 'View Group', Description = 'Allows the selected security group to view the current group' WHERE ID = '3CCB0EC2-006D-4345-895E-5DD2C6C8C791'");
            Sql("UPDATE Permissions SET Name = 'View Shared Folder', Description = 'Allows the selected group to view the shared folder' WHERE ID = '5CCB0EC2-006D-4345-895E-5DD2C6C8C791'");
            Sql("UPDATE Permissions SET Name = 'View DataMart', Description = 'Allows the selected group to view the DataMart' WHERE ID = '6CCB0EC2-006D-4345-895E-5DD2C6C8C791'");
            Sql("DELETE FROM Permissions WHERE ID = '7CCB0EC2-006D-4345-895E-5DD2C6C8C791'"); //REmoves dead right

            Sql("UPDATE Permissions SET Name = 'Delete User', Description = 'Allows the selected group to delete the user' WHERE ID = '2C019772-1B9D-48F8-9FCD-AC44BC6FD97B'");
            Sql("UPDATE Permissions SET Name = 'Delete Group', Description = 'Allows the selected security group to delete the current group' WHERE ID = '3C019772-1B9D-48F8-9FCD-AC44BC6FD97B'");
            Sql("UPDATE Permissions SET Name = 'Delete Shared Folder', Description = 'Allows the selected group to delete the shared folder' WHERE ID = '5C019772-1B9D-48F8-9FCD-AC44BC6FD97B'");
            Sql("UPDATE Permissions SET Name = 'Delete DataMart', Description = 'Allows the selected group to delete the DataMart' WHERE ID = '6C019772-1B9D-48F8-9FCD-AC44BC6FD97B'");
            Sql("DELETE FROM Permissions WHERE ID = '7C019772-1B9D-48F8-9FCD-AC44BC6FD97B'"); //REmoves dead right

        }
        
        public override void Down()
        {
        }
    }
}
