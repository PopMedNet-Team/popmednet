namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSecurityIssueOnAclDeletion : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TRIGGER [dbo].[ACLEntries_DeleteTuples] 
    ON  [dbo].[AclEntries]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE st FROM Security_Tuple1 st 
				INNER JOIN deleted ON st.privilegeId = deleted.privilegeId 
				INNER JOIN SecurityTargets AS targets ON deleted.TargetId = targets.Id AND targets.ObjectId1 = st.ParentId1 AND targets.Arity = 1
				INNER JOIN SecurityMembershipClosure AS m ON m.[End] = deleted.subjectid AND m.[Start] = st.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = st.Id1 


		DELETE st FROM Security_Tuple2 st 
				INNER JOIN deleted ON st.privilegeId = deleted.privilegeId 
				INNER JOIN SecurityTargets AS targets ON deleted.TargetId = targets.Id AND targets.ObjectId1 = st.ParentId1 AND targets.ObjectId2 = st.ParentId2 AND targets.Arity = 2
				INNER JOIN SecurityMembershipClosure AS m ON m.[End] = deleted.subjectid AND m.[Start] = st.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = st.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = targets.ObjectId2 AND ic2.[Start] = st.Id2 

		DELETE st FROM Security_Tuple3 as st
				INNER JOIN deleted ON st.privilegeId = deleted.privilegeId 
				INNER JOIN SecurityTargets AS targets ON deleted.TargetId = targets.Id AND targets.ObjectId1 = st.ParentId1 AND targets.ObjectId2 = st.ParentId2 AND targets.ObjectId3 = st.ParentId3 AND targets.Arity = 3
				INNER JOIN SecurityMembershipClosure AS m ON m.[End] = deleted.subjectid AND m.[Start] = st.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = st.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = targets.ObjectId2 AND ic2.[Start] = st.Id2 
				INNER JOIN SecurityInheritanceClosure AS ic3 ON ic3.[End] = targets.ObjectId3 AND ic3.[Start] = st.Id3		


		DELETE st FROM Security_Tuple4 as st 
				INNER JOIN deleted ON st.privilegeId = deleted.privilegeId 
				INNER JOIN SecurityTargets AS targets ON deleted.TargetId = targets.Id AND targets.ObjectId1 = st.ParentId1 AND targets.ObjectId2 = st.ParentId2 AND targets.ObjectId3 = st.ParentId3 AND targets.ObjectId4 = st.ParentId4 AND targets.Arity = 4
				INNER JOIN SecurityMembershipClosure AS m ON m.[End] = deleted.subjectid AND m.[Start] = st.SubjectId
				INNER JOIN SecurityInheritanceClosure AS ic1 ON ic1.[End] = targets.ObjectId1 AND ic1.[Start] = st.Id1 
				INNER JOIN SecurityInheritanceClosure AS ic2 ON ic2.[End] = targets.ObjectId2 AND ic2.[Start] = st.Id2 
				INNER JOIN SecurityInheritanceClosure AS ic3 ON ic3.[End] = targets.ObjectId3 AND ic3.[Start] = st.Id3
				INNER JOIN SecurityInheritanceClosure AS ic4 ON ic4.[End] = targets.ObjectId4 AND ic4.[Start] = st.Id4
END");
        }
        
        public override void Down()
        {
        }
    }
}
