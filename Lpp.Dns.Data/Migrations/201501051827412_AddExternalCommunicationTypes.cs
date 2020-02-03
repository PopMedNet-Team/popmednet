namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExternalCommunicationTypes : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('90B85A44-D71D-4DEA-B8D8-09C88CA46C75', 'External Communication - Integration Disabled', 'Aqueduct integration is disabled for this network.')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('F9DDB4D0-5DC0-4E6C-B4A3-7235D847EFA1', 'External Communication - Integration Started', 'Aqueduct integration has been started.')");
            Sql("INSERT INTO Events (ID, Name, Description) VALUES ('692782BF-E777-4924-B3D1-8866E0B55839', 'External Communication - Integration Failed', 'The Aqueduct integration failed to initialize.')");
        }
        
        public override void Down()
        {
            Sql("Delete from Events where ID = '90B85A44-D71D-4DEA-B8D8-09C88CA46C75'");
            Sql("Delete from Events where ID = 'F9DDB4D0-5DC0-4E6C-B4A3-7235D847EFA1'");
            Sql("Delete from Events where ID = '692782BF-E777-4924-B3D1-8866E0B55839'");
        }
    }
}
