namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePackageIdentifiersForRequestTypes : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE RequestTypes SET PackageIdentifier = 'Lpp.Dns.DataMart.Model.DataChecker' WHERE ProcessorID = '5DE1CF20-1CE0-49A2-8767-D8BC5D16D36F'");
            Sql("UPDATE RequestTypes SET PackageIdentifier = 'Lpp.Dns.DataMart.Model.ESPQueryBuilder' WHERE ProcessorID = '1BD526D9-46D8-4F66-9191-5731CB8189EE' OR ProcessorID = 'AE85D3E6-93F8-4CB5-BD45-D2F84AB85D83'");
            Sql("UPDATE RequestTypes SET PackageIdentifier = 'Lpp.Dns.DataMart.Model.ESPQueryBuilder.Conditions' WHERE ProcessorID = 'D1C750B3-BA77-4F40-BA7E-F5FF28137CAF'");
            Sql("UPDATE RequestTypes SET PackageIdentifier = 'Lpp.Dns.DataMart.Model.ESPQueryBuilder.I2B2' WHERE ProcessorID = '1BD526D9-46D8-4F66-9191-5731CB8189EE'");
            Sql("UPDATE RequestTypes SET PackageIdentifier = 'Lpp.Dns.DataMart.Model.Metadata' WHERE ProcessorID = '9D0CD143-7DCA-4953-8209-224BDD3AF718'");
            Sql("UPDATE RequestTypes SET PackageIdentifier = 'Lpp.Dns.DataMart.Model.Processors' WHERE ProcessorID = 'C8BC0BD9-A50D-4B9C-9A25-472827C8640A' OR ProcessorID = '55C48A42-B800-4A55-8134-309CC9954D4C' OR ProcessorID = '5d630771-8619-41f7-9407-696302e48237' OR ProcessorID = 'cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb'");
            Sql("UPDATE RequestTypes SET PackageIdentifier = 'Lpp.Dns.DataMart.Model.QueryComposer' WHERE ProcessorID = 'AE0DA7B0-0F73-4D06-B70B-922032B7F0EB'");
            Sql("UPDATE RequestTypes SET PackageIdentifier = 'Lpp.Dns.DataMart.Model.Sample' WHERE ProcessorID = 'F985DBD9-DA7E-41B4-8FBD-2A73B7FCF6DD'");
        }
        
        public override void Down()
        {
        }
    }
}
