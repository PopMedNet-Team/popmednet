using Lpp.Dns.DataMart.Model.QueryComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace PopMedNet.Adapters.AcceptanceTests.PCORNetStandardTerms
{
    public abstract class PCORNetStandardTerms<T> : BaseQueryTest<T>
    {
        //All tests are implemented in the base class

        readonly protected string ConnectionString;

        protected override string RootFolderPath => @".\Resources\PCORNet Standard Terms";

        public PCORNetStandardTerms(string connectionStringKey) : base()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;
        }

        protected abstract Dictionary<string,object> ProvideAdapterSettings();

        protected override IModelAdapter CreateModelAdapter(string testname)
        {
            var adapter = new Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.PCORIModelAdapter(new Lpp.Dns.DataMart.Model.RequestMetadata
            {
                CreatedOn = DateTime.UtcNow,
                MSRequestID = testname
            });

            adapter.Initialize(ProvideAdapterSettings(), Guid.NewGuid().ToString("D"));

            return adapter;
        }

        [DataTestMethod, DataRow("Age_Stratification_#1")]
        public virtual void Age_Stratification_1(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Age_Stratification_#2")]
        public virtual void Age_Stratification_2(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("Age_Stratification_#3")]
        public virtual void Age_Stratification_3(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("Age_Stratification_#4")]
        public virtual void Age_Stratification_4(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("Age_Term_#3")]
        public virtual void Age_Term_3(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("Age_Term_#4")]
        public virtual void Age_Term_4(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("Age_Term_#5")]
        public virtual void Age_Term_5(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("Age_Term_#6")]
        public virtual void Age_Term_6(string filename)
        {
            var request = LoadRequest(filename);

            //set the submission date to now.
            request.Header.SubmittedOn = DateTime.UtcNow;
            request.SyncHeaders();

            var result = RunRequest(filename, request);

            string query = @"DECLARE @date datetime = GETUTCDATE()
DECLARE @minAge int = 0
DECLARE @maxAge int = 15

SELECT demo.HISPANIC as Hispanic, demo.RACE as Race, COUNT(*) AS Patients, 0 AS LowThreshold FROM (
SELECT d.HISPANIC, d.RACE, (CASE WHEN (d.BIRTH_DATE > @date) THEN
/*Birthdate is before the calculation date, get the difference of the years and add a year depending on if they have had a birthday yet or not*/
(DATEDIFF(year, d.BIRTH_DATE, @date) + 
	(CASE WHEN 
	(
		(DATEPART(MM, d.BIRTH_DATE) < DATEPART(MM, @date)) 
		OR 
		(
			(((DATEPART (month, d.BIRTH_DATE)) = (DATEPART (month, @date))) OR ((DATEPART (month, d.BIRTH_DATE) IS NULL) AND (DATEPART (month, @date) IS NULL)))
			AND 
			((DATEPART (day, d.BIRTH_DATE)) < (DATEPART (day, @date))) 
		) 		
	)
	THEN 1 ELSE 0 END
	)
)
ELSE
(DATEDIFF(year, d.BIRTH_DATE, @date) - 
	(CASE WHEN 
	(
		((DATEPART (month, d.BIRTH_DATE)) > (DATEPART (month, @date))) 
		OR 
		(
			(((DATEPART (month, d.BIRTH_DATE)) = (DATEPART (month, @date))) OR ((DATEPART (month, d.BIRTH_DATE) IS NULL) AND (DATEPART (month, @date) IS NULL))) 
			AND 
			((DATEPART (day, d.BIRTH_DATE)) > (DATEPART (day, @date)))
		)
	) 
	THEN 1 ELSE 0 END
	)
)
END) as AGE 
FROM DEMOGRAPHIC d WHERE d.BIRTH_DATE IS NOT NULL
) demo
WHERE demo.AGE >= @minAge AND demo.AGE <= @maxAge
GROUP BY demo.HISPANIC, demo.RACE";

            string responseFileName = filename + "_response";
            var expectedResponse = LoadResponse(responseFileName);
            ManualQueryForExpectedResults(query, expectedResponse);
            ConfirmResponse(expectedResponse, result, System.IO.Path.Combine(ErrorOutputFolder, responseFileName + ".json"));
        }

        [DataTestMethod, DataRow("Age_Term_#7")]
        public virtual void Age_Term_7(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("DX_Term_09")]
        public virtual void DX_Term_09(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("DX_Term_10")]
        public virtual void DX_Term_10(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("DX_Term_11")]
        public virtual void DX_Term_11(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("DX_Term_NI")]
        public virtual void DX_Term_NI(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("DX_Term_OT")]
        public virtual void DX_Term_OT(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("DX_Term_SM")]
        public virtual void DX_Term_SM(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("DX_Term_Startswith")]
        public virtual void DX_Term_Startswith(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("DX_Term_UN")]
        public virtual void DX_Term_UN(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Hispanic_Stratification_#1")]
        public virtual void Hispanic_Stratification_1(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("Hispanic_Term_N")]
        public virtual void Hispanic_Term_N(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Hispanic_Term_NoInfo")]
        public virtual void Hispanic_Term_NoInfo(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Hispanic_Term_Other")]
        public virtual void Hispanic_Term_Other(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("Hispanic_Term_Refuse")]
        public virtual void Hispanic_Term_Refuse(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Hispanic_Term_Unknown")]
        public virtual void Hispanic_Term_Unknown(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Hispanic_Term_Y")]
        public virtual void Hispanic_Term_Y(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        /// <summary>
        /// All patients that have an encounter of any kind should return in the results.Only patients with an encounter should return.
        /// </summary>
        /// <example>Request: https://pmnuat.popmednet.org/requests/details?ID=0a0e7653-31a8-4e7c-9e75-a83901182d4e</example>
        /// <param name="filename"></param>
        [DataTestMethod, DataRow("ObsPeriod_Term_#1")]
        public virtual void ObsPeriod_Term_1(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_All_Term_1")]
        public virtual void Prescribing_All_Term_1(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_All_Term_2")]
        public virtual void Prescribing_All_Term_2(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_All_Term_3")]
        public virtual void Prescribing_All_Term_3(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }


        [DataTestMethod, DataRow("Prescribing_All_Term_4")]
        public virtual void Prescribing_All_Term_4(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_All_Term_5")]
        public virtual void Prescribing_All_Term_5(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_All_Term_6")]
        public virtual void Prescribing_All_Term_6(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_All_Term_7")]
        public virtual void Prescribing_All_Term_7(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_All_Term_8")]
        public virtual void Prescribing_All_Term_8(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_All_Term_9")]
        public virtual void Prescribing_All_Term_9(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_All_Term_10")]
        public virtual void Prescribing_All_Term_10(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_All_Term_11")]
        public virtual void Prescribing_All_Term_11(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_All_Term_14")]
        public virtual void Prescribing_All_Term_14(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_All_Term_15")]
        public virtual void Prescribing_All_Term_15(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_All_Term_16")]
        public virtual void Prescribing_All_Term_16(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_All_Term_17")]
        public virtual void Prescribing_All_Term_17(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_All_Term_18")]
        public virtual void Prescribing_All_Term_18(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_All_Term_19")]
        public virtual void Prescribing_All_Term_19(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_All_Term_20")]
        public virtual void Prescribing_All_Term_20(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_All_Term_21")]
        public virtual void Prescribing_All_Term_21(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_All_Term_22")]
        public virtual void Prescribing_All_Term_22(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }
        
        [DataTestMethod, DataRow("Prescribing_All_Term_23")]
        public virtual void Prescribing_All_Term_23(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }


        [DataTestMethod, DataRow("Prescribing_Complex_1")]
        public virtual void Prescribing_Complex_1(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_Complex_2")]
        public virtual void Prescribing_Complex_2(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_Complex_3")]
        public virtual void Prescribing_Complex_3(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_Complex_4")]
        public virtual void Prescribing_Complex_4(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_Complex_5")]
        public virtual void Prescribing_Complex_5(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_Complex_6")]
        public virtual void Prescribing_Complex_6(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_Complex_7")]
        public virtual void Prescribing_Complex_7(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_Complex_8")]
        public virtual void Prescribing_Complex_8(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_Complex_9")]
        public virtual void Prescribing_Complex_9(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }
        
        [DataTestMethod, DataRow("Prescribing_requirements_1")]
        public virtual void Prescribing_Requirements_1(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_requirements_2")]
        public virtual void Prescribing_Requirements_2(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_requirements_3")]
        public virtual void Prescribing_Requirements_3(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_requirements_4")]
        public virtual void Prescribing_Requirements_4(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_requirements_5")]
        public virtual void Prescribing_Requirements_5(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_requirements_6")]
        public virtual void Prescribing_Requirements_6(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_requirements_7")]
        public virtual void Prescribing_Requirements_7(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_requirements_8")]
        public virtual void Prescribing_Requirements_8(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Prescribing_requirements_9")]
        public virtual void Prescribing_Requirements_9(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }
        
        /// <summary>
        /// This request should return a result for every unique instance in the Race field.  Patients should have a value for BIRTH_DATE.
        /// </summary>
        /// <example>Request: https://pmnuat.popmednet.org/requests/details?ID=c369a232-e82a-4925-8fee-a839011e829a</example>
        /// <param name="filename"></param>
        [DataTestMethod, DataRow("Race_Stratification_#1"), Description("Should return a result for every unique instance in the Race field.  Patients should have a value for BIRTH_DATE.")]
        public virtual void Race_Stratification_1(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Race_Term_AIAN")]
        public virtual void Race_Term_AIAN(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Race_Term_Asian")]
        public virtual void Race_Term_Asian(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Race_Term_BlackAA")]
        public virtual void Race_Term_BlackAA(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Race_Term_Multiple")]
        public virtual void Race_Term_Multiple(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Race_Term_NHOPI")]
        public virtual void Race_Term_NHOPI(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Race_Term_NoInfo")]
        public virtual void Race_Term_NoInfo(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Race_Term_Other")]
        public virtual void Race_Term_Other(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Race_Term_Refuse")]
        public virtual void Race_Term_Refuse(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Race_Term_Unknown")]
        public virtual void Race_Term_Unknown(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Race_Term_White")]
        public virtual void Race_Term_White(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Setting_Term_AV")]
        public virtual void Setting_Term_AV(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Setting_Term_ED")]
        public virtual void Setting_Term_ED(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Setting_Term_EI")]
        public virtual void Setting_Term_EI(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Setting_Term_IC")]
        public virtual void Setting_Term_IC(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Setting_Term_IP")]
        public virtual void Setting_Term_IP(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Setting_Term_IS")]
        public virtual void Setting_Term_IS(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Setting_Term_NI")]
        public virtual void Setting_Term_NI(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Setting_Term_OA")]
        public virtual void Setting_Term_OA(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Setting_Term_OS")]
        public virtual void Setting_Term_OS(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Setting_Term_OT")]
        public virtual void Setting_Term_OT(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Setting_Term_UN")]
        public virtual void Setting_Term_UN(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Sex_Stratification_#1")]
        public virtual void Sex_Stratification_1(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Sex_Term_Ambig")]
        public virtual void Sex_Term_Ambig(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Sex_Term_Female")]
        public virtual void Sex_Term_Female(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Sex_Term_Male")]
        public virtual void Sex_Term_Male(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Sex_Term_NoInfo")]
        public virtual void Sex_Term_NoInfo(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Sex_Term_Other")]
        public virtual void Sex_Term_Other(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Sex_Term_Unknown")]
        public virtual void Sex_Term_Unknown(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }
    }
}
