namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataPartnerCodeToDataMart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataMarts", "DataPartnerCode", c => c.String(maxLength: 255));
            AlterColumn("dbo.DataMarts", "DataPartnerIdentifier", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DataMarts", "DataPartnerIdentifier", c => c.String());
            DropColumn("dbo.DataMarts", "DataPartnerCode");
        }
    }
}
