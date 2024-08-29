using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Client.Lib.Caching
{
    [DataContract, Serializable]
    public class CacheDocument : Model.Document
    {
        internal CacheDocument() : base(string.Empty, string.Empty, string.Empty)
        {

        }

        public CacheDocument(string documentID, string mimeType, string filename) : base(documentID, mimeType, filename)
        {

        }

        public CacheDocument(Guid documentID, string mimeType, string filename, bool isViewable, int size, string kind) : base(documentID, mimeType, filename, isViewable, size, kind)
        {

        }

        public Guid ID
        {
            get
            {
                return Guid.Parse(DocumentID);
            }
        }

        [DataMember]
        public string UploadedOn
        {
            get;set;
        }

        internal static CacheDocument FromDocument(Model.Document document)
        {
            return new CacheDocument(Guid.Parse(document.DocumentID), document.MimeType, document.Filename, document.IsViewable, document.Size, document.Kind);
        }

    }
}
