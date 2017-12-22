namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSPANandMPProcessorIDs : DbMigration
    {
        public override void Up()
        {
            Sql(@"UPDATE RequestTypes SET ProcessorID='C8BC0BD9-A50D-4B9C-9A25-472827C8640A', PackageIdentifier='Lpp.Dns.DataMart.Model.Processors' WHERE ID='BBB00001-16E2-4C53-8AEB-A22200FBAE28'");
            Sql(@"UPDATE RequestTypes SET ProcessorID='5d630771-8619-41f7-9407-696302e48237', PackageIdentifier='Lpp.Dns.DataMart.Model.Processors' WHERE ID='D87F0001-B2E6-4C33-8E9D-A22200FB514E'");
        }
        
        public override void Down()
        {
        }
    }
}
