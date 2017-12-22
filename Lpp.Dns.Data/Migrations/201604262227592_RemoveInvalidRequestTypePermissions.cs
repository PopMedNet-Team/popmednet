namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveInvalidRequestTypePermissions : DbMigration
    {
        public override void Up()
        {
            //clean up at the project datamart level
            Sql(@"DELETE AclProjectDataMartRequestTypes FROM AclProjectDataMartRequestTypes a
JOIN (
	SELECT a.* FROM AclProjectDataMartRequestTypes a
	WHERE
	NOT EXISTS(
	SELECT NULL FROM ProjectDataMarts pdm
	JOIN DataMartInstalledModels im ON pdm.DataMartID = im.DataMartID
	JOIN RequestTypeModels rtm ON im.ModelId = rtm.DataModelID
	JOIN RequestTypes rt ON rtm.RequestTypeID = rt.ID
	WHERE pdm.DataMartID = a.DataMartID AND pdm.ProjectId = a.ProjectID
	AND rt.WorkflowID IS NULL
	)
	AND 
	NOT EXISTS(
	SELECT NULL FROM AclProjectDataMartRequestTypes pdm
	JOIN DataMarts dm ON pdm.DataMartID = dm.ID
	JOIN RequestTypeModels rtm ON rtm.DataModelID = dm.AdapterID
	JOIN RequestTypes rt ON rtm.RequestTypeID = rt.ID
	WHERE pdm.DataMartID = a.DataMartID AND pdm.ProjectID = a.ProjectID
	AND dm.AdapterID IS NOT NULL
	)
	AND
	NOT EXISTS(
	SELECT NULL FROM RequestTypes rt
	WHERE rt.WorkflowID IS NOT NULL
	AND (SELECT COUNT(*) FROM RequestTypeModels rtm WHERE rtm.RequestTypeID = rt.ID) = 0
	)
) d ON (a.SecurityGroupID = d.SecurityGroupID AND a.ProjectID = d.ProjectID AND a.DataMartID = d.DataMartID AND a.RequestTypeID = a.RequestTypeID)");

            //cleanup at the datamart level
            Sql(@"DELETE AclDataMartRequestTypes FROM AclDataMartRequestTypes t
JOIN (
	SELECT a.* FROM AclDataMartRequestTypes a
	WHERE
	NOT EXISTS(
	SELECT NULL FROM DataMarts dm
	JOIN DataMartInstalledModels im ON dm.ID = im.DataMartID
	JOIN RequestTypeModels rtm ON im.ModelId = rtm.DataModelID
	JOIN RequestTypes rt ON rtm.RequestTypeID = rt.ID
	WHERE dm.ID = a.DataMartID
	AND rt.WorkflowID IS NULL
	)
	AND 
	NOT EXISTS(
	SELECT NULL FROM AclDataMartRequestTypes adm
	JOIN DataMarts dm ON adm.DataMartID = dm.ID
	JOIN RequestTypeModels rtm ON rtm.DataModelID = dm.AdapterID
	JOIN RequestTypes rt ON rtm.RequestTypeID = rt.ID
	WHERE adm.DataMartID = a.DataMartID
	AND dm.AdapterID IS NOT NULL
	)
	AND
	NOT EXISTS(
	SELECT NULL FROM RequestTypes rt
	JOIN DataMarts dm ON dm.ID = a.DataMartID
	WHERE rt.WorkflowID IS NOT NULL
	AND (SELECT COUNT(*) FROM RequestTypeModels rtm WHERE rtm.RequestTypeID = rt.ID) = 0
	AND dm.AdapterID IS NOT NULL
	)
) d ON (t.SecurityGroupID = d.SecurityGroupID AND t.DataMartID = d.DataMartID AND t.RequestTypeID = d.RequestTypeID)");
        }
        
        public override void Down()
        {
        }
    }
}
