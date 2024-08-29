using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Web.Mvc;

namespace Lpp.Mvc
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple=false)]
    public class ExportModelBinderAttribute : ExportAttribute, IModelBinderMetadata
    {
        public Type TargetType { get; private set; }

        public ExportModelBinderAttribute( Type targetType ) : base( typeof(IModelBinder) )
        {
            //Contract.Requires( targetType != null );
            TargetType = targetType;
        }
    }

    public interface IModelBinderMetadata
    {
        Type TargetType { get; }
    }
}
