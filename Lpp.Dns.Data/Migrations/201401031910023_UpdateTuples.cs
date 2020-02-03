namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTuples : DbMigration
    {
        public override void Up()
        {
            //Tuple 1
            Sql(@"DROP View dbo._Security_Tuple1");
            Sql(@"CREATE VIEW dbo._Security_Tuple1 WITH SCHEMABINDING
AS
SELECT        ih1.Start AS Id1, t.ObjectId1 AS ParentId1, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) AS DistancesJoined, e.PrivilegeId, m.Start AS SubjectId, COUNT_BIG(*) AS TotalEntries, 
                         SUM(1 - e.Allow) AS DeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND e.Allow = 0 THEN 1 ELSE 0 END) AS ExplicitDeniedEntries, SUM(CASE WHEN m.[End] = m.[Start] AND e.Allow = 1 THEN 1 ELSE 0 END)
                          AS ExplicitAllowedEntries, SUM(CASE WHEN m.[End] = m.[Start] THEN 1 ELSE 0 END) AS NotViaMembership, e.ChangedOn
FROM            dbo.AclEntries AS e INNER JOIN
                         dbo.SecurityTargets AS t ON e.TargetId = t.Id INNER JOIN
                         dbo.SecurityMembershipClosure AS m ON e.SubjectId = m.[End] INNER JOIN
                         dbo.SecurityInheritanceClosure AS ih1 ON ih1.[End] = t.ObjectId1
WHERE        (t.Arity = 1)
GROUP BY ih1.Start, t.ObjectId1, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5), e.PrivilegeId, m.Start, e.ChangedOn");

            Sql(@"CREATE UNIQUE CLUSTERED INDEX _Security_Tuple1_PK ON dbo._Security_Tuple1 (Id1, SubjectId, PrivilegeId, DistancesJoined)");

            Sql(@"CREATE INDEX _Security_Tuple1_Reverse_IX ON dbo._Security_Tuple1 (SubjectId, PrivilegeId, Id1, DistancesJoined, DeniedEntries)");

            Sql("CREATE INDEX _Security_Tuple1_Sync ON dbo._Security_Tuple1 (ChangedOn, PrivilegeId)");

            //Tuple 2
            Sql(@"DROP View dbo._Security_Tuple2");

            Sql(@"CREATE VIEW dbo._Security_Tuple2 WITH SCHEMABINDING
AS
SELECT        ih1.Start AS Id1, ih2.Start AS Id2, t.ObjectId1 AS ParentId1, t.ObjectId2 AS ParentId2, RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih1.Distance, 0)), 5) + RIGHT('00000' + CONVERT(nvarchar(5), 
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
                         e.ChangedOn");
            Sql(@"CREATE UNIQUE CLUSTERED INDEX _Security_Tuple2_PK ON dbo._Security_Tuple2 (Id1, Id2, SubjectId, PrivilegeId, DistancesJoined)");

            Sql(@"CREATE INDEX _Security_Tuple2_Reverse_IX ON dbo._Security_Tuple2 (SubjectId, PrivilegeId, Id1, Id2, DistancesJoined, DeniedEntries)");

            Sql(@"CREATE INDEX _Security_Tuple2_Sync ON dbo._Security_Tuple2 (ChangedOn, PrivilegeId)");

            //Tuple 3
            Sql(@"DROP View dbo._Security_Tuple3");

            Sql(@"CREATE VIEW dbo._Security_Tuple3 WITH SCHEMABINDING
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
                         + RIGHT('00000' + CONVERT(nvarchar(5), ISNULL(ih3.Distance, 0)), 5), e.PrivilegeId, m.Start, e.ChangedOn");

            Sql(@"CREATE UNIQUE CLUSTERED INDEX _Security_Tuple3_PK ON dbo._Security_Tuple3 (Id1, Id2, Id3, SubjectId, PrivilegeId, DistancesJoined)");

            Sql(@"CREATE INDEX _Security_Tuple3_Reverse_IX ON dbo._Security_Tuple3 (SubjectId, PrivilegeId, Id1, Id2, Id3, DistancesJoined, DeniedEntries)");

            Sql(@"CREATE INDEX _Security_Tuple3_Sync ON dbo._Security_Tuple3 (ChangedOn, PrivilegeId)");
        }
        
        public override void Down()
        {
        }
    }
}
