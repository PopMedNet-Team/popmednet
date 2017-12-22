namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTriggerToCleanUpOrphanedRights : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE TRIGGER [dbo].[ProjectRequestTypes_DeleteItem] 
    ON  [dbo].[ProjectRequestTypes]
    AFTER DELETE
AS 
BEGIN
    DELETE AclProjectRequestTypes FROM AclProjectRequestTypes a JOIN inserted ON a.ProjectID = inserted.ProjectID AND a.RequestTypeID = inserted.RequestTypeID
END
");
        }
        
        public override void Down()
        {
        }
    }
}
