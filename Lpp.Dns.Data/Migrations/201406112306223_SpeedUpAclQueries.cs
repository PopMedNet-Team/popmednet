namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpeedUpAclQueries : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE NONCLUSTERED INDEX [_dta_index_AclUsers_24_1865109735__K2_K4_K3_K1] ON [dbo].[AclUsers]
(
	[PermissionID] ASC,
	[Allowed] ASC,
	[UserID] ASC,
	[SecurityGroupID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_1865109735_3_2] ON [dbo].[AclUsers]([UserID], [PermissionID])


CREATE STATISTICS [_dta_stat_1865109735_3_1] ON [dbo].[AclUsers]([UserID], [SecurityGroupID])


CREATE STATISTICS [_dta_stat_1865109735_4_2_3] ON [dbo].[AclUsers]([Allowed], [PermissionID], [UserID])


CREATE STATISTICS [_dta_stat_1865109735_1_2_4_3] ON [dbo].[AclUsers]([SecurityGroupID], [PermissionID], [Allowed], [UserID])


SET ANSI_PADDING ON



CREATE NONCLUSTERED INDEX [_dta_index_Requests_24_98099390__K20D_K24_3_6_9_11_13_16_18_19_22_23_26_27_28_29_30_33_34_35_36_37_38_39_40_41_42_43_46_47_48_] ON [dbo].[Requests]
(
	[SubmittedOn] DESC,
	[ID] ASC
)
INCLUDE ( 	[CreatedOn],
	[Name],
	[IsDeleted],
	[Priority],
	[ActivityDescription],
	[IRBApprovalNo],
	[IsScheduled],
	[RequestTypeID],
	[UpdatedOn],
	[IsTemplate],
	[PurposeOfUse],
	[PhiDisclosureLevel],
	[Schedule],
	[ScheduleCount],
	[ProjectId],
	[RequesterCenterID],
	[WorkplanTypeID],
	[OrganizationID],
	[Description],
	[SubmittedByID],
	[CreatedByID],
	[UpdatedByID],
	[ActivityID],
	[TimeStamp],
	[Identifier],
	[Status],
	[RejectedOn],
	[RejectedByID],
	[CancelledOn],
	[CancelledByID]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_Requests_24_98099390__K39_K37_K38_K35_K30_K24] ON [dbo].[Requests]
(
	[UpdatedByID] ASC,
	[SubmittedByID] ASC,
	[CreatedByID] ASC,
	[OrganizationID] ASC,
	[ProjectId] ASC,
	[ID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_Requests_24_98099390__K30_K35_K24_K39_K37_K38] ON [dbo].[Requests]
(
	[ProjectId] ASC,
	[OrganizationID] ASC,
	[ID] ASC,
	[UpdatedByID] ASC,
	[SubmittedByID] ASC,
	[CreatedByID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_98099390_24_30] ON [dbo].[Requests]([ID], [ProjectId])


CREATE STATISTICS [_dta_stat_98099390_24_35] ON [dbo].[Requests]([ID], [OrganizationID])


CREATE STATISTICS [_dta_stat_98099390_30_34] ON [dbo].[Requests]([ProjectId], [WorkplanTypeID])


CREATE STATISTICS [_dta_stat_98099390_35_34_33] ON [dbo].[Requests]([OrganizationID], [WorkplanTypeID], [RequesterCenterID])


CREATE STATISTICS [_dta_stat_98099390_34_33_30_35] ON [dbo].[Requests]([WorkplanTypeID], [RequesterCenterID], [ProjectId], [OrganizationID])


CREATE STATISTICS [_dta_stat_98099390_39_38_49_47] ON [dbo].[Requests]([UpdatedByID], [CreatedByID], [CancelledByID], [RejectedByID])


CREATE STATISTICS [_dta_stat_98099390_35_30_24_39_37] ON [dbo].[Requests]([OrganizationID], [ProjectId], [ID], [UpdatedByID], [SubmittedByID])


CREATE STATISTICS [_dta_stat_98099390_39_37_38_30_35_24] ON [dbo].[Requests]([UpdatedByID], [SubmittedByID], [CreatedByID], [ProjectId], [OrganizationID], [ID])


CREATE STATISTICS [_dta_stat_98099390_19_34_33_30_35_24_39_37] ON [dbo].[Requests]([RequestTypeID], [WorkplanTypeID], [RequesterCenterID], [ProjectId], [OrganizationID], [ID], [UpdatedByID], [SubmittedByID])


CREATE STATISTICS [_dta_stat_98099390_39_37_38_47_49_34_33_30_35_24] ON [dbo].[Requests]([UpdatedByID], [SubmittedByID], [CreatedByID], [RejectedByID], [CancelledByID], [WorkplanTypeID], [RequesterCenterID], [ProjectId], [OrganizationID], [ID])


CREATE STATISTICS [_dta_stat_98099390_24_34_33_30_35_39_37_38_19_49_47_20] ON [dbo].[Requests]([ID], [WorkplanTypeID], [RequesterCenterID], [ProjectId], [OrganizationID], [UpdatedByID], [SubmittedByID], [CreatedByID], [RequestTypeID], [CancelledByID], [RejectedByID], [SubmittedOn])


CREATE NONCLUSTERED INDEX [_dta_index_AclDataMarts_24_409104548__K2_K3_K1_K4] ON [dbo].[AclDataMarts]
(
	[PermissionID] ASC,
	[DataMartID] ASC,
	[SecurityGroupID] ASC,
	[Allowed] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_AclDataMarts_24_409104548__K2_K4_K3_K1] ON [dbo].[AclDataMarts]
(
	[PermissionID] ASC,
	[Allowed] ASC,
	[DataMartID] ASC,
	[SecurityGroupID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_409104548_3_2] ON [dbo].[AclDataMarts]([DataMartID], [PermissionID])


CREATE STATISTICS [_dta_stat_409104548_3_1] ON [dbo].[AclDataMarts]([DataMartID], [SecurityGroupID])


CREATE STATISTICS [_dta_stat_409104548_3_4_2] ON [dbo].[AclDataMarts]([DataMartID], [Allowed], [PermissionID])


CREATE STATISTICS [_dta_stat_409104548_1_4_2] ON [dbo].[AclDataMarts]([SecurityGroupID], [Allowed], [PermissionID])


CREATE STATISTICS [_dta_stat_409104548_1_2_3_4] ON [dbo].[AclDataMarts]([SecurityGroupID], [PermissionID], [DataMartID], [Allowed])


CREATE STATISTICS [_dta_stat_409104548_4_2_1_3] ON [dbo].[AclDataMarts]([Allowed], [PermissionID], [SecurityGroupID], [DataMartID])


CREATE NONCLUSTERED INDEX [_dta_index_AclProjectOrganizations_24_1259203586__K3_K2_K5_K1] ON [dbo].[AclProjectOrganizations]
(
	[ProjectID] ASC,
	[PermissionID] ASC,
	[Allowed] ASC,
	[SecurityGroupID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_1259203586_1_3] ON [dbo].[AclProjectOrganizations]([SecurityGroupID], [ProjectID])


CREATE STATISTICS [_dta_stat_1259203586_5_1_2] ON [dbo].[AclProjectOrganizations]([Allowed], [SecurityGroupID], [PermissionID])


CREATE STATISTICS [_dta_stat_1259203586_2_5_1] ON [dbo].[AclProjectOrganizations]([PermissionID], [Allowed], [SecurityGroupID])


CREATE STATISTICS [_dta_stat_1259203586_2_1_3_5] ON [dbo].[AclProjectOrganizations]([PermissionID], [SecurityGroupID], [ProjectID], [Allowed])


CREATE NONCLUSTERED INDEX [_dta_index_AclOrganizations_24_441104662__K3_K2_K1_K4] ON [dbo].[AclOrganizations]
(
	[OrganizationID] ASC,
	[PermissionID] ASC,
	[SecurityGroupID] ASC,
	[Allowed] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_441104662_3_1] ON [dbo].[AclOrganizations]([OrganizationID], [SecurityGroupID])


CREATE STATISTICS [_dta_stat_441104662_3_4_2] ON [dbo].[AclOrganizations]([OrganizationID], [Allowed], [PermissionID])


CREATE STATISTICS [_dta_stat_441104662_4_2_3] ON [dbo].[AclOrganizations]([Allowed], [PermissionID], [OrganizationID])


CREATE STATISTICS [_dta_stat_441104662_1_4_2] ON [dbo].[AclOrganizations]([SecurityGroupID], [Allowed], [PermissionID])


CREATE STATISTICS [_dta_stat_441104662_2_3_1_4] ON [dbo].[AclOrganizations]([PermissionID], [OrganizationID], [SecurityGroupID], [Allowed])


CREATE STATISTICS [_dta_stat_441104662_1_2_3_4] ON [dbo].[AclOrganizations]([SecurityGroupID], [PermissionID], [OrganizationID], [Allowed])


CREATE NONCLUSTERED INDEX [_dta_index_AclProjects_24_473104776__K3_K2_K1_K4] ON [dbo].[AclProjects]
(
	[ProjectID] ASC,
	[PermissionID] ASC,
	[SecurityGroupID] ASC,
	[Allowed] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_473104776_3_4] ON [dbo].[AclProjects]([ProjectID], [Allowed])


CREATE STATISTICS [_dta_stat_473104776_2_3] ON [dbo].[AclProjects]([PermissionID], [ProjectID])


CREATE STATISTICS [_dta_stat_473104776_2_4_3] ON [dbo].[AclProjects]([PermissionID], [Allowed], [ProjectID])


CREATE STATISTICS [_dta_stat_473104776_1_4_2] ON [dbo].[AclProjects]([SecurityGroupID], [Allowed], [PermissionID])


CREATE STATISTICS [_dta_stat_473104776_3_1_2_4] ON [dbo].[AclProjects]([ProjectID], [SecurityGroupID], [PermissionID], [Allowed])


CREATE STATISTICS [_dta_stat_473104776_4_2_3_1] ON [dbo].[AclProjects]([Allowed], [PermissionID], [ProjectID], [SecurityGroupID])


CREATE NONCLUSTERED INDEX [_dta_index_AclProjectDataMarts_24_1897109849__K3_K2_K5] ON [dbo].[AclProjectDataMarts]
(
	[ProjectID] ASC,
	[PermissionID] ASC,
	[Allowed] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_1897109849_1_2_5] ON [dbo].[AclProjectDataMarts]([SecurityGroupID], [PermissionID], [Allowed])


CREATE STATISTICS [_dta_stat_1897109849_2_4_1] ON [dbo].[AclProjectDataMarts]([PermissionID], [DataMartID], [SecurityGroupID])


CREATE STATISTICS [_dta_stat_1897109849_4_1_3] ON [dbo].[AclProjectDataMarts]([DataMartID], [SecurityGroupID], [ProjectID])


CREATE STATISTICS [_dta_stat_1897109849_5_4_1_2] ON [dbo].[AclProjectDataMarts]([Allowed], [DataMartID], [SecurityGroupID], [PermissionID])


CREATE STATISTICS [_dta_stat_1897109849_2_5_4_1] ON [dbo].[AclProjectDataMarts]([PermissionID], [Allowed], [DataMartID], [SecurityGroupID])


CREATE STATISTICS [_dta_stat_1897109849_2_5_3_4_1] ON [dbo].[AclProjectDataMarts]([PermissionID], [Allowed], [ProjectID], [DataMartID], [SecurityGroupID])


CREATE STATISTICS [_dta_stat_1897109849_2_3_4_1_5] ON [dbo].[AclProjectDataMarts]([PermissionID], [ProjectID], [DataMartID], [SecurityGroupID], [Allowed])


CREATE NONCLUSTERED INDEX [_dta_index_AclGlobal_24_1929109963__K2_K1_K3] ON [dbo].[AclGlobal]
(
	[PermissionID] ASC,
	[SecurityGroupID] ASC,
	[Allowed] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_AclGlobal_24_1929109963__K2_K3_K1] ON [dbo].[AclGlobal]
(
	[PermissionID] ASC,
	[Allowed] ASC,
	[SecurityGroupID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_1929109963_1_3] ON [dbo].[AclGlobal]([SecurityGroupID], [Allowed])


CREATE NONCLUSTERED INDEX [_dta_index_RequestDataMarts_24_1666104976__K14_K12_3_16] ON [dbo].[RequestDataMarts]
(
	[RequestID] ASC,
	[DataMartID] ASC
)
INCLUDE ( 	[QueryStatusTypeId],
	[TimeStamp]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_RequestDataMarts_24_1666104976__K14_K12] ON [dbo].[RequestDataMarts]
(
	[RequestID] ASC,
	[DataMartID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


SET ANSI_PADDING ON



CREATE NONCLUSTERED INDEX [_dta_index_Users_24_1461580245__K17_31] ON [dbo].[Users]
(
	[ID] ASC
)
INCLUDE ( 	[Username]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_1461580245_31_4] ON [dbo].[Users]([Username], [isDeleted])


CREATE STATISTICS [_dta_stat_1461580245_17_4] ON [dbo].[Users]([ID], [isDeleted])


CREATE STATISTICS [_dta_stat_1461580245_31_17_4] ON [dbo].[Users]([Username], [ID], [isDeleted])


");
        }
        
        public override void Down()
        {
        }
    }
}
