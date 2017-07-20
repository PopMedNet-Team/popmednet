namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveNotSelectedFromActivities : DbMigration
    {
        public override void Up()
        {
            //Remove the 'Not Selected' items from the Activities table. This is now handled by the UI.
			//JH -- Commented out because both DC and JH's attempts have negative side effects.

            //Sql("DELETE FROM Activities WHERE Name = 'Not Selected' AND ParentActivityID IS NULL AND NOT EXISTS(SELECT NULL FROM Activities a WHERE a.ParentActivityID = Activities.ID AND Name <> 'Not Select')");
            //Sql("DELETE FROM Activities WHERE NOT EXISTS(SELECT NULL FROM Activities a WHERE a.ID = Activities.ParentActivityID)");
            //Sql("DELETE FROM Activities WHERE NOT EXISTS(SELECT NULL FROM Activities a WHERE a.ID = Activities.ParentActivityID)");
            //Sql("UPDATE Requests SET ActivityID = NULL WHERE NOT ActivityID IS NULL AND NOT EXISTS(SELECT NULL FROM Activities WHERE a.ID = Requests.ActivityID)");
        }
        
        public override void Down()
        {
        }
    }
}
