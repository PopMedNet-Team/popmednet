using Lpp.Dns.Portal.Root.Areas.DataChecker.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using System;

namespace Lpp.Dns.Portal.Root.Areas.DataChecker.Controllers
{
    public class RxSupController : BaseController
    {
        public override JsonResult ProcessMetrics(Guid documentId)
        {
            var ds = LoadResults(documentId);
            IEnumerable<RxSupItemData> rawResults = (from x in ds.Tables[0].AsEnumerable()
                                                        select new RxSupItemData
                                                        {
                                                            DP = x.Field<string>("DP"),
                                                            RxSup = x.Field<string>("RxSup"),
                                                            n = x.Field<double?>("Total")
                                                        }).ToArray();
            
            var dataPartners = rawResults.GroupBy(g => g.DP).Select(r => r.Key).OrderBy(dp => dp).ToArray();
            var codes = rawResults.GroupBy(g => g.RxSup).Select(r => r.Key).OrderBy(c => ConverterForRangeSort(c)).ToArray();
            var totalCount = rawResults.Sum(x => x.n ?? 0D);

            var overallMetrics = rawResults.GroupBy(r => r.RxSup).Select((k) =>
                    new { 
                        RxSup = k.Key,
                        RxSup_Display = FormatRxSup(k.Key),
                        n = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D,
                        Percent = Math.Round(((k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D) / totalCount) * 100, 2, MidpointRounding.AwayFromZero)
                    }
                ).OrderBy(x => ConverterForRangeSort(x.RxSup)).ToArray(); 

            var codesByPartner = rawResults.GroupBy(g => g.DP)
                                .Select(k => new
                                {
                                    Partner = k.Key,
                                    Total = rawResults.Where(x => x.DP == k.Key).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                    Codes = codes.Select(rx => new { 
                                        Code = rx,
                                        Code_Display = FormatRxSup(rx),
                                        Count = k.Where(x => x.RxSup == rx).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                    }).OrderBy(x => ConverterForRangeSort(x.Code))
                                }).OrderBy(p => p.Partner).ToArray();

            var partnersByCode = rawResults.GroupBy(g => g.RxSup)
                                          .Select(k => new
                                          {
                                              RxSup = k.Key,
                                              RxSup_Display = FormatRxSup(k.Key), 
                                              Total = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                              Partners = dataPartners.Select(dp => new { 
                                                Partner = dp,
                                                Total = rawResults.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                                Count = k.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                              }).OrderBy(dp => dp.Partner)
                                          }).OrderBy(x => ConverterForRangeSort(x.RxSup)).ToArray();            

            return Json(new { DataPartners = dataPartners, OverallMetrics = overallMetrics, CodesByPartner = codesByPartner, PartnersByCode = partnersByCode }, JsonRequestBehavior.AllowGet);
        }

        static string FormatRxSup(string value) {
            switch (value) {
                case "-1":
                    return "< 0";
                case "0":
                    return "0-1";
                case "2":
                    return "2-30";
                case "30":
                    return "31-60";
                case "60":
                    return "61-90";
                case "90":
                    return ">90";
                case "OTHER":
                    return "Other RxSup";
            }

            return "Missing";
        }
    }
}