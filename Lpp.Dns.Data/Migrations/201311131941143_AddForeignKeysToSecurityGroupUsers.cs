namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeysToSecurityGroupUsers : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("SecurityGroupUsers", "SecurityGroupID", "SecurityGroups", "SID", true);
            AddForeignKey("SecurityGroupUsers", "UserID", "Users", "UserId", true);
        }
        
        public override void Down()
        {
            DropForeignKey("SecurityGroupUsers", "SecurityGroupID", "SecurityGroups");
            DropForeignKey("SecurityGroupUsers", "UserID", "Users");
        }
    }
}
