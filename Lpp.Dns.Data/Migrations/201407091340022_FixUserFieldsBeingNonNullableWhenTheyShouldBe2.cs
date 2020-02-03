namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixUserFieldsBeingNonNullableWhenTheyShouldBe2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "PasswordExpiration", c => c.DateTime());
            Sql("UPDATE Users SET PasswordExpiration = NULL WHERE Password IS NULL OR Password = ''");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "PasswordExpiration", c => c.DateTime(nullable: false));
        }
    }
}
