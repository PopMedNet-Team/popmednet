namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CloneTemplatesUsedByMultipleRequestTypes : DbMigration
    {
        public override void Up()
        {
            /** With the integration of the template designer into the requesttype details page the relationship must now be 1:1. Clone any templates that are used by more the one requesttype and reassociate. **/

            //create column for RequestTypeID, CloneRoot in Templates
            Sql("ALTER TABLE [Templates] ADD RequestTypeID uniqueidentifier null, CloneRoot bit null, ParentTemplateID uniqueidentifier null");

            // set cloneroot - true for each template that is used more than once
            Sql("UPDATE [Templates] SET CloneRoot = 1 WHERE (SELECT COUNT(*) FROM RequestTypes rt WHERE rt.TemplateID = Templates.ID) > 1");

            // clone original template for each associated request type with the requesttype id set
            Sql(@"INSERT INTO [Templates] (ID, [Name], [Description], CreatedByID, CreatedOn, Data, [Type], Notes, QueryType, RequestTypeID, ParentTemplateID) SELECT dbo.NewSqlGuid() as ID, LEFT(rt.Name, 255) [Name], t.Description, t.CreatedByID, t.CreatedOn, t.Data, t.Type, t.Notes, t.QueryType, rt.ID as RequestTypeID, t.ID as ParentTemplateID FROM Templates t
JOIN RequestTypes rt ON t.ID = rt.TemplateID
WHERE (SELECT COUNT(*) FROM RequestTypes rt2 WHERE rt2.TemplateID = t.ID) > 1");

            // clone the permissions from the parent template
            Sql(@"INSERT INTO AclTemplates (SecurityGroupID, PermissionID, TemplateID, Allowed, Overridden)
SELECT a.SecurityGroupID, a.PermissionID, t.ID as TemplateID, a.Allowed, a.Overridden FROM Templates t
JOIN AclTemplates a ON t.ParentTemplateID = a.TemplateID
WHERE t.ParentTemplateID IS NOT NULL");

            // update the requesttypes based on the template requesttypeid
            Sql("UPDATE RequestTypes SET TemplateID = t.ID FROM RequestTypes rt JOIN Templates t ON rt.ID = t.RequestTypeID WHERE t.RequestTypeID IS NOT NULL");

            // delete all templates marked as clone root
            Sql("DELETE FROM Templates WHERE CloneRoot = 1");

            // drop column RequestTypeID and cloneroot from Templates
            Sql("ALTER TABLE Templates DROP COLUMN ParentTemplateID");
            Sql("ALTER TABLE Templates DROP COLUMN RequestTypeID");
            Sql("ALTER TABLE Templates DROP COLUMN CloneRoot");
        }
        
        public override void Down()
        {
        }
    }
}
