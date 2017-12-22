namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveEmptyFileTables : DbMigration
    {
        public override void Up()
        {
            DropTable("FileDistributionDocumentSegments");
            DropTable("FileDistributionDocuments");
            DropTable("FileDocumentSegments");
            DropTable("FileDocuments");            
        }
        
        public override void Down()
        {
        }
    }
}
