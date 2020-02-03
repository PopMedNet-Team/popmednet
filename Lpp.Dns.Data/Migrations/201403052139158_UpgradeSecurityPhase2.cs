namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpgradeSecurityPhase2 : DbMigration
    {
        public override void Up()
        {
            Sql(@"  DECLARE @sql nvarchar(max)
                    SET @sql = 'ALTER DATABASE [' + db_name() + '] SET RECURSIVE_TRIGGERS ON'
                    EXECUTE sp_executesql @sql", true);

            Sql(@"DROP TRIGGER SecurityInheritanceClosure_Copy_Delete");
            Sql(@"DROP TRIGGER SecurityInheritanceClosure_Copy_Insert");
            Sql(@"DROP TRIGGER SecurityInheritanceClosure_Copy_Update");

            Sql(@"DROP TABLE dbo.SecurityInheritanceClosure2");
            Sql(@"DROP TABLE dbo.SecurityInheritanceClosure3");
            Sql(@"DROP TABLE dbo.SecurityInheritanceClosure4");
            Sql(@"DROP TABLE dbo.SecurityInheritanceClosure5");

            Sql(@"CREATE TRIGGER [dbo].[ACLEntries_DeleteTuples] 
    ON  [dbo].[AclEntries]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE Security_Tuple1 FROM Security_Tuple1 as tuple
	INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple1 st 
				INNER JOIN deleted ON st.privilegeId = deleted.privilegeId 
				INNER JOIN SecurityTargets AS targets ON deleted.TargetId = targets.Id AND targets.ObjectId1 = st.ParentId1 AND targets.Arity = 1
				INNER JOIN SecurityMembershipClosure AS m ON m.[End] = deleted.subjectid AND m.[Start] = st.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = st.Id1 
				except SELECT ust.* FROM Update_Security_Tuple1 ust
				INNER JOIN deleted ON ust.privilegeId = deleted.privilegeId 
				INNER JOIN SecurityTargets AS targets ON deleted.TargetId = targets.Id AND targets.ObjectId1 = ust.ParentId1 AND targets.Arity = 1
				INNER JOIN SecurityMembershipClosure AS m ON m.[End] = deleted.subjectid AND m.[Start] = ust.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = ust.Id1 
				) a) AS d
	ON d.Id1 = tuple.Id1 AND d.ParentId1 = tuple.ParentId1 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries


		DELETE Security_Tuple2 FROM Security_Tuple2 as tuple
	INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple2 st 
				INNER JOIN deleted ON st.privilegeId = deleted.privilegeId 
				INNER JOIN SecurityTargets AS targets ON deleted.TargetId = targets.Id AND targets.ObjectId1 = st.ParentId1 AND targets.ObjectId2 = st.ParentId2 AND targets.Arity = 2
				INNER JOIN SecurityMembershipClosure AS m ON m.[End] = deleted.subjectid AND m.[Start] = st.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = st.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = targets.ObjectId2 AND ic2.[Start] = st.Id2 
				except SELECT ust.* FROM Update_Security_Tuple2 ust
				INNER JOIN deleted ON ust.privilegeId = deleted.privilegeId 
				INNER JOIN SecurityTargets AS targets ON deleted.TargetId = targets.Id AND targets.ObjectId1 = ust.ParentId1 AND targets.ObjectId2 = ust.ParentId2 AND targets.Arity = 2
				INNER JOIN SecurityMembershipClosure AS m ON m.[End] = deleted.subjectid AND m.[Start] = ust.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = ust.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = targets.ObjectId2 AND ic2.[Start] = ust.Id2 
				) a) AS d
	ON d.Id1 = tuple.Id1 AND d.Id2 = tuple.Id2 AND d.ParentId1 = tuple.ParentId1 AND d.ParentId2 = tuple.ParentId2 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries


		DELETE Security_Tuple3 FROM Security_Tuple3 as tuple
	INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple3 st 
				INNER JOIN deleted ON st.privilegeId = deleted.privilegeId 
				INNER JOIN SecurityTargets AS targets ON deleted.TargetId = targets.Id AND targets.ObjectId1 = st.ParentId1 AND targets.ObjectId2 = st.ParentId2 AND targets.ObjectId3 = st.ParentId3 AND targets.Arity = 3
				INNER JOIN SecurityMembershipClosure AS m ON m.[End] = deleted.subjectid AND m.[Start] = st.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = st.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = targets.ObjectId2 AND ic2.[Start] = st.Id2 
				INNER JOIN SecurityInheritanceClosure AS ic3 ON ic3.[End] = targets.ObjectId3 AND ic3.[Start] = st.Id3
				except SELECT ust.* FROM Update_Security_Tuple3 ust
				INNER JOIN deleted ON ust.privilegeId = deleted.privilegeId 
				INNER JOIN SecurityTargets AS targets ON deleted.TargetId = targets.Id AND targets.ObjectId1 = ust.ParentId1 AND targets.ObjectId2 = ust.ParentId2 AND targets.ObjectId3 = ust.ParentId3 AND targets.Arity = 3
				INNER JOIN SecurityMembershipClosure AS m ON m.[End] = deleted.subjectid AND m.[Start] = ust.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = ust.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = targets.ObjectId2 AND ic2.[Start] = ust.Id2 
				INNER JOIN SecurityInheritanceClosure AS ic3 ON ic3.[End] = targets.ObjectId3 AND ic3.[Start] = ust.Id3 
				) a) AS d
	ON d.Id1 = tuple.Id1 AND d.Id2 = tuple.Id2 AND d.Id3 = tuple.Id3 AND d.ParentId1 = tuple.ParentId1 AND d.ParentId2 = tuple.ParentId2 AND d.ParentId3 = tuple.ParentId3 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries


DELETE Security_Tuple4 FROM Security_Tuple4 as tuple
	INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple4 st 
				INNER JOIN deleted ON st.privilegeId = deleted.privilegeId 
				INNER JOIN SecurityTargets AS targets ON deleted.TargetId = targets.Id AND targets.ObjectId1 = st.ParentId1 AND targets.ObjectId2 = st.ParentId2 AND targets.ObjectId3 = st.ParentId3 AND targets.ObjectId4 = st.ParentId4 AND targets.Arity = 4
				INNER JOIN SecurityMembershipClosure AS m ON m.[End] = deleted.subjectid AND m.[Start] = st.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = st.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = targets.ObjectId2 AND ic2.[Start] = st.Id2 
				INNER JOIN SecurityInheritanceClosure AS ic3 ON ic3.[End] = targets.ObjectId3 AND ic3.[Start] = st.Id3
				INNER JOIN SecurityInheritanceClosure AS ic4 ON ic4.[End] = targets.ObjectId4 AND ic4.[Start] = st.Id4
				except SELECT ust.* FROM Update_Security_Tuple4 ust
				INNER JOIN deleted ON ust.privilegeId = deleted.privilegeId 
				INNER JOIN SecurityTargets AS targets ON deleted.TargetId = targets.Id AND targets.ObjectId1 = ust.ParentId1 AND targets.ObjectId2 = ust.ParentId2 AND targets.ObjectId3 = ust.ParentId3 AND targets.objectid4 = ust.ParentId4 AND targets.Arity = 4
				INNER JOIN SecurityMembershipClosure AS m ON m.[End] = deleted.subjectid AND m.[Start] = ust.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = ust.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = targets.ObjectId2 AND ic2.[Start] = ust.Id2 
				INNER JOIN SecurityInheritanceClosure AS ic3 ON ic3.[End] = targets.ObjectId3 AND ic3.[Start] = ust.Id3
				INNER JOIN SecurityInheritanceClosure AS ic4 ON ic4.[End] = targets.ObjectId4 AND ic4.[Start] = ust.Id4
				) a) AS d
	ON d.Id1 = tuple.Id1 AND d.Id2 = tuple.Id2 AND d.Id3 = tuple.Id3 AND d.ID4 = tuple.ID4 AND d.ParentId1 = tuple.ParentId1 AND d.ParentId2 = tuple.ParentId2 AND d.ParentId3 = tuple.ParentId3 AND d.ParentId4 = tuple.ParentId4 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries
END");

            Sql(@"CREATE TRIGGER [dbo].[ACLEntries_UpdateTuples] 
    ON  [dbo].[AclEntries]
    AFTER UPDATE, INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    MERGE INTO Security_Tuple1 AS Target
	USING (SELECT Source.Id1, Source.parentid1, m.[Start] AS SubjectId, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
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
	USING (SELECT Source.Id1, Source.id2, Source.parentid1, Source.parentid2, m.[Start] AS subjectid, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
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
	USING (SELECT Source.Id1, Source.id2, Source.id3, Source.parentid1, Source.parentid2, Source.parentid3, m.[Start] AS subjectid, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
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
	USING (SELECT Source.Id1, Source.id2, Source.id3, Source.id4, Source.parentid1, Source.parentid2, Source.parentid3, Source.parentid4, m.[start] AS subjectid, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
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

            Sql(@"CREATE TRIGGER [dbo].[SecurityTargets_DeleteTuples] 
    ON  [dbo].[SecurityTargets]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE Security_Tuple1 FROM Security_Tuple1 as tuple
	INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple1 st 
				INNER JOIN deleted ON st.ParentId1 = deleted.ObjectId1 AND deleted.Arity = 1			
				INNER JOIN SecurityMembershipClosure AS m ON m.[Start] = st.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = deleted.ObjectId1 AND ic1.[Start] = st.Id1 
				INNER JOIN AclEntries e ON m.[End] = e.subjectid AND st.PrivilegeId = e.PrivilegeId AND e.TargetId = deleted.Id
				except SELECT ust.* FROM Update_Security_Tuple1 ust
				INNER JOIN deleted ON ust.ParentId1 = deleted.ObjectId1 AND deleted.Arity = 1			
				INNER JOIN SecurityMembershipClosure AS m ON m.[Start] = ust.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = deleted.ObjectId1 AND ic1.[Start] = ust.Id1 
				INNER JOIN AclEntries e ON m.[End] = e.subjectid AND ust.PrivilegeId = e.PrivilegeId AND e.TargetId = deleted.Id
				) a) AS d
	ON d.Id1 = tuple.Id1 AND d.ParentId1 = tuple.ParentId1 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries


		DELETE Security_Tuple2 FROM Security_Tuple2 as tuple
	INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple2 st 
				INNER JOIN deleted ON st.ParentId1 = deleted.ObjectId1 AND st.ParentId2 = deleted.ObjectId2 AND deleted.Arity = 2			
				INNER JOIN SecurityMembershipClosure AS m ON m.[Start] = st.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = deleted.ObjectId1 AND ic1.[Start] = st.Id1 
				INNER JOIN SecurityInheritanceClosure2 AS ic2 ON ic2.[End] = deleted.ObjectId2 AND ic2.[Start] = st.Id2 
				INNER JOIN AclEntries e ON m.[End] = e.subjectid AND st.PrivilegeId = e.PrivilegeId AND e.TargetId = deleted.Id
				except SELECT ust.* FROM Update_Security_Tuple2 ust
				INNER JOIN deleted ON ust.ParentId1 = deleted.ObjectId1 AND ust.ParentId2 = deleted.ObjectId2 AND deleted.Arity = 2			
				INNER JOIN SecurityMembershipClosure AS m ON m.[Start] = ust.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = deleted.ObjectId1 AND ic1.[Start] = ust.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = deleted.ObjectId2 AND ic2.[Start] = ust.Id2 
				INNER JOIN AclEntries e ON m.[End] = e.subjectid AND ust.PrivilegeId = e.PrivilegeId AND e.TargetId = deleted.Id
				) a) AS d
	ON d.Id1 = tuple.Id1 AND d.Id2 = tuple.Id2 AND d.ParentId1 = tuple.ParentId1 AND d.ParentId2 = tuple.ParentId2 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries


		DELETE Security_Tuple3 FROM Security_Tuple3 as tuple
	INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple3 st 
				INNER JOIN deleted ON st.ParentId1 = deleted.ObjectId1 AND st.ParentId2 = deleted.ObjectId2 AND st.ParentId3 = deleted.ObjectId3 AND deleted.Arity = 3			
				INNER JOIN SecurityMembershipClosure AS m ON m.[Start] = st.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = deleted.ObjectId1 AND ic1.[Start] = st.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = deleted.ObjectId2 AND ic2.[Start] = st.Id2 
				INNER JOIN SecurityInheritanceClosure AS ic3 ON ic3.[End] = deleted.ObjectId3 AND ic3.[Start] = st.Id3 
				INNER JOIN AclEntries e ON m.[End] = e.subjectid AND st.PrivilegeId = e.PrivilegeId AND e.TargetId = deleted.Id
				except SELECT ust.* FROM Update_Security_Tuple3 ust
				INNER JOIN deleted ON ust.ParentId1 = deleted.ObjectId1 AND ust.ParentId2 = deleted.ObjectId2 AND ust.ParentId3 = deleted.ObjectId3 AND deleted.Arity = 3			
				INNER JOIN SecurityMembershipClosure AS m ON m.[Start] = ust.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = deleted.ObjectId1 AND ic1.[Start] = ust.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = deleted.ObjectId2 AND ic2.[Start] = ust.Id2 
				INNER JOIN SecurityInheritanceClosure AS ic3 ON ic3.[End] = deleted.ObjectId3 AND ic3.[Start] = ust.Id3 
				INNER JOIN AclEntries e ON m.[End] = e.subjectid AND ust.PrivilegeId = e.PrivilegeId AND e.TargetId = deleted.Id
				) a) AS d
	ON d.Id1 = tuple.Id1 AND d.Id2 = tuple.Id2 AND d.Id3 = tuple.Id3 AND d.ParentId1 = tuple.ParentId1 AND d.ParentId2 = tuple.ParentId2 AND d.ParentId3 = tuple.ParentId3 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries


DELETE Security_Tuple4 FROM Security_Tuple4 as tuple
	INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple4 st 
				INNER JOIN deleted ON st.ParentId1 = deleted.ObjectId1 AND st.ParentId2 = deleted.ObjectId2 AND st.ParentId3 = deleted.ObjectId3 AND st.ParentId4 = deleted.ObjectId4 AND deleted.Arity = 4			
				INNER JOIN SecurityMembershipClosure AS m ON m.[Start] = st.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = deleted.ObjectId1 AND ic1.[Start] = st.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = deleted.ObjectId2 AND ic2.[Start] = st.Id2 
				INNER JOIN SecurityInheritanceClosure AS ic3 ON ic3.[End] = deleted.ObjectId3 AND ic3.[Start] = st.Id3 
				INNER JOIN SecurityInheritanceClosure AS ic4 ON ic4.[End] = deleted.ObjectId4 AND ic4.[Start] = st.Id4 
				INNER JOIN AclEntries e ON m.[End] = e.subjectid AND st.PrivilegeId = e.PrivilegeId AND e.TargetId = deleted.Id
				except SELECT ust.* FROM Update_Security_Tuple4 ust
				INNER JOIN deleted ON ust.ParentId1 = deleted.ObjectId1 AND ust.ParentId2 = deleted.ObjectId2 AND ust.ParentId3 = deleted.ObjectId3 AND ust.ParentId4 = deleted.ObjectId4 AND deleted.Arity = 4			
				INNER JOIN SecurityMembershipClosure AS m ON m.[Start] = ust.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = deleted.ObjectId1 AND ic1.[Start] = ust.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = deleted.ObjectId2 AND ic2.[Start] = ust.Id2 
				INNER JOIN SecurityInheritanceClosure AS ic3 ON ic3.[End] = deleted.ObjectId3 AND ic3.[Start] = ust.Id3 
				INNER JOIN SecurityInheritanceClosure AS ic4 ON ic4.[End] = deleted.ObjectId4 AND ic4.[Start] = ust.Id4 
				INNER JOIN AclEntries e ON m.[End] = e.subjectid AND ust.PrivilegeId = e.PrivilegeId AND e.TargetId = deleted.Id
				) a) AS d
	ON d.Id1 = tuple.Id1 AND d.Id2 = tuple.Id2 AND d.Id3 = tuple.Id3 AND d.ID4 = tuple.ID4 AND d.ParentId1 = tuple.ParentId1 AND d.ParentId2 = tuple.ParentId2 AND d.ParentId3 = tuple.ParentId3 AND d.ParentId4 = tuple.ParentId4 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries
END
");

            Sql(@"CREATE TRIGGER [dbo].[SecurityTargets_UpdateTuples] 
    ON  [dbo].[SecurityTargets]
    AFTER UPDATE, INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    MERGE INTO Security_Tuple1 AS Target
	USING (SELECT Source.Id1, Source.parentid1, m.[Start] AS SubjectId, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
		FROM Update_Security_Tuple1 Source 
		INNER JOIN (SELECT * FROM (SELECT * from inserted except SELECT * FROM deleted) a) inserted ON inserted.ObjectId1 = Source.ParentId1 AND inserted.Arity = 1
		INNER JOIN SecurityMembershipClosure m ON m.[Start] = Source.SubjectId
		INNER JOIN AclEntries e ON inserted.id = e.TargetId AND m.[End] = e.SubjectId
		INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = inserted.ObjectId1 AND ic1.[Start] = Source.Id1 
		) AS Source (Id1, parentid1, subjectid, privilegeid, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) ON 
		(Target.id1 = Source.id1 AND Target.ParentId1 = Source.ParentId1  AND Target.SubjectId = Source.SubjectId AND Target.PrivilegeId = Source.PrivilegeId AND Target.ViaMembership = Source.ViaMembership AND Target.DeniedEntries = Source.DeniedEntries AND Target.ExplicitDeniedEntries = Source.ExplicitDeniedEntries AND Target.ExplicitAllowedEntries = Source.ExplicitAllowedEntries AND EXISTS(SELECT NULL FROM SecurityTargets t WHERE t.ObjectId1 = Source.ParentId1 AND t.Arity = 1))

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT (Id1, ParentId1, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) VALUES (Source.Id1, Source.ParentId1, Source.SubjectId, Source.PrivilegeId, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries);

	--Security Tuple 2
    MERGE INTO Security_Tuple2 AS Target
	USING (SELECT Source.Id1, Source.id2, Source.parentid1, Source.parentid2, m.[Start] AS subjectid, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
			FROM Update_Security_Tuple2 Source 
			INNER JOIN (SELECT * FROM (SELECT * from inserted except SELECT * FROM deleted) a) inserted ON inserted.ObjectId1 = Source.ParentId1 AND inserted.objectId2 = Source.ParentId2 AND inserted.Arity = 2
			INNER JOIN SecurityMembershipClosure m ON m.[Start] = Source.SubjectId
			INNER JOIN AclEntries e ON inserted.id = e.TargetId AND m.[End] = e.SubjectId
			INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = inserted.ObjectId1 AND ic1.[Start] = Source.Id1 
			INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = inserted.ObjectId2 AND ic2.[Start] = Source.Id2 
		) AS Source (Id1, id2, parentid1, parentid2, subjectid, privilegeid, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) ON 
		(Target.id1 = Source.id1 AND Target.id2 = Source.id2 AND Target.ParentId1 = Source.ParentId1 AND Target.ParentId2 = Source.ParentId2 AND Target.SubjectId = Source.SubjectId AND Target.PrivilegeId = Source.PrivilegeId AND Target.ViaMembership = Source.ViaMembership AND Target.DeniedEntries = Source.DeniedEntries AND Target.ExplicitDeniedEntries = Source.ExplicitDeniedEntries AND Target.ExplicitAllowedEntries = Source.ExplicitAllowedEntries AND EXISTS(SELECT NULL FROM SecurityTargets t WHERE t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2 AND t.Arity = 2))

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT (Id1, Id2, ParentId1, ParentId2, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) VALUES (Source.Id1, Source.Id2, Source.ParentId1, Source.ParentId2, Source.SubjectId, Source.PrivilegeId, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries);

	--Security Tuple 3
    MERGE INTO Security_Tuple3 AS Target
	USING (SELECT Source.Id1, Source.id2, Source.id3, Source.parentid1, Source.parentid2, Source.parentid3, m.[Start] AS subjectid, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
			FROM Update_Security_Tuple3 Source
			INNER JOIN (SELECT * FROM (SELECT * from inserted except SELECT * FROM deleted) a) inserted ON inserted.ObjectId1 = Source.ParentId1 AND inserted.objectId2 = Source.ParentId2 AND inserted.objectid3 = Source.ParentId3 AND inserted.Arity = 3
			INNER JOIN SecurityMembershipClosure m ON m.[Start] = Source.SubjectId
			INNER JOIN AclEntries e ON inserted.id = e.TargetId AND m.[End] = e.SubjectId
			INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = inserted.ObjectId1 AND ic1.[Start] = Source.Id1 
			INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = inserted.ObjectId2 AND ic2.[Start] = Source.Id2 
			INNER JOIN SecurityInheritanceClosure AS ic3 ON ic3.[End] = inserted.ObjectId3 AND ic3.[Start] = Source.Id3 

		) AS Source (Id1, id2, id3, parentid1, parentid2, parentid3, subjectid, privilegeid, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) ON 
		(Target.id1 = Source.id1 AND Target.id2 = Source.id2 AND Target.id3 = Source.id3 AND Target.ParentId1 = Source.ParentId1 AND Target.ParentId2 = Source.ParentId2 AND Target.ParentId3 = Source.ParentId3 AND Target.SubjectId = Source.SubjectId AND Target.PrivilegeId = Source.PrivilegeId AND Target.ViaMembership = Source.ViaMembership AND Target.DeniedEntries = Source.DeniedEntries AND Target.ExplicitDeniedEntries = Source.ExplicitDeniedEntries AND Target.ExplicitAllowedEntries = Source.ExplicitAllowedEntries  AND EXISTS(SELECT NULL FROM SecurityTargets t WHERE t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2 AND t.ObjectId3 = Source.ParentId3 AND t.Arity = 3))

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT (Id1, Id2, Id3, ParentId1, ParentId2, ParentId3, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) VALUES (Source.Id1, Source.Id2, Source.Id3, Source.ParentId1, Source.ParentId2, Source.ParentId3, Source.SubjectId, Source.PrivilegeId, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries);

	--Security Tuple 4
    MERGE INTO Security_Tuple4 AS Target
	USING (SELECT Source.Id1, Source.id2, Source.id3, Source.id4, Source.parentid1, Source.parentid2, Source.parentid3, Source.parentid4, m.[start] AS subjectid, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
			FROM Update_Security_Tuple4 Source 
			INNER JOIN (SELECT * FROM (SELECT * from inserted except SELECT * FROM deleted) a) inserted ON inserted.ObjectId1 = Source.ParentId1 AND inserted.objectId2 = Source.ParentId2 AND inserted.objectid3 = Source.ParentId3 AND inserted.objectid4 = Source.ParentId4 AND inserted.Arity = 4
			INNER JOIN SecurityMembershipClosure m ON m.[Start] = Source.SubjectId
			INNER JOIN AclEntries e ON inserted.id = e.TargetId AND m.[End] = e.SubjectId
			INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = inserted.ObjectId1 AND ic1.[Start] = Source.Id1 
			INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = inserted.ObjectId2 AND ic2.[Start] = Source.Id2 
			INNER JOIN SecurityInheritanceClosure AS ic3 ON ic3.[End] = inserted.ObjectId3 AND ic3.[Start] = Source.Id3 
			INNER JOIN SecurityInheritanceClosure AS ic4 ON ic4.[End] = inserted.ObjectId4 AND ic4.[Start] = Source.Id4 
		) AS Source (Id1, id2, id3, id4, parentid1, parentid2, parentid3, parentid4, subjectid, privilegeid, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) ON 
		(Target.id1 = Source.id1 AND Target.id2 = Source.id2 AND Target.id3 = Source.id3 AND Target.id4 = Source.id4 AND Target.ParentId1 = Source.ParentId1 AND Target.ParentId2 = Source.ParentId2 AND Target.ParentId3 = Source.ParentId3 AND Target.ParentId4 = Source.ParentId4 AND Target.SubjectId = Source.SubjectId AND Target.PrivilegeId = Source.PrivilegeId AND Target.ViaMembership = Source.ViaMembership AND Target.DeniedEntries = Source.DeniedEntries AND Target.ExplicitDeniedEntries = Source.ExplicitDeniedEntries AND Target.ExplicitAllowedEntries = Source.ExplicitAllowedEntries  AND EXISTS(SELECT NULL FROM SecurityTargets t WHERE t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2 AND t.ObjectId3 = Source.ParentId3 AND t.ObjectId4 = Source.ParentId4 AND t.Arity = 4))

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT (Id1, Id2, Id3, Id4, ParentId1, ParentId2, ParentId3, ParentId4, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) VALUES (Source.Id1, Source.Id2, Source.Id3, Source.Id4, Source.ParentId1, Source.ParentId2, Source.ParentId3, Source.ParentId4, Source.SubjectId, Source.PrivilegeId, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries);
END
");

            Sql(@"CREATE TRIGGER [dbo].[SecurityMembershipClosure_DeleteTuples] 
    ON  [dbo].[SecurityMembershipClosure]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE Security_Tuple1 FROM Security_Tuple1 as tuple
	INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple1 st 
				INNER JOIN deleted ON deleted.[Start] = st.SubjectId 
				INNER JOIN AclEntries e ON deleted.[End] = e.SubjectId AND e.PrivilegeId = st.PrivilegeId
				INNER JOIN SecurityTargets AS targets ON e.TargetId = targets.Id AND targets.ObjectId1 = st.ParentId1 AND targets.Arity = 1
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = st.Id1 
				except SELECT ust.* FROM Update_Security_Tuple1 ust
				INNER JOIN deleted ON deleted.[Start] = ust.SubjectId 
				INNER JOIN AclEntries e ON deleted.[End] = e.SubjectId AND e.PrivilegeId = ust.PrivilegeId
				INNER JOIN SecurityTargets AS targets ON e.TargetId = targets.Id AND targets.ObjectId1 = ust.ParentId1 AND targets.Arity = 1
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = ust.Id1 
				) a) AS d
	ON d.Id1 = tuple.Id1 AND d.ParentId1 = tuple.ParentId1 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries


		DELETE Security_Tuple2 FROM Security_Tuple2 as tuple
	INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple2 st 
				INNER JOIN deleted ON deleted.[Start] = st.SubjectId 
				INNER JOIN AclEntries e ON deleted.[End] = e.SubjectId AND e.PrivilegeId = st.PrivilegeId
				INNER JOIN SecurityTargets AS targets ON e.TargetId = targets.Id AND targets.ObjectId1 = st.ParentId1 AND targets.ObjectId2 = st.ParentId2 AND targets.Arity = 2
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = st.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = targets.ObjectId2 AND ic2.[Start] = st.Id2 
				except SELECT ust.* FROM Update_Security_Tuple2 ust
				INNER JOIN deleted ON deleted.[Start] = ust.SubjectId 
				INNER JOIN AclEntries e ON deleted.[End] = e.SubjectId AND e.PrivilegeId = ust.PrivilegeId
				INNER JOIN SecurityTargets AS targets ON e.TargetId = targets.Id AND targets.ObjectId1 = ust.ParentId1 AND targets.ObjectId2 = ust.ParentId2 AND targets.Arity = 2
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = ust.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = targets.ObjectId2 AND ic2.[Start] = ust.Id2 
				) a) AS d
	ON d.Id1 = tuple.Id1 AND d.Id2 = tuple.Id2 AND d.ParentId1 = tuple.ParentId1 AND d.ParentId2 = tuple.ParentId2 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries


		DELETE Security_Tuple3 FROM Security_Tuple3 as tuple
	INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple3 st 
				INNER JOIN deleted ON deleted.[Start] = st.SubjectId 
				INNER JOIN AclEntries e ON deleted.[End] = e.SubjectId AND e.PrivilegeId = st.PrivilegeId
				INNER JOIN SecurityTargets AS targets ON e.TargetId = targets.Id AND targets.ObjectId1 = st.ParentId1 AND targets.ObjectId2 = st.ParentId2 AND targets.ObjectId3 = st.ParentId3 AND targets.Arity = 3
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = st.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = targets.ObjectId2 AND ic2.[Start] = st.Id2 
				INNER JOIN SecurityInheritanceClosure AS ic3 ON ic3.[End] = targets.ObjectId3 AND ic3.[Start] = st.Id3 
				except SELECT ust.* FROM Update_Security_Tuple3 ust
				INNER JOIN deleted ON deleted.[Start] = ust.SubjectId 
				INNER JOIN AclEntries e ON deleted.[End] = e.SubjectId AND e.PrivilegeId = ust.PrivilegeId
				INNER JOIN SecurityTargets AS targets ON e.TargetId = targets.Id AND targets.ObjectId1 = ust.ParentId1 AND targets.ObjectId2 = ust.ParentId2 AND targets.ObjectId3 = ust.ParentId3 AND targets.Arity = 3
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = ust.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = targets.ObjectId2 AND ic2.[Start] = ust.Id2 
				INNER JOIN SecurityInheritanceClosure AS ic3 ON ic3.[End] = targets.ObjectId3 AND ic3.[Start] = ust.Id3 
				) a) AS d
	ON d.Id1 = tuple.Id1 AND d.Id2 = tuple.Id2 AND d.Id3 = tuple.Id3 AND d.ParentId1 = tuple.ParentId1 AND d.ParentId2 = tuple.ParentId2 AND d.ParentId3 = tuple.ParentId3 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries


DELETE Security_Tuple4 FROM Security_Tuple4 as tuple
	INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple4 st 
				INNER JOIN deleted ON deleted.[Start] = st.SubjectId 
				INNER JOIN AclEntries e ON deleted.[End] = e.SubjectId AND e.PrivilegeId = st.PrivilegeId
				INNER JOIN SecurityTargets AS targets ON e.TargetId = targets.Id AND targets.ObjectId1 = st.ParentId1 AND targets.ObjectId2 = st.ParentId2 AND targets.ObjectId3 = st.ParentId3 AND targets.ObjectId4 = st.ParentId4 AND targets.Arity = 4
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = st.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = targets.ObjectId2 AND ic2.[Start] = st.Id2 
				INNER JOIN SecurityInheritanceClosure AS ic3 ON ic3.[End] = targets.ObjectId3 AND ic3.[Start] = st.Id3 
				INNER JOIN SecurityInheritanceClosure AS ic4 ON ic4.[End] = targets.ObjectId4 AND ic4.[Start] = st.Id4 
				except SELECT ust.* FROM Update_Security_Tuple4 ust
				INNER JOIN deleted ON deleted.[Start] = ust.SubjectId 
				INNER JOIN AclEntries e ON deleted.[End] = e.SubjectId AND e.PrivilegeId = ust.PrivilegeId
				INNER JOIN SecurityTargets AS targets ON e.TargetId = targets.Id AND targets.ObjectId1 = ust.ParentId1 AND targets.ObjectId2 = ust.ParentId2 AND targets.ObjectId3 = ust.ParentId3 AND targets.ObjectId4 = ust.ParentId4 AND targets.Arity = 4
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = ust.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = targets.ObjectId2 AND ic2.[Start] = ust.Id2 
				INNER JOIN SecurityInheritanceClosure AS ic3 ON ic3.[End] = targets.ObjectId3 AND ic3.[Start] = ust.Id3 
				INNER JOIN SecurityInheritanceClosure AS ic4 ON ic4.[End] = targets.ObjectId4 AND ic4.[Start] = ust.Id4 
				) a) AS d
	ON d.Id1 = tuple.Id1 AND d.Id2 = tuple.Id2 AND d.Id3 = tuple.Id3 AND d.ID4 = tuple.ID4 AND d.ParentId1 = tuple.ParentId1 AND d.ParentId2 = tuple.ParentId2 AND d.ParentId3 = tuple.ParentId3 AND d.ParentId4 = tuple.ParentId4 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries
END");

            Sql(@"CREATE TRIGGER [dbo].[SecurityMembershipClosure_UpdateTuples] 
    ON  [dbo].[SecurityMembershipClosure]
    AFTER UPDATE, INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    MERGE INTO Security_Tuple1 AS Target
	USING (SELECT Source.Id1, Source.parentid1, inserted.[Start] AS SubjectId, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
		FROM Update_Security_Tuple1 Source 
			INNER JOIN (SELECT * FROM (SELECT * from inserted except SELECT * FROM deleted) a) inserted ON inserted.[End] = Source.SubjectId
			INNER JOIN AclEntries e ON e.SubjectId = inserted.[End] AND e.PrivilegeId = Source.PrivilegeId
			INNER JOIN SecurityTargets t ON e.TargetId = t.Id AND t.Arity = 1 AND t.ObjectId1 = Source.ParentId1
			INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = t.ObjectId1 AND ic1.[Start] = Source.Id1
		) AS Source (Id1, parentid1, subjectid, privilegeid, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) ON 
		(Target.id1 = Source.id1 AND Target.ParentId1 = Source.ParentId1  AND Target.SubjectId = Source.SubjectId AND Target.PrivilegeId = Source.PrivilegeId AND Target.ViaMembership = Source.ViaMembership AND Target.DeniedEntries = Source.DeniedEntries AND Target.ExplicitDeniedEntries = Source.ExplicitDeniedEntries AND Target.ExplicitAllowedEntries = Source.ExplicitAllowedEntries AND EXISTS(SELECT NULL FROM SecurityTargets t WHERE t.ObjectId1 = Source.ParentId1 AND t.Arity = 1))

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT (Id1, ParentId1, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) VALUES (Source.Id1, Source.ParentId1, Source.SubjectId, Source.PrivilegeId, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries);

	--Security Tuple 2
    MERGE INTO Security_Tuple2 AS Target
	USING (SELECT Source.Id1, Source.id2, Source.parentid1, Source.parentid2, Source.SubjectId AS subjectid, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
			FROM Update_Security_Tuple2 Source 
			INNER JOIN (SELECT * FROM (SELECT * from inserted except SELECT * FROM deleted) a) inserted ON inserted.[End] = Source.SubjectId
			INNER JOIN AclEntries e ON e.SubjectId = inserted.[End] AND e.PrivilegeId = Source.PrivilegeId
			INNER JOIN SecurityTargets t ON e.TargetId = t.Id AND t.Arity = 2 AND t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2
			INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = t.ObjectId1 AND ic1.[Start] = Source.Id1
			INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = t.ObjectId2 AND ic2.[Start] = Source.Id2 
		) AS Source (Id1, id2, parentid1, parentid2, subjectid, privilegeid, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) ON 
		(Target.id1 = Source.id1 AND Target.id2 = Source.id2 AND Target.ParentId1 = Source.ParentId1 AND Target.ParentId2 = Source.ParentId2 AND Target.SubjectId = Source.SubjectId AND Target.PrivilegeId = Source.PrivilegeId AND Target.ViaMembership = Source.ViaMembership AND Target.DeniedEntries = Source.DeniedEntries AND Target.ExplicitDeniedEntries = Source.ExplicitDeniedEntries AND Target.ExplicitAllowedEntries = Source.ExplicitAllowedEntries AND EXISTS(SELECT NULL FROM SecurityTargets t WHERE t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2 AND t.Arity = 2))

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT (Id1, Id2, ParentId1, ParentId2, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) VALUES (Source.Id1, Source.Id2, Source.ParentId1, Source.ParentId2, Source.SubjectId, Source.PrivilegeId, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries);

	--Security Tuple 3
    MERGE INTO Security_Tuple3 AS Target
	USING (SELECT Source.Id1, Source.id2, Source.id3, Source.parentid1, Source.parentid2, Source.parentid3, Source.SubjectId, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
			FROM Update_Security_Tuple3 Source
			INNER JOIN (SELECT * FROM (SELECT * from inserted except SELECT * FROM deleted) a) inserted ON inserted.[End] = Source.SubjectId
			INNER JOIN AclEntries e ON e.SubjectId = inserted.[End] AND e.PrivilegeId = Source.PrivilegeId
			INNER JOIN SecurityTargets t ON e.TargetId = t.Id AND t.Arity = 3 AND t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2 AND t.ObjectId3 = Source.ParentId3
			INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = t.ObjectId1 AND ic1.[Start] = Source.Id1 
			INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = t.ObjectId2 AND ic2.[Start] = Source.Id2 
			INNER JOIN SecurityInheritanceClosure AS ic3 ON ic3.[End] = t.ObjectId3 AND ic3.[Start] = Source.Id3
		) AS Source (Id1, id2, id3, parentid1, parentid2, parentid3, subjectid, privilegeid, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) ON 
		(Target.id1 = Source.id1 AND Target.id2 = Source.id2 AND Target.id3 = Source.id3 AND Target.ParentId1 = Source.ParentId1 AND Target.ParentId2 = Source.ParentId2 AND Target.ParentId3 = Source.ParentId3 AND Target.SubjectId = Source.SubjectId AND Target.PrivilegeId = Source.PrivilegeId AND Target.ViaMembership = Source.ViaMembership AND Target.DeniedEntries = Source.DeniedEntries AND Target.ExplicitDeniedEntries = Source.ExplicitDeniedEntries AND Target.ExplicitAllowedEntries = Source.ExplicitAllowedEntries  AND EXISTS(SELECT NULL FROM SecurityTargets t WHERE t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2 AND t.ObjectId3 = Source.ParentId3 AND t.Arity = 3))

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT (Id1, Id2, Id3, ParentId1, ParentId2, ParentId3, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) VALUES (Source.Id1, Source.Id2, Source.Id3, Source.ParentId1, Source.ParentId2, Source.ParentId3, Source.SubjectId, Source.PrivilegeId, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries);

	--Security Tuple 4
    MERGE INTO Security_Tuple4 AS Target
	USING (SELECT Source.Id1, Source.id2, Source.id3, Source.id4, Source.parentid1, Source.parentid2, Source.parentid3, Source.parentid4, Source.SubjectId, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
			FROM Update_Security_Tuple4 Source 
			INNER JOIN (SELECT * FROM (SELECT * from inserted except SELECT * FROM deleted) a) inserted ON inserted.[End] = Source.SubjectId
			INNER JOIN AclEntries e ON e.SubjectId = inserted.[End] AND e.PrivilegeId = Source.PrivilegeId
			INNER JOIN SecurityTargets t ON e.TargetId = t.Id AND t.Arity = 4 AND t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2 AND t.ObjectId3 = Source.ParentId3 AND t.ObjectId4 = Source.ParentId4
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

            Sql(@"CREATE TRIGGER [dbo].[SecurityInheritanceClosure_DeleteTuples] 
    ON  [dbo].[SecurityInheritanceClosure]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE Security_Tuple1 FROM Security_Tuple1 as tuple INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple1 st 				
				INNER JOIN SecurityTargets AS targets ON targets.ObjectId1 = st.ParentId1 AND targets.Arity = 1
				INNER JOIN deleted ON deleted.[Start] = st.Id1 AND targets.ObjectId1 = deleted.[End]
				INNER JOIN SecurityMembershipClosure AS m ON m.[Start] = st.SubjectId
				INNER JOIN AclEntries e ON e.PrivilegeId = st.PrivilegeId AND e.SubjectId = m.[End] AND e.TargetId = targets.id
				except SELECT ust.* FROM Update_Security_Tuple1 ust
				INNER JOIN SecurityTargets AS targets ON targets.ObjectId1 = ust.ParentId1 AND targets.Arity = 1
				INNER JOIN deleted ON deleted.[Start] = ust.Id1 AND targets.ObjectId1 = deleted.[End]
				INNER JOIN SecurityMembershipClosure AS m ON m.[Start] = ust.SubjectId
				INNER JOIN AclEntries e ON e.PrivilegeId = ust.PrivilegeId AND e.SubjectId = m.[End] AND e.TargetId = targets.id
				) a) AS d
	ON d.Id1 = tuple.Id1 AND d.ParentId1 = tuple.ParentId1 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries


		DELETE Security_Tuple2 FROM Security_Tuple2 as tuple INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple2 st 
				INNER JOIN SecurityTargets AS targets ON targets.ObjectId1 = st.ParentId1 AND targets.Arity = 1
				INNER JOIN deleted ON (deleted.[Start] = st.Id1 AND targets.ObjectId1 = deleted.[End]) OR (deleted.[Start] = st.Id2 AND targets.ObjectId2 = deleted.[End])
				INNER JOIN SecurityMembershipClosure AS m ON m.[Start] = st.SubjectId
				INNER JOIN AclEntries e ON e.PrivilegeId = st.PrivilegeId AND e.SubjectId = m.[End] AND e.TargetId = targets.id
				except SELECT ust.* FROM Update_Security_Tuple2 ust
				INNER JOIN SecurityTargets AS targets ON targets.ObjectId1 = ust.ParentId1 AND targets.Arity = 1
				INNER JOIN deleted ON (deleted.[Start] = ust.Id1 AND targets.ObjectId1 = deleted.[End]) OR (deleted.[Start] = ust.Id2 AND targets.ObjectId2 = deleted.[End])
				INNER JOIN SecurityMembershipClosure AS m ON m.[Start] = ust.SubjectId
				INNER JOIN AclEntries e ON e.PrivilegeId = ust.PrivilegeId AND e.SubjectId = m.[End] AND e.TargetId = targets.id
				) a) AS d
	ON d.Id1 = tuple.Id1 AND d.Id2 = tuple.Id2 AND d.ParentId1 = tuple.ParentId1 AND d.ParentId2 = tuple.ParentId2 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries

		DELETE Security_Tuple3 FROM Security_Tuple3 as tuple INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple3 st 
			INNER JOIN SecurityTargets AS targets ON targets.ObjectId1 = st.ParentId1 AND targets.Arity = 1
			INNER JOIN deleted ON (deleted.[Start] = st.Id1 AND targets.ObjectId1 = deleted.[End]) OR (deleted.[Start] = st.Id2 AND targets.ObjectId2 = deleted.[End]) OR (deleted.[Start] = st.id3 AND targets.objectid3 = deleted.[End])
			INNER JOIN SecurityMembershipClosure AS m ON m.[Start] = st.SubjectId
			INNER JOIN AclEntries e ON e.PrivilegeId = st.PrivilegeId AND e.SubjectId = m.[End] AND e.TargetId = targets.id
			except SELECT ust.* FROM Update_Security_Tuple3 ust
			INNER JOIN SecurityTargets AS targets ON targets.ObjectId1 = ust.ParentId1 AND targets.Arity = 1
			INNER JOIN deleted ON (deleted.[Start] = ust.Id1 AND targets.ObjectId1 = deleted.[End]) OR (deleted.[Start] = ust.Id2 AND targets.ObjectId2 = deleted.[End]) OR (deleted.[Start] = ust.id3 AND targets.objectid3 = deleted.[End])
			INNER JOIN SecurityMembershipClosure AS m ON m.[Start] = ust.SubjectId
			INNER JOIN AclEntries e ON e.PrivilegeId = ust.PrivilegeId AND e.SubjectId = m.[End] AND e.TargetId = targets.id
			) a) AS d
ON d.Id1 = tuple.Id1 AND d.Id2 = tuple.Id2 AND d.Id3 = tuple.Id3 AND d.ParentId1 = tuple.ParentId1 AND d.ParentId2 = tuple.ParentId2 AND d.ParentId3 = tuple.ParentId3 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries



		DELETE Security_Tuple4 FROM Security_Tuple4 as tuple INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple4 st 
				INNER JOIN SecurityTargets AS targets ON targets.ObjectId1 = st.ParentId1 AND targets.Arity = 1
				INNER JOIN deleted ON (deleted.[Start] = st.Id1 AND targets.ObjectId1 = deleted.[End]) OR (deleted.[Start] = st.Id2 AND targets.ObjectId2 = deleted.[End]) OR (deleted.[Start] = st.id3 AND targets.objectid3 = deleted.[End]) OR (deleted.[Start] = st.id4 AND targets.ObjectId4 = deleted.[End])
				INNER JOIN SecurityMembershipClosure AS m ON m.[Start] = st.SubjectId
				INNER JOIN AclEntries e ON e.PrivilegeId = st.PrivilegeId AND e.SubjectId = m.[End] AND e.TargetId = targets.id
				except SELECT ust.* FROM Update_Security_Tuple4 ust
				INNER JOIN SecurityTargets AS targets ON targets.ObjectId1 = ust.ParentId1 AND targets.Arity = 1
				INNER JOIN deleted ON (deleted.[Start] = ust.Id1 AND targets.ObjectId1 = deleted.[End]) OR (deleted.[Start] = ust.Id2 AND targets.ObjectId2 = deleted.[End]) OR (deleted.[Start] = ust.id3 AND targets.objectid3 = deleted.[End]) OR (deleted.[Start] = ust.id4 AND targets.ObjectId4 = deleted.[End])
				INNER JOIN SecurityMembershipClosure AS m ON m.[Start] = ust.SubjectId
				INNER JOIN AclEntries e ON e.PrivilegeId = ust.PrivilegeId AND e.SubjectId = m.[End] AND e.TargetId = targets.id
				) a) AS d
	ON d.Id1 = tuple.Id1 AND d.Id2 = tuple.Id2 AND d.Id3 = tuple.Id3 AND d.ID4 = tuple.ID4 AND d.ParentId1 = tuple.ParentId1 AND d.ParentId2 = tuple.ParentId2 AND d.ParentId3 = tuple.ParentId3 AND d.ParentId4 = tuple.ParentId4 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries
END");

            Sql(@"CREATE TRIGGER [dbo].[SecurityInheritanceClosure_UpdateTuples] 
    ON  [dbo].[SecurityInheritanceClosure]
    AFTER UPDATE, INSERT
AS 
BEGIN
	SET NOCOUNT ON;

    MERGE INTO Security_Tuple1 AS Target
	USING (SELECT Source.Id1, Source.parentid1, m.[Start] AS SubjectId, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
		FROM Update_Security_Tuple1 Source 
			INNER JOIN SecurityTargets t ON t.Arity = 1 AND t.ObjectId1 = Source.ParentId1
			INNER JOIN AclEntries e ON e.TargetId = t.id AND e.PrivilegeId = Source.PrivilegeId
			INNER JOIN SecurityMembershipClosure m ON m.[End] = e.SubjectId AND m.[Start] = Source.SubjectId
			INNER JOIN (SELECT * FROM (SELECT * from inserted except SELECT * FROM deleted) a) inserted ON inserted.[End] = t.ObjectId1 AND inserted.[Start] = Source.Id1
		) AS Source (Id1, parentid1, subjectid, privilegeid, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) ON 
		(Target.id1 = Source.id1 AND Target.ParentId1 = Source.ParentId1  AND Target.SubjectId = Source.SubjectId AND Target.PrivilegeId = Source.PrivilegeId AND Target.ViaMembership = Source.ViaMembership AND Target.DeniedEntries = Source.DeniedEntries AND Target.ExplicitDeniedEntries = Source.ExplicitDeniedEntries AND Target.ExplicitAllowedEntries = Source.ExplicitAllowedEntries AND EXISTS(SELECT NULL FROM SecurityTargets t WHERE t.ObjectId1 = Source.ParentId1 AND t.Arity = 1))

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT (Id1, ParentId1, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) VALUES (Source.Id1, Source.ParentId1, Source.SubjectId, Source.PrivilegeId, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries);

	--Security Tuple 2
    MERGE INTO Security_Tuple2 AS Target
	USING (SELECT Source.Id1, Source.id2, Source.parentid1, Source.parentid2, m.[Start] AS subjectid, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
			FROM Update_Security_Tuple2 Source 
			INNER JOIN SecurityTargets t ON t.Arity = 1 AND t.ObjectId1 = Source.ParentId1
			INNER JOIN AclEntries e ON e.TargetId = t.id AND e.PrivilegeId = Source.PrivilegeId
			INNER JOIN SecurityMembershipClosure m ON m.[End] = e.SubjectId AND m.[Start] = Source.SubjectId
			INNER JOIN (SELECT * FROM (SELECT * from inserted except SELECT * FROM deleted) a) inserted ON (inserted.[End] = t.ObjectId1 AND inserted.[Start] = Source.Id1) OR (inserted.[End] = t.ObjectId2 AND inserted.[Start] = Source.Id2)
		) AS Source (Id1, id2, parentid1, parentid2, subjectid, privilegeid, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) ON 
		(Target.id1 = Source.id1 AND Target.id2 = Source.id2 AND Target.ParentId1 = Source.ParentId1 AND Target.ParentId2 = Source.ParentId2 AND Target.SubjectId = Source.SubjectId AND Target.PrivilegeId = Source.PrivilegeId AND Target.ViaMembership = Source.ViaMembership AND Target.DeniedEntries = Source.DeniedEntries AND Target.ExplicitDeniedEntries = Source.ExplicitDeniedEntries AND Target.ExplicitAllowedEntries = Source.ExplicitAllowedEntries AND EXISTS(SELECT NULL FROM SecurityTargets t WHERE t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2 AND t.Arity = 2))

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT (Id1, Id2, ParentId1, ParentId2, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) VALUES (Source.Id1, Source.Id2, Source.ParentId1, Source.ParentId2, Source.SubjectId, Source.PrivilegeId, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries);

	--Security Tuple 3
    MERGE INTO Security_Tuple3 AS Target
	USING (SELECT Source.Id1, Source.id2, Source.id3, Source.parentid1, Source.parentid2, Source.parentid3, m.[Start] AS subjectid, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
			FROM Update_Security_Tuple3 Source
			INNER JOIN SecurityTargets t ON t.Arity = 1 AND t.ObjectId1 = Source.ParentId1
			INNER JOIN AclEntries e ON e.TargetId = t.id AND e.PrivilegeId = Source.PrivilegeId
			INNER JOIN SecurityMembershipClosure m ON m.[End] = e.SubjectId AND m.[Start] = Source.SubjectId
			INNER JOIN (SELECT * FROM (SELECT * from inserted except SELECT * FROM deleted) a) inserted ON (inserted.[End] = t.ObjectId1 AND inserted.[Start] = Source.Id1) OR (inserted.[End] = t.ObjectId2 AND inserted.[Start] = Source.Id2) OR (inserted.[End] = t.ObjectId3 AND inserted.[Start] = Source.Id3)
		) AS Source (Id1, id2, id3, parentid1, parentid2, parentid3, subjectid, privilegeid, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) ON 
		(Target.id1 = Source.id1 AND Target.id2 = Source.id2 AND Target.id3 = Source.id3 AND Target.ParentId1 = Source.ParentId1 AND Target.ParentId2 = Source.ParentId2 AND Target.ParentId3 = Source.ParentId3 AND Target.SubjectId = Source.SubjectId AND Target.PrivilegeId = Source.PrivilegeId AND Target.ViaMembership = Source.ViaMembership AND Target.DeniedEntries = Source.DeniedEntries AND Target.ExplicitDeniedEntries = Source.ExplicitDeniedEntries AND Target.ExplicitAllowedEntries = Source.ExplicitAllowedEntries  AND EXISTS(SELECT NULL FROM SecurityTargets t WHERE t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2 AND t.ObjectId3 = Source.ParentId3 AND t.Arity = 3))

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT (Id1, Id2, Id3, ParentId1, ParentId2, ParentId3, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) VALUES (Source.Id1, Source.Id2, Source.Id3, Source.ParentId1, Source.ParentId2, Source.ParentId3, Source.SubjectId, Source.PrivilegeId, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries);

	--Security Tuple 4
    MERGE INTO Security_Tuple4 AS Target
	USING (SELECT Source.Id1, Source.id2, Source.id3, Source.id4, Source.parentid1, Source.parentid2, Source.parentid3, Source.parentid4, m.[start] AS subjectid, Source.privilegeid, Source.ViaMembership AS ViaMembership, Source.DeniedEntries AS DeniedEntries, Source.ExplicitDeniedEntries AS ExplicitDeniedEntries, Source.ExplicitAllowedEntries AS ExplicitAllowedEntries 
			FROM Update_Security_Tuple4 Source 
			INNER JOIN SecurityTargets t ON t.Arity = 1 AND t.ObjectId1 = Source.ParentId1
			INNER JOIN AclEntries e ON e.TargetId = t.id AND e.PrivilegeId = Source.PrivilegeId
			INNER JOIN SecurityMembershipClosure m ON m.[End] = e.SubjectId AND m.[Start] = Source.SubjectId
			INNER JOIN (SELECT * FROM (SELECT * from inserted except SELECT * FROM deleted) a) inserted ON (inserted.[End] = t.ObjectId1 AND inserted.[Start] = Source.Id1) OR (inserted.[End] = t.ObjectId2 AND inserted.[Start] = Source.Id2) OR (inserted.[End] = t.ObjectId3 AND inserted.[Start] = Source.Id3) OR (inserted.[End] = t.ObjectId4 AND inserted.[Start] = Source.Id4)
		) AS Source (Id1, id2, id3, id4, parentid1, parentid2, parentid3, parentid4, subjectid, privilegeid, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) ON 
		(Target.id1 = Source.id1 AND Target.id2 = Source.id2 AND Target.id3 = Source.id3 AND Target.id4 = Source.id4 AND Target.ParentId1 = Source.ParentId1 AND Target.ParentId2 = Source.ParentId2 AND Target.ParentId3 = Source.ParentId3 AND Target.ParentId4 = Source.ParentId4 AND Target.SubjectId = Source.SubjectId AND Target.PrivilegeId = Source.PrivilegeId AND Target.ViaMembership = Source.ViaMembership AND Target.DeniedEntries = Source.DeniedEntries AND Target.ExplicitDeniedEntries = Source.ExplicitDeniedEntries AND Target.ExplicitAllowedEntries = Source.ExplicitAllowedEntries  AND EXISTS(SELECT NULL FROM SecurityTargets t WHERE t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2 AND t.ObjectId3 = Source.ParentId3 AND t.ObjectId4 = Source.ParentId4 AND t.Arity = 4))

	WHEN NOT MATCHED BY TARGET THEN 
		INSERT (Id1, Id2, Id3, Id4, ParentId1, ParentId2, ParentId3, ParentId4, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) VALUES (Source.Id1, Source.Id2, Source.Id3, Source.Id4, Source.ParentId1, Source.ParentId2, Source.ParentId3, Source.ParentId4, Source.SubjectId, Source.PrivilegeId, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries);
END
");
        }
        
        public override void Down()
        {
            Sql(@"  DECLARE @sql nvarchar(max)
                    SET @sql = 'ALTER DATABASE [' + db_name() + '] SET RECURSIVE_TRIGGERS OFF'
                    EXECUTE sp_executesql @sql", true);
        }
    }
}
