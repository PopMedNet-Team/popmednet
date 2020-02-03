namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SimplifyACLByRemovingUserTargets : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ACL", "SecurityGroupID", c => c.Guid(nullable: false));
            AddColumn("dbo.ACL", "Overridden", c => c.Boolean(nullable: false));
            AddColumn("dbo.SecurityGroups", "ParentSecurityGroupID", c => c.Guid());
            DropPrimaryKey("dbo.ACL");
            AddPrimaryKey("dbo.ACL", new[] { "ObjectID", "SecurityGroupID", "AccessControlType" });
            CreateIndex("dbo.SecurityGroups", "ParentSecurityGroupID");
            AddForeignKey("dbo.ACL", "SecurityGroupID", "dbo.SecurityGroups", "SID", cascadeDelete: true);
            AddForeignKey("dbo.SecurityGroups", "ParentSecurityGroupID", "dbo.SecurityGroups", "SID", cascadeDelete: false); //Must use trigger to delete child security groups because of sql limitation
            DropColumn("dbo.ACL", "GroupUserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ACL", "GroupUserID", c => c.Guid(nullable: false));
            DropForeignKey("dbo.SecurityGroups", "ParentSecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.ACL", "SecurityGroupID", "dbo.SecurityGroups");
            DropIndex("dbo.SecurityGroups", new[] { "ParentSecurityGroupID" });
            DropPrimaryKey("dbo.ACL");
            AddPrimaryKey("dbo.ACL", new[] { "ObjectID", "GroupUserID", "AccessControlType" });
            DropColumn("dbo.SecurityGroups", "ParentSecurityGroupID");
            DropColumn("dbo.ACL", "Overridden");
            DropColumn("dbo.ACL", "SecurityGroupID");
        }
    }
}
