using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Utilities.WebSites
{
    public class ODataQueryHandler<T>
    {
        readonly IQueryable<T> _query;
        readonly ODataQueryOptions<T> _options;

        public ODataQueryHandler(IQueryable<T> query, ODataQueryOptions<T> options)
        {
            _query = query;
            _options = options;
        }

        public Models.ODataQueryResult Result()
        {
            long? count = null;
            IQueryable result;

            if (_options != null)
            {
                result = _options.ApplyTo(_query);
                if (_options.Count != null)
                    count = _options.Count.GetEntityCount(_options.Filter != null ? _options.Filter.ApplyTo(_query, new ODataQuerySettings()) : _query);
            }
            else
            {
                count = _query.Count();
                result = _query;
            }

            return new Models.ODataQueryResult(result, count);
        }
    }
}
