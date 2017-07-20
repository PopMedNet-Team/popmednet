using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using System.ServiceModel;

#pragma warning disable 618 //XmlDataDocument is obsolete
namespace Lpp.Dns.DataMart.Lib
{
    public class Util
    {

        #region Constants        

        public const string LoggedInStatus = "LoggedIn";
        public const string LoggedOutStatus = "LoggedOut";
        public const string LoginFailureStatus = "Failed";
        public const string InCompleteStatus = "InComplete";
        public const string ConnectionOKStatus = "Connection OK";
        public const string ConnectionFailedStatus = "Connection Failed";


        static string passPhrase = "688F021D-2F3B-465b-BB90-2F4056D79340";
        static string saltValue = "10A450C7-C15A-4f12-842B-1861131EA0E5";//MacAddress;
        static string hashAlgorithm = "SHA1";
        static int passwordIterations = 2;
        static string initVector = "20EB643D-2EFA-4e";
        static int keySize = 256;
        #endregion

        #region "QueryTypeConstants"

        public const int GenericName = 1;
        public const int DrugClass = 2;
        public const int NDC = 3;
        public const int ICD9Diagnosis = 4;
        public const int ICD9Procedures = 5;
        public const int HCPCSProcedures = 6;
        public const int EligibilityAndEntrolMent = 7;
        public const int FileDistribution = 8;
        public const int RefreshDates = 9;
        public const int ICD9Diagnosis_4_digit = 10;
        public const int ICD9Diagnosis_5_digit = 11;
        public const int ICD9Procedures_4_digit = 12;
        public const int SAS = 13;
        public const int Incident_GenericName = 14;
        public const int Incident_DrugClass = 15;
        public const int Incident_NDC = 16;
        public const int Incident_ICD9Diagnosis = 17;
        public const int Incident_ICD9Procedures = 18;
        public const int Incident_HCPCSProcedures = 19;
        public const int Incident_EligibilityAndEntrolMent = 20;
        public const int Incident_ICD9Diagnosis_4_digit = 21;
        public const int Incident_ICD9Diagnosis_5_digit = 22;
        public const int Incident_ICD9Procedures_4_digit = 23;
        public const int DataMart_Client_Application_Update = 24;
        public const int Query_Builder_Obesity_Module1 = 25;
        public const int Query_Builder_Obesity_Module3 = 26;
        public const int Query_Builder_ADHD_Module1 = 27;
        public const int Query_Builder_ADHD_Module3 = 28;

        public const int MFU_GenericName = 29;
        public const int MFU_DrugClass = 30;
        public const int MFU_ICD9Diagnosis = 31; // 3 digits
        public const int MFU_ICD9Procedures = 32; // 3 digits
        public const int MFU_HCPCSProcedures = 33;
        public const int MFU_ICD9Diagnosis_4_digit = 34;
        public const int MFU_ICD9Diagnosis_5_digit = 35;
        public const int MFU_ICD9Procedures_4_digit = 36;

        #endregion

        public enum Right
        {
            //
            // NOTE: ANY UPDATES MADE HERE MUST ALSO BE MADE IN DRNLib\enums.cs
            //
            Login = 1,
            SubmitQuery = 2,
            ViewOwnQueryDetail = 3,
            ViewOrganizationQueryDetail = 4,
            ViewAnyQueryDetail = 5,
            ViewOwnQueryResultsSummary = 6,
            ViewAnyQueryResultsSummary = 7,
            ViewOwnResultsDetail = 8,
            ViewAnyQueryResultsDetail = 9,
            AdministerSelfUserProfile = 10,
            AdministerAnyUserProfile = 11,
            ////AdministerUsers = 12,-- Removed as a part of right administration change
            CreateAnyUser = 13,
            DeleteAnyUser = 14,
            AdministerDatamartforSameorganization = 15,
            AdministerAnyDatamart = 16,
            ////AdministerDatamarts = 17,-- Removed as a part of right administration change
            //AdministerGroups = 18,-- Removed as a part of right administration change
            //AdministerOrganizations = 19,-- Removed as a part of right administration change
            AdministerQueries = 20,
            AdministerRights = 21,
            ChangeQueryStatus = 22,
            QueryExceution = 23,
            UploadResults = 24,
            AllowQueryReject = 25,
            AllowAddingOrg = 26,
            DeleteOrganization = 27,
            AllowAddingGroup = 28,
            AllowDeletingGroup = 29,
            AllowAddingNewDatamart = 30,
            AllowDeletingDatamart = 31,
            AdministerRoles = 32,
            AllowSubmissionToOwnOrganization = 33,
            AllowRemovingDataMartsFromQuery = 34,
            AllowViewingQueriesByDataMart = 35,
            AllowAddingDataMartsToQuery = 36,

            ViewResultsSummaryForOrganizationUsers = 37,//View result summaries for queries submitted by users of own organization.
            ViewResultsSummaryForGroupUsers = 38,//View result summaries for queries submitted by users of own group.
            ViewResultsDetailsForOrganizationUsers = 39,//View result details for queries submitted by users of own organization.
            ViewResultsDetailsForGroupUsers = 40,//View result details for queries submitted by users of own group.
            AdministerOrganizationUsers = 41,//Administer users within own organization.
            AdministerGroupUsers = 42,//Administer users within own group.
            ViewQueriesSubmittedByGroupUsers = 43,//View status of own group’s queries
            CreateOrganizationalUser = 44,
            DeleteOrganizationalUser = 45,
            CreateGroupUser = 46,
            DeleteGroupUser = 47,
            AllowViewingIndividualResults = 48,
            AllowApprovingUploadedDataMartResults = 49,
            AdministerSASQuery = 50,
            ViewOtherOrganizationResults = 51,
            ViewOnlyQueryEntryPage = 52,
            AllowAggregatingUploadedResults = 53,
            AllowSubmissionToGroupDataMarts = 54,
            AllowSubmissionToAdministeringDatamart = 55,
            AllowReassigningDataMartOrganization = 56,
            ViewOnlyFileDistributionPage = 57,
            ViewSelfAdministeredGroupQuerySummary = 58,
            ViewSelfAdministeredGroupResultSummary = 59,
            ViewSelfAdministeredGroupResultDetail = 60,
            ViewQuerySummaryForSelfAdminOrganization = 61,//View query summary for queries submitted to self-administered Organization and by organization
            ViewResultSummaryForSelfAdminOrganization = 62, //View query summary for queries submitted to self-administered Organization  and by organization 
            AllowApprovingQuerySubmission = 63,
            AllowNotificationSubmission = 64,
            ViewQueryBuilderPage = 65,
            CopyOrganizationNotifications = 66
        }
        public static string Encrypt(string inputText)
        {

            // Convert strings into byte arrays.
            // Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8 
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our plaintext into a byte array.
            // Let us assume that plaintext contains UTF8-encoded characters.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(inputText);

            // First, we must create a password, from which the key will be derived.
            // This password will be generated from the specified passphrase and 
            // salt value. The password will be created using the specified hash 
            // algorithm. Password creation can be done in several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            passPhrase,
                                                            saltValueBytes,
                                                            hashAlgorithm,
                                                            passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate encryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
                                                             keyBytes,
                                                             initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream();

            // Define cryptographic stream (always use Write mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         encryptor,
                                                         CryptoStreamMode.Write);
            // Start encrypting.
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            // Finish encrypting.
            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memory stream into a byte array.
            byte[] cipherTextBytes = memoryStream.ToArray();

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert encrypted data into a base64-encoded string.
            string cipherText = Convert.ToBase64String(cipherTextBytes);

            // Return encrypted string.
            return cipherText;
        }

        public static string Decrypt(string cipherText)
        {
            // Convert strings defining encryption key characteristics into byte
            // arrays. Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our ciphertext into a byte array.
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            // First, we must create a password, from which the key will be 
            // derived. This password will be generated from the specified 
            // passphrase and salt value. The password will be created using
            // the specified hash algorithm. Password creation can be done in
            // several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            passPhrase,
                                                            saltValueBytes,
                                                            hashAlgorithm,
                                                            passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate decryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                                                             keyBytes,
                                                             initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            // Define cryptographic stream (always use Read mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                          decryptor,
                                                          CryptoStreamMode.Read);

            // Since at this point we don't know what the size of decrypted data
            // will be, allocate the buffer long enough to hold ciphertext;
            // plaintext is never longer than ciphertext.
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            // Start decrypting.
            int decryptedByteCount = cryptoStream.Read(plainTextBytes,
                                                       0,
                                                       plainTextBytes.Length);

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert decrypted data into a string. 
            // Let us assume that the original plaintext string was UTF8-encoded.
            string plainText = Encoding.UTF8.GetString(plainTextBytes,
                                                       0,
                                                       decryptedByteCount);

            // Return decrypted string.   
            return plainText;
        }

        /// <summary>
        /// Compares two nullable dates. Returns 0=Equal, 1=Date1>Date2,-1=Date1<Date2
        /// </summary>
        /// <param name="Date1"></param>
        /// <param name="Date2"></param>
        /// <returns></returns>
        public static int CompareNullableDates(DateTime? Date1, DateTime? Date2)
        {
            int CompareResult = 0;

            if (Date1 == null && Date2 == null)
                CompareResult = 0;
            else if (Date1 != null && Date2 == null)
                CompareResult = 1;
            else if (Date1 == null && Date2 != null)
                CompareResult = -1;
            else
            {
                DateTime dt1, dt2;
                DateTime.TryParse(Date1.ToString(), out dt1);
                DateTime.TryParse(Date2.ToString(), out dt2);
                CompareResult = dt1.CompareTo(dt2);
            }

            return CompareResult;
        }

        /// <summary>
        /// Compares two priorities. Returns 0=Equal, 1=Priority1>Priority2,-1=Priority1<Priority2
        /// </summary>
        /// <param name="Priority1"></param>
        /// <param name="Priority2"></param>
        /// <returns></returns>
        public static int CompareActivityPriorities(string Priority1, string Priority2)
        {
            int CompareResult = 0;
            int priorityNum1=0,priorityNum2=0;
            string[] priorities = new string[] { "", "Low", "Normal", "High" };

            if (string.IsNullOrEmpty(Priority1)) Priority1 = "";
            if (string.IsNullOrEmpty(Priority2)) Priority2 = "";

            for (int ctr = 0; ctr < priorities.Length; ctr++)
            {
                if(priorities[ctr].ToUpper().Equals(Priority1.ToUpper()))
                    priorityNum1=ctr;
                if(priorities[ctr].ToUpper().Equals(Priority2.ToUpper()))
                    priorityNum2=ctr;
            }
            CompareResult = priorityNum1.CompareTo(priorityNum2);

            return CompareResult;
        }

        #region "Public Static Methods for Expanding Summary Query Results using all combinations of Period,AgeGroup,Sex,Setting,Drug/Code"
        //Code under this region is a duplication of what is in DataMartLib.Util class.
        //Any change here or there must be synchronised.

        //private static DataTable GenerateSummaryQueryResultCombinations(Query q)
        //{
        //    return GenerateSummaryQueryResultCombinations(q.QueryXml);
        //}
        private static DataTable GenerateSummaryQueryResultCombinations(string QueryXml)
        {
            DataTable dt = new DataTable("Results");
            try
            {
                if (!string.IsNullOrEmpty(QueryXml))
                {
                    DataColumn col;
                    DataRow dr;
                    string PeriodColumn = "Period";
                    string SettingColumn = "Setting";
                    string SexColumn = "Sex";
                    string AgeColumn = "AgeGroup";
                    string CodeColumn = string.Empty;
                    string NameColumn = string.Empty;
                    bool IsQualifiedForExpansion = false;
                    col = new DataColumn(PeriodColumn, "string".GetType());
                    dt.Columns.Add(col);
                    col = new DataColumn(SexColumn, "string".GetType());
                    dt.Columns.Add(col);
                    col = new DataColumn(AgeColumn, "string".GetType());
                    dt.Columns.Add(col);
                    col = new DataColumn(SettingColumn, "string".GetType());
                    dt.Columns.Add(col);

                    #region "Read Query.XML and populate rows"

                    XmlDataDocument ObjectDoc = new XmlDataDocument();
                    ObjectDoc.LoadXml(QueryXml);
                    XPathNavigator Nav = ObjectDoc.CreateNavigator();
                    XPathExpression Expr;
                    XPathNodeIterator INode = null;
                    string nodeValue = string.Empty;

                    string[] Years = new string[] { };
                    string[] Ages = new string[] { };
                    List<string> Genders = new List<string>();
                    List<string> Settings = new List<string>();
                    List<string> Codes = new List<string>();
                    List<string> Names = new List<string>();

                    #region "Get Names for code column e.g. pxcode,dxcode,drugclass,etc..and name column e.g. pxname,dxname,etc.."

                    //The Generation of Combination Rows and Subsequent Expansion Of results applicable only to Prevalence,Incidence Queries.
                    Expr = Nav.Compile("Query/QueryType");
                    INode = Nav.Select(Expr);
                    if (INode.MoveNext())
                    {
                        if (!string.IsNullOrEmpty(INode.Current.GetAttribute("Id", string.Empty)))
                        {
                            string querytypeid = INode.Current.GetAttribute("Id", string.Empty);
                            switch (Int32.Parse(querytypeid))
                            {
                                case ICD9Diagnosis:
                                case ICD9Diagnosis_4_digit:
                                case ICD9Diagnosis_5_digit:
                                case Incident_ICD9Diagnosis:
                                case Incident_ICD9Diagnosis_4_digit:
                                case Incident_ICD9Diagnosis_5_digit:
                                    CodeColumn = "DxCode";
                                    NameColumn = "DxName";
                                    IsQualifiedForExpansion = true;
                                    break;
                                case ICD9Procedures:
                                case ICD9Procedures_4_digit:
                                case HCPCSProcedures:
                                case Incident_ICD9Procedures:
                                case Incident_ICD9Procedures_4_digit:
                                case Incident_HCPCSProcedures:
                                    CodeColumn = "PxCode";
                                    NameColumn = "PxName";
                                    IsQualifiedForExpansion = true;
                                    break;
                                case GenericName:
                                case Incident_GenericName:
                                    CodeColumn = string.Empty;
                                    NameColumn = "GenericName";
                                    IsQualifiedForExpansion = true;
                                    break;
                                case DrugClass:
                                case Incident_DrugClass:
                                    CodeColumn = string.Empty;
                                    NameColumn = "DrugClass";
                                    IsQualifiedForExpansion = true;
                                    break;
                            }
                        }
                    }

                    #endregion

                    if (IsQualifiedForExpansion)
                    {
                        if (!string.IsNullOrEmpty(CodeColumn))
                        {
                            col = new DataColumn(CodeColumn, "string".GetType());
                            dt.Columns.Add(col);
                        }
                        if (!string.IsNullOrEmpty(NameColumn))
                        {
                            col = new DataColumn(NameColumn, "string".GetType());
                            dt.Columns.Add(col);
                        }

                        //Read Selected Periods e.g. 2016,2017, etc.
                        Expr = Nav.Compile("Query/PeriodList");
                        INode = Nav.Select(Expr);
                        if (INode.MoveNext())
                        {
                            nodeValue = INode.Current.Value;
                            if (!string.IsNullOrEmpty(nodeValue))
                            {
                                Years = nodeValue.Split(',');
                            }
                        }

                        //Read Selected Sex stratifications
                        Expr = Nav.Compile("Query/SexStratification");
                        INode = Nav.Select(Expr);
                        if (INode.MoveNext())
                        {
                            string sexStratId = INode.Current.GetAttribute("Id", string.Empty);
                            if (!string.IsNullOrEmpty(sexStratId))
                            {
                                switch (sexStratId)
                                {
                                    case "1"://Female only
                                        Genders.Add("F");
                                        break;
                                    case "2"://Male only
                                        Genders.Add("M");
                                        break;
                                    case "3"://Male and Female
                                        Genders.Add("F");
                                        Genders.Add("M");
                                        break;
                                    case "4": //Male,Female Aggregated
                                        Genders.Add("All");
                                        break;
                                }
                            }
                        }

                        //Read Selected Age stratifications
                        Expr = Nav.Compile("Query/AgeStratification");
                        INode = Nav.Select(Expr);
                        if (INode.MoveNext())
                        {
                            nodeValue = INode.Current.Value;
                            if (!string.IsNullOrEmpty(nodeValue))
                            {
                                int startIndex = nodeValue.IndexOf("(");
                                int endIndex = nodeValue.LastIndexOf(")");
                                if (startIndex > 0 && endIndex > 0 && startIndex < endIndex)
                                {
                                    nodeValue = nodeValue.Substring(startIndex + 1, (endIndex - startIndex) - 1);
                                    Ages = nodeValue.Split(',');
                                }
                            }
                        }

                        //Read Selected setting
                        Expr = Nav.Compile("Query/Setting");
                        INode = Nav.Select(Expr);
                        if (INode.MoveNext())
                        {
                            nodeValue = INode.Current.GetAttribute("Id", string.Empty);
                            if (!string.IsNullOrEmpty(nodeValue))
                            {
                                Settings.Add(nodeValue);
                            }
                        }

                        //Read Selected Codes and their names.
                        Expr = Nav.Compile("Query/Codes/Code");
                        INode = Nav.Select(Expr);
                        while (INode.MoveNext())
                        {
                            if (!string.IsNullOrEmpty(INode.Current.GetAttribute("Id", string.Empty)))
                            {
                                Codes.Add(INode.Current.GetAttribute("Id", string.Empty));
                                Names.Add(INode.Current.Value);
                            }
                        }

                        //Create Combination Rows based on selected Period,AgeGroup,Sex,Setting and Codes.
                        if (Years.Length > 0 && Ages.Length > 0 && Genders.Count > 0 && Settings.Count > 0)
                        {
                            foreach (string year in Years)
                                foreach (string sex in Genders)
                                    foreach (string age in Ages)
                                        foreach (string setting in Settings)
                                            for (int ctr = 0; ctr < Codes.Count; ctr++)
                                            {
                                                dr = dt.NewRow();
                                                dr[PeriodColumn] = year;
                                                dr[SexColumn] = sex;
                                                dr[AgeColumn] = age;
                                                dr[SettingColumn] = setting;
                                                dr[CodeColumn] = Codes[ctr];
                                                dr[NameColumn] = Names[ctr];
                                                dt.Rows.Add(dr);
                                            }
                        }
                    }

                    #endregion

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;

        }

        //public static DataTable ExpandSummaryResults(DataTable RowsWithData, string QueryXML)
        //{
        //    DataTable CombinationRows = GenerateSummaryQueryResultCombinations(QueryXML);
        //    return ExpandSummaryResults(CombinationRows, RowsWithData);
        //}
        //public static DataTable ExpandSummaryResults(DataTable RowsWithData, Query q)
        //{
        //    DataTable CombinationRows = GenerateSummaryQueryResultCombinations(q);
        //    return ExpandSummaryResults(CombinationRows, RowsWithData);
        //}
        public static DataTable ExpandSummaryResults(DataTable CombinationRows, DataTable RowsWithData)
        {
            DataTable MergedTable = null;
            if (CombinationRows != null && CombinationRows.Rows.Count > 0)
            {
                string FilterExpression = string.Empty;

                #region "Create Table Structure For Merged Results"

                //MergedTable gets the structure of CombinationRows. 
                //Then Columns from RowsWithData are Copied into MergedTable structure.
                MergedTable = CombinationRows.Copy();
                foreach (DataColumn dcol in RowsWithData.Columns)
                {
                    if (!MergedTable.Columns.Contains(dcol.ColumnName))
                    {
                        DataColumn newCol = MergedTable.Columns.Add(dcol.ColumnName, "string".GetType());
                    }
                }

                //For all new columns copied from RowsWithData, initialize with value "0"
                //foreach (DataRow dr in MergedTable.Rows)
                //    foreach (DataColumn dc in MergedTable.Columns)
                //        if (!CombinationRows.Columns.Contains(dc.ColumnName))
                //            dr[dc.ColumnName] = "0";

                #endregion

                #region "Merge RowsWithData into MergedTable."

                /*LOGIC
                 * For Each Row Containing Result Data in RowsWithData, Find Matching Combination Rows in MergedTable.
                 * Copy the data From RowsWithData row into matching combination rows.
                 */
                foreach (DataRow drow in RowsWithData.Rows)
                {
                    FilterExpression = string.Empty;
                    foreach (DataColumn dcol in CombinationRows.Columns)
                    {
                        //Do not match with pxname and dxname, they can differ in data mart client database than what is selected in portal. Matching with pxcode,dxcode only makes sense.
                        if (dcol.ColumnName.ToLower() != "pxname" && dcol.ColumnName.ToLower() != "dxname" && RowsWithData.Columns.Contains(dcol.ColumnName) &&
                            drow[dcol.ColumnName] != null && !string.IsNullOrEmpty(drow[dcol.ColumnName].ToString()))
                            FilterExpression = FilterExpression + (string.IsNullOrEmpty(FilterExpression) ? dcol.ColumnName + "='" + drow[dcol.ColumnName].ToString() + "'" : " AND " + dcol.ColumnName + "='" + drow[dcol.ColumnName].ToString() + "'");
                    }
                    DataRow[] MatchingRows = MergedTable.Select(FilterExpression);
                    //Copy the data from Matching Row to the Associated MergedTable.
                    if (MatchingRows != null && MatchingRows.Length > 0)
                    {
                        foreach (DataRow drMerged in MatchingRows)
                        {
                            foreach (DataColumn dc in drow.Table.Columns)
                            {
                                if (!CombinationRows.Columns.Contains(dc.ColumnName))
                                {
                                    if ((drow[dc.ColumnName] != null && !string.IsNullOrEmpty(drow[dc.ColumnName].ToString())))
                                        drMerged[dc.ColumnName] = drow[dc.ColumnName];
                                }
                            }
                        }
                    }
                }

                #endregion
            }
            else
            {
                MergedTable = RowsWithData;
            }
            return MergedTable;
        }

        #endregion

        //internal static DRNHubService.DRNHubService NewDRNHubService(string hubWebServiceUrl)
        //{
        //    return new DataMartLib.DRNHubService.DRNHubServiceClient("BasicHttpBinding_DRNHubService", new EndpointAddress(hubWebServiceUrl));
        //}
    }
}
