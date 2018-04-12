namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAndRenameRequestResponsePermissions : DbMigration
    {
        public override void Up()
        {
            Sql("Update [Permissions] Set [Name] = 'View Individual Results', [Description] = 'Controls who can view individual results' Where ID = 'C025131A-B5EC-46D5-B657-ADE567717A0D'");
            Sql("Update [Permissions] Set [Name] = 'View Aggregate Results', [Description] = 'Controls who can view aggregate results' Where ID = 'BDC57049-27BA-41DF-B9F9-A15ABF19B120'");
        }
        
        public override void Down()
        {
        }
    }
}
