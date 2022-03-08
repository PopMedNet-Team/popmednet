using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace PopMedNet.Adapters.AcceptanceTests.PCORNetStandardTerms
{
    [TestClass]
    public class Oracle18 : PCORNetStandardTerms<Oracle18>
    {
        protected override string ErrorOutputFolder => @".\Error Output\PCORNet Standard Terms Oracle18";

        Dictionary<string, object> adapterSettings = null;

        //setup configuration settings and database specific tests
        public Oracle18() : base("PCORNET_ORACLE18")
        {
        }

        protected override Dictionary<string, object> ProvideAdapterSettings()
        {
            if (adapterSettings == null)
            {
                var connectionStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(ConnectionString);

                adapterSettings = new Dictionary<string, object>(){
                    {"Server", "" },
                    {"Port", "" },
                    {"Database", "" },
                    {"UserID", connectionStringBuilder.UserID },
                    {"Password", connectionStringBuilder.Password },
                    {"DataProvider",  Lpp.Dns.DataMart.Model.Settings.SQLProvider.Oracle.ToString()},
                    {"DatabaseSchema", connectionStringBuilder.UserID }
                };

                //(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={server address})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={service name})))
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\((?:[\w|\=|\.]+)\)");
                var matches = regex.Matches(connectionStringBuilder.DataSource);
                foreach (var m in matches)
                {
                    string capture = m.ToString();
                    string[] split = capture.Substring(1, capture.Length - 2).Split(new[] { '=' });
                    if (string.Equals("HOST", split[0], StringComparison.OrdinalIgnoreCase))
                    {
                        adapterSettings["Server"] = split[1];
                    }
                    else if (string.Equals("PORT", split[0], StringComparison.OrdinalIgnoreCase))
                    {
                        adapterSettings["Port"] = split[1];
                    }
                    else if (string.Equals("SERVICE_NAME", split[0], StringComparison.OrdinalIgnoreCase))
                    {
                        adapterSettings["Database"] = split[1];
                    }
                }
            }

            return adapterSettings;
        }

        [DataTestMethod, DataRow("Age_Term_#6")]
        public override void Age_Term_6(string filename)
        {
            var request = LoadRequest(filename);
            //set the submission date to now.
            request.Header.SubmittedOn = DateTime.UtcNow;
            request.SyncHeaders();
            var result = RunRequest(filename, request);

            var query = string.Format(@"
SELECT demo.HISPANIC as Hispanic, demo.RACE as Race, COUNT(*) AS Patients, 0 AS LowThreshold FROM (
SELECT d.HISPANIC, d.RACE, (CASE WHEN (d.BIRTH_DATE > TO_DATE('{0}', 'MM/DD/YYYY')) THEN
((extract (year from TO_DATE('{0}', 'MM/DD/YYYY')) - extract (year from  d.BIRTH_DATE)) + 
	(CASE WHEN 
	(
		(extract (month from  d.BIRTH_DATE) < extract (month from TO_DATE('{0}', 'MM/DD/YYYY'))) 
		OR 
		(
			(((extract (month from d.BIRTH_DATE)) = (extract (month from  TO_DATE('{0}', 'MM/DD/YYYY')))) OR ((extract (month from  d.BIRTH_DATE) IS NULL) AND (extract (month from TO_DATE('{0}', 'MM/DD/YYYY')) IS NULL)))
			AND 
			((extract (day from d.BIRTH_DATE)) < (extract (day from TO_DATE('{0}', 'MM/DD/YYYY')))) 
		) 		
	)
	THEN 1 ELSE 0 END
	)
)
ELSE
((extract (year from TO_DATE('{0}', 'MM/DD/YYYY')) - extract (year from  d.BIRTH_DATE)) - 
	(CASE WHEN 
	(
		((extract (month from d.BIRTH_DATE)) > (extract (month from TO_DATE('{0}', 'MM/DD/YYYY')))) 
		OR 
		(
			(((extract (month from  d.BIRTH_DATE)) = (extract (month from TO_DATE('{0}', 'MM/DD/YYYY')))) OR ((extract (month from  d.BIRTH_DATE) IS NULL) AND (extract (month from TO_DATE('{0}', 'MM/DD/YYYY')) IS NULL))) 
			AND 
			((extract (day from d.BIRTH_DATE)) > (extract (day from TO_DATE('{0}', 'MM/DD/YYYY'))))
		)
	) 
	THEN 1 ELSE 0 END
	)
)
END) as AGE 
FROM ""{3}"".""DEMOGRAPHIC"" d WHERE d.BIRTH_DATE IS NOT NULL
) demo
WHERE demo.AGE >= {1} AND demo.AGE <= {2}
GROUP BY demo.HISPANIC, demo.RACE
", DateTimeOffset.UtcNow.Date.ToString("MM/dd/yyyy"), 0, 15, adapterSettings["DatabaseSchema"]);

            string responseFileName = filename + "_response";
            var expectedResponse = LoadResponse(responseFileName);
            ManualQueryForExpectedResults(query, expectedResponse);
            ConfirmResponse(expectedResponse, result, System.IO.Path.Combine(ErrorOutputFolder, responseFileName + ".json"));
        }

        [DataTestMethod, DataRow("DX_Term_09"), Ignore]
        public override void DX_Term_09(string filename)
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

        protected override DbConnection GetDbConnection()
        {
            var oraCon = new OracleConnection(ConnectionString);
            oraCon.Open();
            return oraCon;
        }
    }
}
