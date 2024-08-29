namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataPartnerIdentifier : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataMarts", "DataPartnerIdentifier", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DataMarts", "DataPartnerIdentifier");
        }
    }
}
