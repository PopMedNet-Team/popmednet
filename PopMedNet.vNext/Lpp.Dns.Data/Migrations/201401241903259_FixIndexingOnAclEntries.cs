namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixIndexingOnAclEntries : DbMigration
    {
        public override void Up()
        {
            Sql(@"DELETE FROM AclEntries WHERE EXISTS(SELECT NULL FROM ACLEntries AS ACL WHERE AclEntries.TargetId = Acl.TargetId AND AclEntries.SubjectID = acl.SubjectId AND AclEntries.PrivilegeId = acl.PrivilegeId AND AclEntries.Id <> ACL.ID and Acl.ID > AclEntries.ID)");

            DropIndex("dbo.AclEntries", "_dta_index_AclEntries_16_1062294844__K2_K4_K3_K7_6");
            DropIndex("dbo.AclEntries", "_dta_index_AclEntries_16_1062294844__K4_K2_K3_K7");
            DropIndex("dbo.AclEntries", "_dta_index_AclEntries_16_1062294844__K4_K2_K3_K7_6");
            DropIndex("dbo.AclEntries", "_dta_index_AclEntries_16_1062294844__K4_K3_K2_K7_6");
            DropIndex("dbo.AclEntries", "AclEntries_TargetId");

            Sql(@"DECLARE @PKC varchar(max)
                  SET @PKC = (SELECT NAME FROM SYS.KEY_CONSTRAINTS WHERE [TYPE]='PK' AND NAME LIKE 'PK[_][_]AclEntri%')
                  IF EXISTS(SELECT @PKC)
                  BEGIN
                    EXEC('ALTER TABLE AclEntries DROP CONSTRAINT ' + @PKC)
                  END
                ");

            Sql(@"ALTER TABLE AclEntries ADD CONSTRAINT PK_AclEntries_TargetId_SubjectId_PrivilegeId_Order_Allow PRIMARY KEY (TargetId, SubjectId, PrivilegeId, [Order], Allow)");
        }
        
        public override void Down()
        {
        }
    }
}
