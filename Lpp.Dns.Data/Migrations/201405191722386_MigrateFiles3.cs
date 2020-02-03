namespace Lpp.Dns.Data.Migrations
{
    using Lpp.Utilities;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateFiles3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "ItemID", c => c.Guid());

            Sql(
    @"UPDATE Documents SET ItemID = CASE WHEN RequestId IS NULL THEN (SELECT TOP 1 SID FROM RequestRoutingInstances WHERE Documents.RoutingInstanceId = RequestRoutingInstances.Id) ELSE (SELECT TOP 1 SID FROM Queries WHERE Queries.QueryId = RequestID) END");

            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = 'Documents' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = 'RequestID')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE Documents DROP CONSTRAINT [' + @FKName + ']'

EXECUTE sp_executeSql @Sql");

            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = 'Documents' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = 'RoutingInstanceID')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE Documents DROP CONSTRAINT [' + @FKName + ']'

EXECUTE sp_executeSql @Sql");

            Sql(MigrationHelpers.DropForeignKeyScript("Documents", "RequestID"));
            DropForeignKey("Documents", "FK_dbo.Documents_dbo.Queries_RequestID");
            CreateIndex("dbo.Documents", "ItemID");
            DropIndex("Documents", "IX_RequestID");
            DropColumn("dbo.Documents", "RequestID");
            DropColumn("dbo.Documents", "RoutingInstanceID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "RoutingInstanceID", c => c.Int());
            DropForeignKey("dbo.Documents", "Request_ID", "dbo.Queries");
            DropForeignKey("dbo.Documents", "RoutingInstance_ID", "dbo.RequestRoutingInstances");
            DropIndex("dbo.Documents", new[] { "RoutingInstance_ID" });
            DropColumn("dbo.Documents", "RoutingInstance_ID");
            DropColumn("dbo.Documents", "ItemID");
            RenameIndex(table: "dbo.Documents", name: "IX_Request_ID", newName: "IX_RequestID");
            RenameColumn(table: "dbo.Documents", name: "Request_ID", newName: "RequestID");
            AddForeignKey("dbo.Documents", "RequestID", "dbo.Queries", "QueryId", cascadeDelete: true);
        }
    }
}
