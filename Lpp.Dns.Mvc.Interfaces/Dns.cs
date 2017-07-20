using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reactive;
using System.Diagnostics.Contracts;
using System.IO;
using Lpp.Security;

namespace Lpp.Dns
{
    public static class Dns
    {
        public static SecurityObjectKind RequestTypeSecObjectKind = Sec.ObjectKind( "Request Type" );

        public static IDnsDocument Document( string name, string mimeType, bool isViewable, string kind, byte[] Data )
        {
            return new AnonymousDnsDocument( name, mimeType, isViewable, kind, Data );
        }


        public static IDnsModel Model( string id, string modelProcessorID, string name, params IDnsRequestType[] requestTypes )
        {
            return Model( new Guid( id ), new Guid( modelProcessorID ), name, requestTypes );
        }

        public static IDnsModel Model( Guid id, Guid modelProcessorID, string name, params IDnsRequestType[] requestTypes )
        {
            return Model( id, modelProcessorID, name, requestTypes ?? Enumerable.Empty<IDnsRequestType>() );
        }

        public static IDnsModel Model( Guid id, Guid modelProcessorID, string name, IEnumerable<IDnsRequestType> requestTypes )
        {
            return new AnonymousDnsModel( name, id, modelProcessorID, requestTypes );
        }

        public static IDnsRequestType RequestType( string id, string name, string description, string shortDescription = null, bool isMetadataRequest = false )
        {
            return RequestType( new Guid( id ), name, description, shortDescription, isMetadataRequest );
        }

        public static IDnsRequestType RequestType( Guid id, string name, string description, string shortDescription = null, bool isMetadataRequest = false )
        {
            return new AnonymousDnsRequestType( id, name, description, shortDescription, isMetadataRequest );
        }

        public static IDnsResponseExportFormat ExportFormat( string id, string name )
        {
            return new AnonymousNamedID { ID = id, Name = name };
        }

        public static IDnsResponseAggregationMode AggregationMode( string id, string name )
        {
            return new AnonymousNamedID { ID = id, Name = name };
        }


        class AnonymousDnsDocument : IDnsDocument
        {
            public AnonymousDnsDocument( string name, string mimeType, bool isViewable, string kind, byte[] data )
            {
                Name = name;
                MimeType = mimeType;
                Viewable = isViewable;
                Kind = kind;
                FileName = name;
                this.Data = data;
            }

            public Stream ReadStream()
            {
                var ms = new MemoryStream(Data);
                ms.Position = 0;
                return ms;
            }

            public byte[] Data { get; private set; }        
            public long BodySize { get { return Data.Length; } }
            public string MimeType { get; private set; }
            public string Name { get; private set; }
            public bool Viewable { get; private set; }
            public string Kind { get; private set; }
            public string FileName { get; private set; }        
        }

        class AnonymousDnsModel : IDnsModel
        {
            public string Name { get; private set; }
            public Guid ID { get; private set; }
            public Guid ModelProcessorID { get; private set; }
            public IEnumerable<IDnsRequestType> Requests { get; private set; }
            
            public AnonymousDnsModel( string name, Guid id, Guid modelProcessorID, IEnumerable<IDnsRequestType> reqs )
            {
                Name = name;
                ID = id;
                ModelProcessorID = modelProcessorID;
                Requests = reqs;
            }
        }

        class AnonymousDnsRequestType : IDnsRequestType
        {
            public string Name { get; private set; }
            public string Description { get; private set; }
            public string ShortDescription { get; private set; }
            public Guid ID { get; private set; }
            public bool IsMetadataRequest { get; private set; }

            public AnonymousDnsRequestType( Guid id, string name, string description, string shortDescription, bool isMetadata )
            {
                Name = name;
                Description = description;
                ShortDescription = shortDescription;
                ID = id;
                IsMetadataRequest = isMetadata;
            }
        }

        class AnonymousNamedID : IDnsResponseExportFormat, IDnsResponseAggregationMode
        {
            public string ID { get; set; }
            public string Name { get; set; }
        }
    }
}