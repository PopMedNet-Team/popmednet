namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataMartInProjectRightsToOrganization : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('5D6DD388-7842-40A1-A27A-B9782A445E20', 3)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0AC48BA6-4680-40E5-AE7A-F3436B0852A0', 3)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('894619BE-9A73-4DA9-A43A-10BCC563031C', 3)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0CABF382-93D3-4DAC-AA80-2DE500A5F945', 3)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('A58791B5-E8AF-48D0-B9CD-ED0B54E564E6', 3)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('A0F5B621-277A-417C-A862-801D7B9030A2', 3)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE', 3)");
        }
        
        public override void Down()
        {
        }
    }
}
