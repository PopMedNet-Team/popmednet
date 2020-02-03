namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModifyResultsPermission : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF NOT EXISTS(SELECT NULL FROM dbo.[Permissions] WHERE ID = '80500001-D58E-4EEE-8541-A7CA010034F5')
	INSERT INTO dbo.[Permissions](ID, [Name], [Description]) VALUES ('80500001-D58E-4EEE-8541-A7CA010034F5', 'Modify Results', 'Allows the user to upload results for File Distribution and Modular Program requests using the DataMart Client after the routing has been completed.')");
            //datamart
            Sql(@"IF NOT EXISTS(SELECT NULL FROM dbo.PermissionLocations WHERE PermissionID = '80500001-D58E-4EEE-8541-A7CA010034F5' AND [Type] = 1) INSERT INTO dbo.PermissionLocations (PermissionID, [Type]) VALUES ('80500001-D58E-4EEE-8541-A7CA010034F5', 1)");
            //organization
            Sql(@"IF NOT EXISTS(SELECT NULL FROM dbo.PermissionLocations WHERE PermissionID = '80500001-D58E-4EEE-8541-A7CA010034F5' AND [Type] = 3) INSERT INTO dbo.PermissionLocations (PermissionID, [Type]) VALUES ('80500001-D58E-4EEE-8541-A7CA010034F5', 3)");
            //project
            Sql(@"IF NOT EXISTS(SELECT NULL FROM dbo.PermissionLocations WHERE PermissionID = '80500001-D58E-4EEE-8541-A7CA010034F5' AND [Type] = 4) INSERT INTO dbo.PermissionLocations (PermissionID, [Type]) VALUES ('80500001-D58E-4EEE-8541-A7CA010034F5', 4)");
            //project datamart
            Sql(@"IF NOT EXISTS(SELECT NULL FROM dbo.PermissionLocations WHERE PermissionID = '80500001-D58E-4EEE-8541-A7CA010034F5' AND [Type] = 11) INSERT INTO dbo.PermissionLocations (PermissionID, [Type]) VALUES ('80500001-D58E-4EEE-8541-A7CA010034F5', 11)");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM dbo.PermissionLocations WHERE PermissionID = '80500001-D58E-4EEE-8541-A7CA010034F5'");
            Sql("DELETE FROM dbo.[Permissions] WHERE ID = '80500001-D58E-4EEE-8541-A7CA010034F5'");
        }
    }
}
