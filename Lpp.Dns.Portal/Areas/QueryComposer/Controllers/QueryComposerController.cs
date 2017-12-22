using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Lpp.Dns.Portal.Areas.QueryComposer.Controllers
{
    public class QueryComposerController : Controller
    {

        public ActionResult Test()
        {
            return View("~/areas/querycomposer/views/test.cshtml");
        }

        public ActionResult SaveCriteriaGroup()
        {
            return View("~/areas/querycomposer/views/savecriteriagroup.cshtml");
        }

        public ActionResult SummaryView()
        {
            return View("~/areas/querycomposer/views/summaryview.cshtml");
        }

        [HttpGet]
        public JsonResult VisualTerms()
        {
            var terms = QueryComposerAreaRegistration.Terms.GroupBy(g => g.Category).OrderBy(c => c.Key).SelectMany(c =>
                    {
                        if (c.Key == null)
                        {
                            return c.Select(t => new VisualTermModel(t)).ToArray();
                        }
                        else
                        {
                            var category = new VisualTermModel { 
                                                    Name = c.Key, 
                                                    Description = null, 
                                                    TermID = null, 
                                                    Terms = c.OrderBy(t => t.Name).Select(t => new VisualTermModel(t)).ToArray(), 
                                                    ValueTemplate = null,
                                                    IncludeInCriteria = c.Any(t => !string.IsNullOrEmpty(t.CriteriaEditRelativePath)),
                                                    IncludeInStratifiers = c.Any(t => !string.IsNullOrEmpty(t.StratifierEditRelativePath)),
                                                    IncludeInProjectors = c.Any(t => !string.IsNullOrEmpty(t.ProjectionEditRelativePath))
                                            };

                            return new []{ category };
                        }
                    }
                ).ToArray();


            return Json(terms, JsonRequestBehavior.AllowGet);
        }

        public class VisualTermModel
        {
            public VisualTermModel()
            {
            }

            public VisualTermModel(IVisualTerm term)
            {
                TermID = term.TermID;
                Name = term.Name;
                Description = term.Description;
                Terms = null;
                ValueTemplate = term.ValueTemplate;
                IncludeInCriteria = !string.IsNullOrEmpty(term.CriteriaEditRelativePath);
                IncludeInStratifiers = !string.IsNullOrEmpty(term.StratifierEditRelativePath);
                IncludeInProjectors = !string.IsNullOrEmpty(term.ProjectionEditRelativePath);
            }

            public string Name { get; set; }

            public string Description { get; set; }

            public Guid? TermID { get; set; }

            public IEnumerable<VisualTermModel> Terms { get; set; }

            public object ValueTemplate { get; set; }

            public bool IncludeInCriteria { get; set; }

            public bool IncludeInStratifiers { get; set; }

            public bool IncludeInProjectors { get; set; }

        }
    }
}