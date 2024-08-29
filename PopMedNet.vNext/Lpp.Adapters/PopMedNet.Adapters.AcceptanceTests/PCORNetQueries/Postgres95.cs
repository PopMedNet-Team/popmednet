using Lpp.Dns.DataMart.Model.QueryComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace PopMedNet.Adapters.AcceptanceTests.PCORNetQueries
{
    [TestClass]
    public class Postgres95 : PCORNetQueries<Postgres95>
    {
        protected override string ErrorOutputFolder => @".\Error Output\PCORNet Queries Postgres95";

        Dictionary<string, object> adapterSettings = null;

        //setup configuration settings and database specific tests
        public Postgres95() : base("PCORNET_PostgreSQL95")
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

        [DataTestMethod, DataRow("request_25573")]
        public override void request_25573(string filename)
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

            string responseFileName = filename.Replace("request_", "response_");
            var expectedResponse = LoadResponse(responseFileName);
            ManualQueryForExpectedResults(newQuery, expectedResponse);
            ConfirmResponse(expectedResponse, result, System.IO.Path.Combine(ErrorOutputFolder, responseFileName + ".json"));
        }

        [DataTestMethod, DataRow("request_25576")]
        public override void request_25576(string filename)
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
", DateTimeOffset.UtcNow, 40, 41, adapterSettings["DatabaseSchema"]);

            string responseFileName = filename.Replace("request_", "response_");
            var expectedResponse = LoadResponse(responseFileName);
            ManualQueryForExpectedResults(newQuery, expectedResponse);
            ConfirmResponse(expectedResponse, result, System.IO.Path.Combine(ErrorOutputFolder, responseFileName + ".json"));
        }

        protected override DbConnection GetDbConnection()
        {
            var psConn = new NpgsqlConnection(ConnectionString);
            psConn.Open();
            return psConn;
        }
    }
}
