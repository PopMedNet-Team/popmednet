namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConditionsDataModel : DbMigration
    {
        public override void Up()
        {
            Sql(@"insert into [dbo].[DataModels] (ID, Name, RequiresConfiguration) values ('EA26172E-1B5F-4616-B082-7DABFA66E1D2', 'Conditions', 0)");
        }
        
        public override void Down()
        {
            Sql(@"delete from [DNS3_DEV].[dbo].[DataModels] where Name='Conditions'");
        }
    }
}
