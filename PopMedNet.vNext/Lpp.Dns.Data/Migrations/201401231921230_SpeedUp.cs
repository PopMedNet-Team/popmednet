namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpeedUp : DbMigration
    {
        public override void Up()
        {
            Sql(@"


CREATE NONCLUSTERED INDEX [_dta_index_AuditEvents_16_1574296668__K2_K3_K5_K6_K4_7] ON [dbo].[AuditEvents] 
(
	[KindId] ASC,
	[Time] ASC,
	[TargetId2] ASC,
	[TargetId3] ASC,
	[TargetId1] ASC
)
INCLUDE ( [TargetId4]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_1574296668_3_7] ON [dbo].[AuditEvents]([Time], [TargetId4])


CREATE STATISTICS [_dta_stat_1574296668_2_1] ON [dbo].[AuditEvents]([KindId], [Id])


CREATE STATISTICS [_dta_stat_1574296668_1_3] ON [dbo].[AuditEvents]([Id], [Time])


CREATE STATISTICS [_dta_stat_1574296668_4_3_2] ON [dbo].[AuditEvents]([TargetId1], [Time], [KindId])


CREATE STATISTICS [_dta_stat_1574296668_5_6_2] ON [dbo].[AuditEvents]([TargetId2], [TargetId3], [KindId])


CREATE STATISTICS [_dta_stat_1574296668_2_3_7] ON [dbo].[AuditEvents]([KindId], [Time], [TargetId4])


CREATE STATISTICS [_dta_stat_1574296668_5_3_7_6] ON [dbo].[AuditEvents]([TargetId2], [Time], [TargetId4], [TargetId3])


CREATE STATISTICS [_dta_stat_1574296668_7_4_3_2] ON [dbo].[AuditEvents]([TargetId4], [TargetId1], [Time], [KindId])


CREATE STATISTICS [_dta_stat_1574296668_3_5_6_4] ON [dbo].[AuditEvents]([Time], [TargetId2], [TargetId3], [TargetId1])


CREATE STATISTICS [_dta_stat_1574296668_7_6_5_2] ON [dbo].[AuditEvents]([TargetId4], [TargetId3], [TargetId2], [KindId])


CREATE STATISTICS [_dta_stat_1574296668_5_3_6_2_4] ON [dbo].[AuditEvents]([TargetId2], [Time], [TargetId3], [KindId], [TargetId1])


CREATE STATISTICS [_dta_stat_1574296668_7_6_5_3_4] ON [dbo].[AuditEvents]([TargetId4], [TargetId3], [TargetId2], [Time], [TargetId1])


CREATE STATISTICS [_dta_stat_1574296668_3_6_7_2_5_4] ON [dbo].[AuditEvents]([Time], [TargetId3], [TargetId4], [KindId], [TargetId2], [TargetId1])


CREATE NONCLUSTERED INDEX [_dta_index_AclEntries_16_1062294844__K4_K3_K2_K7_6] ON [dbo].[AclEntries] 
(
	[PrivilegeId] ASC,
	[SubjectId] ASC,
	[TargetId] ASC,
	[ChangedOn] ASC
)
INCLUDE ( [Allow]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_AclEntries_16_1062294844__K4_K2_K3_K7_6] ON [dbo].[AclEntries] 
(
	[PrivilegeId] ASC,
	[TargetId] ASC,
	[SubjectId] ASC,
	[ChangedOn] ASC
)
INCLUDE ( [Allow]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_AclEntries_16_1062294844__K2_K4_K3_K7_6] ON [dbo].[AclEntries] 
(
	[TargetId] ASC,
	[PrivilegeId] ASC,
	[SubjectId] ASC,
	[ChangedOn] ASC
)
INCLUDE ( [Allow]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_AclEntries_16_1062294844__K4_K2_K3_K7] ON [dbo].[AclEntries] 
(
	[PrivilegeId] ASC,
	[TargetId] ASC,
	[SubjectId] ASC,
	[ChangedOn] ASC
)WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_1062294844_2_3] ON [dbo].[AclEntries]([TargetId], [SubjectId])


CREATE STATISTICS [_dta_stat_1062294844_7_4_2] ON [dbo].[AclEntries]([ChangedOn], [PrivilegeId], [TargetId])


CREATE NONCLUSTERED INDEX [_dta_index_SecurityInheritanceClosure_16_123863508__K2_K3_K1] ON [dbo].[SecurityInheritanceClosure] 
(
	[End] ASC,
	[Distance] ASC,
	[Start] ASC
)WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_123863508_1_3] ON [dbo].[SecurityInheritanceClosure]([Start], [Distance])


CREATE STATISTICS [_dta_stat_123863508_1_2_3] ON [dbo].[SecurityInheritanceClosure]([Start], [End], [Distance])


CREATE NONCLUSTERED INDEX [_dta_index_SecurityInheritanceClosure3_16_267864021__K2_K1_3] ON [dbo].[SecurityInheritanceClosure3] 
(
	[End] ASC,
	[Start] ASC
)
INCLUDE ( [Distance]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_SecurityInheritanceClosure2_16_219863850__K2_K1_3] ON [dbo].[SecurityInheritanceClosure2] 
(
	[End] ASC,
	[Start] ASC
)
INCLUDE ( [Distance]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_SecurityTargets_16_822293989__K1_K2_K5_K4_K3] ON [dbo].[SecurityTargets] 
(
	[Id] ASC,
	[Arity] ASC,
	[ObjectId3] ASC,
	[ObjectId2] ASC,
	[ObjectId1] ASC
)WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_822293989_5_3] ON [dbo].[SecurityTargets]([ObjectId3], [ObjectId1])


CREATE STATISTICS [_dta_stat_822293989_3_1] ON [dbo].[SecurityTargets]([ObjectId1], [Id])


CREATE STATISTICS [_dta_stat_822293989_4_2] ON [dbo].[SecurityTargets]([ObjectId2], [Arity])


CREATE STATISTICS [_dta_stat_822293989_2_1_3] ON [dbo].[SecurityTargets]([Arity], [Id], [ObjectId1])


CREATE STATISTICS [_dta_stat_822293989_3_2_5] ON [dbo].[SecurityTargets]([ObjectId1], [Arity], [ObjectId3])


CREATE STATISTICS [_dta_stat_822293989_2_5_4] ON [dbo].[SecurityTargets]([Arity], [ObjectId3], [ObjectId2])


CREATE STATISTICS [_dta_stat_822293989_5_4_3_1] ON [dbo].[SecurityTargets]([ObjectId3], [ObjectId2], [ObjectId1], [Id])


CREATE STATISTICS [_dta_stat_822293989_3_4_5_2_1] ON [dbo].[SecurityTargets]([ObjectId1], [ObjectId2], [ObjectId3], [Arity], [Id])


CREATE NONCLUSTERED INDEX [_dta_index_QueriesDataMarts_16_1666104976__K1_K3] ON [dbo].[QueriesDataMarts] 
(
	[QueryId] ASC,
	[QueryStatusTypeId] ASC
)WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_1666104976_3_2] ON [dbo].[QueriesDataMarts]([QueryStatusTypeId], [DataMartId])


CREATE STATISTICS [_dta_stat_1666104976_9_3_1] ON [dbo].[QueriesDataMarts]([IsResultsGrouped], [QueryStatusTypeId], [QueryId])


CREATE STATISTICS [_dta_stat_1666104976_1_3_2] ON [dbo].[QueriesDataMarts]([QueryId], [QueryStatusTypeId], [DataMartId])


CREATE STATISTICS [_dta_stat_1666104976_9_1_2_3] ON [dbo].[QueriesDataMarts]([IsResultsGrouped], [QueryId], [DataMartId], [QueryStatusTypeId])


CREATE NONCLUSTERED INDEX [_dta_index_Queries_16_98099390__K1_K30] ON [dbo].[Queries] 
(
	[QueryId] ASC,
	[ProjectId] ASC
)WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_98099390_23] ON [dbo].[Queries]([IsTemplate])


CREATE STATISTICS [_dta_stat_98099390_1_17] ON [dbo].[Queries]([QueryId], [ActivityId])


CREATE STATISTICS [_dta_stat_98099390_25_30] ON [dbo].[Queries]([OrganizationId], [ProjectId])


CREATE STATISTICS [_dta_stat_98099390_25_2] ON [dbo].[Queries]([OrganizationId], [CreatedByUserId])


CREATE STATISTICS [_dta_stat_98099390_22_2] ON [dbo].[Queries]([Updated], [CreatedByUserId])


CREATE STATISTICS [_dta_stat_98099390_30_24_25] ON [dbo].[Queries]([ProjectId], [SID], [OrganizationId])


CREATE STATISTICS [_dta_stat_98099390_2_25_30] ON [dbo].[Queries]([CreatedByUserId], [OrganizationId], [ProjectId])


CREATE STATISTICS [_dta_stat_98099390_30_1_2] ON [dbo].[Queries]([ProjectId], [QueryId], [CreatedByUserId])


CREATE STATISTICS [_dta_stat_98099390_24_25_2_1] ON [dbo].[Queries]([SID], [OrganizationId], [CreatedByUserId], [QueryId])


CREATE STATISTICS [_dta_stat_98099390_30_1_24_25] ON [dbo].[Queries]([ProjectId], [QueryId], [SID], [OrganizationId])


CREATE STATISTICS [_dta_stat_98099390_30_2_24_25_1] ON [dbo].[Queries]([ProjectId], [CreatedByUserId], [SID], [OrganizationId], [QueryId])


CREATE STATISTICS [_dta_stat_98099390_2_1_25_30_22] ON [dbo].[Queries]([CreatedByUserId], [QueryId], [OrganizationId], [ProjectId], [Updated])


CREATE NONCLUSTERED INDEX [_dta_index_Users_16_1461580245__K1_K17_K5] ON [dbo].[Users] 
(
	[UserId] ASC,
	[SID] ASC,
	[OrganizationId] ASC
)WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_1461580245_17_18] ON [dbo].[Users]([SID], [PasswordExpiration])


CREATE STATISTICS [_dta_stat_1461580245_5_1] ON [dbo].[Users]([OrganizationId], [UserId])


CREATE STATISTICS [_dta_stat_1461580245_17_5_18] ON [dbo].[Users]([SID], [OrganizationId], [PasswordExpiration])


CREATE STATISTICS [_dta_stat_1461580245_17_5_1] ON [dbo].[Users]([SID], [OrganizationId], [UserId])


");
        }
        
        public override void Down()
        {
        }
    }
}
