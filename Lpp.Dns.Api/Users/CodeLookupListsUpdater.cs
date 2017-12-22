using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Lpp.Dns.Api.Users
{

    /// <summary>
    /// Helper class for updating code lookup lists
    /// </summary>
    public class CodeLookupListsUpdater
    {
        static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(CodeLookupListsUpdater));
        readonly DataContext db;
        readonly string baseUrl;
        readonly string serviceUsername;
        readonly string servicePassword;
        readonly string category;
        readonly string source;
        readonly string codeClass;
        readonly string codeLength;
        readonly Lpp.Dns.DTO.Enums.Lists ListID;
        public Lpp.Dns.DTO.Enums.Lists SpanListID;
        public string versionUrl;
        public List<LookupListCategory> categoriesToAdd;
        public List<LookupListCategory> spanCategoriesToAdd;
        public List<LookupListValue> codesToAdd;

        public CodeLookupListsUpdater(DataContext dataContext, string serviceUrl, string serviceUsername, string servicePassword, string category, string source, string codeClass, string codeLength, Lpp.Dns.DTO.Enums.Lists ListID)
        {
            this.db = dataContext;
            this.baseUrl = serviceUrl;
            this.serviceUsername = serviceUsername;
            this.servicePassword = servicePassword;
            this.category = category;
            this.source = source;
            this.codeClass = codeClass;
            this.ListID = ListID;
            this.StatusCode = HttpStatusCode.OK;
            this.StatusMessage = string.Empty;
            this.versionUrl = baseUrl + category + "/" + source + "/" + codeClass;
            this.categoriesToAdd = new List<LookupListCategory>();
            this.spanCategoriesToAdd = new List<LookupListCategory>();
            if (codeLength.IsNullOrEmpty())
            {
                this.codeLength = "";
            }
            else this.codeLength = "code_length=" + codeLength;

        }

        /// <summary>
        /// Gets the HttpStatusCode indicating the success or failure of the update operation.
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the current status message for the update operation.
        /// </summary>
        public string StatusMessage
        {
            get;
            private set;
        }

        public async Task DoUpdate()
        {
            Logger.Debug("Starting the update process for codes with ListID equal to" + ListID);

            Logger.Debug("Retrieving latest version from the CommonWealth Web Service.");
            //Pull the highest version number from MCM
            DTO.LookupListsImportDTO versionsList = await LoadVersions();
            var latestVersion = versionsList.Results.LatestVersion.VersionID;
            
            //Query PMN database for the highest version number (our local version number)
            var localVersion = db.LookupLists.Where(t => t.ListId == this.ListID).Select(j => j.Version).ToString();
            
            //If the latest version is not the local Version, delete codes with the current ListID, and then import the new set of codes
            if (latestVersion != localVersion) {
                string importVersionUrl = "";
                string categoriesUrl = "";
                if (ListID == Lpp.Dns.DTO.Enums.Lists.HCPCSProcedures )
                {
                    importVersionUrl = versionUrl + "/" + latestVersion + "/code?columns=code,category,short_description,data_end";
                    //no categories for hcpcs in pmn database (the categories in the code selector are grouped by code # ranges)
                    //but there are categories in the MCM data so import them here
                    categoriesUrl = versionUrl + "/" + latestVersion + "/code?columns=category";
                }
                if (ListID == Lpp.Dns.DTO.Enums.Lists.GenericName)
                {
                    importVersionUrl = versionUrl + "/" + latestVersion + "/code?columns=generic_name,drug_class,data_end";
                    //the categories for Generic Drug Names are the drug class.
                    categoriesUrl = versionUrl + "/" + latestVersion + "/code?columns=drug_class";
                }
                if (ListID == Lpp.Dns.DTO.Enums.Lists.DrugClass)
                {
                    importVersionUrl = versionUrl + "/" + latestVersion + "/code?columns=drug_class,data_end";
                    //the categories for drug class are the drug class (the category is just a repetition of the drug class name)
                    categoriesUrl = versionUrl + "/" + latestVersion + "/code?columns=drug_class";
                }
                if (this.ListID == Lpp.Dns.DTO.Enums.Lists.ICD9Diagnosis ||
                    this.ListID == Lpp.Dns.DTO.Enums.Lists.ICD9Diagnosis4Digits ||
                    this.ListID == Lpp.Dns.DTO.Enums.Lists.ICD9Diagnosis5Digits ||
                    this.ListID == Lpp.Dns.DTO.Enums.Lists.ICD9Procedures ||
                    this.ListID == Lpp.Dns.DTO.Enums.Lists.ICD9Procedures4Digits)
                {
                    importVersionUrl = versionUrl + "/" + latestVersion + "/code?" + codeLength + "&columns=code,code_unformatted,category,short_description,data_end";
                    categoriesUrl = versionUrl + "/" + latestVersion + "/code?columns=category";
                }

                //SPAN- Diagnosis (ListID = 10) represents all diagnostic codes (Listid=4,7,8)
                //SPAN- Procedure (ListID = 11) represents all procedure codes (Listid = 5,9)
                if (this.ListID == Lpp.Dns.DTO.Enums.Lists.ICD9Diagnosis ||
                    this.ListID == Lpp.Dns.DTO.Enums.Lists.ICD9Diagnosis4Digits ||
                    this.ListID == Lpp.Dns.DTO.Enums.Lists.ICD9Diagnosis5Digits) 
                    { 
                        this.SpanListID = DTO.Enums.Lists.SPANDiagnosis;
                    }
                if (this.ListID == Lpp.Dns.DTO.Enums.Lists.ICD9Procedures ||
                    this.ListID == Lpp.Dns.DTO.Enums.Lists.ICD9Procedures4Digits)
                    {
                        this.SpanListID = DTO.Enums.Lists.SPANProcedure;
                    }


                Logger.Debug("Loading codes and categories from CommonWealth Web Service");
                //Pull in all codes from MCM (deserialize into a list of objects)
                LookupListValuesImportDTO codeValuesList = await LoadValues(importVersionUrl);
                //Pull in all categories from MCM
                LookupListCategoryImportDTO categoriesList = new LookupListCategoryImportDTO();
                if (ListID != DTO.Enums.Lists.HCPCSProcedures)
                {
                    categoriesList = await LoadCategories(categoriesUrl);
                }
                Logger.Debug("Deleting codes and categories with ListId equal to " + ListID);
                //Delete categories and codes for this ListID from the database
                await db.Database.ExecuteSqlCommandAsync("Delete from LookupListValues where ListId= {0}", ListID);
                await db.Database.ExecuteSqlCommandAsync("Delete from LookupListCategories where ListId= {0}", ListID);
                //Delete Span diagnosis and procedure categories only once
                if (this.ListID == DTO.Enums.Lists.ICD9Diagnosis || this.ListID == DTO.Enums.Lists.ICD9Procedures)
                {
                    await db.Database.ExecuteSqlCommandAsync("Delete from LookupListCategories where ListId= {0}", SpanListID);
                    await db.Database.ExecuteSqlCommandAsync("Delete from LookupListValues where ListId= {0}", SpanListID);
                }
                await db.SaveChangesAsync();
                    
                
                
                var i = 1;
                int numOfCategories = 0;
                if (this.ListID != DTO.Enums.Lists.HCPCSProcedures)
                {
                    numOfCategories = categoriesList.Categories.Count(); //for debugging purposes
                    foreach (var c in categoriesList.Categories)
                    {
                        var categoryname = c[0];

                        var result = new LookupListCategory
                        {
                            ListId = this.ListID,
                            CategoryName = categoryname,
                            CategoryId = i

                        };
                        if (result.CategoryName == "")
                        {
                            result.CategoryName = "UNDEFINED";
                        }
                        //Add new category to list that will update the database
                        categoriesToAdd.Add(result);
                        i = i + 1;

                    }
                }

                //Add categories for Span Diagnostics (only add once for diagnosis codes and once for procedure codes)
                if (this.ListID == DTO.Enums.Lists.ICD9Diagnosis || this.ListID == DTO.Enums.Lists.ICD9Procedures)
                {
                    var k = 1;
                    foreach (var c in categoriesList.Categories)
                    {
                        var categoryName = c[0];

                        var result = new LookupListCategory
                        {
                            ListId = SpanListID,
                            CategoryName = categoryName,
                            CategoryId = k

                        };
                        if (result.CategoryName == "")
                        {
                            result.CategoryName = "UNDEFINED";
                        }
                        spanCategoriesToAdd.Add(result);
                        k = k + 1;
                    }
                }

                Logger.Debug("Beginning of Update for  " + numOfCategories + "categories.");
                //Add the categories to database
                db.LookupListCategories.AddRange(categoriesToAdd);
                db.LookupListCategories.AddRange(spanCategoriesToAdd);

                Logger.Debug("Finished processing import of categories. Beginning Saving.");
                await db.SaveChangesAsync();
                Logger.Debug("Finished saving of categories.");
                //Add the codes to the database
                //empty code list:
                List<LookupListValue> codesToAdd = new List<LookupListValue>();
                var numCodes = codesToAdd.Count();
                //for ICD9 codes:
                if (this.ListID == Lpp.Dns.DTO.Enums.Lists.ICD9Diagnosis ||
                    this.ListID == Lpp.Dns.DTO.Enums.Lists.ICD9Diagnosis4Digits ||
                    this.ListID == Lpp.Dns.DTO.Enums.Lists.ICD9Diagnosis5Digits ||
                    this.ListID == Lpp.Dns.DTO.Enums.Lists.ICD9Procedures ||
                    this.ListID == Lpp.Dns.DTO.Enums.Lists.ICD9Procedures4Digits)
                {
                    foreach (var codeToAdd in codeValuesList.Codes)
                    {
                        var code = codeToAdd[0];
                        var codeUnformatted = codeToAdd[1];
                        var codeCategoryName = codeToAdd[2];
                        var codeName = codeToAdd[3];
                        var expiration = codeToAdd[4];

                        var codeCategory = categoriesToAdd.Where(ct => ct.CategoryName == codeCategoryName).FirstOrDefault();
                        
                        var categoryID = codeCategory.IsEmpty() ? 0 : codeCategory.CategoryId;

                        var newCode = new LookupListValue
                        {
                            ListId = this.ListID,
                            CategoryId = categoryID,
                            ItemName = codeName,
                            ItemCode = code,
                            ItemCodeWithNoPeriod = codeUnformatted,
                            ExpireDate = null

                        };
                        //also add code to Span
                        var newSpanCode = new LookupListValue
                        {
                            ListId = this.SpanListID,
                            CategoryId = categoryID,
                            ItemName = codeName,
                            ItemCode = code,
                            ItemCodeWithNoPeriod = codeUnformatted,
                            ExpireDate = null
                        };
                        if (!expiration.IsNullOrEmpty())
                        {
                            var year = expiration.Substring(0, 4).ToInt32();
                            var month = expiration.Substring(4, 2).ToInt32();
                            var day = expiration.Substring(6, 2).ToInt32();
                            DateTime expirationDate = new DateTime(year, month, day);
                            newCode.ExpireDate = expirationDate;
                            newSpanCode.ExpireDate = expirationDate;
                        }

                        codesToAdd.Add(newCode);
                        codesToAdd.Add(newSpanCode);
                    }
                    //push entire array
                    Logger.Debug("Beginning of Update for" + numCodes + "codes with ListId equal to " + ListID);
                    db.LookupListValues.AddRange(codesToAdd);
                }

                //Update for HCPCS codes
                if (this.ListID == Dns.DTO.Enums.Lists.HCPCSProcedures)
                {
                    foreach (var codeToAdd in codeValuesList.Codes)
                    {
                        var code = codeToAdd[0];
                        //var codeCategoryName = codeToAdd[1];
                        var codeDescription = codeToAdd[2];
                        var expiration = codeToAdd[3];
                        
                        //var codeCategory = categoriesToAdd.Where(ct => ct.CategoryName == codeCategoryName).FirstOrDefault();
                        //var categoryID = codeCategory.IsEmpty() ? 0 : codeCategory.CategoryId;

                        if (codeDescription == "" || codeDescription == null)
                        {
                            codeDescription = "UNDEFINED";
                        }

                        var newCode = new LookupListValue
                        {
                            ListId = this.ListID,
                            CategoryId = 0,
                            ItemName = codeDescription,
                            ItemCode = code,
                            ItemCodeWithNoPeriod = code,
                            ExpireDate = null
                        };
                        if (!expiration.IsNullOrEmpty())
                        {
                            var year = expiration.Substring(0, 4).ToInt32();
                            var month = expiration.Substring(4, 2).ToInt32();
                            var day = expiration.Substring(6, 2).ToInt32();
                            DateTime expirationDate = new DateTime(year, month, day);
                            newCode.ExpireDate = expirationDate;
                        }

                        if (code != null && code != "")
                            codesToAdd.Add(newCode);
                         
                    }
                    //Some repeated codes
                    //IEnumerable<LookupListValue> codesDistinct = codesToAdd.DistinctBy(a => new { a.ItemName, a.ItemCode, a.CategoryId }).ToList();
                    //push entire array
                    numCodes = codesToAdd.Count();
                    Logger.Debug("Beginning of Update for" + numCodes +  " codes with ListId equal to " + ListID);
                    db.LookupListValues.AddRange(codesToAdd);
                }
                    
                //Update for Generic Names
                if (this.ListID == Dns.DTO.Enums.Lists.GenericName)
                {
                    foreach (var codeToAdd in codeValuesList.Codes)
                    {
                        var name = codeToAdd[0];
                        var drugClass = codeToAdd[1];
                        var expiration = codeToAdd[2];

                        var codeCategory = categoriesToAdd.Where(ct => ct.CategoryName == drugClass).FirstOrDefault();
                        var categoryID = codeCategory.IsEmpty() ? 0 : codeCategory.CategoryId;

                        var newCode = new LookupListValue
                        {
                            ListId = this.ListID,
                            CategoryId = categoryID,
                            ItemName = name,
                            ItemCode = name,
                            ItemCodeWithNoPeriod = name,
                            ExpireDate = null
                        };

                        if (!expiration.IsNullOrEmpty())
                        {
                            var year = expiration.Substring(0, 4).ToInt32();
                            var month = expiration.Substring(4, 2).ToInt32();
                            var day = expiration.Substring(6, 2).ToInt32();
                            DateTime expirationDate = new DateTime(year, month, day);
                            newCode.ExpireDate = expirationDate;
                        }

                        if (name != null && name != "")
                            codesToAdd.Add(newCode);
                    }
                    Logger.Debug("Beginning of Update for" + numCodes + " codes with ListId equal to " + ListID);
                    db.LookupListValues.AddRange(codesToAdd);
                }   

                //Update for Drug Classes
                if (this.ListID == Dns.DTO.Enums.Lists.DrugClass)
                {
                    foreach (var codeToAdd in codeValuesList.Codes)
                    {
                        var drugClass = codeToAdd[0];
                        var expiration = codeToAdd[1];

                        var codeCategory = categoriesToAdd.Where(ct => ct.CategoryName == drugClass).FirstOrDefault();
                        var categoryID = codeCategory.IsEmpty() ? 0 : codeCategory.CategoryId;

                        var newCode = new LookupListValue
                        {
                            ListId = this.ListID,
                            CategoryId = categoryID,
                            ItemName = drugClass,
                            ItemCode = drugClass,
                            ItemCodeWithNoPeriod = "''",
                            ExpireDate = null
                        };
                        
                        if (!expiration.IsNullOrEmpty())
                        {
                            var year = expiration.Substring(0, 4).ToInt32();
                            var month = expiration.Substring(4, 2).ToInt32();
                            var day = expiration.Substring(6, 2).ToInt32();
                            DateTime expirationDate = new DateTime(year, month, day);
                            newCode.ExpireDate = expirationDate;
                        }
                        if (drugClass != null && drugClass != "")
                            codesToAdd.Add(newCode);

                    }
                    Logger.Debug("Beginning of Update for " + numCodes + " codes with ListId equal to " + ListID);
                    db.LookupListValues.AddRange(codesToAdd);
                }

                LookupList currentLookupList = db.LookupLists.Where(l => l.ListId == ListID).FirstOrDefault();
                currentLookupList.Version = latestVersion;
                if (SpanListID == DTO.Enums.Lists.SPANDiagnosis || SpanListID == DTO.Enums.Lists.SPANProcedure)
                {
                    LookupList currentSpanLookupList = db.LookupLists.Where(s => s.ListId == SpanListID).FirstOrDefault();
                    currentSpanLookupList.Version = latestVersion;
                }
                Logger.Debug("Finished processing import of codes. Beginning Saving.");
                await db.SaveChangesAsync();
                Logger.Debug("Finished Saving of codes.");

            }
           
        }

        async Task<DTO.LookupListsImportDTO> LoadVersions()
        {
            try
            {
                using (var web = new HttpClient())
                {
                    web.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(serviceUsername + ":" + servicePassword)));

                    Logger.Debug("Beginning request of importing Versions from: " + versionUrl);
                    
                    using (var stream = await web.GetStreamAsync(versionUrl))
                    using (var jReader = new Newtonsoft.Json.JsonTextReader(new System.IO.StreamReader(stream)))
                    {
                        var serializer = Newtonsoft.Json.JsonSerializer.CreateDefault();

                        Logger.Debug("Deserializing Versions list from: " + versionUrl);
                        return serializer.Deserialize<DTO.LookupListsImportDTO>(jReader);
                    }
                }
            }
            catch (HttpRequestException httpex)
            {
                Logger.Error("Error getting versions from external service: " + versionUrl, httpex);

                StatusCode = HttpStatusCode.ServiceUnavailable;
                StatusMessage = "There was an error communicating with the external service.";

                return new DTO.LookupListsImportDTO();
            }
        }

        async Task<DTO.LookupListValuesImportDTO> LoadValues (string compareVersionsUrl)
        {
            try
            {
                using (var web = new HttpClient())
                {
                    web.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(serviceUsername + ":" + servicePassword)));

                    Logger.Debug("Beginning request of importing code list from: " + versionUrl);

                    using (var stream = await web.GetStreamAsync(compareVersionsUrl))
                    using (var jReader = new Newtonsoft.Json.JsonTextReader(new System.IO.StreamReader(stream)))
                    {
                        var serializer = Newtonsoft.Json.JsonSerializer.CreateDefault();

                        Logger.Debug("Deserializing list of codes from: " + versionUrl);
                        return serializer.Deserialize<DTO.LookupListValuesImportDTO>(jReader);
                    }
                }
            }
            catch (HttpRequestException httpex)
            {
                Logger.Error("Error getting codes from external service: " + versionUrl, httpex);

                StatusCode = HttpStatusCode.ServiceUnavailable;
                StatusMessage = "There was an error communicating with the external service.";

                return new DTO.LookupListValuesImportDTO();
            }

        }

        async Task<DTO.LookupListCategoryImportDTO> LoadCategories(string categoriesUrl)
        {
            try
            {
                using (var web = new HttpClient())
                {
                    web.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(serviceUsername + ":" + servicePassword)));

                    Logger.Debug("Beginning request of importing categories list from: " + versionUrl);

                    using (var stream = await web.GetStreamAsync(categoriesUrl))
                    using (var jReader = new Newtonsoft.Json.JsonTextReader(new System.IO.StreamReader(stream)))
                    {
                        var serializer = Newtonsoft.Json.JsonSerializer.CreateDefault();

                        Logger.Debug("Deserializing list of categories from: " + versionUrl);
                        return serializer.Deserialize<DTO.LookupListCategoryImportDTO>(jReader);
                    }
                }
            }

            catch (HttpRequestException httpex)
            {
                Logger.Error("Error getting categories from external service: " + versionUrl, httpex);

                StatusCode = HttpStatusCode.ServiceUnavailable;
                StatusMessage = "There was an error communicating with the external service.";

                return new DTO.LookupListCategoryImportDTO();
            }
        }
    }

}