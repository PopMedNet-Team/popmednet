using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using Lpp.Data;
using Lpp.Data.Composition;
using Lpp.Dns.HealthCare.ModularProgram.Data;
using Lpp.Dns.HealthCare.ModularProgram.Data.Entities;

namespace Lpp.Dns.HealthCare.ModularProgram
{
    public class ModularProgramDocumentStream : Stream
    {
        [Import]
        private IRepository<ModularProgramDomain, ModularProgramDocument> Documents { get; set; }
        [Import]
        private IRepository<ModularProgramDomain, ModularProgramDocumentSegment> DocumentSegments { get; set; }

        private ModularProgramDocument document = null;
        private ModularProgramDocumentSegment segment = null;
        private long documentOffset = 0;
        private int segmentOffset = 0;
        private int segmentNumber = 0;

        public ModularProgramDocumentStream()
        {

        }

        public ModularProgramDocumentStream(ModularProgramDocument Document)
        {
            document = Document;
        }

        public override long Position 
        { 
            get { return documentOffset; } 
            set { documentOffset = (int)value; } 
        }

        public override long Length
        {
            get { return document.Segments.Sum(s => s.Content.Length); }
        }

        public override bool CanWrite { get { return false; } }
        public override bool CanSeek { get { return false; } }
        public override bool CanRead { get { return true; } }

        public override int Read(byte[] buffer, int offset, int count)
        {
            Contract.Requires(document != null);
            int bytesRead = 0;
            int remaining = (int)Math.Min(count, Length - documentOffset);
            while (remaining > 0)
            {
                if (segment == null || segmentOffset >= segment.Content.Length)
                {
                    if (segmentNumber < document.Segments.Count)
                    {
                        segment = document.Segments.ElementAt(segmentNumber++);
                        segmentOffset = 0;
                    }
                    else
                    {
                        break;
                    }
                }
                int segmentCount = (int)Math.Min(remaining, segment.Content.Length - segmentOffset);
                Buffer.BlockCopy(segment.Content, segmentOffset, buffer, offset, segmentCount);
                segmentOffset += segmentCount;
                remaining -= segmentCount;
                bytesRead += segmentCount;
            }
            documentOffset += bytesRead;
            return bytesRead;
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
