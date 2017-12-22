namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePCORIAdapterDisplayName : DbMigration
    {
        public override void Up()
        {
            Sql(@"Update DataModels Set Name = 'PCORnet CDM' Where ID = '85EE982E-F017-4BC4-9ACD-EE6EE55D2446'");
        }
        
        public override void Down()
        {
        }
    }
}
