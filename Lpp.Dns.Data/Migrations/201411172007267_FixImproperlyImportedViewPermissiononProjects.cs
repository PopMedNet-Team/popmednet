namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixImproperlyImportedViewPermissiononProjects : DbMigration
    {
        public override void Up()
        {
            Sql(@"DELETE FROM AclGlobal WHERE PermissionID = '4CCB0EC2-006D-4345-895E-5DD2C6C8C791' AND NOT EXISTS(SELECT * FROM Security_Tuple1 WHERE PrivilegeID = '4CCB0EC2-006D-4345-895E-5DD2C6C8C791' AND ID1 = '6A690001-7579-4C74-ADE1-A2210107FA29')");
        }
        
        public override void Down()
        {
        }
    }
}
