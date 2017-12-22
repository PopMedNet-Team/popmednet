namespace Lpp.Dns.Data.Migrations
{
    using Lpp.Utilities;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestPermissions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AclProjectOrganizations",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        ProjectID = c.Guid(nullable: false),
                        OrganizationID = c.Guid(nullable: false),
                        Allowed = c.Boolean(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.ProjectID, t.OrganizationID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.Organizations", t => t.OrganizationID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.ProjectID)
                .Index(t => t.OrganizationID);
            
            CreateTable(
                "dbo.AclProjectOrganizationUsers",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        ProjectID = c.Guid(nullable: false),
                        OrganizationID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                        Allowed = c.Boolean(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.ProjectID, t.OrganizationID, t.UserID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.Organizations", t => t.OrganizationID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.ProjectID)
                .Index(t => t.OrganizationID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.AclProjectUsers",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        ProjectID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                        Allowed = c.Boolean(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.ProjectID, t.UserID })
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.ProjectID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.AclOrganizationUsers",
                c => new
                    {
                        SecurityGroupID = c.Guid(nullable: false),
                        PermissionID = c.Guid(nullable: false),
                        OrganizationID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                        Allowed = c.Boolean(nullable: false),
                        Overridden = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.SecurityGroupID, t.PermissionID, t.OrganizationID, t.UserID })
                .ForeignKey("dbo.Organizations", t => t.OrganizationID, cascadeDelete: true)
                .ForeignKey("dbo.Permissions", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.SecurityGroups", t => t.SecurityGroupID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.SecurityGroupID)
                .Index(t => t.PermissionID)
                .Index(t => t.OrganizationID)
                .Index(t => t.UserID);

            //Add the permissions
            Sql(
    "INSERT INTO Permissions (ID, Name, Description) VALUES ('FDEE0BA5-AC09-4580-BAA4-496362985BF7', 'Change Routings After Submission', 'Controls who can change the routing of the request after submission')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('FDEE0BA5-AC09-4580-BAA4-496362985BF7', 22)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('FDEE0BA5-AC09-4580-BAA4-496362985BF7', 21)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('FDEE0BA5-AC09-4580-BAA4-496362985BF7', 20)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('FDEE0BA5-AC09-4580-BAA4-496362985BF7', 19)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('FDEE0BA5-AC09-4580-BAA4-496362985BF7', 9)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('FDEE0BA5-AC09-4580-BAA4-496362985BF7', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('FDEE0BA5-AC09-4580-BAA4-496362985BF7', 3)");



            Sql(
    "INSERT INTO Permissions (ID, Name, Description) VALUES ('D4494B80-966A-473D-A1B3-4B18BBEF1F34', 'View Submitted Request Status', 'Controls who can see the status of a submitted request')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('D4494B80-966A-473D-A1B3-4B18BBEF1F34', 22)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('D4494B80-966A-473D-A1B3-4B18BBEF1F34', 21)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('D4494B80-966A-473D-A1B3-4B18BBEF1F34', 20)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('D4494B80-966A-473D-A1B3-4B18BBEF1F34', 19)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('D4494B80-966A-473D-A1B3-4B18BBEF1F34', 9)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('D4494B80-966A-473D-A1B3-4B18BBEF1F34', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('D4494B80-966A-473D-A1B3-4B18BBEF1F34', 3)");


            Sql(
    "INSERT INTO Permissions (ID, Name, Description) VALUES ('39683790-A857-4247-85DF-A9B425AC79CC', 'Skip Request Approval', 'Controls who can submit a request to datamarts directly without prior approval')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('39683790-A857-4247-85DF-A9B425AC79CC', 22)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('39683790-A857-4247-85DF-A9B425AC79CC', 21)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('39683790-A857-4247-85DF-A9B425AC79CC', 20)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('39683790-A857-4247-85DF-A9B425AC79CC', 19)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('39683790-A857-4247-85DF-A9B425AC79CC', 9)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('39683790-A857-4247-85DF-A9B425AC79CC', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('39683790-A857-4247-85DF-A9B425AC79CC', 3)");


            Sql(
    "INSERT INTO Permissions (ID, Name, Description) VALUES ('40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', 'Approve/Reject Submission', 'Controls who can approve or reject requests that have been submitted by a user that cannot skip request approval')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', 22)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', 21)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', 20)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', 19)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', 9)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', 3)");


            Sql(
    "INSERT INTO Permissions (ID, Name, Description) VALUES ('BDC57049-27BA-41DF-B9F9-A15ABF19B120', 'View Request Results', 'Controls who can view the results list on a request')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('BDC57049-27BA-41DF-B9F9-A15ABF19B120', 22)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('BDC57049-27BA-41DF-B9F9-A15ABF19B120', 21)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('BDC57049-27BA-41DF-B9F9-A15ABF19B120', 20)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('BDC57049-27BA-41DF-B9F9-A15ABF19B120', 19)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('BDC57049-27BA-41DF-B9F9-A15ABF19B120', 9)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('BDC57049-27BA-41DF-B9F9-A15ABF19B120', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('BDC57049-27BA-41DF-B9F9-A15ABF19B120', 3)");


            Sql(
    "INSERT INTO Permissions (ID, Name, Description) VALUES ('C025131A-B5EC-46D5-B657-ADE567717A0D', 'View Result details', 'Controls who can view the result details of a request')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('C025131A-B5EC-46D5-B657-ADE567717A0D', 22)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('C025131A-B5EC-46D5-B657-ADE567717A0D', 21)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('C025131A-B5EC-46D5-B657-ADE567717A0D', 20)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('C025131A-B5EC-46D5-B657-ADE567717A0D', 19)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('C025131A-B5EC-46D5-B657-ADE567717A0D', 9)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('C025131A-B5EC-46D5-B657-ADE567717A0D', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('C025131A-B5EC-46D5-B657-ADE567717A0D', 3)");


            Sql(
    "INSERT INTO Permissions (ID, Name, Description) VALUES ('0475D452-4B7A-4D3A-8295-4FC122F6A546', 'View History', 'Controls who can view the history of the request and it''s results')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0475D452-4B7A-4D3A-8295-4FC122F6A546', 22)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0475D452-4B7A-4D3A-8295-4FC122F6A546', 21)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0475D452-4B7A-4D3A-8295-4FC122F6A546', 20)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0475D452-4B7A-4D3A-8295-4FC122F6A546', 19)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0475D452-4B7A-4D3A-8295-4FC122F6A546', 9)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0475D452-4B7A-4D3A-8295-4FC122F6A546', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0475D452-4B7A-4D3A-8295-4FC122F6A546', 3)");


            Sql(
    "INSERT INTO Permissions (ID, Name, Description) VALUES ('0549F5C8-6C0E-4491-BE90-EE0F29652422', 'View Request', 'Controls who can view requests')");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0549F5C8-6C0E-4491-BE90-EE0F29652422', 22)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0549F5C8-6C0E-4491-BE90-EE0F29652422', 21)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0549F5C8-6C0E-4491-BE90-EE0F29652422', 20)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0549F5C8-6C0E-4491-BE90-EE0F29652422', 19)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0549F5C8-6C0E-4491-BE90-EE0F29652422', 9)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0549F5C8-6C0E-4491-BE90-EE0F29652422', 4)");
            Sql("INSERT INTO PermissionLocations (PermissionID, Type) VALUES ('0549F5C8-6C0E-4491-BE90-EE0F29652422', 3)");

            //Users Only
            Sql(@"INSERT INTO AclUsers (UserID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT ID3, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple3 WHERE PrivilegeID IN ('C025131A-B5EC-46D5-B657-ADE567717A0D', '39683790-A857-4247-85DF-A9B425AC79CC', 'D4494B80-966A-473D-A1B3-4B18BBEF1F34', '0475D452-4B7A-4D3A-8295-4FC122F6A546', 'FDEE0BA5-AC09-4580-BAA4-496362985BF7', '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', 'BDC57049-27BA-41DF-B9F9-A15ABF19B120', '0549F5C8-6C0E-4491-BE90-EE0F29652422')
AND ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29' AND ID2 = 'F3AB0001-DEF9-43D1-B862-A22100FE1882' AND EXISTS(SELECT NULL FROM Users WHERE ID = ID3)");


            //Projects Only
            Sql(@"INSERT INTO AclProjects (ProjectID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple3 WHERE PrivilegeID IN ('C025131A-B5EC-46D5-B657-ADE567717A0D', '39683790-A857-4247-85DF-A9B425AC79CC', 'D4494B80-966A-473D-A1B3-4B18BBEF1F34', '0475D452-4B7A-4D3A-8295-4FC122F6A546', 'FDEE0BA5-AC09-4580-BAA4-496362985BF7', '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', 'BDC57049-27BA-41DF-B9F9-A15ABF19B120', '0549F5C8-6C0E-4491-BE90-EE0F29652422')
AND ID3 = '1D3A0001-4717-40A3-98A1-A22100FDE0ED' AND ID2 = 'F3AB0001-DEF9-43D1-B862-A22100FE1882' AND EXISTS(SELECT NULL FROM Projects WHERE ID = ID1) ");

            //Organizations Only
            Sql(@"INSERT INTO AclOrganizations (OrganizationID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT ID2, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple3 WHERE PrivilegeID IN ('C025131A-B5EC-46D5-B657-ADE567717A0D', '39683790-A857-4247-85DF-A9B425AC79CC', 'D4494B80-966A-473D-A1B3-4B18BBEF1F34', '0475D452-4B7A-4D3A-8295-4FC122F6A546', 'FDEE0BA5-AC09-4580-BAA4-496362985BF7', '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', 'BDC57049-27BA-41DF-B9F9-A15ABF19B120', '0549F5C8-6C0E-4491-BE90-EE0F29652422')
AND ID3 = '1D3A0001-4717-40A3-98A1-A22100FDE0ED' AND ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29' AND EXISTS(SELECT NULL FROM Organizations WHERE ID = ID2)");

            //Project Users
            Sql(@"INSERT INTO AclProjectUsers (ProjectID, UserID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT ID1, ID3, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) Overridden FROM Security_Tuple3 WHERE PrivilegeID IN ('C025131A-B5EC-46D5-B657-ADE567717A0D', '39683790-A857-4247-85DF-A9B425AC79CC', 'D4494B80-966A-473D-A1B3-4B18BBEF1F34', '0475D452-4B7A-4D3A-8295-4FC122F6A546', 'FDEE0BA5-AC09-4580-BAA4-496362985BF7', '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', 'BDC57049-27BA-41DF-B9F9-A15ABF19B120', '0549F5C8-6C0E-4491-BE90-EE0F29652422')
AND ID2 = 'F3AB0001-DEF9-43D1-B862-A22100FE1882' AND EXISTS(SELECT NULL FROM Projects WHERE ID = ID1) AND EXISTS(SELECT NULL FROM Users WHERE ID = ID3) group by ID1, ID3, SubjectID, PrivilegeID");


            //Project Organizations
            Sql(@"INSERT INTO AclProjectOrganizations (ProjectID, OrganizationID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT ID1, ID2, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple3 WHERE PrivilegeID IN ('C025131A-B5EC-46D5-B657-ADE567717A0D', '39683790-A857-4247-85DF-A9B425AC79CC', 'D4494B80-966A-473D-A1B3-4B18BBEF1F34', '0475D452-4B7A-4D3A-8295-4FC122F6A546', 'FDEE0BA5-AC09-4580-BAA4-496362985BF7', '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', 'BDC57049-27BA-41DF-B9F9-A15ABF19B120', '0549F5C8-6C0E-4491-BE90-EE0F29652422')
AND ID3 = '1D3A0001-4717-40A3-98A1-A22100FDE0ED' AND EXISTS(SELECT NULL FROM Projects WHERE ID = ID1) AND EXISTS(SELECT NULL FROM Organizations WHERE ID = ID2)");

            //Organization Users
            Sql(@"INSERT INTO AclOrganizationUsers (OrganizationID, UserID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT ID2, ID3, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) Overridden FROM Security_Tuple3 WHERE PrivilegeID IN ('C025131A-B5EC-46D5-B657-ADE567717A0D', '39683790-A857-4247-85DF-A9B425AC79CC', 'D4494B80-966A-473D-A1B3-4B18BBEF1F34', '0475D452-4B7A-4D3A-8295-4FC122F6A546', 'FDEE0BA5-AC09-4580-BAA4-496362985BF7', '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', 'BDC57049-27BA-41DF-B9F9-A15ABF19B120', '0549F5C8-6C0E-4491-BE90-EE0F29652422')
AND ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29' AND EXISTS(SELECT NULL FROM Organizations WHERE ID = ID2) AND EXISTS(SELECT NULL FROM Users WHERE ID = ID3) group by ID2, ID3, SubjectID, PrivilegeID");

            // PMN5.0 TWEAKS: REMOVED AS UNUSED AND IS TIMING OUT ON LARGE DB.
            //Project Organization Users
//            Sql(@"INSERT INTO AclProjectOrganizationUsers (ProjectID, OrganizationID, UserID, SecurityGroupID, PermissionID, Allowed, Overridden)
//SELECT DISTINCT ID1, ID2, ID3, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) Overridden FROM Security_Tuple3 WHERE PrivilegeID IN ('C025131A-B5EC-46D5-B657-ADE567717A0D', '39683790-A857-4247-85DF-A9B425AC79CC', 'D4494B80-966A-473D-A1B3-4B18BBEF1F34', '0475D452-4B7A-4D3A-8295-4FC122F6A546', 'FDEE0BA5-AC09-4580-BAA4-496362985BF7', '40DB7DE2-EEFA-4D31-B400-7E72AB34DE99', 'BDC57049-27BA-41DF-B9F9-A15ABF19B120', '0549F5C8-6C0E-4491-BE90-EE0F29652422') AND EXISTS(SELECT NULL FROM Projects WHERE ID = ID1) AND EXISTS(SELECT NULL FROM Organizations WHERE ID = ID2) AND EXISTS(SELECT NULL FROM Users WHERE ID = ID3) group by ID1, ID2, ID3, SubjectID, PrivilegeID");

            //Update Triggers
            Sql(MigrationHelpers.AddAclDeleteScript("AclOrganizationUsers"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclProjectUsers"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclProjectOrganizationUsers"));
            Sql(MigrationHelpers.AddAclDeleteScript("AclProjectOrganizations"));

            Sql(MigrationHelpers.AddAclInsertScript("AclOrganizationUsers"));
            Sql(MigrationHelpers.AddAclInsertScript("AclProjectUsers"));
            Sql(MigrationHelpers.AddAclInsertScript("AclProjectOrganizationUsers"));
            Sql(MigrationHelpers.AddAclInsertScript("AclProjectOrganizations"));

            Sql(MigrationHelpers.AddAclUpdateScript("AclOrganizationUsers"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclProjectUsers"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclProjectOrganizationUsers"));
            Sql(MigrationHelpers.AddAclUpdateScript("AclProjectOrganizations"));

            //Update the Security Group triggers to include these 4 tables

            Sql(@"ALTER TRIGGER [dbo].[SecurityGroups_DeleteItem] 
        ON  [dbo].[SecurityGroups]
        AFTER DELETE
    AS 
    BEGIN
		UPDATE SecurityGroups SET ParentSecurityGroupID = NULL WHERE ParentSecurityGroupID IN (SELECT ID FROM deleted)

        DELETE FROM AclDataMarts WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclGlobal WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclGroups WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclOrganizationDataMarts WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclOrganizations WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclProjectDataMartRequestTypes WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclProjectDataMarts WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclProjects WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclRegistries WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclRequests WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclRequestSharedFolders WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclRequestTypes WHERE SecurityGroupID IN (SELECT ID FROM deleted)
		DELETE FROM AclUsers WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclUserEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclDataMartEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclGroupEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclOrganizationEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM ProjectEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclRegistryEvents WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclOrganizationUsers WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclProjectUsers WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclProjectOrganizationUsers WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        DELETE FROM AclProjectOrganizations WHERE SecurityGroupID IN (SELECT ID FROM deleted)
        --Add others here
	END");

            Sql(@"ALTER TRIGGER [dbo].[SecurityGroups_InsertItem] 
                       ON  [dbo].[SecurityGroups]
                       AFTER INSERT
                    AS 
                    BEGIN
	                    SET NOCOUNT ON;
	                    --Add the contacts from the inherited group
	                    INSERT INTO SecurityGroupUsers (SecurityGroupID, UserID) SELECT inserted.ID, SecurityGroupUsers.UserID FROM SecurityGroupUsers JOIN inserted ON SecurityGroupUsers.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

	                    --Add the ACL's from the inherited group
                        INSERT INTO AclDataMarts (DataMartID, SecurityGroupID, PermissionID, Allowed) SELECT acl.DataMartID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclDataMarts AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed) SELECT inserted.ID, acl.PermissionID, acl.Allowed FROM AclGlobal AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL
						
						INSERT INTO AclGroups(GroupID, SecurityGroupID, PermissionID, Allowed) SELECT acl.GroupID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclGroups AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclOrganizationDataMarts(OrganizationID, DataMartID, SecurityGroupID, PermissionID, Allowed) SELECT acl.OrganizationID, acl.DataMartID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclOrganizationDataMarts AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclOrganizations(OrganizationID, SecurityGroupID, PermissionID, Allowed) SELECT acl.OrganizationID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclOrganizations AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclProjectDataMartRequestTypes(ProjectID, DataMartID, RequestTypeID, SecurityGroupID, PermissionID, Allowed) SELECT acl.ProjectID, acl.DataMartID, acl.RequestTypeID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclProjectDataMartRequestTypes AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclProjectDataMarts(ProjectID, DataMartID, SecurityGroupID, PermissionID, Allowed) SELECT acl.ProjectID, acl.DataMartID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclProjectDataMarts AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclProjects(ProjectID, SecurityGroupID, PermissionID, Allowed) SELECT acl.ProjectID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclProjects AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclRegistries(RegistryID, SecurityGroupID, PermissionID, Allowed) SELECT acl.RegistryID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclRegistries AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclRequests(RequestID, SecurityGroupID, PermissionID, Allowed) SELECT acl.RequestID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclRequests AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclRequestSharedFolders(RequestSharedFolderID, SecurityGroupID, PermissionID, Allowed) SELECT acl.RequestSharedFolderID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclRequestSharedFolders AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclRequestTypes(RequestTypeID, SecurityGroupID, PermissionID, Allowed) SELECT acl.RequestTypeID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclRequestTypes AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

						INSERT INTO AclUsers(UserID, SecurityGroupID, PermissionID, Allowed) SELECT acl.UserID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclUsers AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclUserEvents(UserID, EventID, SecurityGroupID, PermissionID, Allowed) SELECT acl.UserID, acl.EventID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclUserEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclDataMartEvents(DataMartID, EventID, SecurityGroupID, PermissionID, Allowed) SELECT acl.DataMartID, acl.EventID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclDataMartEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclGroupEvents(GroupID, EventID, SecurityGroupID, PermissionID, Allowed) SELECT acl.GroupID, acl.EventID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclGroupEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL
                        
                        INSERT INTO AclOrganizationEvents(OrganizationID, EventID, SecurityGroupID, PermissionID, Allowed) SELECT acl.OrganizationID, acl.EventID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclOrganizationEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO ProjectEvents(ProjectID, EventID, SecurityGroupID, PermissionID, Allowed) SELECT acl.ProjectID, acl.EventID, inserted.ID, acl.PermissionID, acl.Allowed FROM ProjectEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclRegistryEvents(RegistryID, EventID, SecurityGroupID, PermissionID, Allowed) SELECT acl.RegistryID, acl.EventID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclRegistryEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclEvents(EventID, SecurityGroupID, PermissionID, Allowed) SELECT acl.EventID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclEvents AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclOrganizationUsers(OrganizationID, UserID, SecurityGroupID, PermissionID, Allowed) SELECT acl.OrganizationID, acl.UserID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclOrganizationUsers AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclProjectUsers(ProjectID, UserID, SecurityGroupID, PermissionID, Allowed) SELECT acl.ProjectID, acl.UserID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclProjectUsers AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclProjectOrganizationUsers(ProjectID, OrganizationID, UserID, SecurityGroupID, PermissionID, Allowed) SELECT acl.ProjectID, acl.OrganizationID, acl.UserID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclProjectOrganizationUsers AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL

                        INSERT INTO AclProjectOrganizations(ProjectID, OrganizationID, SecurityGroupID, PermissionID, Allowed) SELECT acl.ProjectID, acl.OrganizationID, inserted.ID, acl.PermissionID, acl.Allowed FROM AclProjectOrganizations AS acl JOIN inserted ON acl.SecurityGroupID = inserted.ParentSecurityGroupID WHERE NOT inserted.ParentSecurityGroupID IS NULL
                        --Add other acl tables here


                    END");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AclOrganizationUsers", "UserID", "dbo.Users");
            DropForeignKey("dbo.AclOrganizationUsers", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclOrganizationUsers", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclOrganizationUsers", "OrganizationID", "dbo.Organizations");
            DropForeignKey("dbo.AclProjectUsers", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.AclProjectOrganizationUsers", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.AclProjectOrganizations", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.AclProjectUsers", "UserID", "dbo.Users");
            DropForeignKey("dbo.AclProjectUsers", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclProjectUsers", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclProjectOrganizationUsers", "UserID", "dbo.Users");
            DropForeignKey("dbo.AclProjectOrganizationUsers", "OrganizationID", "dbo.Organizations");
            DropForeignKey("dbo.AclProjectOrganizationUsers", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclProjectOrganizationUsers", "PermissionID", "dbo.Permissions");
            DropForeignKey("dbo.AclProjectOrganizations", "OrganizationID", "dbo.Organizations");
            DropForeignKey("dbo.AclProjectOrganizations", "SecurityGroupID", "dbo.SecurityGroups");
            DropForeignKey("dbo.AclProjectOrganizations", "PermissionID", "dbo.Permissions");
            DropIndex("dbo.AclOrganizationUsers", new[] { "UserID" });
            DropIndex("dbo.AclOrganizationUsers", new[] { "OrganizationID" });
            DropIndex("dbo.AclOrganizationUsers", new[] { "PermissionID" });
            DropIndex("dbo.AclOrganizationUsers", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclProjectUsers", new[] { "UserID" });
            DropIndex("dbo.AclProjectUsers", new[] { "ProjectID" });
            DropIndex("dbo.AclProjectUsers", new[] { "PermissionID" });
            DropIndex("dbo.AclProjectUsers", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclProjectOrganizationUsers", new[] { "UserID" });
            DropIndex("dbo.AclProjectOrganizationUsers", new[] { "OrganizationID" });
            DropIndex("dbo.AclProjectOrganizationUsers", new[] { "ProjectID" });
            DropIndex("dbo.AclProjectOrganizationUsers", new[] { "PermissionID" });
            DropIndex("dbo.AclProjectOrganizationUsers", new[] { "SecurityGroupID" });
            DropIndex("dbo.AclProjectOrganizations", new[] { "OrganizationID" });
            DropIndex("dbo.AclProjectOrganizations", new[] { "ProjectID" });
            DropIndex("dbo.AclProjectOrganizations", new[] { "PermissionID" });
            DropIndex("dbo.AclProjectOrganizations", new[] { "SecurityGroupID" });
            DropTable("dbo.AclOrganizationUsers");
            DropTable("dbo.AclProjectUsers");
            DropTable("dbo.AclProjectOrganizationUsers");
            DropTable("dbo.AclProjectOrganizations");
        }
    }
}
