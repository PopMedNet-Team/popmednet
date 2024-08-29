using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Utilities.Excel
{
    /// <summary>
    /// Helper methods for Excel exports.
    /// </summary>
    public class ExcelEx
    {
        public static readonly char[] InvalidCharacters = new char[] { '\\', '/', '*', ':', '[', ']' };

        readonly System.Collections.Specialized.StringDictionary _existingTabNames;

        public ExcelEx() : this(Array.Empty<string>())
        {

        }

        public ExcelEx(string[] existingTabnames)
        {
            _existingTabNames = new System.Collections.Specialized.StringDictionary();

            if (existingTabnames != null && existingTabnames.Length > 0)
            {
                for(int i = 0; i < existingTabnames.Length; i++)
                {
                    _existingTabNames.Add(existingTabnames[i], null);
                }
            }
        }

        

        public string ValidateTabName(string value)
        {
            string tabName = value.ToStringEx().Trim();

            //Can use all alphanumeric characters but not the following: \ / * ? : [ ]
            for (int i = 0; i < InvalidCharacters.Length; i++)
            {
                if (tabName.Contains(InvalidCharacters[i]))
                {
                    tabName = tabName.Replace(InvalidCharacters[i], '_');
                }
            }

            //Cannot exceed 31 characters
            tabName = tabName.MaxLength(31);

            //Be unique within the workbook
            int index = 0;
            while (_existingTabNames.ContainsKey(tabName))
            {
                index++;
                tabName = tabName.MaxLength(31, index.ToString());
            }

            _existingTabNames.Add(tabName, null);

            return tabName;
        }

    }
}
