using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Lpp.Dns.Data;
using Newtonsoft.Json;

namespace Lpp.Dns.Api.DataMartClient
{
    /// <summary>
    /// 
    /// </summary>
    public class MetadataRefreshPostProcessor : IPostProcessDocumentContent
    {
        static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(MetadataRefreshPostProcessor));
        private DataContext _db = null;
        private string _uploadDir = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="uploadDir"></param>
        public void Initialize(DataContext db, string uploadDir)
        {
            _db = db;
            _uploadDir = uploadDir;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cachedDocumentFileName"></param>
        public async Task ExecuteAsync(Document document, string cachedDocumentFileName)
        {
            if (!string.Equals(document.Kind, "SummaryTables.RefreshDates", StringComparison.OrdinalIgnoreCase))
            {
                Logger.Debug($"Document was not of SummaryTables.RefreshDates with Document ID {document.ID}");
                return;
            }

            if (_db == null)
            {
                Logger.Error("The ExecuteAync Function was called before Initialize");
                throw new Exception("DataContext is null.  Please Call Initialize before ExecuteAsync");
            }

            var dataMartID = await (from dm in _db.DataMarts.AsNoTracking()
                                    join reqDM in _db.RequestDataMarts.AsNoTracking() on dm.ID equals reqDM.DataMartID
                                    join res in _db.Responses.AsNoTracking() on reqDM.ID equals res.RequestDataMartID
                                    where res.ID == document.ItemID
                                    select dm.ID).FirstOrDefaultAsync();

            if (dataMartID == null || dataMartID == Guid.Empty)
            {
                Logger.Error($"Could not find the associated DataMart to the document with Document ID of {document.ID}.");
                throw new Exception("Could not find the associated DataMart to this document.");
            }

            Logger.Debug("Deserializing the Response Document");
            DTO.QueryComposer.QueryComposerResponseDTO deserialzedDoc;
            using (var stream = new FileStream(Path.Combine(_uploadDir, cachedDocumentFileName), FileMode.Open, FileAccess.Read))
            using (var sr = new StreamReader(stream))
            using (var jr = new Newtonsoft.Json.JsonTextReader(sr))
            {
                var serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Converters.Add(new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionConverter());
                deserialzedDoc = serializer.Deserialize<DTO.QueryComposer.QueryComposerResponseDTO>(jr);
            }

            Logger.Debug($"Looping over the Document {document.ID} with DataMart {dataMartID}");
            foreach (var row in deserialzedDoc.Queries.First().Results.FirstOrDefault() ?? Array.Empty<Dictionary<string,object>>())
            {
                var dt = row["DataTable"].ToString();
                var period = row["Period"].ToString();

                var hasQuarter = period.IndexOf('Q') > 0;

                if (hasQuarter)
                {
                    await _db.Database.ExecuteSqlCommandAsync(@"IF NOT EXISTS(SELECT NULL FROM DataMartAvailabilityPeriods_v2 WHERE DataMartID = @DataMartID AND DataTable = @DataTable AND PeriodCategory = @PeriodCategory AND Period = @Period)
                                                                    INSERT INTO DataMartAvailabilityPeriods_v2 (DataMartID, DataTable, PeriodCategory, Period, Year, Quarter)
                                                                    VALUES (@DataMartID, @DataTable, @PeriodCategory, @Period, @Year, @Quarter)",
                                                            new SqlParameter("DataMartID", dataMartID),
                                                            new SqlParameter("DataTable", dt),
                                                            new SqlParameter("PeriodCategory", "Q"),
                                                            new SqlParameter("Period", period),
                                                            new SqlParameter("Year", period.Substring(0, period.IndexOf('Q'))),
                                                            new SqlParameter("Quarter", period.Substring(period.IndexOf('Q') + 1)));
                }
                else
                {
                    await _db.Database.ExecuteSqlCommandAsync(@"IF NOT EXISTS(SELECT NULL FROM DataMartAvailabilityPeriods_v2 WHERE DataMartID = @DataMartID AND DataTable = @DataTable AND PeriodCategory = @PeriodCategory AND Period = @Period)
                                                                    INSERT INTO DataMartAvailabilityPeriods_v2 (DataMartID, DataTable, PeriodCategory, Period, Year)
                                                                    VALUES (@DataMartID, @DataTable, @PeriodCategory, @Period, @Year)",
                                                            new SqlParameter("DataMartID", dataMartID),
                                                            new SqlParameter("DataTable", dt),
                                                            new SqlParameter("PeriodCategory", "Y"),
                                                            new SqlParameter("Period", period),
                                                            new SqlParameter("Year", period));
                }
            }
            Logger.Debug($"Finished parsing the response doucment {document.ID}");
        }
    }
}