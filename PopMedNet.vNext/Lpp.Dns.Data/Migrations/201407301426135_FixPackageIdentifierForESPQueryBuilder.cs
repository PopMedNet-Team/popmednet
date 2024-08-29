namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixPackageIdentifierForESPQueryBuilder : DbMigration
    {
        public override void Up()
        {
            Sql(@"UPDATE RequestTypes SET PackageIdentifier = 'Lpp.Dns.DataMart.Model.ESPQueryBuilder' WHERE ProcessorID = '1BD526D9-46D8-4F66-9191-5731CB8189EE' AND (ID = '15830001-6DFF-47E9-B2FD-A22200FC77C3' OR ID ='84CF0001-475C-46AB-998B-A22200FC6439' OR ID = '6A900001-CFC3-439C-9978-A22200FC5253')");
        }
        
        public override void Down()
        {
        }
    }
}
