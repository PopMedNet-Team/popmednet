namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddToRegistries : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO RegistryItemDefinitions([Id],[Category],[Title])
     VALUES (44,'Classification','Rare Disease/Disorder/Condition')");
        }
        
        public override void Down()
        {
            Sql(@"delete from RegistryItemDefinitions where Id = 44");
        }
    }
}
