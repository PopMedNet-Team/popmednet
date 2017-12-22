namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTermsAndLookupLists : DbMigration
    {
        public override void Up()
        {
            Sql("Insert Into LookupLists(ListID, ListName) Values(14, 'ICD10 Procedure')");
            Sql("Insert Into LookupLists(ListID, ListName) Values(15, 'ICD10 Diagnosis')");
            Sql("Insert Into LookupLists(ListID, ListName) Values(16, 'National Drug Code')");
            Sql("Insert Into LookupLists(ListID, ListName) Values(17, 'Revenue Code')");

            Sql("insert into Terms(ID, Name, [Description]) Values('BD3C3913-3B1D-4AC3-92B2-976BC4580459', 'Biobank', 'If bio samples are available.')");
            Sql("insert into Terms(ID, Name, [Description]) Values('93156E20-5124-4465-9A59-112782AD7F83', 'Chart Available', 'If patient charts are available.')");
            Sql("insert into Terms(ID, Name, [Description]) Values('2B298144-2ED5-473C-BE78-A0BA7E83ACF6', 'National Drug Codes', ' the reported national drug code.')");
            Sql("insert into Terms(ID, Name, [Description]) Values('FD13B2F0-034F-4A0A-AE7C-4E772F220ABF', 'Encounter Types', 'The type of the encounter.')");
            Sql("insert into Terms(ID, Name, [Description]) Values('7DE891A0-8BDA-4CAE-AD5B-B9A5E6E36998', 'Enrollment', 'The enrollment period.')");
            Sql("insert into Terms(ID, Name, [Description]) Values('C1DB7229-4FF6-4BF4-B8CA-672D60D64C75', 'Revenue Codes', 'The reported revenue code of the encounter.')");

            Sql("insert into Terms(ID, Name, [Description]) Values('76F41459-D013-433B-8CBC-EEE496ED6C67', 'Blood Pressure - ESP', 'The ESP Model Blood Pressure')");
            Sql("insert into Terms(ID, Name, [Description]) Values('61D5B7D5-4379-4EB9-A9E5-B3B75553F822', 'Blood Pressure - PCORI', 'The PCORI Model Blood Pressure')");
            Sql("insert into Terms(ID, Name, [Description]) Values('5BBBFF21-5C70-4FD7-A351-EF0555BBCB48', 'Blood Pressure - Sentinel', 'The Sentinel Model Blood Pressure')");
            Sql("insert into Terms(ID, Name, [Description]) Values('AA336D3C-CE0E-4F63-8DDC-F957DA9109EC', 'Age - ESP', 'Age Range for ESP adapter')");
            Sql("insert into Terms(ID, Name, [Description]) Values('9DA5DDCE-9BB7-4CD9-B706-15D92B09E8C2', 'Age - PCORI', 'Age Range for PCORI adapter')");
            Sql("insert into Terms(ID, Name, [Description]) Values('8B48770F-37CD-49ED-88D2-B3D6390BC155', 'Age - Sentinel', 'Age Range for Sentinel adapter')");
            Sql("insert into Terms(ID, Name, [Description]) Values('154752AB-10D1-4E6B-80B3-7FDADF3DCA7F', 'Gender - ESP', 'Gender for ESP adapter')");
            Sql("insert into Terms(ID, Name, [Description]) Values('CF3065BB-1D06-4519-B59F-5332C3D1E7CA', 'Gender - PCORI', 'Gender for PCORI adapter')");
            Sql("insert into Terms(ID, Name, [Description]) Values('F173B6F8-CB6A-4E96-A162-174CD0B78514', 'Gender - Sentinel', 'Gender for Sentinel adapter')");
            Sql("insert into Terms(ID, Name, [Description]) Values('A616DF59-C012-4BCF-8397-5CB2863DED75', 'Postal Code - ESP', 'Postal code for ESP adapter')");
            Sql("insert into Terms(ID, Name, [Description]) Values('5BBC2CF3-99B4-4ED6-B35D-A809BCD79582', 'Postal Code - PCORI', 'Postal code for PCORI adapter')");
            Sql("insert into Terms(ID, Name, [Description]) Values('32A45AE4-3C7D-43D1-B9D0-7E666BDB3632', 'Postal Code - Sentinel', 'Postal code for Sentinel adapter')");
            Sql("insert into Terms(ID, Name, [Description]) Values('D7C58DFC-971B-47A7-8111-9B793ED4A6C1', 'Encounter Type - ESP', 'Encounter type for ESP adapter')");
            Sql("insert into Terms(ID, Name, [Description]) Values('EF7D5BF3-404C-4034-A7C1-F32B08A24F0B', 'Encounter Type - PCORI', 'Encounter type for PCORI adapter')");
            Sql("insert into Terms(ID, Name, [Description]) Values('A701CE17-B3B8-4646-B79B-2FCB80D36526', 'Encounter Type - Sentinel', 'Encounter type for Sentinel adapter')");
        }
        
        public override void Down()
        {
            Sql("Delete from LookupLists where ListId = 14");
            Sql("Delete from LookupLists where ListId = 15");
            Sql("Delete from LookupLists Where ListId = 16");
            Sql("Delete from LookupLists Where ListID = 17");

            Sql("Delete from Terms where ID = 'BD3C3913-3B1D-4AC3-92B2-976BC4580459'");
            Sql("Delete from Terms where ID = '93156E20-5124-4465-9A59-112782AD7F83'");
            Sql("Delete from Terms where ID = '2B298144-2ED5-473C-BE78-A0BA7E83ACF6'");
            Sql("Delete from Terms where ID = 'FD13B2F0-034F-4A0A-AE7C-4E772F220ABF'");
            Sql("Delete from Terms where ID = '7DE891A0-8BDA-4CAE-AD5B-B9A5E6E36998'");
            Sql("Delete from Terms where ID = 'C1DB7229-4FF6-4BF4-B8CA-672D60D64C75'");

            Sql("Delete from Terms where ID = '76F41459-D013-433B-8CBC-EEE496ED6C67'");
            Sql("Delete from Terms where ID = '61D5B7D5-4379-4EB9-A9E5-B3B75553F822'");
            Sql("Delete from Terms where ID = '5BBBFF21-5C70-4FD7-A351-EF0555BBCB48'");
            Sql("Delete from Terms where ID = 'AA336D3C-CE0E-4F63-8DDC-F957DA9109EC'");
            Sql("Delete from Terms where ID = '9DA5DDCE-9BB7-4CD9-B706-15D92B09E8C2'");
            Sql("Delete from Terms where ID = '8B48770F-37CD-49ED-88D2-B3D6390BC155'");
            Sql("Delete from Terms where ID = '154752AB-10D1-4E6B-80B3-7FDADF3DCA7F'");
            Sql("Delete from Terms where ID = 'CF3065BB-1D06-4519-B59F-5332C3D1E7CA'");
            Sql("Delete from Terms where ID = 'F173B6F8-CB6A-4E96-A162-174CD0B78514'");
            Sql("Delete from Terms where ID = 'A616DF59-C012-4BCF-8397-5CB2863DED75'");
            Sql("Delete from Terms where ID = '5BBC2CF3-99B4-4ED6-B35D-A809BCD79582'");
            Sql("Delete from Terms where ID = '32A45AE4-3C7D-43D1-B9D0-7E666BDB3632'");
            Sql("Delete from Terms where ID = 'D7C58DFC-971B-47A7-8111-9B793ED4A6C1'");
            Sql("Delete from Terms where ID = 'EF7D5BF3-404C-4034-A7C1-F32B08A24F0B'");
            Sql("Delete from Terms where ID = 'A701CE17-B3B8-4646-B79B-2FCB80D36526'");
        }
    }
}
