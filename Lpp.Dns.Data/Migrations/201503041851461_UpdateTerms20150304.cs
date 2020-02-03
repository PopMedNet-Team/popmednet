namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTerms20150304 : DbMigration
    {
        public override void Up()
        {
            Sql("Insert into Terms(ID, Name, [Description]) Values ('755CCD61-02CD-4BF3-95FD-0000A44A0027', 'Blood Pressure', 'The diastolic and systolic blood pressure.')");
            Sql("Insert into Terms(ID, Name, [Description]) Values ('9902A45D-1E1C-4327-8D40-0000A44A004A', 'ICD-10 Procedure (3 digit)', 'The reported ICD-10 Procedure codes (3 Digit) of the encounter.')");
            Sql("Insert into Terms(ID, Name, [Description]) Values ('F9A2A021-D203-49C4-A518-0000A44A018A', 'ICD-10 Procedure (4 digit)', 'The reported ICD-10 Procedure codes (4 Digit) of the encounter.')");
            Sql("Insert into Terms(ID, Name, [Description]) Values ('A257DA68-9557-4D6A-AEBD-541AA9BDD145', 'Centre', 'Center(s) visited by the patient.')");
            Sql("Insert into Terms(ID, Name, [Description]) Values ('342C354B-9ECC-479B-BE61-1770E4B44675', 'Tobacco Use', 'The frequency of tobacco usage of the patient.')");
            Sql("Insert into Terms(ID, Name, [Description]) Values ('A54B0CFF-63F2-46D6-A6C8-41F884A888CD', 'Relative Time', 'A tool for describing a series of events, starting an an index event.')");
            Sql("Insert into Terms(ID, Name, [Description]) Values ('E5E68BBD-7FD4-4442-93DB-3E252864DD6E', 'T-SQL', 'A T-SQL script to execute on the data source.')");
        }

        public override void Down()
        {
            Sql("Delete from Terms Where ID = '755CCD61-02CD-4BF3-95FD-0000A44A0027'");
            Sql("Delete from Terms Where ID = '9902A45D-1E1C-4327-8D40-0000A44A004A'");
            Sql("Delete from Terms Where ID = 'F9A2A021-D203-49C4-A518-0000A44A018A'");
            Sql("Delete from Terms Where ID = 'A257DA68-9557-4D6A-AEBD-541AA9BDD145'");
            Sql("Delete from Terms Where ID = '342C354B-9ECC-479B-BE61-1770E4B44675'");
            Sql("Delete from Terms Where ID = 'A54B0CFF-63F2-46D6-A6C8-41F884A888CD'");
            Sql("Delete from Terms Where ID = 'E5E68BBD-7FD4-4442-93DB-3E252864DD6E'");
        }
    }
}
