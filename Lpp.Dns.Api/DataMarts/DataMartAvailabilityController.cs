using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Security;
using Lpp.Utilities.WebSites.Controllers;
using System.Linq;
using System.Web.Http;

namespace Lpp.Dns.Api.DataMarts
{
    /// <summary>
    /// Controller that services requests related to DataMart Availability Periods.
    /// </summary>
    public class DataMartAvailabilityController : LppApiController<DataContext>
    {
        static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns a list of DataMart Availability Periods the user has access to that are filterable using OData
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<DataMartAvailabilityPeriodV2DTO> List()
        {
            return DataContext.GetDataMartsAvailability(Identity.ID);
        }
    }
}
