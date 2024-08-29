namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDataCheckerQueryTypeTerm : DbMigration
    {
        public override void Up()
        {
            //Delete the query type term.
            Sql("DELETE FROM TemplateTerms WHERE TermID = '1F065B02-5BF3-4340-A412-84465C9B164C'");
            Sql("DELETE FROM DataModelSupportedTerms WHERE TermID = '1F065B02-5BF3-4340-A412-84465C9B164C'");
            Sql("DELETE FROM RequestTypeTerms WHERE TermID = '1F065B02-5BF3-4340-A412-84465C9B164C'");
            Sql("DELETE FROM Terms WHERE ID = '1F065B02-5BF3-4340-A412-84465C9B164C'");
        }
        
        public override void Down()
        {
            //no down required, the term would automatically get re-registered with the database, and datamodel on start up of the api if added back.
        }
    }
}
