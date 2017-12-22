namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUpdateActivitiesPermission : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF NOT EXISTS(SELECT NULL FROM Permissions WHERE ID = '37CF0001-9772-4082-8A0B-A45601490CAF')
	INSERT INTO Permissions(ID, [Name], [Description]) VALUES ('37CF0001-9772-4082-8A0B-A45601490CAF', 'Update Activities', 'Controls if the security group has permission to trigger an update of the projects activities.')");
            
            //Add Global to permission locations for Update Activities permission.
            Sql(@"IF NOT EXISTS(SELECT NULL FROM PermissionLocations WHERE PermissionID = '37CF0001-9772-4082-8A0B-A45601490CAF' AND PermissionLocations.Type = 0)
	INSERT INTO PermissionLocations(PermissionID, [Type]) VALUES ('37CF0001-9772-4082-8A0B-A45601490CAF', 0)");
            
            //Add Projects to permission locations for Update Activities permision.
            Sql(@"IF NOT EXISTS(SELECT NULL FROM PermissionLocations WHERE PermissionID = '37CF0001-9772-4082-8A0B-A45601490CAF' AND PermissionLocations.Type = 4)
	INSERT INTO PermissionLocations(PermissionID, [Type]) VALUES ('37CF0001-9772-4082-8A0B-A45601490CAF', 4)");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM AclGlobal WHERE PermissionID = '37CF0001-9772-4082-8A0B-A45601490CAF'");
            Sql("DELETE FROM AclProjects WHERE PermissionID = '37CF0001-9772-4082-8A0B-A45601490CAF'");
            Sql("DELETE FROM PermissionLocations WHERE PermissionID = '37CF0001-9772-4082-8A0B-A45601490CAF'");
            Sql("DELETE FROM Permissions WHERE ID = '37CF0001-9772-4082-8A0B-A45601490CAF'");
        }
    }
}
