namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetPackageIdentifierForRequestTypes : DbMigration
    {
        public override void Up()
        {
            //Update package identifier for SPAN Request, Modular Program: Modular Program, Modular Program: Ad Hoc, Modular Program: Testing, and I2B2 (Embedded)
            Sql(@"UPDATE RequestTypes SET PackageIdentifier = 'Lpp.Dns.DataMart.Model.Processors' WHERE ID IN ('BBB00001-16E2-4C53-8AEB-A22200FBAE28', '2C880001-5E3D-4032-9ADA-A22200FBC595', 'EC1A0001-C467-4F03-A2F7-A22200FBDE89', 'D87F0001-B2E6-4C33-8E9D-A22200FB514E', 'A4850001-B3A7-4596-80BC-A22200FC06E9')");
        }
        
        public override void Down()
        {
        }
    }
}
