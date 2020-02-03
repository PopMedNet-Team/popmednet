namespace Lpp.Dns.Data.Migrations
{
    using Lpp.Dns.DTO.Security;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixLocationsForRequestTypes : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE PermissionLocations SET Type = 7 WHERE PermissionID = '" + PermissionIdentifiers.RequestTypes.Delete.ID + "' AND Type = 23");

            Sql("UPDATE PermissionLocations SET Type = 7 WHERE PermissionID = '" + PermissionIdentifiers.RequestTypes.Edit.ID + "' AND Type = 23");
            Sql("UPDATE PermissionLocations SET Type = 7 WHERE PermissionID = '" + PermissionIdentifiers.RequestTypes.ManageSecurity.ID + "' AND Type = 23");
            Sql("UPDATE PermissionLocations SET Type = 7 WHERE PermissionID = '" + PermissionIdentifiers.RequestTypes.View.ID + "' AND Type = 23");
        }
        
        public override void Down()
        {
        }
    }
}
