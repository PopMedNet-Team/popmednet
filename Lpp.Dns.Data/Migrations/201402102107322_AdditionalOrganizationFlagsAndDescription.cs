namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalOrganizationFlagsAndDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizations", "PragmaticClinicalTrials", c => c.Boolean(nullable: false));
            AddColumn("dbo.Organizations", "Biorepositories", c => c.Boolean(nullable: false));
            AddColumn("dbo.Organizations", "PatientReportedOutcomes", c => c.Boolean(nullable: false));
            AddColumn("dbo.Organizations", "PatientReportedBehaviors", c => c.Boolean(nullable: false));
            AddColumn("dbo.Organizations", "PrescriptionOrders", c => c.Boolean(nullable: false));
            AddColumn("dbo.Organizations", "OrganizationDescription", c => c.String(maxLength: 455));

            CreateIndex("dbo.Organizations", "OrganizationDescription");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Organizations", new string[] { "OrganizationDescription" });

            DropColumn("dbo.Organizations", "OrganizationDescription");
            DropColumn("dbo.Organizations", "PrescriptionOrders");
            DropColumn("dbo.Organizations", "PatientReportedBehaviors");
            DropColumn("dbo.Organizations", "PatientReportedOutcomes");
            DropColumn("dbo.Organizations", "Biorepositories");
            DropColumn("dbo.Organizations", "PragmaticClinicalTrials");
        }
    }
}
