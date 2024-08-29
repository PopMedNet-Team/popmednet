﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.I2B2.Data
{
    public class AgeStratificationSelectionList
    {
        public readonly static AgeStratificationSelectionList five = new AgeStratificationSelectionList("5Year", "5 Year Age Group", 1);
        public readonly static AgeStratificationSelectionList ten = new AgeStratificationSelectionList("10Year", "10 Year Age Group", 2);

        public readonly static AgeStratificationSelectionList[] ages = { five, ten };

        public string Name { get; set; }
        public string Display { get; set; }
        public int Code { get; set; }

        public AgeStratificationSelectionList(string name, string display, int code)
        {
            Name = name;
            Display = display;
            Code = code;
        }

        public static string GetName(int? code)
        {
            if (code == null)
                return null;

            foreach (AgeStratificationSelectionList age in ages)
            {
                if (age.Code == code)
                    return age.Name;
            }

            return null;
        }
        
        public static string GetDisplay(int? code)
        {
            if (code == null)
                return null;

            foreach (AgeStratificationSelectionList age in ages)
            {
                if (age.Code == code)
                    return age.Display;
            }

            return null;
        }
    }
}
