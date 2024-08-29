namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManageDMCSConfigurationACL : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Permissions(ID, Name, Description) VALUES('631EA13B-9456-42C5-B70D-18CC5D28904C', 'Modify DataMart Client Server Configuration', 'Allows the user to configure a DataMart from within the DataMart Client Server.')");
            //DataMart permission location
            Sql("INSERT INTO PermissionLocations(PermissionID, Type) VALUES('631EA13B-9456-42C5-B70D-18CC5D28904C', 1)");
            //Organization permission location
            Sql("INSERT INTO PermissionLocations(PermissionID, Type) VALUES('631EA13B-9456-42C5-B70D-18CC5D28904C', 3)");
        }
        
        public override void Down()
        {
            Sql("delete from PermissionLocations where PermissionID = '631EA13B-9456-42C5-B70D-18CC5D28904C'");
            Sql("delete from Permissions where ID = '631EA13B-9456-42C5-B70D-18CC5D28904C'");
        }
    }
}
