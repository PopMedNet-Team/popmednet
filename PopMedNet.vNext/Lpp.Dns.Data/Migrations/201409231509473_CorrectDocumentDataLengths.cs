namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrectDocumentDataLengths : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE Documents SET [Length] = DATALENGTH(Data) WHERE Documents.Length <> DATALENGTH(Data)");
        }
        
        public override void Down()
        {
        }
    }
}
