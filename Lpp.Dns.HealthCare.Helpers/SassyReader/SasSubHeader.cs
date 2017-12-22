using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// Port of Java SassyReader
namespace Lpp.Dns.General.SassyReader
{
    public class SasSubHeader
    {
        private byte[] _rawData;
        private byte[] _signatureData;

        public SasSubHeader(byte[] rawData, byte[] signatureData)
        {
            _rawData = rawData;
            _signatureData = signatureData;
        }

        public byte[] getSignatureData()
        {
            return _signatureData;
        }

        public byte[] getRawData()
        {
            return _rawData;
        }
    }
}
