namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixRequestTypeSecurityDenieds : DbMigration
    {
        public override void Up()
        {
            Sql(@"TRUNCATE TABLE AclProjectDataMartRequestTypes

INSERT INTO AclProjectDataMartRequestTypes (ProjectID, DataMartID, RequestTypeID, SecurityGroupID, Permission, Overridden)
SELECT DISTINCT ID1, ID3, ID4, SubjectID,
CASE WHEN (SELECT SUM(ExplicitAllowedEntries) FROM Security_Tuple4 st4 WHERE 
st4.ID1 = Security_Tuple4.ID1 AND st4.ID2 = Security_Tuple4.ID2 AND st4.ID3 = Security_Tuple4.ID3 AND st4.ID4 = Security_Tuple4.ID4 AND 
st4.ParentID1 = Security_Tuple4.ParentID1 AND st4.ParentID2 = Security_Tuple4.ParentID2 AND st4.ParentID3 = Security_Tuple4.ParentID3 AND st4.ParentID4 = Security_Tuple4.ParentID4 AND
st4.SubjectID = Security_Tuple4.SubjectID AND PrivilegeID = 'BA5C26D1-448B-4D7D-B237-0E0F04F406E3') > 0 AND (SELECT SUM(ExplicitAllowedEntries) FROM Security_Tuple4 st4 WHERE st4.ID1 = Security_Tuple4.ID1 AND st4.ID2 = Security_Tuple4.ID2 AND st4.ID3 = Security_Tuple4.ID3 AND st4.ID4 = Security_Tuple4.ID4 AND 
st4.ParentID1 = Security_Tuple4.ParentID1 AND st4.ParentID2 = Security_Tuple4.ParentID2 AND st4.ParentID3 = Security_Tuple4.ParentID3 AND st4.ParentID4 = Security_Tuple4.ParentID4 AND
st4.SubjectID = Security_Tuple4.SubjectID AND PrivilegeID = 'FB5EC59C-7129-41C2-8B77-F4E328E1729B') > 0 THEN 3

WHEN (SELECT SUM(ExplicitAllowedEntries) FROM Security_Tuple4 st4 WHERE st4.ID1 = Security_Tuple4.ID1 AND st4.ID2 = Security_Tuple4.ID2 AND st4.ID3 = Security_Tuple4.ID3 AND st4.ID4 = Security_Tuple4.ID4 AND st4.SubjectID = Security_Tuple4.SubjectID AND PrivilegeID = 'BA5C26D1-448B-4D7D-B237-0E0F04F406E3') > 0 THEN 1
WHEN (SELECT SUM(ExplicitAllowedEntries) FROM Security_Tuple4 st4 WHERE st4.ID1 = Security_Tuple4.ID1 AND st4.ID2 = Security_Tuple4.ID2 AND st4.ID3 = Security_Tuple4.ID3 AND st4.ID4 = Security_Tuple4.ID4 AND st4.SubjectID = Security_Tuple4.SubjectID AND PrivilegeID = 'FB5EC59C-7129-41C2-8B77-F4E328E1729B') > 0 THEN 2
ELSE 0
 END AS Permission,
 1 AS Overriden

FROM Security_Tuple4 WHERE 
(PrivilegeID = 'BA5C26D1-448B-4D7D-B237-0E0F04F406E3' OR PrivilegeID = 'FB5EC59C-7129-41C2-8B77-F4E328E1729B')
AND (ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0)
AND EXISTS(SELECT NULL FROM Projects WHERE ID = ID1) AND EXISTS(SELECT NULL FROM DataMarts WHERE ID = ID3) AND EXISTS(SELECT NULL FROM RequestTypes WHERE ID = ID4)
GROUP BY ID1, ID2, ID3, ID4, SubjectID, ParentID1, ParentID2, ParentID3, ParentID4");

            Sql(@"TRUNCATE TABLE AclDataMartRequestTypes

INSERT INTO AclDataMartRequestTypes (DataMartID, RequestTypeID, SecurityGroupID, Permission, Overridden)
SELECT DISTINCT ID3, ID4, SubjectID,
CASE WHEN (SELECT SUM(ExplicitAllowedEntries) FROM Security_Tuple4 st4 WHERE 
st4.ID1 = Security_Tuple4.ID1 AND st4.ID2 = Security_Tuple4.ID2 AND st4.ID3 = Security_Tuple4.ID3 AND st4.ID4 = Security_Tuple4.ID4 AND 
st4.ParentID1 = Security_Tuple4.ParentID1 AND st4.ParentID2 = Security_Tuple4.ParentID2 AND st4.ParentID3 = Security_Tuple4.ParentID3 AND st4.ParentID4 = Security_Tuple4.ParentID4 AND
st4.SubjectID = Security_Tuple4.SubjectID AND PrivilegeID = 'BA5C26D1-448B-4D7D-B237-0E0F04F406E3') > 0 AND (SELECT SUM(ExplicitAllowedEntries) FROM Security_Tuple4 st4 WHERE st4.ID1 = Security_Tuple4.ID1 AND st4.ID2 = Security_Tuple4.ID2 AND st4.ID3 = Security_Tuple4.ID3 AND st4.ID4 = Security_Tuple4.ID4 AND 
st4.ParentID1 = Security_Tuple4.ParentID1 AND st4.ParentID2 = Security_Tuple4.ParentID2 AND st4.ParentID3 = Security_Tuple4.ParentID3 AND st4.ParentID4 = Security_Tuple4.ParentID4 AND
st4.SubjectID = Security_Tuple4.SubjectID AND PrivilegeID = 'FB5EC59C-7129-41C2-8B77-F4E328E1729B') > 0 THEN 3

WHEN (SELECT SUM(ExplicitAllowedEntries) FROM Security_Tuple4 st4 WHERE st4.ID1 = Security_Tuple4.ID1 AND st4.ID2 = Security_Tuple4.ID2 AND st4.ID3 = Security_Tuple4.ID3 AND st4.ID4 = Security_Tuple4.ID4 AND st4.SubjectID = Security_Tuple4.SubjectID AND PrivilegeID = 'BA5C26D1-448B-4D7D-B237-0E0F04F406E3') > 0 THEN 1
WHEN (SELECT SUM(ExplicitAllowedEntries) FROM Security_Tuple4 st4 WHERE st4.ID1 = Security_Tuple4.ID1 AND st4.ID2 = Security_Tuple4.ID2 AND st4.ID3 = Security_Tuple4.ID3 AND st4.ID4 = Security_Tuple4.ID4 AND st4.SubjectID = Security_Tuple4.SubjectID AND PrivilegeID = 'FB5EC59C-7129-41C2-8B77-F4E328E1729B') > 0 THEN 2
ELSE 0
 END AS Permission,
 1 AS Overriden

FROM Security_Tuple4 WHERE 
(PrivilegeID = 'BA5C26D1-448B-4D7D-B237-0E0F04F406E3' OR PrivilegeID = 'FB5EC59C-7129-41C2-8B77-F4E328E1729B')
AND (ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0)
AND ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29' AND EXISTS(SELECT NULL FROM DataMarts WHERE ID = ID3) AND EXISTS(SELECT NULL FROM RequestTypes WHERE ID = ID4)
GROUP BY ID1, ID2, ID3, ID4, SubjectID, ParentID1, ParentID2, ParentID3, ParentID4");

            Sql(@"TRUNCATE TABLE AclRequestTypes

INSERT INTO AclRequestTypes (RequestTypeID, SecurityGroupID, Permission, Overridden)
SELECT DISTINCT ID4, SubjectID,
CASE WHEN (SELECT SUM(ExplicitAllowedEntries) FROM Security_Tuple4 st4 WHERE 
st4.ID1 = Security_Tuple4.ID1 AND st4.ID2 = Security_Tuple4.ID2 AND st4.ID3 = Security_Tuple4.ID3 AND st4.ID4 = Security_Tuple4.ID4 AND 
st4.ParentID1 = Security_Tuple4.ParentID1 AND st4.ParentID2 = Security_Tuple4.ParentID2 AND st4.ParentID3 = Security_Tuple4.ParentID3 AND st4.ParentID4 = Security_Tuple4.ParentID4 AND
st4.SubjectID = Security_Tuple4.SubjectID AND PrivilegeID = 'BA5C26D1-448B-4D7D-B237-0E0F04F406E3') > 0 AND (SELECT SUM(ExplicitAllowedEntries) FROM Security_Tuple4 st4 WHERE st4.ID1 = Security_Tuple4.ID1 AND st4.ID2 = Security_Tuple4.ID2 AND st4.ID3 = Security_Tuple4.ID3 AND st4.ID4 = Security_Tuple4.ID4 AND 
st4.ParentID1 = Security_Tuple4.ParentID1 AND st4.ParentID2 = Security_Tuple4.ParentID2 AND st4.ParentID3 = Security_Tuple4.ParentID3 AND st4.ParentID4 = Security_Tuple4.ParentID4 AND
st4.SubjectID = Security_Tuple4.SubjectID AND PrivilegeID = 'FB5EC59C-7129-41C2-8B77-F4E328E1729B') > 0 THEN 3

WHEN (SELECT SUM(ExplicitAllowedEntries) FROM Security_Tuple4 st4 WHERE st4.ID1 = Security_Tuple4.ID1 AND st4.ID2 = Security_Tuple4.ID2 AND st4.ID3 = Security_Tuple4.ID3 AND st4.ID4 = Security_Tuple4.ID4 AND st4.SubjectID = Security_Tuple4.SubjectID AND PrivilegeID = 'BA5C26D1-448B-4D7D-B237-0E0F04F406E3') > 0 THEN 1
WHEN (SELECT SUM(ExplicitAllowedEntries) FROM Security_Tuple4 st4 WHERE st4.ID1 = Security_Tuple4.ID1 AND st4.ID2 = Security_Tuple4.ID2 AND st4.ID3 = Security_Tuple4.ID3 AND st4.ID4 = Security_Tuple4.ID4 AND st4.SubjectID = Security_Tuple4.SubjectID AND PrivilegeID = 'FB5EC59C-7129-41C2-8B77-F4E328E1729B') > 0 THEN 2
ELSE 0
 END AS Permission,
 1 AS Overriden

FROM Security_Tuple4 WHERE 
(PrivilegeID = 'BA5C26D1-448B-4D7D-B237-0E0F04F406E3' OR PrivilegeID = 'FB5EC59C-7129-41C2-8B77-F4E328E1729B')
AND (ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0)
AND ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29' AND ID3 = '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' AND EXISTS(SELECT NULL FROM RequestTypes WHERE ID = ID4)
GROUP BY ID1, ID2, ID3, ID4, SubjectID, ParentID1, ParentID2, ParentID3, ParentID4");
        }
        
        public override void Down()
        {
        }
    }
}
