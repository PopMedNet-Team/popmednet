namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateWorkflowRolesRequestCreator : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.WorkflowRoles", "IsRequestor", "IsRequestCreator");

            Sql(@"update [dbo].[WorkflowRoles] set Name='Request Creator' WHERE Name='Requestor'");
            Sql(@"update [dbo].[WorkflowRoles] set Description='The Request Creator for the workflow.' WHERE Description='The Requestor of the workflow.'");
            Sql(@"update [dbo].[WorkflowRoles] set Description='The Request Creator for the workflow' WHERE Description='The Requestor of the workflow'");

            Sql(@"INSERT INTO WorkflowRoles (ID, WorkflowID, Name, Description) VALUES ('84210799-7B81-4914-90BE-477507391D0F', 'F64E0001-4F9A-49F0-BF75-A3B501396946', 'Requestor', 'The requestor of the workflow')");
            Sql(@"INSERT INTO WorkflowRoles (ID, WorkflowID, Name, Description) VALUES ('2F1D765F-11C4-4F95-A879-480AB968008B', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Requestor', 'The requestor of the workflow')");
            Sql(@"INSERT INTO WorkflowRoles (ID, WorkflowID, Name, Description) VALUES ('6443B7B5-C507-48F9-A059-A366062D49AE', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8', 'Requestor', 'The requestor of the workflow')");
        
            //AddColumn("dbo.WorkflowRoles", "IsRequestCreator", c => c.Boolean(nullable: false));
            //DropColumn("dbo.WorkflowRoles", "IsRequestor");
        }
        
        public override void Down()
        {
            RenameColumn("dbo.WorkflowRoles", "IsRequestCreator", "IsRequestor");

            Sql(@"update [dbo].[WorkflowRoles] set Name='Requestor' WHERE Name='Request Creator'");
            Sql(@"update [dbo].[WorkflowRoles] set Description='The Requestor of the workflow.' WHERE Description='The Request Creator for the workflow.'");
            Sql(@"update [dbo].[WorkflowRoles] set Description='The Requestor of the workflow' WHERE Description='The Request Creator for the workflow'");

            Sql(@"DELETE FROM [dbo].[WorkflowRoles] WHERE ID='84210799-7B81-4914-90BE-477507391D0F'");
            Sql(@"DELETE FROM [dbo].[WorkflowRoles] WHERE ID='2F1D765F-11C4-4F95-A879-480AB968008B'");
            Sql(@"DELETE FROM [dbo].[WorkflowRoles] WHERE ID='6443B7B5-C507-48F9-A059-A366062D49AE'");

            //AddColumn("dbo.WorkflowRoles", "IsRequestor", c => c.Boolean(nullable: false));
            //DropColumn("dbo.WorkflowRoles", "IsRequestCreator");
        }
    }
}
