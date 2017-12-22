namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFlagsForDataTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataMarts", "LaboratoryResultsTestName", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "LaboratoryResultsDates", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "LaboratoryResultsTestLOINC", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "LaboratoryResultsTestSNOMED", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "LaboratoryResultsTestResultsInterpretation", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "LaboratoryResultsTestOther", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "LaboratoryResultsTestOtherText", c => c.String(maxLength: 80));
            AddColumn("dbo.DataMarts", "BiorepositoriesClaims", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "PatientReportedOutcomesClaims", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "PatientReportedBehaviorsClaims", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "PrescriptionOrdersClaims", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "PharmacyDispensingClaims", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DataMarts", "PharmacyDispensingClaims");
            DropColumn("dbo.DataMarts", "PrescriptionOrdersClaims");
            DropColumn("dbo.DataMarts", "PatientReportedBehaviorsClaims");
            DropColumn("dbo.DataMarts", "PatientReportedOutcomesClaims");
            DropColumn("dbo.DataMarts", "BiorepositoriesClaims");
            DropColumn("dbo.DataMarts", "LaboratoryResultsTestOtherText");
            DropColumn("dbo.DataMarts", "LaboratoryResultsTestOther");
            DropColumn("dbo.DataMarts", "LaboratoryResultsTestResultsInterpretation");
            DropColumn("dbo.DataMarts", "LaboratoryResultsTestSNOMED");
            DropColumn("dbo.DataMarts", "LaboratoryResultsTestLOINC");
            DropColumn("dbo.DataMarts", "LaboratoryResultsDates");
            DropColumn("dbo.DataMarts", "LaboratoryResultsTestName");
        }
    }
}
