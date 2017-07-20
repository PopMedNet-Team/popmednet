namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetDataMartsUnattendedModeFromNullToZero : DbMigration
    {
        public override void Up()
        {
            Sql(@"UPDATE dbo.[DataMarts] SET UnattendedMode = 0 WHERE UnattendedMode IS NULL");
        }
        
        public override void Down()
        {
        }
    }
}
