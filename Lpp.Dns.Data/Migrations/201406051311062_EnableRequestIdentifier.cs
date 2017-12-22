namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnableRequestIdentifier : DbMigration
    {
        public override void Up()
        {

            Sql(@"if exists(select * from sys.columns 
            where Name = N'QueryId' and Object_ID = Object_ID(N'Requests'))
begin
    EXECUTE sp_rename @objname = N'dbo.Requests.QueryId', @newname = N'Identifier', @objtype = N'COLUMN'
    ALTER TABLE dbo.Requests ALTER COLUMN Identifier bigint
end
else
begin
	ALTER TABLE dbo.Requests ADD Identifier bigint IDENTITY
end");
            CreateIndex("dbo.Requests", "Identifier");

            Sql(
                "INSERT INTO RequestTypes (ID, Name, Description, ProcessorID, MetaData, PostProcess, AddFiles, RequiresProcessing) VALUES ('0F1EA012-B588-4775-9E16-CB6DBE12F8BE', 'Diagnosis: PDX', 'Diagnosis: PDX', '5DE1CF20-1CE0-49A2-8767-D8BC5D16D36F', 1, 0, 0, 0)");

            Sql(
                "INSERT INTO RequestTypeModels (RequestTypeID, DataModelID) VALUES ('0F1EA012-B588-4775-9E16-CB6DBE12F8BE', 'CE347EF9-3F60-4099-A221-85084F940EDE')");

            Sql(
                "INSERT INTO RequestTypes (ID, Name, Description, ProcessorID, MetaData, PostProcess, AddFiles, RequiresProcessing) VALUES ('0F1EA013-B588-4775-9E16-CB6DBE12F8BE', 'Dispensing: RxAmt', 'Dispensing: RxAmt', '5DE1CF20-1CE0-49A2-8767-D8BC5D16D36F', 1, 0, 0, 0)");
            Sql(
                "INSERT INTO RequestTypeModels (RequestTypeID, DataModelID) VALUES ('0F1EA013-B588-4775-9E16-CB6DBE12F8BE', 'CE347EF9-3F60-4099-A221-85084F940EDE')");

            Sql(
                "INSERT INTO RequestTypes (ID, Name, Description, ProcessorID, MetaData, PostProcess, AddFiles, RequiresProcessing) VALUES ('0F1EA014-B588-4775-9E16-CB6DBE12F8BE', 'Dispensing: RxSup', 'Dispensing: RxSup', '5DE1CF20-1CE0-49A2-8767-D8BC5D16D36F', 1, 0, 0, 0)");
            Sql(
                "INSERT INTO RequestTypeModels (RequestTypeID, DataModelID) VALUES ('0F1EA014-B588-4775-9E16-CB6DBE12F8BE', 'CE347EF9-3F60-4099-A221-85084F940EDE')");

            Sql(
                "INSERT INTO RequestTypes (ID, Name, Description, ProcessorID, MetaData, PostProcess, AddFiles, RequiresProcessing) VALUES ('0F1EA015-B588-4775-9E16-CB6DBE12F8BE', 'Metadata: Data Completeness', 'Metadata: Data Completeness', '5DE1CF20-1CE0-49A2-8767-D8BC5D16D36F', 1, 0, 0, 0)");
            Sql(
                "INSERT INTO RequestTypeModels (RequestTypeID, DataModelID) VALUES ('0F1EA015-B588-4775-9E16-CB6DBE12F8BE', 'CE347EF9-3F60-4099-A221-85084F940EDE')");

            AddForeignKey("dbo.Requests", "RequestTypeID", "dbo.RequestTypes", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "RequestTypeID", "dbo.RequestTypes");
            DropIndex("dbo.Requests", new[] { "RequestTypeID" });
            DropColumn("dbo.Requests", "Identifier");
        }
    }
}
