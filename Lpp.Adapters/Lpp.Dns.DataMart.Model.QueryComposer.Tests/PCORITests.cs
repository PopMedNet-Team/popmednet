using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lpp.Dns.DataMart.Model.QueryComposer;
using System.Collections.Generic;
using Lpp.Dns.DTO.QueryComposer;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Tests
{
    [TestClass]
    public class PCORITests
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

        private const string ResourceFolder = "../Resources/QueryComposition";

        private QueryComposerResponseQueryResultDTO ExecuteRequest(string json, Dictionary<string, object> settings)
        {
            var dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerQueryDTO>(json);

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
        public void ExecuteGenderBreakdown()
        {
            //Request JSON goes here.
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"b6063da5-c364-4a0f-b551-134d58d5fc24\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"71b4545c-345b-48b2-af5e-f84dc18e4e1a\",\"Values\":{\"Sex\":[1,2,3,4],\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"e229308b-b930-41b8-b5ff-445bb2412e86\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"154752ab-10d1-4e6b-80b3-7fdadf3dca7f\",\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"cf3065bb-1d06-4519-b59f-5332c3d1e7ca\",\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"f173b6f8-cb6a-4e96-a162-174cd0b78514\",\"Values\":{\"Sex\":[]},\"Criteria\":[]}],\"Type\":0}]}],\"Type\":0}]},\"Select\":{\"Fields\":[]}}";
            var currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

            //The Response object basically is a collection of collection of results.
            List<Dictionary<string, object>> response = ((System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>[])(currentResponse.Results))[0];

            //foreach(var item in response){
            //    if (item.ContainsKey("M"))
            //    {
            //        object val;
            //        if (item.TryGetValue("M", out val))
            //        {
            //            if (val.ToString() == "49962")
            //            {
            //                //Success- we have 49962 males
            //                Assert.Fail("Just checking");
            //            }
            //            else
            //            {
            //                Assert.Fail("Male count didn't match");
            //            }
            //        }
            //    }
            //}
            Assert.IsFalse(response.Count == 0, "The query executed successfully but no results were returned.");
        }

        [TestMethod]
        public void ExecuteQuery_PatientsWithBrokenBones()
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"820edc47-31bc-4b4e-bbec-67c9b2f6a42a\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"d9dd6e82-bbca-466a-8022-b54ff3d99a3c\",\"Values\":{\"MinAge\":20,\"MaxAge\":50,\"BirthStartDate\":null,\"BirthEndDate\":null,\"Type\":0,\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"7cf93f5f-7cf2-4d6f-8297-fe43c1df29fd\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"aa336d3c-ce0e-4f63-8ddc-f957da9109ec\",\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"9da5ddce-9bb7-4cd9-b706-15d92b09e8c2\",\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"8b48770f-37cd-49ed-88d2-b3d6390bc155\",\"Values\":{\"Type\":1},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"5e5020dc-c0e4-487f-adf2-45431c2b7695\",\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"733.1\",\"Name\":\"PATHOLOGIC FRACTURE\"},{\"Code\":\"807\",\"Name\":\"FRACTURE RIB STERNUM LARYNX&TRACHEA\"}]},\"Criteria\":[]}],\"Type\":0}]},\"Select\":{\"Fields\":[]}}";
            var currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

            //The Response object basically is a collection of collection of results.
            List<Dictionary<string, object>> response = ((System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>[])(currentResponse.Results))[0];
            Assert.IsFalse(response.Count == 0, "The query executed successfully but no results were returned.");
        }

        [TestMethod]
        public void ExecuteBristolMyersUseCase1() 
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"10b65c4a-7bb8-415c-b04c-0e0c05681d19\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"d9dd6e82-bbca-466a-8022-b54ff3d99a3c\",\"Values\":{\"MinAge\":30,\"MaxAge\":60,\"BirthStartDate\":null,\"BirthEndDate\":null,\"Type\":0,\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"914c8f95-252e-4724-86b0-49a8cacfce5d\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"aa336d3c-ce0e-4f63-8ddc-f957da9109ec\",\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"9da5ddce-9bb7-4cd9-b706-15d92b09e8c2\",\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"8b48770f-37cd-49ed-88d2-b3d6390bc155\",\"Values\":{\"Type\":1},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"71b4545c-345b-48b2-af5e-f84dc18e4e1a\",\"Values\":{\"Sex\":[2],\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"3c275559-429f-47e5-b8c8-23d02c960812\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"154752ab-10d1-4e6b-80b3-7fdadf3dca7f\",\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"cf3065bb-1d06-4519-b59f-5332c3d1e7ca\",\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"f173b6f8-cb6a-4e96-a162-174cd0b78514\",\"Values\":{\"Sex\":[]},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"5e5020dc-c0e4-487f-adf2-45431c2b7695\",\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"359.1\",\"Name\":\"HEREDIT PROGRESSIVE MUSC DYSTROPHY\"}]},\"Criteria\":[]}],\"Type\":0},{\"ID\":\"3ba77e8b-2901-493d-93da-6067ebac0ef9\",\"Name\":\"Paragraph\",\"Operator\":\"2\",\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"5e5020dc-c0e4-487f-adf2-45431c2b7695\",\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"401\",\"Name\":\"ESSENTIAL HYPERTENSION\"},{\"Code\":\"405\",\"Name\":\"SECONDARY HYPERTENSION\"}]},\"Criteria\":[]}],\"Type\":0}]},\"Select\":{\"Fields\":[]}}";
            var currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

            //The Response object basically is a collection of collection of results.
            List<Dictionary<string, object>> response = ((System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>[])(currentResponse.Results))[0];
            Assert.IsFalse(response.Count == 0, "The query executed successfully but no results were returned.");
        }

        [TestMethod]
        public void ExecuteBristolMyersUseCase2()
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"abf41953-bf82-4cf4-b600-b8836921f13c\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"d9dd6e82-bbca-466a-8022-b54ff3d99a3c\",\"Values\":{\"MinAge\":18,\"MaxAge\":55,\"BirthStartDate\":null,\"BirthEndDate\":null,\"Type\":0,\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"e9c982c7-708d-4a3a-b0a2-1142b7323811\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"aa336d3c-ce0e-4f63-8ddc-f957da9109ec\",\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"9da5ddce-9bb7-4cd9-b706-15d92b09e8c2\",\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"8b48770f-37cd-49ed-88d2-b3d6390bc155\",\"Values\":{\"Type\":1},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"71b4545c-345b-48b2-af5e-f84dc18e4e1a\",\"Values\":{\"Sex\":[1,2,4],\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"9358839b-9ce9-4345-8edc-05f81ebf9814\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"154752ab-10d1-4e6b-80b3-7fdadf3dca7f\",\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"cf3065bb-1d06-4519-b59f-5332c3d1e7ca\",\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"f173b6f8-cb6a-4e96-a162-174cd0b78514\",\"Values\":{\"Sex\":[]},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"98a78326-35d2-461a-b858-5b69e0fed28a\",\"Values\":{\"StartDate\":\"2010-01-01T05:00:00.000Z\",\"EndDate\":\"2011-01-01T05:00:00.000Z\"},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"5e5020dc-c0e4-487f-adf2-45431c2b7695\",\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"042\",\"Name\":\"HUMAN IMMUNODEFICIENCY VIRUS [HIV]\"}]},\"Criteria\":[]}],\"Type\":0},{\"ID\":\"1072f921-9402-4948-8e34-b3204ba0abc9\",\"Name\":\"Paragraph\",\"Operator\":\"2\",\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"5e5020dc-c0e4-487f-adf2-45431c2b7695\",\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"426.82\",\"Name\":\"LONG QT SYNDROME\"}]},\"Criteria\":[]}],\"Type\":0}]},\"Select\":{\"Fields\":[]}}";
            var dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerQueryDTO>(json);
            var currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

            //The Response object basically is a collection of collection of results.
            List<Dictionary<string, object>> response = ((System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>[])(currentResponse.Results))[0];
            Assert.IsFalse(response.Count == 0, "The query executed successfully but no results were returned.");


            //Run against BAR Datamart
            currentResponse = ExecuteRequest(json, GetAdapterSettingsBAR());

            //The Response object basically is a collection of collection of results.
            List<Dictionary<string, object>> responseBAR = ((System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>[])(currentResponse.Results))[0];
            Assert.IsFalse(responseBAR.Count == 0, "The query executed successfully but no results were returned.");
        }

        [TestMethod]
        public void ExecuteHypertensionQuery()
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"f9562b3e-77e7-490a-8207-c048ae88f6f5\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"71b4545c-345b-48b2-af5e-f84dc18e4e1a\",\"Index\":1,\"Values\":{\"Sex\":[2],\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"310465e3-a2bc-4edd-8446-a13ed85929ea\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"154752ab-10d1-4e6b-80b3-7fdadf3dca7f\",\"Index\":2,\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"cf3065bb-1d06-4519-b59f-5332c3d1e7ca\",\"Index\":3,\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"f173b6f8-cb6a-4e96-a162-174cd0b78514\",\"Index\":4,\"Values\":{\"Sex\":[]},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"5e5020dc-c0e4-487f-adf2-45431c2b7695\",\"Index\":5,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"401\",\"Name\":\"ESSENTIAL HYPERTENSION\"},{\"Code\":\"405\",\"Name\":\"SECONDARY HYPERTENSION\"}]},\"Criteria\":[]}],\"Type\":0},{\"ID\":\"8803dd0f-6dc7-4d8a-b514-ca6b1e9c75ee\",\"Name\":\"Paragraph\",\"Operator\":\"2\",\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"5e5020dc-c0e4-487f-adf2-45431c2b7695\",\"Index\":6,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"402\",\"Name\":\"HYPERTENSIVE HEART DISEASE\"}]},\"Criteria\":[]}],\"Type\":0}]},\"Select\":{\"Fields\":[]}}";
            var dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerQueryDTO>(json);
            var currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

            //The Response object basically is a collection of collection of results.
            List<Dictionary<string, object>> response = ((System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>[])(currentResponse.Results))[0];
            Assert.IsFalse(response.Count == 0, "The query executed successfully but no results were returned.");


            //Run against BAR Datamart
            currentResponse = ExecuteRequest(json, GetAdapterSettingsBAR());

            //The Response object basically is a collection of collection of results.
            List<Dictionary<string, object>> responseBAR = ((System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>[])(currentResponse.Results))[0];
            Assert.IsFalse(responseBAR.Count == 0, "The query executed successfully but no results were returned.");
        }

        [TestMethod]
        //This unit test tests the combination of sex, Age, Race ethnicity, tobacco use, icd-9 diagnosis, observation period AND NOT icd-9 diagnosis, observation period AND icd-9 diagnosis,observation period
        //Identify white female patients over 40 years old who are current smokers and had a diagnosis of diabetes between 2010-2013, 
        //who DID NOT have any diagnoses for hypertension or stroke during the same period 
        //AND who had a diagnosis of stroke in 2014.
        //identifier # 10199

        public void Excutecurrentfemalesmokersanddiabetes()
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"434efcd6-2872-4236-b1d9-972f346eb8b5\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"702ce918-9a59-4082-a8c7-a9234536fe79\",\"Index\":1,\"Values\":{\"Races\":[5],\"Ethnicities\":[],\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"d733a3f3-7628-4900-a009-1f6173d0d480\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"f8aa2421-00ef-4ac4-9591-4d43a2a6c80d\",\"Index\":2,\"Values\":{\"Races\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"419df1e0-f0cb-43cb-a680-974f1611f3ef\",\"Index\":3,\"Values\":{\"Races\":[],\"Ethnicities\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"bd0e5691-473e-445c-9b45-6d7f1db51269\",\"Index\":4,\"Values\":{\"Races\":[],\"Ethnicities\":[]},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"71b4545c-345b-48b2-af5e-f84dc18e4e1a\",\"Index\":5,\"Values\":{\"Sex\":[1],\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"cc6983b8-4ffd-4d9a-84d8-08606b9d28e2\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"154752ab-10d1-4e6b-80b3-7fdadf3dca7f\",\"Index\":6,\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"cf3065bb-1d06-4519-b59f-5332c3d1e7ca\",\"Index\":7,\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"f173b6f8-cb6a-4e96-a162-174cd0b78514\",\"Index\":8,\"Values\":{\"Sex\":[]},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"d9dd6e82-bbca-466a-8022-b54ff3d99a3c\",\"Index\":9,\"Values\":{\"MinAge\":40,\"MaxAge\":80,\"BirthStartDate\":null,\"BirthEndDate\":null,\"Type\":0,\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"63007bab-bf48-4abc-84b4-04e832ad70ac\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"aa336d3c-ce0e-4f63-8ddc-f957da9109ec\",\"Index\":10,\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"9da5ddce-9bb7-4cd9-b706-15d92b09e8c2\",\"Index\":11,\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"8b48770f-37cd-49ed-88d2-b3d6390bc155\",\"Index\":12,\"Values\":{\"Type\":1},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"342c354b-9ecc-479b-be61-1770e4b44675\",\"Index\":13,\"Values\":{\"TobaccoUses\":[1]},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"5e5020dc-c0e4-487f-adf2-45431c2b7695\",\"Index\":14,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"250\",\"Name\":\"DIABETES MELLITUS\"}]},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"98a78326-35d2-461a-b858-5b69e0fed28a\",\"Index\":15,\"Values\":{\"StartDate\":\"2010-01-01T05:00:00.000Z\",\"EndDate\":\"2013-12-31T05:00:00.000Z\"},\"Criteria\":[]}],\"Type\":0},{\"ID\":\"1612077b-c89a-4297-9fd0-f7fe93b8f206\",\"Name\":\"Paragraph\",\"Operator\":\"2\",\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"5e5020dc-c0e4-487f-adf2-45431c2b7695\",\"Index\":16,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"401\",\"Name\":\"ESSENTIAL HYPERTENSION\"},{\"Code\":\"405\",\"Name\":\"SECONDARY HYPERTENSION\"},{\"Code\":\"416.0\",\"Name\":\"PRIMARY PULMONARY HYPERTENSION\"},{\"Code\":\"459.3\",\"Name\":\"CHRONIC VENOUS HYPERTENSION\"}]},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"98a78326-35d2-461a-b858-5b69e0fed28a\",\"Index\":17,\"Values\":{\"StartDate\":\"2010-01-01T05:00:00.000Z\",\"EndDate\":\"2013-12-31T05:00:00.000Z\"},\"Criteria\":[]}],\"Type\":0},{\"ID\":\"eb4bc03f-9883-4905-88a5-df94d9f6f6ab\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"5e5020dc-c0e4-487f-adf2-45431c2b7695\",\"Index\":1,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"401\",\"Name\":\"ESSENTIAL HYPERTENSION\"},{\"Code\":\"405\",\"Name\":\"SECONDARY HYPERTENSION\"},{\"Code\":\"416.0\",\"Name\":\"PRIMARY PULMONARY HYPERTENSION\"},{\"Code\":\"459.3\",\"Name\":\"CHRONIC VENOUS HYPERTENSION\"}]},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"98a78326-35d2-461a-b858-5b69e0fed28a\",\"Index\":2,\"Values\":{\"StartDate\":\"2014-01-01T05:00:00.000Z\",\"EndDate\":\"2014-12-31T05:00:00.000Z\"},\"Criteria\":[]}],\"Type\":0}]},\"Select\":{\"Fields\":[]}}";

            //QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(json);
            var currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

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
        //This unit test tests the combination of age,sex, race ethnicity AND Icd-9 diagnosis
        //Identify Male Asian patients who are above 25 and have Thyroid and mental disorder (ICD 292.89, 295.4, 246.9
        //identifier # 10201
        public void ExecuteMaleAsianThyrodandmentaldisorder()
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"df80b0be-dd60-46f0-a055-65e87371c2d3\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"71b4545c-345b-48b2-af5e-f84dc18e4e1a\",\"Index\":1,\"Values\":{\"Sex\":[2],\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"344d8efd-8ab2-495c-ab81-a3e49085aec2\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"154752ab-10d1-4e6b-80b3-7fdadf3dca7f\",\"Index\":2,\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"cf3065bb-1d06-4519-b59f-5332c3d1e7ca\",\"Index\":3,\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"f173b6f8-cb6a-4e96-a162-174cd0b78514\",\"Index\":4,\"Values\":{\"Sex\":[]},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"702ce918-9a59-4082-a8c7-a9234536fe79\",\"Index\":5,\"Values\":{\"Races\":[2],\"Ethnicities\":[],\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"5d2a539b-d34e-4eb4-9a91-9e0bda806327\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"f8aa2421-00ef-4ac4-9591-4d43a2a6c80d\",\"Index\":6,\"Values\":{\"Races\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"419df1e0-f0cb-43cb-a680-974f1611f3ef\",\"Index\":7,\"Values\":{\"Races\":[],\"Ethnicities\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"bd0e5691-473e-445c-9b45-6d7f1db51269\",\"Index\":8,\"Values\":{\"Races\":[],\"Ethnicities\":[]},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"d9dd6e82-bbca-466a-8022-b54ff3d99a3c\",\"Index\":9,\"Values\":{\"MinAge\":25,\"MaxAge\":null,\"BirthStartDate\":null,\"BirthEndDate\":null,\"Type\":0,\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"890f98a3-d04d-4cc3-bd37-6ceedc06d288\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"aa336d3c-ce0e-4f63-8ddc-f957da9109ec\",\"Index\":10,\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"9da5ddce-9bb7-4cd9-b706-15d92b09e8c2\",\"Index\":11,\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"8b48770f-37cd-49ed-88d2-b3d6390bc155\",\"Index\":12,\"Values\":{\"Type\":1},\"Criteria\":[]}],\"Type\":0}]}],\"Type\":0},{\"ID\":\"8daf1d74-fe7f-4f0e-b636-717c70673261\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"5e5020dc-c0e4-487f-adf2-45431c2b7695\",\"Index\":13,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"292.89\",\"Name\":\"OTH SPEC DRUG-INDUCD MENTL DISORDER\"},{\"Code\":\"295.4\",\"Name\":\"SCHIZOPHRENIFORM DISORDER\"},{\"Code\":\"246.9\",\"Name\":\"UNSPECIFIED DISORDER OF THYROID\"}]},\"Criteria\":[]}],\"Type\":0}]},\"Select\":{\"Fields\":[]}}";
            //QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(json);
            var currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

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
        //This unit test tests thecombination of DRG codes, sex, Race Ethnicity
         // identifier # 10203
        public void ExecuteDRGENTDisorder()
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"841f1f27-fa24-4d4d-a62c-e5e0687f9d51\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"c757a763-7ac1-49ea-b197-e0ce0910dd1a\",\"Index\":1,\"Values\":{\"Type\":\"1\",\"IsCustomized\":false,\"IsExpanded\":false,\"HasBeenCustomized\":false,\"Codes\":[],\"CodeValues\":[{\"Code\":\"131\",\"Name\":\"Cranial/facial procedures w CC/MCC\"},{\"Code\":\"146\",\"Name\":\"Ear, nose, mouth & throat malignancy w MCC\"},{\"Code\":\"157\",\"Name\":\"Dental & Oral Diseases w MCC\"}]},\"Criteria\":[{\"ID\":\"68ff7378-71df-4d43-9694-d0a14863fc95\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"5cb12a7f-0217-47c6-aad9-672776845406\",\"Index\":2,\"Values\":{\"Type\":0},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"a56ac19d-7c5c-4f9b-be6f-422c0ff37942\",\"Index\":3,\"Values\":{\"Type\":0},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"71b4545c-345b-48b2-af5e-f84dc18e4e1a\",\"Index\":4,\"Values\":{\"Sex\":[1,3],\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"1d35e85d-24f8-4a86-a904-0909002d1e77\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"154752ab-10d1-4e6b-80b3-7fdadf3dca7f\",\"Index\":5,\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"cf3065bb-1d06-4519-b59f-5332c3d1e7ca\",\"Index\":6,\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"f173b6f8-cb6a-4e96-a162-174cd0b78514\",\"Index\":7,\"Values\":{\"Sex\":[]},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"702ce918-9a59-4082-a8c7-a9234536fe79\",\"Index\":8,\"Values\":{\"Races\":[1],\"Ethnicities\":[],\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"def8b9dd-a74f-4f88-bf0e-95e6e22097a1\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"f8aa2421-00ef-4ac4-9591-4d43a2a6c80d\",\"Index\":9,\"Values\":{\"Races\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"419df1e0-f0cb-43cb-a680-974f1611f3ef\",\"Index\":10,\"Values\":{\"Races\":[],\"Ethnicities\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"bd0e5691-473e-445c-9b45-6d7f1db51269\",\"Index\":11,\"Values\":{\"Races\":[],\"Ethnicities\":[]},\"Criteria\":[]}],\"Type\":0}]}],\"Type\":0}]},\"Select\":{\"Fields\":[]}}";
            //QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(json);
            var currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

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
        //This unit test tests the combination of Revenue code, age, sex AND ICd-10 diagnosis, ICD-9 Diagnosis AND NOT HCPCS
        //Question name:Revenuecode, ICD and hcpcs
        //identifier# 10204
        public void ExecuteRevenuecodeICDandHCPCS()
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"63ba8fbc-ea0b-44f3-92f3-91fb342fb0b8\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"c1db7229-4ff6-4bf4-b8ca-672d60d64c75\",\"Index\":1,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"3101\",\"Name\":\"Adult Care - Day Care, Medical, and Social - Hourly\"},{\"Code\":\"543\",\"Name\":\"Ambulance - Heart mobile\"},{\"Code\":\"843\",\"Name\":\"Continuous Ambulatory Peritoneal Dialysis Outpatient or Home - Home Equipment\"}]},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"d9dd6e82-bbca-466a-8022-b54ff3d99a3c\",\"Index\":2,\"Values\":{\"MinAge\":30,\"MaxAge\":null,\"BirthStartDate\":null,\"BirthEndDate\":null,\"Type\":0,\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"7bfc87ac-4f55-4b99-948e-3d182d85f38e\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"aa336d3c-ce0e-4f63-8ddc-f957da9109ec\",\"Index\":3,\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"9da5ddce-9bb7-4cd9-b706-15d92b09e8c2\",\"Index\":4,\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"8b48770f-37cd-49ed-88d2-b3d6390bc155\",\"Index\":5,\"Values\":{\"Type\":1},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"71b4545c-345b-48b2-af5e-f84dc18e4e1a\",\"Index\":6,\"Values\":{\"Sex\":[],\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"3dbcab32-dac5-4f27-b88c-dfeeaa150db1\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"154752ab-10d1-4e6b-80b3-7fdadf3dca7f\",\"Index\":7,\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"cf3065bb-1d06-4519-b59f-5332c3d1e7ca\",\"Index\":8,\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"f173b6f8-cb6a-4e96-a162-174cd0b78514\",\"Index\":9,\"Values\":{\"Sex\":[]},\"Criteria\":[]}],\"Type\":0}]}],\"Type\":0},{\"ID\":\"eb11e788-ec51-4fc4-8715-9c68fa0850a0\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"825fd547-525d-4e8d-8e9f-a45600cf8f9a\",\"Index\":12,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"A000\",\"Name\":\"Cholera due to Vibrio cholerae 01, biovar cholerae\"},{\"Code\":\"A031\",\"Name\":\"Shigellosis due to Shigella flexneri\"},{\"Code\":\"A039\",\"Name\":\"Shigellosis, unspecified\"},{\"Code\":\"A227\",\"Name\":\"Anthrax sepsis\"},{\"Code\":\"A229\",\"Name\":\"Anthrax, unspecified\"}]},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"5e5020dc-c0e4-487f-adf2-45431c2b7695\",\"Index\":13,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"240.9\",\"Name\":\"GOITER UNSPECIFIED\"},{\"Code\":\"250\",\"Name\":\"DIABETES MELLITUS\"},{\"Code\":\"250.10\",\"Name\":\"DB W/KA TYPE II/UNS NOT UNCNTRL\"},{\"Code\":\"320.0\",\"Name\":\"HEMOPHILUS MENINGITIS\"},{\"Code\":\"320.8\",\"Name\":\"MENINGITIS DUE OTHER SPEC BACTERIA - (320.8 )\"},{\"Code\":\"V01.0\",\"Name\":\"CONTACT WITH OR EXPOSURE TO CHOLERA\"},{\"Code\":\"V01.3\",\"Name\":\"CONTACT W/OR EXPOSURE TO SMALLPOX\"},{\"Code\":\"800.00\",\"Name\":\"CLOS FX VALT SKULL W/O ICI UNS SOC\"},{\"Code\":\"800.65\",\"Name\":\"OPN FX VLT SKL-CERB LAC LOC NO RTRN\"},{\"Code\":\"631\",\"Name\":\"OTHER ABNORMAL PRODUCT CONCEPTION\"},{\"Code\":\"630\",\"Name\":\"HYDATIDIFORM MOLE\"}]},\"Criteria\":[]}],\"Type\":0},{\"ID\":\"6798ae80-6c17-4ba6-80ff-50d0daad8c8d\",\"Name\":\"Paragraph\",\"Operator\":\"2\",\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"096a0001-73b4-405d-b45f-a3ca014c6e7d\",\"Index\":15,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"1000F\",\"Name\":\"TOBACCO USE ASSESSED\"},{\"Code\":\"1002F\",\"Name\":\"ANGINAL SYMPTOMS&LVL ACTV ASSESSED\"},{\"Code\":\"1004F\",\"Name\":\"CLINICAL SYMPTOMS VOL >LOAD ASSESSED\"},{\"Code\":\"14001\",\"Name\":\"ATT/REARRANGEMENT TRUNK 10.1-30.0CM\"},{\"Code\":\"14021\",\"Name\":\"ATT/REARRANGEMENT SCALP/ARM/LEG 10.1-30.0 CM\"},{\"Code\":\"14000\",\"Name\":\"ADJACENT TISSUE TRANSFER/REARGMT TRUNK 10 CM/<\"}]},\"Criteria\":[]}],\"Type\":0}]},\"Select\":{\"Fields\":[]}}";
            //QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(json);
            var currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

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
        //This unit test tests the combination of all Demographics concepts OR drug code AND Observation period
        //Question name:demographics,drugcode,observation
        //Identifier # 10205

        public void Executedemographicsdrugcodeobservationperiod ()
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"0be67a6b-0cd8-4b19-87d8-6dafa14ac770\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"d9dd6e82-bbca-466a-8022-b54ff3d99a3c\",\"Index\":1,\"Values\":{\"MinAge\":25,\"MaxAge\":35,\"BirthStartDate\":null,\"BirthEndDate\":null,\"Type\":0,\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"9e03d441-2d3b-4b0c-b297-ee086ef0e2c7\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"aa336d3c-ce0e-4f63-8ddc-f957da9109ec\",\"Index\":2,\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"9da5ddce-9bb7-4cd9-b706-15d92b09e8c2\",\"Index\":3,\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"8b48770f-37cd-49ed-88d2-b3d6390bc155\",\"Index\":4,\"Values\":{\"Type\":1},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"bd3c3913-3b1d-4ac3-92b2-976bc4580459\",\"Index\":5,\"Values\":{\"IsTrue\":true},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"93156e20-5124-4465-9a59-112782ad7f83\",\"Index\":6,\"Values\":{\"IsTrue\":true},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"8b5faa77-4a4b-4ac7-b817-69f1297e24c5\",\"Index\":7,\"Values\":{\"Type\":0,\"IsCustomized\":false,\"IsExpanded\":false,\"HasBeenCustomized\":false,\"Codes\":[],\"CodeValues\":[]},\"Criteria\":[{\"ID\":\"b09379cf-7a6b-413e-9128-de75deef2ba3\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"a616df59-c012-4bcf-8397-5cb2863ded75\",\"Index\":8,\"Values\":{\"Type\":0},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"5bbc2cf3-99b4-4ed6-b35d-a809bcd79582\",\"Index\":9,\"Values\":{\"Type\":0},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"32a45ae4-3c7d-43d1-b9d0-7e666bdb3632\",\"Index\":10,\"Values\":{\"Type\":0},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"702ce918-9a59-4082-a8c7-a9234536fe79\",\"Index\":11,\"Values\":{\"Races\":[1,4,2,3,5,6],\"Ethnicities\":[2],\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"87617e0c-d119-43e3-a63b-0c66280d1522\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"f8aa2421-00ef-4ac4-9591-4d43a2a6c80d\",\"Index\":12,\"Values\":{\"Races\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"419df1e0-f0cb-43cb-a680-974f1611f3ef\",\"Index\":13,\"Values\":{\"Races\":[],\"Ethnicities\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"bd0e5691-473e-445c-9b45-6d7f1db51269\",\"Index\":14,\"Values\":{\"Races\":[],\"Ethnicities\":[]},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"71b4545c-345b-48b2-af5e-f84dc18e4e1a\",\"Index\":15,\"Values\":{\"Sex\":[4,1],\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"0eb15f4a-747c-4cc7-9c0d-9d28263bda5a\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"154752ab-10d1-4e6b-80b3-7fdadf3dca7f\",\"Index\":16,\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"cf3065bb-1d06-4519-b59f-5332c3d1e7ca\",\"Index\":17,\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"f173b6f8-cb6a-4e96-a162-174cd0b78514\",\"Index\":18,\"Values\":{\"Sex\":[]},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"342c354b-9ecc-479b-be61-1770e4b44675\",\"Index\":19,\"Values\":{\"TobaccoUses\":[]},\"Criteria\":[]}],\"Type\":0},{\"ID\":\"cb00382d-290a-40e7-a939-1f4c427861ab\",\"Name\":\"Paragraph\",\"Operator\":\"1\",\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"2b298144-2ed5-473c-be78-a0ba7e83acf6\",\"Index\":26,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"0054-0140\",\"Name\":\"Acarbose\"},{\"Code\":\"0002-1200-10\",\"Name\":\"Amyvid - 1 VIAL, MULTI-DOSE in 1 CAN (0002-1200-10)  > 10 mL in 1 VIAL, MULTI-DOSE\"},{\"Code\":\"0002-3235-60\",\"Name\":\"Cymbalta - 60 CAPSULE, DELAYED RELEASE in 1 BOTTLE (0002-3235-60)\"},{\"Code\":\"0002-3235\",\"Name\":\"Cymbalta\"},{\"Code\":\"0002-4464-01\",\"Name\":\"Cialis - 1 BLISTER PACK in 1 CARTON (0002-4464-01)  > 3 TABLET, FILM COATED in 1 BLISTER PACK\"},{\"Code\":\"0002-4464-30\",\"Name\":\"Cialis - 30 TABLET, FILM COATED in 1 BOTTLE (0002-4464-30)\"},{\"Code\":\"0002-4464\",\"Name\":\"Cialis\"}]},\"Criteria\":[]}],\"Type\":0},{\"ID\":\"da4aa6f0-0488-4086-a7cb-1669beba37c5\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"98a78326-35d2-461a-b858-5b69e0fed28a\",\"Index\":27,\"Values\":{\"StartDate\":\"2003-01-01T05:00:00.000Z\",\"EndDate\":null},\"Criteria\":[]}],\"Type\":0}]},\"Select\":{\"Fields\":[]}}";
            //QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(json);
            var currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

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
        //This unit test tests the combination of all Diagnosis concepts AND NOT observation period OR Drugcode
        //Question name:diagnosisobservationperioddrugcode
        //identifier # 10206

        public void Executediagnosisobservationperioddrugcode ()
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"97a69607-51b4-4790-b409-a4b8ea39cc80\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"ec593176-d0bf-4e5a-bcff-4bbd13e88dbf\",\"Index\":1,\"Values\":{\"Condition\":\"20\"},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"c757a763-7ac1-49ea-b197-e0ce0910dd1a\",\"Index\":2,\"Values\":{\"Type\":0,\"IsCustomized\":false,\"IsExpanded\":false,\"HasBeenCustomized\":false,\"Codes\":[],\"CodeValues\":[{\"Code\":\"132\",\"Name\":\"Cranial/facial procedures w/o CC/MCC\"},{\"Code\":\"330\",\"Name\":\"Major small & large bowel procedures w CC\"},{\"Code\":\"331\",\"Name\":\"Major small & large bowel procedures w/o CC/MCC\"}]},\"Criteria\":[{\"ID\":\"0ca95ef3-abd1-467d-ae5a-dd2d2e701513\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"5cb12a7f-0217-47c6-aad9-672776845406\",\"Index\":3,\"Values\":{\"Type\":0},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"a56ac19d-7c5c-4f9b-be6f-422c0ff37942\",\"Index\":4,\"Values\":{\"Type\":0},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"825fd547-525d-4e8d-8e9f-a45600cf8f9a\",\"Index\":5,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"A030\",\"Name\":\"Shigellosis due to Shigella dysenteriae\"},{\"Code\":\"A4901\",\"Name\":\"Methicillin susceptible Staphylococcus aureus infection, unspecified site\"},{\"Code\":\"A888\",\"Name\":\"Other specified viral infections of central nervous system\"}]},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"5e5020dc-c0e4-487f-adf2-45431c2b7695\",\"Index\":6,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"240.9\",\"Name\":\"GOITER UNSPECIFIED\"},{\"Code\":\"242.0\",\"Name\":\"TOXIC DIFFUSE GOITER\"},{\"Code\":\"760.3\",\"Name\":\"FTUS/NB AFFCT OTH CHRN MAT RESP DZ\"},{\"Code\":\"760.6\",\"Name\":\"SURGICAL OPERATION MOTHER AND FETUS\"},{\"Code\":\"E800\",\"Name\":\"RW ACC INVLV COLL W/ROLLING STOCK\"},{\"Code\":\"E800.3\",\"Name\":\"RW COLL W/ROLL STOCK-PEDL CYCLST\"}]},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"ec593176-d0bf-4e5a-bcff-4bbd13e88dbf\",\"Index\":10,\"Values\":{\"Condition\":\"30\"},\"Criteria\":[]}],\"Type\":0},{\"ID\":\"164e76e2-2ea7-438c-972c-f3ede98f0440\",\"Name\":\"Paragraph\",\"Operator\":\"2\",\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"98a78326-35d2-461a-b858-5b69e0fed28a\",\"Index\":7,\"Values\":{\"StartDate\":null,\"EndDate\":\"2015-05-12T04:00:00.000Z\"},\"Criteria\":[]}],\"Type\":0},{\"ID\":\"7bf2e5bd-caa8-4955-bcb0-229bde5c7c8c\",\"Name\":\"Paragraph\",\"Operator\":\"1\",\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"2b298144-2ed5-473c-be78-a0ba7e83acf6\",\"Index\":8,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"0009-0233-01\",\"Name\":\"Bacitracin - 1 VIAL in 1 CARTON (0009-0233-01)  > 10 mL in 1 VIAL\"},{\"Code\":\"0093-7372-10\",\"Name\":\"Amlodipine and Benazepril Hydrochloride - 1000 CAPSULE in 1 BOTTLE (0093-7372-10)\"},{\"Code\":\"0781-2266-22\",\"Name\":\"Atomoxetine hydrochloride - 2000 CAPSULE in 1 BOTTLE (0781-2266-22)\"},{\"Code\":\"0781-2265-22\",\"Name\":\"Atomoxetine hydrochloride - 2000 CAPSULE in 1 BOTTLE (0781-2265-22)\"}]},\"Criteria\":[]}],\"Type\":0}]},\"Select\":{\"Fields\":[]}}";
            //QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(json);
            var currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

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
        //This unit test tests the combination of all Procedure concepts AND NOT Encounter Period OR NOT Observation period
        //Question name:procedureencounterperiodobservationperiod
        //identifier # 10207

        public void Executeprocedureencounterperiodobservationperiod ()
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"24971dbd-eb84-45f0-a621-983149452c69\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"096a0001-73b4-405d-b45f-a3ca014c6e7d\",\"Index\":1,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"1002F\",\"Name\":\"ANGINAL SYMPTOMS&LVL ACTV ASSESSED\"},{\"Code\":\"1004F\",\"Name\":\"CLINICAL SYMPTOMS VOL >LOAD ASSESSED\"},{\"Code\":\"13121\",\"Name\":\"REPAIR COMPLEX SCALP/ARM/LEG 2.6 CM-7.5 CM\"},{\"Code\":\"13102\",\"Name\":\"REPAIR COMPLEX TRUNK EA 5 CM/<\"}]},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"9902a45d-1e1c-4327-8d40-0000a44a004a\",\"Index\":2,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"2W00X0Z\",\"Name\":\"Placement Anatomical Regions Change Head External Traction Apparatus No Qualifier\"},{\"Code\":\"2W00X4Z\",\"Name\":\"Placement Anatomical Regions Change Head External Bandage No Qualifier\"}]},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"e1cc0001-1d9a-442a-94c4-a3ca014c7b94\",\"Index\":3,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"01.02\",\"Name\":\"VENTRICULOPUNCT THRU PREV IMPL CATH\"},{\"Code\":\"01.12\",\"Name\":\"OPEN BIOPSY OF CEREBRAL MENINGES\"},{\"Code\":\"72.2\",\"Name\":\"MID FORCEPS OPERATION\"},{\"Code\":\"72.39\",\"Name\":\"OTHER HIGH FORCEPS OPERATION\"},{\"Code\":\"72.5\",\"Name\":\"BREECH EXTRACTION\"},{\"Code\":\"40.0\",\"Name\":\"INCISION OF LYMPHATIC STRUCTURES\"},{\"Code\":\"40.19\",\"Name\":\"OTH DX PROC LYMPHATIC STRUCTURES\"},{\"Code\":\"40.23\",\"Name\":\"EXCISION OF AXILLARY LYMPH NODE\"}]},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"d6fbfdff-5b8a-49e7-a356-a293a01dceef\",\"Index\":4,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"56115-9\",\"Name\":\"Waist Circumference by NCFS\"},{\"Code\":\"8288-3\",\"Name\":\"Head Occipital-frontal circumference by US\"},{\"Code\":\"8293-3\",\"Name\":\"Chest Circumference --inspiration\"}]},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"71fba79a-d3dc-4248-90ce-ddbfeb521277\",\"Index\":5,\"Values\":{\"OtherCode\":\"40.19,72\"},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"c1db7229-4ff6-4bf4-b8ca-672d60d64c75\",\"Index\":6,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"634\",\"Name\":\"Drugs - Erythropoietin (EPO) Less than 10,000 Units\"},{\"Code\":\"635\",\"Name\":\"Drugs - Erythropoietin (EPO) Above 10,000 Units\"}]},\"Criteria\":[]}],\"Type\":0},{\"ID\":\"160185f5-1265-41bf-8e98-a5cf69b2732a\",\"Name\":\"Paragraph\",\"Operator\":\"2\",\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"db1a5f53-36f6-4b14-a2fc-b090b2b10a4c\",\"Index\":7,\"Values\":{\"AdmissionDate\":\"1984-01-12T05:00:00.000Z\",\"DischargeDate\":null},\"Criteria\":[]}],\"Type\":0},{\"ID\":\"d594a3ef-9573-40ae-b13d-bc32c81b1e47\",\"Name\":\"Paragraph\",\"Operator\":\"3\",\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"98a78326-35d2-461a-b858-5b69e0fed28a\",\"Index\":8,\"Values\":{\"StartDate\":null,\"EndDate\":\"2015-05-12T04:00:00.000Z\"},\"Criteria\":[]}],\"Type\":0}]},\"Select\":{\"Fields\":[]}}";
            //QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(json);
            var currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

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
        //This unit test tests the combination of customized encounter types AND NOT Encounter period
        //question name: customized encounter types
        //identifier # 10208 
        //customized encounter types values- PCORI - inpatient hospital stay
        //                             sentinel - emergency department, inpatient hospital stay,nonacute instituitional stay
        //AND NOT Encounter period - discharge date - 5/12/2015

        public void Executecustomizedencountertypes ()
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"81529245-4e7d-453a-9ffd-15b652b1586a\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"fd13b2f0-034f-4a0a-ae7c-4e772f220abf\",\"Index\":1,\"Values\":{\"Encounters\":[],\"IsCustomized\":true,\"HasBeenCustomized\":true,\"IsExpanded\":true},\"Criteria\":[{\"ID\":\"afe3c6ba-cf3a-4f17-ab86-8def28701eae\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"d7c58dfc-971b-47a7-8111-9b793ed4a6c1\",\"Index\":2,\"Values\":{\"Encounters\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"ef7d5bf3-404c-4034-a7c1-f32b08a24f0b\",\"Index\":3,\"Values\":{\"Encounters\":[4]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"a701ce17-b3b8-4646-b79b-2fcb80d36526\",\"Index\":4,\"Values\":{\"Encounters\":[3,5,2]},\"Criteria\":[]}],\"Type\":0}]}],\"Type\":0},{\"ID\":\"17b2490a-8d96-42b6-8239-1059e0b49507\",\"Name\":\"Paragraph\",\"Operator\":\"2\",\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"db1a5f53-36f6-4b14-a2fc-b090b2b10a4c\",\"Index\":5,\"Values\":{\"AdmissionDate\":null,\"DischargeDate\":\"2015-05-12T04:00:00.000Z\"},\"Criteria\":[]}],\"Type\":0}]},\"Select\":{\"Fields\":[]}}";
            //QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(json);
            var currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

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
        //This unit test tests the combination of customized sex AND NOT customized age,customized race ethnicity
        //Question nmae: customizedsexagerace
        //identifier# 10209

        public void Executecustomizedsexageraceethnicity ()
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"eae05c99-54bb-41ec-983d-ed34dc016162\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"71b4545c-345b-48b2-af5e-f84dc18e4e1a\",\"Index\":1,\"Values\":{\"Sex\":[],\"IsCustomized\":true,\"HasBeenCustomized\":true,\"IsExpanded\":true},\"Criteria\":[{\"ID\":\"f52d51d0-3fff-4608-82bd-054c369454a7\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"154752ab-10d1-4e6b-80b3-7fdadf3dca7f\",\"Index\":2,\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"cf3065bb-1d06-4519-b59f-5332c3d1e7ca\",\"Index\":3,\"Values\":{\"Sex\":[1]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"f173b6f8-cb6a-4e96-a162-174cd0b78514\",\"Index\":4,\"Values\":{\"Sex\":[4]},\"Criteria\":[]}],\"Type\":0}]}],\"Type\":0},{\"ID\":\"579767f4-ba84-496d-a51e-c78c3875b61e\",\"Name\":\"Paragraph\",\"Operator\":\"2\",\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"d9dd6e82-bbca-466a-8022-b54ff3d99a3c\",\"Index\":5,\"Values\":{\"MinAge\":null,\"MaxAge\":null,\"BirthStartDate\":null,\"BirthEndDate\":null,\"Type\":0,\"IsCustomized\":true,\"HasBeenCustomized\":true,\"IsExpanded\":true},\"Criteria\":[{\"ID\":\"29b8a122-13a3-4706-bfd0-9ff954985af3\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"aa336d3c-ce0e-4f63-8ddc-f957da9109ec\",\"Index\":6,\"Values\":{\"Type\":\"2\"},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"9da5ddce-9bb7-4cd9-b706-15d92b09e8c2\",\"Index\":7,\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"8b48770f-37cd-49ed-88d2-b3d6390bc155\",\"Index\":8,\"Values\":{\"Type\":\"2\"},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"702ce918-9a59-4082-a8c7-a9234536fe79\",\"Index\":9,\"Values\":{\"Races\":[],\"Ethnicities\":[],\"IsCustomized\":true,\"HasBeenCustomized\":true,\"IsExpanded\":true},\"Criteria\":[{\"ID\":\"8b51d47f-8add-42a3-86c6-86d1dbe3110d\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"f8aa2421-00ef-4ac4-9591-4d43a2a6c80d\",\"Index\":10,\"Values\":{\"Races\":[5,6]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"419df1e0-f0cb-43cb-a680-974f1611f3ef\",\"Index\":11,\"Values\":{\"Races\":[],\"Ethnicities\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"bd0e5691-473e-445c-9b45-6d7f1db51269\",\"Index\":12,\"Values\":{\"Races\":[],\"Ethnicities\":[]},\"Criteria\":[]}],\"Type\":0}]}],\"Type\":0}]},\"Select\":{\"Fields\":[]}}";
            //QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(json);
            var currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

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
        //This unit test tests the combination of customized sex,customized postalcode,customized age,customized race ethnicity
        //AND customized DRG AND NOT encountertypes
        //Question name: all customized
        //identifier # 10210
        public void Executeallcustomized ()
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"0cdae781-3293-44e4-8e30-8e83863001b3\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"d9dd6e82-bbca-466a-8022-b54ff3d99a3c\",\"Index\":1,\"Values\":{\"MinAge\":null,\"MaxAge\":null,\"BirthStartDate\":null,\"BirthEndDate\":null,\"Type\":0,\"IsCustomized\":true,\"HasBeenCustomized\":true,\"IsExpanded\":true},\"Criteria\":[{\"ID\":\"5e865a12-8126-411d-9b32-ee66ac63f1c1\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"aa336d3c-ce0e-4f63-8ddc-f957da9109ec\",\"Index\":2,\"Values\":{\"Type\":\"2\"},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"9da5ddce-9bb7-4cd9-b706-15d92b09e8c2\",\"Index\":3,\"Values\":{\"Type\":\"2\"},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"8b48770f-37cd-49ed-88d2-b3d6390bc155\",\"Index\":4,\"Values\":{\"Type\":\"2\"},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"8b5faa77-4a4b-4ac7-b817-69f1297e24c5\",\"Index\":5,\"Values\":{\"Type\":0,\"IsCustomized\":true,\"IsExpanded\":true,\"HasBeenCustomized\":true,\"Codes\":[],\"CodeValues\":[]},\"Criteria\":[{\"ID\":\"0eedb4bd-a2e2-430a-8f05-dc2dd4e8427c\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"a616df59-c012-4bcf-8397-5cb2863ded75\",\"Index\":6,\"Values\":{\"Type\":0},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"5bbc2cf3-99b4-4ed6-b35d-a809bcd79582\",\"Index\":7,\"Values\":{\"Type\":0},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"32a45ae4-3c7d-43d1-b9d0-7e666bdb3632\",\"Index\":8,\"Values\":{\"Type\":\"1\"},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"702ce918-9a59-4082-a8c7-a9234536fe79\",\"Index\":9,\"Values\":{\"Races\":[],\"Ethnicities\":[],\"IsCustomized\":true,\"HasBeenCustomized\":true,\"IsExpanded\":true},\"Criteria\":[{\"ID\":\"49f7fde8-39b3-4d40-9a1f-2424826ea67e\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"f8aa2421-00ef-4ac4-9591-4d43a2a6c80d\",\"Index\":10,\"Values\":{\"Races\":[2,6]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"419df1e0-f0cb-43cb-a680-974f1611f3ef\",\"Index\":11,\"Values\":{\"Races\":[],\"Ethnicities\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"bd0e5691-473e-445c-9b45-6d7f1db51269\",\"Index\":12,\"Values\":{\"Races\":[1,6],\"Ethnicities\":[2]},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"71b4545c-345b-48b2-af5e-f84dc18e4e1a\",\"Index\":13,\"Values\":{\"Sex\":[],\"IsCustomized\":true,\"HasBeenCustomized\":true,\"IsExpanded\":true},\"Criteria\":[{\"ID\":\"50035965-69eb-44f0-9f6e-24f282cf62cc\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"154752ab-10d1-4e6b-80b3-7fdadf3dca7f\",\"Index\":14,\"Values\":{\"Sex\":[1]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"cf3065bb-1d06-4519-b59f-5332c3d1e7ca\",\"Index\":15,\"Values\":{\"Sex\":[6]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"f173b6f8-cb6a-4e96-a162-174cd0b78514\",\"Index\":16,\"Values\":{\"Sex\":[4]},\"Criteria\":[]}],\"Type\":0}]}],\"Type\":0},{\"ID\":\"1c49e754-8042-42dd-8e9e-1c90a18327cf\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"c757a763-7ac1-49ea-b197-e0ce0910dd1a\",\"Index\":24,\"Values\":{\"Type\":0,\"IsCustomized\":false,\"IsExpanded\":false,\"HasBeenCustomized\":false,\"Codes\":[],\"CodeValues\":[]},\"Criteria\":[{\"ID\":\"1831dae9-7366-4867-9acb-c2b5e29bf303\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"5cb12a7f-0217-47c6-aad9-672776845406\",\"Index\":25,\"Values\":{\"Type\":0},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"a56ac19d-7c5c-4f9b-be6f-422c0ff37942\",\"Index\":26,\"Values\":{\"Type\":0},\"Criteria\":[]}],\"Type\":0}]}],\"Type\":0},{\"ID\":\"5759fff8-c1be-42f9-8fb4-f2d304534767\",\"Name\":\"Paragraph\",\"Operator\":\"2\",\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"fd13b2f0-034f-4a0a-ae7c-4e772f220abf\",\"Index\":31,\"Values\":{\"Encounters\":[6],\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"087d700e-a47e-4b54-b118-ccbe00365981\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"d7c58dfc-971b-47a7-8111-9b793ed4a6c1\",\"Index\":32,\"Values\":{\"Encounters\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"ef7d5bf3-404c-4034-a7c1-f32b08a24f0b\",\"Index\":33,\"Values\":{\"Encounters\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"a701ce17-b3b8-4646-b79b-2fcb80d36526\",\"Index\":34,\"Values\":{\"Encounters\":[]},\"Criteria\":[]}],\"Type\":0}]}],\"Type\":0}]},\"Select\":{\"Fields\":[]}}";
            //QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(json);
            var currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

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
        //This unit test tests the combination of concepts in timeline mode. 
        //Question name: Timeline1
        //identifier #10211
        //index event-sex,age AND ICd-9 diagnosis
        //1st event - ICD-10 diagnosis AND DRG
        //2nd event- null
        

        public void Executtimeline1 ()
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"6a29e0e4-9b1a-4277-b729-9cdf6ec0475a\",\"Name\":\"Index Event\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"a54b0cff-63f2-46d6-a6c8-41f884a888cd\",\"Index\":1,\"Values\":{\"Name\":\"Index Event\",\"StartYear\":0,\"StartMonth\":0,\"StartDay\":0,\"EndYear\":0,\"EndMonth\":0,\"EndDay\":0},\"Criteria\":[{\"ID\":\"22a209c5-fdfa-4526-bd58-47d6cf46dbc4\",\"Name\":\"'Index Event' Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"d9dd6e82-bbca-466a-8022-b54ff3d99a3c\",\"Index\":2,\"Values\":{\"MinAge\":null,\"MaxAge\":null,\"BirthStartDate\":null,\"BirthEndDate\":null,\"Type\":0,\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"a3dc1bb1-95c1-4c2c-a473-771668342a36\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"aa336d3c-ce0e-4f63-8ddc-f957da9109ec\",\"Index\":3,\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"9da5ddce-9bb7-4cd9-b706-15d92b09e8c2\",\"Index\":4,\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"8b48770f-37cd-49ed-88d2-b3d6390bc155\",\"Index\":5,\"Values\":{\"Type\":1},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"71b4545c-345b-48b2-af5e-f84dc18e4e1a\",\"Index\":6,\"Values\":{\"Sex\":[],\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"77ed7ffa-c87f-49ab-b9b6-7ab206857c91\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"154752ab-10d1-4e6b-80b3-7fdadf3dca7f\",\"Index\":7,\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"cf3065bb-1d06-4519-b59f-5332c3d1e7ca\",\"Index\":8,\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"f173b6f8-cb6a-4e96-a162-174cd0b78514\",\"Index\":9,\"Values\":{\"Sex\":[]},\"Criteria\":[]}],\"Type\":0}]}],\"Type\":0},{\"ID\":\"258b2554-c06f-4b80-a949-953990ecd794\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"5e5020dc-c0e4-487f-adf2-45431c2b7695\",\"Index\":10,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"560\",\"Name\":\"INTEST OBST W/O MENTION HERN\",\"CategoryID\":9},{\"Code\":\"560.1\",\"Name\":\"PARALYTIC ILEUS\",\"CategoryID\":9},{\"Code\":\"560.32\",\"Name\":\"FECAL IMPACTION\",\"CategoryID\":9},{\"Code\":\"560.31\",\"Name\":\"GALLSTONE ILEUS\",\"CategoryID\":9},{\"Code\":\"560.30\",\"Name\":\"UNSPECIFIED IMPACTION OF INTESTINE\",\"CategoryID\":9},{\"Code\":\"480\",\"Name\":\"VIRAL PNEUMONIA\",\"CategoryID\":8},{\"Code\":\"480.9\",\"Name\":\"UNSPECIFIED VIRAL PNEUMONIA\",\"CategoryID\":8},{\"Code\":\"482.81\",\"Name\":\"PNEUMONIA DUE TO ANAEROBES\",\"CategoryID\":8},{\"Code\":\"482.84\",\"Name\":\"LEGIONNAIRES+ DISEASE\",\"CategoryID\":8}]},\"Criteria\":[]}],\"Type\":0}]}],\"Type\":2},{\"ID\":\"8c41ebe5-0271-4384-b30b-ed1161645b8a\",\"RelatedToID\":\"6a29e0e4-9b1a-4277-b729-9cdf6ec0475a\",\"Name\":\"heart attack\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"a54b0cff-63f2-46d6-a6c8-41f884a888cd\",\"Index\":11,\"Values\":{\"Name\":\"heart attack\",\"StartYear\":1,\"StartMonth\":0,\"StartDay\":0,\"EndYear\":4,\"EndMonth\":0,\"EndDay\":0},\"Criteria\":[{\"ID\":\"11e59938-154a-4e28-b812-4f39986f5143\",\"Name\":\"'heart attack' Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"825fd547-525d-4e8d-8e9f-a45600cf8f9a\",\"Index\":12,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"A010\",\"Name\":\"Typhoid fever\",\"CategoryID\":2},{\"Code\":\"A0104\",\"Name\":\"Typhoid arthritis\",\"CategoryID\":2},{\"Code\":\"A013\",\"Name\":\"Paratyphoid fever C\",\"CategoryID\":2}]},\"Criteria\":[]}],\"Type\":0},{\"ID\":\"79428ac4-4eda-4b7e-921e-d5ad564a70b9\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"c757a763-7ac1-49ea-b197-e0ce0910dd1a\",\"Index\":13,\"Values\":{\"Type\":\"1\",\"IsCustomized\":false,\"IsExpanded\":false,\"HasBeenCustomized\":false,\"Codes\":[],\"CodeValues\":[{\"Code\":\"169\",\"Name\":\"Mouth procedures w/o CC/MCC\",\"CategoryID\":3},{\"Code\":\"51\",\"Name\":\"Description not available\",\"CategoryID\":3},{\"Code\":\"52\",\"Name\":\"Other ear, nose, mouth & throat O.R. procedures w CC/MCC\",\"CategoryID\":3}]},\"Criteria\":[{\"ID\":\"813ddd0a-0f20-4cc7-8a6f-c6a076dba8d7\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"5cb12a7f-0217-47c6-aad9-672776845406\",\"Index\":14,\"Values\":{\"Type\":0},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"a56ac19d-7c5c-4f9b-be6f-422c0ff37942\",\"Index\":15,\"Values\":{\"Type\":0},\"Criteria\":[]}],\"Type\":0}]}],\"Type\":0}]}],\"Type\":1},{\"ID\":\"b4395945-28c1-46b7-8286-0685783676b9\",\"RelatedToID\":\"8c41ebe5-0271-4384-b30b-ed1161645b8a\",\"Name\":\"recovery\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"a54b0cff-63f2-46d6-a6c8-41f884a888cd\",\"Index\":16,\"Values\":{\"Name\":\"recovery\",\"StartYear\":0,\"StartMonth\":0,\"StartDay\":0,\"EndYear\":2,\"EndMonth\":0,\"EndDay\":0},\"Criteria\":[{\"ID\":\"744eb29f-52e1-4686-b3ac-c06c10e04b89\",\"Name\":\"'recovery' Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[],\"Type\":0}]}],\"Type\":1}]},\"Select\":{\"Fields\":[]}}";
            //QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(json);
            var currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

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
        //This unit test tests the combination of concepts in timeline mode. 
        //Question name: Timeline2
        //identifier # 10212
        //index event-race ethnicity customized, age customized AND NOT Icd 10 Diagnosis
        //1st event - condition OR NOT DRG
        


        public void Executtimeline2 ()
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"59ae0e0b-e6c0-461b-9894-cf1ddaf0b1ec\",\"Name\":\"Index Event\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"a54b0cff-63f2-46d6-a6c8-41f884a888cd\",\"Index\":1,\"Values\":{\"Name\":\"Index Event\",\"StartYear\":0,\"StartMonth\":0,\"StartDay\":0,\"EndYear\":0,\"EndMonth\":0,\"EndDay\":0},\"Criteria\":[{\"ID\":\"3b88cef4-9409-42cd-b6f9-057665d021da\",\"Name\":\"'Index Event' Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"702ce918-9a59-4082-a8c7-a9234536fe79\",\"Index\":2,\"Values\":{\"Races\":[],\"Ethnicities\":[],\"IsCustomized\":true,\"HasBeenCustomized\":true,\"IsExpanded\":true},\"Criteria\":[{\"ID\":\"cc3bdf59-6af0-4d9e-a765-da6ed08a9040\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"f8aa2421-00ef-4ac4-9591-4d43a2a6c80d\",\"Index\":3,\"Values\":{\"Races\":[1,4]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"419df1e0-f0cb-43cb-a680-974f1611f3ef\",\"Index\":4,\"Values\":{\"Races\":[],\"Ethnicities\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"bd0e5691-473e-445c-9b45-6d7f1db51269\",\"Index\":5,\"Values\":{\"Races\":[6],\"Ethnicities\":[2]},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"d9dd6e82-bbca-466a-8022-b54ff3d99a3c\",\"Index\":6,\"Values\":{\"MinAge\":null,\"MaxAge\":null,\"BirthStartDate\":null,\"BirthEndDate\":null,\"Type\":0,\"IsCustomized\":true,\"HasBeenCustomized\":true,\"IsExpanded\":true},\"Criteria\":[{\"ID\":\"ed9f1551-9ddd-4b4f-b9e8-258994351866\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"aa336d3c-ce0e-4f63-8ddc-f957da9109ec\",\"Index\":7,\"Values\":{\"Type\":\"2\"},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"9da5ddce-9bb7-4cd9-b706-15d92b09e8c2\",\"Index\":8,\"Values\":{\"Type\":\"2\"},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"8b48770f-37cd-49ed-88d2-b3d6390bc155\",\"Index\":9,\"Values\":{\"Type\":1},\"Criteria\":[]}],\"Type\":0}]}],\"Type\":0},{\"ID\":\"f012c1ad-3c9d-4e29-b364-2e5701fe3a38\",\"Name\":\"Paragraph\",\"Operator\":\"2\",\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"825fd547-525d-4e8d-8e9f-a45600cf8f9a\",\"Index\":10,\"Values\":{\"Codes\":[],\"CodeValues\":[]},\"Criteria\":[]}],\"Type\":0}]}],\"Type\":2},{\"ID\":\"457465fc-c028-40b9-bf97-ef6cb0feb889\",\"RelatedToID\":\"59ae0e0b-e6c0-461b-9894-cf1ddaf0b1ec\",\"Name\":\"Next Event\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"a54b0cff-63f2-46d6-a6c8-41f884a888cd\",\"Index\":11,\"Values\":{\"Name\":\"Next Event\",\"StartYear\":0,\"StartMonth\":0,\"StartDay\":0,\"EndYear\":0,\"EndMonth\":0,\"EndDay\":0},\"Criteria\":[{\"ID\":\"fcb53ee4-4269-4b46-a126-283b8a83c584\",\"Name\":\"'Next Event' Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"ec593176-d0bf-4e5a-bcff-4bbd13e88dbf\",\"Index\":12,\"Values\":{\"Condition\":\"22\"},\"Criteria\":[]}],\"Type\":0},{\"ID\":\"da935120-3213-4952-9124-13839bdc64ae\",\"Name\":\"Paragraph\",\"Operator\":\"3\",\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"c757a763-7ac1-49ea-b197-e0ce0910dd1a\",\"Index\":13,\"Values\":{\"Type\":0,\"IsCustomized\":true,\"IsExpanded\":true,\"HasBeenCustomized\":true,\"Codes\":[],\"CodeValues\":[]},\"Criteria\":[{\"ID\":\"aad5af88-2032-4872-aeb0-deb8a62cfc8e\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"5cb12a7f-0217-47c6-aad9-672776845406\",\"Index\":14,\"Values\":{\"Type\":0},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"a56ac19d-7c5c-4f9b-be6f-422c0ff37942\",\"Index\":15,\"Values\":{\"Type\":0},\"Criteria\":[]}],\"Type\":0}]}],\"Type\":0}]}],\"Type\":1}]},\"Select\":{\"Fields\":[]}}";
            //QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(json);
            var currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

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
        //This unit test tests the combination of concepts in timeline mode. 
        //Question name: Timeline3
        //identifier # 10213
        //index event-null
        //1st event - icd 9 procedure, other procedure and revenue code



        public void Executtimeline3()
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"fc9373d2-5443-4801-976c-016391ad1a49\",\"Name\":\"Index Event\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"a54b0cff-63f2-46d6-a6c8-41f884a888cd\",\"Index\":1,\"Values\":{\"Name\":\"Index Event\",\"StartYear\":0,\"StartMonth\":0,\"StartDay\":0,\"EndYear\":0,\"EndMonth\":0,\"EndDay\":0},\"Criteria\":[{\"ID\":\"771700be-8e95-4ce2-add8-07dce8fd4b14\",\"Name\":\"'Index Event' Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[],\"Type\":0}]}],\"Type\":2},{\"ID\":\"425d25f7-84d8-4ac8-917e-cbe7fa1320b7\",\"RelatedToID\":\"fc9373d2-5443-4801-976c-016391ad1a49\",\"Name\":\"diabetes\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"a54b0cff-63f2-46d6-a6c8-41f884a888cd\",\"Index\":2,\"Values\":{\"Name\":\"diabetes\",\"StartYear\":5,\"StartMonth\":0,\"StartDay\":0,\"EndYear\":0,\"EndMonth\":0,\"EndDay\":0},\"Criteria\":[{\"ID\":\"c1f82d41-f95f-4742-ac94-67460b936fde\",\"Name\":\"'diabetes' Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"e1cc0001-1d9a-442a-94c4-a3ca014c7b94\",\"Index\":3,\"Values\":{\"Codes\":[],\"CodeValues\":[]},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"71fba79a-d3dc-4248-90ce-ddbfeb521277\",\"Index\":4,\"Values\":{\"OtherCode\":null},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"c1db7229-4ff6-4bf4-b8ca-672d60d64c75\",\"Index\":5,\"Values\":{\"Codes\":[],\"CodeValues\":[]},\"Criteria\":[]}],\"Type\":0}]}],\"Type\":1}]},\"Select\":{\"Fields\":[]}}";
            //QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(json);
            var currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

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
        //This unit test tests the usecase1 (sharepoint) . 
        //Question name: usecase1
        //identifier # 10214
        //Age 35 to 55, sex female, condition prediabetes and type1 diabetes
        //Paragraph AND NOT ICD 9 diagnosis - 415,416



        public void Executeusecase1 ()
        {
            string json = "{\"Header\":{},\"Where\":{\"Criteria\":[{\"ID\":\"4f8d3b31-ecd8-4ee2-9833-1b8e5482e0a5\",\"Name\":\"Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"d9dd6e82-bbca-466a-8022-b54ff3d99a3c\",\"Index\":1,\"Values\":{\"MinAge\":35,\"MaxAge\":55,\"BirthStartDate\":null,\"BirthEndDate\":null,\"Type\":0,\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"6fd49c56-5f5e-4459-80d5-3fefc45d5e00\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"aa336d3c-ce0e-4f63-8ddc-f957da9109ec\",\"Index\":2,\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"9da5ddce-9bb7-4cd9-b706-15d92b09e8c2\",\"Index\":3,\"Values\":{\"Type\":1},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"8b48770f-37cd-49ed-88d2-b3d6390bc155\",\"Index\":4,\"Values\":{\"Type\":1},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"71b4545c-345b-48b2-af5e-f84dc18e4e1a\",\"Index\":5,\"Values\":{\"Sex\":[1],\"IsCustomized\":false,\"HasBeenCustomized\":false,\"IsExpanded\":false},\"Criteria\":[{\"ID\":\"6c197d0c-21e9-4a59-a6a4-cc6adc005372\",\"Name\":\"Sub Paragraph\",\"Operator\":0,\"Criteria\":[],\"Terms\":[{\"Operator\":1,\"Type\":\"154752ab-10d1-4e6b-80b3-7fdadf3dca7f\",\"Index\":6,\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"cf3065bb-1d06-4519-b59f-5332c3d1e7ca\",\"Index\":7,\"Values\":{\"Sex\":[]},\"Criteria\":[]},{\"Operator\":1,\"Type\":\"f173b6f8-cb6a-4e96-a162-174cd0b78514\",\"Index\":8,\"Values\":{\"Sex\":[]},\"Criteria\":[]}],\"Type\":0}]},{\"Operator\":0,\"Type\":\"ec593176-d0bf-4e5a-bcff-4bbd13e88dbf\",\"Index\":9,\"Values\":{\"Condition\":\"23\"},\"Criteria\":[]},{\"Operator\":0,\"Type\":\"ec593176-d0bf-4e5a-bcff-4bbd13e88dbf\",\"Index\":10,\"Values\":{\"Condition\":\"20\"},\"Criteria\":[]}],\"Type\":0},{\"ID\":\"061d0f4f-03aa-4437-a6d8-6207547728bc\",\"Name\":\"Paragraph\",\"Operator\":\"2\",\"Criteria\":[],\"Terms\":[{\"Operator\":0,\"Type\":\"5e5020dc-c0e4-487f-adf2-45431c2b7695\",\"Index\":11,\"Values\":{\"Codes\":[],\"CodeValues\":[{\"Code\":\"415\",\"Name\":\"ACUTE PULMONARY HEART DISEASE\",\"CategoryID\":7},{\"Code\":\"416\",\"Name\":\"CHRONIC PULMONARY HEART DISEASE\",\"CategoryID\":7}]},\"Criteria\":[]}],\"Type\":0}]},\"Select\":{\"Fields\":[]}}";
            //QueryComposerRequestDTO dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerRequestDTO>(json);
            var currentResponse = ExecuteRequest(json, GetAdapterSettingsAUH());

            //The Response object basically is a collection of collection of results.
            List<Dictionary<string, object>> response = ((System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>[])(currentResponse.Results))[0];
            Assert.IsFalse(response.Count == 0, "The query executed successfully but no results were returned.");


            //Run against BAR Datamart
            /* currentResponse = ExecuteRequest(json, GetAdapterSettingsBAR());

             //The Response object basically is a collection of collection of results.
             List<Dictionary<string, object>> responseBAR = ((System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>[])(currentResponse.Results))[0];
             Assert.IsFalse(responseBAR.Count == 0, "The query executed successfully but no results were returned.");*/
        }



       
    }
}
