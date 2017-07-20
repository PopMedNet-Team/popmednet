namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdditionalInstructionsToRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "AdditionalInstructions", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "AdditionalInstructions");
        }
    }
}
