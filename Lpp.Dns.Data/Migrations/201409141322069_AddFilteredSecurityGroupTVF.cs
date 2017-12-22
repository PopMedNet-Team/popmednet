namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFilteredSecurityGroupTVF : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE FUNCTION FilteredSecurityGroups
(	
	@UserID uniqueidentifier
)
RETURNS TABLE 
AS
RETURN 
(
SELECT DISTINCT SecurityGroups.* FROM SecurityGroups 
	LEFT OUTER JOIN Projects p ON SecurityGroups.OwnerID = p.ID
	LEFT OUTER JOIN Organizations o ON SecurityGroups.OwnerID = o.ID

WHERE
	( --Projects
		NOT p.ID IS NULL
		AND
		(
			EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '468E7007-E95F-435C-8FAF-0B9FBC9CA997', p.ID))
			AND
			NOT EXISTS(SELECT NULL FROM dbo.AclProjectsFiltered(@UserID, '468E7007-E95F-435C-8FAF-0B9FBC9CA997', p.ID) pf WHERE pf.Allowed = 0)
		)
		OR
		(
			EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '468E7007-E95F-435C-8FAF-0B9FBC9CA997'))
			AND
			NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '468E7007-E95F-435C-8FAF-0B9FBC9CA997') gf WHERE gf.Allowed = 0)
		)
		OR
		( --DataMarts
			EXISTS(SELECT NULL FROM ProjectDataMarts pdm WHERE pdm.ProjectiD = p.ID AND EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, '668E7007-E95F-435C-8FAF-0B9FBC9CA997', pdm.DataMartID)))
			AND
			NOT EXISTS(SELECT NULL FROM ProjectDataMarts pdm WHERE pdm.ProjectiD = p.ID AND EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, '668E7007-E95F-435C-8FAF-0B9FBC9CA997', pdm.DataMartID) f WHERE f.Allowed = 0))
		)
	)
	OR 
	( --Organizations
		NOT o.ID IS NULL
		AND
		(
			EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '068E7007-E95F-435C-8FAF-0B9FBC9CA997', o.ID))
			AND
			NOT EXISTS(SELECT NULL FROM dbo.AclOrganizationsFiltered(@UserID, '068E7007-E95F-435C-8FAF-0B9FBC9CA997', o.ID) f WHERE f.Allowed = 0)
		)
		OR
		(
			EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '068E7007-E95F-435C-8FAF-0B9FBC9CA997'))
			AND
			NOT EXISTS(SELECT NULL FROM dbo.AclGlobalFiltered(@UserID, '068E7007-E95F-435C-8FAF-0B9FBC9CA997') gf WHERE gf.Allowed = 0)
		)
		OR
		( --DataMarts
			EXISTS(SELECT NULL FROM DataMarts dm WHERE dm.OrganizationID = o.ID AND EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, '668E7007-E95F-435C-8FAF-0B9FBC9CA997', dm.ID)))
			AND
			NOT EXISTS(SELECT NULL FROM DataMarts dm WHERE dm.OrganizationID = o.ID AND EXISTS(SELECT NULL FROM dbo.AclDataMartsFiltered(@UserID, '668E7007-E95F-435C-8FAF-0B9FBC9CA997', dm.ID) f WHERE f.Allowed = 0))
		)
	)
)
");
        }
        
        public override void Down()
        {
        }
    }
}
