namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixAuditProperties : DbMigration
    {
        public override void Up()
        {
            Sql(@"UPDATE AuditPropertyValues SET IntValue = 0 WHERE IntValue IS NULL");
            Sql(@"UPDATE AuditPropertyValues SET DoubleValue = 0 WHERE DoubleValue IS NULL");
            Sql(@"UPDATE AuditPropertyValues SET DateTimeValue = '2013-10-10' WHERE DateTimeValue IS NULL");
            Sql(@"UPDATE AuditPropertyValues SET GuidValue = '00000000-0000-0000-0000-000000000000' WHERE GuidValue IS NULL");
        }
        
        public override void Down()
        {

        }
    }
}
