using Lpp.Dns.DataMart.Model.QueryComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace PopMedNet.Adapters.AcceptanceTests
{
    [TestClass]
    public class PCORNetAdaptable_Postgres96 : PCORNetAdaptable<PCORNetAdaptable_Postgres96>
    {
        protected override string ErrorOutputFolder => @".\Error Output\PCORNet Adaptable Postgres96";

        Dictionary<string, object> adapterSettings = null;

        //setup configuration settings and database specific tests
        public PCORNetAdaptable_Postgres96() : base("PCORNET_PostgreSQL96")
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
                    {"DatabaseSchema", "PCORNET_5_1"},
                    {"DataProvider", Lpp.Dns.DataMart.Model.Settings.SQLProvider.PostgreSQL.ToString()}
                };
            }

            return adapterSettings;
        }

        protected override DbConnection GetDbConnection()
        {
            var psConn = new NpgsqlConnection(ConnectionString);
            psConn.Open();
            return psConn;
        }
    }

    [TestClass]
    public class PCORNetAdaptable_Postgres95 : PCORNetAdaptable<PCORNetAdaptable_Postgres95>
    {
        protected override string ErrorOutputFolder => @".\Error Output\PCORNet Adaptable Postgres95";

        Dictionary<string, object> adapterSettings = null;

        //setup configuration settings and database specific tests
        public PCORNetAdaptable_Postgres95() : base("PCORNET_PostgreSQL95")
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
                    {"DatabaseSchema", "PCORNET_5_1"},
                    {"DataProvider", Lpp.Dns.DataMart.Model.Settings.SQLProvider.PostgreSQL.ToString()}
                };
            }

            return adapterSettings;
        }

        protected override DbConnection GetDbConnection()
        {
            var psConn = new NpgsqlConnection(ConnectionString);
            psConn.Open();
            return psConn;
        }
    }

    [TestClass]
    public class PCORNetAdaptable_Oracle12 : PCORNetAdaptable<PCORNetAdaptable_Oracle12>
    {
        protected override string ErrorOutputFolder => @".\Error Output\PCORNet Adaptable Oracle12";

        Dictionary<string, object> adapterSettings = null;

        //setup configuration settings and database specific tests
        public PCORNetAdaptable_Oracle12() : base("PCORNET_ORACLE12")
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

        protected override DbConnection GetDbConnection()
        {
            var oraCon = new OracleConnection(ConnectionString);
            oraCon.Open();
            return oraCon;
        }
    }

    [TestClass]
    public class PCORNetAdaptable_Oracle18 : PCORNetAdaptable<PCORNetAdaptable_Oracle18>
    {
        protected override string ErrorOutputFolder => @".\Error Output\PCORNet Adaptable Oracle18";

        Dictionary<string, object> adapterSettings = null;

        //setup configuration settings and database specific tests
        public PCORNetAdaptable_Oracle18() : base("PCORNET_ORACLE18")
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

        protected override DbConnection GetDbConnection()
        {
            var oraCon = new OracleConnection(ConnectionString);
            oraCon.Open();
            return oraCon;
        }
    }

    [TestClass]
    public class PCORNetAdaptable_SQLServer2014 : PCORNetAdaptable<PCORNetAdaptable_SQLServer2014>
    {
        protected override string ErrorOutputFolder => @".\Error Output\PCORNet Adaptable SQLServer2014";

        Dictionary<string, object> adapterSettings = null;

        //setup configuration settings and database specific tests
        public PCORNetAdaptable_SQLServer2014() : base("PCORNET_SQLServer2014")
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
                    {"DataProvider", Lpp.Dns.DataMart.Model.Settings.SQLProvider.SQLServer.ToString()}
                };
            }

            return adapterSettings;
        }

        protected override DbConnection GetDbConnection()
        {
            var mssmsConn = new System.Data.SqlClient.SqlConnection(ConnectionString);
            mssmsConn.Open();
            return mssmsConn;
        }
    }

    [TestClass]
    public class PCORNetAdaptable_SQLServer2016 : PCORNetAdaptable<PCORNetAdaptable_SQLServer2016>
    {
        protected override string ErrorOutputFolder => @".\Error Output\PCORNet Adaptable SQLServer2016";

        Dictionary<string, object> adapterSettings = null;

        //setup configuration settings and database specific tests
        public PCORNetAdaptable_SQLServer2016() : base("PCORNET_SQLServer2016")
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
                    {"DataProvider", Lpp.Dns.DataMart.Model.Settings.SQLProvider.SQLServer.ToString()}
                };
            }

            return adapterSettings;
        }

        protected override DbConnection GetDbConnection()
        {
            var mssmsConn = new System.Data.SqlClient.SqlConnection(ConnectionString);
            mssmsConn.Open();
            return mssmsConn;
        }
    }

    public abstract class PCORNetAdaptable<T> : BaseQueryTest<T>
    {
        //All tests are implemented in the base class

        readonly protected string ConnectionString;

        protected override string RootFolderPath => @".\Resources\PCORNet Adaptable";        

        public PCORNetAdaptable(string connectionStringKey) : base()
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

        [DataTestMethod, DataRow("ADPT_#47727")]
        public virtual void ADPT_47727(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_#47734")]
        public virtual void ADPT_47734(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_#47735")]
        public virtual void ADPT_47735(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_#47736")]
        public virtual void ADPT_47736(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_#47740")]
        public virtual void ADPT_47740(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_#47749")]
        public virtual void ADPT_47749(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_#47750")]
        public virtual void ADPT_47750(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_#47760")]
        public virtual void ADPT_47760(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_#47766")]
        public virtual void ADPT_47766(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_1_#1B")]
        public virtual void ADPT_UC_1_1B(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }        

        [DataTestMethod, DataRow("ADPT_UC_1_#2")]
        public virtual void ADPT_UC_1_2(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_1_#3")]
        public virtual void ADPT_UC_1_3(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_1_#4")]
        public virtual void ADPT_UC_1_4(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_1_#5")]
        public virtual void ADPT_UC_1_5(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_1_#6")]
        public virtual void ADPT_UC_1_6(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_1_#7A")]
        public virtual void ADPT_UC_1_7A(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_1_#7B")]
        public virtual void ADPT_UC_1_7B(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_1_#8")]
        public virtual void ADPT_UC_1_8(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_1_#9")]
        public virtual void ADPT_UC_1_9(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_2_#1")]
        public virtual void ADPT_UC_2_1(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_2_#2A")]
        public virtual void ADPT_UC_2_2A(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_2_#2B")]
        public virtual void ADPT_UC_2_2B(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_2_#2C")]
        public virtual void ADPT_UC_2_2C(string filename)
        {
            var result = RunRequest(filename); 
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_2_#3")]
        public virtual void ADPT_UC_2_3(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_2_#3B")]
        public virtual void ADPT_UC_2_3B(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_2_#4")]
        public virtual void ADPT_UC_2_4(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_2_#5")]
        public virtual void ADPT_UC_2_5(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_2_#6")]
        public virtual void ADPT_UC_2_6(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_2_#7")]
        public virtual void ADPT_UC_2_7(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }

        [DataTestMethod, DataRow("ADPT_UC_2_#7B")]
        public virtual void ADPT_UC_2_7B(string filename)
        {
            var result = RunRequest(filename);
            var expected = ConfirmResponse(filename + "_response", result);
        }
    }
}
