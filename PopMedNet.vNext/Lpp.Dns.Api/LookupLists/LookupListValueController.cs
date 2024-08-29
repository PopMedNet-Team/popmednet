using Lpp.Dns.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using Lpp.Dns.Data;
using System.Data.Entity;
using Lpp.Utilities.WebSites.Controllers;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Api.LookupLists
{
    /// <summary>
    /// Looks up Values from the lookup lists
    /// </summary>
    public class LookupListValueController : LppApiController<DataContext>
    {
        /// <summary>
        /// Gets a list of LookupList
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<LookupListValueDTO> GetList()
        {
            var result = from d in DataContext.LookupListValues
                         select new LookupListValueDTO
                         {
                             ListId = d.ListId,
                             CategoryId = d.CategoryId,
                             ItemName = d.ItemName,
                             ItemCode = d.ItemCode,
                             ItemCodeWithNoPeriod = d.ItemCodeWithNoPeriod,
                             ExpireDate = d.ExpireDate,
                             ID = d.ID
                         };

            return result;
        }

        /// <summary>
        /// Returns a full detailed list of codes based on the codes passed in
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        [HttpPost]
        public IQueryable<LookupListValueDTO> GetCodeDetailsByCode(LookupListDetailRequestDTO details)
        {
            if (details.Codes == null)
                details.Codes = new List<string>();

            var results = from d in DataContext.LookupListValues
                                 where d.ListId == details.ListID && details.Codes.Contains(d.ItemCode)
                                 select new LookupListValueDTO
                                 {
                                     CategoryId = d.CategoryId,
                                     ID = d.ID,
                                     ItemCode = d.ItemCode,
                                     ItemCodeWithNoPeriod = d.ItemCodeWithNoPeriod,
                                     ItemName = d.ItemName,
                                     ListId = d.ListId,
                                     ExpireDate = d.ExpireDate
                                 };

            return results;
        }

        /// <summary>
        /// Returns a list of values based on the lookup text provided
        /// </summary>
        /// <param name="listID"></param>
        /// <param name="lookup"></param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<LookupListValueDTO> LookupList(Lists listID, string lookup)
      {
            lookup = lookup.Trim();

            var result = from d in DataContext.LookupListValues
                         where d.ListId == listID
                         select d;

            if (lookup.EndsWith("*") && lookup.StartsWith("*")) {
                lookup = lookup.Trim('*');
                result = result.Where(r => r.ItemCode.Contains(lookup) || r.ItemName.Contains(lookup));
            } else if (lookup.StartsWith("*")) {
                lookup = lookup.Trim('*');
                result = result.Where(r => r.ItemCode.EndsWith(lookup) || r.ItemName.EndsWith(lookup));
            } else if (lookup.EndsWith("*")) {
                lookup = lookup.Trim('*');
                result = result.Where(r => r.ItemCode.StartsWith(lookup) || r.ItemName.StartsWith(lookup));
            } else {
                result = result.Where(r => r.ItemCode.StartsWith(lookup) || r.ItemCode.EndsWith(lookup) || r.ItemCode.Contains(lookup) || r.ItemName.StartsWith(lookup) || r.ItemName.EndsWith(lookup) || r.ItemName.Contains(lookup));
            }

            return result.Select(d => new LookupListValueDTO
                         {
                             ListId = d.ListId,
                             CategoryId = d.CategoryId,
                             ItemName = d.ItemName,
                             ItemCode = d.ItemCode,
                             ItemCodeWithNoPeriod = d.ItemCodeWithNoPeriod,
                             ExpireDate = d.ExpireDate,
                             ID = d.ID
                         });
        }

        /// <summary>
        /// Gets a list of LookupList Values for the List Type.
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <param name="ListId"></param>
        /// <returns></returns>
        //[HttpGet]
        //public IQueryable<LookupListValueDTO> Get(int CategoryId, int? ListId)
        //{
        //    var result = from d in DataContext.LookupListValues
        //                 where CategoryId == d.CategoryId
        //                 select new LookupListValueDTO
        //                 {
        //                     ListId = d.ListId,
        //                     CategoryId = d.CategoryId,
        //                     ItemName = d.ItemName,
        //                     ItemCode = d.ItemCode,
        //                     ItemCodeWithNoPeriod = d.ItemCodeWithNoPeriod,
        //                     ID = d.ID
        //                 };

        //    return result;
        //}       
    
    }
}
