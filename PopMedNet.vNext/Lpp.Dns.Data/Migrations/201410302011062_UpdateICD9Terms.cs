namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateICD9Terms : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE Terms SET [Name] = 'HCPCS Procedure Codes', [Description] = 'The reported HCPCS Procedure codes of the encounter.' WHERE ID = '096A0001-73B4-405D-B45F-A3CA014C6E7D'");
            Sql("UPDATE Terms SET [Name] = 'ICD-9 Diagnosis Codes (3 digit)', [Description] = 'The reported ICD-9 Diagnosis codes (3 digit) of the encounter.' WHERE ID = '5E5020DC-C0E4-487F-ADF2-45431C2B7695'");
            Sql("UPDATE Terms SET [Name] = 'ICD-9 Procedure Codes (3 digit)', [Description] = 'The reported ICD-9 Procedure codes (3 digit) of the encounter.' WHERE ID = 'E1CC0001-1D9A-442A-94C4-A3CA014C7B94'");
            Sql(@"IF NOT EXISTS(SELECT NULL FROM Terms WHERE ID = 'D0800001-2810-48ED-96B9-A3D40146BAAE')
	INSERT INTO Terms (ID, [Name], [Description], [Type]) VALUES ('D0800001-2810-48ED-96B9-A3D40146BAAE','ICD-9 Diagnosis Codes (4 digit)','The reported ICD-9 Diagnosis codes (4 digit) of the encounter.',3)");
            Sql(@"IF NOT EXISTS(SELECT NULL FROM Terms WHERE ID = '80750001-6C3B-4C2D-90EC-A3D40146C26D')
	INSERT INTO Terms (ID, [Name], [Description], [Type]) VALUES ('80750001-6C3B-4C2D-90EC-A3D40146C26D','ICD-9 Diagnosis Codes (5 digit)','The reported ICD-9 Diagnosis codes (5 digit) of the encounter.',3)");
            Sql(@"IF NOT EXISTS(SELECT NULL FROM Terms WHERE ID = '9E870001-1D48-4AA3-8889-A3D40146CCB3')
	INSERT INTO Terms (ID, [Name], [Description], [Type]) VALUES ('9E870001-1D48-4AA3-8889-A3D40146CCB3','ICD-9 Procedure Codes (4 digit)','The reported ICD-9 Procedure codes (4 digit) of the encounter.',3)");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM Terms WHERE ID IN ('9E870001-1D48-4AA3-8889-A3D40146CCB3','80750001-6C3B-4C2D-90EC-A3D40146C26D','D0800001-2810-48ED-96B9-A3D40146BAAE')");
            Sql("UPDATE Terms SET [Name] = 'ICD-9 Diagnosis Codes', [Description] = 'The reported ICD-9 Diagnosis codes of the encounter.' WHERE ID = '5E5020DC-C0E4-487F-ADF2-45431C2B7695'");
            Sql("UPDATE Terms SET [Name] = 'ICD-9 Procedure Codes', [Description] = 'The reported ICD-9 Procedure codes of the encounter.' WHERE ID = 'E1CC0001-1D9A-442A-94C4-A3CA014C7B94'");
        }
    }
}
