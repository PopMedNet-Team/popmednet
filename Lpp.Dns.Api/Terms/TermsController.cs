using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;
using Lpp.Dns.DTO.QueryComposer;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities;
using Lpp.Utilities.WebSites.Attributes;
using Lpp.Utilities.WebSites.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Lpp.Dns.Api.Terms
{
    /// <summary>
    /// Controller that supports the Terms
    /// </summary>
    public class TermsController : LppApiDataController<Term, TermDTO, DataContext, PermissionDefinition>
    {
        /// <summary>
        /// lists's the terms not allowed in the template with the specified templateID
        /// </summary>
        [HttpGet]
        public IQueryable<TemplateTermDTO> ListTemplateTerms(Guid id)
        {
            var x = DataContext.TemplateTerms.Where(TT => TT.TemplateID == id).Select(t => new TemplateTermDTO {
                TemplateID = t.TemplateID,
                Allowed = t.Allowed,
                Section = t.Section,
                TermID = t.TermID
            });
            return x;
        }

        /// <summary>
        /// Endpoint for Parsing a file that contains a list of codes to return a DTO for the MDQ.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> ParseCodeList()
        {

            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Content must be mime multipart.");
            }

            string uploadPath = System.Web.Configuration.WebConfigurationManager.AppSettings["DocumentsUploadFolder"] ?? string.Empty;
            if (string.IsNullOrEmpty(uploadPath))
                uploadPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Uploads/");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var provider = new ChunkedMultipartFormDataStreamProvider<IEnumerable<QueryComposerCriteriaDTO>>(uploadPath, HttpContext.Current.Request);

            var o = await Request.Content.ReadAsMultipartAsync(provider);

            var result = o.GetResult();

            if (result.uploaded == false)
            {
                return Request.CreateResponse(HttpStatusCode.Created, result);
            }

            if (string.IsNullOrEmpty(o.FileName))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Filename is missing.");

            try
            {

                IEnumerable<CodeImport> parsedCodes;
                if (o.MetaData.FileExtension == ".csv")
                {
                    parsedCodes = ParseCSV(Path.Combine(uploadPath, o.MetaData.UploadUid + ".part"));
                }
                else if (o.MetaData.FileExtension == ".xlsx")
                {
                    parsedCodes = ParseExcel(Path.Combine(uploadPath, o.MetaData.UploadUid + ".part"));
                }
                else
                    throw new NotSupportedException("The file type is not supported, the file must be of .xlsx or .csv");

                result.Result = ConvertToQueryComposerCriteria(parsedCodes);

                return Request.CreateResponse(HttpStatusCode.Created, result);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                File.Delete(Path.Combine(uploadPath, o.MetaData.UploadUid + ".part"));
            }
        }

        IEnumerable<CodeImport> ParseExcel(string file)
        {
            IList<CodeImport> codes = new List<CodeImport>();

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file), false))
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
                                    try
                                    {
                                        if (column.IsNullOrEmpty() || string.Equals("EMPTY", column, StringComparison.OrdinalIgnoreCase))
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
                                                addedCode.SearchMethodType = string.Equals("1", value, StringComparison.OrdinalIgnoreCase) ? TextSearchMethodType.ExactMatch : TextSearchMethodType.StartsWith;
                                                break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                        throw;
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

        IEnumerable<CodeImport> ParseCSV(string file)
        {
            IList<CodeImport> listDTO = new List<CodeImport>();

            using (var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file)))
            {
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.TrimWhiteSpace = true;

                //get the headers
                string[] headers = parser.ReadFields();

                //read the code import values
                while (parser.EndOfData == false)
                {
                    CodeImport code = new CodeImport();

                    string[] line = parser.ReadFields();
                    int lineNumber = 1;
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(headers[i]))
                            continue;

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
                                code.SearchMethodType = string.Equals("1", line[i], StringComparison.OrdinalIgnoreCase) ? TextSearchMethodType.ExactMatch : TextSearchMethodType.StartsWith;
                                break;
                        }
                    }

                    if (code.IsEmpty() == false)
                    {
                        listDTO.Add(code);
                    }
                    lineNumber++;
                }
            }

            return listDTO.OrderBy(c => c.CriteriaIndex).ThenBy(c => c.CodeType).ThenBy(c => c.SearchMethodType);
        }

        static readonly Guid DiagnosisCodeTermID = new Guid("86110001-4bab-4183-b0ea-a4bc0125a6a7");
        static readonly Guid ProcedureCodeTermID = new Guid("f81ae5de-7b35-4d7a-b398-a72200ce7419");

        QueryComposerCriteriaDTO[] ConvertToQueryComposerCriteria(IEnumerable<CodeImport> codes)
        {
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
                            Operator = DTO.Enums.QueryComposerOperators.Or,
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

        static string GetValue(SpreadsheetDocument doc, Cell cell)
        {
            string value = cell.CellValue.InnerText;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            }
            return value;
        }

    }

    /// <summary>
    /// DTO to hold code import information when read from the import file.
    /// </summary>
    public class CodeImport
    {
        /// <summary>
        /// Initializes a new CodeImport dto.
        /// </summary>
        public CodeImport()
        {
            CriteriaIndex = 0;
            CodeType = null;
            Code = null;
        }
        /// <summary>
        /// Gets or sets the index of the criteria the code term belongs to.
        /// </summary>
        public int CriteriaIndex { get; set; }
        /// <summary>
        /// Gets or sets the string representation of the Code Type.
        /// </summary>
        public string CodeType { get; set; }

        /// <summary>
        /// Returns the CodeType value as a numeric representation of the CodeType.
        /// </summary>
        public int NumericCodeType
        {
            get
            {
                //Diagnosis CodeTypes
                if (string.Equals(CodeType, "DX-Any", StringComparison.OrdinalIgnoreCase))
                {
                    return -1;
                }
                else if (string.Equals(CodeType, "DX-ICD-9-CM", StringComparison.OrdinalIgnoreCase))
                {
                    return 3;
                }
                else if (string.Equals(CodeType, "DX-ICD-10-CM", StringComparison.OrdinalIgnoreCase))
                {
                    return 4;
                }
                else if (string.Equals(CodeType, "DX-ICD-11-CM", StringComparison.OrdinalIgnoreCase))
                {
                    return 5;
                }
                else if (string.Equals(CodeType, "DX-SNOMED-CT", StringComparison.OrdinalIgnoreCase))
                {
                    return 6;
                }
                else if (string.Equals(CodeType, "DX-NoInformation", StringComparison.OrdinalIgnoreCase))
                {
                    return 1;
                }
                else if (string.Equals(CodeType, "DX-Unknown", StringComparison.OrdinalIgnoreCase))
                {
                    return 0;
                }
                else if (string.Equals(CodeType, "DX-Other", StringComparison.OrdinalIgnoreCase))
                {
                    return 2;
                }
                //Procedure CodeTypes
                else if (string.Equals(CodeType, "PX-Any", StringComparison.OrdinalIgnoreCase))
                {
                    return 1;
                }
                else if (string.Equals(CodeType, "PX-ICD-9-CM", StringComparison.OrdinalIgnoreCase))
                {
                    return 2;
                }
                else if (string.Equals(CodeType, "PX-ICD-10-PCS", StringComparison.OrdinalIgnoreCase))
                {
                    return 3;
                }
                else if (string.Equals(CodeType, "PX-ICD-11-PCS", StringComparison.OrdinalIgnoreCase))
                {
                    return 4;
                }
                else if (string.Equals(CodeType, "PX-CPT", StringComparison.OrdinalIgnoreCase) || string.Equals(CodeType, "PX-HCPCS", StringComparison.OrdinalIgnoreCase))
                {
                    return 5;
                }
                else if (string.Equals(CodeType, "PX-LOINC", StringComparison.OrdinalIgnoreCase))
                {
                    return 6;
                }
                else if (string.Equals(CodeType, "PX-NDC", StringComparison.OrdinalIgnoreCase))
                {
                    return 7;
                }
                else if (string.Equals(CodeType, "PX-Revenue", StringComparison.OrdinalIgnoreCase))
                {
                    return 8;
                }
                else if (string.Equals(CodeType, "PX-NoInformation", StringComparison.OrdinalIgnoreCase))
                {
                    return 9;
                }
                else if (string.Equals(CodeType, "PX-Unknown", StringComparison.OrdinalIgnoreCase))
                {
                    return 10;
                }
                else if (string.Equals(CodeType, "PX-Other", StringComparison.OrdinalIgnoreCase))
                {
                    return 11;
                }

                throw new Exception(string.Format("{0} is not a valid Code Type.  Please resend the Code List with a valid Code Type", CodeType));
            }
        }

        /// <summary>
        /// Gets or sets the code value.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the search method type for the code value.
        /// </summary>
        public DTO.Enums.TextSearchMethodType SearchMethodType { get; set; }

        /// <summary>
        /// Indicates if of the CodeType, and Code have been set.
        /// By default the CriteriaIndex will be 0, and the SearchMethodType will be ExactMatch (0).
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(CodeType + Code);
        }

        /// <summary>
        /// Returns the dto values as a single string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Criteria Index:{0}, CodeType:{1}, Code: {2}, Exact Match: {3}", CriteriaIndex, CodeType, Code, SearchMethodType);
        }
    }
}
