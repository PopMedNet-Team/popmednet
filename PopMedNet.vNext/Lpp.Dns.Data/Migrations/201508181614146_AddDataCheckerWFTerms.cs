namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataCheckerWFTerms : DbMigration
    {
        public override void Up()
        {
            Sql("If Not Exists (Select Null From Terms where Id = 'C2BFBB73-8F93-4318-A8C8-73570494FF29') Begin Insert Into Terms(ID, Name, [Description], [Type]) Values('C2BFBB73-8F93-4318-A8C8-73570494FF29', 'Age Distribution', 'DataChecker - Age Distribution Term', 3) End");
            Sql("If Not Exists (Select Null From Terms where Id = '4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B') Begin Insert Into Terms(ID, Name, [Description], [Type]) Values('4DB7FEF6-EFEC-4741-92E8-4C7FC2E6691B', 'Data Partners', 'DataChecker - Data Partners Term', 3) End");
            Sql("If Not Exists (Select Null From Terms where Id = 'E1DA9C98-3F95-4F6E-AEDF-B42E2881DB73') Begin Insert Into Terms(ID, Name, [Description], [Type]) Values('E1DA9C98-3F95-4F6E-AEDF-B42E2881DB73', 'ICD-9 Diagnosis', 'DataChecker - ICD-9 Diagnosis Term', 3) End");
            Sql("If Not Exists (Select Null From Terms where Id = '1CC38753-CD3F-4696-AF5F-9818EABF8AD0') Begin Insert Into Terms(ID, Name, [Description], [Type]) Values('1CC38753-CD3F-4696-AF5F-9818EABF8AD0', 'ICD-9 Procedure', 'DataChecker - ICD-10 Procedure Term', 3) End");
            Sql("If Not Exists (Select Null From Terms where Id = '02A7FD90-AAD2-4986-9C5D-1D22E27AB960') Begin Insert Into Terms(ID, Name, [Description], [Type]) Values('02A7FD90-AAD2-4986-9C5D-1D22E27AB960', 'NDC', 'Datachecker - National Drug Codes Term', 3) End");
            Sql("If Not Exists (Select Null From Terms where Id = '7B18DAC8-08C7-4E83-9100-A4BB49708DAE') Begin Insert Into Terms(ID, Name, [Description], [Type]) Values('7B18DAC8-08C7-4E83-9100-A4BB49708DAE', 'Ethnicity', 'Datachecker - Ethnicity Term', 3) End");
            Sql("If Not Exists (Select Null From Terms where Id = '010414AC-1F22-488F-B948-BE0C673CFEE2') Begin Insert Into Terms(ID, Name, [Description], [Type]) Values('010414AC-1F22-488F-B948-BE0C673CFEE2', 'Race', 'Datachecker - Race Term', 3) End");
            Sql("If Not Exists (Select Null From Terms where Id = '8B6E6E5C-7FE8-4962-AA37-2641D396CFF7') Begin Insert Into Terms(ID, Name, [Description], [Type]) Values('8B6E6E5C-7FE8-4962-AA37-2641D396CFF7', 'Diagnosis PDX', 'Datachecker - Diagnosis PDX Term', 3) End");
            Sql("If Not Exists (Select Null From Terms where Id = '8EDC3AF2-DBB6-462F-91A3-E91F52DAB3FB') Begin Insert Into Terms(ID, Name, [Description], [Type]) Values('8EDC3AF2-DBB6-462F-91A3-E91F52DAB3FB', 'Metadata Completeness', 'Datachecker - Metadata Completeness Term', 3) End");
            Sql("If Not Exists (Select Null From Terms where Id = 'E60113C0-9475-4BD4-92E5-04B063D07B30') Begin Insert Into Terms(ID, Name, [Description], [Type]) Values('E60113C0-9475-4BD4-92E5-04B063D07B30', 'Dispensing RX Amt', 'Datachecker - Dispensing Rx Amount Term', 3) End");
            Sql("If Not Exists (Select Null From Terms where Id = 'B996CA0B-8E82-4E23-BAE0-2507B56464BE') Begin Insert Into Terms(ID, Name, [Description], [Type]) Values('B996CA0B-8E82-4E23-BAE0-2507B56464BE', 'Dispensing RX Sup', 'Datachecker - Dispnensing Rx Supply Term', 3) End");
            Sql("If Not Exists (Select Null From Terms where Id = '0255F535-B529-4753-834A-BE5AFB4B4E5E') Begin Insert Into Terms(ID, Name, [Description], [Type]) Values('0255F535-B529-4753-834A-BE5AFB4B4E5E', 'Encounter', 'Datachecker - Encounter Term', 3) End");
            Sql("If Not Exists (Select Null From Terms where Id = '1F065B02-5BF3-4340-A412-84465C9B164C') Begin Insert Into Terms(ID, Name, [Description], [Type]) Values('1F065B02-5BF3-4340-A412-84465C9B164C', 'Query Type', 'Datachecker - Query Type Term', 3) End");
            Sql("If Not Exists (Select Null From Terms where Id = '484806D5-237A-4414-9621-5E240DAE1CAD') Begin Insert Into Terms(ID, Name, [Description], [Type]) Values('484806D5-237A-4414-9621-5E240DAE1CAD', 'Sex Distribution', 'Datachecker - Sex Distribution Term', 3) End");
            Sql("If Not Exists (Select Null From Terms where Id = '332A6ABA-2F01-4BA9-9A05-78D136403E97') Begin Insert Into Terms(ID, Name, [Description], [Type]) Values('332A6ABA-2F01-4BA9-9A05-78D136403E97', 'Weight Distribution', 'Datachecker - Weight Distribution Term', 3) End");
            Sql("If Not Exists (Select Null From Terms where Id = '4316F265-8A02-44F8-9502-9688E2B0F3A0') Begin Insert Into Terms(ID, Name, [Description], [Type]) Values('4316F265-8A02-44F8-9502-9688E2B0F3A0', 'Height Distribution', 'Datachecker - Height Distribution Term', 3) End");
        }
        
        public override void Down()
        {
        }
    }
}
