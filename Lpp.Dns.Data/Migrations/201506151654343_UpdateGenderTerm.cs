namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateGenderTerm : DbMigration
    {
        public override void Up()
        {
            Sql("update Terms Set Name = 'Sex', Description = 'The reported Sex of the patient subject.' where ID = '71B4545C-345B-48B2-AF5E-F84DC18E4E1A'");
        }
        
        public override void Down()
        {
        }
    }
}
