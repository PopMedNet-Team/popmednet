using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// Port of Java SassyReader
namespace Lpp.Dns.General.SassyReader
{
    public class SasHeader
    {
        private readonly String sasRelease;
        private readonly String sasHost;
        private readonly int pageSize;
        private readonly int pageCount;

        public SasHeader(String sasRelease, String sasHost, int pageSize,
                int pageCount)
        {
            this.sasRelease = sasRelease;
            this.sasHost = sasHost;
            this.pageSize = pageSize;
            this.pageCount = pageCount;
        }

        public String getSasRelease()
        {
            return sasRelease;
        }

        public String getSasHost()
        {
            return sasHost;
        }

        public int getPageSize()
        {
            return pageSize;
        }

        public int getPageCount()
        {
            return pageCount;
        }

        public String toString()
        {
            return "SasHeader [sasRelease=" + sasRelease + ", sasHost=" + sasHost
                    + ", pageSize=" + pageSize + ", pageCount=" + pageCount + "]";
        }
    }
}
