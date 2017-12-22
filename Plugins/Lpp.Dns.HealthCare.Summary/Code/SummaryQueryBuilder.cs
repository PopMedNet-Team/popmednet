using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Data.SqlClient;
//using Lpp.Data;
using Lpp.Dns.HealthCare.Summary.Data;
using Lpp.Dns.Data;

namespace Lpp.Dns.HealthCare.Summary
{
    public class SummaryQueryBuilder
    {
        // TODO Needs to be rewritten without repository parameter.
        /// <summary>
        /// Caches the periods (aka "refresh dates") metadata in the database.
        /// </summary>
        /// <param name="datamart">The datamart from which the metadata is derived</param>
        /// <param name="datasetXml">The metadata dataset in XML format</param>
        /// <param name="requestTypes">Request types supported by this model</param>
        /// <param name="r">The repository to store cache</param>
        public static void CachePeriods(IDnsDataMart datamart, string datasetXml, IEnumerable<SummaryRequestType> requestTypes)
        {
            MemoryStream stream = new MemoryStream(System.Text.ASCIIEncoding.ASCII.GetBytes(datasetXml));
            DataSet DataSet = new DataSet();
            DataSet.ReadXml(stream);

            //Save Data Availability period to be used in the query entry page
            using (var db = new DataContext())
            {
                IList<DataMartAvailabilityPeriod> dmAvailPeriods = new List<DataMartAvailabilityPeriod>();
                foreach (SummaryRequestType qt in requestTypes)
                {
                    DataRow[] dRowCollection = DataSet.Tables[0].Select("QueryTypeId = " + qt.QueryTypeId);

                    StringBuilder sbr = new StringBuilder(string.Empty);
                    if (null != dRowCollection && dRowCollection.Length > 0)
                    {
                        foreach (DataRow dr in dRowCollection)
                        {
                            string period = (dr["Period"] != null) ? dr["Period"].ToString() : string.Empty;
                            string periodCat = period.IndexOf('Q') >= 0 ? "Q" : "Y";

                            if (string.IsNullOrEmpty(period))
                                continue;

                            var dmAvailPeriod = db.DataMartAvailabilityPeriods.Where(p => p.DataMartID == datamart.ID && p.Period == period && p.PeriodCategory == periodCat && p.RequestTypeID == qt.ID).FirstOrDefault();

                            if (dmAvailPeriod == null)
                            {
                                DataMartAvailabilityPeriod l = new DataMartAvailabilityPeriod();
                                l.DataMartID = datamart.ID;
                                l.Active = true;
                                l.Period = period;
                                l.PeriodCategory = periodCat;
                                l.RequestTypeID = qt.ID;
                                dmAvailPeriods.Add(l);
                            }
                            else
                            {
                                if (!dmAvailPeriod.Active)
                                {
                                    dmAvailPeriod.Active = true;
                                    dmAvailPeriods.Add(dmAvailPeriod);
                                }
                            }
                        }
                    }

                }
                db.DataMartAvailabilityPeriods.AddRange(dmAvailPeriods);
                db.SaveChanges();

            }

        }

    }

}
