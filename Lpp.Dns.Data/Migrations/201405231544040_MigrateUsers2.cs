namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateUsers2 : DbMigration
    {
        public override void Up()
        {
            AddPrimaryKey("Users", "ID");
            //This is not me being pedantic, this is because the column is locked because of corruption upgrading from SQL Server 2005 to 2008 so the column cannot be altered because the constraint is not findable to delete.
            AddColumn("Users", "Login", c => c.String(false, 50, defaultValue: ""));
            Sql("UPDATE Users SET Login = Username");
            DropColumn("Users", "Username");
            RenameColumn("Users", "Login", "Username");
            CreateIndex("Users", "Username");

            AlterColumn("Users", "Password", c => c.String(true, 100));
            AlterColumn("Users", "Email", c => c.String(true, 400));
            CreateIndex("Users", "Email");
            AlterColumn("Users", "Title", c => c.String(true, 100));

            AddColumn("Users", "FN", c => c.String(false, 100, defaultValue: ""));
            Sql("UPDATE Users SET FN = ISNULL(FirstName, '')");
            DropColumn("Users", "FirstName");
            RenameColumn("Users", "FN", "FirstName");

            AddColumn("Users", "LN", c => c.String(false, 100, defaultValue: ""));
            Sql("UPDATE Users SET LN = ISNULL(LastName, '')");
            DropColumn("Users", "LastName");
            RenameColumn("Users", "LN", "LastName");

            RenameColumn("Users", "LastUpdated", "LastUpdatedOn");
            RenameColumn("Users", "SignupDate", "SignedUpOn");
            RenameColumn("Users", "ActiveDate", "ActivatedOn");
        }
        
        public override void Down()
        {
        }
    }
}
