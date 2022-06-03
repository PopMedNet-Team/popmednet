using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;

namespace PopMedNet.Adapters.AcceptanceTests.TOMSync
{
    [TestClass]
    public class CodeListUpdatingTests
    {
        readonly string serviceUrl = null;
        readonly string serviceUsername = null;
        readonly string servicePassword = null;
        readonly IEnumerable<ListDefinition> listDefinitions;

        public CodeListUpdatingTests()
        {
            serviceUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["LookupLists.Url"];
            serviceUsername = (System.Web.Configuration.WebConfigurationManager.AppSettings["LookupLists.Import.User"] ?? string.Empty).DecryptString();
            servicePassword = (System.Web.Configuration.WebConfigurationManager.AppSettings["LookupLists.Import.Password"] ?? string.Empty).DecryptString();

            listDefinitions = new[] {
                new ListDefinition(serviceUrl, "rx", "fdb", "fdb-etc", -1, Lists.GenericName),
                new ListDefinition(serviceUrl, "rx", "fdb", "fdb-etc", -1, Lists.DrugClass),
                new ListDefinition(serviceUrl, "dx", "optum", "icd-9-cm", 3, Lists.ICD9Diagnosis),
                new ListDefinition(serviceUrl, "dx", "optum", "icd-9-cm", 4, Lists.ICD9Diagnosis4Digits),
                new ListDefinition(serviceUrl, "dx", "optum", "icd-9-cm", 5, Lists.ICD9Diagnosis5Digits),
                new ListDefinition(serviceUrl, "px", "optum", "icd-9-cm", 3, Lists.ICD9Procedures),
                new ListDefinition(serviceUrl, "px", "optum", "icd-9-cm", 4, Lists.ICD9Procedures4Digits),
                new ListDefinition(serviceUrl, "px", "optum", "hcpcs-cpt", -1, Lists.HCPCSProcedures)
            };
        }

        [TestMethod]
        public async Task GetVersions()
        {
            using (var http = CreateHttpClient())
            {
                foreach (var list in listDefinitions)
                {
                    await GetVersionAsync(http, list);

                }
            }
        }

        [TestMethod]
        public async Task GetCategories()
        {
            using (var http = CreateHttpClient())
            {
                foreach (var list in listDefinitions)
                {
                    await GetVersionAsync(http, list);

                    Console.WriteLine(list.CategoriesUrl);

                    var categoriesResult = await GetListCategoriesAsync(http, list);

                    if (categoriesResult == null)
                        continue;

                    Assert.IsTrue(categoriesResult.Metadata.results.count > 0, "No categories returned by the service. Url:" + list.CategoriesUrl);
                }
            }
        }

        [TestMethod]
        public async Task GetValues()
        {
            using (var http = CreateHttpClient())
            {
                foreach (var list in listDefinitions)
                {
                    await GetVersionAsync(http, list);

                    var valuesResult = await GetListValuesAsync(http, list);

                    if(valuesResult == null)
                    {
                        Assert.Fail("No result returned for getting values from:" + list.ValuesUrl);
                        return;
                    }

                    Assert.IsTrue(valuesResult.Metadata.results.count > 0, "No values returned by the service. Url:" + list.ValuesUrl);
                   
                }
            }
        }

        HttpClient CreateHttpClient()
        {
            var web = new HttpClient();
            web.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(serviceUsername + ":" + servicePassword)));
            return web;
        }

        async Task<LookupListsImportDTO> GetVersionAsync(HttpClient web, ListDefinition listDefinition)
        {
            string result;
            using (var stream = await web.GetStreamAsync(listDefinition.VersionUrl))
            using (var reader = new System.IO.StreamReader(stream))
            {
                result = await reader.ReadToEndAsync();
                //Console.WriteLine(result);
            }

            Assert.IsNotNull(result);

            var obj = Newtonsoft.Json.Linq.JObject.Parse(result);
            if (obj.ContainsKey("error"))
            {
                //connection failed
                Assert.Fail(obj["error"]["message"].ToString());
                return null;
            }

            var listVersion = obj.ToObject<LookupListsImportDTO>();
            Assert.IsNotNull(listVersion, "Unable to convert json to LookupListsImportDTO");

            //Console.WriteLine(listVersion.Results.LatestVersion.url + "  Version:" + listVersion.Results.LatestVersion.VersionID);

            listDefinition.SetLatestVersion(listVersion.Results.LatestVersion);
            return listVersion;
        }

        async Task<LookupListCategoryImportDTO> GetListCategoriesAsync(HttpClient web, ListDefinition listDefinition)
        {
            if (string.IsNullOrEmpty(listDefinition.CategoriesUrl))
            {
                return null;
            }

            string result;
            using (var stream = await web.GetStreamAsync(listDefinition.CategoriesUrl))
            using (var reader = new System.IO.StreamReader(stream))
            {
                result = await reader.ReadToEndAsync();
                //Console.WriteLine(result);
            }

            Assert.IsNotNull(result);

            var obj = Newtonsoft.Json.Linq.JObject.Parse(result);
            if (obj.ContainsKey("error"))
            {
                //connection failed
                Assert.Fail(obj["error"]["message"].ToString());
                return null;
            }

            var categories = obj.ToObject<LookupListCategoryImportDTO>();
            Assert.IsNotNull(categories, "Unable to convert json to LookupListCategoryImportDTO");

            return categories;
        }

        async Task<LookupListValuesImportDTO> GetListValuesAsync(HttpClient web, ListDefinition listDefinition)
        {
            if (string.IsNullOrEmpty(listDefinition.ValuesUrl))
            {
                return null;
            }

            string result;
            using (var stream = await web.GetStreamAsync(listDefinition.ValuesUrl))
            using (var reader = new System.IO.StreamReader(stream))
            {
                result = await reader.ReadToEndAsync();
                //Console.WriteLine(result);
            }

            Assert.IsNotNull(result);

            var obj = Newtonsoft.Json.Linq.JObject.Parse(result);
            if (obj.ContainsKey("error"))
            {
                //connection failed
                Assert.Fail(obj["error"]["message"].ToString());
                return null;
            }

            var values = obj.ToObject<LookupListValuesImportDTO>();
            Assert.IsNotNull(values, "Unable to convert json to LookupListValuesImportDTO");

            return values;
        }

        class ListDefinition
        {
            readonly string _category;
            readonly string _source;
            readonly string _codeClass;
            readonly int _codeLength;
            readonly Lists _listType;
            readonly string _baseUrl;
            LookupListsVersionImport _latestVersion = null;

            public ListDefinition(string baseUrl, string category, string source, string codeClass, int codeLength, Lpp.Dns.DTO.Enums.Lists listType)
            {
                _category = category;
                _source = source;
                _codeClass = codeClass;
                _codeLength = codeLength;
                _listType = listType;
                _baseUrl = baseUrl;
            }

            public string VersionUrl
            {
                get
                {
                    return $"{_baseUrl}/{_category}/{_source}/{_codeClass}";
                }
            }

            public string CategoriesUrl
            {
                get
                {
                    switch (_listType)
                    {
                        case Lists.GenericName:
                            return _latestVersion.url + "/code?columns=drug_class";
                        case Lists.DrugClass:
                            return _latestVersion.url + "/code?columns=drug_class";
                        case Lists.ICD9Diagnosis:
                        case Lists.ICD9Diagnosis4Digits:
                        case Lists.ICD9Diagnosis5Digits:
                        case Lists.ICD9Procedures:
                        case Lists.ICD9Procedures4Digits:
                            return _latestVersion.url + "/code?columns=category";
                        default:
                            return string.Empty;
                    }
                }
            }

            public string ValuesUrl
            {
                get
                {
                    switch (_listType)
                    {
                        case Lists.GenericName:
                            return _latestVersion.url + "/code?columns=generic_name,drug_class,data_end";
                        case Lists.DrugClass:
                            return _latestVersion.url + "/code?columns=drug_class,data_end";
                        case Lists.ICD9Diagnosis:
                        case Lists.ICD9Diagnosis4Digits:
                        case Lists.ICD9Diagnosis5Digits:
                        case Lists.ICD9Procedures:
                        case Lists.ICD9Procedures4Digits:
                            return _latestVersion.url + "/code?" + _codeLength + "&columns=code,code_unformatted,category,short_description,data_end";
                        case Lists.HCPCSProcedures:
                            return _latestVersion.url + "/code?columns=code,category,short_description,data_end";
                        default:
                            return string.Empty;
                    }
                }
            }


            public void SetLatestVersion(LookupListsVersionImport version)
            {
                _latestVersion = version;
            }

        }
    }
}
