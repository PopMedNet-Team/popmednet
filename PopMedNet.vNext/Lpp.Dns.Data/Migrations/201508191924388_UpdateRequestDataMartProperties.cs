namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRequestDataMartProperties : DbMigration
    {
        public override void Up()
        {
            //empty migration to keep the migration hash consitent. Added existing column UpdatedOn to RequestDataMart
        }
        
        public override void Down()
        {
            
        }
    }
}
