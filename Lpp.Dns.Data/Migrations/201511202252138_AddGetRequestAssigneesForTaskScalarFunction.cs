namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGetRequestAssigneesForTaskScalarFunction : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE FUNCTION [dbo].[GetRequestAssigneesForTask](@TaskID uniqueidentifier, @delimiter nvarchar(max)) RETURNS nvarchar(max) 
AS
BEGIN
	/** Get all the request users for a specific task. **/
	DECLARE @assignees nvarchar(max) = 
		(SELECT (wr.Name + ': ' + u.Username + @delimiter)  AS [text()] FROM Tasks t
		JOIN TaskReferences tr ON tr.TaskID = t.ID AND tr.Type = 2
		JOIN RequestUsers ru ON tr.ItemID = ru.RequestID
		JOIN Users u on ru.UserID = u.ID
		JOIN WorkflowRoles wr ON ru.WorkflowRoleID = wr.ID
		WHERE t.ID = @TaskID
		FOR XML PATH(''), TYPE).value('(./text())[1]','NVARCHAR(MAX)')

	DECLARE @valueLength int = LEN(@assignees)
	DECLARE @delimiterLength int = LEN(@delimiter)
	IF (@valueLength > 0 AND @delimiterLength > 0)
		SET @assignees = SUBSTRING(@assignees, 0, @valueLength - @delimiterLength + 1)
	
	RETURN @assignees
END");
        }
        
        public override void Down()
        {
            Sql("DROP FUNCTION [dbo].[GetRequestAssigneesForTask]");
        }
    }
}
