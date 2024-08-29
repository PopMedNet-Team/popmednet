namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class FixSampleRequestTypeID : DbMigration
    {
        public override void Up()
        {
            Sql(@"update [dbo].requesttypemodels
  set RequestTypeID='B8F2B52E-CBF9-4EE8-94EB-FC226E2426B6'
  where DataModelID='C59F449C-230C-4A6D-B37F-AB62C60ED471'");
            Sql(@"delete from [dbo].requesttypes where id='F985DBD9-DA7E-41B4-8FBD-2A73B7FCF6DD'");
        }

        public override void Down()
        {
            Sql(@"update [dbo].requesttypemodels
  set RequestTypeID='F985DBD9-DA7E-41B4-8FBD-2A73B7FCF6DD'
  where DataModelID='C59F449C-230C-4A6D-B37F-AB62C60ED471'");
            Sql(@"insert into [dbo].requesttypes (id, name, metadata, postprocess, addfiles, requiresprocessing) values('F985DBD9-DA7E-41B4-8FBD-2A73B7FCF6DD, 0, 0, 1, 0)");
        }
    }

}
