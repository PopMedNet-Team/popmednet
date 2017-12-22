using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Lpp.Data.Composition;

namespace Lpp.Dns.Model
{
    public interface INamed
    {
        string Name { get; }
    }
}