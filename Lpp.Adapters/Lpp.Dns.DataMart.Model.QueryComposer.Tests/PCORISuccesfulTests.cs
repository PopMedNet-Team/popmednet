using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lpp.Dns.DataMart.Model.QueryComposer;
using System.Collections.Generic;
using Lpp.Dns.DTO.QueryComposer;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Tests
{
    [TestClass]
    public class PCORISuccesfulTests
    {
        #region "Shared Methods"
        private static Dictionary<string, object> GetAdapterSettingsAUH()
        {
            //Create and return the settings for the adapter here.
            Dictionary<string, object> settings = new Dictionary<string, object>();
            settings.Add("Server", "(local)"); //The SQL Server
            settings.Add("UserID", "cdmv3"); //The sql user account to use
            settings.Add("Password", "Password1!"); //The password of the sql user
            settings.Add("Database", "CDMv3_AUH_DM"); //The database to connect to

            settings.Add("DataProvider", "SQLServer"); //SQL Server when using MS SQL.

            //Ignore
            settings.Add("Port", null); //The Port to connect to
            settings.Add("DataSourceName", null);
            settings.Add("LowThresholdValue", null);
            return settings;
        }

        private static Dictionary<string, object> GetAdapterSettingsBAR()
        {
            //Create and return the settings for the adapter here.
            Dictionary<string, object> settings = new Dictionary<string, object>();
            settings.Add("Server", "(local)"); //The SQL Server
            settings.Add("UserID", "cdmv3"); //The sql user account to use
            settings.Add("Password", "Password1!"); //The password of the sql user
            settings.Add("Database", "CDMv3_BAR_DM"); //The database to connect to

            settings.Add("DataProvider", "SQLServer"); //SQL Server when using MS SQL.

            //Ignore
            settings.Add("Port", null); //The Port to connect to
            settings.Add("DataSourceName", null);
            settings.Add("LowThresholdValue", null);
            return settings;
        }

        private QueryComposerResponseDTO ExecuteRequest(string json, Dictionary<string, object> settings)
        {
            QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(json);

            //The connection string to the Adapter's database.
            Adapters.PCORI.PCORIModelAdapter pcori = new Adapters.PCORI.PCORIModelAdapter(new RequestMetadata
            {
                CreatedOn = DateTime.UtcNow,
                MSRequestID = "Unit Test Request"
            });
            pcori.Initialize(settings);

            //Execute the query
            return pcori.Execute(dto, false);
        }
        #endregion

        [TestMethod]
        //This unit test tests the below question
        //Drug code PredniSONE (0054-0017) ,  PredniSONE (0054-0018)  , or PredniSONE (0054-0019)
        public void Executdrugcodeprednisone()
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"b959f58f-1f75-42a6-bddd-1d908250350c\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"2b298144-2ed5-473c-be78-a0ba7e83acf6\",\"Index\":1,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"0054-0017\",\"Name\":\"PredniSONE\"},{\"Code\":\"0054-0018\",\"Name\":\"PredniSONE\"},{\"Code\":\"0054-0019\",\"Name\":\"PredniSONE\"}]},\"Criteria\":[]}],\"Type\":0}]},\"Select\":{\"Fields\":[]}}";
            //QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(json);
            QueryComposerResponseDTO currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

            //The Response object basically is a collection of collection of results.
            List<Dictionary<string, object>> response = ((System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>[])(currentResponse.Results))[0];
            Assert.IsFalse(response.Count == 0, "The query executed successfully but no results were returned.");


            //Run against BAR Datamart
            /* currentResponse = ExecuteRequest(json, GetAdapterSettingsBAR());

             //The Response object basically is a collection of collection of results.
             List<Dictionary<string, object>> responseBAR = ((System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>[])(currentResponse.Results))[0];
             Assert.IsFalse(responseBAR.Count == 0, "The query executed successfully but no results were returned.");*/
        }

        [TestMethod]
        public void ExecuteBristolMyersUseCase1()
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"10b65c4a-7bb8-415c-b04c-0e0c05681d19\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"d9dd6e82-bbca-466a-8022-b54ff3d99a3c\",\"Values\":{\"MinAge\":30,\"MaxAge\":60,\"BirthStartDate\":null,\"BirthEndDate\":null,\"Type\":0,\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"914c8f95-252e-4724-86b0-49a8cacfce5d\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"aa336d3c-ce0e-4f63-8ddc-f957da9109ec\",\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"9da5ddce-9bb7-4cd9-b706-15d92b09e8c2\",\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"8b48770f-37cd-49ed-88d2-b3d6390bc155\",\"Values\":{\"Type\":1},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"71b4545c-345b-48b2-af5e-f84dc18e4e1a\",\"Values\":{\"Sex\":[2],\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"3c275559-429f-47e5-b8c8-23d02c960812\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"154752ab-10d1-4e6b-80b3-7fdadf3dca7f\",\"Values\":{\"Genders\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"cf3065bb-1d06-4519-b59f-5332c3d1e7ca\",\"Values\":{\"Genders\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"f173b6f8-cb6a-4e96-a162-174cd0b78514\",\"Values\":{\"Genders\":[]},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"5e5020dc-c0e4-487f-adf2-45431c2b7695\",\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"359.1\",\"Name\":\"HEREDIT PROGRESSIVE MUSC DYSTROPHY\"}]},\"Criteria\":[]}],\"Type\":0},{\"ID\":\"3ba77e8b-2901-493d-93da-6067ebac0ef9\",\"Name\":\"Paragraph\",\"Operator\":\"2\",\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"5e5020dc-c0e4-487f-adf2-45431c2b7695\",\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"401\",\"Name\":\"ESSENTIAL HYPERTENSION\"},{\"Code\":\"405\",\"Name\":\"SECONDARY HYPERTENSION\"}]},\"Criteria\":[]}],\"Type\":0}]},\"Select\":{\"Fields\":[]}}";
            QueryComposerResponseDTO currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

            //The Response object basically is a collection of collection of results.
            List<Dictionary<string, object>> response = ((System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>[])(currentResponse.Results))[0];
            Assert.IsFalse(response.Count == 0, "The query executed successfully but no results were returned.");
        }
    }
}
