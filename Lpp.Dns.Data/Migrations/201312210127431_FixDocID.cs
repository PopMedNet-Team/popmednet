namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDocID : DbMigration
    {
        public override void Up()
        {
            //RenameColumn(table: "dbo.Documents", name: "ID", newName: "DocId");
        }
        
        public override void Down()
        {
            //RenameColumn(table: "dbo.Documents", name: "DocId", newName: "ID");
        }
    }
}
