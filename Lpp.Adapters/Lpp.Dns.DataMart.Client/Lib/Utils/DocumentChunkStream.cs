using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lpp.Dns.DataMart.Model;
using Lpp.Dns.DataMart.Lib;

namespace Lpp.Dns.DataMart.Lib.Classes
{
    [Serializable]
    public class DocumentChunkStream : Stream
    {
        private Guid documentID;
        private NetWorkSetting networkSetting;
        private int documentOffset = 0;

        public override long Position { get { return documentOffset; } set { documentOffset = (int) value; } }
        public override long Length
        {
            get { return 0; }
        }

        public override bool CanWrite { get { return false; } }
        public override bool CanSeek { get { return false; } }
        public override bool CanRead { get { return true; } }

        public DocumentChunkStream(Guid documentID, NetWorkSetting networkSetting)
        {
            this.documentID = documentID;
            this.networkSetting = networkSetting;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            byte[] DocContent = DnsServiceManager.GetDocumentChunk(documentID, documentOffset, count, networkSetting);

            int size = DocContent.Length;
            Buffer.BlockCopy(DocContent, 0, buffer, offset, size);
            documentOffset += size;
            return size;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }
    }
}