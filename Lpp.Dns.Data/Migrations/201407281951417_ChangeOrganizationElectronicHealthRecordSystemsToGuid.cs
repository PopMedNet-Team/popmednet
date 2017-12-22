namespace Lpp.Dns.Data.Migrations
{
    using Lpp.Utilities;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeOrganizationElectronicHealthRecordSystemsToGuid : DbMigration
    {
        public override void Up()
        {
            MigrationHelpers.DropPrimaryKeyScript("OrganizationElectronicHealthRecordSystems");
            DropPrimaryKey("OrganizationElectronicHealthRecordSystems", "PK_dbo.OrganizationElectronicHealthRecordSystems", null);
            DropColumn("OrganizationElectronicHealthRecordSystems", "ID");
            AddColumn("OrganizationElectronicHealthRecordSystems", "ID", c => c.Guid(false, true, defaultValueSql: "[dbo].[newsqlguid]()"));
            AddPrimaryKey("OrganizationElectronicHealthRecordSystems", "ID");
        }
        
        public override void Down()
        {
        }
    }
}
