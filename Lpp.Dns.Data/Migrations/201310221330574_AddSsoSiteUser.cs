namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSsoSiteUser : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO Users (UserName, Password, OrganizationId, Email, RoleTypeId, PasswordEncryptionLength, PasswordExpiration, isActive)
                VALUES ('SsoSite', '" + Lpp.Utilities.Password.ComputeHash("SsoApiPassword1@") + "', 1, '', 3, 14, '2099-01-01', 1)");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM Users WHERE UserName = 'SsoSite'");
        }
    }
}
