namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContentModifiedOnToDocuments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "ContentModifiedOn", c => c.DateTime(nullable: true));
            //Set the modified on date to the create date for existing documents
            Sql("UPDATE Documents SET ContentModifiedOn = CreatedOn");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "ContentModifiedOn");
        }
    }
}
