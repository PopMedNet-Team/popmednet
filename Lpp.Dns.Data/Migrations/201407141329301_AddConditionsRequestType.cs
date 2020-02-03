namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConditionsRequestType : DbMigration
    {
        public override void Up()
        {
            Sql(@"insert into [dbo].[RequestTypes] (ID, Name, ProcessorID, MetaData, PostProcess, AddFiles, RequiresProcessing) values ('4EEE0635-AC4C-49A2-9CF7-2A6C923DC176', 'Conditions', 'D1C750B3-BA77-4F40-BA7E-F5FF28137CAF', 0, 0, 0, 0)");
            Sql(@"insert into [dbo].[RequestTypeModels] (RequestTypeID, DataModelID) values ('4EEE0635-AC4C-49A2-9CF7-2A6C923DC176', 'EA26172E-1B5F-4616-B082-7DABFA66E1D2')");
        }
        
        public override void Down()
        {
            Sql(@"delete from [DNS3_DEV].[dbo].[RequestTypes] where Name='Conditions'");
            Sql(@"delete from [DNS3_DEV].[dbo].[RequestTypeModels] where RequestTypeID='4EEE0635-AC4C-49A2-9CF7-2A6C923DC176' AND DataModelID='EA26172E-1B5F-4616-B082-7DABFA66E1D2'");
        }
    }
}
