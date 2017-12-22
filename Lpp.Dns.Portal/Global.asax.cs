using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Routing;
using LinqKit;
using log4net;
using log4net.Config;
using Lpp.Audit;
using Lpp.Composition;
using Lpp.Security;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Web.Mvc;
using System.Data.Entity;
using Lpp.Dns.Data;
using Lpp.Utilities.WebSites;

namespace Lpp.Dns.Portal.Root
{
    public class Global : Lpp.Mvc.LppMvcComposableApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            base.BaseApplication_Start();
            DbConfiguration.SetConfiguration(new DatabaseConfiguration());
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/Web.config")));

            AreaRegistration.RegisterAllAreas();

#if !DEBUG 
            Aspose.Cells.License lic = new Aspose.Cells.License();
            lic.SetLicense(Server.MapPath("Aspose.Cells.lic"));
#endif

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Registers the notifier that will send emails etc on notifications.
            DataContext.RegisteryNotifier(new Notifier());
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            DataContext datacontext = HttpContext.Current.Items["DataContext"] as DataContext;
            if (datacontext == null)
            {
                HttpContext.Current.Items["DataContext"] = new DataContext();
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            DataContext datacontext = HttpContext.Current.Items["DataContext"] as DataContext;
            if (datacontext != null)
            {
                HttpContext.Current.Items["DataContext"] = null;
                datacontext.Dispose();
                datacontext = null;
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }


        class LoggerExport
        {
            static readonly ILog _logger = log4net.LogManager.GetLogger("DNS Web Portal");
            [Export]
            public ILog Logger { get { return _logger; } }
        }

        //class DnsDomainPersistenceConfig
        //{
        //    [Export]
        //    public PersistenceConfig<DnsDomain> Config
        //    {
        //        get
        //        {
        //            var cs = ConfigurationManager.ConnectionStrings[typeof(DnsDomain).FullName];
        //            return new PersistenceConfig<DnsDomain>
        //            {
        //                ConnectionString = cs == null ? null : cs.ConnectionString,
        //                CommandTimeoutSeconds = 300,

        //                // The following line is here for debugging purposes.
        //                // If uncommented, it will dump all SQL queries to the log file.
        //                //CreateConnection = TracingDbProvider.Instance.CreateConnection
        //            };
        //        }
        //    }
        //}

        #region Tracing DbProvider
#if DEBUG
        public class TracingDbProvider : DbProviderFactory, IServiceProvider
        {
            public static readonly TracingDbProvider Instance = new TracingDbProvider();

            public override DbConnection CreateConnection()
            {
                return new TracingDbConnection(SqlClientFactory.Instance.CreateConnection());
            }
            public override DbCommand CreateCommand()
            {
                return new Command(SqlClientFactory.Instance.CreateCommand(), null);
            }
            public override DbCommandBuilder CreateCommandBuilder()
            {
                return base.CreateCommandBuilder();
            }

            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(DbProviderServices)) return new Services();
                return (SqlClientFactory.Instance as IServiceProvider).GetService(serviceType);
            }

            class TracingDbConnection : DbConnection
            {
                public readonly DbConnection _inner;
                public TracingDbConnection(DbConnection inner)
                {
                    //Contract.Requires( inner != null );
                    _inner = inner;
                }

                protected override DbProviderFactory DbProviderFactory { get { return Instance; } }
                protected override DbTransaction BeginDbTransaction(System.Data.IsolationLevel isolationLevel) { return _inner.BeginTransaction(isolationLevel); }
                public override void ChangeDatabase(string databaseName) { _inner.ChangeDatabase(databaseName); }
                public override void Close() { _inner.Close(); }
                public override string ConnectionString { get { return _inner.ConnectionString; } set { _inner.ConnectionString = value; } }
                protected override DbCommand CreateDbCommand() { return new Command(_inner.CreateCommand(), this); }
                public override string DataSource { get { return _inner.DataSource; } }
                public override string Database { get { return _inner.Database; } }
                public override void Open() { _inner.Open(); }
                public override string ServerVersion { get { return _inner.ServerVersion; } }
                public override System.Data.ConnectionState State { get { return _inner.State; } }
            }

            class Reader : DbDataReader
            {
                private readonly DbDataReader _inner;
                private readonly ILog _log;
                private readonly int _timestamp;
                public Reader(DbDataReader inner, ILog log, int timestamp) { _inner = inner; _log = log; _timestamp = timestamp; }
                public override void Close()
                {
                    _log.Info("DB DataReader closing " + _timestamp);
                    _inner.Close();
                    _log.Info("DB DataReader closed " + _timestamp);
                }

                public override int Depth { get { return _inner.Depth; } }
                public override int FieldCount { get { return _inner.FieldCount; } }
                public override bool GetBoolean(int ordinal) { return _inner.GetBoolean(ordinal); }
                public override byte GetByte(int ordinal) { return _inner.GetByte(ordinal); }
                public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length) { return _inner.GetBytes(ordinal, dataOffset, buffer, bufferOffset, length); }
                public override char GetChar(int ordinal) { return _inner.GetChar(ordinal); }
                public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length) { return _inner.GetChars(ordinal, dataOffset, buffer, bufferOffset, length); }
                public override string GetDataTypeName(int ordinal) { return _inner.GetDataTypeName(ordinal); }
                public override DateTime GetDateTime(int ordinal) { return _inner.GetDateTime(ordinal); }
                public override decimal GetDecimal(int ordinal) { return _inner.GetDecimal(ordinal); }
                public override double GetDouble(int ordinal) { return _inner.GetDouble(ordinal); }
                public override System.Collections.IEnumerator GetEnumerator() { return (_inner as System.Collections.IEnumerable).GetEnumerator(); }
                public override Type GetFieldType(int ordinal) { return _inner.GetFieldType(ordinal); }
                public override float GetFloat(int ordinal) { return _inner.GetFloat(ordinal); }
                public override Guid GetGuid(int ordinal) { return _inner.GetGuid(ordinal); }
                public override short GetInt16(int ordinal) { return _inner.GetInt16(ordinal); }
                public override int GetInt32(int ordinal) { return _inner.GetInt32(ordinal); }
                public override long GetInt64(int ordinal) { return _inner.GetInt64(ordinal); }
                public override string GetName(int ordinal) { return _inner.GetName(ordinal); }
                public override int GetOrdinal(string name) { return _inner.GetOrdinal(name); }
                public override System.Data.DataTable GetSchemaTable() { return _inner.GetSchemaTable(); }
                public override string GetString(int ordinal) { return _inner.GetString(ordinal); }
                public override object GetValue(int ordinal) { return _inner.GetValue(ordinal); }
                public override int GetValues(object[] values) { return _inner.GetValues(values); }
                public override bool HasRows { get { return _inner.HasRows; } }
                public override bool IsClosed { get { return _inner.IsClosed; } }
                public override bool IsDBNull(int ordinal) { return _inner.IsDBNull(ordinal); }
                public override bool NextResult() { return _inner.NextResult(); }
                public override bool Read() { return _inner.Read(); }
                public override int RecordsAffected { get { return _inner.RecordsAffected; } }
                public override object this[string name] { get { return _inner[name]; } }
                public override object this[int ordinal] { get { return _inner[ordinal]; } }
            }

            class Command : DbCommand
            {
                private static int _counter;
                private readonly DbCommand _inner;
                private readonly int _timestamp;
                private TracingDbConnection _connection;
                public Command(DbCommand inner, TracingDbConnection conn)
                {
                    //Contract.Requires( inner != null );
                    _timestamp = Interlocked.Increment(ref _counter);
                    _inner = inner;
                    _connection = conn;
                }

                protected override DbDataReader ExecuteDbDataReader(System.Data.CommandBehavior behavior)
                {
                    return WithTrace("ExecuteReader", () => new Reader(_inner.ExecuteReader(behavior), Log, _timestamp));
                }
                public override int ExecuteNonQuery()
                {
                    return WithTrace("ExecuteNonQuery", () => _inner.ExecuteNonQuery());
                }
                public override object ExecuteScalar()
                {
                    return WithTrace("ExecuteScalar", () => _inner.ExecuteScalar());
                }

                static readonly ILog Log = LogManager.GetLogger("SQL TRACE");
                private T WithTrace<T>(string method, Func<T> a)
                {
                    Log.Info("DB Command Start '" + method + "'" + _timestamp + "\r\n" + CommandText);
                    var res = a();
                    Log.Info("DB Command End '" + method + "'" + _timestamp);
                    return res;
                }

                public override void Cancel() { _inner.Cancel(); }
                public override string CommandText { get { return _inner.CommandText; } set { _inner.CommandText = value; } }
                public override int CommandTimeout { get { return _inner.CommandTimeout; } set { _inner.CommandTimeout = value; } }
                public override System.Data.CommandType CommandType { get { return _inner.CommandType; } set { _inner.CommandType = value; } }
                protected override DbParameter CreateDbParameter() { return _inner.CreateParameter(); }
                protected override DbConnection DbConnection { get { return _connection; } set { _inner.Connection = value == null ? null : (value as TracingDbConnection)._inner; } }
                protected override DbParameterCollection DbParameterCollection { get { return _inner.Parameters; } }
                protected override DbTransaction DbTransaction { get { return _inner.Transaction; } set { _inner.Transaction = value; } }
                public override bool DesignTimeVisible { get { return false; } set { } }
                public override void Prepare() { _inner.Prepare(); }
                public override System.Data.UpdateRowSource UpdatedRowSource { get { return _inner.UpdatedRowSource; } set { _inner.UpdatedRowSource = value; } }
            }

            class CmdDefinition : DbCommandDefinition
            {
                private readonly DbCommandDefinition _inner;
                public CmdDefinition(DbCommandDefinition inner)
                {
                    //Contract.Requires( inner != null );
                    _inner = inner;
                }

                public override DbCommand CreateCommand()
                {
                    return new Command(_inner.CreateCommand(), null);
                }
            }

            class Services : DbProviderServices
            {
                readonly DbProviderServices _inner = (SqlClientFactory.Instance as IServiceProvider).GetService(typeof(DbProviderServices)) as DbProviderServices;

                protected override DbCommandDefinition CreateDbCommandDefinition(DbProviderManifest providerManifest, DbCommandTree commandTree)
                {
                    return new CmdDefinition(_inner.CreateCommandDefinition(providerManifest, commandTree));
                }

                protected override DbProviderManifest GetDbProviderManifest(string manifestToken)
                {
                    return _inner.GetProviderManifest(manifestToken);
                }

                protected override string GetDbProviderManifestToken(DbConnection connection)
                {
                    var cn = connection as TracingDbConnection;
                    return _inner.GetProviderManifestToken(cn._inner);
                }
            }
        }
#endif
        #endregion

    }
}