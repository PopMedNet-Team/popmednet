using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace RequestCriteria.Models
{
    public class RequestCriteriaHelper
    {
        public static RequestCriteriaData ToServerModel(string json)
        {
            return JsonConvert.DeserializeObject<RequestCriteriaData>(json, new TermCreationConverter());
        }

        public static string ToClientModel(RequestCriteriaData data)
        {
            return JsonConvert.SerializeObject(data);
        }

        // Doesn't seem to work, I think this is because of the interface lists
        //
        //public static string ToXML(RequestCriteriaData data)
        //{
        //    string xml = string.Empty;
        //    XmlSerializer xs = new XmlSerializer(typeof(RequestCriteriaData));
        //    using (StringWriter sw = new StringWriter())
        //    {
        //        using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings { OmitXmlDeclaration = true }))
        //        {
        //            xs.Serialize(xmlWriter, data, null);
        //            xml = sw.ToString();
        //        }
        //    }

        //    return xml;
        //}
    }

    public class TermCreationConverter : JsonCreationConverter<ITermData>
    {
        protected override ITermData Create(Type objectType, JObject jObject)
        {
            TermTypes termtype;
            if (Enum.TryParse<TermTypes>(jObject["TermType"].ToString(), out termtype))
            {
                switch (termtype)
                {
                    case TermTypes.AgeRangeTerm:
                        return new AgeRangeData();

                    case TermTypes.AgeStratifierTerm:
                        return new AgeStratifierData();

                    case TermTypes.ClinicalSettingTerm:
                        return new ClinicalSettingData();

                    case TermTypes.CodesTerm:
                        return new CodesData();

                    case TermTypes.DataPartnerTerm:
                        return new DataPartnersData();

                    case TermTypes.DateRangeTerm:
                        return new DateRangeData();

                    case TermTypes.EthnicityTerm:
                        return new EthnicityData();

                    case TermTypes.MetricTerm:
                        return new MetricsData();

                    case TermTypes.ProjectTerm:
                        return new ProjectData();

                    case TermTypes.RaceTerm:
                        return new RaceData();

                    case TermTypes.RequestStatusTerm:
                        return new RequestStatusData();

                    case TermTypes.SexTerm:
                        return new SexData();

                    case TermTypes.WorkplanTypeTerm:
                        return new WorkplanTypeData();

                    case TermTypes.ReportAggregationLevelTerm:
                        return new ReportAggregationLevelData();

                    case TermTypes.RequesterCenterTerm:
                        return new RequesterCenterData();
                    case TermTypes.EncounterTypeTerm:
                        return new EncounterData();
                    case TermTypes.MetaDataTableTerm:
                        return new MetaDataTableData();
                    case TermTypes.PDXTerm:
                        return new PDXData();
                    case TermTypes.RxAmtTerm:
                        return new RxAmountData();
                    case TermTypes.RxSupTerm:
                        return new RxSupData();
                }
            }

            throw new Exception("JSON Term Converter: unable to map term type");
        }
    }

    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        /// <summary
        /// Create an instance of objectType, based properties in the JSON object
        /// </summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">contents of JSON object that will be deserialized</param>
        /// <returns></returns>
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            T target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
