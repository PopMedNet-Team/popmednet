namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveNewRequestDraftLocation : DbMigration
    {
        public override void Up()
        {

            Sql(@"DELETE FROM ProjectDataMartEvents WHERE EventID = '6549439E-E3E4-4F4C-92CF-88FB81FF8869'");

        }
        
        public override void Down()
        {
        }
    }
}
