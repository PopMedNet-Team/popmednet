using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.IO;
using Lpp.Mvc;
using Lpp.Dns.Data;

namespace Lpp.Dns.DocumentVisualizers
{
    public class TextDocumentVisualizer<TView> : IDnsDocumentVisualizer
        where TView : WebViewPage<Models.DocumentContentModel>
    {
        public Func<HtmlHelper, IHtmlString> Visualize( Document doc )
        {
            using (var db = new DataContext())
            {
                string text;
                using (var stream = new StreamReader(doc.GetStream(db)))
                {
                    text = stream.ReadToEnd();
                }

                return html => html.Partial<TView>().WithModel(new Models.DocumentContentModel { Content = text });
            }
        }
    }
}
