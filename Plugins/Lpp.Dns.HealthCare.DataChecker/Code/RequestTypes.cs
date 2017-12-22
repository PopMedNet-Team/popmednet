using System;
using System.Collections.Generic;

namespace Lpp.Dns.HealthCare.DataChecker
{
    public class DataCheckerRequestType : IDnsRequestType
    {        
        public static IEnumerable<DataCheckerRequestType> All { get { return RequestTypes; } }

        public const string DATA_CHECKER_RACE = "{5CA5940A-CF8B-48CC-836C-66B2EB97AFB3}";
        public const string DATA_CHECKER_ETHNICITY = "{4EE29758-DCFF-4D2A-A7A8-626C81FBA367}";
        public const string DATA_CHECKER_DIAGNOSIS = "{D5DA7ACA-7179-4EA5-BD9C-534D47B6C6C4}";
        public const string DATA_CHECKER_PROCEDURE = "{39F8E764-BDD8-4D75-AE50-809C59C28E43}";
        public const string DATA_CHECKER_NDC = "{0F1EA011-B588-4775-9E16-CB6DBE12F8BE}";
        public const string DATA_CHECKER_DIAGNOSIS_PDX = "{0F1EA012-B588-4775-9E16-CB6DBE12F8BE}";
        public const string DATA_CHECKER_DISPENSING_RXAMT = "{0F1EA013-B588-4775-9E16-CB6DBE12F8BE}";
        public const string DATA_CHECKER_DISPENSING_RXSUP = "{0F1EA014-B588-4775-9E16-CB6DBE12F8BE}";
        public const string DATA_CHECKER_METADATA_COMPLETENESS = "{0F1EA015-B588-4775-9E16-CB6DBE12F8BE}";

        public static readonly IEnumerable<DataCheckerRequestType> RequestTypes = new[]
        {
            new  DataCheckerRequestType( DATA_CHECKER_RACE, "Demographic: Race", "", "Demographic: Race"),
            new  DataCheckerRequestType( DATA_CHECKER_ETHNICITY, "Demographic: Ethnicity", "", "Demographic: Ethnicity"),
            new  DataCheckerRequestType( DATA_CHECKER_DIAGNOSIS, "Diagnosis: Diagnosis Codes", "", "Diagnosis: Diagnosis Codes"),
            new  DataCheckerRequestType( DATA_CHECKER_PROCEDURE, "Procedure: Procedure Codes", "", "Procedure: Procedure Codes"),
            new  DataCheckerRequestType( DATA_CHECKER_NDC, "Dispensing: NDC", "", "Dispensing: NDC"),
            new  DataCheckerRequestType( DATA_CHECKER_DIAGNOSIS_PDX, "Diagnosis: PDX", "", "Diagnosis: PDX"),
            new  DataCheckerRequestType( DATA_CHECKER_DISPENSING_RXAMT, "Dispensing: RxAmt", "", "Dispensing: RxAmt"),
            new  DataCheckerRequestType( DATA_CHECKER_DISPENSING_RXSUP, "Dispensing: RxSup", "", "Dispensing: RxSup"),
            new  DataCheckerRequestType( DATA_CHECKER_METADATA_COMPLETENESS, "Metadata: Data Completeness", "", "Metadata: Data Completeness"),
        };

        public Guid ID { get; private set; }
        public string StringId { get; private set; } // Used for direct comparisons to the const strings in the "New Query Type GUID Constants" section, as Guid-to-string conversions strip braces and change case.
        public string Name { get; private set; }
        public string Description { get { return _description; } }
        public string ShortDescription { get; private set; }
        public bool IsMetadataRequest { get { return false; } }

        public DataCheckerRequestType(string id, string name, string description, string shortDescription)
        {
            ID = new Guid( id );
            StringId = id;
            Name = name;
            ShortDescription = shortDescription;
        }

        private const string _description = @"This page allows you to create a Data Checker request.

<p>For more information on submitting requests, see <a href=https://popmednet.atlassian.net/wiki/display/DOC/Submitting+Requests target=_blank>PopMedNet User's Guide: Submitting Requests</a>.</p> 

<p>For specific information regarding Data Checker requests, see <a href=https://popmednet.atlassian.net/wiki/display/DOC/Data+Checker target=_blank>PopMedNet User's Guide: Data Checker</a>.</p>";
    }

}