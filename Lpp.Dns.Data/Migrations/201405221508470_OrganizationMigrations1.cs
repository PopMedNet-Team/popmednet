namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrganizationMigrations1 : DbMigration
    {
        public override void Up()
        {
            //Remove


            //OrganizationsGroups
            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1 Tab.CONSTRAINT_NAME from 
    INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, 
    INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col 
WHERE 
    Col.Constraint_Name = Tab.Constraint_Name
    AND Col.Table_Name = Tab.Table_Name
    AND Constraint_Type = 'PRIMARY KEY '
    AND Col.Table_Name = 'OrganizationsGroups')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE OrganizationsGroups DROP CONSTRAINT ' + @FKName

EXECUTE sp_executeSql @Sql");

            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = 'OrganizationsGroups' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = 'GroupId')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE OrganizationsGroups DROP CONSTRAINT ' + @FKName

EXECUTE sp_executeSql @Sql");

            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = 'OrganizationsGroups' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = 'OrganizationId')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE OrganizationsGroups DROP CONSTRAINT ' + @FKName

EXECUTE sp_executeSql @Sql");

            AddColumn("OrganizationsGroups", "OrganizationSID", c => c.Guid(true));
            AddColumn("OrganizationsGroups", "GroupSID", c => c.Guid(true));

            Sql(
                @"UPDATE OrganizationsGroups SET OrganizationSID = (SELECT TOP 1 SID FROM Organizations WHERE OrganizationID = OrganizationsGroups.OrganizationId)");

            Sql(
                @"UPDATE OrganizationsGroups SET GroupSID = (SELECT TOP 1 SID FROM Groups WHERE GroupId = OrganizationsGroups.GroupId)");
            AlterColumn("OrganizationsGroups", "OrganizationSID", c => c.Guid(false));
            AlterColumn("OrganizationsGroups", "GroupSID", c => c.Guid(false));
            DropColumn("OrganizationsGroups", "OrganizationId");
            DropColumn("OrganizationsGroups", "GroupId");
            RenameColumn("OrganizationsGroups", "OrganizationSID", "OrganizationID");
            RenameColumn("OrganizationsGroups", "GroupSID", "GroupID");
            RenameTable("OrganizationsGroups", "OrganizationGroups");

            //Queries
            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = 'Queries' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = 'OrganizationId')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE Queries DROP CONSTRAINT ' + @FKName

EXECUTE sp_executeSql @Sql");
            AddColumn("Queries", "OrganizationSID", c => c.Guid(true));
            Sql(
                @"UPDATE Queries SET OrganizationSID = (SELECT TOP 1 SID FROM Organizations WHERE OrganizationID = Queries.OrganizationId)");
            AlterColumn("Queries", "OrganizationSID", c => c.Guid(false));
            DropIndex("Queries", "ix_org");
            DropIndex("Queries", "Request_Org");
            DropIndex("Queries",
                "_dta_index_Queries_9_98099390__K22D_K2_1_3_6_11_13_15_17_18_19_20_21_23_24_25_26_27_28_29_30");
            Sql(@"DECLARE curAllStats CURSOR  FOR
SELECT Name
FROM   sys.stats
WHERE  object_id = object_id('Queries')
       --AND auto_created <> 0
 
DECLARE  @StatName NVARCHAR(512)
 
OPEN curAllStats
 
FETCH NEXT FROM curAllStats
INTO @StatName
 
WHILE @@FETCH_STATUS = 0
  BEGIN    
    BEGIN TRY
        EXEC( 'drop statistics Queries.' + @StatName)
    END TRY
    BEGIN CATCH
    END CATCH
	Print 'drop statistics Queries.' + @StatName
    
    FETCH NEXT FROM curAllStats
    INTO @StatName
  END -- WHILE
 
CLOSE curAllStats
 
DEALLOCATE curAllStats");

            DropColumn("Queries", "OrganizationId");
            RenameColumn("Queries", "OrganizationSID", "OrganizationID");
            CreateIndex("Queries", "OrganizationID");

            //OrganizationRegistries
            AddColumn("Registries", "SID", c => c.Guid(false, defaultValueSql: "[dbo].[newsqlguid]()")); //Must add it first since it doesn't exist
            DropPrimaryKey("OrganizationsRegistries");
            DropForeignKey("OrganizationsRegistries", "FK_OrganizationsRegistries_Organizations");
            DropForeignKey("OrganizationsRegistries", "FK_OrganizationsRegistries_Registries");
            AddColumn("OrganizationsRegistries", "OrganizationSID", c => c.Guid(true));
            AddColumn("OrganizationsRegistries", "RegistrySID", c => c.Guid(true));
            Sql(
                @"UPDATE OrganizationsRegistries SET OrganizationSID = (SELECT TOP 1 SID FROM Organizations WHERE OrganizationID = OrganizationsRegistries.OrganizationId)");

            Sql(
                @"UPDATE OrganizationsRegistries SET RegistrySID = (SELECT TOP 1 SID FROM Registries WHERE Id = OrganizationsRegistries.RegistryId)");
            AlterColumn("OrganizationsRegistries", "OrganizationSID", c => c.Guid(false));
            AlterColumn("OrganizationsRegistries", "RegistrySID", c => c.Guid(false));
            DropColumn("OrganizationsRegistries", "OrganizationId");
            DropColumn("OrganizationsRegistries", "RegistryId");
            RenameColumn("OrganizationsRegistries", "OrganizationSID", "OrganizationID");
            RenameColumn("OrganizationsRegistries", "RegistrySID", "RegistryID");
            RenameTable("OrganizationsRegistries", "OrganizationRegistries");

            //OrganizationElectronicHealthRecordSystems
            DropForeignKey("OrganizationElectronicHealthRecordSystems", "FK_dbo.OrganizationElectronicHealthRecordSystems_dbo.Organizations_OrganizationID");
            DropIndex("OrganizationElectronicHealthRecordSystems", "IX_OrganizationID");
            AddColumn("OrganizationElectronicHealthRecordSystems", "OrganizationSID", c => c.Guid(true));
            Sql(
                "UPDATE OrganizationElectronicHealthRecordSystems SET OrganizationSID = (SELECT TOP 1 SID FROM Organizations WHERE OrganizationId = OrganizationElectronicHealthRecordSystems.OrganizationId)");
            DropColumn("OrganizationElectronicHealthRecordSystems", "OrganizationId");
            AlterColumn("OrganizationElectronicHealthRecordSystems", "OrganizationSID", c => c.Guid(false));
            RenameColumn("OrganizationElectronicHealthRecordSystems", "OrganizationSID", "OrganizationID");

            //RegistryItemDefinitions
            AddColumn("RegistryItemDefinitions", "SID", c => c.Guid(false, defaultValueSql: "[dbo].[newsqlguid]()"));            


            //Registries_RegistryItemDefinitions
            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1 Tab.CONSTRAINT_NAME from 
    INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, 
    INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col 
WHERE 
    Col.Constraint_Name = Tab.Constraint_Name
    AND Col.Table_Name = Tab.Table_Name
    AND Constraint_Type = 'PRIMARY KEY '
    AND Col.Table_Name = 'Registries_RegistryItemDefinitions')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE Registries_RegistryItemDefinitions DROP CONSTRAINT ' + @FKName

EXECUTE sp_executeSql @Sql");
            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = 'Registries_RegistryItemDefinitions' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = 'Registry_Id')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE Registries_RegistryItemDefinitions DROP CONSTRAINT ' + @FKName

EXECUTE sp_executeSql @Sql");
            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = 'Registries_RegistryItemDefinitions' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = 'RegistryItemDefinition_Id')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE Registries_RegistryItemDefinitions DROP CONSTRAINT ' + @FKName

EXECUTE sp_executeSql @Sql");
            AddColumn("Registries_RegistryItemDefinitions", "RegistryID", c => c.Guid(true));
            AddColumn("Registries_RegistryItemDefinitions", "RegistryItemDefinitionID", c => c.Guid(true));
            Sql(
                @"UPDATE Registries_RegistryItemDefinitions SET RegistryID = (SELECT TOP 1 SID FROM Registries WHERE id = Registries_RegistryItemDefinitions.Registry_Id)");
            Sql(
    @"UPDATE Registries_RegistryItemDefinitions SET RegistryItemDefinitionID = (SELECT TOP 1 SID FROM RegistryItemDefinitions WHERE id = Registries_RegistryItemDefinitions.RegistryItemDefinition_Id)");

            AlterColumn("Registries_RegistryItemDefinitions", "RegistryID", c => c.Guid(false));
            AlterColumn("Registries_RegistryItemDefinitions", "RegistryItemDefinitionID", c => c.Guid(false));
            DropColumn("Registries_RegistryItemDefinitions", "Registry_Id");            
            DropColumn("Registries_RegistryItemDefinitions", "RegistryItemDefinition_Id");
            RenameTable("Registries_RegistryItemDefinitions", "RegistryDefinitions");           

            //Projects (for Group)
            AddColumn("Projects", "GroupSID", c => c.Guid(true));
            Sql(@"UPDATE Projects SET GroupSID = (SELECT TOP 1 SID FROM Groups WHERE GroupId = Projects.GroupId)");
            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = 'Projects' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = 'GroupId')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE Projects DROP CONSTRAINT ' + @FKName

EXECUTE sp_executeSql @Sql");
            DropIndex("Projects", "IX_GroupID");
            DropColumn("Projects", "GroupId");
            RenameColumn("Projects", "GroupSID", "GroupID");
            CreateIndex("Projects", "GroupID");

            //RequestRegistrySearchResults
            DropIndex("RequestRegistrySearchResults", "IX_ResultRegistryId");
            AddColumn("RequestRegistrySearchResults", "ResultRegistrySID", c => c.Guid(true));
            Sql(
                "UPDATE RequestRegistrySearchResults set ResultRegistrySID = (SELECT TOP 1 SID FROM Registries WHERE Id = RequestRegistrySearchResults.ResultRegistryId)");
            AlterColumn("RequestRegistrySearchResults", "ResultRegistrySID", c => c.Guid(false));
            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = 'RequestRegistrySearchResults' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = 'ResultRegistryID')

PRINT @FKName
DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE RequestRegistrySearchResults DROP CONSTRAINT [' + @FKName + ']'

EXECUTE sp_executeSql @Sql");

            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = 'RequestRegistrySearchResults' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = 'ResultRegistryID')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE RequestRegistrySearchResults DROP CONSTRAINT ' + @FKName

EXECUTE sp_executeSql @Sql");
            DropPrimaryKey("RequestRegistrySearchResults", "PK_dbo.RequestRegistrySearchResults");
            DropColumn("RequestRegistrySearchResults", "ResultRegistryID");
            RenameColumn("RequestRegistrySearchResults", "ResultRegistrySID", "ResultRegistryID");

            //SecurityGroups
            AddColumn("SecurityGroups", "OrganizationSID", c => c.Guid(true));
            Sql(
                @"UPDATE SecurityGroups SET OrganizationSID = (SELECT TOP 1 SID FROM Organizations WHERE OrganizationId = SecurityGroups.OrganizationId)");
            AlterColumn("SecurityGroups", "OrganizationSID", c => c.Guid(false));
            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = 'SecurityGroups' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = 'OrganizationId')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE SecurityGroups DROP CONSTRAINT [' + @FKName + ']'

EXECUTE sp_executeSql @Sql");

            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = 'SecurityGroups' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = 'OrganizationId')

IF (NOT @FKName IS NULL)
BEGIN
    DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE SecurityGroups DROP CONSTRAINT [' + @FKName + ']'

    EXECUTE sp_executeSql @Sql
END");


            DropIndex("SecurityGroups", "IX_OrganizationID");
            DropColumn("SecurityGroups", "OrganizationId");
            RenameColumn("SecurityGroups", "OrganizationSID", "OrganizationID");
            CreateIndex("SecurityGroups", "OrganizationID");
            
            //Groups
            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1 Tab.CONSTRAINT_NAME from 
    INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, 
    INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col 
WHERE 
    Col.Constraint_Name = Tab.Constraint_Name
    AND Col.Table_Name = Tab.Table_Name
    AND Constraint_Type = 'PRIMARY KEY '
    AND Col.Table_Name = 'Groups')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE Groups DROP CONSTRAINT ' + @FKName

EXECUTE sp_executeSql @Sql");
            DropColumn("Groups", "GroupId");
            RenameColumn("Groups", "SID", "ID");
            AddPrimaryKey("Groups", "ID");

            //Registries
            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1 Tab.CONSTRAINT_NAME from 
    INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, 
    INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col 
WHERE 
    Col.Constraint_Name = Tab.Constraint_Name
    AND Col.Table_Name = Tab.Table_Name
    AND Constraint_Type = 'PRIMARY KEY '
    AND Col.Table_Name = 'Registries')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE Registries DROP CONSTRAINT ' + @FKName

EXECUTE sp_executeSql @Sql");
            DropColumn("Registries", "id");
            RenameColumn("Registries", "SID", "ID");
            AddPrimaryKey("Registries", "ID");

            //DataMarts
            DropIndex("DataMarts", "IX_OrganizationID");
            DropIndex("DataMarts", "DataMart_Org");
            AddColumn("DataMarts", "OrganizationSID", c => c.Guid(true));
            Sql(
                "UPDATE DataMarts SET OrganizationSID = (SELECT TOP 1 SID FROM Organizations WHERE Organizationid = DAtaMarts.OrganizationId)");
            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = 'DataMarts' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = 'OrganizationId')

IF (NOT @FKName IS NULL)
BEGIN
    DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE DataMarts DROP CONSTRAINT [' + @FKName + ']'

    EXECUTE sp_executeSql @Sql
END");
            DropColumn("DataMarts", "OrganizationId");
            AlterColumn("DataMarts", "OrganizationSID", c => c.Guid(false));
            RenameColumn("DataMarts", "OrganizationSID", "OrganizationID");

            //RequestOrganizationSearchResults
            DropIndex("RequestOrganzationSearchResults", "IX_ResultOrganizationId");
            AddColumn("RequestOrganzationSearchResults", "ResultOrganizationSID", c => c.Guid(true));
            Sql(
                "UPDATE RequestOrganzationSearchResults SET ResultOrganizationSID = (SELECT TOP 1 SID FROM Organizations AS o where o.OrganizationId = RequestOrganzationSearchResults.ResultOrganizationID)");
            DropPrimaryKey("RequestOrganzationSearchResults", "PK_dbo.RequestOrganzationSearchResults");
            DropForeignKey("RequestOrganzationSearchResults", "ResultOrganizationId");
            DropForeignKey("RequestOrganzationSearchResults", "FK_dbo.RequestOrganzationSearchResults_dbo.Organizations_ResultOrganizationId");
            DropColumn("RequestOrganzationSearchResults", "ResultOrganizationId");
            AlterColumn("RequestOrganzationSearchResults", "ResultOrganizationSID", c => c.Guid(false));
            RenameColumn("RequestOrganzationSearchResults", "ResultOrganizationSID", "ResultOrganizationID");

            //Users
            AddColumn("Users", "OrganizationSID", c => c.Guid(true));
            Sql("UPDATE Users SET OrganizationSID = (SELECT TOP 1 SID FROM Organizations AS o where o.OrganizationId = Users.OrganizationId)");
            DropForeignKey("Users", "FK_Users_Organizations_OrgId");
            DropIndex("Users", "_dta_index_Users_16_1461580245__K1_K17_K5");
            DropIndex("Users", "User_Org");
            AlterColumn("Users", "OrganizationSID", c => c.Guid(false));
            Sql(@"DECLARE curAllStats CURSOR  FOR
SELECT Name
FROM   sys.stats
WHERE  object_id = object_id('Users')
       --AND auto_created <> 0
 
DECLARE  @StatName NVARCHAR(512)
 
OPEN curAllStats
 
FETCH NEXT FROM curAllStats
INTO @StatName
 
WHILE @@FETCH_STATUS = 0
  BEGIN    
    BEGIN TRY
        EXEC( 'drop statistics Users.' + @StatName)
    END TRY
    BEGIN CATCH
    END CATCH
	Print 'drop statistics Users.' + @StatName
    
    FETCH NEXT FROM curAllStats
    INTO @StatName
  END -- WHILE
 
CLOSE curAllStats
 
DEALLOCATE curAllStats");

            DropColumn("Users", "OrganizationId");
            RenameColumn("Users", "OrganizationSID", "OrganizationID");

            //Organizations            
            DropIndex("Organizations", "ix_sid");
            AddColumn("Organizations", "ParentOrganizationID", c => c.Guid(true));
            Sql(
                @"UPDATE Organizations SET ParentOrganizationID = (SELECT TOP 1 SID FROM Organizations AS o where o.OrganizationId = Organizations.ParentId)");
            Sql(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = 'Organizations' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = 'ParentId')

IF (NOT @FKName IS NULL)
BEGIN
    DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE Organizations DROP CONSTRAINT [' + @FKName + ']'

    EXECUTE sp_executeSql @Sql
END");
            DropPrimaryKey("Organizations", "PK_Organizations_OrganizationId");
            DropColumn("Organizations", "ParentId");            
            DropColumn("Organizations", "OrganizationId");
            RenameColumn("Organizations", "SID", "ID");
            AddPrimaryKey("Organizations", "ID");
            AddForeignKey("Organizations", "ParentOrganizationID", "Organizations", "ID");

            //Add

            //RegistryItemDefinitions
            DropPrimaryKey("RegistryItemDefinitions");
            RenameTable("RegistryItemDefinitions", "RegistryItemDefinitionLookup");
            //Drop ID Column
            DropColumn("RegistryItemDefinitionLookup", "Id");
            //Rename SID
            RenameColumn("RegistryItemDefinitionLookup", "SID", "ID");
            AlterColumn("RegistryItemDefinitionLookup", "ID", c => c.Guid(false));
            //Add PK
            AddPrimaryKey("RegistryItemDefinitionLookup", "ID");


            //OrganizationGroups
            AddPrimaryKey("OrganizationGroups", new string[] {"OrganizationID", "GroupID"});
            AddForeignKey("OrganizationGroups", "OrganizationID", "Organizations", "ID", true);
            AddForeignKey("OrganizationGroups", "GroupID", "Groups", "ID", true);

            //Queries
            AddForeignKey("Queries", "OrganizationID", "Organizations", "ID", true);

            //OrganizationRegistries
            AddPrimaryKey("OrganizationRegistries", new string[] {"OrganizationID", "RegistryID"});
            AddForeignKey("OrganizationRegistries", "OrganizationID", "Organizations", "ID", true);
            AddForeignKey("OrganizationRegistries", "RegistryID", "Registries", "ID", true);

            //OrganizationElectronicHealthRecordSystems
            AddForeignKey("OrganizationElectronicHealthRecordSystems", "OrganizationID", "Organizations", "ID", true);
            CreateIndex("OrganizationElectronicHealthRecordSystems", "OrganizationID");


            //RegistryDefinitions
            AddPrimaryKey("RegistryDefinitions", new string[] {"RegistryID", "RegistryItemDefinitionID"});
            AddForeignKey("RegistryDefinitions", "RegistryID", "Registries", "ID", true);
            AddForeignKey("RegistryDefinitions", "RegistryItemDefinitionID", "RegistryItemDefinitionLookup", "ID", true);

            //Projects
            AddForeignKey("Projects", "GroupID", "Groups", "ID", true);

            //RequestRegistrySearchResults
            AddPrimaryKey("RequestRegistrySearchResults", new string[] {"SearchRequestID", "ResultRegistryID"});
            AddForeignKey("RequestRegistrySearchResults", "ResultRegistryID", "Registries", "ID", true);

            //SecurityGroups
            AddForeignKey("SecurityGroups", "OrganizationID", "Organizations", "ID", true);

            //DataMarts
            AddForeignKey("DataMarts", "OrganizationID", "Organizations", "ID", true);
            CreateIndex("DataMarts", "OrganizationID");

            //RequestOrganizationSearchResults
            RenameTable("RequestOrganzationSearchResults", "RequestOrganizationSearchResults");
            AddPrimaryKey("RequestOrganizationSearchResults", new string[] {"SearchRequestId", "ResultOrganizationID"});
            //AddForeignKey("RequestOrganizationSearchResults", "ResultOrganizationID", "Organizations", "ID", true); --Commented because it would create a stack overflow

            //Users
            //AddForeignKey("Users", "OrganizationID", "Organizations", "ID", true);--Commented because it would create a stack overflow
            CreateIndex("Users", "OrganizationID");

            Sql(@"CREATE TRIGGER [dbo].[OrganizationDelete] 
    ON  [dbo].[Organizations]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	UPDATE Organizations SET ParentOrganizationID = NULL WHERE ID IN (SELECT ID FROM deleted)
	DELETE FROM RequestOrganizationSearchResults WHERE ResultOrganizationID IN (SELECT ID FROM deleted)
    DELETE FROM Users WHERE OrganizationID IN (SELECT ID FROM deleted)
END");
        }
        
        public override void Down()
        {
        }
    }
}
