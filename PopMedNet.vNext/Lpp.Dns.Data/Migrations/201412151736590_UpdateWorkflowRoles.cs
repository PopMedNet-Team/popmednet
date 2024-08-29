namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateWorkflowRoles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkflowRoles", "IsRequestor", c => c.Boolean(nullable: false, defaultValue:false));

            Sql(@"UPDATE WorkflowRoles SET [Name] = 'Scientific Lead', [Description] = 'The Scientific Lead for the workflow.' WHERE ID = 'B96BD827-3942-4DF0-888A-5927751E8EF1'");
            
            //Default workflow
            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowRoles WHERE ID = 'B96BD807-3942-4DF0-888A-5927751E8EF1')
	INSERT INTO WorkflowRoles (ID, WorkflowID, [Name], [Description]) VALUES ('B96BD807-3942-4DF0-888A-5927751E8EF1', 'F64E0001-4F9A-49F0-BF75-A3B501396946', 'Primary Analyst', 'THe Primary Analyst of the workflow.')
IF NOT EXISTS(SELECT NULL FROM WorkflowRoles WHERE ID = 'B96BD817-3942-4DF0-888A-5927751E8EF1')
	INSERT INTO WorkflowRoles (ID, WorkflowID, [Name], [Description]) VALUES ('B96BD817-3942-4DF0-888A-5927751E8EF1', 'F64E0001-4F9A-49F0-BF75-A3B501396946', 'Secondary Analyst', 'The Secondary Analyst of the workflow.')
IF NOT EXISTS(SELECT NULL FROM WorkflowRoles WHERE ID = 'B96BD827-3942-4DF0-888A-5927751E8EF1')
	INSERT INTO WorkflowRoles (ID, WorkflowID, [Name], [Description]) VALUES ('B96BD827-3942-4DF0-888A-5927751E8EF1', 'F64E0001-4F9A-49F0-BF75-A3B501396946', 'Scientific Lead', 'The Scientific Lead of the workflow.')
IF NOT EXISTS(SELECT NULL FROM WorkflowRoles WHERE ID = 'B96BD897-3942-4DF0-888A-5927751E8EF1')
	INSERT INTO WorkflowRoles (ID, WorkflowID, [Name], [Description]) VALUES ('B96BD897-3942-4DF0-888A-5927751E8EF1', 'F64E0001-4F9A-49F0-BF75-A3B501396946', 'Requestor', 'The Requestor of the workflow.')");
            
            //Modular Program workflow
            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowRoles WHERE ID = '018E0001-8715-4159-AB4A-A402011F2DBF')
	INSERT INTO WorkflowRoles (ID, WorkflowID, [Name], [Description]) VALUES ('018E0001-8715-4159-AB4A-A402011F2DBF', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Primary Analyst', 'THe Primary Analyst of the workflow.')
IF NOT EXISTS(SELECT NULL FROM WorkflowRoles WHERE ID = 'A2320001-A251-4A8C-857A-A402011F437A')
	INSERT INTO WorkflowRoles (ID, WorkflowID, [Name], [Description]) VALUES ('A2320001-A251-4A8C-857A-A402011F437A', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Secondary Analyst', 'The Secondary Analyst of the workflow.')
IF NOT EXISTS(SELECT NULL FROM WorkflowRoles WHERE ID = '81B00001-C01C-49DA-9BE2-A402011F437A')
	INSERT INTO WorkflowRoles (ID, WorkflowID, [Name], [Description]) VALUES ('81B00001-C01C-49DA-9BE2-A402011F437A', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Scientific Lead', 'The Scientific Lead of the workflow.')
IF NOT EXISTS(SELECT NULL FROM WorkflowRoles WHERE ID = '06460001-288E-4532-833D-A402011F437A')
	INSERT INTO WorkflowRoles (ID, WorkflowID, [Name], [Description]) VALUES ('06460001-288E-4532-833D-A402011F437A', '5CE55AF8-9737-4E7A-8E0A-8C483B23EA1D', 'Requestor', 'The Requestor of the workflow.')");
            
            //Summary Query workflow
            Sql(@"IF NOT EXISTS(SELECT NULL FROM WorkflowRoles WHERE ID = '0EF40001-83DA-4968-8120-A402011F56F4')
	INSERT INTO WorkflowRoles (ID, WorkflowID, [Name], [Description]) VALUES ('0EF40001-83DA-4968-8120-A402011F56F4', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8', 'Primary Analyst', 'THe Primary Analyst of the workflow.')
IF NOT EXISTS(SELECT NULL FROM WorkflowRoles WHERE ID = '892D0001-444E-437A-9A8C-A402011F56F4')
	INSERT INTO WorkflowRoles (ID, WorkflowID, [Name], [Description]) VALUES ('892D0001-444E-437A-9A8C-A402011F56F4', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8', 'Secondary Analyst', 'The Secondary Analyst of the workflow.')
IF NOT EXISTS(SELECT NULL FROM WorkflowRoles WHERE ID = '41190001-D26D-4775-996D-A402011F56F4')
	INSERT INTO WorkflowRoles (ID, WorkflowID, [Name], [Description]) VALUES ('41190001-D26D-4775-996D-A402011F56F4', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8', 'Scientific Lead', 'The Scientific Lead of the workflow.')
IF NOT EXISTS(SELECT NULL FROM WorkflowRoles WHERE ID = 'FDB30001-028D-4E92-9357-A402011F56F4')
	INSERT INTO WorkflowRoles (ID, WorkflowID, [Name], [Description]) VALUES ('FDB30001-028D-4E92-9357-A402011F56F4', '7A82FE34-BE6B-40E5-AABF-B40A3DBE73B8', 'Requestor', 'The Requestor of the workflow.')");

            //update existing requestor role definitions
            Sql(@"UPDATE WorkflowRoles SET IsRequestor = 1 WHERE ID = 'B96BD897-3942-4DF0-888A-5927751E8EF1' OR ID = '06460001-288E-4532-833D-A402011F437A' OR ID = 'FDB30001-028D-4E92-9357-A402011F56F4'");
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkflowRoles", "IsRequestor");
        }
    }
}
