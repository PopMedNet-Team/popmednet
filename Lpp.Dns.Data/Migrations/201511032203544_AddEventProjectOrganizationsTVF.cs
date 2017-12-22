namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEventProjectOrganizationsTVF : DbMigration
    {
        public override void Up()
        {
            Sql("IF EXISTS(SELECT NULL FROM INFORMATION_SCHEMA.ROUTINES WHERE SPECIFIC_SCHEMA = 'dbo' AND ROUTINE_TYPE = 'FUNCTION' AND SPECIFIC_NAME = 'EventProjectOrganizations') DROP FUNCTION dbo.EventProjectOrganizations");

            Sql(@"CREATE FUNCTION [dbo].[EventProjectOrganizations]
(	
	@UserID uniqueidentifier,
	@EventID uniqueidentifier
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT DISTINCT e.ProjectID, e.OrganizationID FROM ProjectOrganizationEvents e JOIN SecurityGroupUsers sgu ON e.SecurityGroupID = sgu.SecurityGroupID WHERE sgu.UserID = @UserID AND e.EventID = @EventID AND e.Allowed = 1 AND 
		NOT EXISTS(SELECT NULL FROM ProjectOrganizationEvents ne WHERE ne.EventID = e.EventID AND ne.ProjectID = e.ProjectID AND ne.OrganizationID = e.OrganizationID AND ne.SecurityGroupID = e.SecurityGroupID AND ne.Allowed = 0)
)");

        }
        
        public override void Down()
        {

            Sql(@"drop function dbo.EventProjectOrganizations");

        }
    }
}
