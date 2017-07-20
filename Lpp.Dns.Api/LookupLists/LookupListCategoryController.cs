using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lpp.Utilities.WebSites.Controllers;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.Api.LookupLists
{
    /// <summary>
    /// Responsible for all interactions and management of LookupListCategories
    /// </summary>
    public class LookupListCategoryController : LppApiController<DataContext>
    {
        /// <summary>
        /// Gets a list of LookupList
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<LookupListCategoryDTO> GetList(Lists listID)
        {
            var result = from d in DataContext.LookupListCategories where d.ListId == listID
                         select new LookupListCategoryDTO
                         {
                             ListId = d.ListId,
                             CategoryId = d.CategoryId,
                             CategoryName = d.CategoryName,
                         };

            return result;
        }


        
        //[HttpGet]
        //public IQueryable<LookupListCategoryDTO> Get(int ListId)
        //{
        //    var result = from d in DataContext.LookupListCategories
        //                 where ListId == d.ListId
        //                 select new LookupListCategoryDTO
        //                 {
        //                     ListId = d.ListId,
        //                     CategoryId = d.CategoryId,
        //                     CategoryName = d.CategoryName,
        //                 };

        //    return result;
        //}
    }
}
