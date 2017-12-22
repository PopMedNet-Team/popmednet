namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSecurityGroupsTypedWrong : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE SecurityGroups SET Type = CASE WHEN EXISTS(SELECT NULL FROM Projects WHERE ID = SecurityGroups.OwnerID) THEN 2 ELSE 1 END");
        }
        
        public override void Down()
        {
        }
    }
}
