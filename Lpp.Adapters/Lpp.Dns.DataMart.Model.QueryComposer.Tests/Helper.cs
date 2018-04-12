using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Oracle.ManagedDataAccess.Client;
//using Oracle.ManagedDataAccess.Types;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Tests
{
    public static class Helper
    {

        #region PCORI

        /// <summary>
        /// Creates a QueryComposerModelProcessor for PCORI model.
        /// </summary>
        /// <param name="connectionString">The connection string of the pcori database.</param>
        /// <returns></returns>
        public static Lpp.Dns.DataMart.Model.QueryComposerModelProcessor CreateQueryComposerModelProcessorForPCORI(string connectionString)
        {
            var connectionStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);

            var processor = new Lpp.Dns.DataMart.Model.QueryComposerModelProcessor();
            processor.Settings = new Dictionary<string, object> {
                {"ModelID", new Guid("85EE982E-F017-4BC4-9ACD-EE6EE55D2446")},
                {"Server", connectionStringBuilder.DataSource },
                {"UserID", connectionStringBuilder.UserID },
                {"Password", connectionStringBuilder.Password },
                {"Database", connectionStringBuilder.InitialCatalog },
                {"DataProvider", Lpp.Dns.DataMart.Model.Settings.SQLProvider.SQLServer.ToString()}
            };

            return processor;
        }

        public static Lpp.Dns.DataMart.Model.PCORIQueryBuilder.DataContext CreatePCORIDataContext(string connectionString, bool logToConsole = true)
        {
            var db = CreatePCORIDataContext(Model.Settings.SQLProvider.SQLServer, connectionString, logToConsole);
            return db;
        }

        public static Lpp.Dns.DataMart.Model.PCORIQueryBuilder.DataContext CreatePCORIDataContext(Model.Settings.SQLProvider dbProvider, string connectionString, bool logToConsole = true)
        {
            string defaultSchema = "";
            System.Data.Common.DbConnection connection;
            if (dbProvider == Settings.SQLProvider.SQLServer)
            {
                connection = new System.Data.SqlClient.SqlConnection(connectionString);
            }
            else if (dbProvider == Settings.SQLProvider.ODBC)
            {
                connection = new System.Data.Odbc.OdbcConnection(connectionString);
            }
            else if (dbProvider == Settings.SQLProvider.PostgreSQL)
            {
                defaultSchema = "dbo";
                connection = new Npgsql.NpgsqlConnection(connectionString);
            }
            else if (dbProvider == Settings.SQLProvider.MySQL)
            {
                defaultSchema = "dbo";
                connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
            }
            else if (dbProvider == Settings.SQLProvider.Oracle)
            {
                defaultSchema = "C##PCORNETUSER";
                connection = new Oracle.ManagedDataAccess.Client.OracleConnection(connectionString);
            }
            else
            {
                throw new NotSupportedException("The specified SQLProvider of " + dbProvider + " is not supported.");
            }

            var db = new Lpp.Dns.DataMart.Model.PCORIQueryBuilder.DataContext(connection, defaultSchema);
            if (logToConsole)
            {
                db.Database.Log = (s) =>
                {
                    Console.WriteLine(s);
                };
            }

            return db;
        }

        public static QueryComposer.Adapters.PCORI.PCORIModelAdapter CreatePCORIModelAdapterAdapter(string connectionString)
        {
            return CreatePCORIModelAdapterAdapter(connectionString, Settings.SQLProvider.SQLServer);
        }

        public static QueryComposer.Adapters.PCORI.PCORIModelAdapter CreatePCORIModelAdapterAdapter(string connectionString, Lpp.Dns.DataMart.Model.Settings.SQLProvider sqlProvider, string schema = null)
        {
            Dictionary<string, object> adapterSettings;

            if (sqlProvider == Settings.SQLProvider.SQLServer)
            {
                var connectionStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
                adapterSettings = new Dictionary<string, object>(){
                    {"Server", connectionStringBuilder.DataSource },
                    {"UserID", connectionStringBuilder.UserID },
                    {"Password", connectionStringBuilder.Password },
                    {"Database", connectionStringBuilder.InitialCatalog },
                    {"DataProvider", sqlProvider.ToString()}
                };
            }
            else if (sqlProvider == Settings.SQLProvider.PostgreSQL)
            {
                var postgresConnectionStringBuilder = new Npgsql.NpgsqlConnectionStringBuilder(connectionString);
                
                adapterSettings = new Dictionary<string, object>(){
                    {"Server", postgresConnectionStringBuilder.Host },
                    {"Port", postgresConnectionStringBuilder.Port.ToString() },
                    {"UserID", postgresConnectionStringBuilder.Username },
                    //{"Password", System.Text.Encoding.UTF8.GetString(postgresConnectionStringBuilder.PasswordAsByteArray) },
                    {"Password", postgresConnectionStringBuilder.Password },
                    {"Database", postgresConnectionStringBuilder.Database },
                    {"ConnectionTimeout", postgresConnectionStringBuilder.Timeout.ToString() },
                    {"CommandTimeout", postgresConnectionStringBuilder.CommandTimeout.ToString()},
                    {"DatabaseSchema", schema},
                    {"DataProvider", sqlProvider.ToString()}
                };
            }
            else if (sqlProvider == Settings.SQLProvider.Oracle)
            {
                var oracleConnectionStringBuilder = new Oracle.ManagedDataAccess.Client.OracleConnectionStringBuilder(connectionString);
                adapterSettings = new Dictionary<string, object>(){
                    {"Server", "" },
                    {"Port", "" },
                    {"Database", "" },
                    {"UserID", oracleConnectionStringBuilder.UserID },
                    {"Password", oracleConnectionStringBuilder.Password },
                    {"DataProvider", sqlProvider.ToString()},
                    {"DatabaseSchema", schema }
                };

                //(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={server address})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={service name})))
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\((?:[\w|\=|\.]+)\)");
                var matches = regex.Matches(oracleConnectionStringBuilder.DataSource);
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
            else if (sqlProvider == Settings.SQLProvider.MySQL)
            {
                var mysqlConnectionStringBuilder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(connectionString);
                adapterSettings = new Dictionary<string, object>(){
                    {"Server", mysqlConnectionStringBuilder.Server },
                    {"Port", mysqlConnectionStringBuilder.Port.ToString() },
                    {"UserID", mysqlConnectionStringBuilder.UserID },
                    {"Password", mysqlConnectionStringBuilder.Password },
                    {"Database", mysqlConnectionStringBuilder.Database },
                    {"ConnectionTimeout", mysqlConnectionStringBuilder.ConnectionTimeout.ToString() },
                    {"CommandTimeout", mysqlConnectionStringBuilder.DefaultCommandTimeout.ToString()},
                    {"DataProvider", sqlProvider.ToString()}
                };
            }
            else
            {
                throw new NotImplementedException("Support for parsing configuration string into adapter settings not completed yet.");
            }

            var adapter = new QueryComposer.Adapters.PCORI.PCORIModelAdapter();
            adapter.Initialize(adapterSettings);
            return adapter;
        }
        #endregion PCORI

        #region ESP

        public static Lpp.Dns.DataMart.Model.ESPQueryBuilder.DataContext CreateESPDataContext(string connectionString, bool logToConsole = true)
        {
            var db = Lpp.Dns.DataMart.Model.ESPQueryBuilder.DataContext.Create(connectionString);

            if (logToConsole)
            {
                db.Database.Log = (s) =>
                {
                    Console.WriteLine(s);
                };
            }

            return db;
        }

        public static QueryComposer.Adapters.ESP.ESPModelAdapter CreateESPModelAdapterAdapter(string connectionString)
        {
            var connectionStringBuilder = new Npgsql.NpgsqlConnectionStringBuilder(connectionString);            
            var adapterSettings = new Dictionary<string, object>(){
                    {"Server", connectionStringBuilder.Host },
                    {"Port", connectionStringBuilder.Port},
                    //{"UserID", connectionStringBuilder.UserName },
                    //{"Password", System.Text.Encoding.UTF8.GetString(connectionStringBuilder.PasswordAsByteArray) },
                    {"UserID", connectionStringBuilder.Username },
                    {"Password", connectionStringBuilder.Password },
                    {"Database", connectionStringBuilder.Database },
                    {"ConnectionTimeout", connectionStringBuilder.Timeout },
                    {"CommandTimeout", connectionStringBuilder.CommandTimeout },
                    {"DataProvider", Lpp.Dns.DataMart.Model.Settings.SQLProvider.PostgreSQL.ToString()}
                };

            var adapter = new QueryComposer.Adapters.ESP.ESPModelAdapter();
            adapter.Initialize(adapterSettings);
            return adapter;
        }
        #endregion

        #region SummaryQuery
        public static QueryComposer.Adapters.SummaryQuery.IncidenceModelAdapter CreateINCSummaryModelAdapterAdapter(string connectionString)
        {
            var connectionStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
            var adapterSettings = new Dictionary<string, object>(){
                    {"Server", connectionStringBuilder.DataSource },
                    {"UserID", connectionStringBuilder.UserID },
                    {"Password", connectionStringBuilder.Password },
                    {"Database", connectionStringBuilder.InitialCatalog },
                    {"DataProvider", Lpp.Dns.DataMart.Model.Settings.SQLProvider.SQLServer.ToString()}
                };

            var adapter = new QueryComposer.Adapters.SummaryQuery.IncidenceModelAdapter();
            adapter.Initialize(adapterSettings);
            return adapter;
        }

        public static QueryComposer.Adapters.SummaryQuery.MostFrequentlyUsedQueriesModelAdapter CreateMFUSummaryModelAdapterAdapter(string connectionString)
        {
            var connectionStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
            var adapterSettings = new Dictionary<string, object>(){
                    {"Server", connectionStringBuilder.DataSource },
                    {"UserID", connectionStringBuilder.UserID },
                    {"Password", connectionStringBuilder.Password },
                    {"Database", connectionStringBuilder.InitialCatalog },
                    {"DataProvider", Lpp.Dns.DataMart.Model.Settings.SQLProvider.SQLServer.ToString()}
                };

            var adapter = new QueryComposer.Adapters.SummaryQuery.MostFrequentlyUsedQueriesModelAdapter();
            adapter.Initialize(adapterSettings);
            return adapter;
        }

        public static QueryComposer.Adapters.SummaryQuery.PrevalenceModelAdapter CreatePrevSummaryModelAdapterAdapter(string connectionString)
        {
            var connectionStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
            var adapterSettings = new Dictionary<string, object>(){
                    {"Server", connectionStringBuilder.DataSource },
                    {"UserID", connectionStringBuilder.UserID },
                    {"Password", connectionStringBuilder.Password },
                    {"Database", connectionStringBuilder.InitialCatalog },
                    {"DataProvider", Lpp.Dns.DataMart.Model.Settings.SQLProvider.SQLServer.ToString()}
                };

            var adapter = new QueryComposer.Adapters.SummaryQuery.PrevalenceModelAdapter();
            adapter.Initialize(adapterSettings);
            return adapter;
        }


        #endregion


        #region DataChecker

        public static QueryComposer.Adapters.DataChecker.DataCheckerModelAdapter CreateQueryComposerModelProcessorForDataChecker(string connectionString)
        {
            var connectionStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
            var adapterSettings = new Dictionary<string, object>(){
                    {"Server", connectionStringBuilder.DataSource },
                    {"UserID", connectionStringBuilder.UserID },
                    {"Password", connectionStringBuilder.Password },
                    {"Database", connectionStringBuilder.InitialCatalog },
                    {"DataProvider", Lpp.Dns.DataMart.Model.Settings.SQLProvider.SQLServer.ToString()}
                };

            var adapter = new QueryComposer.Adapters.DataChecker.DataCheckerModelAdapter();
            adapter.Initialize(adapterSettings);
            return adapter;
        }

        public static Lpp.Dns.DataMart.Model.QueryComposer.Adapters.DataChecker.DataContext CreateDataCheckerDataContext(string connectionString, bool logToConsole = true)
        {
            var db = CreateDataCheckerDataContext(Model.Settings.SQLProvider.SQLServer, connectionString, logToConsole);
            return db;
        }

        public static Lpp.Dns.DataMart.Model.QueryComposer.Adapters.DataChecker.DataContext CreateDataCheckerDataContext(Model.Settings.SQLProvider dbProvider, string connectionString, bool logToConsole = true)
        {
            string defaultSchema = "";
            System.Data.Common.DbConnection connection;
            if (dbProvider == Settings.SQLProvider.SQLServer)
            {
                connection = new System.Data.SqlClient.SqlConnection(connectionString);
            }
            else if (dbProvider == Settings.SQLProvider.ODBC)
            {
                connection = new System.Data.Odbc.OdbcConnection(connectionString);
            }
            else if (dbProvider == Settings.SQLProvider.PostgreSQL)
            {
                defaultSchema = "dbo";
                connection = new Npgsql.NpgsqlConnection(connectionString);
            }
            else if (dbProvider == Settings.SQLProvider.MySQL)
            {
                defaultSchema = "dbo";
                connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
            }
            else if (dbProvider == Settings.SQLProvider.Oracle)
            {
                defaultSchema = "C##PCORNETUSER";
                connection = new Oracle.ManagedDataAccess.Client.OracleConnection(connectionString);
            }
            else
            {
                throw new NotSupportedException("The specified SQLProvider of " + dbProvider + " is not supported.");
            }

            var db = new Lpp.Dns.DataMart.Model.QueryComposer.Adapters.DataChecker.DataContext(connection, defaultSchema);
            if (logToConsole)
            {
                db.Database.Log = (s) =>
                {
                    Console.WriteLine(s);
                };
            }

            return db;
        }

        #endregion


        public static void DumpQueryComposerResults(IEnumerable<IEnumerable<Dictionary<string, object>>> results)
        {
            foreach (var r1 in results)
            {
                foreach (var r2 in r1)
                {
                    Console.WriteLine("Results:");
                    foreach (var pair in r2)
                    {
                        Console.WriteLine("{0}  {1}", pair.Key, pair.Value);
                    }
                }
            }
        }
    }
}
