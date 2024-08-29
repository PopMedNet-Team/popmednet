using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace Lpp.Dns.LocalDataMart
{
    public struct ModelProcessorResult
    {
        public bool IsFinished { get; set; }
        public IEnumerable<IDnsDocument> Documents { get; set; }
    }

    public interface IModelProcessor
    {
        Guid Id { get; }
        ModelProcessorResult Iterate( IDnsRequestContext ctx );
    }
}