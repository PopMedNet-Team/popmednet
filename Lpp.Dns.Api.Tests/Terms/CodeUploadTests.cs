using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Lpp.Utilities;
using Lpp.Dns.DTO.QueryComposer;
using Lpp.Dns.Api.Terms;

namespace Lpp.Dns.Api.Tests.Terms
{
    [TestClass]
    public class CodeUploadTests
    {
        const string ResourcesFolder = "../Terms";
        
        static readonly log4net.ILog logger;
        static CodeUploadTests()
        {
            log4net.Config.XmlConfigurator.Configure();
            logger = log4net.LogManager.GetLogger(typeof(CodeUploadTests));
        }

        [TestMethod]
        public void ParseCSV_Example01_Diagnosis_AcuteKidneyFailure()
        {
            IEnumerable<CodeImport> codes = ParseExcel("Example01_DIAGNOSIS_Acute Kidney Failure.xlsx");

            foreach (var code in codes)
            {
                logger.Debug($"{ code.CriteriaIndex }, { code.CodeType }, { code.Code }, { code.SearchMethodType }");
            }

            var export = ConvertToQueryComposerCriteria(codes);

            var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented, NullValueHandling = Newtonsoft.Json.NullValueHandling.Include };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(export, jsonSettings);

            logger.Debug(Environment.NewLine + json);
        }

        

        [TestMethod]
        public void ParseExcel_Example01_Diagnosis_AcuteKidneyFailure()
        {
            IEnumerable<CodeImport> codes = ParseExcel("Example01_DIAGNOSIS_Acute Kidney Failure.xlsx");

            foreach (var code in codes)
            {
                logger.Debug($"{ code.CriteriaIndex }, { code.CodeType }, { code.Code }, { code.SearchMethodType }");
            }

            var export = ConvertToQueryComposerCriteria(codes);

            var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented, NullValueHandling = Newtonsoft.Json.NullValueHandling.Include };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(export, jsonSettings);

            logger.Debug(Environment.NewLine + json);

        }

        [TestMethod]
        public void ParseExcel_PMNDEV_6597_DX_ICD10_MDQ1()
        {
            IEnumerable<CodeImport> codes = ParseExcel("PMNDEV-6597_DX_ICD10_MDQ1.xlsx");

            foreach (var code in codes)
            {
                logger.Debug($"{ code.CriteriaIndex }, { code.CodeType }, { code.Code }, { code.SearchMethodType }");
            }

            var export = ConvertToQueryComposerCriteria(codes);

            var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented, NullValueHandling = Newtonsoft.Json.NullValueHandling.Include };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(export, jsonSettings);

            logger.Debug(Environment.NewLine + json);
        }

        [TestMethod]
        public void ParseExcel_PMNDEV_6597_DX_ICD9_MDQ1()
        {
            IEnumerable<CodeImport> codes = ParseExcel("PMNDEV-6597_DX_ICD9_MDQ1.xlsx");

            foreach (var code in codes)
            {
                logger.Debug($"{ code.CriteriaIndex }, { code.CodeType }, { code.Code }, { code.SearchMethodType }");
            }

            var export = ConvertToQueryComposerCriteria(codes);

            var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented, NullValueHandling = Newtonsoft.Json.NullValueHandling.Include };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(export, jsonSettings);

            logger.Debug(Environment.NewLine + json);
        }

        [TestMethod]
        public void ParseExcel_PMNDEV_6597_DX_ICD9_MDQ2()
        {
            IEnumerable<CodeImport> codes = ParseExcel("PMNDEV-6597_DX_ICD9_MDQ2.xlsx");

            foreach (var code in codes)
            {
                logger.Debug($"{ code.CriteriaIndex }, { code.CodeType }, { code.Code }, { code.SearchMethodType }");
            }

            var export = ConvertToQueryComposerCriteria(codes);

            var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented, NullValueHandling = Newtonsoft.Json.NullValueHandling.Include };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(export, jsonSettings);

            logger.Debug(Environment.NewLine + json);
        }

        [TestMethod]
        public void ParseExcel_MISC4()
        {
            IEnumerable<CodeImport> codes = ParseExcel("MISC4.xlsx");

            foreach (var code in codes)
            {
                logger.Debug($"{ code.CriteriaIndex }, { code.CodeType }, { code.Code }, { code.SearchMethodType }");
            }

            var export = ConvertToQueryComposerCriteria(codes);

            var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented, NullValueHandling = Newtonsoft.Json.NullValueHandling.Include };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(export, jsonSettings);

            logger.Debug(Environment.NewLine + json);
        }

        [TestMethod]
        public void ParseExcel_MISC5()
        {
            IEnumerable<CodeImport> codes = ParseExcel("MISC5.xlsx");

            foreach (var code in codes)
            {
                logger.Debug($"{ code.CriteriaIndex }, { code.CodeType }, { code.Code }, { code.SearchMethodType }");
            }

            var export = ConvertToQueryComposerCriteria(codes);

            var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented, NullValueHandling = Newtonsoft.Json.NullValueHandling.Include };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(export, jsonSettings);

            logger.Debug(Environment.NewLine + json);
        }

        [TestMethod]
        public void ParseExcel_PMNDEV_6597_DX_ICD9_MDQ1_B()
        {
            IEnumerable<CodeImport> codes = ParseExcel("PMNDEV-6597_ DX_ICD9_MDQ1_B.xlsx");

            foreach (var code in codes)
            {
                logger.Debug($"{ code.CriteriaIndex }, { code.CodeType }, { code.Code }, { code.SearchMethodType }");
            }

            var export = ConvertToQueryComposerCriteria(codes);

            var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented, NullValueHandling = Newtonsoft.Json.NullValueHandling.Include };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(export, jsonSettings);

            logger.Debug(Environment.NewLine + json);
        }

        [TestMethod]
        public void ParseExcel_PMNDEV_6597_PX_NI_UN_MDQ1_Revised()
        {
            IEnumerable<CodeImport> codes = ParseExcel("PMNDEV-6597_PX_NI_UN_MDQ1_Revised.xlsx");

            foreach (var code in codes)
            {
                logger.Debug($"{ code.CriteriaIndex }, { code.CodeType }, { code.Code }, { code.SearchMethodType }");
            }

            var export = ConvertToQueryComposerCriteria(codes);

            var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented, NullValueHandling = Newtonsoft.Json.NullValueHandling.Include };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(export, jsonSettings);

            logger.Debug(Environment.NewLine + json);
        }

        [TestMethod]
        public void ParseExcel_PMNDEV_6882_Test()
        {
            IEnumerable<CodeImport> codes = ParseExcel("PMNDEV-6882_Test.xlsx");

            foreach (var code in codes)
            {
                logger.Debug($"{ code.CriteriaIndex }, { code.CodeType }, { code.Code }, { code.SearchMethodType }");
            }

            var export = ConvertToQueryComposerCriteria(codes);

            var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented, NullValueHandling = Newtonsoft.Json.NullValueHandling.Include };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(export, jsonSettings);

            logger.Debug(Environment.NewLine + json);
        }

        [TestMethod]
        public void ParseExcel_PMNDEV_6882_Test2()
        {
            IEnumerable<CodeImport> codes = ParseExcel("PMNDEV-6882_Test2.xlsx");

            foreach (var code in codes)
            {
                logger.Debug($"{ code.CriteriaIndex }, { code.CodeType }, { code.Code }, { code.SearchMethodType }");
            }

            var export = ConvertToQueryComposerCriteria(codes);

            var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented, NullValueHandling = Newtonsoft.Json.NullValueHandling.Include };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(export, jsonSettings);

            logger.Debug(Environment.NewLine + json);
        }

        [TestMethod]
        public void ParseExcel_PMNDEV_6882_Test4()
        {
            IEnumerable<CodeImport> codes = ParseExcel("PMNDEV-6882_Test4.xlsx");

            foreach (var code in codes)
            {
                logger.Debug($"{ code.CriteriaIndex }, { code.CodeType }, { code.Code }, { code.SearchMethodType }");
            }

            var export = ConvertToQueryComposerCriteria(codes);

            var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented, NullValueHandling = Newtonsoft.Json.NullValueHandling.Include };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(export, jsonSettings);

            logger.Debug(Environment.NewLine + json);
        }

        [TestMethod]
        public void ParseExcel_PMNDEV_6882_Test5()
        {
            IEnumerable<CodeImport> codes = ParseExcel("PMNDEV-6882_Test5.xlsx");

            foreach (var code in codes)
            {
                logger.Debug($"{ code.CriteriaIndex }, { code.CodeType }, { code.Code }, { code.SearchMethodType }");
            }

            var export = ConvertToQueryComposerCriteria(codes);

            var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented, NullValueHandling = Newtonsoft.Json.NullValueHandling.Include };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(export, jsonSettings);

            logger.Debug(Environment.NewLine + json);
        }

        [TestMethod]
        public void ParseCSV_PMNDEV_6882_Test5()
        {
            IEnumerable<CodeImport> codes = ParseCSV("PMNDEV-6882_Test5.csv");

            foreach (var code in codes)
            {
                logger.Debug($"{ code.CriteriaIndex }, { code.CodeType }, { code.Code }, { code.SearchMethodType }");
            }

            var export = ConvertToQueryComposerCriteria(codes);

            var jsonSettings = new Newtonsoft.Json.JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented, NullValueHandling = Newtonsoft.Json.NullValueHandling.Include };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(export, jsonSettings);

            logger.Debug(Environment.NewLine + json);
        }



        IEnumerable<CodeImport> ParseCSV(string filename)
        {
            List<CodeImport> codes = new List<CodeImport>();

            string path = Path.Combine(ResourcesFolder, filename);
            using (var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(path))
            {
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.TrimWhiteSpace = true;

                //get the headers
                string[] headers = parser.ReadFields();
                int lineNumber = 1;
                while (parser.EndOfData == false)
                {
                    CodeImport code = new CodeImport();

                    string[] line = parser.ReadFields();

                    for (int i = 0; i < line.Length; i++)
                    {
                        switch (headers[i].ToUpperInvariant())
                        {
                            case "CRITERIAINDEX":
                                try
                                {
                                    code.CriteriaIndex = Convert.ToInt32(line[i]);
                                }catch
                                {
                                    throw new FormatException($"Unable to convert Criteria Index to integer on line { lineNumber }. Value: \"{ line[i] }\"");
                                }
                                break;
                            case "CODETYPE":
                                code.CodeType = line[i];
                                break;
                            case "CODE":
                                code.Code = line[i];
                                break;
                            case "EXACTMATCH":
                                code.SearchMethodType = string.Equals("1", line[i], StringComparison.OrdinalIgnoreCase) ? DTO.Enums.TextSearchMethodType.ExactMatch : DTO.Enums.TextSearchMethodType.StartsWith;
                                break;
                        }
                    }

                    if (code.IsEmpty() == false)
                    {
                        codes.Add(code);
                    }

                    lineNumber++;
                }
            }

            return codes.OrderBy(c => c.CriteriaIndex).ThenBy(c => c.CodeType).ThenBy(c => c.SearchMethodType);
        }
        

        IEnumerable<CodeImport> ParseExcel(string filename)
        {
            List<CodeImport> codes = new List<CodeImport>();

            string path = Path.Combine(ResourcesFolder, filename);
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(path, false))
            {
                //Read the first Sheet from Excel file.
                Sheet sheet = spreadsheetDocument.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();

                //Get the Worksheet instance.
                Worksheet worksheet = (spreadsheetDocument.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;

                //Fetch all the rows present in the Worksheet.
                IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();
                Dictionary<string, string> headers = null;
                foreach (var row in rows)
                {
                    if (row.RowIndex.Value == 1)
                    {
                        headers = new Dictionary<string, string>();
                        foreach (Cell cell in row.Descendants<Cell>())
                        {
                            if (cell.CellReference.HasValue == false)
                                continue;

                            string columnReference = System.Text.RegularExpressions.Regex.Replace(cell.CellReference.Value.ToUpper(), @"[\d]", string.Empty);
                            if (string.IsNullOrEmpty(columnReference))
                                continue;

                            if (cell.CellValue.IsEmpty() || cell.CellValue.IsNull())
                            {
                                headers.Add(columnReference, "EMPTY");
                            }
                            else
                            {
                                string value = GetValue(spreadsheetDocument, cell);
                                headers.Add(columnReference, (value ?? string.Empty).ToUpperInvariant());
                            }
                        }
                    }
                    else
                    {
                        if (!row.Descendants<Cell>().All(x => x.CellValue.IsNull() || x.CellValue.IsEmpty()))
                        {
                            CodeImport addedCode = new CodeImport();

                            foreach (var cell in row.Descendants<Cell>())
                            {
                                if (cell.CellReference.HasValue == false)
                                    continue;

                                string columnReference = System.Text.RegularExpressions.Regex.Replace(cell.CellReference.Value.ToUpper(), @"[\d]", string.Empty);
                                if (string.IsNullOrEmpty(columnReference))
                                    continue;
                                
                                if (headers.TryGetValue(columnReference, out string column))
                                {
                                    if (column.IsNullOrEmpty())
                                        continue;

                                    string value = GetValue(spreadsheetDocument, cell);
                                    switch (column.ToUpperInvariant())
                                    {
                                        case "CRITERIAINDEX":
                                            try
                                            {
                                                addedCode.CriteriaIndex = Convert.ToInt32(value);
                                            }
                                            catch
                                            {
                                                throw new FormatException($"Unable to convert Criteria Index to integer at cell reference: { cell.CellReference.Value }. Value: \"{ value }\"");
                                            }
                                            break;
                                        case "CODETYPE":
                                            addedCode.CodeType = value;
                                            break;
                                        case "CODE":
                                            addedCode.Code = value;
                                            break;
                                        case "EXACTMATCH":
                                            addedCode.SearchMethodType = string.Equals("1", value, StringComparison.OrdinalIgnoreCase) ? DTO.Enums.TextSearchMethodType.ExactMatch : DTO.Enums.TextSearchMethodType.StartsWith;
                                            break;
                                    }
                                }

                            }

                            if (addedCode.IsEmpty() == false)
                            {
                                codes.Add(addedCode);
                            }
                        }

                    }
                }
            }

            return codes.OrderBy(c => c.CriteriaIndex).ThenBy(c => c.CodeType).ThenBy(c => c.SearchMethodType);
        }        

        static string GetValue(SpreadsheetDocument doc, Cell cell)
        {
            string value = cell.CellValue.InnerText;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            }
            return value;
        }

        QueryComposerCriteriaDTO[] ConvertToQueryComposerCriteria(IEnumerable<CodeImport> codes)
        {
            Guid DiagnosisCodeTermID = new Guid("86110001-4bab-4183-b0ea-a4bc0125a6a7");
            Guid ProcedureCodeTermID = new Guid("f81ae5de-7b35-4d7a-b398-a72200ce7419");

            QueryComposerCriteriaDTO[] export = new QueryComposerCriteriaDTO[codes.Max(c => c.CriteriaIndex + 1)];

            foreach (var criteriaGrouping in codes.GroupBy(c => c.CriteriaIndex))
            {

                List<QueryComposerTermDTO> terms = new List<QueryComposerTermDTO>();
                foreach (var grouping in criteriaGrouping.GroupBy(k => new { k.CodeType, k.NumericCodeType, k.SearchMethodType }))
                {
                    Guid termID;
                    if (grouping.Key.CodeType.StartsWith("DX-", StringComparison.OrdinalIgnoreCase))
                    {
                        termID = DiagnosisCodeTermID;
                    }
                    else if (grouping.Key.CodeType.StartsWith("PX-", StringComparison.OrdinalIgnoreCase))
                    {
                        termID = ProcedureCodeTermID;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("CodeType", grouping.Key.CodeType, string.Format("The CodeType \"{0}\" is invalid, all CodeTypes must start with \"DX-\" or \"PX-\".", grouping.Key.CodeType));
                    }

                    var values = grouping.Select(c => c.Code).Where(c => !string.IsNullOrWhiteSpace(c)).Distinct(StringComparer.OrdinalIgnoreCase);

                    terms.Add(
                        new QueryComposerTermDTO
                        {
                            Operator = DTO.Enums.QueryComposerOperators.And,
                            Type = termID,
                            Values = new Dictionary<string, object> {
                                { "Values", new {
                                        CodeType = grouping.Key.NumericCodeType,
                                        CodeValues = string.Join("; ", values),
                                        grouping.Key.SearchMethodType
                                    }
                                }
                            }

                        }
                    );
                }

                var innerCriteria = new QueryComposerCriteriaDTO
                {
                    ID = DatabaseEx.NewGuid(),
                    Name = "i_codeterms",
                    Operator = DTO.Enums.QueryComposerOperators.And,
                    Terms = terms.ToArray(),
                    Type = DTO.Enums.QueryComposerCriteriaTypes.Paragraph
                };

                export[criteriaGrouping.Key] = new QueryComposerCriteriaDTO { Criteria = new[] { innerCriteria } };
            }

            return export;
        }

    }
}
