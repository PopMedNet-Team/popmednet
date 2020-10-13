namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixProjectDataMartAcls : DbMigration
    {
        public override void Up()
        {
            Sql("DELETE acl FROM AclProjectDataMarts acl WHERE NOT EXISTS(Select NULL from ProjectDataMarts pd WHERE acl.ProjectID = pd.ProjectId AND acl.DataMartID = pd.DataMartID)");

            Sql("DELETE acl FROM AclProjectOrganizations acl WHERE NOT EXISTS(Select NULL from ProjectOrganizations pd WHERE acl.ProjectID = pd.ProjectId AND acl.OrganizationID = pd.OrganizationID)");

            Sql(@"CREATE TRIGGER [dbo].[ProjectOrganizations_DeleteItem] 
                    ON  [dbo].[ProjectOrganizations]
                    AFTER DELETE
                AS 
                BEGIN
	                -- SET NOCOUNT ON added to prevent extra result sets from
	                -- interfering with SELECT statements.
	                SET NOCOUNT ON;
	                DELETE a FROM [AclProjectOrganizations] a INNER JOIN deleted ON a.ProjectID = deleted.ProjectId AND a.OrganizationID = deleted.OrganizationID
	                DELETE a FROM [ProjectOrganizationEvents] a INNER JOIN deleted ON a.ProjectID = deleted.ProjectId AND a.OrganizationID = deleted.OrganizationID
                END
GO");
        }

        public override void Down()
        {
            Sql(@"DROP TRIGGER [dbo].[ProjectOrganizations_DeleteItem]");
        }
    }
}
