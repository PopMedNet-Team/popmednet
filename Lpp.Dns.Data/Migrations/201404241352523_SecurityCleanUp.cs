namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SecurityCleanUp : DbMigration
    {
        public override void Up()
        {
            Sql(@"
	TRUNCATE TABLE Security_Tuple1
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
");
        }
        
        public override void Down()
        {
        }
    }
}
