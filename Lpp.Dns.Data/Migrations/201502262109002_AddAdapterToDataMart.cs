namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdapterToDataMart : DbMigration
    {
        public override void Up()
        {
            Sql("IF NOT EXISTS(SELECT NULL FROM DataModels WHERE ID = '85EE982E-F017-4BC4-9ACD-EE6EE55D2446' ) INSERT INTO DataModels (ID, Name, RequiresConfiguration) VALUES ('85EE982E-F017-4BC4-9ACD-EE6EE55D2446', 'PCORI', 0)");            

            AddColumn("dbo.DataMarts", "AdapterID", c => c.Guid(nullable: true));
            AddColumn("dbo.DataModels", "QueryComposer", c => c.Boolean(nullable: false, defaultValue: false));
            CreateIndex("dbo.DataMarts", "AdapterID");
            AddForeignKey("dbo.DataMarts", "AdapterID", "dbo.DataModels", "ID");

            Sql("UPDATE DataModels SET QueryComposer = 1 WHERE ID IN ('7C69584A-5602-4FC0-9F3F-A27F329B1113', '1B0FFD4C-3EEF-479D-A5C4-69D8BA0D0154', '4F364773-20A0-4036-800B-841421CB3209', '805DF412-4ACC-4BA0-B0AD-7C4C24FD9F67', '4C99FA21-CDEA-4B09-B95B-EEBDDA05ADEA', '85EE982E-F017-4BC4-9ACD-EE6EE55D2446','455C772A-DF9B-4C6B-A6B0-D4FD4DD98488')");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DataMarts", "AdapterID", "dbo.DataModels");
            DropIndex("dbo.DataMarts", new[] { "AdapterID" });
            DropColumn("dbo.DataModels", "QueryComposer");
            DropColumn("dbo.DataMarts", "AdapterID");
        }
    }
}
