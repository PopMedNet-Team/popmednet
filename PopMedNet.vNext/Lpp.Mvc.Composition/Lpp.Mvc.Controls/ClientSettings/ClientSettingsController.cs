using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics.Contracts;
using Lpp.Mvc;
using System.Linq.Expressions;
using System.Web.Mvc.Html;
using System.ComponentModel.Composition;


namespace Lpp.Mvc.Controls
{
    [ExportController, Export, AutoRoute]
    public class ClientSettingsController : Controller
    {
        [Import] public IClientSettingsService Service { get; set; }

        public ContentResult ClientScript()
        {
            Response.ContentType = "text/javascript";
            return null;
        }

        [HttpPost]
        public ActionResult SetSetting( string key, string value )
        {
            Service.SetSettings( new SortedList<string,string> { { key, value } } );
            return new EmptyResult();
        }
    }
}