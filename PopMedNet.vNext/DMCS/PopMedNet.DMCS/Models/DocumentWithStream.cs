using PopMedNet.DMCS.Data.Model;
using System;
using System.IO;

namespace PopMedNet.DMCS.Models
{
    public class DocumentWithStream
    {
        public Guid ID { get; set; }
        public Document Document { get; set; }
        public Stream Stream { get; set; }
    }
}
