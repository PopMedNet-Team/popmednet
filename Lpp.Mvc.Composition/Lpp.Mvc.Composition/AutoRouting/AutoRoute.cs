using System;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace Lpp.Mvc
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class AutoRouteAttribute : Attribute, IComposableControllerMetadata
    {
        public bool AutoRoute { get { return true; } }
    }

    public interface IComposableControllerMetadata
    {
        bool AutoRoute { get; }
    }
}