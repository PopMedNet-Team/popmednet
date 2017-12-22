namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDataMartDescriptionField2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DataMarts", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DataMarts", "Description", c => c.String(maxLength: 510, unicode: false));
        }
    }
}
