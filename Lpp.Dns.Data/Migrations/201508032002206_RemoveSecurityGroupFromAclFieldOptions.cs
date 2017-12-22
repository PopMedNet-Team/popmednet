namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSecurityGroupFromAclFieldOptions : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE FROM dbo.AclProjectFieldOptions");
            Sql("DELETE FROM dbo.AclGlobalFieldOptions");

            DropForeignKey("dbo.AclGlobalFieldOptions", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclProjectFieldOptions", "SecurityGroupID", "dbo.SecurityGroups");
            DropIndex("dbo.AclGlobalFieldOptions", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclProjectFieldOptions", new[] { "SecurityGroupID" });
            DropPrimaryKey("dbo.AclGlobalFieldOptions");
            DropPrimaryKey("dbo.AclProjectFieldOptions");
            AddPrimaryKey("dbo.AclGlobalFieldOptions", new[] { "FieldIdentifier", "Permission" });
            AddPrimaryKey("dbo.AclProjectFieldOptions", new[] { "FieldIdentifier", "Permission", "ProjectID" });
            DropColumn("dbo.AclGlobalFieldOptions", "SecurityGroupID");
            DropColumn("dbo.AclProjectFieldOptions", "SecurityGroupID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AclProjectFieldOptions", "SecurityGroupID", c => c.Guid(nullable: false));
            AddColumn("dbo.AclGlobalFieldOptions", "SecurityGroupID", c => c.Guid(nullable: false));
            DropPrimaryKey("dbo.AclProjectFieldOptions");
            DropPrimaryKey("dbo.AclGlobalFieldOptions");
            AddPrimaryKey("dbo.AclProjectFieldOptions", new[] { "SecurityGroupID", "FieldIdentifier", "Permission", "ProjectID" });
            AddPrimaryKey("dbo.AclGlobalFieldOptions", new[] { "SecurityGroupID", "FieldIdentifier", "Permission" });
            CreateIndex("dbo.AclProjectFieldOptions", "SecurityGroupID");
            CreateIndex("dbo.AclGlobalFieldOptions", "SecurityGroupID");
            AddForeignKey("dbo.AclProjectFieldOptions", "SecurityGroupID", "dbo.SecurityGroups", "ID", cascadeDelete: true);
            AddForeignKey("dbo.AclGlobalFieldOptions", "SecurityGroupID", "dbo.SecurityGroups", "ID", cascadeDelete: true);
        }
    }
}
