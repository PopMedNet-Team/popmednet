namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixGroupApprovalRequiredInDb : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE Groups SET isApprovalRequired = 0 WHERE isApprovalRequired IS NULL");
            AlterColumn("dbo.Groups", "isApprovalRequired", c => c.Boolean(false, false));
        }
        
        public override void Down()
        {
        }
    }
}
