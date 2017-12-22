using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// Port of Java SassyReader
namespace Lpp.Dns.General.SassyReader
{
    public interface ISasReaderCallback
    {
        /**
         * Callback method that accepts a column discovered by the {@link SasReader}
         * .
         * 
         * @param columnIndex
         *            the index (0-based) of the column
         * @param columnName
         *            the physical name of the column
         * @param columnLabel
         *            the logical label of the column (often more user-friendly than
         *            name)
         * @param columnType
         *            the type of the column
         * @param columnLength
         *            the length of the column
         */
        void column(int columnIndex, String columnName, String columnLabel,
                SasColumnType columnType, int columnLength);

        /**
         * Should the reader read the data/rows (or only columns?)
         * 
         * @return true if data/rows should be read.
         */
        bool readData();

        /**
         * Callback method that accepts an array of row data.
         * 
         * @param rowNumber
         *            the row number (1 = first row)
         * @param rowData
         *            the row data
         * @return true if more rows should be read.
         */
        bool row(int rowNumber, Object[] rowData);
    }


    /**
     * Represents the supported sas column types in SassyReader.
     * 
     * @author Kasper Sørensen
     */
    public enum SasColumnType
    {
        NUMERIC, CHARACTER
    }
}
