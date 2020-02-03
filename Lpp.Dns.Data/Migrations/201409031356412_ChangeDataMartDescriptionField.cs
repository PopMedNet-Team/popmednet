namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDataMartDescriptionField : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DataMarts", "Description", c => c.String(unicode: true));
        }
        
        public override void Down()
        {
        }
    }
}
