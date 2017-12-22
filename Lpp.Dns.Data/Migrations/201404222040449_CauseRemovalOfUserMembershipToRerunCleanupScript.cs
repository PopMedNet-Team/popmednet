namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CauseRemovalOfUserMembershipToRerunCleanupScript : DbMigration
    {
        public override void Up()
        {
            Sql(@"
ALTER trigger [dbo].[SecurityMembership_Delete] on [dbo].[SecurityMembership] after delete as
	declare @alternativeRoutes table( [Start] uniqueidentifier not null, [End] uniqueidentifier not null, [OriginalDistance] int not null, [AlternativeDistance] int null )

	INSERT INTO @alternativeRoutes([Start], [End], OriginalDistance, [AlternativeDistance]) SELECT [Start], [End], 1, 1 FROM deleted

	-- Simply delete empty loops
	delete from SecurityMembershipClosure
	where [Start] = [End] and [Start] in (select [Start] from deleted where [Start] = [End])

	-- For all non-loop edges, find alternative routes
	
	insert into @alternativeRoutes( [Start], [End], [OriginalDistance], [AlternativeDistance] )
	select d.[Start], d.[End], 1, max( one.[Distance] + two.[Distance] )
	from deleted d
	left join SecurityMembershipClosure one on d.[Start] = one.[Start] and d.[Start] <> d.[End]
	left join SecurityMembershipClosure two on d.[End] = two.[End] and one.[End] = two.[Start]
	and one.[Start] <> one.[End] and two.[Start] <> two.[End]
	group by d.[Start], d.[End]

	delete c
	from @alternativeRoutes a
	-- This trick is here in order to consider four possible paths: bf+a+af, a+af, bf+a, a
	-- for all possible combinations of ""bf"" and ""af"".
	-- Simply doing ""left join"" will not give us the latter three options unless there are no possible values for ""bf"" or ""af"".
	-- Therefore, we artificially fabricate a case when there are no possible values for ""bf"" by including value 1 for bb.x
	-- and a case with no values for ""af"" by including value 1 for aa.x
	cross apply ( select 0 as x union select 1 ) bb   
	cross apply ( select 0 as x union select 1 ) aa   
	left join SecurityMembershipClosure bf on bf.[End] = a.[Start] and bb.x = 0
	left join SecurityMembershipClosure af on af.[Start] = a.[End] and aa.x = 0
	inner join SecurityMembershipClosure c 
		on c.[Start] = isnull( bf.[Start], a.[Start] ) and c.[End] = isnull( af.[End], a.[End] ) 
		and c.Distance = a.OriginalDistance + isnull( bf.Distance, 0 ) + isnull( af.Distance, 0 )
		and a.AlternativeDistance is null

	update c set [Distance] = c.[Distance] + a.[AlternativeDistance] - 1
	from @alternativeRoutes a
	cross apply ( select 0 as x union select 1 ) bb
	cross apply ( select 0 as x union select 1 ) aa
	left join SecurityMembershipClosure bf on bf.[End] = a.[Start] and bb.x = 0
	left join SecurityMembershipClosure af on af.[Start] = a.[End] and aa.x = 0
	inner join SecurityMembershipClosure c 
		on c.[Start] = isnull( bf.[Start], a.[Start] ) and c.[End] = isnull( af.[End], a.[End] ) 
		and c.Distance = a.OriginalDistance + isnull( bf.Distance, 0 ) + isnull( af.Distance, 0 )
		and a.AlternativeDistance is not null

	--This is a special case. In this case and only this case, rebuild all of the security tuples
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


	--Security Tuple 5 Initial Inject
	--INSERT INTO Security_Tuple5 (ID1, Id2, Id3, id4, id5, ParentID1, ParentID2, ParentId3, parentid4, parentid5, SubjectId, PrivilegeId, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) 
	--SELECT DISTINCT ID1, ID2, Id3, id4, id5, ParentID1, ParentID2, ParentId3, parentid4, parentid5, SubjectId, PrivilegeId, ViaMembership, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries FROM Update_Security_Tuple5 ut			
");
        }
        
        public override void Down()
        {
        }
    }
}
