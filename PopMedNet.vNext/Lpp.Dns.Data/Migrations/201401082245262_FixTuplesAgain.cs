namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixTuplesAgain : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER VIEW [dbo].[_Security_Tuple1]
WITH SCHEMABINDING 
AS
SELECT        ih1.Start AS Id1, t.ObjectId1 AS ParentId1, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) AS DistancesJoined, dbo.AclEntries.PrivilegeId, m.Start AS SubjectId, COUNT_BIG(*) AS TotalEntries, 
                         SUM(1 - dbo.AclEntries.Allow) AS DeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND AclEntries.Allow = 0 THEN 1 ELSE 0 END) AS ExplicitDeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND 
                         AclEntries.Allow = 1 THEN 1 ELSE 0 END) AS ExplicitAllowedEntries, SUM(CASE WHEN m.[End] = m.[Start] THEN 1 ELSE 0 END) AS NotViaMembership, dbo.AclEntries.ChangedOn
FROM            dbo.AclEntries INNER JOIN
                         dbo.SecurityTargets AS t ON dbo.AclEntries.TargetId = t.Id INNER JOIN
                         dbo.SecurityMembershipClosure AS m ON dbo.AclEntries.SubjectId = m.[End] INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih1 ON ih1.[End] = t.ObjectId1
WHERE        (t.Arity = 1)
GROUP BY ih1.Start, t.ObjectId1, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5), dbo.AclEntries.PrivilegeId, m.Start, dbo.AclEntries.ChangedOn");

            Sql(@"CREATE UNIQUE CLUSTERED INDEX [_Security_Tuple1_PK] ON [dbo].[_Security_Tuple1]
(
	[Id1] ASC,
	[SubjectId] ASC,
	[PrivilegeId] ASC,
	[DistancesJoined] ASC,
	[ChangedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
");

            Sql(@"CREATE NONCLUSTERED INDEX [_Security_Tuple1_Reverse_IX] ON [dbo].[_Security_Tuple1] 
(
	[SubjectId] ASC,
	[PrivilegeId] ASC,
	[Id1] ASC,
	[DistancesJoined] ASC,
	[DeniedEntries] ASC,
	[ChangedOn] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = ON, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
");
        }
        
        public override void Down()
        {
        }
    }
}
