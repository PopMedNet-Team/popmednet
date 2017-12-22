namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDuplicateInsertsOnTriggers : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TRIGGER [dbo].[ACLEntries_UpdateTuples] 
    ON  [dbo].[AclEntries]
    AFTER UPDATE, INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    MERGE INTO Security_Tuple1 AS Target
	USING (SELECT DISTINCT Source.Id1, Source.parentid1, m.[Start] AS SubjectId, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
		FROM Update_Security_Tuple1 Source 
			INNER JOIN (SELECT * FROM (SELECT * from inserted except SELECT * FROM deleted) a) inserted ON inserted.PrivilegeId = Source.PrivilegeId
			INNER JOIN SecurityMembershipClosure m ON m.[End] = inserted.SubjectId AND m.[Start] = Source.SubjectId
			INNER JOIN SecurityTargets t ON inserted.TargetId = t.Id AND t.Arity = 1 AND t.ObjectId1 = Source.ParentId1
			INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = t.ObjectId1 AND ic1.[Start] = Source.Id1 
		) AS Source (Id1, parentid1, subjectid, privilegeid, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) ON 
		(Target.id1 = Source.id1 AND Target.ParentId1 = Source.ParentId1  AND Target.SubjectId = Source.SubjectId AND Target.PrivilegeId = Source.PrivilegeId AND Target.ViaMembership = Source.ViaMembership AND Target.DeniedEntries = Source.DeniedEntries AND Target.ExplicitDeniedEntries = Source.ExplicitDeniedEntries AND Target.ExplicitAllowedEntries = Source.ExplicitAllowedEntries AND EXISTS(SELECT NULL FROM SecurityTargets t WHERE t.ObjectId1 = Source.ParentId1 AND t.Arity = 1))

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT (Id1, ParentId1, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) VALUES (Source.Id1, Source.ParentId1, Source.SubjectId, Source.PrivilegeId, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries);

	--Security Tuple 2
    MERGE INTO Security_Tuple2 AS Target
	USING (SELECT DISTINCT Source.Id1, Source.id2, Source.parentid1, Source.parentid2, m.[Start] AS subjectid, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
			FROM Update_Security_Tuple2 Source 
			INNER JOIN (SELECT * FROM (SELECT * from inserted except SELECT * FROM deleted) a) inserted ON inserted.PrivilegeId = Source.PrivilegeId
			INNER JOIN SecurityMembershipClosure m ON m.[End] = inserted.SubjectId AND m.[Start] = Source.SubjectId
			INNER JOIN SecurityTargets t ON inserted.TargetId = t.Id AND t.Arity = 2 AND t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2
			INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = t.ObjectId1 AND ic1.[Start] = Source.Id1 
			INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = t.ObjectId2 AND ic2.[Start] = Source.Id2 
		) AS Source (Id1, id2, parentid1, parentid2, subjectid, privilegeid, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) ON 
		(Target.id1 = Source.id1 AND Target.id2 = Source.id2 AND Target.ParentId1 = Source.ParentId1 AND Target.ParentId2 = Source.ParentId2 AND Target.SubjectId = Source.SubjectId AND Target.PrivilegeId = Source.PrivilegeId AND Target.ViaMembership = Source.ViaMembership AND Target.DeniedEntries = Source.DeniedEntries AND Target.ExplicitDeniedEntries = Source.ExplicitDeniedEntries AND Target.ExplicitAllowedEntries = Source.ExplicitAllowedEntries AND EXISTS(SELECT NULL FROM SecurityTargets t WHERE t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2 AND t.Arity = 2))

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT (Id1, Id2, ParentId1, ParentId2, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) VALUES (Source.Id1, Source.Id2, Source.ParentId1, Source.ParentId2, Source.SubjectId, Source.PrivilegeId, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries);

	--Security Tuple 3
    MERGE INTO Security_Tuple3 AS Target
	USING (SELECT DISTINCT Source.Id1, Source.id2, Source.id3, Source.parentid1, Source.parentid2, Source.parentid3, m.[Start] AS subjectid, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
			FROM Update_Security_Tuple3 Source
			INNER JOIN (SELECT * FROM (SELECT * from inserted except SELECT * FROM deleted) a) inserted ON inserted.PrivilegeId = Source.PrivilegeId
			INNER JOIN SecurityMembershipClosure m ON m.[End] = inserted.SubjectId AND m.[Start] = Source.SubjectId
			INNER JOIN SecurityTargets t ON inserted.TargetId = t.Id AND t.Arity = 3 AND t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2 AND t.ObjectId3 = Source.ParentId3
			INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = t.ObjectId1 AND ic1.[Start] = Source.Id1 
			INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = t.ObjectId2 AND ic2.[Start] = Source.Id2 
			INNER JOIN SecurityInheritanceClosure AS ic3 ON ic3.[End] = t.ObjectId3 AND ic3.[Start] = Source.Id3
		) AS Source (Id1, id2, id3, parentid1, parentid2, parentid3, subjectid, privilegeid, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) ON 
		(Target.id1 = Source.id1 AND Target.id2 = Source.id2 AND Target.id3 = Source.id3 AND Target.ParentId1 = Source.ParentId1 AND Target.ParentId2 = Source.ParentId2 AND Target.ParentId3 = Source.ParentId3 AND Target.SubjectId = Source.SubjectId AND Target.PrivilegeId = Source.PrivilegeId AND Target.ViaMembership = Source.ViaMembership AND Target.DeniedEntries = Source.DeniedEntries AND Target.ExplicitDeniedEntries = Source.ExplicitDeniedEntries AND Target.ExplicitAllowedEntries = Source.ExplicitAllowedEntries  AND EXISTS(SELECT NULL FROM SecurityTargets t WHERE t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2 AND t.ObjectId3 = Source.ParentId3 AND t.Arity = 3))

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT (Id1, Id2, Id3, ParentId1, ParentId2, ParentId3, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) VALUES (Source.Id1, Source.Id2, Source.Id3, Source.ParentId1, Source.ParentId2, Source.ParentId3, Source.SubjectId, Source.PrivilegeId, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries);

	--Security Tuple 4
    MERGE INTO Security_Tuple4 AS Target
	USING (SELECT DISTINCT Source.Id1, Source.id2, Source.id3, Source.id4, Source.parentid1, Source.parentid2, Source.parentid3, Source.parentid4, m.[start] AS subjectid, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
			FROM Update_Security_Tuple4 Source 
			INNER JOIN (SELECT * FROM (SELECT * from inserted except SELECT * FROM deleted) a) inserted ON inserted.PrivilegeId = Source.PrivilegeId
			INNER JOIN SecurityMembershipClosure m ON m.[End] = inserted.SubjectId AND m.[Start] = Source.SubjectId
			INNER JOIN SecurityTargets t ON inserted.TargetId = t.Id AND t.Arity = 4 AND t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2 AND t.ObjectId3 = Source.ParentId3 AND t.ObjectId4 = Source.ParentId4
			INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = t.ObjectId1 AND ic1.[Start] = Source.Id1 
			INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = t.ObjectId2 AND ic2.[Start] = Source.Id2 
			INNER JOIN SecurityInheritanceClosure AS ic3 ON ic3.[End] = t.ObjectId3 AND ic3.[Start] = Source.Id3
			INNER JOIN SecurityInheritanceClosure AS ic4 ON ic4.[End] = t.ObjectId4 AND ic4.[Start] = Source.Id4
		) AS Source (Id1, id2, id3, id4, parentid1, parentid2, parentid3, parentid4, subjectid, privilegeid, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) ON 
		(Target.id1 = Source.id1 AND Target.id2 = Source.id2 AND Target.id3 = Source.id3 AND Target.id4 = Source.id4 AND Target.ParentId1 = Source.ParentId1 AND Target.ParentId2 = Source.ParentId2 AND Target.ParentId3 = Source.ParentId3 AND Target.ParentId4 = Source.ParentId4 AND Target.SubjectId = Source.SubjectId AND Target.PrivilegeId = Source.PrivilegeId AND Target.ViaMembership = Source.ViaMembership AND Target.DeniedEntries = Source.DeniedEntries AND Target.ExplicitDeniedEntries = Source.ExplicitDeniedEntries AND Target.ExplicitAllowedEntries = Source.ExplicitAllowedEntries  AND EXISTS(SELECT NULL FROM SecurityTargets t WHERE t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2 AND t.ObjectId3 = Source.ParentId3 AND t.ObjectId4 = Source.ParentId4 AND t.Arity = 4))

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT (Id1, Id2, Id3, Id4, ParentId1, ParentId2, ParentId3, ParentId4, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) VALUES (Source.Id1, Source.Id2, Source.Id3, Source.Id4, Source.ParentId1, Source.ParentId2, Source.ParentId3, Source.ParentId4, Source.SubjectId, Source.PrivilegeId, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries);
END
");
        }
        
        public override void Down()
        {
        }
    }
}
