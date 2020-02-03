namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpeedImprovement2 : DbMigration
    {
        public override void Up()
        {
            Sql(@"


CREATE NONCLUSTERED INDEX [_dta_index_AclEntries_9_1062294844__K4_K3_K2_K7_6] ON [dbo].[AclEntries]
(
	[PrivilegeId] ASC,
	[SubjectId] ASC,
	[TargetId] ASC,
	[ChangedOn] ASC
)
INCLUDE ( 	[Allow]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_AclEntries_9_1062294844__K4_K2_K3_K7_6] ON [dbo].[AclEntries]
(
	[PrivilegeId] ASC,
	[TargetId] ASC,
	[SubjectId] ASC,
	[ChangedOn] ASC
)
INCLUDE ( 	[Allow]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_AclEntries_9_1062294844__K2_K4_K3_K7_6] ON [dbo].[AclEntries]
(
	[TargetId] ASC,
	[PrivilegeId] ASC,
	[SubjectId] ASC,
	[ChangedOn] ASC
)
INCLUDE ( 	[Allow]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_AclEntries_9_1062294844__K3_K4] ON [dbo].[AclEntries]
(
	[SubjectId] ASC,
	[PrivilegeId] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


SET ANSI_PADDING ON



CREATE NONCLUSTERED INDEX [_dta_index_Queries_9_98099390__K22D_K2_1_3_6_11_13_15_17_18_19_20_21_23_24_25_26_27_28_29_30] ON [dbo].[Queries]
(
	[Updated] DESC,
	[CreatedByUserId] ASC
)
INCLUDE ( 	[QueryId],
	[CreatedAt],
	[Name],
	[Priority],
	[ActivityDescription],
	[ActivityDueDate],
	[ActivityId],
	[IsScheduled],
	[RequestTypeId],
	[Submitted],
	[UpdatedByUserId],
	[IsTemplate],
	[SID],
	[OrganizationId],
	[PurposeOfUse],
	[PhiDisclosureLevel],
	[Schedule],
	[ScheduleCount],
	[ProjectId]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


SET ANSI_PADDING ON



CREATE NONCLUSTERED INDEX [_dta_index_AuditPropertyValues_9_1814297523__K2_K1_3_4_5_6_7_8] ON [dbo].[AuditPropertyValues]
(
	[EventId] ASC,
	[Id] ASC
)
INCLUDE ( 	[PropertyId],
	[IntValue],
	[StringValue],
	[DoubleValue],
	[DateTimeValue],
	[GuidValue]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_AuditEvents_9_1574296668__K2_K4_K3_K1_5_6_7_8_9_10_11_12_13] ON [dbo].[AuditEvents]
(
	[KindId] ASC,
	[TargetId1] ASC,
	[Time] ASC,
	[Id] ASC
)
INCLUDE ( 	[TargetId2],
	[TargetId3],
	[TargetId4],
	[TargetId5],
	[TargetId6],
	[TargetId7],
	[TargetId8],
	[TargetId9],
	[TargetId10]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_1574296668_3_2_1] ON [dbo].[AuditEvents]([Time], [KindId], [Id])


CREATE STATISTICS [_dta_stat_1574296668_4_1_2_3] ON [dbo].[AuditEvents]([TargetId1], [Id], [KindId], [Time])


CREATE NONCLUSTERED INDEX [_dta_index_SecurityTargets_9_822293989__K1_K2_K6_K5_K4_K3] ON [dbo].[SecurityTargets]
(
	[Id] ASC,
	[Arity] ASC,
	[ObjectId4] ASC,
	[ObjectId3] ASC,
	[ObjectId2] ASC,
	[ObjectId1] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_SecurityTargets_9_822293989__K4_K2_K1_K3] ON [dbo].[SecurityTargets]
(
	[ObjectId2] ASC,
	[Arity] ASC,
	[Id] ASC,
	[ObjectId1] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_822293989_2_4_1] ON [dbo].[SecurityTargets]([Arity], [ObjectId2], [Id])


CREATE STATISTICS [_dta_stat_822293989_1_3_4] ON [dbo].[SecurityTargets]([Id], [ObjectId1], [ObjectId2])


CREATE STATISTICS [_dta_stat_822293989_5_2_6] ON [dbo].[SecurityTargets]([ObjectId3], [Arity], [ObjectId4])


CREATE STATISTICS [_dta_stat_822293989_5_2_3_1] ON [dbo].[SecurityTargets]([ObjectId3], [Arity], [ObjectId1], [Id])


CREATE STATISTICS [_dta_stat_822293989_2_4_6_1] ON [dbo].[SecurityTargets]([Arity], [ObjectId2], [ObjectId4], [Id])


CREATE STATISTICS [_dta_stat_822293989_1_2_3_4] ON [dbo].[SecurityTargets]([Id], [Arity], [ObjectId1], [ObjectId2])


CREATE STATISTICS [_dta_stat_822293989_6_1_5_4_3] ON [dbo].[SecurityTargets]([ObjectId4], [Id], [ObjectId3], [ObjectId2], [ObjectId1])


CREATE STATISTICS [_dta_stat_822293989_3_2_6_1_5] ON [dbo].[SecurityTargets]([ObjectId1], [Arity], [ObjectId4], [Id], [ObjectId3])


CREATE STATISTICS [_dta_stat_822293989_6_2_1_5_4_3] ON [dbo].[SecurityTargets]([ObjectId4], [Arity], [Id], [ObjectId3], [ObjectId2], [ObjectId1])


");
        }
        
        public override void Down()
        {
        }
    }
}
