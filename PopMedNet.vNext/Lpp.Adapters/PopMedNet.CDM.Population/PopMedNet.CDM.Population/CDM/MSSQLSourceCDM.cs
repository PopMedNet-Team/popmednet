using Serilog;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PopMedNet.CDM.Population.CDM
{
    public class MSSQLSourceCDM : ISourceCDM
    {
        private readonly DbConnection Connection;
        private readonly ILogger Logger;

        public MSSQLSourceCDM(DbConnection connection, ILogger logger)
        {
            Connection = connection;
            Logger = logger;
        }

        public async Task InitializeAsync()
        {
            this.Logger.Information("Initializing MSSQL Source CDM");
        }

        public async Task<DbSchema[]> RetrieveSourceRecordsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var schemas = new ConcurrentBag<DbSchema>();
                var tables = new List<string>();

                using (var conn = new SqlConnection(Connection.ConnectionString))
                {
                    await conn.OpenAsync(cancellationToken);

                    this.Logger.Information($"Starting to gather all the tables within the database: {conn.Database}");
                    using (var dbSchemaCmd = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'", conn))
                    {
                        using (var reader = await dbSchemaCmd.ExecuteReaderAsync(cancellationToken))
                        {
                            while (await reader.ReadAsync())
                            {
                                var table = reader[0].ToString();
                                this.Logger.Information($"Found Sql Table: {table}");
                                tables.Add(table);
                            }
                        }
                    }
                }

                foreach (var t in tables)
                {
                    var columns = new List<string>();

                    using (var conn = new SqlConnection(Connection.ConnectionString))
                    {
                        await conn.OpenAsync(cancellationToken);

                        using (var tableSchemaCmd = new SqlCommand($"SELECT COLUMN_NAME, ORDINAL_POSITION, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{t}'", conn))
                        {
                            using (var reader = await tableSchemaCmd.ExecuteReaderAsync(cancellationToken))
                            {
                                while (await reader.ReadAsync())
                                {
                                    this.Logger.Information($"Sql Table {t} found column: {reader[0].ToString()}");
                                    columns.Add(reader[0].ToString());
                                }
                            }
                        }

                        var cmdString = $"SELECT {string.Join(",", columns)} FROM {t}";
                        this.Logger.Information($"Querying table {t} for its records");
                        DataTable table = new DataTable();
                        using (var command = new SqlCommand(cmdString, conn))
                        {
                            SqlDataAdapter ds = new SqlDataAdapter(command);
                            ds.Fill(table);
                        }

                        schemas.Add(new DbSchema
                        {
                            TableName = t,
                            Records = table,
                            Columns = columns.Select(x => new DbColumn { Name = x }).ToArray()
                        });

                        this.Logger.Information($"Gathered all the records for table: {t}");
                    }
                }

                return schemas.ToArray();
            }
            catch (System.Exception ex)
            {
                this.Logger.Error(ex, $"An Error occurred:");
                throw;
            }
        }
    }
}
