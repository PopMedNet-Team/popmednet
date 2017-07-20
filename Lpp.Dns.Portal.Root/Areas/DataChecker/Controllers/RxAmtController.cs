using Lpp.Dns.Portal.Root.Areas.DataChecker.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using System;

namespace Lpp.Dns.Portal.Root.Areas.DataChecker.Controllers
{
    public class RxAmtController : BaseController
    {
        public override JsonResult ProcessMetrics(Guid documentId)
        {
            var ds = LoadResults(documentId);
            IEnumerable<RxAmtItemData> rawResults = (from x in ds.Tables[0].AsEnumerable()
                                                        select new RxAmtItemData
                                                        {
                                                            DP = x.Field<string>("DP"),
                                                            RxAmt = x.Field<string>("RxAmt"),
                                                            n = x.Field<double?>("Total")
                                                        }).ToArray();

            var dataPartners = rawResults.GroupBy(g => g.DP).Select(r => r.Key).OrderBy(dp => dp).ToArray();
            var codes = rawResults.GroupBy(g => g.RxAmt).Select(r => r.Key).OrderBy(c => ConverterForRangeSort(c)).ToArray();
            var totalCount = rawResults.Sum(x => x.n ?? 0D);

            var overallMetrics = rawResults.GroupBy(r => r.RxAmt).Select((k) =>
                    new
                    {
                        RxAmt = k.Key,
                        RxAmt_Display = FormatRxAmt(k.Key),
                        n = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D,
                        Percent = Math.Round(((k.Sum(x => (double?)(x.n ?? 0d)) ?? 0D) / totalCount) * 100, 2, MidpointRounding.AwayFromZero)
                    }
                ).OrderBy(x => ConverterForRangeSort(x.RxAmt)).ToArray();

            var codesByPartner = rawResults.GroupBy(g => g.DP)
                                .Select(k => new
                                {
                                    Partner = k.Key,
                                    Total = rawResults.Where(x => x.DP == k.Key).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                    Codes = codes.Select(rx => new
                                    {
                                        Code = rx,
                                        Code_Display = FormatRxAmt(rx),
                                        Count = k.Where(x => x.RxAmt == rx).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                    }).OrderBy(x => ConverterForRangeSort(x.Code))
                                }).OrderBy(x => x.Partner).ToArray();

            var partnersByCode = rawResults.GroupBy(g => g.RxAmt)
                                          .Select(k => new
                                          {
                                              RxAmt = k.Key,
                                              RxAmt_Display = FormatRxAmt(k.Key),
                                              Total = k.Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                              Partners = dataPartners.Select(dp => new
                                              {
                                                  Partner = dp,
                                                  Total = rawResults.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d,
                                                  Count = k.Where(x => x.DP == dp).Sum(x => (double?)(x.n ?? 0d)) ?? 0d
                                              }).OrderBy(x => x.Partner)
                                          }).OrderBy(x => ConverterForRangeSort(x.RxAmt)).ToArray();

            return Json(new { DataPartners = dataPartners, OverallMetrics = overallMetrics, CodesByPartner = codesByPartner, PartnersByCode = partnersByCode }, JsonRequestBehavior.AllowGet);
        }

        public static string FormatRxAmt(string value) {
            switch (value) {
                case "-1":
                    return "< 0";
                case "0":
                    return "0-1";
                case "30":
                    return "2-30";
                case "60":
                    return "31-60";
                case "90":
                    return "61-90";
                case "120":
                    return "91-120";
                case "180":
                    return "121-180";
                case "181":
                    return ">180";
                case "OTHER":
                    return "Other RxAmt";
            }

            return "Missing";
        }
    }
}