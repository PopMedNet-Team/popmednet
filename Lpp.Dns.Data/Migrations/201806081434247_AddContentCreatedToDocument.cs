namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContentCreatedToDocument : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "ContentCreatedOn", c => c.DateTime(nullable:true));
            Sql("UPDATE Documents SET ContentCreatedOn = CreatedOn");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "ContentCreatedOn");
        }
    }
}
