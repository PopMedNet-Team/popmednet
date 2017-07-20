using Lpp.Utilities;

namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateQueriesToRequests2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Requests", "FK_Queries_Activities");
            DropForeignKey("Requests", "FK_Queries_QueryTypes");
            DropForeignKey("Requests", "FK_Queries_Users");
            Sql(MigrationHelpers.DropForeignKeyScript("Requests", "UpdatedByUserId"));
            Sql(MigrationHelpers.DropForeignKeyScript("Requests", "ProjectID"));
            Sql("DROP TRIGGER QueryDeleteTargets");
            Sql("DROP TRIGGER QueryUpdateTargets");
            Sql("DROP TRIGGER OrganizationDeleteTargets");
            Sql("DROP TRIGGER OrganizationUpdateTargets");

            AlterColumn("ResponseGroups", "Name", c => c.String(false, 255));
            CreateIndex("ResponseGroups", "Name");

            RenameColumn("DataMarts", "DataMartName", "Name");
            AlterColumn("DataMarts", "Name", c => c.String(false, 255));
            CreateIndex("DataMarts", "Name");
            AddColumn("DataMarts", "A", c => c.String(false, 100, defaultValue: ""));
            Sql("UPDATE DataMarts SET A = Acronym WHERE NOT Acronym IS NULL AND Acronym <> ''");
            DropColumn("DataMarts", "Acronym");
            RenameColumn("DataMarts", "A", "Acronym");
            CreateIndex("DataMarts", "Acronym");

            AlterColumn("Requests", "Name", c => c.String(false, 255));
            CreateIndex("Requests", "Name");
            DropForeignKey("Requests", "FK_Queries_QueryTypes");
            DropColumn("Requests", "QueryTypeId");
            DropColumn("Requests", "QueryText");
            AddColumn("Requests", "Description", c => c.String(false, null, defaultValue: ""));
            Sql(
                "UPDATE Requests SET Description = QueryDescription WHERE NOT QueryDescription IS NULL");
            DropColumn("Requests", "QueryDescription");
            //CreateIndex("Requests", "Description");
            DropIndex("Requests", "Queries_Submitted");
            RenameColumn("Requests", "Submitted", "SubmittedOn");
            CreateIndex("Requests", "SubmittedOn");
            Sql("UPDATE Requests SET isAdminQuery = 0 WHERE isAdminQuery IS NULL");
            AlterColumn("Requests", "IsAdminQuery", c => c.Boolean(false, false));
            AddColumn("Requests", "SubmittedByID", c => c.Guid(true));
            Sql("UPDATE Requests SET SubmittedByID = (SELECT TOP 1 SID FROM Users WHERE Users.UserId = Requests.CreatedByUserId) WHERE NOT SubmittedOn IS NULL");
            RenameColumn("Requests", "CreatedAt", "CreatedOn");
            CreateIndex("Requests", "CreatedOn");
            DropIndex("Requests", "Queries_Updated");
            RenameColumn("Requests", "Updated", "UpdatedOn");
            CreateIndex("Requests", "UpdatedOn");
            AlterColumn("Requests", "ActivityOfQuery", c => c.String(true, 255));
            AlterColumn("Requests", "ActivityDescription", c => c.String(true, 255));
            AlterColumn("Requests", "ActivityPriority", c => c.String(true, 50));

            AlterColumn("Groups", "GroupName", c => c.String(false, 255));
            RenameColumn("Groups", "GroupName", "Name");
            CreateIndex("Groups", "Name");

            RenameColumn("Organizations", "OrganizationName", "Name");
            AlterColumn("Organizations", "Name", c => c.String(false, 255));
            CreateIndex("Organizations", "Name");
            RenameColumn("Organizations", "OrganizationAcronym", "Acronym");
            AddColumn("Organizations", "A", c => c.String(false, 100, defaultValue: ""));
            Sql("UPDATE Organizations SET A = Acronym WHERE NOT Acronym IS NULL");
            DropColumn("Organizations", "Acronym");
            RenameColumn("Organizations", "A", "Acronym");

            DropIndex("RequestDataMartResponses", "IX_Count");
            RenameColumn("RequestDataMartResponses", "SubmitTime", "SubmittedOn");
            CreateIndex("RequestDataMartResponses", "SubmittedOn");
            //modify trigger to for column rename
            Sql(@"ALTER TRIGGER [dbo].[RequestDataMartResponsesInsert] 
	ON  [dbo].[RequestDataMartResponses]
	AFTER INSERT
AS 
BEGIN
	UPDATE RequestDataMartResponses SET Count = (SELECT COUNT(*) FROM RequestDataMartResponses r WHERE r.RequestDataMartID = RequestDataMartResponses.RequestDataMartID AND r.SubmittedOn < RequestDataMartResponses.SubmittedOn) + 1 WHERE RequestDataMartResponses.ID IN (SELECT ID FROM inserted)
END");
            AlterColumn("RequestDataMarts", "ErrorMessage", c => c.String(true));
            AlterColumn("RequestDataMarts", "ErrorDetail", c => c.String(true));
            AlterColumn("RequestDataMarts", "RejectReason", c => c.String(true));
            


            AlterColumn("DataAvailabilityPeriod", "Period", c => c.String(false, 100));
            AlterColumn("DataAvailabilityPeriodCategory", "CategoryType", c => c.String(false, 255));
            AlterColumn("DataAvailabilityPeriodCategory", "CategoryDescription", c => c.String(false));

            DropIndex("Documents", "IX_Name");
            AlterColumn("Documents", "Name", c => c.String(false, 255));
            CreateIndex("Documents", "Name");
        }
        
        public override void Down()
        {
        }
    }
}
