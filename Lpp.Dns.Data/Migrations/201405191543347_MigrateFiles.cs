namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateFiles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "CreatedOn", c => c.DateTime(nullable: false, defaultValueSql: "GETUTCDATE()"));
            CreateIndex("dbo.Documents", "Name");

            DropColumn("dbo.Documents", "DocId");


        }
        
        public override void Down()
        {
            DropIndex("dbo.Documents", new[] { "Name" });
            DropColumn("dbo.Documents", "CreatedOn");
        }
    }
}
