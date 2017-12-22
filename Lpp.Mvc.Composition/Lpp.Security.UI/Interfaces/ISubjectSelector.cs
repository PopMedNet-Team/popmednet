using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Composition.Modules;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
using Lpp.Mvc;
using System.Web;
using System.Web.Mvc;

namespace Lpp.Security.UI
{
    public interface ISubjectSelector
    {
        /// <summary>
        /// Renders a selector for choosing a security subject (group or user) and a JavaScript function that opens 
        /// ("launches") that selector
        /// </summary>
        /// <param name="html">HTML helper to use for rendering</param>
        /// <param name="chooseGroupFunctionName">Name of JavaScript function to display the selector.
        /// This function should be of the form: ([subj] -> ()) -> (), where subj = { Id, Name }
        /// In other words, this function accepts a callback that should be called when the user chooses subject(s) and passed
        /// a list of those chosen subjects.</param>
        IHtmlString Render( HtmlHelper html, string chooseGroupFunctionName );
    }
}