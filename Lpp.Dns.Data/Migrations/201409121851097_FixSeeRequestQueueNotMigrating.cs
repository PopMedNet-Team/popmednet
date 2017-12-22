namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSeeRequestQueueNotMigrating : DbMigration
    {
        public override void Up()
        {
            // PMN5.0 TWEAKS: ADDED MIN/MAX TO AVOID DUPLICATE KEY PROBLEM. THERE SHOULDN'T BE ANY IN THE ORIGINAL DB ANYWAY.

            Sql(@"INSERT INTO AclProjectDataMarts (SecurityGroupID, PermissionID, ProjectID, DataMartID, Allowed, Overridden)
                SELECT SubjectID, PrivilegeID, ID1, ID3, MIN(CASE WHEN ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries = 0 THEN 0 ELSE 1 END) As Overriden FROM Security_Tuple3 st3 WHERE NOT EXISTS(SELECT NULL FROM AclProjectDataMarts a WHERE a.SecurityGroupID = st3.SubjectID AND a.PermissionID = st3.PrivilegeID AND a.ProjectID = st3.ID1 AND a.DataMartID = st3.ID3) AND st3.ID1 <> '6A690001-7579-4C74-ADE1-A2210107FA29' AND ID3 <> '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' AND PrivilegeID = '5D6DD388-7842-40A1-A27A-B9782A445E20' AND EXISTS(SELECT NULL FROM SecurityGroups WHERE SecurityGroups.ID = st3.SubjectID) GROUP BY SubjectID, PrivilegeID, ID1, ID3");

        }
        
        public override void Down()
        {
        }
    }
}
