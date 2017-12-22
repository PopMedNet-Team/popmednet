namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTerms20150312 : DbMigration
    {
        public override void Up()
        {
            Sql("Insert into Terms(ID, Name, [Description]) Values ('AE7269B1-E470-4CC5-A079-0000A45A0017', 'Date of Birth', 'The date of birth of the patient.')");
            Sql("Insert into Terms(ID, Name, [Description]) Values ('0F3A0AE0-6D6D-44AA-A146-0000A45A0017', 'Time of Birth', 'The time of birth of the patient.')");
            Sql("Insert into Terms(ID, Name, [Description]) Values ('8BC627CA-5729-4E7A-9702-0000A45A0018', 'Height', 'The height of the patient.')");
            Sql("Insert into Terms(ID, Name, [Description]) Values ('3B0ED310-DDE9-4836-9857-0000A45A0018', 'Weight', 'The weight of the patient.')");
        }
        
        public override void Down()
        {
            Sql("Delete from Terms Where ID = 'AE7269B1-E470-4CC5-A079-0000A45A0017'");
            Sql("Delete from Terms Where ID = '0F3A0AE0-6D6D-44AA-A146-0000A45A0017'");
            Sql("Delete from Terms Where ID = '8BC627CA-5729-4E7A-9702-0000A45A0018'");
            Sql("Delete from Terms Where ID = '3B0ED310-DDE9-4836-9857-0000A45A0018'");
        }
    }
}
