namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAclProjectRequesttypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AclProjectRequestTypes",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        RequestTypeID = c.Guid(nullable: false),
                        Permission = c.Int(nullable: false),
                        ProjectID = c.Guid(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.RequestTypeID, t.Permission, t.ProjectID })
                .ForeignKey("dbo.RequestTypes", t => t.RequestTypeID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.RequestTypeID)
                .Index(t => t.ProjectID);
            
            //Add the triggers
            Sql(@"CREATE TRIGGER [dbo].[AclProjectRequestTypes_DeleteItem] 
                        ON  [dbo].[AclProjectRequestTypes]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [AclProjectRequestTypes] a INNER JOIN deleted ON a.RequestTypeID = deleted.RequestTypeID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID AND a.ProjectID = deleted.ProjectID WHERE a.Overridden = 0
						END
                    END");
            Sql(@"CREATE TRIGGER [dbo].[AclProjectRequestTypes_InsertItem] 
                        ON  [dbo].[AclProjectRequestTypes]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [AclProjectRequestTypes] (ProjectID, RequestTypeID, SecurityGroupID, Permission, Overridden) SELECT inserted.ProjectID, inserted.RequestTypeID, SecurityGroups.ID, inserted.Permission, 1 FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID  WHERE NOT EXISTS(SELECT NULL FROM [AclProjectRequestTypes] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.RequestTypeID = inserted.RequestTypeID AND a.ProjectID = inserted.ProjectID)
						END
                    END");

            Sql(@"CREATE TRIGGER [dbo].[AclProjectRequestTypes_UpdateItem] 
                        ON  [dbo].[AclProjectRequestTypes]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [AclProjectRequestTypes] SET [AclProjectRequestTypes].Permission = inserted.Permission FROM [AclProjectRequestTypes] INNER JOIN inserted ON [AclProjectRequestTypes].RequestTypeID = inserted.RequestTypeID AND [AclProjectRequestTypes].ProjectID = inserted.ProjectID JOIN SecurityGroups ON [AclProjectRequestTypes].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [AclProjectRequestTypes].Overridden = 0 
						END
                    END");

            //Migrate data
            //Project DataMart Request Types
            Sql(@"INSERT INTO AclProjectRequestTypes (ProjectID, RequestTypeID, SecurityGroupID, Permission, Overridden)
SELECT DISTINCT ID1, ID4, SubjectID,
CASE WHEN SUM(ExplicitDeniedEntries) > 0 THEN 0 
WHEN (SELECT SUM(ExplicitAllowedEntries) FROM Security_Tuple4 st4 WHERE st4.ID1 = Security_Tuple4.ID1 AND st4.ID2 = Security_Tuple4.ID2 AND st4.ID3 = Security_Tuple4.ID3 AND st4.ID4 = Security_Tuple4.ID4 AND st4.SubjectID = Security_Tuple4.SubjectID AND PrivilegeID = 'BA5C26D1-448B-4D7D-B237-0E0F04F406E3') > 0 AND (SELECT SUM(ExplicitAllowedEntries) FROM Security_Tuple4 st4 WHERE st4.ID1 = Security_Tuple4.ID1 AND st4.ID2 = Security_Tuple4.ID2 AND st4.ID3 = Security_Tuple4.ID3 AND st4.ID4 = Security_Tuple4.ID4 AND st4.SubjectID = Security_Tuple4.SubjectID AND PrivilegeID = 'FB5EC59C-7129-41C2-8B77-F4E328E1729B') > 0 THEN 3

WHEN (SELECT SUM(ExplicitAllowedEntries) FROM Security_Tuple4 st4 WHERE st4.ID1 = Security_Tuple4.ID1 AND st4.ID2 = Security_Tuple4.ID2 AND st4.ID3 = Security_Tuple4.ID3 AND st4.ID4 = Security_Tuple4.ID4 AND st4.SubjectID = Security_Tuple4.SubjectID AND PrivilegeID = 'BA5C26D1-448B-4D7D-B237-0E0F04F406E3') > 0 THEN 1
WHEN (SELECT SUM(ExplicitAllowedEntries) FROM Security_Tuple4 st4 WHERE st4.ID1 = Security_Tuple4.ID1 AND st4.ID2 = Security_Tuple4.ID2 AND st4.ID3 = Security_Tuple4.ID3 AND st4.ID4 = Security_Tuple4.ID4 AND st4.SubjectID = Security_Tuple4.SubjectID AND PrivilegeID = 'FB5EC59C-7129-41C2-8B77-F4E328E1729B') > 0 THEN 2
ELSE -1
 END AS Permission,
 1 AS Overriden

FROM Security_Tuple4 WHERE 
(PrivilegeID = 'BA5C26D1-448B-4D7D-B237-0E0F04F406E3' OR PrivilegeID = 'FB5EC59C-7129-41C2-8B77-F4E328E1729B')
AND (ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0)
AND EXISTS(SELECT NULL FROM Projects WHERE ID = ID1) AND ID3 = '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' AND EXISTS(SELECT NULL FROM RequestTypes WHERE ID = ID4)
GROUP BY ID1, ID2, ID3, ID4, SubjectID");

        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AclProjectRequestTypes", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.AclProjectRequestTypes", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclProjectRequestTypes", "RequestTypeID", "dbo.RequestTypes");
            DropIndex("dbo.AclProjectRequestTypes", new[] { "ProjectID" });
            DropIndex("dbo.AclProjectRequestTypes", new[] { "RequestTypeID" });
            DropIndex("dbo.AclProjectRequestTypes", new[] { "SecurityGroupID" });
            DropTable("dbo.AclProjectRequestTypes");
        }
    }
}
