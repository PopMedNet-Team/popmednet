namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpeedUp1 : DbMigration
    {
        public override void Up()
        {
            Sql(@"SET ANSI_PADDING ON



CREATE NONCLUSTERED INDEX [_dta_index_Requests_6_210099789__K20D_K24_K9_1_3_6_11_15_19_30_35_37_39_42_45_46_47_48] ON [dbo].[Requests]
(
	[SubmittedOn] DESC,
	[ID] ASC,
	[IsDeleted] ASC
)
INCLUDE ( 	[Identifier],
	[CreatedOn],
	[Name],
	[Priority],
	[DueDate],
	[RequestTypeID],
	[ProjectId],
	[OrganizationID],
	[SubmittedByID],
	[CreatedByID],
	[Status],
	[RejectedOn],
	[RejectedByID],
	[CancelledOn],
	[CancelledByID]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_210099789_35_24_9] ON [dbo].[Requests]([OrganizationID], [ID], [IsDeleted])


CREATE STATISTICS [_dta_stat_210099789_37_39_46] ON [dbo].[Requests]([SubmittedByID], [CreatedByID], [RejectedByID])


CREATE STATISTICS [_dta_stat_210099789_24_9_37_39_46] ON [dbo].[Requests]([ID], [IsDeleted], [SubmittedByID], [CreatedByID], [RejectedByID])


CREATE STATISTICS [_dta_stat_210099789_24_9_39_37_30] ON [dbo].[Requests]([ID], [IsDeleted], [CreatedByID], [SubmittedByID], [ProjectId])


CREATE STATISTICS [_dta_stat_210099789_19_24_9_30_35_39] ON [dbo].[Requests]([RequestTypeID], [ID], [IsDeleted], [ProjectId], [OrganizationID], [CreatedByID])


CREATE STATISTICS [_dta_stat_210099789_24_9_39_48_46_37_30_35] ON [dbo].[Requests]([ID], [IsDeleted], [CreatedByID], [CancelledByID], [RejectedByID], [SubmittedByID], [ProjectId], [OrganizationID])


CREATE STATISTICS [_dta_stat_210099789_9_24_30_39_37_19_48_46_20] ON [dbo].[Requests]([IsDeleted], [ID], [ProjectId], [CreatedByID], [SubmittedByID], [RequestTypeID], [CancelledByID], [RejectedByID], [SubmittedOn])


CREATE STATISTICS [_dta_stat_210099789_30_24_9_35_39_37_19_48_46_20] ON [dbo].[Requests]([ProjectId], [ID], [IsDeleted], [OrganizationID], [CreatedByID], [SubmittedByID], [RequestTypeID], [CancelledByID], [RejectedByID], [SubmittedOn])


CREATE STATISTICS [_dta_stat_477960779_1_5] ON [dbo].[AclProjectDataMarts]([SecurityGroupID], [Allowed])


CREATE STATISTICS [_dta_stat_445960665_1_4] ON [dbo].[AclUsers]([SecurityGroupID], [Allowed])


CREATE STATISTICS [_dta_stat_1498488417_4_5] ON [dbo].[AclProjectOrganizations]([OrganizationID], [Allowed])


CREATE STATISTICS [_dta_stat_1498488417_2_4_1] ON [dbo].[AclProjectOrganizations]([PermissionID], [OrganizationID], [SecurityGroupID])
");
        }
        
        public override void Down()
        {
        }
    }
}
