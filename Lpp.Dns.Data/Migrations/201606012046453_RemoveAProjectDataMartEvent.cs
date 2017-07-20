namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAProjectDataMartEvent : DbMigration
    {
        public override void Up()
        {

            Sql(@"DELETE FROM ProjectDataMartEvents WHERE EventID = '29AEE006-1C2A-4304-B3C9-8771D96ACDF1'");

        }
        
        public override void Down()
        {
        }
    }
}
