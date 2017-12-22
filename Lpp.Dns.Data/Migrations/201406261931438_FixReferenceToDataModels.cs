namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixReferenceToDataModels : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.DataMartInstalledModels", "ModelID");
            AddForeignKey("dbo.DataMartInstalledModels", "ModelID", "dbo.DataModels", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DataMartInstalledModels", "ModelID", "dbo.DataModels");
            DropIndex("dbo.DataMartInstalledModels", new[] { "ModelID" });
        }
    }
}
