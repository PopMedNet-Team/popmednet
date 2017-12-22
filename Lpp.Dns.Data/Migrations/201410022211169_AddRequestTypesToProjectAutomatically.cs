namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestTypesToProjectAutomatically : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE RequestTypes SET TemplateID = NULL WHERE TemplateID = '" + Guid.Empty + "'");
            AlterColumn("RequestTypes", "TemplateID", c => c.Guid(true, defaultValue: null));
            Sql(@"INSERT INTO ProjectRequestTypes (ProjectID, RequestTypeID) SELECT DISTINCT Projects.ID, RequestTypeModels.RequestTypeID FROM Projects INNER JOIN ProjectDataMarts ON Projects.ID = ProjectDataMarts.ProjectId INNER JOIN DataMarts ON ProjectDataMarts.DataMartID = DataMarts.ID INNER JOIN DataMartInstalledModels ON DataMarts.ID = DataMartInstalledModels.DataMartID INNER JOIN RequestTypeModels ON DataMartInstalledModels.ModelId = RequestTypeModels.DataModelID INNER JOIN RequestTypes ON RequestTypeModels.RequestTypeID = RequestTypes.ID 

WHERE RequestTypes.TemplateID IS NULL AND RequestTypes.WorkflowID IS NULL AND NOT EXISTS(SELECT NULL FROM ProjectRequestTypes prt WHERE prt.ProjectID = Projects.ID AND prt.RequestTypeID = RequestTypes.ID)");
        }
        
        public override void Down()
        {
        }
    }
}
