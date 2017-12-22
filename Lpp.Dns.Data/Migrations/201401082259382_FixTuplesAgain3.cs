namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixTuplesAgain3 : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER VIEW [dbo].[_Security_Tuple3]
WITH SCHEMABINDING 
AS
SELECT        ih1.Start AS Id1, ih2.Start AS Id2, ih3.Start AS Id3, t.ObjectId1 AS ParentId1, t.ObjectId2 AS ParentId2, t.ObjectId3 AS ParentId3, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) 
                         + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih2.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih3.Distance, 0)), 5) AS DistancesJoined, e.PrivilegeId, m.Start AS SubjectId, COUNT_BIG(*) 
                         AS TotalEntries, SUM(1 - e.Allow) AS DeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND e.Allow = 0 THEN 1 ELSE 0 END) AS ExplicitDeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND 
                         e.Allow = 1 THEN 1 ELSE 0 END) AS ExplicitAllowedEntries, SUM(CASE WHEN m.[End] = m.[Start] THEN 1 ELSE 0 END) AS NotViaMembership, e.ChangedOn
FROM            dbo.AclEntries AS e INNER JOIN
                         dbo.SecurityTargets AS t ON e.TargetId = t.Id INNER JOIN
                         dbo.SecurityMembershipClosure AS m ON e.SubjectId = m.[End] INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih1 ON ih1.[End] = t.ObjectId1 INNER JOIN
                         dbo.SecurityInheritanceClosure2 AS ih2 ON ih2.[End] = t.ObjectId2 INNER JOIN
                         dbo.SecurityInheritanceClosure3 AS ih3 ON ih3.[End] = t.ObjectId3
WHERE        (t.Arity = 3)
GROUP BY ih1.Start, ih2.Start, ih3.Start, t.ObjectId1, t.ObjectId2, t.ObjectId3, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih2.Distance, 0)), 5) 
                         + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih3.Distance, 0)), 5), e.PrivilegeId, m.Start, e.ChangedOn
");
            Sql(@"CREATE UNIQUE CLUSTERED INDEX [_Security_Tuple3_PK] ON [dbo].[_Security_Tuple3] 
(
	[Id1] ASC,
	[Id2] ASC,
	[Id3] ASC,
	[SubjectId] ASC,
	[PrivilegeId] ASC,
	[DistancesJoined] ASC,
    [ChangedOn] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = ON, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
");

            Sql(@"CREATE NONCLUSTERED INDEX [_Security_Tuple3_Reverse_IX] ON [dbo].[_Security_Tuple3] 
(
	[SubjectId] ASC,
	[PrivilegeId] ASC,
	[Id1] ASC,
	[Id2] ASC,
	[Id3] ASC,
	[DistancesJoined] ASC,
	[DeniedEntries] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = ON, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]");





            Sql(@"ALTER VIEW [dbo].[_Security_Tuple4]
WITH SCHEMABINDING 
AS
SELECT        ih1.Start AS Id1, ih2.Start AS Id2, ih3.Start AS Id3, ih4.Start AS Id4, t.ObjectId1 AS ParentId1, t.ObjectId2 AS ParentId2, t.ObjectId3 AS ParentId3, t.ObjectId4 AS ParentId4, RIGHT('00000' + CONVERT(nvarchar(5), 
                         ISNULL(ih1.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih2.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih3.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), 
                         ISNULL(ih4.Distance, 0)), 5) AS DistancesJoined, e.PrivilegeId, m.Start AS SubjectId, COUNT_BIG(*) AS TotalEntries, SUM(1 - e.Allow) AS DeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND 
                         e.Allow = 0 THEN 1 ELSE 0 END) AS ExplicitDeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND e.Allow = 1 THEN 1 ELSE 0 END) AS ExplicitAllowedEntries, 
                         SUM(CASE WHEN m.[End] = m.[Start] THEN 1 ELSE 0 END) AS NotViaMembership
FROM            dbo.AclEntries AS e INNER JOIN
                         dbo.SecurityTargets AS t ON e.TargetId = t.Id INNER JOIN
                         dbo.SecurityMembershipClosure AS m ON e.SubjectId = m.[End] INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih1 ON ih1.[End] = t.ObjectId1 INNER JOIN
                         dbo.SecurityInheritanceClosure2 AS ih2 ON ih2.[End] = t.ObjectId2 INNER JOIN
                         dbo.SecurityInheritanceClosure3 AS ih3 ON ih3.[End] = t.ObjectId3 INNER JOIN
                         dbo.SecurityInheritanceClosure4 AS ih4 ON ih4.[End] = t.ObjectId4
WHERE        (t.Arity = 4)
GROUP BY ih1.Start, ih2.Start, ih3.Start, ih4.Start, t.ObjectId1, t.ObjectId2, t.ObjectId3, t.ObjectId4, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), 
                         ISNULL(ih2.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih3.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih4.Distance, 0)), 5), e.PrivilegeId, m.Start
");
            Sql(@"CREATE UNIQUE CLUSTERED INDEX [_Security_Tuple4_PK] ON [dbo].[_Security_Tuple4] 
(
	[Id1] ASC,
	[Id2] ASC,
	[Id3] ASC,
	[Id4] ASC,
	[SubjectId] ASC,
	[PrivilegeId] ASC,
	[DistancesJoined] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = ON, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
");

            Sql(@"CREATE NONCLUSTERED INDEX [_Security_Tuple4_Reverse_IX] ON [dbo].[_Security_Tuple4] 
(
	[SubjectId] ASC,
	[PrivilegeId] ASC,
	[Id1] ASC,
	[Id2] ASC,
	[Id3] ASC,
	[Id4] ASC,
	[DistancesJoined] ASC,
	[DeniedEntries] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = ON, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
");
        }
        
        public override void Down()
        {
        }
    }
}
