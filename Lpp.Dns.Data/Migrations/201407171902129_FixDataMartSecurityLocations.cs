namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDataMartSecurityLocations : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM PermissionLocations WHERE PermissionID = '668E7007-E95F-435C-8FAF-0B9FBC9CA997' AND Type = 3");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('668E7007-E95F-435C-8FAF-0B9FBC9CA997', 1)");

            Sql("DELETE FROM PermissionLocations WHERE PermissionID = '6CCB0EC2-006D-4345-895E-5DD2C6C8C791' AND Type = 3");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('6CCB0EC2-006D-4345-895E-5DD2C6C8C791', 1)");

            Sql("DELETE FROM PermissionLocations WHERE PermissionID = '6C019772-1B9D-48F8-9FCD-AC44BC6FD97B' AND Type = 3");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('6C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 1)");

            Sql("DELETE FROM PermissionLocations WHERE PermissionID = '6B42D2D7-F7A7-4119-9CC5-22991DC12AD3' AND Type = 3");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('6B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 1)");

            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('5D6DD388-7842-40A1-A27A-B9782A445E20', 1)");

            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0AC48BA6-4680-40E5-AE7A-F3436B0852A0', 1)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('894619BE-9A73-4DA9-A43A-10BCC563031C', 1)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0CABF382-93D3-4DAC-AA80-2DE500A5F945', 1)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', 1)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('A0F5B621-277A-417C-A862-801D7B9030A2', 1)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE', 1)");

            Sql("DELETE FROM PermissionLocations WHERE PermissionID = '2B42D2D7-F7A7-4119-9CC5-22991DC12AD3' AND Type = 3");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('2B42D2D7-F7A7-4119-9CC5-22991DC12AD3', 9)");

            Sql("DELETE FROM PermissionLocations WHERE PermissionID = '2C019772-1B9D-48F8-9FCD-AC44BC6FD97B' AND Type = 3");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('2C019772-1B9D-48F8-9FCD-AC44BC6FD97B', 9)");

            Sql("DELETE FROM PermissionLocations WHERE PermissionID = '2CCB0EC2-006D-4345-895E-5DD2C6C8C791' AND Type = 3");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('2CCB0EC2-006D-4345-895E-5DD2C6C8C791', 9)");

            Sql("DELETE FROM PermissionLocations WHERE PermissionID = '268E7007-E95F-435C-8FAF-0B9FBC9CA997' AND Type = 3");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('268E7007-E95F-435C-8FAF-0B9FBC9CA997', 9)");
        }
        
        public override void Down()
        {
        }
    }
}
