namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataMartsToGuid1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataMarts", "DataMartTypeSID", c => c.Guid(nullable: false, defaultValueSql: "[dbo].[newsqlguid]()"));
            AddColumn("dbo.DataMartTypes", "SID", c => c.Guid(nullable: false, defaultValueSql: "[dbo].[newsqlguid]()"));
            AddColumn("dbo.RequestDataMartSearchResults", "ResultDataMartSID", c => c.Guid(nullable: false, defaultValueSql: "[dbo].[newsqlguid]()"));
            AddColumn("dbo.Projects_DataMarts", "DataMartSID", c => c.Guid(nullable: false, defaultValueSql: "[dbo].[newsqlguid]()"));
            AddColumn("dbo.RequestRoutingInstances", "DataMartSID", c => c.Guid(nullable: false, defaultValueSql: "[dbo].[newsqlguid]()"));
            AddColumn("dbo.QueriesDataMarts", "DataMartSID", c => c.Guid(nullable: false, defaultValueSql: "[dbo].[newsqlguid]()"));
            AddColumn("dbo.DataMartInstalledModels", "DataMartSID", c => c.Guid(nullable: false, defaultValueSql: "[dbo].[newsqlguid]()"));

            Sql(
                "UPDATE DataMarts SET DataMartTypeSID = (SELECT TOP 1 SID FROM DataMartTypes WHERE DataMartTypeId = DataMarts.DataMartTypeId)");

            Sql(
                "UPDATE RequestDataMartSearchResults SET ResultDataMartSID = (SELECT TOP 1 SID FROM DataMarts WHERE DataMartId = RequestDataMartSearchResults.ResultDataMartId)");
            Sql(
                "UPDATE Projects_DataMarts SET DataMartSID = (SELECT TOP 1 SID FROM DataMarts WHERE DataMartId = Projects_DataMarts.DataMartId)");

            Sql(
                "UPDATE RequestRoutingInstances SET DataMartSID = (SELECT TOP 1 SID FROM DataMarts WHERE DataMartId = RequestRoutingInstances.DataMartId)");

            Sql("UPDATE QueriesDataMarts SET DataMartSID = (SELECT TOP 1 SID FROM DataMarts WHERE DataMartId = QueriesDataMarts.DataMartId)");

            Sql("UPDATE DataMartInstalledModels SET DataMartSID = (SELECT TOP 1 SID FROM DataMarts WHERE DataMartId = DataMartInstalledModels.DataMartId)");
        }
        
        public override void Down()
        {
            DropColumn("dbo.DataMartInstalledModels", "DataMartSID");
            DropColumn("dbo.QueriesDataMarts", "DataMartSID");
            DropColumn("dbo.RequestRoutingInstances", "DataMartSID");
            DropColumn("dbo.Projects_DataMarts", "DataMartSID");
            DropColumn("dbo.RequestDataMartSearchResults", "ResultDataMartSID");
            DropColumn("dbo.DataMartTypes", "SID");
            DropColumn("dbo.DataMarts", "DataMartTypeSID");
        }
    }
}
