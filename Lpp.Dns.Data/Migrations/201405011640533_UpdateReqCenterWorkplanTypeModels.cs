namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReqCenterWorkplanTypeModels : DbMigration
    {
        public override void Up()
        {
            Sql(@"DELETE FROM RequesterCenters");
            Sql(@"DELETE FROM WorkplanTypes");
            CreateIndex("dbo.RequesterCenters", "NetworkID");
            CreateIndex("dbo.WorkplanTypes", "NetworkID");
            AddForeignKey("dbo.RequesterCenters", "NetworkID", "dbo.Networks", "ID", cascadeDelete: true);
            AddForeignKey("dbo.WorkplanTypes", "NetworkID", "dbo.Networks", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkplanTypes", "NetworkID", "dbo.Networks");
            DropForeignKey("dbo.RequesterCenters", "NetworkID", "dbo.Networks");
            DropIndex("dbo.WorkplanTypes", new[] { "NetworkID" });
            DropIndex("dbo.RequesterCenters", new[] { "NetworkID" });
        }
    }
}
