using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Dns.DTO.Enums;
using Lpp.Utilities.Legacy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Lpp.Dns.Portal.Root.Areas.Api.Controllers
{
    public class DemographicsController : Controller
    {
        [Import]
        public IDemographicsService Demographics { get; set; }
        
        //This is using MVC right now because we can't upgrade to use Web API.

        [HttpGet]
        public JsonResult GetRegionsAndTowns(string country, string state)
        {
            using (var db = new DataContext())
            {
                var results = new
                {
                    Regions = (from d in db.Demographics where d.Country == country && d.State == state && d.Region != null && d.Region != "" select d.Region).Distinct().OrderBy(d => d).ToArray(),
                    Towns = (from d in db.Demographics where d.Country == country && d.State == state && d.Town != null && d.Town != "" select d.Town).Distinct().OrderBy(d => d).ToArray()
                };

                return Json(results, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetCensusDataByState(string country, string state, Stratifications stratification)
        {
            IEnumerable results = Demographics.GetCensusDataByState(country, state, stratification);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCensusDataByRegion(string country, string state, string region, Stratifications stratification)
        {
            IEnumerable results = Demographics.GetCensusDataByRegion(country, state, region, stratification);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCensusDataByTown(string country, string state, string town, Stratifications stratification)
        {
            IEnumerable results = Demographics.GetCensusDataByTown(country, state, town, stratification);
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets census results stratified by the specified stratifications and aggregated by the specified locations (groups of zips).
        /// </summary>
        /// <param name="locations">A collection of zipcode groups representing a location.</param>
        /// <param name="stratification"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetCensusDataByZCTA(string locations, Stratifications stratification)
        {
            IEnumerable<IEnumerable<string>> locationZipCodes = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<IEnumerable<string>>>(locations);
            //List<IEnumerable<ZCTACensusData>> results = new List<IEnumerable<ZCTACensusData>>();
            ArrayList results = new ArrayList();
            foreach (var location in locationZipCodes)
            {
                if (!location.Any())
                    continue;

                var locationResults = Demographics.GetCensusDataByZip(location, stratification);
                results.Add(new { LocationKey = string.Join(",", location), Results = (locationResults ?? Enumerable.Empty<ZCTACensusData>()) });
            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetGeographicLocationStates()
        {
            using (var db = new DataContext())
            {
                var query = db.GeographicLocations.AsNoTracking().Select(g => new { g.State, g.StateAbbrev }).Distinct().OrderBy(g => g.State);
                var states = await query.ToArrayAsync();

                return Json(states, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> QueryGeographicLocations(string lookup)
        {
            lookup = lookup.Trim();
            using (var db = new DataContext())
            {
                var query = db.GeographicLocations.AsNoTracking().AsQueryable();

                if (lookup.EndsWith("*") && lookup.StartsWith("*"))
                {
                    lookup = lookup.Trim('*');

                    query = query.Where(g => g.Location.Contains(lookup) || g.PostalCode.Contains(lookup) || g.State.Contains(lookup) || g.StateAbbrev.Contains(lookup));
                }
                else if (lookup.StartsWith("*"))
                {
                    lookup = lookup.Trim('*');
                    query = query.Where(g => g.Location.EndsWith(lookup) || g.PostalCode.EndsWith(lookup) || g.State.EndsWith(lookup) || g.StateAbbrev.EndsWith(lookup));
                }
                else if (lookup.EndsWith("*"))
                {
                    lookup = lookup.Trim('*');
                    query = query.Where(g => g.Location.StartsWith(lookup) || g.PostalCode.StartsWith(lookup) || g.State.StartsWith(lookup) || g.StateAbbrev.StartsWith(lookup));
                }
                else
                {
                    query = query.Where(g =>
                            g.Location.Contains(lookup) || g.PostalCode.Contains(lookup) || g.State.Contains(lookup) || g.StateAbbrev.Contains(lookup) ||
                            g.Location.EndsWith(lookup) || g.PostalCode.EndsWith(lookup) || g.State.EndsWith(lookup) || g.StateAbbrev.EndsWith(lookup) ||
                            g.Location.StartsWith(lookup) || g.PostalCode.StartsWith(lookup) || g.State.StartsWith(lookup) || g.StateAbbrev.StartsWith(lookup)
                        );
                }

                var result = await query.GroupBy(g => new { g.Location, g.StateAbbrev })
                                  .Select(g => new { Location = g.Key.Location, StateAbbrev = g.Key.StateAbbrev, PostalCodes = g.Select(k => k.PostalCode) })
                                  .OrderBy(g => g.StateAbbrev).ThenBy(g => g.Location)
                                  .ToArrayAsync();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetGeographicLocationsByState(string stateAbbrev)
        {
            using (var db = new DataContext())
            {
                var query = db.GeographicLocations.AsNoTracking()
                    .Where(g => g.StateAbbrev == stateAbbrev)
                    .GroupBy(g => new { g.Location, g.StateAbbrev })
                    .Select(g => new { Location = g.Key.Location, StateAbbrev = g.Key.StateAbbrev, PostalCodes = g.Select(k => k.PostalCode) })
                                  .OrderBy(g => g.Location);

                var result = await query.ToArrayAsync();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
