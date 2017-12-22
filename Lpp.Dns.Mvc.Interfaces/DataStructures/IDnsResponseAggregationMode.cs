using System;

namespace Lpp.Dns
{
    public interface IDnsResponseAggregationMode
    {
        string ID { get; }
        string Name { get; }
    }
}