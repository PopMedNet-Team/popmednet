namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResetAdapterIDForDataMartIfSetToQueryComposer : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE DataMarts SET AdapterID = NULL WHERE AdapterID = '455C772A-DF9B-4C6B-A6B0-D4FD4DD98488'");
        }
        
        public override void Down()
        {
        }
    }
}
