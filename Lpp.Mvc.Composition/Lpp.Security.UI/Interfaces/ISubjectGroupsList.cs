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
    public interface ISubjectGroupsList : IUIWidget
    {
        ISubjectGroupsList<TDomain> ForDomain<TDomain>();
    }

    public interface ISubjectGroupsList<TDomain>
    {
        /// <summary>
        /// Renders a list of current subject's "parents" with "Add" and "Remove" buttons.
        /// </summary>
        /// <param name="subject">The subject to operate on.</param>
        /// <param name="fieldName">Name of the form field that will hold the resulting list of "parents" as a comma-delimited string</param>
        /// <param name="groupSelector">UI for choosing a group</param>
        IHtmlString ForSubject( ISecuritySubject subject, string fieldName, IJsControlledView groupSelector, bool enabled = true );
    }
}