namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModularProgramRequestTypeNames : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE RequestTypes SET [Name] = 'Ad Hoc' WHERE ID = '2C880001-5E3D-4032-9ADA-A22200FBC595'");
            Sql("UPDATE RequestTypes SET [Name] = 'Modular Program' WHERE ID = 'BBB00001-16E2-4C53-8AEB-A22200FBAE28'");
            Sql("UPDATE RequestTypes SET [Name] = 'Testing' WHERE ID = 'EC1A0001-C467-4F03-A2F7-A22200FBDE89'");
        }
        
        public override void Down()
        {
            Sql("UPDATE RequestTypes SET [Name] = 'Modular Program: Ad Hoc' WHERE ID = '2C880001-5E3D-4032-9ADA-A22200FBC595'");
            Sql("UPDATE RequestTypes SET [Name] = 'Modular Program: Modular Program' WHERE ID = 'BBB00001-16E2-4C53-8AEB-A22200FBAE28'");
            Sql("UPDATE RequestTypes SET [Name] = 'Modular Program: Testing' WHERE ID = 'EC1A0001-C467-4F03-A2F7-A22200FBDE89'");
        }
    }
}
