namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixWorkflowActivityCompletionMapDuplicates : DbMigration
    {
        public override void Up()
        {
            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '3CF0FEA0-26B9-4042-91F3-7192D44F6F7C' AND SourceWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' AND DestinationWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' AND WorkflowID = 'F64E0001-4F9A-49F0-BF75-A3B501396946'");

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '3CF0FEA0-26B9-4042-91F3-7192D44F6F7C' AND SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND DestinationWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'");

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '49F9C682-9FAD-4AE5-A2C5-19157E227186' AND SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND DestinationWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'");

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '7821FC45-9FD5-4597-A405-B021E5ED14FA' AND SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND DestinationWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND WorkflowID = '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D'");

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '3CF0FEA0-26B9-4042-91F3-7192D44F6F7C' AND SourceWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND DestinationWorkflowActivityID = 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'");

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '3CF0FEA0-26B9-4042-91F3-7192D44F6F7C' AND SourceWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' AND DestinationWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' AND WorkflowID = '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8'");

            Sql(@"DELETE FROM WorkflowActivityCompletionMaps WHERE WorkflowActivityResultID = '3CF0FEA0-26B9-4042-91F3-7192D44F6F7C' AND SourceWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' AND DestinationWorkflowActivityID = 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55' AND WorkflowID = '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663'");

        }
        
        public override void Down()
        {
            Sql(@"INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID], [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('3CF0FEA0-26B9-4042-91F3-7192D44F6F7C', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'F64E0001-4F9A-49F0-BF75-A3B501396946')");

            Sql(@"INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID], [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('3CF0FEA0-26B9-4042-91F3-7192D44F6F7C', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')");

            Sql(@"INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID], [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('49F9C682-9FAD-4AE5-A2C5-19157E227186', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')");

            Sql(@"INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID], [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('7821FC45-9FD5-4597-A405-B021E5ED14FA', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D')");

            Sql(@"INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID], [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('3CF0FEA0-26B9-4042-91F3-7192D44F6F7C', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', 'ACBA0001-0CE4-4C00-8DD3-A3B5013A3344', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");

            Sql(@"INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID], [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('3CF0FEA0-26B9-4042-91F3-7192D44F6F7C', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8')");

            Sql(@"INSERT INTO [dbo].[WorkflowActivityCompletionMaps] ([WorkflowActivityResultID], [SourceWorkflowActivityID], [DestinationWorkflowActivityID], [WorkflowID]) VALUES ('3CF0FEA0-26B9-4042-91F3-7192D44F6F7C', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', 'D0E659B8-1155-4F44-9728-B4B6EA4D4D55', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663')");
        }
    }
}
