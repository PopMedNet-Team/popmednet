using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.IO;
using Lpp.Mvc;
using System.ComponentModel.Composition;
using Lpp.Composition;

namespace Lpp.Dns.DocumentVisualizers
{
    public class TextDocumentVisualizers
    {
        [DnsDocumentVisualizer( "text/plain" ), Export, ExportMetadata( ExportScope.Key, TransactionScope.Id )]
        public IDnsDocumentVisualizer PlainText { get { return new TextDocumentVisualizer<Views.PlainText>(); } }

        [DnsDocumentVisualizer( "text/html" ), Export, ExportMetadata( ExportScope.Key, TransactionScope.Id )]
        public IDnsDocumentVisualizer Html { get { return new TextDocumentVisualizer<Views.Html>(); } }

        [DnsDocumentVisualizer( "text/xml" ), Export, ExportMetadata( ExportScope.Key, TransactionScope.Id )]
        public IDnsDocumentVisualizer Xml { get { return new TextDocumentVisualizer<Views.Xml>(); } }
    }
}
