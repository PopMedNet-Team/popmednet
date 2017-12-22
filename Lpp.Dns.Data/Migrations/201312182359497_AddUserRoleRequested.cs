namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserRoleRequested : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "RoleRequested", c => c.String(maxLength:255));

            //re-create the view with the additional columns
            Sql("DROP VIEW vwUsers");
            Sql(@"
            CREATE view [dbo].[vwUsers] as
	select u.*, cast( case when isnull(u.isdeleted,0) = 1 or isnull(o.isdeleted,0) = 1 then 1 else 0 end as bit ) as EffectiveIsDeleted
	from users u inner join organizations o on u.organizationid = o.organizationid
            ");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "RoleRequested");
        }
    }
}
