namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SpeedUpRequestQuery : DbMigration
    {
        public override void Up()
        {
            Sql(@"
CREATE NONCLUSTERED INDEX [_dta_index_AclUsers_24_1865109735__K2_K3] ON [dbo].[AclUsers]
(
	[PermissionID] ASC,
	[UserID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_AclUsers_24_1865109735__K2_K1] ON [dbo].[AclUsers]
(
	[PermissionID] ASC,
	[SecurityGroupID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_1865109735_3_4] ON [dbo].[AclUsers]([UserID], [Allowed])


CREATE STATISTICS [_dta_stat_1865109735_1_3_4] ON [dbo].[AclUsers]([SecurityGroupID], [UserID], [Allowed])


CREATE NONCLUSTERED INDEX [_dta_index_AclOrganizationUsers_24_1355203928__K2_K5_K4_K1_K3] ON [dbo].[AclOrganizationUsers]
(
	[PermissionID] ASC,
	[Allowed] ASC,
	[UserID] ASC,
	[SecurityGroupID] ASC,
	[OrganizationID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_AclOrganizationUsers_24_1355203928__K2_K4_K1_K3_K5] ON [dbo].[AclOrganizationUsers]
(
	[PermissionID] ASC,
	[UserID] ASC,
	[SecurityGroupID] ASC,
	[OrganizationID] ASC,
	[Allowed] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_1355203928_3_5] ON [dbo].[AclOrganizationUsers]([OrganizationID], [Allowed])


CREATE STATISTICS [_dta_stat_1355203928_4_5] ON [dbo].[AclOrganizationUsers]([UserID], [Allowed])


CREATE STATISTICS [_dta_stat_1355203928_4_1] ON [dbo].[AclOrganizationUsers]([UserID], [SecurityGroupID])


CREATE STATISTICS [_dta_stat_1355203928_2_3] ON [dbo].[AclOrganizationUsers]([PermissionID], [OrganizationID])


CREATE STATISTICS [_dta_stat_1355203928_4_3_5] ON [dbo].[AclOrganizationUsers]([UserID], [OrganizationID], [Allowed])


CREATE STATISTICS [_dta_stat_1355203928_1_5_2_3] ON [dbo].[AclOrganizationUsers]([SecurityGroupID], [Allowed], [PermissionID], [OrganizationID])


CREATE STATISTICS [_dta_stat_1355203928_5_2_3_4] ON [dbo].[AclOrganizationUsers]([Allowed], [PermissionID], [OrganizationID], [UserID])


CREATE NONCLUSTERED INDEX [_dta_index_AclProjectOrganizations_24_1259203586__K2_K5_K3_K4] ON [dbo].[AclProjectOrganizations]
(
	[PermissionID] ASC,
	[Allowed] ASC,
	[ProjectID] ASC,
	[OrganizationID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_AclProjectOrganizations_24_1259203586__K2_K5_K3_K1_K4] ON [dbo].[AclProjectOrganizations]
(
	[PermissionID] ASC,
	[Allowed] ASC,
	[ProjectID] ASC,
	[SecurityGroupID] ASC,
	[OrganizationID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_1259203586_5_3] ON [dbo].[AclProjectOrganizations]([Allowed], [ProjectID])


CREATE STATISTICS [_dta_stat_1259203586_3_4] ON [dbo].[AclProjectOrganizations]([ProjectID], [OrganizationID])


CREATE STATISTICS [_dta_stat_1259203586_2_4_3] ON [dbo].[AclProjectOrganizations]([PermissionID], [OrganizationID], [ProjectID])


CREATE STATISTICS [_dta_stat_1259203586_4_2_5_3_1] ON [dbo].[AclProjectOrganizations]([OrganizationID], [PermissionID], [Allowed], [ProjectID], [SecurityGroupID])


CREATE NONCLUSTERED INDEX [_dta_index_AclDataMarts_24_409104548__K2_K3] ON [dbo].[AclDataMarts]
(
	[PermissionID] ASC,
	[DataMartID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_AclProjects_24_473104776__K2_K4_K1_K3] ON [dbo].[AclProjects]
(
	[PermissionID] ASC,
	[Allowed] ASC,
	[SecurityGroupID] ASC,
	[ProjectID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_AclProjectDataMarts_24_1897109849__K2_K5_K1_K3_K4] ON [dbo].[AclProjectDataMarts]
(
	[PermissionID] ASC,
	[Allowed] ASC,
	[SecurityGroupID] ASC,
	[ProjectID] ASC,
	[DataMartID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_AclProjectDataMarts_24_1897109849__K1_K2_K4_K3] ON [dbo].[AclProjectDataMarts]
(
	[SecurityGroupID] ASC,
	[PermissionID] ASC,
	[DataMartID] ASC,
	[ProjectID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


SET ANSI_PADDING ON



CREATE NONCLUSTERED INDEX [_dta_index_Requests_24_98099390__K24_K38_K37_K30_K35_K19_K49_K47_K42_3_6_11_15_20_43_46_48] ON [dbo].[Requests]
(
	[ID] ASC,
	[CreatedByID] ASC,
	[SubmittedByID] ASC,
	[ProjectId] ASC,
	[OrganizationID] ASC,
	[RequestTypeID] ASC,
	[CancelledByID] ASC,
	[RejectedByID] ASC,
	[Identifier] ASC
)
INCLUDE ( 	[CreatedOn],
	[Name],
	[Priority],
	[DueDate],
	[SubmittedOn],
	[Status],
	[RejectedOn],
	[CancelledOn]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


SET ANSI_PADDING ON



CREATE NONCLUSTERED INDEX [_dta_index_Requests_24_98099390__K24_K30_K35_K38_K37_K19_K49_K47_K42_3_6_11_15_20_43_46_48] ON [dbo].[Requests]
(
	[ID] ASC,
	[ProjectId] ASC,
	[OrganizationID] ASC,
	[CreatedByID] ASC,
	[SubmittedByID] ASC,
	[RequestTypeID] ASC,
	[CancelledByID] ASC,
	[RejectedByID] ASC,
	[Identifier] ASC
)
INCLUDE ( 	[CreatedOn],
	[Name],
	[Priority],
	[DueDate],
	[SubmittedOn],
	[Status],
	[RejectedOn],
	[CancelledOn]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


SET ANSI_PADDING ON



CREATE NONCLUSTERED INDEX [_dta_index_Requests_24_98099390__K42_K24_3_6_11_15_19_20_30_37_38_43_46_47_48_49] ON [dbo].[Requests]
(
	[Identifier] ASC,
	[ID] ASC
)
INCLUDE ( 	[CreatedOn],
	[Name],
	[Priority],
	[DueDate],
	[RequestTypeID],
	[SubmittedOn],
	[ProjectId],
	[SubmittedByID],
	[CreatedByID],
	[Status],
	[RejectedOn],
	[RejectedByID],
	[CancelledOn],
	[CancelledByID]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_Requests_24_98099390__K30_K24] ON [dbo].[Requests]
(
	[ProjectId] ASC,
	[ID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE STATISTICS [_dta_stat_98099390_38_49] ON [dbo].[Requests]([CreatedByID], [CancelledByID])


CREATE STATISTICS [_dta_stat_98099390_24_49_47] ON [dbo].[Requests]([ID], [CancelledByID], [RejectedByID])


CREATE STATISTICS [_dta_stat_98099390_38_37_24] ON [dbo].[Requests]([CreatedByID], [SubmittedByID], [ID])


CREATE STATISTICS [_dta_stat_98099390_24_38_49_47] ON [dbo].[Requests]([ID], [CreatedByID], [CancelledByID], [RejectedByID])


CREATE STATISTICS [_dta_stat_98099390_49_47_38_37_24_30_35] ON [dbo].[Requests]([CancelledByID], [RejectedByID], [CreatedByID], [SubmittedByID], [ID], [ProjectId], [OrganizationID])


CREATE STATISTICS [_dta_stat_98099390_24_30_38_37_19_49_47_42] ON [dbo].[Requests]([ID], [ProjectId], [CreatedByID], [SubmittedByID], [RequestTypeID], [CancelledByID], [RejectedByID], [Identifier])


CREATE STATISTICS [_dta_stat_98099390_24_19_30_35_38_37_49_47_42] ON [dbo].[Requests]([ID], [RequestTypeID], [ProjectId], [OrganizationID], [CreatedByID], [SubmittedByID], [CancelledByID], [RejectedByID], [Identifier])


CREATE NONCLUSTERED INDEX [_dta_index_AclGlobal_24_1929109963__K2_K1] ON [dbo].[AclGlobal]
(
	[PermissionID] ASC,
	[SecurityGroupID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


CREATE NONCLUSTERED INDEX [_dta_index_RequestDataMarts_24_1666104976__K12_14] ON [dbo].[RequestDataMarts]
(
	[DataMartID] ASC
)
INCLUDE ( 	[RequestID]) WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]


");
        }
        
        public override void Down()
        {
        }
    }
}
