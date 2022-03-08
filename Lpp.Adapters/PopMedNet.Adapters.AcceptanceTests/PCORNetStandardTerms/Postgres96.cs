using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace PopMedNet.Adapters.AcceptanceTests.PCORNetStandardTerms
{
    [TestClass]
    public class Postgres96 : PCORNetStandardTerms<Postgres96>
    {
        protected override string ErrorOutputFolder => @".\Error Output\PCORNet Standard Terms Postgres96";

        Dictionary<string, object> adapterSettings = null;

        //setup configuration settings and database specific tests
        public Postgres96() : base("PCORNET_PostgreSQL96")
        {
        }

        protected override Dictionary<string, object> ProvideAdapterSettings()
        {
            if (adapterSettings == null)
            {
                var connectionStringBuilder = new NpgsqlConnectionStringBuilder(ConnectionString);

                adapterSettings = new Dictionary<string, object>(){
                    {"Server", connectionStringBuilder.Host },
                    {"Port", connectionStringBuilder.Port },
                    {"UserID", connectionStringBuilder.Username },
                    {"Password", connectionStringBuilder.Password},
                    {"Database", connectionStringBuilder.Database },
                    {"DatabaseSchema", "pcornet_cdm"},
                    {"DataProvider", Lpp.Dns.DataMart.Model.Settings.SQLProvider.PostgreSQL.ToString()}
                };
            }

            return adapterSettings;
        }

        [DataTestMethod, DataRow("Age_Term_#6")]
        public override void Age_Term_6(string filename)
        {
            var request = LoadRequest(filename);
            request.Header.SubmittedOn = DateTime.UtcNow;
            request.SyncHeaders();

            var result = RunRequest(filename, request);

            string newQuery = string.Format(@"
   SELECT demo.""HISPANIC"" as Hispanic, demo.""RACE"" as Race, COUNT(*) AS Patients, 0 AS LowThreshold FROM (
SELECT d.""HISPANIC"", d.""RACE"", (CASE WHEN (d.""BIRTH_DATE"" > cast('{0}' as date)) THEN
(date_part('year',age(date_trunc('year', cast('{0}' as date)),date_trunc('year',d.""BIRTH_DATE"")))::int4 +
    (CASE WHEN 
	(
        (cast(extract(Month FROM d.""BIRTH_DATE"") as int4) < cast(extract(Month FROM  cast('{0}' as date)) as int4)
        OR 
        (
            (cast(extract(Month FROM d.""BIRTH_DATE"") as int4) = cast(extract(Month FROM  cast('{0}' as date)) as int4) OR cast(extract(Month FROM d.""BIRTH_DATE"") as int4) IS NULL AND cast(extract(Month FROM  cast('{0}' as date)) as int4) IS NULL) 
            AND
            cast(extract(Day FROM d.""BIRTH_DATE"") as int4) < cast(extract(Day FROM  cast('{0}' as date)) as int4)
        )) 	 
	)
	THEN (1) ELSE (0) END)
 )
ELSE
(
    (date_part('year',age(date_trunc('year', cast('{0}' as date)),date_trunc('year',d.""BIRTH_DATE"")))::int4 -
    (CASE WHEN 
	(
        (cast(extract(Month FROM d.""BIRTH_DATE"") as int4) > cast(extract(Month FROM  cast('{0}' as date)) as int4)
        OR
        (
            (cast(extract(Month FROM d.""BIRTH_DATE"") as int4) = cast(extract(Month FROM  cast('{0}' as date)) as int4) OR cast(extract(Month FROM d.""BIRTH_DATE"") as int4) IS NULL AND cast(extract(Month FROM  cast('{0}' as date)) as int4) IS NULL) 
            AND
            cast(extract(Day FROM d.""BIRTH_DATE"") as int4) > cast(extract(Day FROM  cast('{0}' as date)) as int4)
        )) 	 
	)
	THEN (1) ELSE (0) END)
 )
)
END) as AGE 
FROM ""{3}"".""DEMOGRAPHIC"" d WHERE d.""BIRTH_DATE"" IS NOT NULL
) as demo
WHERE demo.AGE >= {1} AND demo.AGE <= {2}
GROUP BY demo.""HISPANIC"", demo.""RACE"";
", DateTimeOffset.UtcNow, 0, 15, adapterSettings["DatabaseSchema"]);

            string responseFileName = filename + "_response";
            var expectedResponse = LoadResponse(responseFileName);
            ManualQueryForExpectedResults(newQuery, expectedResponse);
            ConfirmResponse(expectedResponse, result, System.IO.Path.Combine(ErrorOutputFolder, responseFileName + ".json"));
        }

        [DataTestMethod, DataRow("DX_Term_09"), Ignore]
        public override void DX_Term_09(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Race_Term_NoInfo"), Ignore]
        public override void Race_Term_NoInfo(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("DX_Term_NI"), Ignore]
        public virtual void DX_Term_NI(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        [DataTestMethod, DataRow("Race_Term_Unknown"), Ignore]
        public virtual void Race_Term_Unknown(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result, new[] { "LowThreshold" });
        }

        protected override DbConnection GetDbConnection()
        {
            var psConn = new NpgsqlConnection(ConnectionString);
            psConn.Open();
            return psConn;
        }
    }

}
