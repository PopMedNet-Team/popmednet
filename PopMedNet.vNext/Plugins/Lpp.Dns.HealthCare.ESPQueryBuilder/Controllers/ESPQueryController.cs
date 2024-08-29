using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Helpers;
//using Lpp.Data;
using Lpp.Dns.Model;
using Lpp.Dns.Portal.Models;
using Lpp.Mvc;
using Lpp.Mvc.Controls;
using Lpp.Security;
using System.Reflection;
using Lpp.Dns.General.CriteriaGroup.Views.CriteriaGroup.Terms;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Views.ESPQueryBuilder.Terms;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Data;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Models;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Data.Entities;
using System.Net;

namespace Lpp.Dns.HealthCare.ESPQueryBuilder.Controllers
{
    [Export, ExportController, AutoRoute]
    public class ESPQueryController : BaseController
    {
       // [Import] public ILog Log { get; set; }

        private static ESPQueryBuilderModel m = new ESPQueryBuilderModel();

        public ESPQueryController()
        {
            InitializeModel(m);
        }

        public ActionResult Term(string term)
        {
            // TODO Use Terms class to map this to partial View.
            try
            {
                switch (term)
                {
                    case "AgeRange":
                        return View<AgeRange>().WithModel(m);
                    case "DiseaseSelector":
                        return View<DiseaseSelector>().WithModel(m);
                    case "Gender":
                        return View<Gender>().WithModel(m);
                    case "ICD9CodeSelector":
                        return View<ICD9CodeSelector>().WithModel(m);
                    case "ObservationPeriod":
                        return View<ObservationPeriod>().WithoutModel();
                    case "ZipCodeSelector":
                        return View<Lpp.Dns.HealthCare.ESPQueryBuilder.Views.ESPQueryBuilder.Terms.ZipCodeSelector>().WithModel(m);
                    case "CustomLocation":
                        return View<Lpp.Dns.HealthCare.ESPQueryBuilder.Views.ESPQueryBuilder.Terms.CustomLocation>().WithModel(m);
                    case "PredefinedLocation":
                        return View<Lpp.Dns.HealthCare.ESPQueryBuilder.Views.ESPQueryBuilder.Terms.PredefinedLocation>().WithModel(m);
                    case "RaceSelector":
                        return View<RaceSelector>().WithModel(m);
                    case "SmokingSelector":
                        return View<SmokingSelector>().WithModel(m);
                    case "EthnicitySelector":
                        return View<EthnicitySelector>().WithModel(m);
                    case "Visits":
                        return View<Visits>().WithModel(m);
                    case "Dummy":
                        return null;
                    default:
                        string message = "No view matches term: " + term + ".";
                        //Log.Debug(message);
                        return new HttpStatusCodeResult((int) HttpStatusCode.NotFound, message);
                }
            }
            catch 
            {
                string message = "Error retrieving the view for the term: " + term + " to display.";
                //Log.Debug(message);
                return new HttpStatusCodeResult((int) HttpStatusCode.InternalServerError, message);
            }
        }

        private void InitializeModel(ESPQueryBuilderModel m)
        {
            m.DiseaseSelections = DiseaseSelectionList.diseases.Select(disease => new ESPRequestBuilderSelection { Name = disease.Name, Display = disease.Display, Value = disease.Code });
            m.SexSelections = SexSelectionList.sexes.Select(sex => new StratificationCategoryLookUp { CategoryText = sex.Name, StratificationCategoryId = sex.Code });
            m.RaceSelections = RaceSelectionList.races.Select(race => new StratificationCategoryLookUp { CategoryText = race.Name, StratificationCategoryId = race.Code });
            m.SmokingSelections = SmokingSelectionList.smokings.Select(smoking => new StratificationCategoryLookUp { CategoryText = smoking.Name, StratificationCategoryId = smoking.Code });
            m.EthnicitySelections = RaceSelectionList.ethnicities.Select(race => new StratificationCategoryLookUp { CategoryText = race.Name, StratificationCategoryId = race.Code });
        }
    }
}