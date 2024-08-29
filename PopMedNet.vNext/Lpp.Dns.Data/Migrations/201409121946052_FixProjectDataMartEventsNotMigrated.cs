namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixProjectDataMartEventsNotMigrated : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO ProjectDataMartEvents (SecurityGroupID, EventID, ProjectID, DataMartID, Allowed, Overridden)

SELECT DISTINCT SubjectID, ID4, ID1, ID3, CASE WHEN ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries = 0 THEN 0 ELSE 1 END AS Overriden FROM Security_Tuple4 WHERE PrivilegeID = 'E1A20001-2BE1-48EA-8FC6-A22200E7A7F9'
AND EXISTS(SELECT NULL FROM Projects WHERE Projects.ID = ID1)
AND EXISTS(SELECT NULL FROM DataMarts WHERE DataMarts.ID = ID3)
AND NOT EXISTS(SELECT NULL FROM ProjectDataMartEvents e WHERE E.SecurityGroupID = SubjectID AND e.EventID = ID4 AND e.ProjectID = ID1 AND e.DataMartID = ID3)
AND EXISTS(SELECT NULL FROM SecurityGroups WHERE ID = SubjectID)");

        }
        
        public override void Down()
        {

        }
    }
}
