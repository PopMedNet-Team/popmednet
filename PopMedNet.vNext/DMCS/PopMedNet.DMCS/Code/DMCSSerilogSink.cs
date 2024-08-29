using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PopMedNet.DMCS.Data;
using PopMedNet.DMCS.Models;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.Code
{

    public class DMCSSerilogSink : IBatchedLogEventSink
    {
        readonly IFormatProvider formatProvider;
        readonly IConfiguration configuration;
        readonly IServiceProvider services;

        public DMCSSerilogSink(IConfiguration config, IFormatProvider formatProvider, IServiceProvider services)
        {
            this.formatProvider = formatProvider;
            this.configuration = config;
            this.services = services;
        }

        public async Task EmitBatchAsync(IEnumerable<LogEvent> batch) 
        {
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("ID", typeof(Guid));
            table.Columns.Add("DateTime", typeof(DateTimeOffset));
            table.Columns.Add("Level", typeof(int));
            table.Columns.Add("Source", typeof(string));
            table.Columns.Add("Message", typeof(string));
            table.Columns.Add("Exception", typeof(string));
            table.Columns.Add("ResponseID", typeof(Guid));

            foreach (var item in batch)
            {
                var message = item.RenderMessage(formatProvider);
                var source = string.Empty;

                if (item.Properties.ContainsKey("SourceContext"))
                {
                    source = item.Properties.Where(x => x.Key == "SourceContext").Select(x => x.Value).FirstOrDefault().ToString();
                }               

                var objs = new List<object>();
                var newID = DatabaseEx.NewGuid();
                objs.Add(newID);
                objs.Add(item.Timestamp);
                objs.Add((int)item.Level);
                objs.Add(source);
                objs.Add(message);

                string exceptionString = string.Empty;
                if (item.Exception != null)
                {
                    exceptionString = JsonConvert.SerializeObject(item.Exception);
                    objs.Add(exceptionString);
                }
                else
                {
                    objs.Add(DBNull.Value);
                }

                if (item.Properties.ContainsKey("ResponseID"))
                    objs.Add(item.Properties["ResponseID"].ToString());
                else
                    objs.Add(DBNull.Value);

                table.Rows.Add(objs.ToArray());

                if (item.Properties.ContainsKey("ResponseID"))
                {
                    using (var scope = this.services.CreateScope())
                    {
                        var logHub = scope.ServiceProvider.GetRequiredService<IHubContext<LogHub>>();
                        
                        await logHub.Clients.Group(item.Properties["ResponseID"].ToString()).SendAsync("addedResponseLog", new LogDTO
                        {
                            DateTime = item.Timestamp,
                            Level = (Data.Enums.LogEventLevel)(LogEventLevel)((int)item.Level),
                            Message = message,
                            Source = source,
                            Exception = exceptionString
                        });
                    }
                }
            }

            using (var conn = new SqlConnection(this.configuration.GetConnectionString("DBContextConnection")))
            {
                await conn.OpenAsync();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran))
                        {
                            bulkCopy.DestinationTableName = "[dbo].[Logs]";
                            bulkCopy.ColumnMappings.Add("ID", "ID");
                            bulkCopy.ColumnMappings.Add("DateTime", "DateTime");
                            bulkCopy.ColumnMappings.Add("Level", "Level");
                            bulkCopy.ColumnMappings.Add("Source", "Source");
                            bulkCopy.ColumnMappings.Add("Message", "Message");
                            bulkCopy.ColumnMappings.Add("Exception", "Exception");
                            bulkCopy.ColumnMappings.Add("ResponseID", "ResponseID");

                            await bulkCopy.WriteToServerAsync(table);

                        }
                        await tran.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await tran.RollbackAsync();
                        throw;
                    }
                }
                await conn.CloseAsync();
            }
        }

        public async Task OnEmptyBatchAsync()
        {
            await Task.CompletedTask;
        }
    }

    public static class DMCSSerilogSinkExtensions
    {
        public static LoggerConfiguration DMCSSerilogSink(this LoggerSinkConfiguration loggerSinkConfiguration,
                  IConfiguration appConfiguration = null,
                  IFormatProvider formatProvider = null,
                  IServiceProvider services = null)
        {
            var dmcsSink = new DMCSSerilogSink(appConfiguration, formatProvider, services);

            var batchingOptions = new PeriodicBatchingSinkOptions
            {
                BatchSizeLimit = 100,
                Period = TimeSpan.FromSeconds(2),
                EagerlyEmitFirstEvent = true,
                QueueLimit = 10000
            };

            var batchingSink = new PeriodicBatchingSink(dmcsSink, batchingOptions);

            return loggerSinkConfiguration.Sink(batchingSink);
        }
    }
}
