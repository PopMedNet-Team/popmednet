namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSecurityGroupsSelfParented : DbMigration
    {
        public override void Up()
        {
            Sql(@"UPDATE SecurityGroups SET ParentSecurityGroupID = NULL WHERE ParentSecurityGroupID = ID");
        }
        
        public override void Down()
        {
        }
    }
}
