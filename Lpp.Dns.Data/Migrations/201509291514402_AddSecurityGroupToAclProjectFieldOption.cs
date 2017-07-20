namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSecurityGroupToAclProjectFieldOption : DbMigration
    {
        public override void Up()
        {
            //Remove all current entries in table because from this point on SecurityGroupID will be required
            Sql("DELETE FROM dbo.AclProjectFieldOptions");

            AddColumn("dbo.AclProjectFieldOptions", "SecurityGroupID", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AclProjectFieldOptions", "SecurityGroupID");
        }
    }
}
