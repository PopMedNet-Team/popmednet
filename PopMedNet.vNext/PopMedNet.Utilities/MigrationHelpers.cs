using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Utilities
{
    public static class MigrationHelpers
    {
        public const string GuidDefaultValue = "[dbo].[newsqlguid]()";
        public static string DropPrimaryKeyScript(string tableName)
        {
            var result = string.Format(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1 Tab.CONSTRAINT_NAME from 
    INFORMATION_SCHEMA.TABLE_CONSTRAINTS Tab, 
    INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE Col 
WHERE 
    Col.Constraint_Name = Tab.Constraint_Name
    AND Col.Table_Name = Tab.Table_Name
    AND Constraint_Type = 'PRIMARY KEY '
    AND Col.Table_Name = '{0}')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE {0} DROP CONSTRAINT [' + @FKName + ']'

EXECUTE sp_executeSql @Sql", tableName);
            System.Diagnostics.Debug.Write(result);
            return result;
        }

        public static string DropForeignKeyScript(string tableName, string columnName)
        {
            return string.Format(@"DECLARE @FKName AS nvarchar(max) = (SELECT TOP 1
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id

WHERE OBJECT_NAME(f.parent_object_id) = '{0}' AND COL_NAME(fc.parent_object_id,
    fc.parent_column_id) = '{1}')

DECLARE @Sql AS nvarchar(max) = 'ALTER TABLE {0} DROP CONSTRAINT [' + @FKName + ']'

EXECUTE sp_executeSql @Sql", tableName, columnName);
        }

        public static string ClearStatsScript(string tableName)
        {
            return string.Format(@"DECLARE curAllStats CURSOR  FOR
SELECT Name
FROM   sys.stats
WHERE  object_id = object_id('{0}')
       --AND auto_created <> 0
 
DECLARE  @StatName NVARCHAR(512)
 
OPEN curAllStats
 
FETCH NEXT FROM curAllStats
INTO @StatName
 
WHILE @@FETCH_STATUS = 0
  BEGIN
    BEGIN TRY
    EXEC( 'drop statistics {0}.' + @StatName)
    END TRY
    BEGIN CATCH
    END CATCH
	Print 'drop statistics {0}.' + @StatName
    
    FETCH NEXT FROM curAllStats
    INTO @StatName
  END -- WHILE
 
CLOSE curAllStats
 
DEALLOCATE curAllStats", tableName);
        }

        public static string AddAclDeleteScript(string tableName, bool alter = false)
        {
            return string.Format((alter ? "ALTER" : "CREATE ") + @" TRIGGER [dbo].[{0}_DeleteItem] 
                        ON  [dbo].[{0}]
                        AFTER DELETE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM deleted) > 0)
						BEGIN
							DELETE a FROM [{0}] a INNER JOIN deleted ON a.PermissionID = deleted.PermissionID JOIN SecurityGroups ON a.SecurityGroupID = SecurityGroups.ID AND SecurityGroups.ParentSecurityGroupID = deleted.SecurityGroupID WHERE a.Overridden = 0
						END
                    END", tableName);
        }

        public static string DropAclDeleteScript(string tableName)
        {
            return string.Format("DROP TRIGGER [dbo].[{0}_DeleteItem]", tableName);
        }

        public static string AddAclInsertScript(string tableName, bool alter = false)
        {
            return string.Format((alter ? "ALTER" : "CREATE ") + @" TRIGGER [dbo].[{0}_InsertItem] 
                        ON  [dbo].[{0}]
                        AFTER INSERT
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN						
							INSERT INTO [{0}] (SecurityGroupID, PermissionID, Allowed) SELECT SecurityGroups.ID, inserted.PermissionID, inserted.Allowed FROM SecurityGroups JOIN inserted ON SecurityGroups.ParentSecurityGroupID = inserted.SecurityGroupID WHERE NOT EXISTS(SELECT NULL FROM [{0}] AS a JOIN SecurityGroups sg ON a.SecurityGroupID = sg.ID WHERE sg.ParentSecurityGroupID = inserted.SecurityGroupID AND a.PermissionID = inserted.PermissionID)
						END
                    END", tableName);
        }

        public static string DropAclInsertScript(string tableName)
        {
            return string.Format("DROP TRIGGER [dbo].[{0}_InsertItem]", tableName);
        }

        public static string AddAclUpdateScript(string tableName, bool alter = false)
        {
            return string.Format((alter ? "ALTER" : "CREATE ") + @" TRIGGER [dbo].[{0}_UpdateItem] 
                        ON  [dbo].[{0}]
                        AFTER UPDATE
                    AS 
                    BEGIN
	                    -- SET NOCOUNT ON added to prevent extra result sets from
	                    -- interfering with SELECT statements.
	                    SET NOCOUNT ON;
						IF((SELECT COUNT(*) FROM inserted) > 0)
						BEGIN		
							UPDATE [{0}] SET [{0}].Allowed = inserted.Allowed FROM [{0}] INNER JOIN inserted ON [{0}].PermissionID = inserted.PermissionID JOIN SecurityGroups ON [{0}].SecurityGroupID = SecurityGroups.ID AND inserted.SecurityGroupID = SecurityGroups.ParentSecurityGroupID WHERE [{0}].Overridden = 0
						END
                    END", tableName);
        }

        public static string DropAclUpdateScript(string tableName)
        {
            return string.Format("DROP TRIGGER [dbo].[{0}_UpdateItem]", tableName);
        }        
    }
}
