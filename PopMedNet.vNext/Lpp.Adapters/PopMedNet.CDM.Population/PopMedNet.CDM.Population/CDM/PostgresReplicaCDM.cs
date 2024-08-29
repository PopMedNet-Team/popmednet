using Npgsql;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PopMedNet.CDM.Population.CDM
{
    public class PostgresReplicaCDM : IReplicaCDM
    {
        private readonly DbConnection Connection;
        private readonly ILogger Logger;

        public PostgresReplicaCDM(DbConnection connection, ILogger logger)
        {
            Connection = connection;
            Logger = logger;
        }

        public async Task InitializeAsync()
        {
            this.Logger.Information("Initializing Postgres Replica CDM");
        }

        public async Task PopulateAsync(DbSchema[] schemas)
        {
            Dictionary<Type, NpgsqlTypes.NpgsqlDbType> typeDict = new Dictionary<Type, NpgsqlTypes.NpgsqlDbType>();

            typeDict.Add(typeof(int), NpgsqlTypes.NpgsqlDbType.Integer);
            typeDict.Add(typeof(double), NpgsqlTypes.NpgsqlDbType.Double);
            typeDict.Add(typeof(decimal), NpgsqlTypes.NpgsqlDbType.Numeric);
            typeDict.Add(typeof(string), NpgsqlTypes.NpgsqlDbType.Varchar);
            typeDict.Add(typeof(DateTime), NpgsqlTypes.NpgsqlDbType.Date);
            typeDict.Add(typeof(char[]), NpgsqlTypes.NpgsqlDbType.Varchar);
            typeDict.Add(typeof(Guid), NpgsqlTypes.NpgsqlDbType.Uuid);


            using (var conn = new NpgsqlConnection(Connection.ConnectionString))
            {
                await conn.OpenAsync();
                this.Logger.Information($"Opened connection to {conn.Database} on {conn.DataSource}");
                try
                {
                    foreach (var schema in schemas)
                    {
                        var cleanCmd = new NpgsqlCommand($"TRUNCATE TABLE \"{this.Connection.DbSchema}\".\"{schema.TableName}\"", conn);

                        await cleanCmd.ExecuteNonQueryAsync();
                        this.Logger.Information($"table {schema.TableName} truncated");

                        string sql = $"COPY \"{this.Connection.DbSchema}\".\"{schema.TableName}\" ( ";
                        foreach (System.Data.DataColumn col in schema.Records.Columns)
                        {
                            sql += ($"\"{col.ColumnName}\",");
                        }
                        sql = sql.TrimEnd(',') + ") FROM STDIN (FORMAT BINARY)";


                        int nRows = schema.Records.Rows.Count;
                        using (var BulkWrite = conn.BeginBinaryImport(sql))
                        {
                            for (int idRow = 0; idRow < nRows; idRow++)
                            {
                                BulkWrite.StartRow();
                                foreach (System.Data.DataColumn col in schema.Records.Columns)
                                {
                                    if (schema.Records.Rows[idRow].IsNull(col))
                                    {
                                        BulkWrite.WriteNull();
                                    }
                                    else
                                    {
                                        if (col.DataType == typeof(string) && string.IsNullOrEmpty(schema.Records.Rows[idRow].Field<string>(col)))
                                        {
                                            BulkWrite.WriteNull();
                                        }
                                        else
                                        {
                                            BulkWrite.Write(schema.Records.Rows[idRow][col.Ordinal], typeDict[col.DataType]);
                                        }
                                    }
                                }

                            }
                            BulkWrite.Complete();
                            this.Logger.Information($"{schema.Records.Rows.Count} records have been imported into {schema.TableName}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Error(ex, $"An error occured while doing operations.  Rolling back all changes to {conn.Database} on {conn.DataSource}");
                    throw;
                }

                await conn.CloseAsync();
            }
        }
    }
}
