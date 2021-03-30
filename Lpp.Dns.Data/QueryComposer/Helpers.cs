using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.QueryComposer
{
    public static class Helpers
    {
        /// <summary>
        /// Parses the query json of the request and up-converts to the multi-query structure if required.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static DTO.QueryComposer.QueryComposerRequestDTO ParseRequestJSON(Request request)
        {
            return ParseRequestJSON(request.ID, request.Name, request.DueDate, request.Priority, request.Query);
        }
        /// <summary>
        /// Parses the query json of a request and up-converts to the multi-query structure if required.
        /// </summary>
        /// <param name="requestID"></param>
        /// <param name="requestName"></param>
        /// <param name="dueDate"></param>
        /// <param name="priority"></param>
        /// <param name="queryJSON"></param>
        /// <returns></returns>
        public static DTO.QueryComposer.QueryComposerRequestDTO ParseRequestJSON(Guid requestID, string requestName, DateTime? dueDate,Dns.DTO.Enums.Priorities priority, string queryJSON)
        {
            if (string.IsNullOrWhiteSpace(queryJSON))
                return null;

            var jsonObj = Newtonsoft.Json.Linq.JObject.Parse(queryJSON);

            if (jsonObj.ContainsKey("SchemaVersion"))
            {
                return jsonObj.ToObject<DTO.QueryComposer.QueryComposerRequestDTO>();
            }

            var requestDTO = new DTO.QueryComposer.QueryComposerRequestDTO
            {
                Header = new DTO.QueryComposer.QueryComposerRequestHeaderDTO
                {
                    ID = requestID,
                    Name = requestName,
                    DueDate = dueDate,
                    Priority = priority
                },
                Queries = new[] { jsonObj.ToObject<DTO.QueryComposer.QueryComposerQueryDTO>() }
            };

            return requestDTO;
        }

        /// <summary>
        /// Determines if the term exists in any query within the request.
        /// </summary>
        /// <param name="termTypeID">The ID of the term type.</param>
        /// <param name="requestDTO">The QueryComposer request dto.</param>
        /// <returns></returns>
        public static bool HasTermInAnyCriteria(Guid termTypeID, DTO.QueryComposer.QueryComposerRequestDTO requestDTO)
        {
            var terms = GetAllTerms(termTypeID, requestDTO);
            return terms.Any();
        }
        /// <summary>
        /// Returns all terms used within request of the specified type.
        /// </summary>
        /// <param name="termTypeID"></param>
        /// <param name="requestDTO"></param>
        /// <returns></returns>
        public static IEnumerable<DTO.QueryComposer.QueryComposerTermDTO> GetAllTerms(Guid termTypeID, DTO.QueryComposer.QueryComposerRequestDTO requestDTO)
        {
            return GetAllTerms(termTypeID, requestDTO.Queries.SelectMany(q => q.Where.Criteria));
        }

        /// <summary>
        /// Returns all terms of the specified type within the criteria.
        /// </summary>
        /// <param name="termTypeID"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static IEnumerable<DTO.QueryComposer.QueryComposerTermDTO> GetAllTerms(Guid termTypeID, IEnumerable<DTO.QueryComposer.QueryComposerCriteriaDTO> criteria)
        {
            foreach (var term in criteria.SelectMany(c => c.Terms).Where(t => t.Type == termTypeID))
            {
                yield return term;
            }

            foreach (var subCriteria in criteria.SelectMany(c => c.Criteria))
            {
                foreach (var term in GetAllTerms(termTypeID, new[] { subCriteria }))
                {
                    yield return term;
                }
            }
        }
    }
}
