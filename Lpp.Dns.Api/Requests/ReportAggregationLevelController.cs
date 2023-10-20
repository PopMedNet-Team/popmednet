using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Utilities.WebSites.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lpp.Utilities;
using System.Threading.Tasks;
using System.Data.Entity;
using Lpp.Dns.DTO.Security;
using System.Collections.Specialized;
using System.Web;

namespace Lpp.Dns.Api.Requests
{
    public class ReportAggregationLevelController : LppApiDataController<ReportAggregationLevel, ReportAggregationLevelDTO, DataContext, PermissionDefinition>
    {
        /// <summary>
        /// Marks the Report Aggregation Level as deleted by setting it's DeletedOn value to UTC now.
        /// </summary>
        /// <param name="ID">The ID's of the items to mark as deleted.</param>
        /// <returns></returns>
        [HttpDelete]
        public override async Task Delete([FromUri] IEnumerable<Guid> ID)
        {
            //Override hard delete. Instead just update the "DeletedOn" term
            var deletedRAL = DataContext.ReportAggregationLevels.Where(r => ID.Contains(r.ID)).FirstOrDefault();
            if (deletedRAL != null)
            {
                deletedRAL.DeletedOn = DateTime.UtcNow;

                await DataContext.SaveChangesAsync();
            }

        }
    

    }
}