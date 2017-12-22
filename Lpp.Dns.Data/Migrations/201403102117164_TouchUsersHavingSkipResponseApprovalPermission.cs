namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TouchUsersHavingSkipResponseApprovalPermission : DbMigration
    {
        public override void Up()
        {
            //Turn off recursive triggers at James's advice since the 'touch' operation causes a recursive error to happen
            Sql(@"  DECLARE @sql nvarchar(max)
                    SET @sql = 'ALTER DATABASE [' + db_name() + '] SET RECURSIVE_TRIGGERS OFF'
                    EXECUTE sp_executesql @sql", true);

            //Touch user's that have the skip approval response permission set so that the permissions are included in next Wbd sync
            Sql("UPDATE AclEntries SET ChangedOn = GETUTCDATE() WHERE PrivilegeId = 'A0F5B621-277A-417C-A862-801D7B9030A2'");
        }
        
        public override void Down()
        {
        }
    }
}
