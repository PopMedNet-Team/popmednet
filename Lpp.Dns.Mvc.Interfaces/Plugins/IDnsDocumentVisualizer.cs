using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reactive;
using System.Diagnostics.Contracts;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using Lpp.Dns.DTO;
using Lpp.Dns.Data;

namespace Lpp.Dns
{
    [ContractClass( typeof( Contracts.IDnsDocumentVisualizerContract ) )]
    public interface IDnsDocumentVisualizer
    {
        Func<HtmlHelper, IHtmlString> Visualize( Document doc );
    }

    public interface IDnsDocumentVisualizerMetadata
    {
        string[] MimeTypes { get; }
    }

    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method,Inherited = true, AllowMultiple=false)]
    public class DnsDocumentVisualizerAttribute : Attribute, IDnsDocumentVisualizerMetadata
    {
        public string[] MimeTypes { get; private set; }

        public DnsDocumentVisualizerAttribute( params string[] mimeTypes )
        {
            MimeTypes = mimeTypes;
        }
    }

    namespace Contracts
    {
        [ContractClassFor( typeof( IDnsDocumentVisualizer ) )]
        abstract class IDnsDocumentVisualizerContract : IDnsDocumentVisualizer
        {
            public Func<HtmlHelper, IHtmlString> Visualize( Document doc )
            {
                //Contract.Requires( doc != null );
                throw new NotImplementedException();
            }
        }
    }
}