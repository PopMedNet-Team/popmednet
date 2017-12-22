using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;

namespace Lpp.Dns.General.Metadata
{
    public class MetadataSearchRequestType : IDnsRequestType
    {
        public static IEnumerable<MetadataSearchRequestType> All { get { return RequestTypes; } }

        //public const string Metadata_Search = "{BDA33485-0C1A-4A9D-8671-90BF13D3CDB1}";

        public static readonly IEnumerable< MetadataSearchRequestType> RequestTypes = new[]
        {
            new  MetadataSearchRequestType( "{0C330F69-5927-43C8-9036-68CC9D6186C7}", "DataMart Search", "", "Search for datamarts using metadata"),
            new  MetadataSearchRequestType( "{9E22D68A-7DC3-4AD5-B38A-03EA5F72C654}", "Organization Search", "", "Search for organizations using metadata"),
            new  MetadataSearchRequestType( "{2CA2379E-40D6-4e59-BD41-FC116D304A43}", "Registry Search", "", "Search for registry information using metadata"),
            new  MetadataSearchRequestType( "{C5D10001-B1FE-4292-A744-A22200FCE11B}", "Request Search", "", "Search for submitted requests using metadata")
        };


        public Guid ID { get; private set; }
        public string Name { get; private set; }
        public string Description { get { return _description; } }
        public string ShortDescription { get; private set; }
        public bool IsMetadataRequest { get { return true; } }

        public MetadataSearchRequestType(string id, string name, string description, string shortDescription)
        {
            ID = new Guid( id );
            Name = name;
            ShortDescription = shortDescription;
        }

        private const string _description = @"This page allows you to perform a search of the network metadata.

<p>For more information on metadata searches, see <a href=https://popmednet.atlassian.net/wiki/display/DOC/Metadata+Searching target=_blank>PopMedNet User's Guide: Metadata Searching</a>.</p>
";
    }

}