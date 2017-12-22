namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetOldDraftsToPrivate : DbMigration
    {
        public override void Up()
        {
            Sql(@"UPDATE Requests SET private=1, status=200 WHERE status < 300");
        }
        
        public override void Down()
        {
        }
    }
}
