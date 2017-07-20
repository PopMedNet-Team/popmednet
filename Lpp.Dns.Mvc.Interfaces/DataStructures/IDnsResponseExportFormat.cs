using System;

namespace Lpp.Dns
{
    public interface IDnsResponseExportFormat
    {
        string ID { get; }
        string Name { get; }
    }
}