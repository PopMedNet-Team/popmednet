namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLongitudinalCaptureFieldsToDataMart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DataMarts", "InpatientEncountersEncounterID", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "InpatientEncountersProviderIdentifier", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "OutpatientEncountersEncounterID", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "OutpatientEncountersProviderIdentifier", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "LongitudinalCapturePatientID", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "LongitudinalCaptureStart", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "LongitudinalCaptureStop", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "LongitudinalCaptureOther", c => c.Boolean(nullable: false));
            AddColumn("dbo.DataMarts", "LongitudinalCaptureOtherValue", c => c.String(maxLength: 80));            
        }
        
        public override void Down()
        {            
            DropColumn("dbo.DataMarts", "LongitudinalCaptureOtherValue");
            DropColumn("dbo.DataMarts", "LongitudinalCaptureOther");
            DropColumn("dbo.DataMarts", "LongitudinalCaptureStop");
            DropColumn("dbo.DataMarts", "LongitudinalCaptureStart");
            DropColumn("dbo.DataMarts", "LongitudinalCapturePatientID");
            DropColumn("dbo.DataMarts", "OutpatientEncountersProviderIdentifier");
            DropColumn("dbo.DataMarts", "OutpatientEncountersEncounterID");
            DropColumn("dbo.DataMarts", "InpatientEncountersProviderIdentifier");
            DropColumn("dbo.DataMarts", "InpatientEncountersEncounterID");
        }
    }
}
