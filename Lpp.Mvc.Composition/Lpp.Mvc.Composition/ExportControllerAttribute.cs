using System;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Lpp.Composition;

namespace Lpp.Mvc
{
    [MetadataAttribute]
    [AttributeUsage( AttributeTargets.Class )]
    public class ExportControllerAttribute : ExportAttribute, IExportScopeMetadata
    {
        public string Scope { get { return TransactionScope.Id; } }

        public ExportControllerAttribute() : base( typeof( IController ) )
        {
        }
    }
}