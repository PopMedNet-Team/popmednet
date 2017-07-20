namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixFKOnAclRequestType : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.AclRequestTypes");
            AddPrimaryKey("dbo.AclRequestTypes", new[] { "SecurityGroupID", "PermissionID", "RequestTypeID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.AclRequestTypes");
            AddPrimaryKey("dbo.AclRequestTypes", new[] { "SecurityGroupID", "PermissionID" });
        }
    }
}
