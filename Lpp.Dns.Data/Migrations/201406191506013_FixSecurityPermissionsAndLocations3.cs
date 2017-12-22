namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSecurityPermissionsAndLocations3 : DbMigration
    {
        public override void Up()
        {
            //Data Mart
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('5D6DD388-7842-40A1-A27A-B9782A445E20', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('5D6DD388-7842-40A1-A27A-B9782A445E20', 0)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0AC48BA6-4680-40E5-AE7A-F3436B0852A0', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0AC48BA6-4680-40E5-AE7A-F3436B0852A0', 0)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('894619BE-9A73-4DA9-A43A-10BCC563031C', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('894619BE-9A73-4DA9-A43A-10BCC563031C', 0)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0CABF382-93D3-4DAC-AA80-2DE500A5F945', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0CABF382-93D3-4DAC-AA80-2DE500A5F945', 0)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', 0)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('A0F5B621-277A-417C-A862-801D7B9030A2', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('A0F5B621-277A-417C-A862-801D7B9030A2', 0)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE', 0)");

            //Requests
            //Edit
            Sql(@"INSERT INTO Permissions (ID, Name, Description) VALUES ('8B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 'Edit Request', 'Allows the selected group to edit a request.')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('8B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('8B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 6)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('8B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 0)");
            //Still need to find where these are stored and copy them over.

            //Delete
            Sql(@"INSERT INTO Permissions (ID, Name, Description) VALUES ('8C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 'Delete Request', 'Allows the selected group to delete a request.')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('8C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('8C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 6)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('8C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 0)");
            //Still need to find where these are stored and copy them over.


            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('FDEE0BA5-AC09-4580-BAA4-496362985BF7', 0)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('D4494B80-966A-473D-A1B3-4B18BBEF1F34', 0)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('39683790-A857-4247-85DF-A9B425AC79CC', 0)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', 0)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('BDC57049-27BA-41DF-B9F9-A15ABF19B120', 0)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0475D452-4B7A-4D3A-8295-4FC122F6A546', 0)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0549F5C8-6C0E-4491-BE90-EE0F29652422', 0)");
        }
        
        public override void Down()
        {
        }
    }
}
