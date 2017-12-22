using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Mvc;

namespace Lpp.Dns.HealthCare.SPANQueryBuilder.Code
{
    public class SPANQueryRequestType : IDnsRequestType
    {
        public static IEnumerable<SPANQueryRequestType> All { get { return RequestTypes; } }

        public const string SPAN_Request = "{D87F0001-B2E6-4C33-8E9D-A22200FB514E}";

        public static readonly IEnumerable<SPANQueryRequestType> RequestTypes = new[]
        {
            new  SPANQueryRequestType( SPAN_Request, "SPAN Request", "", "SPAN Request to Datamarts.")
        };

        public Guid ID { get; private set; }
        public string Name { get; private set; }
        public string Description { get { return _description; } }
        public string ShortDescription { get; private set; }
        public bool IsMetadataRequest { get { return false; } }

        public SPANQueryRequestType(string id, string name, string description, string shortDescription)
        {
            ID = new Guid(id);
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