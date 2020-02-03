namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeLengthOfDescriptionFields : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.DataMarts", new string[] { "DataMartDescription" });
            AlterColumn("dbo.DataMarts", "DataMartDescription", c => c.String());
            AlterColumn("dbo.Registries", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Registries", "Description", c => c.String(maxLength: 500));
            AlterColumn("dbo.DataMarts", "DataMartDescription", c => c.String(maxLength: 440));
        }
    }
}
