using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reactive;
using System.Diagnostics.Contracts;
using System.IO;

namespace Lpp.Dns
{
    public interface IDnsPersistentDocument : IDnsDocument
    {
        Guid ID { get; }

        string ReadStreamAsString();
    }
}