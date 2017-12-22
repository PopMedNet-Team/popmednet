namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixTyposInPrivs : DbMigration
    {
        public override void Up()
        {
            Sql("update Permissions SET Description = 'Allows the security group to make changes to tasks associated with the workflow activity' where ID = '75FC4DEA-220C-486D-9E8C-AC2B6F6F8415'");
            Sql("update Permissions SET Description = 'Allows the security group to close or complete a task associated with the specified workflow activity' where ID = '32DC49AE-E845-4EA9-80CD-CC0281559443'");
        }
        
        public override void Down()
        {
        }
    }
}
