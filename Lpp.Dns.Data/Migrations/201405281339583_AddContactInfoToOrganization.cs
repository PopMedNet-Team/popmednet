namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContactInfoToOrganization : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE dbo.Organizations ALTER Column ContactEmail nvarchar(510) null");
            Sql("ALTER TABLE dbo.Organizations ALTER Column ContactFirstName nvarchar(100) null");
            Sql("ALTER TABLE dbo.Organizations ALTER Column ContactLastName nvarchar(100) null");
            Sql("ALTER TABLE dbo.Organizations ALTER Column ContactPhone nvarchar(15) null");
            Sql("ALTER TABLE dbo.Organizations ALTER Column SpecialRequirements nvarchar(1000) null");
            Sql("ALTER TABLE dbo.Organizations ALTER Column UsageRestrictions nvarchar(1000) null");
            Sql("ALTER TABLE dbo.Organizations ALTER Column InpatientEHRApplication nvarchar(512) null");
            Sql("ALTER TABLE dbo.Organizations ALTER Column OutpatientEHRApplication nvarchar(512) null");
            Sql("ALTER TABLE dbo.Organizations ALTER Column OtherInpatientEHRApplication nvarchar(512) null");
            Sql("ALTER TABLE dbo.Organizations ALTER Column OtherOutpatientEHRApplication nvarchar(512) null");
            Sql("ALTER TABLE dbo.Organizations ALTER Column ObservationClinicalExperience nvarchar(1000) null");
        }
        
        public override void Down()
        {
        }
    }
}
