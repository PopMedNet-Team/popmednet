using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Data;
using System.Xml;
using System.Xml.XPath;

#pragma warning disable 618 //XmlDataDocument is obsolete
namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters
{
    /// <summary>
    /// Utility class for expanding Summary Query results using all combinations of Period, AgeGroup, Sex, Setting, Drug/Code.
    /// </summary>
    public class SummaryQueryUtil
    {
        //internal const string LOW_CELL_COUNT_COLUMNS = "dispensings members dayssupply events episodespans days";
        internal static readonly string[] LOW_CELL_COUNT_COLUMNS = { "dispensings", "members", "dayssupply", "events", "episodespans", "days", "total" };


        public static bool IsCheckedColumn(string columnName)
        {
            string lcColumnName = columnName.ToLower();

            return (from c in LOW_CELL_COUNT_COLUMNS
                    where lcColumnName.IndexOf(c) >= 0
                    select c).ToList().Count() > 0;
        }

        /// <summary>
        /// Returns other columns in the table that are calculated based on the specified column.
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static List<string> GetDependentComputedColumns(string colName, Dictionary<string, object> row)
        {
            List<string> columns = new List<string>();

            switch (colName.ToLower())
            {
                case "events":
                    foreach (string dc in row.Keys)
                    {
                        if(dc.ToLower().Contains("event rate"))
                            columns.Add(dc);
                    }
                    foreach (string dc in row.Keys)
                    {
                        if (dc.ToLower().Contains("events per member"))
                            columns.Add(dc);
                    }
                    break;
                case "members":
                    foreach (string dc in row.Keys)
                    {
                        if (dc.ToLower().Contains("user rate"))
                            columns.Add(dc);
                    }
                    foreach (string dc in row.Keys)
                    {
                        if (dc.ToLower().Contains("prevalence rate"))
                            columns.Add(dc);
                    }
                    foreach (string dc in row.Keys)
                    {
                        if (dc.ToLower().Contains("incidence rate"))
                            columns.Add(dc);
                    }
                    foreach (string dc in row.Keys)
                    {
                        if (dc.ToLower().Contains("days per user"))
                            columns.Add(dc);
                    }
                    foreach (string dc in row.Keys)
                    {
                        if (dc.ToLower().Contains("events per member"))
                            columns.Add(dc);
                    }
                    break;
                case "dayssupply":
                    foreach (string dc in row.Keys)
                    {
                        if (dc.ToLower().Contains("dayssupply rate"))
                            columns.Add(dc);
                    }
                    foreach (string dc in row.Keys)
                    {
                        if (dc.ToLower().Contains("days per user"))
                            columns.Add(dc);
                    }
                    foreach (string dc in row.Keys)
                    {
                        if (dc.ToLower().Contains("days per dispensing"))
                            columns.Add(dc);
                    }
                    break;
                case "dispensings":
                    foreach (string dc in row.Keys)
                    {
                        if (dc.ToLower().Contains("dispensing rate"))
                            columns.Add(dc);
                    }
                    foreach (string dc in row.Keys)
                    {
                        if (dc.ToLower().Contains("days per dispensing"))
                            columns.Add(dc);
                    }
                    break;
            }

            return columns;
        }
    }
}
