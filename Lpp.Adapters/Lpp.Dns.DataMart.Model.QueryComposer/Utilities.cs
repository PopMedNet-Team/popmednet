using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Dns.DataMart.Model.Settings;
using System.Linq.Expressions;
using System.ComponentModel;
using MySql.Data.Entity;
using System.Data.Entity.Core.Objects;
//using Oracle.ManagedDataAccess.Client;
//using Oracle.ManagedDataAccess.Types;

namespace Lpp.Dns.DataMart.Model.QueryComposer
{
    public static class Utilities
    {

        public static string BuildConnectionString(IDictionary<string,object> settings, log4net.ILog logger)
        {
            string server = settings.GetAsString("Server", "");
            string port = settings.GetAsString("Port", "");
            string userId = settings.GetAsString("UserID", "");
            string password = settings.GetAsString("Password", "");
            string database = settings.GetAsString("Database", "");
            string dataSourceName = settings.GetAsString("DataSourceName", "");
            string connectionTimeout = settings.GetAsString("ConnectionTimeout", "15");
            string commandTimeout = settings.GetAsString("CommandTimeout", "120");
            string dataSource = settings.GetAsString("Data Source", "");

            logger.Debug("Connection timeout: " + connectionTimeout + ", Command timeout: " + commandTimeout);

            if (!settings.ContainsKey("DataProvider"))
                throw new Exception(CommonMessages.Exception_MissingDataProviderType);

            string connectionString = string.Empty;
            switch ((Model.Settings.SQLProvider)Enum.Parse(typeof(Model.Settings.SQLProvider), settings.GetAsString("DataProvider", ""), true))
            {
                case Model.Settings.SQLProvider.ODBC:
                    if (string.IsNullOrEmpty(dataSourceName))
                    {
                        throw new Exception(CommonMessages.Exception_MissingODBCDatasourceName);
                    }
                    connectionString = string.Format("DSN={0}", dataSourceName);
                    break;

                case Model.Settings.SQLProvider.PostgreSQL:
                    if (string.IsNullOrEmpty(server))
                    {
                        throw new Exception(CommonMessages.Exception_MissingDatabaseServer);
                    }
                    if (string.IsNullOrEmpty(database))
                    {
                        throw new Exception(CommonMessages.Exception_MissingDatabaseName);
                    }
                    if (!string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(password))
                    {
                        throw new Exception(CommonMessages.Exception_MissingDatabasePassword);
                    }

                    if (port == null || port == string.Empty)
                        port = "5432";
                    connectionString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};Timeout={5};CommandTimeout={6}", server, port, userId, password, database, connectionTimeout, commandTimeout);
                    break;

                case Model.Settings.SQLProvider.SQLServer:
                    if (string.IsNullOrEmpty(server))
                    {
                        throw new Exception(CommonMessages.Exception_MissingDatabaseServer);
                    }
                    if (string.IsNullOrEmpty(database))
                    {
                        throw new Exception(CommonMessages.Exception_MissingDatabaseName);
                    }
                    if (!string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(password))
                    {
                        throw new Exception(CommonMessages.Exception_MissingDatabasePassword);
                    }

                    if (port != null && port != string.Empty) server += ", " + port;
                    connectionString = userId != null && userId != string.Empty ? String.Format("server={0};User ID={1};Password={2};Database={3}; Connection Timeout={4}", server, userId, password, database, connectionTimeout) :
                                                                                  String.Format("server={0};integrated security=True;Database={1}; Connection Timeout={2}", server, database, connectionTimeout);
                    break;

                case Model.Settings.SQLProvider.MySQL:
                    if (string.IsNullOrEmpty(server))
                    {
                        throw new Exception(CommonMessages.Exception_MissingDatabaseServer);
                    }
                    if (string.IsNullOrEmpty(database))
                    {
                        throw new Exception(CommonMessages.Exception_MissingDatabaseName);
                    }
                    if (!string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(password))
                    {
                        throw new Exception(CommonMessages.Exception_MissingDatabasePassword);
                    }

                    if (port == null || port == string.Empty)
                        port = "3306";
                    connectionString = String.Format("Server={0};Port={1};Database={2};User Id={3};Password={4}", server, port, database, userId, password);
                    break;

                case Model.Settings.SQLProvider.Oracle:
                    if (string.IsNullOrEmpty(server))
                    {
                        throw new Exception(CommonMessages.Exception_MissingDatabaseServer);
                    }
                    if (string.IsNullOrEmpty(database))
                    {
                        throw new Exception(CommonMessages.Exception_MissingDatabaseName);
                    }
                    if (!string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(password))
                    {
                        throw new Exception(CommonMessages.Exception_MissingDatabasePassword);
                    }
                    Oracle.ManagedDataAccess.Client.OracleConnectionStringBuilder builder = new Oracle.ManagedDataAccess.Client.OracleConnectionStringBuilder();
                    //If userID is set to "/", password is not needed
                    if (string.IsNullOrWhiteSpace(password) && (userId == "/"))
                    {
                        builder.UserID = userId;
                    }
                    if (!string.IsNullOrWhiteSpace(password) && (!string.IsNullOrWhiteSpace(userId)))
                    {
                        builder.UserID = userId;
                        builder.Password = password;
                    }
                    //If Port is filled in
                    if (!string.IsNullOrWhiteSpace(port))
                    {
                        builder.ConnectionString = string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SERVICE_NAME={2})));User ID={3};Password={4};", server, port, database, userId, password);
                    }
                    //if port is not filled in, set default port to 1521
                    if (string.IsNullOrWhiteSpace(port))
                    {
                        builder.ConnectionString = string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SERVICE_NAME={2})));User ID={3};Password={4};", server, 1521, database, userId, password);
                    }

                    connectionString = builder.ConnectionString;
                    break;

                default:
                    throw new Exception(CommonMessages.Exception_InvalidDataProviderType);
            }
            return connectionString;
        }

        public static int GetCommandTimeout(IDictionary<string, object> settings)
        {
            string commandTimeout = settings.GetAsString("CommandTimeout", "600");
            int val = 600;
            if (Int32.TryParse(commandTimeout, out val))
                return val;
            else return 600;
        }

        public static System.Data.Common.DbConnection OpenConnection(IDictionary<string, object> settings, log4net.ILog logger, bool openConnection = true)
        {
            string connectionString = BuildConnectionString(settings, logger);

            System.Data.Common.DbConnection conn = null;
            var dataProvider = (Model.Settings.SQLProvider)Enum.Parse(typeof(Model.Settings.SQLProvider), settings.GetAsString("DataProvider", ""), true);
            switch (dataProvider)
            {
                case SQLProvider.ODBC:
                    conn = new System.Data.Odbc.OdbcConnection(connectionString);
                    break;
                case SQLProvider.SQLServer:
                    conn = new System.Data.SqlClient.SqlConnection(connectionString);
                    break;
                case SQLProvider.PostgreSQL:
                    conn = new Npgsql.NpgsqlConnection(connectionString);
                    break;
                case SQLProvider.MySQL:
                    conn = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
                    break;
                case SQLProvider.Oracle:
                    conn = new Oracle.ManagedDataAccess.Client.OracleConnection(connectionString);
                    break;
                default:
                    throw new NotSupportedException("Only ODBC, SqlServer, PostgreSQL, and MySQL currently supported.");
            }

            if (openConnection)
            {
                conn.Open();
            }

            return conn;
        }

        public static object ConvertDBNullToNull(this object value)
        {
            if (value is DBNull)
            {
                return null;
            }
            return value;
        }

        /// <summary>
        /// Returns the description attribute of an Enum as applicable instead of just the tostring version.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="useDescription"></param>
        /// <returns></returns>
        public static string ToString(this Enum obj, bool useDescription = true)
        {
            if (!useDescription)
                return obj.ToString();

            string description = obj.ToString();
            try
            {
                var type = obj.GetType();
                var memberInfo = type.GetMember(obj.ToString());
                if (memberInfo != null && memberInfo.Length > 0)
                {
                    var attributes = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                    if(attributes != null && attributes.Length > 0)
                        description = ((System.ComponentModel.DescriptionAttribute)attributes[0]).Description;
                }
            }
            catch { }

            return description;
        }

        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        /// <summary>
        /// Creates a lambda that maps the fields specified between the specified types.
        /// </summary>
        /// <typeparam name="T">The source type.</typeparam>
        /// <typeparam name="R">The destination type.</typeparam>
        /// <param name="fields">The comma delimited fields to map between the types.</param>
        /// <returns></returns>
        public static Func<T, R> MapToClass<T, R>(string fields)
        {
            return MapToClass<T, R>(fields.Split(',').Select(o => o.Trim()));
        }

        /// <summary>
        /// Creates a lambda that maps the fields specified between the specified types.
        /// </summary>
        /// <typeparam name="T">The source type.</typeparam>
        /// <typeparam name="R">The destination type.</typeparam>
        /// <param name="fields">The fields to map between the types.</param>
        /// <returns></returns>
        public static Func<T, R> MapToClass<T, R>(IEnumerable<string> fields)
        {
            // input parameter "o" of originating type
            var xParameter = Expression.Parameter(typeof(T), "o");

            // create initializers
            var bindings = fields
                .Select(property =>
                {
                    var originatingMemberInfo = typeof(T).GetProperty(property);
                    // original value "o.Field1"; source
                    var xOriginal = Expression.Property(xParameter, originatingMemberInfo);

                    // property "Field1"; destination
                    var mi = typeof(R).GetProperty(property);
                    // set value "Field1 = o.Field1"
                    return Expression.Bind(mi, xOriginal);
                }
            );

            // new statement "new R()"
            var xNew = Expression.New(typeof(R));

            // initialization "new R { Field1 = o.Field1, Field2 = o.Field2 }"
            var xInit = Expression.MemberInit(xNew, bindings);

            // expression "o => new R { Field1 = o.Field1, Field2 = o.Field2 }"
            var lambda = Expression.Lambda<Func<T, R>>(xInit, xParameter);

            // compile to Func<Data, Data>
            return lambda.Compile();
        }
    }

    public static class ExceptionHelpers
    {
        public static string UnwindException(this Exception exception, bool showStackTrace = false)
        {
            if (exception.InnerException == null)
                return exception.ToString();

            StringBuilder sb = new StringBuilder();
            UnwindException(exception, sb, showStackTrace);
            return sb.ToString();
        }

        public static void UnwindException(this Exception exception, StringBuilder sb, bool showStackTrace = false)
        {
            sb.AppendLine(showStackTrace ? exception.ToString() : exception.Message);

            if (exception.InnerException != null)
                UnwindException(exception.InnerException, sb, showStackTrace);
        }
    }

    public static class QueryComposerDTOHelpers
    {
        /// <summary>
        /// Gets the JObject representing the term values from the specified QueryComposerTermDTO.
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public static Newtonsoft.Json.Linq.JObject GetValuesJObject(this DTO.QueryComposer.QueryComposerTermDTO term)
        {
            return term.Values["Values"] as Newtonsoft.Json.Linq.JObject;
        }

        /// <summary>
        /// Gets the JToken from the QueryComposerTermDTO for the specified key.
        /// </summary>
        /// <param name="term"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Newtonsoft.Json.Linq.JToken GetValue(this DTO.QueryComposer.QueryComposerTermDTO term, string key)
        {
            return ((Newtonsoft.Json.Linq.JObject)term.Values["Values"]).GetValue(key);
        }

        /// <summary>
        /// Gets the value as a string from the QueryComposerTermDTO for the specified key.
        /// </summary>
        /// <param name="term"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetStringValue(this DTO.QueryComposer.QueryComposerTermDTO term, string key)
        {
            return (string)term.GetValue(key);
        }

        /// <summary>
        /// Gets the value as an enum from the QueryComposerTermDTO for the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="term"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetEnumValue<T>(this DTO.QueryComposer.QueryComposerTermDTO term, string key, out T value) where T : struct, IConvertible
        {
            if (term == null)
            {
                value = default(T);
                return false;
            }

            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enum type.");
            }

            return Enum.TryParse<T>(term.GetStringValue(key), out value);
        }

        /// <summary>
        /// Returns the content of the specified key as an IEnumerable&gt;string>.
        /// </summary>
        /// <param name="term"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetStringCollection(this DTO.QueryComposer.QueryComposerTermDTO term, string key){
            var token = term.GetValue(key);
            return token.Values<string>();
        }

        public static IEnumerable<string> GetCodeStringCollection(this DTO.QueryComposer.QueryComposerTermDTO term)
        {
            if (term.GetValue("CodeValues") == null)
            {
                //legacy stuff
                var token = term.GetValue("Codes");
                return token.Values<string>();
            }
            else
            {
                var token = term.GetValue("CodeValues");
                List<Dns.DTO.CodeSelectorValueDTO> dto = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dns.DTO.CodeSelectorValueDTO>>(token.ToString());
                return dto.Select(p => p.Code);
            }
        }

        /// <summary>
        /// Parses the "CodeValues" value of the term into a collection of Dns.DTO.CodeSelectorValueDTO.
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public static IEnumerable<Dns.DTO.CodeSelectorValueDTO> GetCodeSelectorValues(this DTO.QueryComposer.QueryComposerTermDTO term)
        {
            var token = term.GetValue("CodeValues");
            if (token == null)
                return Enumerable.Empty<Dns.DTO.CodeSelectorValueDTO>();

            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dns.DTO.CodeSelectorValueDTO>>(token.ToString());
        }
    }

    public static class IQueryableExtensions
    {
        /// <summary>
        /// For an Entity Framework IQueryable, returns the SQL with inlined Parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string ToTraceQuery(this IQueryable query)
        {
            ObjectQuery objectQuery = GetQueryFromQueryable(query);

            var result = objectQuery.ToTraceString();
            foreach (var parameter in objectQuery.Parameters)
            {
                var name = "@" + parameter.Name;
                var value = "'" + parameter.Value.ToString() + "'";
                result = result.Replace(name, value);
            }

            return result;
        }

        /// <summary>
        /// For an Entity Framework IQueryable, returns the SQL and Parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string ToTraceString(this IQueryable query)
        {
            ObjectQuery objectQuery = GetQueryFromQueryable(query);

            var traceString = new StringBuilder();

            traceString.AppendLine(objectQuery.ToTraceString());
            traceString.AppendLine();

            foreach (var parameter in objectQuery.Parameters)
            {
                traceString.AppendLine(parameter.Name + " [" + parameter.ParameterType.FullName + "] = " + parameter.Value);
            }

            return traceString.ToString();
        }

        private static System.Data.Entity.Core.Objects.ObjectQuery GetQueryFromQueryable(IQueryable query)
        {
            var internalQueryField = query.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(f => f.Name.Equals("_internalQuery")).FirstOrDefault();
            var internalQuery = internalQueryField.GetValue(query);
            var objectQueryField = internalQuery.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Where(f => f.Name.Equals("_objectQuery")).FirstOrDefault();
            return objectQueryField.GetValue(internalQuery) as System.Data.Entity.Core.Objects.ObjectQuery;
        }
    }
}
