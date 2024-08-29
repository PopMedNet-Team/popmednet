using Common.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;

/// Port of Java SassyReader
namespace Lpp.Dns.General.SassyReader
{
    /// <summary>
    /// Port of non-metadata portion of Java SassyReader
    /// </summary>
    public class SasReader
    {
        [Import]
        public ILog Logger { get; set; }


        // Subheader 'signatures'
        private static readonly byte[] SUBH_ROWSIZE = IO.toBytes(0xf7, 0xf7, 0xf7, 0xf7);
        private static readonly byte[] SUBH_COLSIZE = IO.toBytes(0xf6, 0xf6, 0xf6, 0xf6);
        private static readonly byte[] SUBH_COLTEXT = IO.toBytes(0xFD, 0xFF, 0xFF, 0xFF);
        private static readonly byte[] SUBH_COLATTR = IO.toBytes(0xFC, 0xFF, 0xFF, 0xFF);
        private static readonly byte[] SUBH_COLNAME = IO.toBytes(0xFF, 0xFF, 0xFF, 0xFF);
        private static readonly byte[] SUBH_COLLABS = IO.toBytes(0xFE, 0xFB, 0xFF, 0xFF);

        /**
         * Magic number
         */
        private static readonly byte[] MAGIC = IO.toBytes(0x0, 0x0, 0x0, 0x0, 0x0,
                0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0xc2, 0xea, 0x81, 0x60, 0xb3,
                0x14, 0x11, 0xcf, 0xbd, 0x92, 0x8, 0x0, 0x9, 0xc7, 0x31, 0x8c,
                0x18, 0x1f, 0x10, 0x11);

        private static SasFile _file;
        private static Stream _stream;

        public SasReader(SasFile file)
        {
            if (file == null)
            {
                throw new Exception("file cannot be null");
            }
            _file = file;
        }

        public SasReader(Stream stream)
        {
            _stream = stream;
        }

        public SasFile getFile()
        {
            return _file;
        }

        protected static bool isMagicNumber(int[] data)
        {
            return isMagicNumber(IO.toBytes(data));
        }

        protected static bool isMagicNumber(byte[] data)
        {
            return isIdentical(data, MAGIC);
        }

        private static bool isIdentical(byte[] data, byte[] expected)
        {
            if (data == null)
            {
                return false;
            }
            byte[] comparedBytes;
            if (data.Length > expected.Length)
            {
                comparedBytes = new byte[expected.Length];
                Array.Copy(data, comparedBytes, expected.Length);
            }
            else
            {
                comparedBytes = data;
            }
            return ArraysEquals(expected, comparedBytes);
        }

        private static bool ArraysEquals(byte[] a, byte[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }

            return true;
        }

        public void read(ISasReaderCallback callback)
        {
            if(_stream != null)
                read(_stream, callback);
            else
                using (FileStream fis = new FileStream(_file.PathName, FileMode.Open, FileAccess.Read))
                {
                    read(fis, callback);
                }
        }

        private void read(Stream fis, ISasReaderCallback callback)
        {
            SasHeader header = readHeader(fis);
            //Logger.Info(string.Format("Header: {0}", header));
            readPages(fis, header, callback);
            //Logger.Info("Done!");
        }

        private void readPages(Stream fis, SasHeader header, ISasReaderCallback callback) 
        {
            List<SasSubHeader> subHeaders = new List<SasSubHeader>();
            List<int> columnOffsets = new List<int>();
            List<int> columnLengths = new List<int>();
            List<SasColumnType> columnTypes = new List<SasColumnType>();
            bool subHeadersParsed = false;

            int rowCount = 0;

            int pageSize = header.getPageSize();
            int pageCount = header.getPageCount();

            // these variables will define the default amount of rows per page and
            // other defaults
            int row_count = -1;
            int row_count_fp = -1;
            int row_length = -1;
            int col_count = -1;

            for (int pageNumber = 0; pageNumber < pageCount; pageNumber++) {
                //Logger.Info(string.Format("Reading page no. {0}", pageNumber));
                byte[] pageData = new byte[pageSize];
                int read = fis.Read(pageData, 0, pageSize);
                if (read == -1) {
                    // reached end of file
                    break;
                }

                byte pageType = IO.readByte(pageData, 17);

                switch (pageType) {
                case 0:
                case 1:
                case 2:
                    // accepted type
                    //Logger.Info(string.Format("Page type supported: {0}", pageType));
                    break;
                case 4:
                    // accepted but not supported
                    //Logger.Info(string.Format("Page type not fully supported: {0}", pageType));
                    break;
                default:
                    throw new Exception("Page " + pageNumber + " has unknown type: " + pageType);
                }

                if (pageType == 0 || pageType == 2) {
                    // Read subheaders
                    int subhCount = IO.readInt(pageData, 20);
                    for (int subHeaderNumber = 0; subHeaderNumber < subhCount; subHeaderNumber++) {
                        int _base = 24 + subHeaderNumber * 12;

                        int offset = IO.readInt(pageData, _base);
                        int length = IO.readInt(pageData, _base + 4);

                        if (length > 0) {
                            byte[] rawData = IO.readBytes(pageData, offset, length);
                            byte[] signatureData = IO.readBytes(rawData, 0, 4);
                            SasSubHeader subHeader = new SasSubHeader(rawData,
                                    signatureData);
                            subHeaders.Add(subHeader);
                        }
                    }
                }

                if ((pageType == 1 || pageType == 2)) {

                    if (!subHeadersParsed) {
                        // Parse subheaders

                        SasSubHeader rowSize = getSubHeader(subHeaders,
                                SUBH_ROWSIZE, "ROWSIZE");
                        row_length = IO.readInt(rowSize.getRawData(), 20);
                        row_count = IO.readInt(rowSize.getRawData(), 24);
                        int col_count_7 = IO.readInt(rowSize.getRawData(), 36);
                        row_count_fp = IO.readInt(rowSize.getRawData(), 60);

                        SasSubHeader colSize = getSubHeader(subHeaders,
                                SUBH_COLSIZE, "COLSIZE");
                        int col_count_6 = IO.readInt(colSize.getRawData(), 4);
                        col_count = col_count_6;

                        //if (col_count_7 != col_count_6) {
                        //    Logger.Warn(
                        //            string.Format("({0}) Column count mismatch: {1} vs. {2}",
                        //            _file.PathName, col_count_6, col_count_7 ));
                        //}

                        SasSubHeader colText = getSubHeader(subHeaders,
                                SUBH_COLTEXT, "COLTEXT");

                        List<SasSubHeader> colAttrHeaders = getSubHeaders(
                                subHeaders, SUBH_COLATTR, "COLATTR");
                        SasSubHeader colAttr;
                        if (!colAttrHeaders.Any()) {
                            throw new Exception(
                                    "No column attribute subheader found");
                        } else if (colAttrHeaders.Count == 1) {
                            colAttr = colAttrHeaders[0];
                        } else {
                            colAttr = spliceColAttrSubHeaders(colAttrHeaders);
                        }

                        SasSubHeader colName = getSubHeader(subHeaders,
                                SUBH_COLNAME, "COLNAME");

                        List<SasSubHeader> colLabels = getSubHeaders(subHeaders,
                                SUBH_COLLABS, "COLLABS");
                        if (colLabels.Any() && colLabels.Count != col_count) {
                            throw new Exception(
                                    "Unexpected column label count ("
                                            + colLabels.Count + ") expected 0 or "
                                            + col_count);
                        }

                        for (int i = 0; i < col_count; i++) {
                            int _base = 12 + i * 8;

                            String columnName;
                            byte amd = IO.readByte(colName.getRawData(), _base);
                            if (amd == 0) {
                                int off = IO.readShort(colName.getRawData(),
                                        _base + 2) + 4;
                                int len = IO.readShort(colName.getRawData(),
                                        _base + 4);
                                columnName = IO.readString(colText.getRawData(),
                                        off, len);
                            } else {
                                columnName = "COL" + i;
                            }

                            // Read column labels
                            String label;
                            if (colLabels != null && colLabels.Any()) {
                                _base = 42;
                                byte[] rawData = colLabels[i].getRawData();
                                int off = IO.readShort(rawData, _base) + 4;
                                short len = IO.readShort(rawData, _base + 2);
                                if (len > 0) {
                                    label = IO.readString(colText.getRawData(),
                                            off, len);
                                } else {
                                    label = null;
                                }
                            } else {
                                label = null;
                            }

                            // Read column offset, width, type (required)
                            _base = 12 + i * 12;

                            int offset = IO.readInt(colAttr.getRawData(), _base);
                            columnOffsets.Add(offset);

                            int length = IO.readInt(colAttr.getRawData(), _base + 4);
                            columnLengths.Add(length);

                            short columnTypeCode = IO.readShort(
                                    colAttr.getRawData(), _base + 10);
                            SasColumnType columnType = (columnTypeCode == 1 ? SasColumnType.NUMERIC
                                    : SasColumnType.CHARACTER);
                            columnTypes.Add(columnType);

                                //Logger.Debug(string.Format(
                                //        "Column no. {0} read: name={1},label={2},type={3},length={4}",
                                //                i, columnName, label,
                                //                columnType, length ));
                            callback.column(i, columnName, label, columnType,
                                    length);
                        }

                        subHeadersParsed = true;
                    }

                    if (!callback.readData()) {
                        //Logger.Info("Callback decided to not read data");
                        return;
                    }

                    // Read data
                    int row_count_p;
                    int _base2;
                    if (pageType == 2) {
                        row_count_p = row_count_fp;
                        int subhCount = IO.readInt(pageData, 20);
                        _base2 = 24 + subhCount * 12;
                        _base2 = _base2 + _base2 % 8;
                    } else {
                        row_count_p = IO.readInt(pageData, 18);
                        _base2 = 24;
                    }

                    if (row_count_p > row_count) {
                        row_count_p = row_count;
                    }

                    for (int row = 0; row < row_count_p; row++) {
                        Object[] rowData = new Object[col_count];
                        for (int col = 0; col < col_count; col++) {
                            int off = _base2 + columnOffsets[col];
                            int len = columnLengths[col];

                            SasColumnType columnType = columnTypes[col];
                            if (len > 0) {
                                byte[] raw = IO.readBytes(pageData, off, len);
                                if (columnType == SasColumnType.NUMERIC && len < 8) {
                                    byte[] bb = new byte[8];
                                    for (int j = 0; j < len; j++) {
                                        bb[j] = raw[j];
                                    }
                                    for (int j = 0; j < 8 - len; j++) {
                                        bb[j] = (byte) 0x00;
                                    }

                                    raw = bb;

                                    // col$length <- 8
                                    len = 8;
                                }

                                Object value;
                                if (columnType == SasColumnType.CHARACTER) {
                                    String str = IO.readString(raw, 0, len);
                                    str = str.Trim();
                                    value = str;
                                } else {
                                    value = IO.readNumber(raw, 0, len);
                                }
                                rowData[col] = value;
                            }
                        }

                        //Logger.Debug(string.Format("Row no. {0} read: ", row));
                        //foreach(Object c in rowData)
                        //{
                        //    Logger.Debug(c);
                        //}

                        rowCount++;
                        bool next = callback.row(rowCount, rowData);
                        if (!next) {
                            //Logger.Info("Callback decided to stop iteration");
                            return;
                        }

                        _base2 = _base2 + row_length;
                    }
                }
            }
        }

        private SasSubHeader spliceColAttrSubHeaders(
                List<SasSubHeader> colAttrHeaders) {
            int colAttrHeadersSize = colAttrHeaders.Count;
            //Logger.Info(string.Format("Splicing {0} column attribute headers", colAttrHeadersSize));

            byte[] result = IO.readBytes(colAttrHeaders[0].getRawData(), 0,
                    colAttrHeaders[0].getRawData().Length - 8);

            for (int i = 1; i < colAttrHeadersSize; i++) {
                byte[] rawData = colAttrHeaders[i].getRawData();
                result = IO.concat(result,
                        IO.readBytes(rawData, 12, rawData.Length - 20));
            }

            return new SasSubHeader(result, null);
        }

        private List<SasSubHeader> getSubHeaders(List<SasSubHeader> subHeaders,
                byte[] signature, String name) {
            List<SasSubHeader> result = new List<SasSubHeader>();
            foreach (SasSubHeader subHeader in subHeaders) {
                byte[] signatureData = subHeader.getSignatureData();
                if (isIdentical(signatureData, signature)) {
                    result.Add(subHeader);
                }
            }
            return result;
        }

        private SasSubHeader getSubHeader(List<SasSubHeader> subHeaders,
                byte[] signature, String name) {
            List<SasSubHeader> result = getSubHeaders(subHeaders, signature, name);
            if (!result.Any()) {
                throw new Exception("Could not find sub header: " + name);
            } else if (result.Count != 1) {
                throw new Exception("Multiple (" + result.Count
                        + ") instances of the same sub header: " + name);
            }
            return result[0];
        }

        private SasHeader readHeader(Stream fis) 
        {
            byte[] header = new byte[1024];
            int read = fis.Read(header, 0, 1024);
            if (read != 1024) {
                throw new Exception("Header too short (not a sas7bdat file?): " + read);
            }

            if (!isMagicNumber(header)) {
                throw new Exception("Magic number mismatch!");
            }

            int pageSize = IO.readInt(header, 200);
            if (pageSize < 0) {
                throw new Exception("Page size is negative: " + pageSize);
            }

            int pageCount = IO.readInt(header, 204);
            if (pageCount < 1) {
                throw new Exception("Page count is not positive: "
                        + pageCount);
            }

            //Logger.Info(string.Format("Page size={0}, page count={1}", pageSize, pageCount));

            String sasRelease = IO.readString(header, 216, 8);
            String sasHost = IO.readString(header, 224, 8);

            return new SasHeader(sasRelease, sasHost, pageSize, pageCount);
        }
    }

    public class SasFile
    {
        public string PathName { get; set; }

        public SasFile(string pathname)
        {
            PathName = pathname;
        }
    }
}
