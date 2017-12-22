namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateSecurityGroupInheritence : DbMigration
    {
        public override void Up()
        {
            Sql(
                @"UPDATE SecurityGroups SET ParentSecurityGroupID = (SELECT TOP 1 [End] FROM SecurityMembershipClosure WHERE Start = SecurityGroups.ID AND EXISTS(SELECT NULL FROM SecurityGroups sg WHERE sg.ID = SecurityMembershipClosure.[End]))");
        }
        
        public override void Down()
        {
        }
    }
}
