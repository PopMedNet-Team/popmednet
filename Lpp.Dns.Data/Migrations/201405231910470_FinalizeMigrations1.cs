using Lpp.Utilities;

namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinalizeMigrations1 : DbMigration
    {
        public override void Up()
        {
            CreateTable("Permissions", c => new
            {
                ID = c.Guid(false, true, defaultValueSql: MigrationHelpers.GuidDefaultValue),
                TimeStamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                Name = c.String(false, 250),
                Description = c.String()
            }).PrimaryKey(t => t.ID)
                .Index(t => t.Name);

            AddColumn("Activities", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));

            AddColumn("DataMarts", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));

            AddColumn("DataMartTypes", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));

            AddColumn("Documents", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));

            AddColumn("Groups", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));

            AddColumn("Networks", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("Organizations", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("OrganizationElectronicHealthRecordSystems", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("Projects", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("Registries", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("Requests", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("RequestDataMarts", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("RequestDataMartResponseSearchResults", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("RequestDataMartResponses", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("ResponseGroups", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("WorkplanTypes", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("RequesterCenters", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("Roles", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("Users", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));

            AddColumn("SsoEndpoints", "TimeStamp",
                c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));

            Sql(MigrationHelpers.DropPrimaryKeyScript("SsoEndpoints"));
            DropColumn("SsoEndpoints", "ID");
            AddColumn("SsoEndpoints", "ID", c => c.Guid(false, defaultValueSql: MigrationHelpers.GuidDefaultValue));
            AddPrimaryKey("SsoEndpoints", "ID");

            DropColumn("DataMartTypes", "DataMartTypeID");

            Sql(@"  CREATE TRIGGER [dbo].[WorkplanTypesDelete] 
		ON  [dbo].[WorkplanTypes]
		AFTER DELETE
	AS 
	BEGIN
		UPDATE Requests SET WorkPlanTypeID = NULL WHERE WorkPlanTypeID IN (SELECT ID FROM deleted)
	END");
            Sql(@"  CREATE TRIGGER [dbo].[RequesterCentersDelete] 
		ON  [dbo].[RequesterCenters]
		AFTER DELETE
	AS 
	BEGIN
		UPDATE Requests SET RequesterCenterID = NULL WHERE RequesterCenterID IN (SELECT ID FROM deleted)
	END");
        }
        
        public override void Down()
        {
        }
    }
}
