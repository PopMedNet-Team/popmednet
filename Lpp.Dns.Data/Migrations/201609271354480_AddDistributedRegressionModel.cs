namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDistributedRegressionModel : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO DataModels(ID, Name,RequiresConfiguration, QueryComposer)VALUES('4C8A25DC-6816-4202-88F4-6D17E72A43BC', 'Distributed Regression',1, 1)");
        }
        
        public override void Down()
        {
            Sql("Delete from DataModels where ID = '4C8A25DC-6816-4202-88F4-6D17E72A43BC'");
        }
    }
}
