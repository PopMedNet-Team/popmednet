namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CleanUpRequestTypeSecurity : DbMigration
    {
        public override void Up()
        {
            Sql(@"DELETE AclProjectRequestTypes FROM AclProjectRequestTypes a JOIN Projects ON a.ProjectID = Projects.ID WHERE NOT EXISTS(SELECT NULL FROM ProjectRequestTypes WHERE ProjectRequestTypes.ProjectID = Projects.ID AND ProjectRequestTypes.RequestTypeID = a.RequestTypeID)");

            Sql(@"DELETE AclProjectDataMartRequestTypes FROM AclProjectDataMartRequestTypes a JOIN Projects ON a.ProjectID = Projects.ID WHERE NOT EXISTS(SELECT NULL FROM ProjectRequestTypes WHERE ProjectRequestTypes.ProjectID = Projects.ID AND ProjectRequestTypes.RequestTypeID = a.RequestTypeID)");

            Sql(@"ALTER TRIGGER [dbo].[ProjectRequestTypes_DeleteItem] 
    ON  [dbo].[ProjectRequestTypes]
    AFTER DELETE
AS 
BEGIN
    DELETE AclProjectRequestTypes FROM AclProjectRequestTypes a WHERE EXISTS(SELECT NULL FROM inserted WHERE inserted.ProjectID = a.ProjectID AND inserted.RequestTypeID = a.RequestTypeID)
	DELETE AclProjectDataMartRequestTypes FROM AclProjectRequestTypes a  WHERE EXISTS(SELECT NULL FROM inserted WHERE inserted.ProjectID = a.ProjectID AND inserted.RequestTypeID = a.RequestTypeID)
END
");

        }
        
        public override void Down()
        {
        }
    }
}
