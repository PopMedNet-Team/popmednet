using Lpp.Dns.DataMart.Model.QueryComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Adapters.AcceptanceTests
{
    [TestClass]
    public class PCORNetQueries_SQLServer2014 : PCORNetQueries<PCORNetQueries_SQLServer2014>
    {
        protected override string ErrorOutputFolder => @".\Error Output\PCORNet Queries SQLServer2014";

        Dictionary<string, object> adapterSettings = null;

        //setup configuration settings and database specific tests
        public PCORNetQueries_SQLServer2014() : base("PCORNET_SQLServer2014")
        {
        }

        protected override Dictionary<string, object> ProvideAdapterSettings()
        {
            if (adapterSettings == null)
            {
                var connectionStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(ConnectionString);

                adapterSettings = new Dictionary<string, object>(){
                    {"Server", connectionStringBuilder.DataSource },
                    {"UserID", connectionStringBuilder.UserID },
                    {"Password", connectionStringBuilder.Password },
                    {"Database", connectionStringBuilder.InitialCatalog },
                    {"DataProvider", Lpp.Dns.DataMart.Model.Settings.SQLProvider.SQLServer.ToString()},
                    {"LowThresholdValue", 1 }
                };
            }

            return adapterSettings;
        }
    }

    [TestClass]
    public class PCORNetQueries_SQLServer2016 : PCORNetQueries<PCORNetQueries_SQLServer2016>
    {
        protected override string ErrorOutputFolder => @".\Error Output\PCORNet Queries SQLServer2016";

        Dictionary<string, object> adapterSettings = null;

        //setup configuration settings and database specific tests
        public PCORNetQueries_SQLServer2016() : base("PCORNET_SQLServer2016")
        {
        }

        protected override Dictionary<string, object> ProvideAdapterSettings()
        {
            if (adapterSettings == null)
            {
                var connectionStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(ConnectionString);

                adapterSettings = new Dictionary<string, object>(){
                    {"Server", connectionStringBuilder.DataSource },
                    {"UserID", connectionStringBuilder.UserID },
                    {"Password", connectionStringBuilder.Password },
                    {"Database", connectionStringBuilder.InitialCatalog },
                    {"DataProvider", Lpp.Dns.DataMart.Model.Settings.SQLProvider.SQLServer.ToString()},
                    {"LowThresholdValue", 1 }
                };
            }

            return adapterSettings;
        }
    }

    public abstract class PCORNetQueries<T> : BaseQueryTest<T>
    {
        //All tests are implemented in the base class

        readonly protected string ConnectionString;

        protected override string RootFolderPath => @".\Resources\PCORNet Queries";

        public PCORNetQueries(string connectionStringKey) : base()
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;
        }

        protected abstract Dictionary<string, object> ProvideAdapterSettings();

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

        [DataTestMethod, DataRow("request_25130")]
        public virtual void request_25130(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25131")]
        public virtual void request_25131(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25132")]
        public virtual void request_25132(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25133")]
        public virtual void request_25133(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25134")]
        public virtual void request_25134(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25135")]
        public virtual void request_25135(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25137")]
        public virtual void request_25137(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_25138")]
        public virtual void request_25138(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_25139")]
        public virtual void request_25139(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_25140")]
        public virtual void request_25140(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_25141")]
        public virtual void request_25141(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_25142")]
        public virtual void request_25142(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_25144")]
        public virtual void request_25144(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25145")]
        public virtual void request_25145(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_25146")]
        public virtual void request_25146(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25153")]
        public virtual void request_25153(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_25169")]
        public virtual void request_25169(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25172")]
        public virtual void request_25172(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25175")]
        public virtual void request_25175(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25190")]
        public virtual void request_25190(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25191")]
        public virtual void request_25191(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25192")]
        public virtual void request_25192(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25193")]
        public virtual void request_25193(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25194")]
        public virtual void request_25194(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25195")]
        public virtual void request_25195(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25196")]
        public virtual void request_25196(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25197")]
        public virtual void request_25197(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25227")]
        public virtual void request_25227(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25255")]
        public virtual void request_25255(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("request_25571")]
        public virtual void request_25571(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_25572")]
        public virtual void request_25572(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_25573")]
        public virtual void request_25573(string filename)
        {
            var result = RunRequest(filename);

            string query = @"DECLARE @date datetime = GETUTCDATE()
DECLARE @minAge int = 0
DECLARE @maxAge int = 15

SELECT demo.HISPANIC as Hispanic, demo.RACE as Race, COUNT(*) AS Patients FROM (
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

            string responseFileName = filename.Replace("request_", "response_");
            var expectedResponse = LoadResponse(responseFileName);
            ManualQueryForExpectedResults(query, expectedResponse);
            ConfirmResponse(expectedResponse, result, System.IO.Path.Combine(ErrorOutputFolder, responseFileName + ".json"));
        }

        [DataTestMethod, DataRow("request_25576")]
        public virtual void request_25576(string filename)
        {
            var result = RunRequest(filename);

            string query = @"DECLARE @date datetime = GETUTCDATE()
DECLARE @minAge int = 40
DECLARE @maxAge int = 41

SELECT demo.HISPANIC as Hispanic, demo.RACE as Race, COUNT(*) AS Patients FROM (
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

            string responseFileName = filename.Replace("request_", "response_");
            var expectedResponse = LoadResponse(responseFileName);
            ManualQueryForExpectedResults(query, expectedResponse);
            ConfirmResponse(expectedResponse, result, System.IO.Path.Combine(ErrorOutputFolder, responseFileName + ".json"));
        }

        [DataTestMethod, DataRow("request_25668")]
        [ExpectedException(typeof(ArgumentException), "Expecting an ArgumentException to be throw for a missing Observation Period term.")]
        public virtual void request_25668(string filename)
        {
            //This test should fail validation due to no observation period term being defined
            var result = RunRequest(filename);
        }

        [DataTestMethod, DataRow("request_25669")]
        public virtual void request_25669(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_25679")]
        public virtual void request_25679(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_25699")]
        public virtual void request_25699(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_25969")]
        public virtual void request_25969(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_26111")]
        public virtual void request_26111(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_26114")]
        public virtual void request_26114(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_26115")]
        [ExpectedException(typeof(ArgumentException), "Expecting an ArgumentException to be throw for a missing Observation Period term.")]
        public virtual void request_26115(string filename)
        {
            //This test should fail validation due to no observation period term being defined
            var result = RunRequest(filename);
        }

        [DataTestMethod, DataRow("request_26117")]
        public virtual void request_26117(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_26122")]
        public virtual void request_26122(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27166")]
        public virtual void request_27166(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27167")]
        public virtual void request_27167(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27168")]
        public virtual void request_27168(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27169")]
        public virtual void request_27169(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27170")]
        public virtual void request_27170(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27171")]
        public virtual void request_27171(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27172")]
        public virtual void request_27172(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27173")]
        public virtual void request_27173(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27174")]
        public virtual void request_27174(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27175")]
        public virtual void request_27175(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27176")]
        public virtual void request_27176(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27177")]
        public virtual void request_27177(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27178")]
        public virtual void request_27178(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27179")]
        public virtual void request_27179(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27180")]
        public virtual void request_27180(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27181")]
        public virtual void request_27181(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27182")]
        public virtual void request_27182(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27183")]
        public virtual void request_27183(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27184")]
        public virtual void request_27184(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27185")]
        public virtual void request_27185(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27186")]
        public virtual void request_27186(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27187")]
        public virtual void request_27187(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27188")]
        public virtual void request_27188(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27189")]
        public virtual void request_27189(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27190")]
        public virtual void request_27190(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27191")]
        public virtual void request_27191(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27192")]
        public virtual void request_27192(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27193")]
        public virtual void request_27193(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27194")]
        public virtual void request_27194(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27195")]
        public virtual void request_27195(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27196")]
        public virtual void request_27196(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27197")]
        public virtual void request_27197(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27198")]
        public virtual void request_27198(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27199")]
        public virtual void request_27199(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27200")]
        public virtual void request_27200(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27201")]
        public virtual void request_27201(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27202")]
        public virtual void request_27202(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27203")]
        public virtual void request_27203(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27204")]
        public virtual void request_27204(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27205")]
        public virtual void request_27205(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27206")]
        public virtual void request_27206(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27207")]
        public virtual void request_27207(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27208")]
        public virtual void request_27208(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27209")]
        public virtual void request_27209(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27210")]
        public virtual void request_27210(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        [DataTestMethod, DataRow("request_27211")]
        public virtual void request_27211(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename.Replace("request_", "response_"), result);
        }

        /// <summary>
        /// Executes a manual sql query, and populates a QueryComposerResponseDTO's results collection.
        /// The collection objects will be created based on the defined properties, and the column names of the sql response must
        /// match the defined property names.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="expectedResponse"></param>
        void ManualQueryForExpectedResults(string sql, Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO expectedResponse)
        {
            var properties = expectedResponse.Properties.Select(p => p.As).ToArray();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

            using (var conn = new System.Data.SqlClient.SqlConnection(ConnectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            foreach (string propertyName in properties)
                            {
                                int propertyOrdinal = reader.GetOrdinal(propertyName);
                                if (propertyOrdinal >= 0)
                                {
                                    row.Add(propertyName, reader.GetFieldValue<object>(propertyOrdinal));
                                }
                            }
                            if (row.Count > 0)
                            {
                                rows.Add(row);
                            }
                        }
                    }
                }

            }
            expectedResponse.ResponseDateTime = DateTime.UtcNow;
            expectedResponse.Results = new[] { rows };
        }

    }

}
