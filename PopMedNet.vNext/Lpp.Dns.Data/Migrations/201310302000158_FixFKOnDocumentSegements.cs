namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixFKOnDocumentSegements : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TABLE FileDistributionDocumentSegments DROP Constraint FK_FileDistributionDocumentSegments_FileDistributionDocuments");
            Sql(@"ALTER TABLE FileDistributionDocumentSegments ADD Constraint FK_FileDistributionDocumentSegments_FileDistributionDocuments foreign key(Document_Id) references FileDistributionDocuments(Id) on delete cascade on update cascade");

            Sql(@"ALTER TABLE FileDocumentSegments DROP Constraint FK_FileDocumentSegments_FileDocuments");
            Sql(@"ALTER TABLE FileDocumentSegments ADD Constraint FK_FileDocumentSegments_FileDocuments foreign key(Document_Id) references FileDocuments(Id) on delete cascade on update cascade");
        }
        
        public override void Down()
        {
            //None, this should never ever be undone.
        }
    }
}
