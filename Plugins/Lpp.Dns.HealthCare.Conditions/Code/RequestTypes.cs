using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;
using Lpp.Dns.HealthCare.Conditions.Data.Entities;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.HealthCare.Conditions
{
    public class ConditionsRequestType : IDnsRequestType
    {
        public static IEnumerable<ConditionsRequestType> All { get { return RequestTypes; } }

        public const string CONDITIONS = "{4EEE0635-AC4C-49A2-9CF7-2A6C923DC176}";

        public static readonly IEnumerable<ConditionsRequestType> RequestTypes = new[]
        {
            new ConditionsRequestType( CONDITIONS, "Conditions", "", "Compose a request based on conditions.", false, true ),
        };


        public Guid ID { get; private set; }
        public string StringId { get; private set; } // Used for direct comparisons to the const strings in the "New Query Type GUID Constants" section, as Guid-to-string conversions strip braces and change case.
        public string Name { get; private set; }
        public string Description { get { return _description; } }
        public bool IsMetadataRequest { get { return false; } }
        public string ShortDescription { get; private set; }
        public bool ShowDiseaseSelector { get; private set; }
        public bool ShowICD9CodeSelector { get; private set; }

        public ConditionsRequestType( string id, string name, string description, string shortDescription, bool showICD9CodeSelector, bool showDiseaseSelector, Lists lookupList = default( Lists ) )
        {
            ID = new Guid( id );
            StringId = id;
            Name = name;
//            Description = description;
            ShortDescription = shortDescription;
            ShowDiseaseSelector = showDiseaseSelector;
            ShowICD9CodeSelector = showICD9CodeSelector;
        }

        private const string _description = @"This page allows you to create menu-driven queries. Select a query category and query type using the drop down menus (step one) and follow the instructions to submit a query. Fill in all details about the purpose and activity associated with the query. Data partners will review this information and respond accordingly. All steps are required unless noted as optional with a blue asterisk (*).<br/>
<br/>
The data represent summary counts of people and encounters/dispensings within each stratum. <br/>
<br/>
Counts can be summed across age and sex strata within an observation period (e.g., year). To prevent double-counting, DO NOT sum person counts across periods or locations of service. For example, a person with diabetes in the outpatient setting in 2004 may also be counted in 2004 under inpatient diabetes and in 2005 under outpatient diabetes";
    }

}