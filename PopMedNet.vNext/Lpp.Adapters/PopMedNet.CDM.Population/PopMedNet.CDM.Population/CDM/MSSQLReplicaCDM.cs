using Serilog;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PopMedNet.CDM.Population.CDM
{
    public class MSSQLReplicaCDM : IReplicaCDM
    {
        private readonly DbConnection Connection;
        private readonly ILogger Logger;

        public MSSQLReplicaCDM(DbConnection connection, ILogger logger)
        {
            Connection = connection;
            Logger = logger;
        }

        public async Task InitializeAsync()
        {
            this.Logger.Information("Initializing MSSQL Replica CDM");
        }

        public async Task PopulateAsync(DbSchema[] schemas)
        {
            using (var conn = new SqlConnection(Connection.ConnectionString))
            {
                await conn.OpenAsync();
                this.Logger.Information($"Opened connection to {conn.Database} on {conn.DataSource}");
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (var schema in schemas)
                        {
                            var cleanCmd = new SqlCommand($"TRUNCATE TABLE {schema.TableName}", conn, tran);

                            await cleanCmd.ExecuteNonQueryAsync();
                            this.Logger.Information($"table {schema.TableName} truncated");
                            using (var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran))
                            {
                                bulkCopy.DestinationTableName = $"[dbo].{schema.TableName}";

                                foreach (var col in schema.Columns)
                                {
                                    bulkCopy.ColumnMappings.Add(col.Name, col.Name);
                                }

                                await bulkCopy.WriteToServerAsync(schema.Records);
                            }
                            this.Logger.Information($"{schema.Records.Rows.Count} records have been imported into {schema.TableName}");
                        }

                        await tran.CommitAsync();
                        this.Logger.Information($"All transactions have been committed.");
                    }
                    catch (Exception ex)
                    {
                        this.Logger.Error(ex, $"An error occured while doing operations.  Rolling back all changes to {conn.Database} on {conn.DataSource}");
                        await tran.RollbackAsync();
                        throw;
                    }
                }
                await conn.CloseAsync();
            }
        }
    }
}
