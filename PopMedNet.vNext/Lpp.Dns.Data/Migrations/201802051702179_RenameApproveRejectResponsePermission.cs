namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameApproveRejectResponsePermission : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE [Permissions] SET [Name] = 'Approve, Reject, and Resubmit Responses', [Description] = 'Allows the user to approve, reject, or resubmit responses that are awaiting approval' WHERE ID = 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6'");
        }
        
        public override void Down()
        {
            Sql("UPDATE [Permissions] SET [Name] = 'Approve/Reject Response', [Description] = 'Allows the user to approve or reject responses' WHERE ID = 'A58791B5-E8AF-48D0-B9CD-ED0B54E564E6'");
        }
    }
}
