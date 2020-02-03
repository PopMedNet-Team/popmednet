namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDupeGroupIDFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Projects", "FK_Projects_Groups_GroupID");
        }
        
        public override void Down()
        {
        }
    }
}
