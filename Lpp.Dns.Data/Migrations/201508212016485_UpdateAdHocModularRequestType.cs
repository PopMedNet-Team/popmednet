namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAdHocModularRequestType : DbMigration
    {
        public override void Up()
        {
            /** PMNDEV-4666: somehow a template and workflow was associated with the legacy Modular Program: Ad Hoc request type. It also got associated with more than just the modular program model on the Edge 5.4 database.
             * This is a refresh to make sure it is correct for all environments.**/

            //remove the tempate and workflow settings, and make sure the name and description are correct.
            Sql("UPDATE RequestTypes SET [Name] = 'Modular Program: Ad Hoc', [Description] = 'Ad Hoc Program Submission to DataMarts', WorkflowID = NULL, TemplateID = NULL WHERE ID = '2C880001-5E3D-4032-9ADA-A22200FBC595'");
            //remove all associations to models other than Modular Program
            Sql("DELETE FROM RequestTypeModels WHERE RequestTypeID = '2C880001-5E3D-4032-9ADA-A22200FBC595' AND DataModelID <> '1B0FFD4C-3EEF-479D-A5C4-69D8BA0D0154'");


        }
        
        public override void Down()
        {
        }
    }
}
