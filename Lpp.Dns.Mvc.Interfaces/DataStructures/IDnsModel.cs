using System;
using System.Collections.Generic;

namespace Lpp.Dns
{
    public interface IDnsModel
    {
        string Name { get; }
        Guid ID { get; }
        Guid ModelProcessorID { get; }
        IEnumerable<IDnsRequestType> Requests { get; }
    }
}