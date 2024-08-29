namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixIsApprovalRequired : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE Organizations SET isApprovalRequired = 0 WHERE isApprovalRequired IS NULL");

            AlterColumn("Organizations", "isApprovalRequired", c => c.Boolean(false, defaultValue: false));
        }
        
        public override void Down()
        {
        }
    }
}
