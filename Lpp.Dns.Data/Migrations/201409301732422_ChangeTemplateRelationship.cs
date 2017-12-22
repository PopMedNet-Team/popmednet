namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTemplateRelationship : DbMigration
    {
        public override void Up()
        {
            DropTable("RequestTypeTemplates");
            DropTable("ProjectTemplates");
        }
        
        public override void Down()
        {
        }
    }
}
