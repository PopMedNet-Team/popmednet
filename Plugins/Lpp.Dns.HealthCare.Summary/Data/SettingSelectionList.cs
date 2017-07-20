using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.HealthCare.Summary.Data
{
    public class SettingSelectionList
    {
        public readonly static SettingSelectionList Inpatient = new SettingSelectionList("Inpatient", "IP");
        public readonly static SettingSelectionList Outpatient = new SettingSelectionList("Outpatient(Ambulatory)", "AV");
        public readonly static SettingSelectionList Emergency = new SettingSelectionList("Emergency Department", "ED");
        public readonly static SettingSelectionList AnySetting = new SettingSelectionList("Any Setting", "AN");

        public readonly static SettingSelectionList NewInpatient = new SettingSelectionList("Inpatient", "1");
        public readonly static SettingSelectionList NewOutpatient = new SettingSelectionList("Outpatient(Ambulatory)", "2");
        public readonly static SettingSelectionList NewEmergency = new SettingSelectionList("Emergency Department", "3");
        public readonly static SettingSelectionList NewAnySetting = new SettingSelectionList("Any Setting", "4");


        public readonly static SettingSelectionList[] Settings = { Inpatient, Outpatient, Emergency, AnySetting, NewInpatient, NewOutpatient, NewEmergency, NewAnySetting };

        public string Name { get; set; }
        public string Code { get; set; }

        public SettingSelectionList(string name, string code)
        {
            Name = name;
            Code = code;
        }

        public static string GetName(string code)
        {
            foreach (SettingSelectionList Setting in Settings)
            {
                if (Setting.Code == code)
                    return Setting.Name;
            }

            return null;
        }
    }

    public partial class SettingSelectionLookUp
    {
        public string Code { get; set; }
        public string Name { get; set; }

    }
}
