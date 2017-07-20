namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedWorkflowRoles : DbMigration
    {
        public override void Up()
        {

            Sql(@"update [dbo].[WorkflowRoles] set Description='The Requestor that initiated the request.' WHERE ID='84210799-7B81-4914-90BE-477507391D0F'");
            Sql(@"update [dbo].[WorkflowRoles] set Description='The Requestor that initiated the request.' WHERE ID='2F1D765F-11C4-4F95-A879-480AB968008B'");
            Sql(@"update [dbo].[WorkflowRoles] set Description='The Requestor that initiated the request.' WHERE ID='6443B7B5-C507-48F9-A059-A366062D49AE'");

        }
        
        public override void Down()
        {
            Sql(@"update [dbo].[WorkflowRoles] set Description='The Requestor of the workflow.' WHERE ID='84210799-7B81-4914-90BE-477507391D0F'");
            Sql(@"update [dbo].[WorkflowRoles] set Description='The Requestor of the workflow.' WHERE ID='2F1D765F-11C4-4F95-A879-480AB968008B'");
            Sql(@"update [dbo].[WorkflowRoles] set Description='The Requestor of the workflow.' WHERE ID='6443B7B5-C507-48F9-A059-A366062D49AE'");
        }
    }
}
