using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.ESPQueryBuilder.Data
{
    public class ZipCodeSelectionList
    {
        public readonly static ZipCodeSelectionList Amherst = new ZipCodeSelectionList("Amherst, MA", "01002", 1);
        public readonly static ZipCodeSelectionList Boston = new ZipCodeSelectionList("Boston, MA", "14025", 2);
        public readonly static ZipCodeSelectionList Atlanta = new ZipCodeSelectionList("Atlanta, GA", "67008", 3);

        public readonly static ZipCodeSelectionList[] zipCodes = { Amherst, Boston, Atlanta };

        public string Name { get; set; }
        public string ZipCode { get; set; }
        public int Code { get; set; }

        public ZipCodeSelectionList(string name, string zipCode, int code)
        {
            Name = name;
            ZipCode = zipCode;
            Code = code;
        }

        public static string GetName(string zipCode)
        {
            foreach (ZipCodeSelectionList zip in zipCodes)
            {
                if (zip.ZipCode == zipCode)
                    return zip.Name;
            }

            return null;
        }
        
    }
}
