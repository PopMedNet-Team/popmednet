namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixUserFieldsBeingNonNullableWhenTheyShouldBe : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "PasswordRestorationToken", c => c.Guid());
            AlterColumn("dbo.Users", "PasswordRestorationTokenExpiration", c => c.DateTime());

            Sql("UPDATE Users SET PasswordRestorationToken = NULL, PasswordRestorationTokenExpiration = NULL WHERE PasswordRestorationToken = '00000000-0000-0000-0000-000000000000'");

            CreateIndex("dbo.Users", "PasswordRestorationToken");
            CreateIndex("dbo.Users", "SignedUpOn");
            CreateIndex("dbo.Users", "ActivatedOn");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "ActivatedOn" });
            DropIndex("dbo.Users", new[] { "SignedUpOn" });
            DropIndex("dbo.Users", new[] { "PasswordRestorationToken" });
            AlterColumn("dbo.Users", "PasswordRestorationTokenExpiration", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "PasswordRestorationToken", c => c.Guid(nullable: false));
        }
    }
}
