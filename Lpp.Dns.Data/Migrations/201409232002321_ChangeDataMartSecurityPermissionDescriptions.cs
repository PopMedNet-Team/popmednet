namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDataMartSecurityPermissionDescriptions : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE Permissions SET Description = 'Allows the user see requests for this DataMart and Project on the DMC' WHERE ID='5D6DD388-7842-40A1-A27A-B9782A445E20'");
            Sql("UPDATE Permissions SET Description = 'Allows the user to run and upload results using the DataMart Client' WHERE ID='0AC48BA6-4680-40E5-AE7A-F3436B0852A0'");
            Sql("UPDATE Permissions SET Description = 'Allows the user to place a hold on submitted requests on the DataMart Client' WHERE ID='894619BE-9A73-4DA9-A43A-10BCC563031C'");
            Sql("UPDATE Permissions SET Description = 'Allows the user to reject submitted requests on the DataMart Client' WHERE ID='0CABF382-93D3-4DAC-AA80-2DE500A5F945'");
            Sql("UPDATE Permissions SET Description = 'Allows the user to approve or reject responses' WHERE ID='A58791B5-E8AF-48D0-B9CD-ED0B54E564E6'");
            Sql("UPDATE Permissions SET Description = 'Allows the user to skip response approval when submitting response' WHERE ID='A0F5B621-277A-417C-A862-801D7B9030A2'");
        }
        
        public override void Down()
        {
        }
    }
}
