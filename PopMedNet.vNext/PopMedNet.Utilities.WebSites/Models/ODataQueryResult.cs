using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Utilities.WebSites.Models
{
    public class ODataQueryResult
    {
        public ODataQueryResult(IQueryable results, long? count = null)
        {
            Results = results;
            Count = count;
        }

        public long? Count { get; private set; }
        public IQueryable Results { get; private set; }
        
    }
}
