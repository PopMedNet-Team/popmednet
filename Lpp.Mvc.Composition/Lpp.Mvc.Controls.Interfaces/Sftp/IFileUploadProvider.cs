using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Lpp.Mvc.Controls
{
    public interface IFileUploadProvider
    {
        long UploadDocument(Guid requestId, string fileName, Stream inputStream);
    }
}

