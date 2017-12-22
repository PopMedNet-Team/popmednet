namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixRegistryACLTypos : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE Permissions Set Description = 'Allows the selected security group to manage security of the selected registry.' Where ID = '268F7007-E95F-435C-8FAF-0B9FBC9CA997' ");
            Sql("UPDATE Permissions Set Description = 'Allows the selected security group to edit the selected registry.' Where ID = '2B42D2E7-F7A7-4119-9CC5-22991DC12AD3' ");
            Sql("UPDATE Permissions Set Description = 'Allows the selected security group to view the selected registry.' Where ID = '2CCB0FC2-006D-4345-895E-5DD2C6C8C791' ");
            Sql("UPDATE Permissions Set Description = 'Allows the selected security group to delete the selected registry.' Where ID = '2C019782-1B9D-48F8-9FCD-AC44BC6FD97B' ");
        }
        
        public override void Down()
        {
            Sql("UPDATE Permissions Set Description = 'Allows the selected seucrity group to manage seurity of the selected registry.' Where ID = '268F7007-E95F-435C-8FAF-0B9FBC9CA997' ");
            Sql("UPDATE Permissions Set Description = 'Allows the selected seucrity group to edit the selected registry.' Where ID = '2B42D2E7-F7A7-4119-9CC5-22991DC12AD3' ");
            Sql("UPDATE Permissions Set Description = 'Allows the selected seucrity group to view the selected registry.' Where ID = '2CCB0FC2-006D-4345-895E-5DD2C6C8C791' ");
            Sql("UPDATE Permissions Set Description = 'Allows the selected seucrity group to delete the selected registry.' Where ID = '2C019782-1B9D-48F8-9FCD-AC44BC6FD97B' ");
        }
    }
}
