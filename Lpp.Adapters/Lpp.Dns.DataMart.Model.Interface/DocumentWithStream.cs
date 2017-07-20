using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model
{
    [DataContract, Serializable]
    public class DocumentWithStream : MarshalByRefObject, IDisposable
    {
        readonly System.IO.Stream _stream;
        readonly Document _document;
        readonly Guid _id;

        public DocumentWithStream(Guid documentID, Document document, System.IO.Stream stream)
        {
            _id = documentID;
            _document = document;
            _stream = stream;
        }

        [DataMember]
        public Guid ID { get { return _id; } }

        [DataMember]
        public Document Document { get { return _document; } }

        [DataMember]
        public System.IO.Stream Stream { get { return _stream; } }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}
