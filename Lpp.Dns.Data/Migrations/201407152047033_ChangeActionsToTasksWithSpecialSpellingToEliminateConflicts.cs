namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeActionsToTasksWithSpecialSpellingToEliminateConflicts : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Actions", newName: "Tasks");
            RenameTable(name: "dbo.ActionUsers", newName: "TaskUsers");
            RenameTable(name: "dbo.ActionReferences", newName: "TaskReferences");
            RenameColumn(table: "dbo.TaskReferences", name: "ActionID", newName: "TaskID");
            RenameColumn(table: "dbo.TaskUsers", name: "ActionID", newName: "TaskID");
            RenameIndex(table: "dbo.TaskReferences", name: "IX_ActionID", newName: "IX_TaskID");
            RenameIndex(table: "dbo.TaskUsers", name: "IX_ActionID", newName: "IX_TaskID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.TaskUsers", name: "IX_TaskID", newName: "IX_ActionID");
            RenameIndex(table: "dbo.TaskReferences", name: "IX_TaskID", newName: "IX_ActionID");
            RenameColumn(table: "dbo.TaskUsers", name: "TaskID", newName: "ActionID");
            RenameColumn(table: "dbo.TaskReferences", name: "TaskID", newName: "ActionID");
            RenameTable(name: "dbo.TaskReferences", newName: "ActionReferences");
            RenameTable(name: "dbo.TaskUsers", newName: "ActionUsers");
            RenameTable(name: "dbo.Tasks", newName: "Actions");
        }
    }
}
