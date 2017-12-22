using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;

namespace Lpp.Dns.HealthCare.SASDistribution
{
    public class SASDistributionRequestType : IDnsRequestType
    {
        public static IEnumerable<SASDistributionRequestType> All { get { return RequestTypes; } }

        public const string SAS_Distribution = "{27A90001-D8EF-4B7E-9BC9-A22200FB82F3}";

        public static readonly IEnumerable< SASDistributionRequestType> RequestTypes = new[]
        {
            new  SASDistributionRequestType( SAS_Distribution, "SAS Distribution", "", "Distribute SAS files to DataMarts.")
        };


        public Guid ID { get; private set; }
        public string Name { get; private set; }
        public string Description { get { return _description; } }
        public string ShortDescription { get; private set; }
        public bool IsMetadataRequest { get { return false; } }

        public SASDistributionRequestType(string id, string name, string description, string shortDescription)
        {
            ID = new Guid( id );
            Name = name;
//            Description = description;
            ShortDescription = shortDescription;
        }

        private const string _description = @"This page allows you to INSERT LONG DESCRIPTION HERE. All steps are required unless noted as optional with a blue asterisk (*).<br/>
<br/>
INSERT DESCRIPTION HERE. <br/>
<br/>
INSERT DESCRIPTION HERE";
    }

}