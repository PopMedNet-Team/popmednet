using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.IO;
using Lpp.Mvc;
using System.Data;
using System.ComponentModel.Composition;
using Lpp.Composition;
using Lpp.Dns.Data;

namespace Lpp.Dns.DocumentVisualizers
{
    [DnsDocumentVisualizer( "x-application/lpp-dns-table" )]
    [Export( typeof( IDnsDocumentVisualizer ) ), PartMetadata( ExportScope.Key, TransactionScope.Id )]
    public class GridDocumentVisualizer : IDnsDocumentVisualizer
    {
        public Func<HtmlHelper, IHtmlString> Visualize( Document doc )
        {
            var ds = new DataSet();
            using (var db = new DataContext())
            {
                try
                {
                    using (var s = doc.GetStream(db))
                    {
                        ds.ReadXml(s, XmlReadMode.ReadSchema);
                    }
                }
                catch (Exception ex)
                {
                    return html => html.Partial<Views.Error>().WithModel(ex);
                }

                return html => html.Partial<Views.Grid>().WithModel(ds);
            }
        }
    }
}