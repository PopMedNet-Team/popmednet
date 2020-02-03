namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpgradeSecurityPhase1 : DbMigration
    {
        public override void Up()
        {


            Sql("IF EXISTS(select * FROM sys.views where name = 'Security_Tuple1') DROP View dbo.Security_Tuple1");
            Sql("IF EXISTS(select * FROM sys.views where name = 'Security_Tuple2') DROP View dbo.Security_Tuple2");
            Sql("IF EXISTS(select * FROM sys.views where name = 'Security_Tuple3') DROP View dbo.Security_Tuple3");
            Sql("IF EXISTS(select * FROM sys.views where name = 'Security_Tuple4') DROP View dbo.Security_Tuple4");
            Sql("IF EXISTS(select * FROM sys.views where name = 'Security_Tuple5') DROP View dbo.Security_Tuple5");

            CreateTable("dbo.Security_Tuple1", 
                c => new
            {
                ID1 = c.Guid(false),
                ParentID1 = c.Guid(false),
                SubjectID = c.Guid(false),
                PrivilegeID = c.Guid(false),
                ViaMembership = c.Int(false),
                DeniedEntries = c.Int(false),
                ExplicitDeniedEntries = c.Int(false),
                ExplicitAllowedEntries = c.Int(false)
            }).PrimaryKey(t => new {t.ID1, t.ParentID1, t.SubjectID, t.PrivilegeID, t.ViaMembership, t.DeniedEntries, t.ExplicitDeniedEntries, t.ExplicitAllowedEntries});


            CreateTable("dbo.Security_Tuple2",
                c => new
                {
                    ID1 = c.Guid(false),
                    ID2 = c.Guid(false),
                    ParentID1 = c.Guid(false),
                    ParentID2 = c.Guid(false),
                    SubjectID = c.Guid(false),
                    PrivilegeID = c.Guid(false),
                    ViaMembership = c.Int(false),
                    DeniedEntries = c.Int(false),
                    ExplicitDeniedEntries = c.Int(false),
                    ExplicitAllowedEntries = c.Int(false)
                }).PrimaryKey(t => new { t.ID1, t.ID2, t.ParentID1, t.ParentID2, t.SubjectID, t.PrivilegeID, t.ViaMembership, t.DeniedEntries, t.ExplicitDeniedEntries, t.ExplicitAllowedEntries });

            CreateTable("dbo.Security_Tuple3",
                c => new
                {
                    ID1 = c.Guid(false),
                    ID2 = c.Guid(false),
                    ID3 = c.Guid(false),
                    ParentID1 = c.Guid(false),
                    ParentID2 = c.Guid(false),
                    ParentID3 = c.Guid(false),
                    SubjectID = c.Guid(false),
                    PrivilegeID = c.Guid(false),
                    ViaMembership = c.Int(false),
                    DeniedEntries = c.Int(false),
                    ExplicitDeniedEntries = c.Int(false),
                    ExplicitAllowedEntries = c.Int(false)
                }).PrimaryKey(t => new { t.ID1, t.ID2, t.ID3, t.ParentID1, t.ParentID2, t.ParentID3, t.SubjectID, t.PrivilegeID, t.ViaMembership, t.DeniedEntries, t.ExplicitDeniedEntries, t.ExplicitAllowedEntries });

            CreateTable("dbo.Security_Tuple4",
                c => new
                {
                    ID1 = c.Guid(false),
                    ID2 = c.Guid(false),
                    ID3 = c.Guid(false),
                    ID4 = c.Guid(false),
                    ParentID1 = c.Guid(false),
                    ParentID2 = c.Guid(false),
                    ParentID3 = c.Guid(false),
                    ParentID4 = c.Guid(false),
                    SubjectID = c.Guid(false),
                    PrivilegeID = c.Guid(false),
                    ViaMembership = c.Int(false),
                    DeniedEntries = c.Int(false),
                    ExplicitDeniedEntries = c.Int(false),
                    ExplicitAllowedEntries = c.Int(false)
                }).PrimaryKey(t => new { t.ID1, t.ID2, t.ID3, t.ID4, t.ParentID1, t.ParentID2, t.ParentID3, t.ParentID4, t.SubjectID, t.PrivilegeID, t.ViaMembership, t.DeniedEntries, t.ExplicitDeniedEntries, t.ExplicitAllowedEntries });

            CreateTable("dbo.Security_Tuple5",
                c => new
                {
                    ID1 = c.Guid(false),
                    ID2 = c.Guid(false),
                    ID3 = c.Guid(false),
                    ID4 = c.Guid(false),
                    ID5 = c.Guid(false),
                    ParentID1 = c.Guid(false),
                    ParentID2 = c.Guid(false),
                    ParentID3 = c.Guid(false),
                    ParentID4 = c.Guid(false),
                    ParentID5 = c.Guid(false),
                    SubjectID = c.Guid(false),
                    PrivilegeID = c.Guid(false),
                    ViaMembership = c.Int(false),
                    DeniedEntries = c.Int(false),
                    ExplicitDeniedEntries = c.Int(false),
                    ExplicitAllowedEntries = c.Int(false)
                }).PrimaryKey(t => new { t.ID1, t.ID2, t.ID3, t.ID4, t.ID5, t.ParentID1, t.ParentID2, t.ParentID3, t.ParentID4, t.ParentID5, t.SubjectID, t.PrivilegeID, t.ViaMembership, t.DeniedEntries, t.ExplicitDeniedEntries, t.ExplicitAllowedEntries });


            Sql(@"CREATE VIEW [dbo].[Update_Security_Tuple1]
AS
SELECT        Id1, ParentId1, SubjectId, PrivilegeId, 1 - NotViaMembership AS ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries
FROM            (SELECT        ih1.Start AS Id1, t.ObjectId1 AS ParentId1, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) AS DistancesJoined, dbo.AclEntries.PrivilegeId, m.Start AS SubjectId, COUNT_BIG(*) AS TotalEntries, 
                         SUM(1 - dbo.AclEntries.Allow) AS DeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND AclEntries.Allow = 0 THEN 1 ELSE 0 END) AS ExplicitDeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND 
                         AclEntries.Allow = 1 THEN 1 ELSE 0 END) AS ExplicitAllowedEntries, SUM(CASE WHEN m.[End] = m.[Start] THEN 1 ELSE 0 END) AS NotViaMembership
FROM            dbo.AclEntries INNER JOIN
                         dbo.SecurityTargets AS t ON dbo.AclEntries.TargetId = t.Id INNER JOIN
                         dbo.SecurityMembershipClosure AS m ON dbo.AclEntries.SubjectId = m.[End] INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih1 ON ih1.[End] = t.ObjectId1
WHERE        (t.Arity = 1)
GROUP BY ih1.Start, t.ObjectId1, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5), dbo.AclEntries.PrivilegeId, m.Start
) AS x
WHERE        (DistancesJoined =
                             (SELECT        MIN(DistancesJoined) AS Expr1
                               FROM            (SELECT        ih1.Start AS Id1, t.ObjectId1 AS ParentId1, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) AS DistancesJoined, dbo.AclEntries.PrivilegeId, m.Start AS SubjectId, COUNT_BIG(*) AS TotalEntries, 
                         SUM(1 - dbo.AclEntries.Allow) AS DeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND AclEntries.Allow = 0 THEN 1 ELSE 0 END) AS ExplicitDeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND 
                         AclEntries.Allow = 1 THEN 1 ELSE 0 END) AS ExplicitAllowedEntries, SUM(CASE WHEN m.[End] = m.[Start] THEN 1 ELSE 0 END) AS NotViaMembership
FROM            dbo.AclEntries INNER JOIN
                         dbo.SecurityTargets AS t ON dbo.AclEntries.TargetId = t.Id INNER JOIN
                         dbo.SecurityMembershipClosure AS m ON dbo.AclEntries.SubjectId = m.[End] INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih1 ON ih1.[End] = t.ObjectId1
WHERE        (t.Arity = 1)
GROUP BY ih1.Start, t.ObjectId1, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5), dbo.AclEntries.PrivilegeId, m.Start
) AS y
                               WHERE        (x.SubjectId = SubjectId) AND (x.PrivilegeId = PrivilegeId) AND (x.Id1 = Id1)))
");

            Sql(@"CREATE VIEW [dbo].[Update_Security_Tuple2]
AS
SELECT        Id1, Id2, ParentId1, ParentId2, SubjectId, PrivilegeId, 1 - NotViaMembership AS ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries
FROM            (SELECT        ih1.Start AS Id1, ih2.Start AS Id2, t.ObjectId1 AS ParentId1, t.ObjectId2 AS ParentId2, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), 
                         ISNULL(ih2.Distance, 0)), 5) AS DistancesJoined, e.PrivilegeId, m.Start AS SubjectId, COUNT_BIG(*) AS TotalEntries, SUM(1 - e.Allow) AS DeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND 
                         e.Allow = 0 THEN 1 ELSE 0 END) AS ExplicitDeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND e.Allow = 1 THEN 1 ELSE 0 END) AS ExplicitAllowedEntries, 
                         SUM(CASE WHEN m.[End] = m.[Start] THEN 1 ELSE 0 END) AS NotViaMembership
FROM            dbo.AclEntries AS e INNER JOIN
                         dbo.SecurityTargets AS t ON e.TargetId = t.Id INNER JOIN
                         dbo.SecurityMembershipClosure AS m ON e.SubjectId = m.[End] INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih1 ON ih1.[End] = t.ObjectId1 INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih2 ON ih2.[End] = t.ObjectId2
WHERE        (t.Arity = 2)
GROUP BY ih1.Start, ih2.Start, t.ObjectId1, t.ObjectId2, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih2.Distance, 0)), 5), e.PrivilegeId, m.Start
                         
) AS x 
WHERE        (DistancesJoined =
                             (SELECT        MIN(DistancesJoined) AS Expr1
                               FROM            (SELECT        ih1.Start AS Id1, ih2.Start AS Id2, t.ObjectId1 AS ParentId1, t.ObjectId2 AS ParentId2, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), 
                         ISNULL(ih2.Distance, 0)), 5) AS DistancesJoined, e.PrivilegeId, m.Start AS SubjectId, COUNT_BIG(*) AS TotalEntries, SUM(1 - e.Allow) AS DeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND 
                         e.Allow = 0 THEN 1 ELSE 0 END) AS ExplicitDeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND e.Allow = 1 THEN 1 ELSE 0 END) AS ExplicitAllowedEntries, 
                         SUM(CASE WHEN m.[End] = m.[Start] THEN 1 ELSE 0 END) AS NotViaMembership
FROM            dbo.AclEntries AS e INNER JOIN
                         dbo.SecurityTargets AS t ON e.TargetId = t.Id INNER JOIN
                         dbo.SecurityMembershipClosure AS m ON e.SubjectId = m.[End] INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih1 ON ih1.[End] = t.ObjectId1 INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih2 ON ih2.[End] = t.ObjectId2
WHERE        (t.Arity = 2)
GROUP BY ih1.Start, ih2.Start, t.ObjectId1, t.ObjectId2, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih2.Distance, 0)), 5), e.PrivilegeId, m.Start
) AS y
                               WHERE        (x.SubjectId = SubjectId) AND (x.PrivilegeId = PrivilegeId) AND (x.Id1 = Id1) AND (x.Id2 = Id2)))
");

            Sql(@"CREATE VIEW [dbo].[Update_Security_Tuple3]
AS
SELECT        Id1, Id2, Id3, ParentId1, ParentId2, ParentId3, SubjectId, PrivilegeId, 1 - NotViaMembership AS ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries
FROM            (SELECT        ih1.Start AS Id1, ih2.Start AS Id2, ih3.Start AS Id3, t.ObjectId1 AS ParentId1, t.ObjectId2 AS ParentId2, t.ObjectId3 AS ParentId3, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) 
                         + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih2.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih3.Distance, 0)), 5) AS DistancesJoined, e.PrivilegeId, m.Start AS SubjectId, COUNT_BIG(*) 
                         AS TotalEntries, SUM(1 - e.Allow) AS DeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND e.Allow = 0 THEN 1 ELSE 0 END) AS ExplicitDeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND 
                         e.Allow = 1 THEN 1 ELSE 0 END) AS ExplicitAllowedEntries, SUM(CASE WHEN m.[End] = m.[Start] THEN 1 ELSE 0 END) AS NotViaMembership
FROM            dbo.AclEntries AS e INNER JOIN
                         dbo.SecurityTargets AS t ON e.TargetId = t.Id INNER JOIN
                         dbo.SecurityMembershipClosure AS m ON e.SubjectId = m.[End] INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih1 ON ih1.[End] = t.ObjectId1 INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih2 ON ih2.[End] = t.ObjectId2 INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih3 ON ih3.[End] = t.ObjectId3
WHERE        (t.Arity = 3)
GROUP BY ih1.Start, ih2.Start, ih3.Start, t.ObjectId1, t.ObjectId2, t.ObjectId3, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih2.Distance, 0)), 5) 
                         + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih3.Distance, 0)), 5), e.PrivilegeId, m.Start
) AS x
WHERE        (DistancesJoined =
                             (SELECT        MIN(DistancesJoined) AS Expr1
                               FROM            (SELECT        ih1.Start AS Id1, ih2.Start AS Id2, ih3.Start AS Id3, t.ObjectId1 AS ParentId1, t.ObjectId2 AS ParentId2, t.ObjectId3 AS ParentId3, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) 
                         + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih2.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih3.Distance, 0)), 5) AS DistancesJoined, e.PrivilegeId, m.Start AS SubjectId, COUNT_BIG(*) 
                         AS TotalEntries, SUM(1 - e.Allow) AS DeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND e.Allow = 0 THEN 1 ELSE 0 END) AS ExplicitDeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND 
                         e.Allow = 1 THEN 1 ELSE 0 END) AS ExplicitAllowedEntries, SUM(CASE WHEN m.[End] = m.[Start] THEN 1 ELSE 0 END) AS NotViaMembership
FROM            dbo.AclEntries AS e INNER JOIN
                         dbo.SecurityTargets AS t ON e.TargetId = t.Id INNER JOIN
                         dbo.SecurityMembershipClosure AS m ON e.SubjectId = m.[End] INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih1 ON ih1.[End] = t.ObjectId1 INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih2 ON ih2.[End] = t.ObjectId2 INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih3 ON ih3.[End] = t.ObjectId3
WHERE        (t.Arity = 3)
GROUP BY ih1.Start, ih2.Start, ih3.Start, t.ObjectId1, t.ObjectId2, t.ObjectId3, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih2.Distance, 0)), 5) 
                         + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih3.Distance, 0)), 5), e.PrivilegeId, m.Start
) AS y
                               WHERE        (x.SubjectId = SubjectId) AND (x.PrivilegeId = PrivilegeId) AND (x.Id1 = Id1) AND (x.Id2 = Id2) AND (x.Id3 = Id3)))
");

            Sql(@"CREATE view [dbo].[Update_Security_Tuple4]
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
                         dbo.SecurityInheritanceClosure AS ih2 ON ih2.[End] = t.ObjectId2 INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih3 ON ih3.[End] = t.ObjectId3 INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih4 ON ih4.[End] = t.ObjectId4
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
                         dbo.SecurityInheritanceClosure AS ih2 ON ih2.[End] = t.ObjectId2 INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih3 ON ih3.[End] = t.ObjectId3 INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih4 ON ih4.[End] = t.ObjectId4
WHERE        (t.Arity = 4)
GROUP BY ih1.Start, ih2.Start, ih3.Start, ih4.Start, t.ObjectId1, t.ObjectId2, t.ObjectId3, t.ObjectId4, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), 
                         ISNULL(ih2.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih3.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih4.Distance, 0)), 5), e.PrivilegeId, m.Start
) y
	where x.SubjectId = y.SubjectId and x.PrivilegeId = y.PrivilegeId and x.Id1 = y.Id1 and x.Id2 = y.Id2 and x.Id3 = y.Id3 and x.Id4 = y.Id4
)
");

            Sql(@"TRUNCATE TABLE Security_Tuple1
TRUNCATE TABLE Security_Tuple2
TRUNCATE TABLE Security_Tuple3
TRUNCATE TABLE Security_Tuple4
--TRUNCATE TABLE Security_Tuple5

--Security Tuple 1 Initial Inject
INSERT INTO Security_Tuple1 (ID1, ParentID1, SubjectId, PrivilegeId, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) 
SELECT DISTINCT ID1, ParentID1, SubjectId, PrivilegeId, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries FROM Update_Security_Tuple1 ut


--Security Tuple 2 Initial Inject
INSERT INTO Security_Tuple2 (ID1, Id2, ParentID1, ParentID2, SubjectId, PrivilegeId, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) 
SELECT DISTINCT ID1, ID2, ParentID1, ParentID2, SubjectId, PrivilegeId, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries FROM Update_Security_Tuple2 ut


--Security Tuple 3 Initial Inject
INSERT INTO Security_Tuple3 (ID1, Id2, Id3, ParentID1, ParentID2, ParentId3, SubjectId, PrivilegeId, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) 
SELECT DISTINCT ID1, ID2, Id3, ParentID1, ParentID2, ParentId3, SubjectId, PrivilegeId, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries FROM Update_Security_Tuple3 ut


--Security Tuple 4 Initial Inject
INSERT INTO Security_Tuple4 (ID1, Id2, Id3, id4, ParentID1, ParentID2, ParentId3, parentid4, SubjectId, PrivilegeId, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) 
SELECT DISTINCT ID1, ID2, Id3, id4, ParentID1, ParentID2, ParentId3, parentid4, SubjectId, PrivilegeId, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries FROM Update_Security_Tuple4 ut


--Security Tuple 5 Initial Inject
--INSERT INTO Security_Tuple5 (ID1, Id2, Id3, id4, id5, ParentID1, ParentID2, ParentId3, parentid4, parentid5, SubjectId, PrivilegeId, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) 
--SELECT DISTINCT ID1, ID2, Id3, id4, id5, ParentID1, ParentID2, ParentId3, parentid4, parentid5, SubjectId, PrivilegeId, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries FROM Update_Security_Tuple5 ut");
        }
        
        public override void Down()
        {

        }
    }
}
