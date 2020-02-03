using Lpp.Dns.Data;
using Lpp.Dns.DTO;
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

            //using (var db = new DataContext())
            //{
            //    var query = from d in db.Demographics
            //                where d.Country == country && d.State == state
            //                select d;

            //    IEnumerable results;

            //    //All
            //    if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Age) == Stratifications.Age && (stratification & Stratifications.Gender) == Stratifications.Gender)
            //    {
            //        results = (from d in query
            //                   group d by new { d.Country, d.State, d.Ethnicity, d.AgeGroup, d.Gender } into g
            //                   select new
            //                   {
            //                       Country = g.Key.Country,
            //                       State = g.Key.State,
            //                       Location = (string)null,
            //                       Ethnicity = g.Key.Ethnicity,
            //                       AgeGroup = g.Key.AgeGroup,
            //                       Gender = g.Key.Gender,
            //                       Count = g.Sum(s => (int?)s.Count) ?? 0
            //                   }).ToArray();
            //    }
            //    //Ethnicity and Age
            //    else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Age) == Stratifications.Age)
            //    {
            //        results = (from d in query
            //                   group d by new { d.Country, d.State, d.Ethnicity, d.AgeGroup } into g
            //                   select new
            //                   {
            //                       Country = g.Key.Country,
            //                       State = g.Key.State,
            //                       Location = (string)null,
            //                       Ethnicity = g.Key.Ethnicity,
            //                       AgeGroup = g.Key.AgeGroup,
            //                       Count = g.Sum(s => (int?)s.Count) ?? 0
            //                   }).ToArray();
            //    }
            //    //Ethnicity and Gender
            //    else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Gender) == Stratifications.Gender)
            //    {
            //        results = (from d in query
            //                   group d by new { d.Country, d.State, d.Ethnicity, d.Gender } into g
            //                   select new
            //                   {
            //                       Country = g.Key.Country,
            //                       State = g.Key.State,
            //                       Location = (string)null,
            //                       Ethnicity = g.Key.Ethnicity,
            //                       Gender = g.Key.Gender,
            //                       Count = g.Sum(s => (int?)s.Count) ?? 0
            //                   }).ToArray();
            //    }
            //    //Age and Gender
            //    else if ((stratification & Stratifications.Age) == Stratifications.Age && (stratification & Stratifications.Gender) == Stratifications.Gender)
            //    {
            //        results = (from d in query
            //                   group d by new { d.Country, d.State, d.AgeGroup, d.Gender } into g
            //                   select new
            //                   {
            //                       Country = g.Key.Country,
            //                       State = g.Key.State,
            //                       Location = (string)null,
            //                       AgeGroup = g.Key.AgeGroup,
            //                       Gender = g.Key.Gender,
            //                       Count = g.Sum(s => (int?)s.Count) ?? 0
            //                   }).ToArray();
            //    }

            //        //Ethnicity
            //    else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity)
            //    {
            //        results = (from d in query
            //                   group d by new { d.Country, d.State, d.Ethnicity } into g
            //                   select new
            //                   {
            //                       Country = g.Key.Country,
            //                       State = g.Key.State,
            //                       Location = (string)null,
            //                       Ethnicity = g.Key.Ethnicity,
            //                       Count = g.Sum(s => (int?)s.Count) ?? 0
            //                   }).ToArray();
            //    }
            //    //Age
            //    else if ((stratification & Stratifications.Age) == Stratifications.Age)
            //    {
            //        results = (from d in query
            //                   group d by new { d.Country, d.State, d.AgeGroup } into g
            //                   select new
            //                   {
            //                       Country = g.Key.Country,
            //                       State = g.Key.State,
            //                       Location = (string)null,
            //                       AgeGroup = g.Key.AgeGroup,
            //                       Count = g.Sum(s => (int?)s.Count) ?? 0
            //                   }).ToArray();
            //    }
            //    //Gender
            //    else if ((stratification & Stratifications.Gender) == Stratifications.Gender)
            //    {
            //        results = (from d in query
            //                   group d by new { d.Country, d.State, d.Gender } into g
            //                   select new
            //                   {
            //                       Country = g.Key.Country,
            //                       State = g.Key.State,
            //                       Location = (string)null,
            //                       Gender = g.Key.Gender,
            //                       Count = g.Sum(s => (int?)s.Count) ?? 0
            //                   }).ToArray();
            //    }
            //    else //None
            //    {
            //        results = (from d in query
            //                   group d by new { d.Country, d.State } into g
            //                   select new
            //                   {
            //                       Country = g.Key.Country,
            //                       State = g.Key.State,
            //                       Location = (string)null,
            //                       Count = g.Sum(s => (int?)s.Count) ?? 0
            //                   }).ToArray();
            //    }

            //    return Json(results, JsonRequestBehavior.AllowGet);
            //}
        }

        //private IQueryable<T> ReturnGroupedAndSelected<T>(IQueryable<Demographic> query, params string[] fieldNames)
        //{
        //    var fieldNameList = fieldNames.ToList();


        //    Dictionary<string, PropertyInfo> sourceProperties = fieldNameList.ToDictionary(name => name, name => query.ElementType.GetProperty(name));
        //    Type dynamicType = LinqRuntimeTypeBuilder.GetDynamicType(sourceProperties.Values);

        //    var itemParam = Expression.Parameter(query.ElementType, "g");

        //    IEnumerable<MemberBinding> bindings = dynamicType.GetFields().Select(p => Expression.Bind(p, Expression.Property(itemParam, sourceProperties[p.Name]))).OfType<MemberBinding>();

        //    var grouper = Expression.Lambda<Func<Demographic, dynamic>>(Expression.MemberInit(Expression.New(dynamicType.GetConstructor(Type.EmptyTypes)), bindings), itemParam);

        //    var q = query.GroupBy(grouper);

        //    fieldNameList.Add("Count");

        //    sourceProperties = fieldNameList.ToDictionary(name => name, name => query.ElementType.GetProperty(name));
        //    dynamicType = LinqRuntimeTypeBuilder.GetDynamicType(sourceProperties.Values);

        //    bindings = dynamicType.GetFields().Select(p => Expression.Bind(p, Expression.Property(itemParam, sourceProperties[p.Name]))).OfType<MemberBinding>();

        //    var selector = Expression.Lambda<Func<Demographic, T>>(Expression.MemberInit(Expression.New(dynamicType.GetConstructor(Type.EmptyTypes)), bindings), itemParam);

        //    q.Select(selector);

        //    var final = query.Provider.CreateQuery(Expression.Call(typeof(Queryable), "Select", new Type[] { query.ElementType, dynamicType },
        //         Expression.Constant(query), selector));
        //}


        [HttpGet]
        public JsonResult GetCensusDataByRegion(string country, string state, string region, Stratifications stratification)
        {
            IEnumerable results = Demographics.GetCensusDataByRegion(country, state, region, stratification);
            return Json(results, JsonRequestBehavior.AllowGet);;

            //using (var db = new DataContext())
            //{
            //    var query = from d in db.Demographics
            //                where d.Country == country && d.State == state && d.Region == region
            //                select d;

            //    IEnumerable results;

            //    //All
            //    if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Age) == Stratifications.Age && (stratification & Stratifications.Gender) == Stratifications.Gender)
            //    {
            //        results = (from d in query
            //                       group d by new { d.Country, d.State, d.Region, d.Ethnicity, d.AgeGroup, d.Gender } into g
            //                       select new
            //                       {
            //                           Country = g.Key.Country,
            //                           State = g.Key.State,
            //                           Location = g.Key.Region,
            //                           Ethnicity = g.Key.Ethnicity,
            //                           AgeGroup = g.Key.AgeGroup,
            //                           Gender = g.Key.Gender,
            //                           Count = g.Sum(s => (int?) s.Count) ?? 0
            //                       }).ToArray();                    
            //    }
            //    //Ethnicity and Age
            //    else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Age) == Stratifications.Age)
            //    {
            //        results = (from d in query
            //                       group d by new { d.Country, d.State, d.Region, d.Ethnicity, d.AgeGroup } into g
            //                       select new
            //                       {
            //                           Country = g.Key.Country,
            //                           State = g.Key.State,
            //                           Location = g.Key.Region,
            //                           Ethnicity = g.Key.Ethnicity,
            //                           AgeGroup = g.Key.AgeGroup,
            //                           Count = g.Sum(s => (int?)s.Count) ?? 0
            //                       }).ToArray();
            //    }
            //    //Ethnicity and Gender
            //    else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Gender) == Stratifications.Gender)
            //    {
            //        results = (from d in query
            //                       group d by new { d.Country, d.State, d.Region, d.Ethnicity, d.Gender } into g
            //                       select new
            //                       {
            //                           Country = g.Key.Country,
            //                           State = g.Key.State,
            //                           Location = g.Key.Region,
            //                           Ethnicity = g.Key.Ethnicity,
            //                           Gender = g.Key.Gender,
            //                           Count = g.Sum(s => (int?)s.Count) ?? 0
            //                       }).ToArray();
            //    }
            //    //Age and Gender
            //    else if ((stratification & Stratifications.Age) == Stratifications.Age && (stratification & Stratifications.Gender) == Stratifications.Gender)
            //    {
            //        results = (from d in query
            //                       group d by new { d.Country, d.State, d.Region, d.AgeGroup, d.Gender } into g
            //                       select new
            //                       {
            //                           Country = g.Key.Country,
            //                           State = g.Key.State,
            //                           Location = g.Key.Region,
            //                           AgeGroup = g.Key.AgeGroup,
            //                           Gender = g.Key.Gender,
            //                           Count = g.Sum(s => (int?)s.Count) ?? 0
            //                       }).ToArray();
            //    }

            //        //Ethnicity
            //    else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity)
            //    {
            //        results = (from d in query
            //                       group d by new { d.Country, d.State, d.Region, d.Ethnicity } into g
            //                       select new
            //                       {
            //                           Country = g.Key.Country,
            //                           State = g.Key.State,
            //                           Location = g.Key.Region,
            //                           Ethnicity = g.Key.Ethnicity,
            //                           Count = g.Sum(s => (int?)s.Count) ?? 0
            //                       }).ToArray();
            //    }
            //    //Age
            //    else if ((stratification & Stratifications.Age) == Stratifications.Age)
            //    {
            //        results = (from d in query
            //                       group d by new { d.Country, d.State, d.Region, d.Ethnicity, d.AgeGroup } into g
            //                       select new
            //                       {
            //                           Country = g.Key.Country,
            //                           State = g.Key.State,
            //                           Location = g.Key.Region,
            //                           AgeGroup = g.Key.AgeGroup,
            //                           Count = g.Sum(s => (int?)s.Count) ?? 0
            //                       }).ToArray();
            //    }
            //    //Gender
            //    else if ((stratification & Stratifications.Gender) == Stratifications.Gender)
            //    {
            //        results = (from d in query
            //                       group d by new { d.Country, d.State, d.Region, d.Gender } into g
            //                       select new
            //                       {
            //                           Country = g.Key.Country,
            //                           State = g.Key.State,
            //                           Location = g.Key.Region,
            //                           Gender = g.Key.Gender,
            //                           Count = g.Sum(s => (int?)s.Count) ?? 0
            //                       }).ToArray();
            //    }
            //    else //None
            //    {
            //        results = (from d in query
            //                       group d by new { d.Country, d.State, d.Region } into g
            //                       select new
            //                       {
            //                           Country = g.Key.Country,
            //                           State = g.Key.State,
            //                           Location = g.Key.Region,
            //                           Count = g.Sum(s => (int?)s.Count) ?? 0
            //                       }).ToArray();
            //    }

            //    return Json(results, JsonRequestBehavior.AllowGet);
            //}
        }

        [HttpGet]
        public JsonResult GetCensusDataByTown(string country, string state, string town, Stratifications stratification)
        {
            IEnumerable results = Demographics.GetCensusDataByTown(country, state, town, stratification);
            return Json(results, JsonRequestBehavior.AllowGet); 

            //using (var db = new DataContext())
            //{
            //    var query = from d in db.Demographics
            //                where d.Country == country && d.State == state && d.Town == town
            //                select d;

            //    IEnumerable results;

            //    //All
            //    if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Age) == Stratifications.Age && (stratification & Stratifications.Gender) == Stratifications.Gender)
            //    {
            //        results = (from d in query
            //                   group d by new { d.Country, d.State, d.Town, d.Ethnicity, d.AgeGroup, d.Gender } into g
            //                   select new
            //                   {
            //                       Country = g.Key.Country,
            //                       State = g.Key.State,
            //                       Location = g.Key.Town,
            //                       Ethnicity = g.Key.Ethnicity,
            //                       AgeGroup = g.Key.AgeGroup,
            //                       Gender = g.Key.Gender,
            //                       Count = g.Sum(s => (int?)s.Count) ?? 0
            //                   }).ToArray();
            //    }
            //    //Ethnicity and Age
            //    else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Age) == Stratifications.Age)
            //    {
            //        results = (from d in query
            //                   group d by new { d.Country, d.State, d.Town, d.Ethnicity, d.AgeGroup } into g
            //                   select new
            //                   {
            //                       Country = g.Key.Country,
            //                       State = g.Key.State,
            //                       Location = g.Key.Town,
            //                       Ethnicity = g.Key.Ethnicity,
            //                       AgeGroup = g.Key.AgeGroup,
            //                       Count = g.Sum(s => (int?)s.Count) ?? 0
            //                   }).ToArray();
            //    }
            //    //Ethnicity and Gender
            //    else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity && (stratification & Stratifications.Gender) == Stratifications.Gender)
            //    {
            //        results = (from d in query
            //                   group d by new { d.Country, d.State, d.Town, d.Ethnicity, d.Gender } into g
            //                   select new
            //                   {
            //                       Country = g.Key.Country,
            //                       State = g.Key.State,
            //                       Location = g.Key.Town,
            //                       Ethnicity = g.Key.Ethnicity,
            //                       Gender = g.Key.Gender,
            //                       Count = g.Sum(s => (int?)s.Count) ?? 0
            //                   }).ToArray();
            //    }
            //    //Age and Gender
            //    else if ((stratification & Stratifications.Age) == Stratifications.Age && (stratification & Stratifications.Gender) == Stratifications.Gender)
            //    {
            //        results = (from d in query
            //                   group d by new { d.Country, d.State, d.Town, d.AgeGroup, d.Gender } into g
            //                   select new
            //                   {
            //                       Country = g.Key.Country,
            //                       State = g.Key.State,
            //                       Location = g.Key.Town,
            //                       AgeGroup = g.Key.AgeGroup,
            //                       Gender = g.Key.Gender,
            //                       Count = g.Sum(s => (int?)s.Count) ?? 0
            //                   }).ToArray();
            //    }

            //        //Ethnicity
            //    else if ((stratification & Stratifications.Ethnicity) == Stratifications.Ethnicity)
            //    {
            //        results = (from d in query
            //                   group d by new { d.Country, d.State, d.Town, d.Ethnicity } into g
            //                   select new
            //                   {
            //                       Country = g.Key.Country,
            //                       State = g.Key.State,
            //                       Location = g.Key.Town,
            //                       Ethnicity = g.Key.Ethnicity,
            //                       Count = g.Sum(s => (int?)s.Count) ?? 0
            //                   }).ToArray();
            //    }
            //    //Age
            //    else if ((stratification & Stratifications.Age) == Stratifications.Age)
            //    {
            //        results = (from d in query
            //                   group d by new { d.Country, d.State, d.Town, d.Ethnicity, d.AgeGroup } into g
            //                   select new
            //                   {
            //                       Country = g.Key.Country,
            //                       State = g.Key.State,
            //                       Location = g.Key.Town,
            //                       Ethnicity = g.Key.Ethnicity,
            //                       AgeGroup = g.Key.AgeGroup,
            //                       Count = g.Sum(s => (int?)s.Count) ?? 0
            //                   }).ToArray();
            //    }
            //    //Gender
            //    else if ((stratification & Stratifications.Gender) == Stratifications.Gender)
            //    {
            //        results = (from d in query
            //                   group d by new { d.Country, d.State, d.Town, d.Gender } into g
            //                   select new
            //                   {
            //                       Country = g.Key.Country,
            //                       State = g.Key.State,
            //                       Location = g.Key.Town,
            //                       Gender = g.Key.Gender,
            //                       Count = g.Sum(s => (int?)s.Count) ?? 0
            //                   }).ToArray();
            //    }
            //    else //None
            //    {
            //        results = (from d in query
            //                   group d by new { d.Country, d.State, d.Town } into g
            //                   select new
            //                   {
            //                       Country = g.Key.Country,
            //                       State = g.Key.State,
            //                       Location = g.Key.Town,
            //                       Count = g.Sum(s => (int?)s.Count) ?? 0
            //                   }).ToArray();
            //    }

            //    return Json(results, JsonRequestBehavior.AllowGet);
            //}
        }
    }
}
