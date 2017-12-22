using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;
using Lpp.Dns.HealthCare.ESPQueryBuilder.Data.Entities;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.HealthCare.ESPQueryBuilder
{
    public class ESPQueryBuilderRequestType : IDnsRequestType
    {
        public static IEnumerable<ESPQueryBuilderRequestType> All { get { return RequestTypes; } }

        public const string QUERY_COMPOSER = "{15830001-6DFF-47E9-B2FD-A22200FC77C3}";

        public static readonly IEnumerable<ESPQueryBuilderRequestType> RequestTypes = new[]
        {
            new ESPQueryBuilderRequestType( QUERY_COMPOSER, "Query Composer", "", "Compose a complex query to create a report of counts stratified by age, race, sex, and period.", false, true ) // TODO Completed implementation will have both false.
        };


        public Guid ID { get; private set; }
        public string StringId { get; private set; } // Used for direct comparisons to the const strings in the "New Query Type GUID Constants" section, as Guid-to-string conversions strip braces and change case.
        public string Name { get; private set; }
        public string Description { get { return _description; } }
        public bool IsMetadataRequest { get { return false; } }
        public string ShortDescription { get; private set; }
        public bool ShowDiseaseSelector { get; private set; }
        public bool ShowICD9CodeSelector { get; private set; }

        public ESPQueryBuilderRequestType( string id, string name, string description, string shortDescription, bool showICD9CodeSelector, bool showDiseaseSelector, Lists lookupList = default( Lists ) )
        {
            ID = new Guid( id );
            StringId = id;
            Name = name;
//            Description = description;
            ShortDescription = shortDescription;
            ShowDiseaseSelector = showDiseaseSelector;
            ShowICD9CodeSelector = showICD9CodeSelector;
        }

        private const string _description = @"This page allows you to create a Query Composer request. Fill in all details about the purpose and activity associated with the request. Data partners will review this information and respond accordingly.<br/><br/>
For more information on submitting requests, see <a href='https://popmednet.atlassian.net/wiki/display/DOC/Submitting+Requests' target='_blank'>PopMedNet User's Guide: Submitting Requests</a>.<br/><br/>
For specific information regarding ESP Query Composer requests, see <a href='https://popmednet.atlassian.net/wiki/display/DOC/Query+Composer+Request' target='_blank'>PopMedNet User's Guide: Query Composer Request</a>";
    }

}