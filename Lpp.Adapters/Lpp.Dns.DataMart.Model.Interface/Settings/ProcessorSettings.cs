using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.Settings
{
    public static class ProcessorSettings
    {

        public static IEnumerable<ProcessorSetting> CreateDbSettings(IEnumerable<SQLProvider> providers)
        {
            if (providers == null || !providers.Any())
                yield break;

            yield return new ProcessorSetting { Title = "Data Provider", Key = "DataProvider", ValueType = typeof(SQLProvider) };

            if (providers.Any(p => p == SQLProvider.SQLServer || p == SQLProvider.PostgreSQL))
            {
                yield return new ProcessorSetting { Title = "Server", Key = "Server", ValueType = typeof(string) };
                yield return new ProcessorSetting { Title = "Port", Key = "Port", ValueType = typeof(string) };
                yield return new ProcessorSetting { Title = "User ID", Key = "UserID", ValueType = typeof(string) };
                yield return new ProcessorSetting { Title = "Password", Key = "Password", ValueType = typeof(string) };
                yield return new ProcessorSetting { Title = "Database", Key = "Database", ValueType = typeof(string) };
            }

            if (providers.Any(p => p == SQLProvider.ODBC))
            {
                yield return new ProcessorSetting { Title = "Data Source", Key = "DataSourceName", ValueType = typeof(string) };
            }

            yield return new ProcessorSetting { Title = "Connection Timeout", Key = "ConnectionTimeout", ValueType = typeof(string) };
            yield return new ProcessorSetting { Title = "Command Timeout", Key = "CommandTimeout", ValueType = typeof(string) };
        }

        public static readonly string[] DbSettingKeys = new[] { "DataProvider", "Server", "Port", "UserID", "Password", "Database", "DataSourceName", "ConnectionTimeout", "CommandTimeout" };

        public static void AddDbSettingKeys(IDictionary<string, string> properties, IEnumerable<SQLProvider> providers)
        {
            if (providers == null || !providers.Any())
                return;

            properties.Add("DataProvider", string.Empty);

            if (providers.Any(p => p == SQLProvider.SQLServer || p == SQLProvider.PostgreSQL))
            {
                properties.Add("Server", string.Empty);
                properties.Add("Port", string.Empty);
                properties.Add("UserID", string.Empty);
                properties.Add("Password", string.Empty);
                properties.Add("Database", string.Empty);
            }

            if (providers.Any(p => p == SQLProvider.ODBC))
            {
                properties.Add("DataSourceName", string.Empty);
            }

            properties.Add("ConnectionTimeout", "");
            properties.Add("CommandTimeout", "");
        }

        public static bool IsDbSetting(string key)
        {
            return DbSettingKeys.Contains(key, StringComparer.InvariantCultureIgnoreCase);
        }

        public static bool HasDbSettings(IEnumerable<ProcessorSetting> settings)
        {
            if (settings == null || !settings.Any())
                return false;

            return settings.Any(s => IsDbSetting(s.Key));
        }

        /// <summary>
        /// Tries to get the value from the specified settings collection for the specified key, falls back to the default value on any failure to get the value or the value is null.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static object GetSetting(this IDictionary<string, object> settings, string key, object defaultValue)
        {
            if (settings == null || !settings.ContainsKey(key))
                return defaultValue;

            object value = null;
            if (!settings.TryGetValue(key, out value))
                return defaultValue;

            if (value == null)
                return defaultValue;

            return value;
        }

        /// <summary>
        /// Tries to get the value as a string from the specified settings collection for the specified key.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetAsString(this IDictionary<string, object> settings, string key, string defaultValue)
        {
            return (GetSetting(settings, key, defaultValue) ?? "").ToString();
        }

    }
}
