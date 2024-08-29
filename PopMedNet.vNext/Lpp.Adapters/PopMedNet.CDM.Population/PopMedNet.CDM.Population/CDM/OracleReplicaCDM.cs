using Oracle.ManagedDataAccess.Client;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.CDM.Population.CDM
{
    public class OracleReplicaCDM : IReplicaCDM
    {
        private readonly DbConnection Connection;
        private readonly ILogger Logger;

        public OracleReplicaCDM(DbConnection connection, ILogger logger)
        {
            Connection = connection;
            Logger = logger;
        }

        public async Task InitializeAsync()
        {
            this.Logger.Information("Initializing Oracle Replica CDM");
        }

        public async Task PopulateAsync(DbSchema[] schemas)
        {
            using (var conn = new OracleConnection(Connection.ConnectionString))
            {
                await conn.OpenAsync();
                this.Logger.Information($"Opened connection to {conn.Database} on {conn.DataSource}");
                try
                {
                    foreach (var schema in schemas)
                    {
                        var cleanCmd = new OracleCommand($"TRUNCATE TABLE {schema.TableName}", conn);

                        await cleanCmd.ExecuteNonQueryAsync();
                        this.Logger.Information($"table {schema.TableName} truncated");
                        using (var bulkCopy = new OracleBulkCopy(conn, OracleBulkCopyOptions.Default))
                        {
                            bulkCopy.DestinationTableName = $"{schema.TableName}";

                            foreach (var col in schema.Columns)
                            {
                                bulkCopy.ColumnMappings.Add(col.Name, col.Name);
                            }

                            bulkCopy.WriteToServer(schema.Records);
                        }
                        this.Logger.Information($"{schema.Records.Rows.Count} records have been imported into {schema.TableName}");
                    }

                    this.Logger.Information($"All transactions have been committed.");
                }
                catch (Exception ex)
                {
                    this.Logger.Error(ex, $"An error occured while doing operations.");
                    throw;
                }
                await conn.CloseAsync();
            }
        }
    }
}
