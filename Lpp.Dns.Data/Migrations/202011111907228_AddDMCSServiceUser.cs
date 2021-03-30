namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDMCSServiceUser : DbMigration
    {
        public override void Up()
        {
            Sql($"INSERT INTO [dbo].[Users] ([Password],[Email],[ID],[PasswordExpiration],[isActive],[ActivatedOn],[FailedLoginCount],[Username],[FirstName],[LastName],[PasswordVersion],[UserType]) VALUES('OfFqHjcbHzORTsRgEaqMtZ+HgsiqullVVA/TPFVjVKA=','dmcs@popmednet.org','3FCD3FBD-A1D8-4D09-BFF0-C819199F19D7','{DateTime.Now.AddYears(10)}',1,'{DateTime.Now}',0,'dmcs-syncuser','dmcs-sync','user',1,3)");
        }

        public override void Down()
        {
            Sql("DELETE FROM USERS where ID = '3FCD3FBD-A1D8-4D09-BFF0-C819199F19D7'");
        }
    }
}
