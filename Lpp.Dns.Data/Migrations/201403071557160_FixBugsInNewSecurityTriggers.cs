namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixBugsInNewSecurityTriggers : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER trigger [dbo].[SecurityMembership_Insert] on [dbo].[SecurityMembership] after insert as
	if exists(select * from SecurityMembershipClosure e 
              inner join inserted i 
              on e.[Start] = i.[End] and e.[End] = i.[Start] and i.[Start] <> i.[End])
    begin
		raiserror('Attempt to create a cycle', 16, 1)
		rollback tran
		return
    end

	declare c cursor local forward_only read_only for select [Start], [End] from inserted
	open c
	declare @s uniqueidentifier, @e uniqueidentifier
	fetch next from c into @s, @e

	while @@fetch_status = 0 begin
		declare @incomingClosures table( [Start] uniqueidentifier not null, [End] uniqueidentifier not null, [Distance] int not null )
	
		insert into @incomingClosures([Start],[End],[Distance]) values( @s, @e, case when @s = @e then 0 else 1 end )

		if @s <> @e begin
			insert into @incomingClosures([Start],[End],[Distance])
			select bf.[Start], af.[End], bf.[Distance] + af.[Distance] + 1
			from SecurityMembershipClosure bf
			inner join SecurityMembershipClosure af 
			on @s = bf.[End] and @e = af.[Start] and bf.[Start] <> bf.[End] and af.[Start] <> af.[End]
    
			insert into @incomingClosures([Start],[End],[Distance])
			select @s, af.[End], af.[Distance] + 1
			from SecurityMembershipClosure af where @e = af.[Start] and af.[Start] <> af.[End]

			insert into @incomingClosures([Start],[End],[Distance])
			select bf.[Start], @e, bf.[Distance] + 1
			from SecurityMembershipClosure bf where @s = bf.[End] and bf.[Start] <> bf.[End]
		end

		update SecurityMembershipClosure set [Distance] = i.[Distance]
		from SecurityMembershipClosure c
		inner join @incomingClosures i on c.[Start] = i.[Start] and c.[End] = i.[End] and c.[Distance] > i.[Distance]

		insert into SecurityMembershipClosure([Start], [End], [Distance])
		select i.[Start], i.[End], i.[Distance] from @incomingClosures i
		left join SecurityMembershipClosure ex on ex.[Start] = i.[Start] and ex.[End] = i.[End]
		where ex.[Start] is null

		--Now update the security -tuples here.
		INSERT INTO Security_Tuple1 (Id1, ParentId1, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) SELECT DISTINCT Source.Id1, Source.parentid1, Source.SubjectId, Source.privilegeid, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries
		FROM Update_Security_Tuple1 Source 
			INNER JOIN SecurityTargets t ON t.ObjectId1 = Source.ParentId1 AND t.Arity = 1
			INNER JOIN AclEntries e ON Source.PrivilegeId = e.PrivilegeId
			INNER JOIN @incomingClosures i ON i.[End] = e.SubjectId AND i.[Start] = Source.SubjectId
		WHERE NOT EXISTS(SELECT NULL FROM Security_Tuple1 st WHERE st.id1 = Source.Id1 AND st.ParentID1 = Source.ParentId1 AND st.PrivilegeID = Source.PrivilegeId AND st.SubjectID = Source.SubjectId AND st.ViaMembership = Source.ViaMembership AND st.DeniedEntries = Source.DeniedEntries AND st.ExplicitAllowedEntries = source.ExplicitAllowedEntries AND st.ExplicitDeniedEntries = source.ExplicitDeniedEntries)

		INSERT INTO Security_Tuple2 (Id1, Id2, ParentId1, ParentId2, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) SELECT DISTINCT Source.Id1, Source.Id2, Source.parentid1, Source.ParentId2, Source.SubjectId, Source.privilegeid, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries
		FROM Update_Security_Tuple2 Source 
			INNER JOIN SecurityTargets t ON t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2 AND t.Arity = 2
			INNER JOIN AclEntries e ON Source.PrivilegeId = e.PrivilegeId
			INNER JOIN @incomingClosures i ON i.[End] = e.SubjectId AND i.[Start] = Source.SubjectId
		WHERE NOT EXISTS(SELECT NULL FROM Security_Tuple2 st WHERE st.id1 = Source.Id1 AND st.ID2 = source.Id2 AND st.ParentID1 = Source.ParentId1 AND st.ParentID2 = Source.ParentId2 AND st.PrivilegeID = Source.PrivilegeId AND st.SubjectID = Source.SubjectId AND st.ViaMembership = Source.ViaMembership AND st.DeniedEntries = Source.DeniedEntries AND st.ExplicitAllowedEntries = source.ExplicitAllowedEntries AND st.ExplicitDeniedEntries = source.ExplicitDeniedEntries)


		INSERT INTO Security_Tuple3 (Id1, Id2, Id3, ParentId1, ParentId2, ParentId3, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) SELECT DISTINCT Source.Id1, Source.Id2, Source.Id3, Source.parentid1, Source.ParentId2, Source.ParentId3, Source.SubjectId, Source.privilegeid, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries
		FROM Update_Security_Tuple3 Source 
			INNER JOIN SecurityTargets t ON t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2 AND t.ObjectId3 = source.ParentId3 AND t.Arity = 3
			INNER JOIN AclEntries e ON Source.PrivilegeId = e.PrivilegeId
			INNER JOIN @incomingClosures i ON i.[End] = e.SubjectId AND i.[Start] = Source.SubjectId
		WHERE NOT EXISTS(SELECT NULL FROM Security_Tuple3 st WHERE st.id1 = Source.Id1 AND st.ID2 = source.Id2 AND st.ID3 = source.Id3 AND st.ParentID1 = Source.ParentId1 AND st.ParentID2 = Source.ParentId2 AND st.ParentID3 = source.ParentId3 AND st.PrivilegeID = Source.PrivilegeId AND st.SubjectID = Source.SubjectId AND st.ViaMembership = Source.ViaMembership AND st.DeniedEntries = Source.DeniedEntries AND st.ExplicitAllowedEntries = source.ExplicitAllowedEntries AND st.ExplicitDeniedEntries = source.ExplicitDeniedEntries)

		INSERT INTO Security_Tuple4 (Id1, Id2, Id3, Id4, ParentId1, ParentId2, ParentId3, ParentId4, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) SELECT DISTINCT Source.Id1, Source.Id2, Source.Id3, Source.Id4, Source.parentid1, Source.ParentId2, Source.ParentId3, Source.ParentId4, Source.SubjectId, Source.privilegeid, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries
		FROM Update_Security_Tuple4 Source 
			INNER JOIN AclEntries e ON Source.PrivilegeId = e.PrivilegeId
			INNER JOIN @incomingClosures i ON i.[End] = e.SubjectId AND i.[Start] = Source.SubjectId
		WHERE NOT EXISTS(SELECT NULL FROM Security_Tuple4 st WHERE st.id1 = Source.Id1 AND st.ID2 = source.Id2 AND st.ID3 = source.Id3 AND st.id4 = source.Id4 AND st.ParentID1 = Source.ParentId1 AND st.ParentID2 = Source.ParentId2 AND st.ParentID3 = source.ParentId3 and ST.ParentID4 = SOURCE.ParentId4 AND st.PrivilegeID = Source.PrivilegeId AND st.SubjectID = Source.SubjectId AND st.ViaMembership = Source.ViaMembership AND st.DeniedEntries = Source.DeniedEntries AND st.ExplicitAllowedEntries = source.ExplicitAllowedEntries AND st.ExplicitDeniedEntries = source.ExplicitDeniedEntries)


		fetch next from c into @s, @e
	end

	close c
	deallocate c
");

            Sql(@"ALTER trigger [dbo].[SecurityInheritance_Insert] on [dbo].[SecurityInheritance] after insert as
	if exists(select * from SecurityInheritanceClosure e 
              inner join inserted i 
              on e.[Start] = i.[End] and e.[End] = i.[Start] and i.[Start] <> i.[End])
    begin
		raiserror('Attempt to create a cycle', 16, 1)
		rollback tran
		return
    end

	declare c cursor local forward_only read_only for select [Start], [End] from inserted
	open c
	declare @s uniqueidentifier, @e uniqueidentifier
	fetch next from c into @s, @e

	declare @incomingClosures table( [Start] uniqueidentifier not null, [End] uniqueidentifier not null, [Distance] int not null )

	while @@fetch_status = 0 begin
		
	
		insert into @incomingClosures([Start],[End],[Distance]) values( @s, @e, case when @s = @e then 0 else 1 end )

		if @s <> @e begin
			insert into @incomingClosures([Start],[End],[Distance])
			select bf.[Start], af.[End], bf.[Distance] + af.[Distance] + 1
			from SecurityInheritanceClosure bf
			inner join SecurityInheritanceClosure af 
			on @s = bf.[End] and @e = af.[Start] and bf.[Start] <> bf.[End] and af.[Start] <> af.[End]
    
			insert into @incomingClosures([Start],[End],[Distance])
			select @s, af.[End], af.[Distance] + 1
			from SecurityInheritanceClosure af where @e = af.[Start] and af.[Start] <> af.[End]

			insert into @incomingClosures([Start],[End],[Distance])
			select bf.[Start], @e, bf.[Distance] + 1
			from SecurityInheritanceClosure bf where @s = bf.[End] and bf.[Start] <> bf.[End]
		end

		update SecurityInheritanceClosure set [Distance] = i.[Distance]
		from SecurityInheritanceClosure c
		inner join @incomingClosures i on c.[Start] = i.[Start] and c.[End] = i.[End] and c.[Distance] > i.[Distance]

		insert into SecurityInheritanceClosure([Start], [End], [Distance])
		select i.[Start], i.[End], i.[Distance] from @incomingClosures i
		left join SecurityInheritanceClosure ex on ex.[Start] = i.[Start] and ex.[End] = i.[End]
		where ex.[Start] is null


		--Now update the security -tuples here.
		INSERT INTO Security_Tuple1 (Id1, ParentId1, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) SELECT DISTINCT Source.Id1, Source.parentid1, Source.SubjectId, Source.privilegeid, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries
		FROM Update_Security_Tuple1 Source 
			INNER JOIN SecurityTargets t ON t.ObjectId1 = Source.ParentId1 AND t.Arity = 1
			INNER JOIN @incomingClosures i ON i.[End] = Source.ParentId1 AND i.[Start] = Source.Id1
			INNER JOIN SecurityInheritanceClosure c on i.[End] = c.[End]
		WHERE NOT EXISTS(SELECT NULL FROM Security_Tuple1 st WHERE st.id1 = Source.Id1 AND st.ParentID1 = Source.ParentId1 AND st.PrivilegeID = Source.PrivilegeId AND st.SubjectID = Source.SubjectId AND st.ViaMembership = Source.ViaMembership AND st.DeniedEntries = Source.DeniedEntries AND st.ExplicitAllowedEntries = source.ExplicitAllowedEntries AND st.ExplicitDeniedEntries = source.ExplicitDeniedEntries)

		INSERT INTO Security_Tuple2 (Id1, Id2, ParentId1, ParentId2, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) SELECT DISTINCT Source.Id1, Source.Id2, Source.parentid1, Source.ParentId2, Source.SubjectId, Source.privilegeid, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries
		FROM Update_Security_Tuple2 Source 
			INNER JOIN SecurityTargets t ON t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2 AND t.Arity = 2
			INNER JOIN @incomingClosures i ON (i.[End] = Source.ParentId1 AND i.[Start] = Source.Id1) OR (i.[End] = Source.ParentId2 AND i.[Start] = Source.Id2)
			INNER JOIN SecurityInheritanceClosure c on i.[End] = c.[End]
		WHERE NOT EXISTS(SELECT NULL FROM Security_Tuple2 st WHERE st.id1 = Source.Id1 AND st.ID2 = source.Id2 AND st.ParentID1 = Source.ParentId1 AND st.ParentID2 = Source.ParentId2 AND st.PrivilegeID = Source.PrivilegeId AND st.SubjectID = Source.SubjectId AND st.ViaMembership = Source.ViaMembership AND st.DeniedEntries = Source.DeniedEntries AND st.ExplicitAllowedEntries = source.ExplicitAllowedEntries AND st.ExplicitDeniedEntries = source.ExplicitDeniedEntries)


		INSERT INTO Security_Tuple3 (Id1, Id2, Id3, ParentId1, ParentId2, ParentId3, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) SELECT DISTINCT Source.Id1, Source.Id2, Source.Id3, Source.parentid1, Source.ParentId2, Source.ParentId3, Source.SubjectId, Source.privilegeid, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries
		FROM Update_Security_Tuple3 Source 
			INNER JOIN SecurityTargets t ON t.ObjectId1 = Source.ParentId1 AND t.ObjectId2 = Source.ParentId2 AND t.ObjectId3 = Source.ParentId3 AND t.Arity = 3
			INNER JOIN @incomingClosures i ON (i.[End] = Source.ParentId1 AND i.[Start] = Source.Id1) OR (i.[End] = Source.ParentId2 AND i.[Start] = Source.Id2) OR (i.[End] = Source.ParentId3 AND i.[Start] = Source.Id3)
			INNER JOIN SecurityInheritanceClosure c on i.[End] = c.[End]
		WHERE NOT EXISTS(SELECT NULL FROM Security_Tuple3 st WHERE st.id1 = Source.Id1 AND st.ID2 = source.Id2 AND st.ID3 = source.Id3 AND st.ParentID1 = Source.ParentId1 AND st.ParentID2 = Source.ParentId2 AND st.ParentID3 = source.ParentId3 AND st.PrivilegeID = Source.PrivilegeId AND st.SubjectID = Source.SubjectId AND st.ViaMembership = Source.ViaMembership AND st.DeniedEntries = Source.DeniedEntries AND st.ExplicitAllowedEntries = source.ExplicitAllowedEntries AND st.ExplicitDeniedEntries = source.ExplicitDeniedEntries)


		INSERT INTO Security_Tuple4 (Id1, Id2, Id3, Id4, ParentId1, ParentId2, ParentId3, ParentId4, SubjectId, PrivilegeId, ViaMemberShip, DeniedEntries, ExplicitDeniedEntries, ExplicitAllowedEntries) SELECT DISTINCT Source.Id1, Source.Id2, Source.Id3, Source.Id4, Source.parentid1, Source.ParentId2, Source.ParentId3, Source.ParentId4, Source.SubjectId, Source.privilegeid, Source.ViaMembership, Source.DeniedEntries, Source.ExplicitDeniedEntries, Source.ExplicitAllowedEntries
		FROM Update_Security_Tuple4 Source 
			INNER JOIN @incomingClosures i ON (i.[End] = Source.ParentId1 AND i.[Start] = Source.Id1) OR (i.[End] = Source.ParentId2 AND i.[Start] = Source.Id2) OR (i.[End] = Source.ParentId3 AND i.[Start] = Source.Id3) OR (i.[End] = Source.ParentId4 AND i.[Start] = Source.Id4)
			INNER JOIN SecurityInheritanceClosure c on i.[End] = c.[End]
		WHERE NOT EXISTS(SELECT NULL FROM Security_Tuple4 st WHERE st.id1 = Source.Id1 AND st.ID2 = source.Id2 AND st.ID3 = source.Id3 AND st.id4 = source.Id4 AND st.ParentID1 = Source.ParentId1 AND st.ParentID2 = Source.ParentId2 AND st.ParentID3 = source.ParentId3 and ST.ParentID4 = SOURCE.ParentId4 AND st.PrivilegeID = Source.PrivilegeId AND st.SubjectID = Source.SubjectId AND st.ViaMembership = Source.ViaMembership AND st.DeniedEntries = Source.DeniedEntries AND st.ExplicitAllowedEntries = source.ExplicitAllowedEntries AND st.ExplicitDeniedEntries = source.ExplicitDeniedEntries)


		fetch next from c into @s, @e
	end

	close c
	deallocate c");

        }
        
        public override void Down()
        {
        }
    }
}
