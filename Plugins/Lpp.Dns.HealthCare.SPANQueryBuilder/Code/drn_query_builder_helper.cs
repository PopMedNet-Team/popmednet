using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Lpp.Dns.HealthCare.SPANQueryBuilder.Code
{
    public class drn_query_builder_helper
    {
        private drn_query_builder _qb = null;

        public drn_query_builder_helper()
        {
            _qb = new drn_query_builder()
            {               
                datamart = new [] { new drn_query_builderDatamart() }, 
                exclusion_criteria = new [] { new drn_query_builderExclusion_criteria() },
                inclusion_criteria = new [] { new drn_query_builderInclusion_criteria() },
                index_variable = new [] { new drn_query_builderIndex_variable() },               
                report = new [] { new drn_query_builderReport() },
                version = "0.0.1"
            };
        }

        public void Save(String PathName)
        {
            try
            {
                StreamWriter sw = new StreamWriter(PathName);
                sw.Write(XMLString);
                sw.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Load(String PathName)
        {
            try
            {
                StreamReader sr = new StreamReader(PathName);
                XMLString = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        private Byte[] StringToUTF8ByteArray(String XMLString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(XMLString);
        }

        public Byte[] XMLStringByteArray
        {
            get { return StringToUTF8ByteArray(XMLString); }
        }

        public String XMLString
        {
            get
            {
                try
                {
                    MemoryStream memoryStream = new MemoryStream();
                    XmlSerializer xs = new XmlSerializer(typeof(drn_query_builder), "http://lincolnpeak.com");
                    XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                    xs.Serialize(xmlTextWriter, _qb);
                    memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                    return UTF8ByteArrayToString(memoryStream.ToArray());
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    return string.Empty;
                }
            }
            set
            {
                try
                {
                    MemoryStream ms = new MemoryStream(StringToUTF8ByteArray(value));
                    XmlSerializer xs = new XmlSerializer(typeof(drn_query_builder), "http://lincolnpeak.com");
                    _qb = (drn_query_builder)xs.Deserialize(ms);
                }
                catch (Exception e)
                {
                    _qb = null;
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
        }

        public string QueryType
        {
            get { return _qb.query_type; }
            set { _qb.query_type = value; }
        }

        public String QueryName
        {
            get { return _qb.query_name; }
            set { _qb.query_name = value; }
        }

        public String QueryDescription
        {
            get { return _qb.query_description; }
            set { _qb.query_description = value; }
        }

        public String Version
        {
            get { return _qb.version; }
            set { _qb.version = value; }
        }

        public String SubmitterEmail
        {
            get { return _qb.submitter_email; }
            set { _qb.submitter_email = value; }
        }

        public String PeriodStart
        {
            get { return _qb.period_start; }
            set { _qb.period_start = value; }
        }

        public String PeriodEnd
        {
            get { return _qb.period_end; }
            set { _qb.period_end = value; }
        }

        public String EnrollmentPrior
        {
            get { return _qb.enroll_prior; }
            set { _qb.enroll_prior = value; }
        }

        public String EnrollmentPost
        {
            get { return _qb.enroll_post; }
            set { _qb.enroll_post = value; }
        }

        public String ContinuousEnrollment
        {
            get { return _qb.enroll_cont; }
            set { _qb.enroll_cont = value; }
        }

        public drn_query_builderIndex_variable IndexVariable
        {
            get { return _qb.index_variable[0]; }
            set { _qb.index_variable = new drn_query_builderIndex_variable[1]; _qb.index_variable[0] = value; }
        }

        public drn_query_builderInclusion_criteria InclusionCriteria
        {
            get { return _qb.inclusion_criteria[0]; }
            set { _qb.inclusion_criteria = new drn_query_builderInclusion_criteria[1]; _qb.inclusion_criteria[0] = value; }
        }

        public drn_query_builderExclusion_criteria ExclusionCriteria
        {
            get { return _qb.exclusion_criteria[0]; }
            set { _qb.exclusion_criteria = new drn_query_builderExclusion_criteria[1]; _qb.exclusion_criteria[0] = value; }
        }

        public drn_query_builderReport[] Reports
        {
            get { return _qb.report; }
            set { _qb.report = value; }
        }

        public drn_query_builderDatamart[] DataMarts
        {
            get { return _qb.datamart; }
            set { _qb.datamart = value; }
        }
    }
}