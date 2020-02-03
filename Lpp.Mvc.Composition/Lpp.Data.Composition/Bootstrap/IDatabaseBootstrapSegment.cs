using System.Data.Entity;
using System;

namespace Lpp.Data.Composition
{
    public interface IDatabaseBootstrapSegment
    {
        Type Domain { get; }
        void Execute( DbContext ctx );
    }
}