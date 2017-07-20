namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveListRequestPermission : DbMigration
    {
        public override void Up()
        {
            Sql(@"DELETE FROM Permissions WHERE ID = '8DCA22F0-EA18-4353-BA45-CC2692C7A844'");
            Sql(@"DELETE FROM AclProjects WHERE PermissionID = '8DCA22F0-EA18-4353-BA45-CC2692C7A844'");
            Sql(@"DELETE FROM AclGlobal WHERE PermissionID = '8DCA22F0-EA18-4353-BA45-CC2692C7A844'");
        }
        
        public override void Down()
        {
        }
    }
}
