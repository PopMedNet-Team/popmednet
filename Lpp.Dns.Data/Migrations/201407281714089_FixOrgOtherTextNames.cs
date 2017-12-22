namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixOrgOtherTextNames : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Organizations", "OtherClaimsText", c => c.String(maxLength: 80));
            //AddColumn("dbo.Organizations", "DataModelOtherText", c => c.String(maxLength: 80));
            //DropColumn("dbo.Organizations", "OtherClaims");
            //DropColumn("dbo.Organizations", "DataModelOther");
            Sql(@"sp_RENAME 'Organizations.DataModelOther', 'DataModelOtherText' , 'COLUMN'");
            Sql(@"sp_RENAME 'Organizations.OtherClaims', 'OtherClaimsText' , 'COLUMN'");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Organizations", "DataModelOther", c => c.String(maxLength: 80));
            //AddColumn("dbo.Organizations", "OtherClaims", c => c.String(maxLength: 80));
            //DropColumn("dbo.Organizations", "DataModelOtherText");
            //DropColumn("dbo.Organizations", "OtherClaimsText");
            Sql(@"sp_RENAME 'Organizations.DataModelOtherText', 'DataModelOther' , 'COLUMN'");
            Sql(@"sp_RENAME 'Organizations.OtherClaimsText', 'OtherClaims' , 'COLUMN'");
        }
    }
}
