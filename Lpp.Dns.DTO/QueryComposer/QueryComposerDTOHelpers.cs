using System;
using System.Collections.Generic;
using System.Linq;

namespace Lpp.Dns.DTO.QueryComposer
{
    public static class QueryComposerDTOHelpers
    {
        /// <summary>
        /// Gets the JObject representing the term values from the specified QueryComposerTermDTO.
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public static Newtonsoft.Json.Linq.JObject GetValuesJObject(this DTO.QueryComposer.QueryComposerTermDTO term)
        {
            return term.Values["Values"] as Newtonsoft.Json.Linq.JObject;
        }

        /// <summary>
        /// Gets the JToken from the QueryComposerTermDTO for the specified key.
        /// </summary>
        /// <param name="term"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Newtonsoft.Json.Linq.JToken GetValue(this DTO.QueryComposer.QueryComposerTermDTO term, string key)
        {
            return ((Newtonsoft.Json.Linq.JObject)term.Values["Values"]).GetValue(key);
        }

        /// <summary>
        /// Gets the value as a string from the QueryComposerTermDTO for the specified key.
        /// </summary>
        /// <param name="term"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetStringValue(this DTO.QueryComposer.QueryComposerTermDTO term, string key)
        {
            return (string)term.GetValue(key);
        }

        /// <summary>
        /// Gets the value as an enum from the QueryComposerTermDTO for the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="term"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetEnumValue<T>(this DTO.QueryComposer.QueryComposerTermDTO term, string key, out T value) where T : struct, IConvertible
        {
            if (term == null)
            {
                value = default(T);
                return false;
            }

            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enum type.");
            }

            return Enum.TryParse<T>(term.GetStringValue(key), out value);
        }

        /// <summary>
        /// Returns the content of the specified key as an IEnumerable&gt;string>.
        /// </summary>
        /// <param name="term"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetStringCollection(this DTO.QueryComposer.QueryComposerTermDTO term, string key)
        {
            var token = term.GetValue(key);
            return token.Values<string>();
        }

        public static IEnumerable<string> GetCodeStringCollection(this DTO.QueryComposer.QueryComposerTermDTO term)
        {
            if (term.GetValue("CodeValues") == null)
            {
                //legacy stuff
                var token = term.GetValue("Codes");
                return token.Values<string>();
            }
            else
            {
                var token = term.GetValue("CodeValues");
                List<Dns.DTO.CodeSelectorValueDTO> dto = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dns.DTO.CodeSelectorValueDTO>>(token.ToString());
                return dto.Select(p => p.Code);
            }
        }

        public static IEnumerable<string> GetCodeNameStringCollection(this DTO.QueryComposer.QueryComposerTermDTO term)
        {
            if (term.GetValue("CodeValues") == null)
            {
                //legacy stuff
                var token = term.GetValue("Codes");
                return token.Values<string>();
            }
            else
            {
                var token = term.GetValue("CodeValues");
                List<Dns.DTO.CodeSelectorValueDTO> dto = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dns.DTO.CodeSelectorValueDTO>>(token.ToString());
                return dto.Select(p => p.Name);
            }
        }

        /// <summary>
        /// Parses the "CodeValues" value of the term into a collection of Dns.DTO.CodeSelectorValueDTO.
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public static IEnumerable<Dns.DTO.CodeSelectorValueDTO> GetCodeSelectorValues(this DTO.QueryComposer.QueryComposerTermDTO term)
        {
            var token = term.GetValue("CodeValues");
            if (token == null)
                return Enumerable.Empty<Dns.DTO.CodeSelectorValueDTO>();

            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dns.DTO.CodeSelectorValueDTO>>(token.ToString());
        }

        /// <summary>
        /// Returns all terms from all criteria defined in all queries for the request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IEnumerable<DTO.QueryComposer.QueryComposerTermDTO> FlattenToTerms(this DTO.QueryComposer.QueryComposerRequestDTO request)
        {
            foreach (var criteria in request.Queries.SelectMany(q => q.Where.Criteria))
            {
                //foreach root critiera in each query
                foreach (var term in FlattenCriteriaToTerms(criteria).Where(t => t != null))
                    yield return term;
            }
        }

        /// <summary>
        /// Returns all the fields from all the Selects defined in all queries for the request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IEnumerable<QueryComposerFieldDTO> FlattenStratifiers(this QueryComposerRequestDTO request)
        {
            foreach(var select in request.Queries.Select(q => q.Select))
            {
                foreach (var field in FlattenSelectToFields(select).Where(f => f != null))
                    yield return field;
            }
        }

        /// <summary>
        /// Returns all terms from all criteria defined in the query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IEnumerable<DTO.QueryComposer.QueryComposerTermDTO> FlattenToTerms(this DTO.QueryComposer.QueryComposerQueryDTO query)
        {
            foreach (var criteria in query.Where.Criteria)
            {
                //foreach root critiera in each query
                foreach (var term in FlattenCriteriaToTerms(criteria).Where(t => t != null))
                    yield return term;
            }
        }

        /// <summary>
        /// Returns all terms contained within the specified criteria and all it's sub-criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static IEnumerable<DTO.QueryComposer.QueryComposerTermDTO> FlattenCriteriaToTerms(this DTO.QueryComposer.QueryComposerCriteriaDTO criteria)
        {
            if (criteria.Criteria != null)
            {
                foreach (var subCriteria in criteria.Criteria)
                {
                    foreach (var t in FlattenCriteriaToTerms(subCriteria).Where(t => t != null))
                        yield return t;
                }
            }

            foreach (var term in Enumerable.DefaultIfEmpty(criteria.Terms).Where(t => t != null))
            {
                yield return term;
            }
        }

        /// <summary>
        /// Returns all the fields contained within the specified select.
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        public static IEnumerable<QueryComposerFieldDTO> FlattenSelectToFields(this QueryComposerSelectDTO select)
        {
            foreach(var field in Enumerable.DefaultIfEmpty(select.Fields).Where(f => f != null))
            {
                yield return field;
            }
        }

        /// <summary>
        /// Deserializes a request, it will convert a previous schema version to the current RequestDTO.
        /// </summary>
        /// <param name="json">The json to deserialize to a <see cref="DTO.QueryComposer.QueryComposerRequestDTO"/>.</param>
        /// <returns></returns>
        public static DTO.QueryComposer.QueryComposerRequestDTO DeserializeRequest(string json)
        {
            Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.Linq.JObject.Parse(json.Trim());

            var serializationSettings = new Newtonsoft.Json.JsonSerializerSettings();
            serializationSettings.Converters.Add(new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionConverter());
            serializationSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate;
            Newtonsoft.Json.JsonSerializer serializer = Newtonsoft.Json.JsonSerializer.Create(serializationSettings);

            if (obj.ContainsKey("SchemaVersion"))
            {
                return obj.ToObject<DTO.QueryComposer.QueryComposerRequestDTO>(serializer);
            }

            var query = obj.ToObject<DTO.QueryComposer.QueryComposerQueryDTO>(serializer);

            var requestDTO = new DTO.QueryComposer.QueryComposerRequestDTO
            {
                Header = new DTO.QueryComposer.QueryComposerRequestHeaderDTO
                {
                    ID = query.Header.ID,
                    Name = query.Header.Name,
                    Description = query.Header.Description,
                    DueDate = query.Header.DueDate,
                    Priority = query.Header.Priority,
                    SubmittedOn = query.Header.SubmittedOn,
                    ViewUrl = query.Header.ViewUrl
                },
                Queries = new[] {
            query
            }
            };

            return requestDTO;
        }

        public static DTO.QueryComposer.QueryComposerResponseDTO DeserializeResponse(string json)
        {
            Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.Linq.JObject.Parse(json.Trim());

            var serializationSettings = new Newtonsoft.Json.JsonSerializerSettings();
            serializationSettings.Converters.Add(new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionConverter());
            serializationSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate;
            Newtonsoft.Json.JsonSerializer serializer = Newtonsoft.Json.JsonSerializer.Create(serializationSettings);

            if (obj.ContainsKey("SchemaVersion"))
            {
                return obj.ToObject<DTO.QueryComposer.QueryComposerResponseDTO>(serializer);
            }

            var queryResponse = obj.ToObject<DTO.QueryComposer.QueryComposerResponseQueryResultDTO>(serializer);

            var response = new DTO.QueryComposer.QueryComposerResponseDTO 
            { 
                Header = new QueryComposerResponseHeaderDTO { 
                    ID = queryResponse.ID,
                },
                Errors = Enumerable.Empty<QueryComposerResponseErrorDTO>(),
                Queries = new[] { queryResponse }
            };

            response.RefreshQueryDates();
            response.RefreshErrors();

            return response;
        }

        /// <summary>
        /// Gets the first result from a response.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static IEnumerable<IDictionary<string, object>> FirstQueryResult(this DTO.QueryComposer.QueryComposerResponseDTO response)
        {
            if (response == null || response.Queries == null)
                return null;

            return response.Queries.SelectMany(q => q.Results).FirstOrDefault();
        }

        /// <summary>
        /// Confirms if the request has a query that contains the specified term in the stratifiers.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="termID">The term ID to look for.</param>
        /// <returns>True if an instance of the term is found, else false.</returns>
        public static bool HasStratifier(this QueryComposerRequestDTO request, Guid termID)
        {
            return request.Queries.SelectMany(q => q.Select.Fields.Where(t => t.Type == termID)).Any();
        }

    }
}