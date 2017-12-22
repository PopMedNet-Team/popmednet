namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataMart_ChangeClaimsToNonNullBoolean : DbMigration
    {
        public override void Up()
        {
            //set initial default values to false
            Sql("UPDATE DataMarts SET InpatientClaims = 0 WHERE InpatientClaims IS NULL");
            Sql("UPDATE DataMarts SET OutpatientClaims = 0 WHERE OutpatientClaims IS NULL");
            Sql("UPDATE DataMarts SET OutpatientPharmacyClaims = 0 WHERE OutpatientPharmacyClaims IS NULL");
            Sql("UPDATE DataMarts SET EnrollmentClaims = 0 WHERE EnrollmentClaims IS NULL");
            Sql("UPDATE DataMarts SET LaboratoryResultsClaims = 0 WHERE LaboratoryResultsClaims IS NULL");
            Sql("UPDATE DataMarts SET VitalSignsClaims = 0 WHERE VitalSignsClaims IS NULL");

            AlterColumn("dbo.DataMarts", "InpatientClaims", c => c.Boolean(nullable: false, defaultValue: false));
            AlterColumn("dbo.DataMarts", "OutpatientClaims", c => c.Boolean(nullable: false, defaultValue: false));
            AlterColumn("dbo.DataMarts", "OutpatientPharmacyClaims", c => c.Boolean(nullable: false, defaultValue: false));
            AlterColumn("dbo.DataMarts", "EnrollmentClaims", c => c.Boolean(nullable: false, defaultValue: false));
            AlterColumn("dbo.DataMarts", "LaboratoryResultsClaims", c => c.Boolean(nullable: false, defaultValue: false));
            AlterColumn("dbo.DataMarts", "VitalSignsClaims", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DataMarts", "VitalSignsClaims", c => c.Boolean());
            AlterColumn("dbo.DataMarts", "LaboratoryResultsClaims", c => c.Boolean());
            AlterColumn("dbo.DataMarts", "EnrollmentClaims", c => c.Boolean());
            AlterColumn("dbo.DataMarts", "OutpatientPharmacyClaims", c => c.Boolean());
            AlterColumn("dbo.DataMarts", "OutpatientClaims", c => c.Boolean());
            AlterColumn("dbo.DataMarts", "InpatientClaims", c => c.Boolean());
        }
    }
}
