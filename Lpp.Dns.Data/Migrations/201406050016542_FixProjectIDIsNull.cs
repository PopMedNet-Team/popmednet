namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixProjectIDIsNull : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE Requests SET ProjectID = (SELECT TOP 1 ID FROM Projects) WHERE ProjectID IS NULL");
            AlterColumn("Requests", "ProjectID", c => c.Guid(false));
        }
        
        public override void Down()
        {
        }
    }
}
