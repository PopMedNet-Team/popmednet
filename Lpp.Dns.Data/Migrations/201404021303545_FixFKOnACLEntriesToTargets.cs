namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixFKOnACLEntriesToTargets : DbMigration
    {
        public override void Up()
        {
            Sql(@"DECLARE @constraintname nvarchar(max) = (select TOP 1 CONSTRAINT_NAME
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS
where TABLE_NAME = 'AclEntries'
AND CONSTRAINT_NAME LIKE 'FK__AclEntrie__Targe%')

DECLARE @sql nvarchar(max) = 'ALTER TABLE ACLEntries DROP CONSTRAINT ' + @constraintname

execute sp_executesql @sql");

            AddForeignKey("AclEntries", "TargetId", "SecurityTargets", "Id", true);
        }
        
        public override void Down()
        {
        }
    }
}
