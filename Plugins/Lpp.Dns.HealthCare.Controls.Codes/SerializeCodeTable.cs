using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Odbc;
namespace Lpp.Dns.HealthCare.Controls.Code.Serializer
{
    public class SerializeCodeTable
    {
        public enum CodeTable
        {
            DX_ICD9_3Digit,
            DX_ICD9_4Digit,
            DX_ICD9_5Digit,
            PX_ICD9_3Digit,
            PX_ICD9_4Digit,
            HCPCS,
            GENERIC_NAME,
            DRUG_CLASS,
            DRUG_CODE
        }

        /// <summary>
        /// Serializes a table containing healthcare codes to an XML file
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="XMLPath"></param>
        /// <returns>number of entries written to the xml file</returns>
        public void Serialize(CodeTable table, String xmlPath)
        {
            OdbcConnection cn = new OdbcConnection(string.Format("DSN={0}", Properties.Settings.Default.DataSourceName));
            try
            {
                OdbcDataAdapter da = new OdbcDataAdapter("SELECT * FROM " + Table(table) + " as CODES", cn);
                DataSet objDataSet = new DataSet("CODES");
                da.Fill(objDataSet);
                DataTable t = objDataSet.Tables[0];
                t.TableName = "CODE_ENTRY";

                foreach (DataColumn dc in t.Columns)
                {
                    dc.ColumnMapping = MappingType.Element;
                    switch (dc.ColumnName)
                    {
                        case "code":
                            dc.ColumnName = "CODE";
                            break;
                        case "dcode":
                            dc.ColumnName = "CODE_WITH_DECIMAL";
                            break;
                        case "srt_descrip":
                        case "drugclass":
                        case "genericname":
                            dc.ColumnName = "DESCRIPTION";
                            break;
                        case "lng_descrip":
                            dc.ColumnName = "DESCRIPTION_LONG";
                            break;
                        default:
                            break;

                    }
                }
                if (t.Columns["CODE"] == null)
                    t.Columns.Add(new DataColumn("CODE", typeof(string), "'UNDEFINED'", MappingType.Element));
                if (t.Columns["CODE_WITH_DECIMAL"] == null)
                    t.Columns.Add(new DataColumn("CODE_WITH_DECIMAL", typeof(string), "'UNDEFINED'", MappingType.Element));
                if (t.Columns["DESCRIPTION"] == null)
                    t.Columns.Add(new DataColumn("DESCRIPTION", typeof(string), "'UNDEFINED'", MappingType.Element));
                if (t.Columns["DESCRIPTION_LONG"] == null)
                    t.Columns.Add(new DataColumn("DESCRIPTION_LONG", typeof(string), "'UNDEFINED'", MappingType.Element));

                objDataSet.WriteXml(xmlPath, XmlWriteMode.IgnoreSchema);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                if (cn != null)
                    cn.Close();
            }
            return;
        }
    }
}
