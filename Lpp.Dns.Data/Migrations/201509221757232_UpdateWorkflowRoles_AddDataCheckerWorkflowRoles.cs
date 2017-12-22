namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateWorkflowRoles_AddDataCheckerWorkflowRoles : DbMigration
    {
        public override void Up()
        {
            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowRoles WHERE ID = '0473C16B-2F4B-40A2-B01A-68093D66FE70')
	                    INSERT INTO WorkflowRoles (ID, WorkflowID, [Name], [Description], IsRequestCreator) VALUES ('0473C16B-2F4B-40A2-B01A-68093D66FE70', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663', 'Primary Analyst', 'The primary analyst of the workflow', 0)
                    IF NOT EXISTS(SELECT NULL FROM WorkflowRoles WHERE ID = 'B5C4CFA7-2C63-4C8B-A981-4BC5A41AAFC7')
	                    INSERT INTO WorkflowRoles (ID, WorkflowID, [Name], [Description], IsRequestCreator) VALUES ('B5C4CFA7-2C63-4C8B-A981-4BC5A41AAFC7', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663', 'Request Creator', 'The Request Creator for the workflow', 1)
                    IF NOT EXISTS(SELECT NULL FROM WorkflowRoles WHERE ID = '1BED75B0-F61E-4BE1-8570-8033D9DB50D1')
	                    INSERT INTO WorkflowRoles (ID, WorkflowID, [Name], [Description], IsRequestCreator) VALUES ('1BED75B0-F61E-4BE1-8570-8033D9DB50D1', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663', 'Requestor', 'The Requestor that initiated the request.', 0)
                    IF NOT EXISTS(SELECT NULL FROM WorkflowRoles WHERE ID = 'DA89E0F7-AB88-4AF3-B16B-78A30D52B5D2')
	                    INSERT INTO WorkflowRoles (ID, WorkflowID, [Name], [Description], IsRequestCreator) VALUES ('DA89E0F7-AB88-4AF3-B16B-78A30D52B5D2', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663', 'Scientific Lead', 'The Scientific Lead for the workflow.', 0)
                    IF NOT EXISTS(SELECT NULL FROM WorkflowRoles WHERE ID = 'B3354EF6-BBA0-4C56-90C9-FDE69CB78362')
	                    INSERT INTO WorkflowRoles (ID, WorkflowID, [Name], [Description], IsRequestCreator) VALUES ('B3354EF6-BBA0-4C56-90C9-FDE69CB78362', '942A2B39-0E9C-4ECE-9E2C-C85DF0F42663', 'Secondary Analyst', 'The secondary analyst of the workflow', 0)");
        }
        
        public override void Down()
        {
        }
    }
}
