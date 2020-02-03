namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixRequestTypePermissionGrantedProjectLevelReversed : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE AclProjectRequestTypes SET Permission = 4 WHERE Permission = 0");
            Sql("UPDATE AclProjectRequestTypes SET Permission = 0 WHERE Permission = 1");
            Sql("UPDATE AclProjectRequestTypes SET Permission = 1 WHERE Permission = 4");
        }
        
        public override void Down()
        {
        }
    }
}
