namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveInvalidProjectDataMartRequestTypePermissions : DbMigration
    {
        public override void Up()
        {
            /* Remove any acls that are no longer valid - the project datamarts request type list was showing request types that were not associated to the project before PMNDEV-5724 */
            Sql(@"DELETE a FROM AclProjectDataMartRequestTypes a
JOIN (
	SELECT * FROM AclProjectDataMartRequestTypes pdmrt
	WHERE NOT EXISTS(SELECT NULL FROM ProjectRequestTypes prt WHERE prt.ProjectID = pdmrt.ProjectID AND prt.RequestTypeID = pdmrt.RequestTypeID)
) pdmrt ON a.DataMartID = pdmrt.DataMartID AND a.RequestTypeID = pdmrt.RequestTypeID AND pdmrt.ProjectID = a.ProjectID");

        }
        
        public override void Down()
        {
        }
    }
}
