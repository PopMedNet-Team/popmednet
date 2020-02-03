using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;
using Lpp.Dns.HealthCare.I2B2.Data.Entities;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.HealthCare.I2B2
{
    public class I2B2RequestType
    {
        public static IEnumerable<I2B2RequestType> All { get { return RequestTypes; } }

        public const string I2B2_QUERY = "{A4850001-B3A7-4596-80BC-A22200FC06E9}";

        public static readonly IEnumerable<I2B2RequestType> RequestTypes = new[]
        {
            new I2B2RequestType( I2B2_QUERY, "I2B2 (Embedded)", "", "Compose i2b2 queries", true, false ),
        };


        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get { return _description; } }
        public string ShortDescription { get; private set; }
        public bool ShowDiseaseSelector { get; private set; }
        public bool ShowICD9CodeSelector { get; private set; }

        public I2B2RequestType( string id, string name, string description, string shortDescription, bool showICD9CodeSelector, bool showDiseaseSelector, Lists lookupList = default( Lists ) )
        {
            Id = new Guid( id );
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