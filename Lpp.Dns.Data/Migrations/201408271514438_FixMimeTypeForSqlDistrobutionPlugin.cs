namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixMimeTypeForSqlDistrobutionPlugin : DbMigration
    {
        public override void Up()
        {
            Sql(@"update Documents SET MimeType = 'text/plain' where Mimetype = 'text/plan'");
        }
        
        public override void Down()
        {
        }
    }
}
