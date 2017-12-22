namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SCNR4 : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO [dbo].[DataModels]
           ([ID]
           ,[Name]
           ,[Description]
           ,[RequiresConfiguration])
     VALUES
           ('3F491126-9A48-449F-BE77-A3990439D677'
           ,'Rest Query Builder'
           ,NULL
           ,0)");
            Sql(@"INSERT INTO [dbo].[RequestTypes]
           ([ID]
           ,[Name]
           ,[Description]
           ,[ProcessorID]
           ,[MetaData]
           ,[PostProcess]
           ,[AddFiles]
           ,[RequiresProcessing]
           ,[PackageIdentifier]
           ,[TemplateID]
           ,[WorkflowID])
     VALUES
           ('D9A20001-E827-4AC3-A078-A43B010F7F57'
           ,'Rest Query Composer'
           ,'Compose Query to a Rest Adapter'
           ,'55C48A42-B800-4A55-8134-309CC9954D4C'
           ,0
           ,0
           ,0
           ,0
           ,'Lpp.Dns.DataMart.Model.Processors'
           ,NULL
           ,NULL)");
            Sql(@"INSERT INTO [dbo].[RequestTypeModels]
           ([RequestTypeID]
           ,[DataModelID])
     VALUES
           ('D9A20001-E827-4AC3-A078-A43B010F7F57'
           ,'3F491126-9A48-449F-BE77-A3990439D677')");
        }
        
        public override void Down()
        {
        }
    }
}
