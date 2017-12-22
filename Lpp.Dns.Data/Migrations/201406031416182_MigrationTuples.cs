using Lpp.Utilities;

namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class MigrationTuples : DbMigration
    {
        public override void Up()
        {
            Sql("TRUNCATE TABLE AclGlobal");
            Sql("TRUNCATE TABLE AclOrganizations");
            Sql("TRUNCATE TABLE AclDataMarts");
            Sql("TRUNCATE TABLE AclGroups");
            Sql("TRUNCATE TABLE AclOrganizationDataMarts");
            Sql("TRUNCATE TABLE AclProjectDataMartRequestTypes");
            Sql("TRUNCATE TABLE AclProjectDataMarts");
            Sql("TRUNCATE TABLE AclProjects");
            Sql("TRUNCATE TABLE AclRequests");
            Sql("TRUNCATE TABLE AclRequestSharedFolders");
            Sql("TRUNCATE TABLE AclRequestTypes");
            Sql("TRUNCATE TABLE AclUsers");

            Sql(
    "INSERT INTO Permissions (ID, Name, Description, Type) VALUES ('D68E7007-E95F-435C-8FAF-0B9FBC9CA997', 'Manage Access', 'Controls who can give permissions for the selected item to other users and groups', 0)");

            //Remove the FKs previously created from the Acl tables and add triggers because we have to support no value and we can't put null into a PK so we're going to use Guid.Empty
            Sql(MigrationHelpers.DropForeignKeyScript("AclDataMarts", "DataMartID"));
            DropForeignKey("AclOrganizations", "FK_dbo.AclOrganizations_dbo.Organizations_OrganizationID");
            Sql(MigrationHelpers.DropForeignKeyScript("AclProjects", "ProjectID"));
            Sql(MigrationHelpers.DropForeignKeyScript("AclGroups", "GroupID"));
            Sql(MigrationHelpers.DropForeignKeyScript("AclUsers", "UserID"));
            Sql(MigrationHelpers.DropForeignKeyScript("AclRegistries", "RegistryID"));
            DropForeignKey("AclProjectDataMarts", "FK_dbo.AclProjectDataMarts_dbo.Projects_ProjectID");
            Sql(MigrationHelpers.DropForeignKeyScript("AclProjectDataMarts", "DataMartID"));
            Sql(MigrationHelpers.DropForeignKeyScript("AclProjectDataMartRequestTypes", "ProjectID"));
            Sql(MigrationHelpers.DropForeignKeyScript("AclProjectDataMartRequestTypes", "DataMartID"));
            Sql(MigrationHelpers.DropForeignKeyScript("AclProjectDataMartRequestTypes", "RequestTypeID"));

            var Empty = Guid.Empty.ToString();

            //Global

            //Login
            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '5FBA8EF3-F9A3-4ACC-A3D0-09905FA16E8E'");

            //List Users
            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '5ECFEC21-CD59-4505-B7F2-F52FFC4C263E'");

            //List DataMarts
            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = 'ECD72B1B-50F5-4E3A-BED2-375880435FD1'");

            //List Organizations
            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = 'FFAB8A4A-35FB-4EE7-A946-5874DE13BA58'");

            //List Security Groups
            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '65C197D9-8A69-4350-AA73-C5F6E252C84E'");

            //List Groups
            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = 'FB9B0C98-7BFD-4479-ABE5-0DC093ED44CD'");

            //List Tasks
            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = 'D5C2E426-80C9-40C4-81FB-89ADF85F6362'");

            //Create Organizations
            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '5652252C-0265-4E47-8480-6FEF4690B7A5'");

            //Create Organization Groups
            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '064FBC63-B8F1-4C31-B5AB-AB42DE5779C75'");

            //Create Shared Folders
            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = 'E7EFB727-AE14-49D9-8D73-F691B00B8251'");

            //Run Events Report
            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = 'BA7687E7-E149-4772-8F3F-7C8568769998'");

            //List Registries
            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '860CEFFB-3006-48B1-AC47-60BDC9C3FD35'");

            //Create Registries
            //            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
            //SELECT SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '39A642B4-E782-4051-9329-3A7246052E16'");

            //Create Network
            Sql(@"INSERT INTO AclGlobal (SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '4F3914D9-BD36-4B9F-A6B9-A368199BA94C'");

            //Organization

            //Create Users
            Sql(@"INSERT INTO AclOrganizations (OrganizationID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> 'F3AB0001-DEF9-43D1-B862-A22100FE1882' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = 'AF37A115-9D40-4F38-8BAF-4B050AC6F185'");

            //Create DataMarts
            Sql(@"INSERT INTO AclOrganizations (OrganizationID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT  CASE WHEN ID1 <> 'F3AB0001-DEF9-43D1-B862-A22100FE1882' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '135F153D-D0BE-4D51-B55C-4B8807E74584'");

            //Create Registries
            Sql(@"INSERT INTO AclOrganizations (OrganizationID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> 'F3AB0001-DEF9-43D1-B862-A22100FE1882' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '92F1A228-44E4-4A5A-9C78-0FC37F4B18C6'");

            //Approve/Reject Registrations
            Sql(@"INSERT INTO AclOrganizations (OrganizationID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> 'F3AB0001-DEF9-43D1-B862-A22100FE1882' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = 'ECF3B864-7DB3-497B-A2E4-F2B435EF2803'");

            //Administer WBD
            Sql(@"INSERT INTO AclOrganizations (OrganizationID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> 'F3AB0001-DEF9-43D1-B862-A22100FE1882' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = 'F9870001-7C06-4B4B-8F76-A2A701102FF0'");

            //Copy Organization
            Sql(@"INSERT INTO AclOrganizations (OrganizationID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> 'F3AB0001-DEF9-43D1-B862-A22100FE1882' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '64A00001-A1D6-41DD-AB20-A2B200EEB9A3'");

            //Manage Access
            Sql(@"INSERT INTO AclOrganizations (OrganizationID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> 'F3AB0001-DEF9-43D1-B862-A22100FE1882' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = 'D68E7007-E95F-435C-8FAF-0B9FBC9CA997' AND (ID1 = 'F3AB0001-DEF9-43D1-B862-A22100FE1882' OR EXISTS(SELECT NULL FROM Organizations WHERE ID = Security_Tuple1.ID1))");

            //Edit
            Sql(@"INSERT INTO AclOrganizations (OrganizationID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> 'F3AB0001-DEF9-43D1-B862-A22100FE1882' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '1B42D2D7-F7A7-4119-9CC5-22991DC12AD3' AND (ID1 = 'F3AB0001-DEF9-43D1-B862-A22100FE1882' OR EXISTS(SELECT NULL FROM Organizations WHERE ID = Security_Tuple1.ID1))");

            //Delete
            Sql(@"INSERT INTO AclOrganizations (OrganizationID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> 'F3AB0001-DEF9-43D1-B862-A22100FE1882' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '1C019772-1B9D-48F8-9FCD-AC44BC6FD97B' AND (ID1 = 'F3AB0001-DEF9-43D1-B862-A22100FE1882' OR EXISTS(SELECT NULL FROM Organizations WHERE ID = Security_Tuple1.ID1))");

            //View
            Sql(@"INSERT INTO AclOrganizations (OrganizationID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> 'F3AB0001-DEF9-43D1-B862-A22100FE1882' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '4CCB0EC2-006D-4345-895E-5DD2C6C8C791' AND (ID1 = 'F3AB0001-DEF9-43D1-B862-A22100FE1882' OR EXISTS(SELECT NULL FROM Organizations WHERE ID = Security_Tuple1.ID1))");


            //DataMart

            //Request Metadata
            Sql(@"INSERT INTO AclDataMarts (DataMartID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID2 <> '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' THEN ID2 ELSE '" + Empty + @"' END AS ID2, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) AS Overridden FROM Security_Tuple2 WHERE PrivilegeID = 'F487C17A-873B-489B-A0AC-92EC07976D4A' Group by ID2, SubjectID, PrivilegeID");

            //Install Models
            Sql(@"INSERT INTO AclDataMarts (DataMartID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID2 <> '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' THEN ID2 ELSE '" + Empty + @"' END AS ID2, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) AS Overridden FROM Security_Tuple2 WHERE PrivilegeID = '7710B3EA-B91E-4C85-978F-6BFCDE8C817C' Group by ID2, SubjectID, PrivilegeID");

            //Uninstall Models
            Sql(@"INSERT INTO AclDataMarts (DataMartID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID2 <> '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' THEN ID2 ELSE '" + Empty + @"' END AS ID2, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) AS Overridden FROM Security_Tuple2 WHERE PrivilegeID = 'D4770F67-7DB5-4D47-9413-CA1C777179C9' Group by ID2, SubjectID, PrivilegeID");

            //Run DataMart Audit Report
            Sql(@"INSERT INTO AclDataMarts (DataMartID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID2 <> '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' THEN ID2 ELSE '" + Empty + @"' END AS ID2, SubjectID, PrivilegeID, MIN( CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) AS Overridden FROM Security_Tuple2 WHERE PrivilegeID = 'EFC6DA52-1625-4209-9BBA-5C4BF1D38188' Group by ID2, SubjectID, PrivilegeID");

            //Copy DataMart
            Sql(@"INSERT INTO AclDataMarts (DataMartID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID2 <> '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' THEN ID2 ELSE '" + Empty + @"' END AS ID2, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) AS Overridden FROM Security_Tuple2 WHERE PrivilegeID = 'BB640001-5BA7-4658-93AF-A2B201579BFA' Group by ID2, SubjectID, PrivilegeID");

            //Manage Access
            Sql(@"INSERT INTO AclDataMarts (DataMartID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID2 <> '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' THEN ID2 ELSE '" + Empty + @"' END AS ID2, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) AS Overridden FROM Security_Tuple2 WHERE PrivilegeID = 'D68E7007-E95F-435C-8FAF-0B9FBC9CA997' AND (ID2 = '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' OR EXISTS(SELECT NULL FROM DataMarts WHERE ID = Security_Tuple2.ID2)) Group by ID2, SubjectID, PrivilegeID");

            //Edit
            Sql(@"INSERT INTO AclDataMarts (DataMartID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID2 <> '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' THEN ID2 ELSE '" + Empty + @"' END AS ID2, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END )AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) AS Overridden FROM Security_Tuple2 WHERE PrivilegeID = '1B42D2D7-F7A7-4119-9CC5-22991DC12AD3' AND (ID2 = '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' OR EXISTS(SELECT NULL FROM DataMarts WHERE ID = Security_Tuple2.ID2)) Group by ID2, SubjectID, PrivilegeID");

            //Delete
            Sql(@"INSERT INTO AclDataMarts (DataMartID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID2 <> '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' THEN ID2 ELSE '" + Empty + @"' END AS ID2, SubjectID, PrivilegeID, Min(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) Overridden FROM Security_Tuple2 WHERE PrivilegeID = '1C019772-1B9D-48F8-9FCD-AC44BC6FD97B' AND (ID2 = '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' OR EXISTS(SELECT NULL FROM DataMarts WHERE ID = Security_Tuple2.ID2)) Group by ID2, SubjectID, PrivilegeID");

            //View
            Sql(@"INSERT INTO AclDataMarts (DataMartID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID2 <> '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' THEN ID2 ELSE '" + Empty + @"' END AS ID2, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) AS Overridden FROM Security_Tuple2 WHERE PrivilegeID = '4CCB0EC2-006D-4345-895E-5DD2C6C8C791' AND (ID2 = '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' OR EXISTS(SELECT NULL FROM DataMarts WHERE ID = Security_Tuple2.ID2)) GROUP BY ID2, SubjectID, PrivilegeID");



            //DataMart In Project
            //See Request Queue
            Sql(@"INSERT INTO AclProjectDataMarts (ProjectID, DataMartID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6A690001-7579-4C74-ADE1-A2210107FA29' THEN ID1 ELSE '" + Empty + @"' END AS ID1, ID3, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple3 WHERE PrivilegeID = 'BB640001-5BA7-4658-93AF-A2B201579BFA'");

            //Upload Results
            Sql(@"INSERT INTO AclProjectDataMarts (ProjectID, DataMartID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6A690001-7579-4C74-ADE1-A2210107FA29' THEN ID1 ELSE '" + Empty + @"' END AS ID1, ID3, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) AS Overridden FROM Security_Tuple3 WHERE PrivilegeID = '0AC48BA6-4680-40E5-AE7A-F3436B0852A0' AND (ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29' OR EXISTS(SELECT NULL FROM Projects WHERE ID = Security_Tuple3.ID1)) GROUP BY ID1, ID3, SubjectID, PrivilegeID");

            //Hold Requests
            Sql(@"INSERT INTO AclProjectDataMarts (ProjectID, DataMartID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6A690001-7579-4C74-ADE1-A2210107FA29' THEN ID1 ELSE '" + Empty + @"' END AS ID1, ID3, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple3 WHERE PrivilegeID = '894619BE-9A73-4DA9-A43A-10BCC563031C' AND (ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29' OR EXISTS(SELECT NULL FROM Projects WHERE ID = Security_Tuple3.ID1))");

            //Reject Requests
            Sql(@"INSERT INTO AclProjectDataMarts (ProjectID, DataMartID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6A690001-7579-4C74-ADE1-A2210107FA29' THEN ID1 ELSE '" + Empty + @"' END AS ID1, ID3, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple3 WHERE PrivilegeID = '0CABF382-93D3-4DAC-AA80-2DE500A5F945' AND (ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29' OR EXISTS(SELECT NULL FROM Projects WHERE ID = Security_Tuple3.ID1))");

            //Approve/Reject Response
            Sql(@"INSERT INTO AclProjectDataMarts (ProjectID, DataMartID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6A690001-7579-4C74-ADE1-A2210107FA29' THEN ID1 ELSE '" + Empty + @"' END AS ID1, ID3, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple3 WHERE PrivilegeID = 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6' AND (ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29' OR EXISTS(SELECT NULL FROM Projects WHERE ID = Security_Tuple3.ID1))");

            // PMN5.0 TWEAKS: ADDED MIN/MAX TO AVOID DUPLICATE KEY PROBLEM. THERE SHOULDN'T BE ANY IN THE ORIGINAL DB ANYWAY.
            //Skip Response Approval
            Sql(@"INSERT INTO AclProjectDataMarts (ProjectID, DataMartID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6A690001-7579-4C74-ADE1-A2210107FA29' THEN ID1 ELSE '" + Empty + @"' END AS ID1, ID3, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) Overridden FROM Security_Tuple3 WHERE PrivilegeID = 'A0F5B621-277A-417C-A862-801D7B9030A2' AND (ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29' OR EXISTS(SELECT NULL FROM Projects WHERE ID = Security_Tuple3.ID1)) GROUP BY ID1, ID3, SubjectID, PrivilegeID");

            //Group/Ungroup
            Sql(@"INSERT INTO AclProjectDataMarts (ProjectID, DataMartID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6A690001-7579-4C74-ADE1-A2210107FA29' THEN ID1 ELSE '" + Empty + @"' END AS ID1, ID3, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple3 WHERE PrivilegeID = 'F6D18BEF-BD4F-4484-AAC2-C80DB3A505EE' AND (ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29' OR EXISTS(SELECT NULL FROM Projects WHERE ID = Security_Tuple3.ID1))");

            //User

            //Change Password
            Sql(@"INSERT INTO AclUsers (UserID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID2 <> '1D3A0001-4717-40A3-98A1-A22100FDE0ED' THEN ID2 ELSE '" + Empty + @"' END AS ID2, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) Overridden FROM Security_Tuple2 WHERE PrivilegeID = '4A7C9495-BB01-4EA7-9419-65ACE6B24865' group by ID2, SubjectID, PrivilegeID");

            //Change Login
            Sql(@"INSERT INTO AclUsers (UserID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID2 <> '1D3A0001-4717-40A3-98A1-A22100FDE0ED' THEN ID2 ELSE '" + Empty + @"' END AS ID2, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) Overridden FROM Security_Tuple2 WHERE PrivilegeID = '92687123-6F38-400E-97EC-C837AA92305F' group by ID2, SubjectID, PrivilegeID");

            //Manage Notifications
            Sql(@"INSERT INTO AclUsers (UserID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID2 <> '1D3A0001-4717-40A3-98A1-A22100FDE0ED' THEN ID2 ELSE '" + Empty + @"' END AS ID2, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) Overridden FROM Security_Tuple2 WHERE PrivilegeID = '22FB4F13-0492-417F-ACA1-A1338F705748' group by ID2, SubjectID, PrivilegeID");

            //Change Certificate
            Sql(@"INSERT INTO AclUsers (UserID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID2 <> '1D3A0001-4717-40A3-98A1-A22100FDE0ED' THEN ID2 ELSE '" + Empty + @"' END AS ID2, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) Overridden FROM Security_Tuple2 WHERE PrivilegeID = 'FDE2D32E-A045-4062-9969-00962E182367' group by ID2, SubjectID, PrivilegeID");

            //Manage Access
            Sql(@"INSERT INTO AclUsers (UserID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID2 <> '1D3A0001-4717-40A3-98A1-A22100FDE0ED' THEN ID2 ELSE '" + Empty + @"' END AS ID2, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) Overridden FROM Security_Tuple2 WHERE PrivilegeID = 'D68E7007-E95F-435C-8FAF-0B9FBC9CA997' AND (ID2 = '1D3A0001-4717-40A3-98A1-A22100FDE0ED' OR EXISTS(SELECT NULL FROM Users WHERE ID = Security_Tuple2.ID2)) group by ID2, SubjectID, PrivilegeID");

            //Edit
            Sql(@"INSERT INTO AclUsers (UserID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID2 <> '1D3A0001-4717-40A3-98A1-A22100FDE0ED' THEN ID2 ELSE '" + Empty + @"' END AS ID2, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MIN(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) Overridden FROM Security_Tuple2 WHERE PrivilegeID = '1B42D2D7-F7A7-4119-9CC5-22991DC12AD3' AND (ID2 = '1D3A0001-4717-40A3-98A1-A22100FDE0ED' OR EXISTS(SELECT NULL FROM Users WHERE ID = Security_Tuple2.ID2)) group by ID2, SubjectID, PrivilegeID");

            //Delete
            Sql(@"INSERT INTO AclUsers (UserID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID2 <> '1D3A0001-4717-40A3-98A1-A22100FDE0ED' THEN ID2 ELSE '" + Empty + @"' END AS ID2, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) Overridden FROM Security_Tuple2 WHERE PrivilegeID = '1C019772-1B9D-48F8-9FCD-AC44BC6FD97B' AND (ID2 = '1D3A0001-4717-40A3-98A1-A22100FDE0ED' OR EXISTS(SELECT NULL FROM Users WHERE ID = Security_Tuple2.ID2)) group by ID2, SubjectID, PrivilegeID");

            //View
            Sql(@"INSERT INTO AclUsers (UserID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID2 <> '1D3A0001-4717-40A3-98A1-A22100FDE0ED' THEN ID2 ELSE '" + Empty + @"' END AS ID2, SubjectID, PrivilegeID, MIN(CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END) AS Allowed, MAX(CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END) AS Overridden FROM Security_Tuple2 WHERE PrivilegeID = '4CCB0EC2-006D-4345-895E-5DD2C6C8C791' AND (ID2 = '1D3A0001-4717-40A3-98A1-A22100FDE0ED' OR EXISTS(SELECT NULL FROM Users WHERE ID = Security_Tuple2.ID2)) GROUP BY ID2, SubjectID, PrivilegeID");


            //Group

            //List Projects
            Sql(@"INSERT INTO AclGroups (GroupID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6C380001-FD30-4A47-BC64-A22100FE22EF' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '8C5E44DC-284E-45D8-A014-A0CD815883AE'");

            //Create Projects
            Sql(@"INSERT INTO AclGroups (GroupID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6C380001-FD30-4A47-BC64-A22100FE22EF' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '93623C60-6425-40A0-91A0-01FA34920913'");

            //Manage Access
            Sql(@"INSERT INTO AclGroups (GroupID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6C380001-FD30-4A47-BC64-A22100FE22EF' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = 'D68E7007-E95F-435C-8FAF-0B9FBC9CA997' AND (ID1 = '6C380001-FD30-4A47-BC64-A22100FE22EF' OR EXISTS(SELECT NULL FROM Groups WHERE ID = Security_Tuple1.ID1))");

            //Edit
            Sql(@"INSERT INTO AclGroups (GroupID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6C380001-FD30-4A47-BC64-A22100FE22EF' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '1B42D2D7-F7A7-4119-9CC5-22991DC12AD3' AND (ID1 = '6C380001-FD30-4A47-BC64-A22100FE22EF2' OR EXISTS(SELECT NULL FROM Groups WHERE ID = Security_Tuple1.ID1))");

            //Delete
            Sql(@"INSERT INTO AclGroups (GroupID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6C380001-FD30-4A47-BC64-A22100FE22EF' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '1C019772-1B9D-48F8-9FCD-AC44BC6FD97B' AND (ID1 = '6C380001-FD30-4A47-BC64-A22100FE22EF' OR EXISTS(SELECT NULL FROM Groups WHERE ID = Security_Tuple1.ID1))");

            //View
            Sql(@"INSERT INTO AclGroups (GroupID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6C380001-FD30-4A47-BC64-A22100FE22EF' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '4CCB0EC2-006D-4345-895E-5DD2C6C8C791' AND (ID1 = '6C380001-FD30-4A47-BC64-A22100FE22EF' OR EXISTS(SELECT NULL FROM Groups WHERE ID = Security_Tuple1.ID1))");


            //Project

            //List Requests
            Sql(@"INSERT INTO AclProjects (ProjectID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6A690001-7579-4C74-ADE1-A2210107FA29' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '8DCA22F0-EA18-4353-BA45-CC2692C7A844'");

            //Resubmit Requests
            Sql(@"INSERT INTO AclProjects (ProjectID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6A690001-7579-4C74-ADE1-A2210107FA29' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = 'B3D4266D-5DC6-497E-848F-567442F946F4'");

            //Copy Project
            Sql(@"INSERT INTO AclProjects (ProjectID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6A690001-7579-4C74-ADE1-A2210107FA29' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '25BD0001-4739-41D8-BC74-A2AF01733B64'");

            //Manage Access
            Sql(@"INSERT INTO AclProjects (ProjectID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6A690001-7579-4C74-ADE1-A2210107FA29' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = 'D68E7007-E95F-435C-8FAF-0B9FBC9CA997' AND (ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29' OR EXISTS(SELECT NULL FROM Projects WHERE ID = Security_Tuple1.ID1))");

            //Edit
            Sql(@"INSERT INTO AclProjects (ProjectID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6A690001-7579-4C74-ADE1-A2210107FA29' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '1B42D2D7-F7A7-4119-9CC5-22991DC12AD3' AND (ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29' OR EXISTS(SELECT NULL FROM Projects WHERE ID = Security_Tuple1.ID1))");

            //Delete
            Sql(@"INSERT INTO AclProjects (ProjectID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6A690001-7579-4C74-ADE1-A2210107FA29' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '1C019772-1B9D-48F8-9FCD-AC44BC6FD97B' AND (ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29' OR EXISTS(SELECT NULL FROM Projects WHERE ID = Security_Tuple1.ID1))");

            //View
            Sql(@"INSERT INTO AclProjects (ProjectID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6A690001-7579-4C74-ADE1-A2210107FA29' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '4CCB0EC2-006D-4345-895E-5DD2C6C8C791' AND (ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29' OR EXISTS(SELECT NULL FROM Projects WHERE ID = Security_Tuple1.ID1))");


            //Project, DataMart & Request Type

            //Submit for Manual Processing
            Sql(@"INSERT INTO AclProjectDataMartRequestTypes (ProjectID, DataMartID, RequestTypeID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6A690001-7579-4C74-ADE1-A2210107FA29' THEN ID1 ELSE '" + Empty + @"' END AS ID1, CASE WHEN ID3 <> '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' THEN ID3 ELSE '" + Empty + @"' END AS ID3, ID4, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple4 s WHERE PrivilegeID = 'BA5C26D1-448B-4D7D-B237-0E0F04F406E3'
AND (SELECT COUNT(*) FROM AclProjectDataMartRequestTypes a WHERE a.ProjectID=s.ID1 AND a.DataMartID=s.ID3 AND a.RequestTypeID=s.ID4 AND a.SecurityGroupID=s.SubjectID AND a.PermissionID=s.PrivilegeID) < 0");

            //Submit for automatic processing
            Sql(@"INSERT INTO AclProjectDataMartRequestTypes (ProjectID, DataMartID, RequestTypeID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '6A690001-7579-4C74-ADE1-A2210107FA29' THEN ID1 ELSE '" + Empty + @"' END AS ID1, CASE WHEN ID3 <> '7A0C0001-B9A3-4F4B-9855-A22100FE0BA4' THEN ID3 ELSE '" + Empty + @"' END AS ID3, ID4, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple4 s WHERE PrivilegeID = 'FB5EC59C-7129-41C2-8B77-F4E328E1729B'
AND (SELECT COUNT(*) FROM AclProjectDataMartRequestTypes a WHERE a.ProjectID=s.ID1 AND a.DataMartID=s.ID3 AND a.RequestTypeID=s.ID4 AND a.SecurityGroupID=s.SubjectID AND a.PermissionID=s.PrivilegeID) < 0");

            //Request Shared Folder

            //Temporarily drop FK constraint
            Sql(@"ALTER TABLE [dbo].[AclRequestSharedFolders] DROP CONSTRAINT [FK_dbo.AclRequestSharedFolders_dbo.RequestSharedFolders_RequestSharedFolderID]");

            //Add Requests
            Sql(@"INSERT INTO AclRequestSharedFolders (RequestSharedFolderID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = 'A811302C-9352-45A2-A721-C16E510C4738'");

            //Remove Requests
            Sql(@"INSERT INTO AclRequestSharedFolders (RequestSharedFolderID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '333A8C57-6543-4C6D-B9DA-8B06E186F71D'");

            //Delete any AclRequestSharedFolders request with invalid RequestSharedFolderID
            Sql(@"delete from aclrequestsharedfolders where requestsharedfolderid in (select requestsharedfolderid from aclrequestsharedfolders where requestsharedfolderid not in (select id from requestsharedfolders))");

            Sql(@"ALTER TABLE [dbo].[AclRequestSharedFolders] WITH CHECK  ADD  CONSTRAINT [FK_dbo.AclRequestSharedFolders_dbo.RequestSharedFolders_RequestSharedFolderID] FOREIGN KEY(RequestSharedFolderID)
REFERENCES [dbo].[RequestSharedFolders] (ID)
ON DELETE CASCADE");
            Sql(@"ALTER TABLE [dbo].[AclRequestSharedFolders] CHECK CONSTRAINT [FK_dbo.AclRequestSharedFolders_dbo.RequestSharedFolders_RequestSharedFolderID]");

            //Registry
            //Manage Access
            Sql(@"INSERT INTO AclRegistries (RegistryID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '29CF75D9-1525-48A8-971D-8F9C3B8DDBD1' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = 'D68E7007-E95F-435C-8FAF-0B9FBC9CA997' AND (ID1 = '29CF75D9-1525-48A8-971D-8F9C3B8DDBD1' OR EXISTS(SELECT NULL FROM Registries WHERE ID = Security_Tuple1.ID1))");

            //Edit
            Sql(@"INSERT INTO AclRegistries (RegistryID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '29CF75D9-1525-48A8-971D-8F9C3B8DDBD1' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '1B42D2D7-F7A7-4119-9CC5-22991DC12AD3' AND (ID1 = '29CF75D9-1525-48A8-971D-8F9C3B8DDBD1' OR EXISTS(SELECT NULL FROM Registries WHERE ID = Security_Tuple1.ID1))");

            //Delete
            Sql(@"INSERT INTO AclRegistries (RegistryID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '29CF75D9-1525-48A8-971D-8F9C3B8DDBD1' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '1C019772-1B9D-48F8-9FCD-AC44BC6FD97B' AND (ID1 = '29CF75D9-1525-48A8-971D-8F9C3B8DDBD1' OR EXISTS(SELECT NULL FROM Registries WHERE ID = Security_Tuple1.ID1))");

            //View
            Sql(@"INSERT INTO AclRegistries (RegistryID, SecurityGroupID, PermissionID, Allowed, Overridden)
SELECT DISTINCT CASE WHEN ID1 <> '29CF75D9-1525-48A8-971D-8F9C3B8DDBD1' THEN ID1 ELSE '" + Empty + @"' END AS ID1, SubjectID, PrivilegeID, CASE WHEN DeniedEntries = 0 AND ExplicitDeniedEntries = 0 THEN 1 ELSE 0 END AS Allowed, CASE WHEN ExplicitAllowedEntries > 0 OR ExplicitDeniedEntries > 0 THEN 1 ELSE 0 END Overridden FROM Security_Tuple1 WHERE PrivilegeID = '4CCB0EC2-006D-4345-895E-5DD2C6C8C791' AND (ID1 = '29CF75D9-1525-48A8-971D-8F9C3B8DDBD1' OR EXISTS(SELECT NULL FROM Registries WHERE ID = Security_Tuple1.ID1))");


            //Add triggers to clean up on deletes now that the fk isn't there.

            Sql(@"ALTER TRIGGER [dbo].[DataMartsDelete] 
		ON  [dbo].[DataMarts]
		AFTER DELETE
	AS 
	BEGIN
		DELETE FROM RequestDataMartResponseSearchResults WHERE ItemID IN (SELECT ID FROM deleted)
		DELETE FROM AclDataMarts WHERE DataMartID IN (SELECT ID FROM deleted)
        DELETE FROM AclProjectDataMarts WHERE DataMartID IN (SELECT ID FROM deleted)
        DELETE FROM AclProjectDataMartRequestTypes WHERE DataMartID IN (SELECT ID FROM deleted)
	END");

            Sql(@"ALTER TRIGGER [dbo].[OrganizationDelete] 
    ON  [dbo].[Organizations]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	UPDATE Organizations SET ParentOrganizationID = NULL WHERE ID IN (SELECT ID FROM deleted)
    DELETE FROM Users WHERE OrganizationID IN (SELECT ID FROM deleted)
    DELETE FROM RequestDataMartResponseSearchResults WHERE ItemID IN (SELECT ID FROM deleted)
	DELETE FROM AclOrganizations WHERE OrganizationID IN (SELECT ID FROM deleted)
END");

            Sql("DROP TRIGGER dbo.ProjectDeleteTargets");
            Sql("DROP TRIGGER dbo.ProjectUpdateTargets");

            Sql(@"CREATE TRIGGER [dbo].[ProjectDelete] 
    ON  [dbo].[Projects]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE FROM AclProjects WHERE ProjectID IN (SELECT ID FROM deleted)
    DELETE FROM AclProjectDataMarts WHERE ProjectID IN (SELECT ID FROM deleted) 
    DELETE FROM AclProjectDataMartRequestTypes WHERE ProjectID IN (SELECT ID FROM deleted)
END");

            Sql(@"CREATE TRIGGER [dbo].[GroupDelete] 
    ON  [dbo].[Groups]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE FROM AclGroups WHERE GroupID IN (SELECT ID FROM deleted)
END");

            Sql(@"ALTER TRIGGER [dbo].[Users_DeleteItem] 
        ON  [dbo].[Users]
        AFTER DELETE
    AS 
    BEGIN
		DELETE FROM SecurityGroups WHERE ID IN (SELECT ID FROM deleted)
        DELETE FROM AclUsers WHERE UserID IN (SELECT ID FROM deleted)
	END");

            Sql(@"DROP TRIGGER dbo.RegistriesDeleteTargets");
            Sql(@"DROP TRIGGER dbo.RegistriesUpdateTargets");

            Sql(@"  ALTER TRIGGER [dbo].[RegistriesDelete] 
		ON  [dbo].[Registries]
		AFTER DELETE
	AS 
	BEGIN
		DELETE FROM RequestDataMartResponseSearchResults WHERE ItemID IN (SELECT ID FROM deleted)
        DELETE FROM AclRegistries WHERE RegistryID IN (SELECT ID FROM deleted)
	END");

            Sql(@"CREATE TRIGGER [dbo].[RequestTypeDelete] 
    ON  [dbo].[RequestTypes]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE FROM AclProjectDataMartRequestTypes WHERE RequestTypeID IN (SELECT ID FROM deleted)
END");



            //Events done in next batch as new tables required
        }

        public override void Down()
        {
        }
    }
}