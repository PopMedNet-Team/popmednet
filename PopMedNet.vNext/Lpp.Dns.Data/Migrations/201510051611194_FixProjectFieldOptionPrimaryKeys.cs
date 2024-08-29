namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixProjectFieldOptionPrimaryKeys : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.AclProjectFieldOptions");
            AddPrimaryKey("dbo.AclProjectFieldOptions", new[] { "FieldIdentifier", "Permission", "ProjectID", "SecurityGroupID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.AclProjectFieldOptions");
            AddPrimaryKey("dbo.AclProjectFieldOptions", new[] { "FieldIdentifier", "Permission", "ProjectID" });
        }
    }
}
