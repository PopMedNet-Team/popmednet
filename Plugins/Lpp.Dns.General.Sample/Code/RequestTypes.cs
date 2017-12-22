using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;

namespace Lpp.Dns.General.Sample
{
    public class SampleRequestType : IDnsRequestType
    {
        public static IEnumerable<SampleRequestType> All { get { return RequestTypes; } }

        public const string ModelProcessorId = "{F985DBD9-DA7E-41B4-8FBD-2A73B7FCF6DD}";

        public static readonly IEnumerable<SampleRequestType> RequestTypes = new[]
        {
            new  SampleRequestType( "{B8F2B52E-CBF9-4EE8-94EB-FC226E2426B6}", "Sample", "", "Sample")
        };


        public Guid ID { get; private set; }
        public string Name { get; private set; }
        public string Description { get { return _description; } }
        public string ShortDescription { get; private set; }
        public bool IsMetadataRequest { get { return false; } }

        public SampleRequestType(string id, string name, string description, string shortDescription)
        {
            ID = new Guid( id );
            Name = name;
            ShortDescription = shortDescription;
        }

        private const string _description = @"This page allows you to INSERT LONG DESCRIPTION HERE. All steps are required unless noted as optional with a blue asterisk (*).<br/>
<br/>
INSERT DESCRIPTION HERE. <br/>
<br/>
INSERT DESCRIPTION HERE";
    }

}