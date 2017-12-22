using System;

namespace Lpp.Dns
{
    public interface IDnsRequestType
    {
        string Name { get; }
        string Description { get; }
        string ShortDescription { get; }
        Guid ID { get; }
        bool IsMetadataRequest { get; }
    }
}