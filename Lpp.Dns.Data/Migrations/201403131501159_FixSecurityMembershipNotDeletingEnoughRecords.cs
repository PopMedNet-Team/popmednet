namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSecurityMembershipNotDeletingEnoughRecords : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TRIGGER [dbo].[SecurityMembershipClosure_DeleteTuples] 
    ON  [dbo].[SecurityMembershipClosure]
    AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	DELETE Security_Tuple1 FROM Security_Tuple1 as tuple
	INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple1 st 
				INNER JOIN deleted ON deleted.[Start] = st.SubjectId 
				INNER JOIN AclEntries e ON deleted.[End] = e.SubjectId AND e.PrivilegeId = st.PrivilegeId
				except SELECT ust.* FROM Update_Security_Tuple1 ust
				INNER JOIN deleted ON deleted.[Start] = ust.SubjectId 
				INNER JOIN AclEntries e ON deleted.[End] = e.SubjectId AND e.PrivilegeId = ust.PrivilegeId
				) a) AS d
	ON d.Id1 = tuple.Id1 AND d.ParentId1 = tuple.ParentId1 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries


		DELETE Security_Tuple2 FROM Security_Tuple2 as tuple
	INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple2 st 
				INNER JOIN deleted ON deleted.[Start] = st.SubjectId 
				INNER JOIN AclEntries e ON deleted.[End] = e.SubjectId AND e.PrivilegeId = st.PrivilegeId
				except SELECT ust.* FROM Update_Security_Tuple2 ust
				INNER JOIN deleted ON deleted.[Start] = ust.SubjectId 
				INNER JOIN AclEntries e ON deleted.[End] = e.SubjectId AND e.PrivilegeId = ust.PrivilegeId
				) a) AS d
	ON d.Id1 = tuple.Id1 AND d.Id2 = tuple.Id2 AND d.ParentId1 = tuple.ParentId1 AND d.ParentId2 = tuple.ParentId2 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries


		DELETE Security_Tuple3 FROM Security_Tuple3 as tuple
	INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple3 st 
				INNER JOIN deleted ON deleted.[Start] = st.SubjectId 
				INNER JOIN AclEntries e ON deleted.[End] = e.SubjectId AND e.PrivilegeId = st.PrivilegeId
				except SELECT ust.* FROM Update_Security_Tuple3 ust
				INNER JOIN deleted ON deleted.[Start] = ust.SubjectId 
				INNER JOIN AclEntries e ON deleted.[End] = e.SubjectId AND e.PrivilegeId = ust.PrivilegeId
				) a) AS d
	ON d.Id1 = tuple.Id1 AND d.Id2 = tuple.Id2 AND d.Id3 = tuple.Id3 AND d.ParentId1 = tuple.ParentId1 AND d.ParentId2 = tuple.ParentId2 AND d.ParentId3 = tuple.ParentId3 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries


DELETE Security_Tuple4 FROM Security_Tuple4 as tuple
	INNER JOIN (SELECT * FROM (SELECT st.* from Security_Tuple4 st 
				INNER JOIN deleted ON deleted.[Start] = st.SubjectId 
				INNER JOIN AclEntries e ON deleted.[End] = e.SubjectId AND e.PrivilegeId = st.PrivilegeId
				except SELECT ust.* FROM Update_Security_Tuple4 ust
				INNER JOIN deleted ON deleted.[Start] = ust.SubjectId 
				INNER JOIN AclEntries e ON deleted.[End] = e.SubjectId AND e.PrivilegeId = ust.PrivilegeId
				) a) AS d
	ON d.Id1 = tuple.Id1 AND d.Id2 = tuple.Id2 AND d.Id3 = tuple.Id3 AND d.ID4 = tuple.ID4 AND d.ParentId1 = tuple.ParentId1 AND d.ParentId2 = tuple.ParentId2 AND d.ParentId3 = tuple.ParentId3 AND d.ParentId4 = tuple.ParentId4 AND d.PrivilegeId = tuple.PrivilegeId AND d.SubjectId = tuple.SubjectId AND d.ViaMembership = tuple.ViaMembership AND d.DeniedEntries = tuple.ViaMembership AND d.ExplicitAllowedEntries = tuple.ExplicitAllowedEntries AND d.ExplicitDeniedEntries = tuple.ExplicitDeniedEntries
END");
        }
        
        public override void Down()
        {
        }
    }
}
