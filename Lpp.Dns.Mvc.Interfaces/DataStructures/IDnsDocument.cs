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
    public interface IDnsDocument
    {
        Stream ReadStream();
        //byte[] Data { get; }
        long BodySize { get; }
        string Name { get; }
        string MimeType { get; }
        bool Viewable { get; }
        string Kind { get; }
        string FileName { get; }
    }
}