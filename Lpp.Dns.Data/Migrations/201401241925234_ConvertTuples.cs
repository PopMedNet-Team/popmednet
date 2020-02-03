namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConvertTuples : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER view [dbo].[Security_Tuple5]
as
select 
	Id1, Id2, Id3, Id4, Id5, ParentId1, ParentId2, ParentId3, ParentId4, ParentId5,
	SubjectId, PrivilegeId, 1-NotViaMembership as ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries
from (select 
		ih1.[Start] as Id1, ih2.[Start] as Id2, ih3.[Start] as Id3, ih4.[Start] as Id4, ih5.[Start] as Id5, 
		t.ObjectId1 as ParentId1, t.ObjectId2 as ParentId2, t.ObjectId3 as ParentId3, t.ObjectId4 as ParentId4, t.ObjectId5 as ParentId5,
		right( '00000' + convert( nvarchar(5), isnull( ih1.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih2.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih3.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih4.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih5.Distance, 0 ) ), 5 ) as DistancesJoined,
		e.PrivilegeId, m.[Start] as SubjectId,
		count_big(*) as TotalEntries, sum(1-e.Allow) as DeniedEntries,
		sum( case when m.[End] = m.[Start] and e.Allow = 0 then 1 else 0 end ) as ExplicitDeniedEntries,
		sum( case when m.[End] = m.[Start] and e.Allow = 1 then 1 else 0 end ) as ExplicitAllowedEntries,
		sum( case when m.[End] = m.[Start] then 1 else 0 end ) as NotViaMembership
	from dbo.AclEntries e
	inner join dbo.SecurityTargets t on e.TargetId = t.Id
	inner join dbo.SecurityMembershipClosure m on e.SubjectId = m.[End]
	inner join dbo.SecurityInheritanceClosure ih1 on ih1.[End] = t.ObjectId1
inner join dbo.SecurityInheritanceClosure2 ih2 on ih2.[End] = t.ObjectId2
inner join dbo.SecurityInheritanceClosure3 ih3 on ih3.[End] = t.ObjectId3
inner join dbo.SecurityInheritanceClosure4 ih4 on ih4.[End] = t.ObjectId4
inner join dbo.SecurityInheritanceClosure5 ih5 on ih5.[End] = t.ObjectId5
	where t.Arity = 5
	group by 
		ih1.[Start], ih2.[Start], ih3.[Start], ih4.[Start], ih5.[Start], 
		t.ObjectId1, t.ObjectId2, t.ObjectId3, t.ObjectId4, t.ObjectId5,
		right( '00000' + convert( nvarchar(5), isnull( ih1.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih2.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih3.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih4.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih5.Distance, 0 ) ), 5 ),
		e.PrivilegeId, m.[Start]) x
where x.DistancesJoined = (
	select min(y.DistancesJoined) from (select 
		ih1.[Start] as Id1, ih2.[Start] as Id2, ih3.[Start] as Id3, ih4.[Start] as Id4, ih5.[Start] as Id5, 
		t.ObjectId1 as ParentId1, t.ObjectId2 as ParentId2, t.ObjectId3 as ParentId3, t.ObjectId4 as ParentId4, t.ObjectId5 as ParentId5,
		right( '00000' + convert( nvarchar(5), isnull( ih1.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih2.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih3.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih4.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih5.Distance, 0 ) ), 5 ) as DistancesJoined,
		e.PrivilegeId, m.[Start] as SubjectId,
		count_big(*) as TotalEntries, sum(1-e.Allow) as DeniedEntries,
		sum( case when m.[End] = m.[Start] and e.Allow = 0 then 1 else 0 end ) as ExplicitDeniedEntries,
		sum( case when m.[End] = m.[Start] and e.Allow = 1 then 1 else 0 end ) as ExplicitAllowedEntries,
		sum( case when m.[End] = m.[Start] then 1 else 0 end ) as NotViaMembership
	from dbo.AclEntries e
	inner join dbo.SecurityTargets t on e.TargetId = t.Id
	inner join dbo.SecurityMembershipClosure m on e.SubjectId = m.[End]
	inner join dbo.SecurityInheritanceClosure ih1 on ih1.[End] = t.ObjectId1
inner join dbo.SecurityInheritanceClosure2 ih2 on ih2.[End] = t.ObjectId2
inner join dbo.SecurityInheritanceClosure3 ih3 on ih3.[End] = t.ObjectId3
inner join dbo.SecurityInheritanceClosure4 ih4 on ih4.[End] = t.ObjectId4
inner join dbo.SecurityInheritanceClosure5 ih5 on ih5.[End] = t.ObjectId5
	where t.Arity = 5
	group by 
		ih1.[Start], ih2.[Start], ih3.[Start], ih4.[Start], ih5.[Start], 
		t.ObjectId1, t.ObjectId2, t.ObjectId3, t.ObjectId4, t.ObjectId5,
		right( '00000' + convert( nvarchar(5), isnull( ih1.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih2.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih3.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih4.Distance, 0 ) ), 5 ) + right( '00000' + convert( nvarchar(5), isnull( ih5.Distance, 0 ) ), 5 ),
		e.PrivilegeId, m.[Start]) y
	where x.SubjectId = y.SubjectId and x.PrivilegeId = y.PrivilegeId and x.Id1 = y.Id1 and x.Id2 = y.Id2 and x.Id3 = y.Id3 and x.Id4 = y.Id4 and x.Id5 = y.Id5
)", true);            

            Sql(@"ALTER view [dbo].[Security_Tuple4]
as
select 
	Id1, Id2, Id3, Id4, ParentId1, ParentId2, ParentId3, ParentId4,
	SubjectId, PrivilegeId, 1-NotViaMembership as ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries
from (SELECT        ih1.Start AS Id1, ih2.Start AS Id2, ih3.Start AS Id3, ih4.Start AS Id4, t.ObjectId1 AS ParentId1, t.ObjectId2 AS ParentId2, t.ObjectId3 AS ParentId3, t.ObjectId4 AS ParentId4, RIGHT('00000' + CONVERT(nvarchar(5), 
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
) x
where x.DistancesJoined = (
	select min(y.DistancesJoined) from (SELECT        ih1.Start AS Id1, ih2.Start AS Id2, ih3.Start AS Id3, ih4.Start AS Id4, t.ObjectId1 AS ParentId1, t.ObjectId2 AS ParentId2, t.ObjectId3 AS ParentId3, t.ObjectId4 AS ParentId4, RIGHT('00000' + CONVERT(nvarchar(5), 
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
) y
	where x.SubjectId = y.SubjectId and x.PrivilegeId = y.PrivilegeId and x.Id1 = y.Id1 and x.Id2 = y.Id2 and x.Id3 = y.Id3 and x.Id4 = y.Id4
)", true);

            Sql(@"ALTER VIEW [dbo].[Security_Tuple3]
AS
SELECT        Id1, Id2, Id3, ParentId1, ParentId2, ParentId3, SubjectId, PrivilegeId, 1 - NotViaMembership AS ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries, ChangedOn
FROM            (SELECT        ih1.Start AS Id1, ih2.Start AS Id2, ih3.Start AS Id3, t.ObjectId1 AS ParentId1, t.ObjectId2 AS ParentId2, t.ObjectId3 AS ParentId3, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) 
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
) AS x
WHERE        (DistancesJoined =
                             (SELECT        MIN(DistancesJoined) AS Expr1
                               FROM            (SELECT        ih1.Start AS Id1, ih2.Start AS Id2, ih3.Start AS Id3, t.ObjectId1 AS ParentId1, t.ObjectId2 AS ParentId2, t.ObjectId3 AS ParentId3, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) 
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
) AS y
                               WHERE        (x.SubjectId = SubjectId) AND (x.PrivilegeId = PrivilegeId) AND (x.Id1 = Id1) AND (x.Id2 = Id2) AND (x.Id3 = Id3)))", true);

            Sql(@"ALTER VIEW [dbo].[Security_Tuple2]
AS
SELECT        Id1, Id2, ParentId1, ParentId2, SubjectId, PrivilegeId, 1 - NotViaMembership AS ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries, ChangedOn
FROM            (SELECT        ih1.Start AS Id1, ih2.Start AS Id2, t.ObjectId1 AS ParentId1, t.ObjectId2 AS ParentId2, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), 
                         ISNULL(ih2.Distance, 0)), 5) AS DistancesJoined, e.PrivilegeId, m.Start AS SubjectId, COUNT_BIG(*) AS TotalEntries, SUM(1 - e.Allow) AS DeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND 
                         e.Allow = 0 THEN 1 ELSE 0 END) AS ExplicitDeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND e.Allow = 1 THEN 1 ELSE 0 END) AS ExplicitAllowedEntries, 
                         SUM(CASE WHEN m.[End] = m.[Start] THEN 1 ELSE 0 END) AS NotViaMembership, e.ChangedOn
FROM            dbo.AclEntries AS e INNER JOIN
                         dbo.SecurityTargets AS t ON e.TargetId = t.Id INNER JOIN
                         dbo.SecurityMembershipClosure AS m ON e.SubjectId = m.[End] INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih1 ON ih1.[End] = t.ObjectId1 INNER JOIN
                         dbo.SecurityInheritanceClosure2 AS ih2 ON ih2.[End] = t.ObjectId2
WHERE        (t.Arity = 2)
GROUP BY ih1.Start, ih2.Start, t.ObjectId1, t.ObjectId2, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih2.Distance, 0)), 5), e.PrivilegeId, m.Start, 
                         e.ChangedOn
) AS x 
WHERE        (DistancesJoined =
                             (SELECT        MIN(DistancesJoined) AS Expr1
                               FROM            (SELECT        ih1.Start AS Id1, ih2.Start AS Id2, t.ObjectId1 AS ParentId1, t.ObjectId2 AS ParentId2, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), 
                         ISNULL(ih2.Distance, 0)), 5) AS DistancesJoined, e.PrivilegeId, m.Start AS SubjectId, COUNT_BIG(*) AS TotalEntries, SUM(1 - e.Allow) AS DeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND 
                         e.Allow = 0 THEN 1 ELSE 0 END) AS ExplicitDeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND e.Allow = 1 THEN 1 ELSE 0 END) AS ExplicitAllowedEntries, 
                         SUM(CASE WHEN m.[End] = m.[Start] THEN 1 ELSE 0 END) AS NotViaMembership, e.ChangedOn
FROM            dbo.AclEntries AS e INNER JOIN
                         dbo.SecurityTargets AS t ON e.TargetId = t.Id INNER JOIN
                         dbo.SecurityMembershipClosure AS m ON e.SubjectId = m.[End] INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih1 ON ih1.[End] = t.ObjectId1 INNER JOIN
                         dbo.SecurityInheritanceClosure2 AS ih2 ON ih2.[End] = t.ObjectId2
WHERE        (t.Arity = 2)
GROUP BY ih1.Start, ih2.Start, t.ObjectId1, t.ObjectId2, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih2.Distance, 0)), 5), e.PrivilegeId, m.Start, 
                         e.ChangedOn
) AS y
                               WHERE        (x.SubjectId = SubjectId) AND (x.PrivilegeId = PrivilegeId) AND (x.Id1 = Id1) AND (x.Id2 = Id2)))", true);

            Sql(@"ALTER VIEW [dbo].[Security_Tuple1]
AS
SELECT        Id1, ParentId1, SubjectId, PrivilegeId, 1 - NotViaMembership AS ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries, ChangedOn
FROM            (SELECT        ih1.Start AS Id1, t.ObjectId1 AS ParentId1, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) AS DistancesJoined, dbo.AclEntries.PrivilegeId, m.Start AS SubjectId, COUNT_BIG(*) AS TotalEntries, 
                         SUM(1 - dbo.AclEntries.Allow) AS DeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND AclEntries.Allow = 0 THEN 1 ELSE 0 END) AS ExplicitDeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND 
                         AclEntries.Allow = 1 THEN 1 ELSE 0 END) AS ExplicitAllowedEntries, SUM(CASE WHEN m.[End] = m.[Start] THEN 1 ELSE 0 END) AS NotViaMembership, dbo.AclEntries.ChangedOn
FROM            dbo.AclEntries INNER JOIN
                         dbo.SecurityTargets AS t ON dbo.AclEntries.TargetId = t.Id INNER JOIN
                         dbo.SecurityMembershipClosure AS m ON dbo.AclEntries.SubjectId = m.[End] INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih1 ON ih1.[End] = t.ObjectId1
WHERE        (t.Arity = 1)
GROUP BY ih1.Start, t.ObjectId1, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5), dbo.AclEntries.PrivilegeId, m.Start, dbo.AclEntries.ChangedOn
) AS x
WHERE        (DistancesJoined =
                             (SELECT        MIN(DistancesJoined) AS Expr1
                               FROM            (SELECT        ih1.Start AS Id1, t.ObjectId1 AS ParentId1, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) AS DistancesJoined, dbo.AclEntries.PrivilegeId, m.Start AS SubjectId, COUNT_BIG(*) AS TotalEntries, 
                         SUM(1 - dbo.AclEntries.Allow) AS DeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND AclEntries.Allow = 0 THEN 1 ELSE 0 END) AS ExplicitDeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND 
                         AclEntries.Allow = 1 THEN 1 ELSE 0 END) AS ExplicitAllowedEntries, SUM(CASE WHEN m.[End] = m.[Start] THEN 1 ELSE 0 END) AS NotViaMembership, dbo.AclEntries.ChangedOn
FROM            dbo.AclEntries INNER JOIN
                         dbo.SecurityTargets AS t ON dbo.AclEntries.TargetId = t.Id INNER JOIN
                         dbo.SecurityMembershipClosure AS m ON dbo.AclEntries.SubjectId = m.[End] INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih1 ON ih1.[End] = t.ObjectId1
WHERE        (t.Arity = 1)
GROUP BY ih1.Start, t.ObjectId1, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5), dbo.AclEntries.PrivilegeId, m.Start, dbo.AclEntries.ChangedOn
) AS y
                               WHERE        (x.SubjectId = SubjectId) AND (x.PrivilegeId = PrivilegeId) AND (x.Id1 = Id1)))", true);

            Sql(@"DROP VIEW dbo._Security_Tuple5", true);
            Sql(@"DROP VIEW dbo._Security_Tuple4", true);
            Sql(@"DROP VIEW dbo._Security_Tuple3", true);
            Sql(@"DROP VIEW [dbo].[_Security_Tuple2]", true);
            Sql(@"DROP VIEW dbo._Security_Tuple1");
        }
        
        public override void Down()
        {
        }
    }
}
