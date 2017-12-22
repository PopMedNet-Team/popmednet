namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRequesttypeAclsToUsePermissionEnum2 : DbMigration
    {
        public override void Up()
        {
            //Update/Add the triggers

            //DataMart Request Types
            Sql(@"CREATE TRIGGER [dbo].[AclDataMartRequestTypes_DeleteItem] 
                        ON  [dbo].[AclDataMartRequestTypes]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclDataMartRequestTypes] a INNER JOIN deleted ON a.RequestTypeID = deleted.RequestTypeID AND a.DataMartID = deleted.DataMartID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID WHERE a.Overridden = 0
						END
                    END");
            Sql(@"CREATE TRIGGER [dbo].[AclDataMartRequestTypes_InsertItem] 
                        ON  [dbo].[AclDataMartRequestTypes]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclDataMartRequestTypes] (DataMartID, RequestTypeID, SecurityGroupID, Permission, Overridden) SELECT inserted.DataMartID, inserted.RequestTypeID, SecurityGroups.ID, inserted.Permission, 1 FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID  WHERE NOT EXISTS(SELECT NULL FROM [AclDataMartRequestTypes] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.RequestTypeID = inserted.RequestTypeID AND a.DataMartID = inserted.DataMartID)
						END
                    END");

            Sql(@"CREATE TRIGGER [dbo].[AclDataMartRequestTypes_UpdateItem] 
                        ON  [dbo].[AclDataMartRequestTypes]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [AclDataMartRequestTypes] SET [AclDataMartRequestTypes].Permission = inserted.Permission FROM [AclDataMartRequestTypes] INNER JOIN inserted ON [AclDataMartRequestTypes].RequestTypeID = inserted.RequestTypeID AND [AclDataMartRequestTypes].DataMartID = inserted.DataMartID JOIN SecurityGroups ON [AclDataMartRequestTypes].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [AclDataMartRequestTypes].Overridden = 0
						END
                    END");

            //Project Datamart Request Types
            Sql(@"ALTER TRIGGER [dbo].[AclProjectDataMartRequestTypes_UpdateItem] 
                        ON  [dbo].[AclProjectDataMartRequestTypes]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [AclProjectDataMartRequestTypes] SET [AclProjectDataMartRequestTypes].Permission = inserted.Permission FROM [AclProjectDataMartRequestTypes] INNER JOIN inserted ON [AclProjectDataMartRequestTypes].RequestTypeID = inserted.RequestTypeID AND [AclProjectDataMartRequestTypes].DataMartID = inserted.DataMartID AND [AclProjectDataMartRequestTypes].ProjectID = inserted.ProjectID JOIN SecurityGroups ON [AclProjectDataMartRequestTypes].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [AclProjectDataMartRequestTypes].Overridden = 0 
						END
                    END");

            Sql(@"ALTER TRIGGER [dbo].[AclProjectDataMartRequestTypes_InsertItem] 
                        ON  [dbo].[AclProjectDataMartRequestTypes]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclProjectDataMartRequestTypes] (ProjectID, DataMartID, RequestTypeID, SecurityGroupID, Permission, Overridden) SELECT inserted.ProjectID, inserted.DataMartID, inserted.RequestTypeID, SecurityGroups.ID, inserted.Permission, 1 FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID  WHERE NOT EXISTS(SELECT NULL FROM [AclProjectDataMartRequestTypes] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.RequestTypeID = inserted.RequestTypeID AND a.DataMartID = inserted.DataMartID AND a.ProjectID = inserted.ProjectID)
						END
                    END");

            Sql(@"ALTER TRIGGER [dbo].[AclProjectDataMartRequestTypes_DeleteItem] 
                        ON  [dbo].[AclProjectDataMartRequestTypes]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclProjectDataMartRequestTypes] a INNER JOIN deleted ON a.RequestTypeID = deleted.RequestTypeID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID AND a.DataMartID = deleted.DataMartID AND a.ProjectID = deleted.ProjectID WHERE a.Overridden = 0
						END
                    END");

            //Request Types Only
            Sql(@"ALTER TRIGGER [dbo].[AclRequestTypes_DeleteItem] 
                        ON  [dbo].[AclRequestTypes]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclRequestTypes] a INNER JOIN deleted ON a.Permission = deleted.Permission JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID AND a.RequestTypeID = deleted.RequestTypeID WHERE a.Overridden = 0
						END
                    END");
            Sql(@"ALTER TRIGGER [dbo].[AclRequestTypes_InsertItem] 
                        ON  [dbo].[AclRequestTypes]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclRequestTypes] (SecurityGroupID, RequestTypeID, Permission, Overridden) SELECT SecurityGroups.ID, inserted.RequestTypeID, inserted.Permission, 1 FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [AclRequestTypes] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.RequestTypeID = inserted.RequestTypeID)
						END
                    END");
            Sql(@"ALTER TRIGGER [dbo].[AclRequestTypes_UpdateItem] 
                        ON  [dbo].[AclRequestTypes]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [AclRequestTypes] SET [AclRequestTypes].Permission = inserted.Permission FROM [AclRequestTypes] INNER JOIN inserted ON [AclRequestTypes].RequestTypeID = inserted.RequestTypeID JOIN SecurityGroups ON [AclRequestTypes].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [AclRequestTypes].Overridden = 0
						END
                    END");



            //Translate the data from the old tables.

            //Project DataMart Request Types
            Sql(@"INSERT INTO AclProjectDataMartRequestTypes (ProjectID, DataMartID, RequestTypeID, SecurityGroupID, Permission, Overridden)
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

            //Data Mart Request Types
            Sql(@"INSERT INTO AclDataMartRequestTypes (DataMartID, RequestTypeID, SecurityGroupID, Permission, Overridden)
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

            //Request Types Only
            Sql(@"INSERT INTO AclRequestTypes (RequestTypeID, SecurityGroupID, Permission, Overridden)
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
