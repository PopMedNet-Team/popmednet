using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DataMart.Model.Settings;

namespace Lpp.Dns.DataMart.Client.Database
{
    public sealed class ConnectionTester
    {

        public bool Test(IDictionary<string,object> settings)
        {
            SQLProvider sqlProvider = (Model.Settings.SQLProvider)Enum.Parse(typeof(Model.Settings.SQLProvider), (settings["DataProvider"] ?? string.Empty).ToString(), true);

            if (sqlProvider == SQLProvider.ODBC)
            {
                return TestODBC((settings["DataSourceName"] ?? string.Empty).ToString());
            }

            object server, port, database, userID, password, connectionTimeout, commandTimeout, encrypted;
            settings.TryGetValue("Server", out server);
            settings.TryGetValue("Port", out port);
            settings.TryGetValue("Database", out database);
            settings.TryGetValue("UserID", out userID);
            settings.TryGetValue("Password", out password);
            settings.TryGetValue("ConnectionTimeout", out connectionTimeout);
            settings.TryGetValue("CommandTimeout", out commandTimeout);
            settings.TryGetValue("Encrypt", out encrypted);

            switch (sqlProvider)
            {
                case Model.Settings.SQLProvider.SQLServer:
                    return TestSQLServer((server ?? string.Empty).ToString(), (port ?? string.Empty).ToString(), (database ?? string.Empty).ToString(), (userID ?? string.Empty).ToString(), (password ?? string.Empty).ToString(), Convert.ToInt32(connectionTimeout ?? 0), bool.Parse(encrypted.ToString()));
                case Model.Settings.SQLProvider.PostgreSQL:
                    return TestPostgreSQL((server ?? string.Empty).ToString(), (port ?? string.Empty).ToString(), (database ?? string.Empty).ToString(), (userID ?? string.Empty).ToString(), (password ?? string.Empty).ToString(), Convert.ToInt32(connectionTimeout ?? 0), Convert.ToInt32(commandTimeout ?? 0), bool.Parse(encrypted.ToString()));
                //case Model.Settings.SQLProvider.MySQL:
                //    return TestMySQL((server ?? string.Empty).ToString(), (port ?? string.Empty).ToString(), (database ?? string.Empty).ToString(), (userID ?? string.Empty).ToString(), (password ?? string.Empty).ToString(), Convert.ToInt32(connectionTimeout ?? 0));
                case Model.Settings.SQLProvider.Oracle:
                    return TestOracle((server ?? string.Empty).ToString(), (port ?? string.Empty).ToString(), (database ?? string.Empty).ToString(), (userID ?? string.Empty).ToString(), (password ?? string.Empty).ToString(), Convert.ToInt32(connectionTimeout ?? 0));
                default:
                    throw new NotSupportedException("The selected data provider is not supported.");
            }
        }

        bool TestODBC(string dsn)
        {
            using (OdbcConnection conn = new OdbcConnection(string.Format("DSN={0}", dsn)))
            {
                return TestConnection(conn);
            }
        }

        bool TestSQLServer(string server, string port, string database, string userID, string password, int connectionTimeout, bool encrypted)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder();
            builder.DataSource = server + (string.IsNullOrWhiteSpace(port) ? string.Empty : ", " + port);
            builder.InitialCatalog = database;
            builder.ConnectTimeout = connectionTimeout;
            builder.Encrypt = encrypted;
            builder.TrustServerCertificate = encrypted;
            if (!string.IsNullOrWhiteSpace(userID))
            {
                builder.UserID = userID;
                builder.Password = password;
            }
            else
            {
                builder.IntegratedSecurity = true;
            }

            using (var conn = new System.Data.SqlClient.SqlConnection(builder.ToString()))
            {
                return TestConnection(conn);
            }
        }

        bool TestPostgreSQL(string server, string port, string database, string userID, string password, int connectionTimeout, int commandTimeout, bool encrypted)
        {
            Npgsql.NpgsqlConnectionStringBuilder builder = new Npgsql.NpgsqlConnectionStringBuilder();
            builder.Host = server;
            if (!string.IsNullOrWhiteSpace(port))
            {
                builder.Port = Convert.ToInt32(port);
            }
            builder.Database = database;
            builder.Username = userID;
            builder.Password = password;
            builder.Timeout = connectionTimeout;
            builder.CommandTimeout = commandTimeout;
            builder.SslMode = encrypted ? Npgsql.SslMode.Require : Npgsql.SslMode.Prefer;
            builder.TrustServerCertificate = encrypted;
            using (var conn = new Npgsql.NpgsqlConnection(builder.ToString()))
            {
                return TestConnection(conn);
            }
        }

        //bool TestMySQL(string server, string port, string database, string userID, string password, int connectionTimeout)
        //{
        //MySql.Data.MySqlClient.MySqlConnectionStringBuilder builder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder();
        //builder.Server = server;
        //    uint portMySQL = Convert.ToUInt32(port);
        //builder.Port = portMySQL;
        //    builder.Database = database;
        //    uint connectionTimeoutMySQL = Convert.ToUInt32(connectionTimeout);
        //builder.ConnectionTimeout = connectionTimeoutMySQL;
        //    if (!string.IsNullOrWhiteSpace(userID))
        //    {
        //        builder.UserID = userID;
        //        builder.Password = password;
        //    }
        //    else
        //    {
        //        builder.IntegratedSecurity = true;
        //    }

        //    using (var conn = new MySql.Data.MySqlClient.MySqlConnection(builder.ToString()))
        //    {
        //        return TestConnection(conn);
        //    }
        //}

        bool TestOracle(string server, string port, string database, string userID, string password, int connectionTimeout)
        {
            Oracle.ManagedDataAccess.Client.OracleConnectionStringBuilder builder = new Oracle.ManagedDataAccess.Client.OracleConnectionStringBuilder();
            //If userID is set to "/", password is not needed
            if (string.IsNullOrWhiteSpace(password) && (userID == "/"))
            {
                builder.UserID = userID;
            }
            if (!string.IsNullOrWhiteSpace(password) && (!string.IsNullOrWhiteSpace(userID)))
            {
                builder.UserID = userID;
                builder.Password = password;
            }
            //If Port is filled in
            if (!string.IsNullOrWhiteSpace(port))
            {
                builder.ConnectionString = string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SERVICE_NAME={2})));User ID={3};Password={4};", server, port, database, userID, password);
            }
            //if port is not filled in, set default port to 1521
            if (string.IsNullOrWhiteSpace(port))
            {
                builder.ConnectionString = string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SERVICE_NAME={2})));User ID={3};Password={4};", server, 1521, database, userID, password);
            }
            Oracle.ManagedDataAccess.Client.OracleConnection conn = new Oracle.ManagedDataAccess.Client.OracleConnection(builder.ConnectionString);
            using (conn)
            {
                return TestConnection(conn);
            }
        }

        bool TestConnection(System.Data.Common.DbConnection connection)
        {
            try
            {
                connection.Open();
            }
            catch(System.Data.SqlClient.SqlException e)
            {
                if (e.Number == 53)
                {
                    throw new Exception("Could not connect to the server. Please verify that the server information is correct and try again. If you are connecting to a remote server, please verify that you are properly connected to the internet.", e);
                }
                else if(e.Number == 258)
                {
                    throw new Exception("Could not connect to the server using this port number. Please verify that this information is correct and try again.", e);
                }
                else if(e.Number == 18456)
                {
                    throw new Exception("There was a problem with your user ID and/or password. Please verify that this information is correct and try again.", e);
                }
                else if(e.Number == 4060)
                {
                    throw new Exception("There was a problem with the database name. Please verify that this information is correct and try again.", e);
                }
                else
                    throw;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }
    }
}
