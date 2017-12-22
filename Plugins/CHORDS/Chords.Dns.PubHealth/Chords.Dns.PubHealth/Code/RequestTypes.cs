using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;
//using Lpp.Dns;

namespace Lpp.Dns.PubHealth
{
    public class TestRequestType : IDnsRequestType
    {
        public static IEnumerable<TestRequestType> All { get { return RequestTypes; } }

        public const string PubHealthTest = "{AA00364C-6976-4A02-98EA-18A7FE8754BF}";

        public static readonly IEnumerable<TestRequestType> RequestTypes = new[]
        {
            new  TestRequestType( PubHealthTest, "Public Health Test Request Type", "", "Test Request Type for Public Health.")
        };


        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get { return _description; } }
        public string ShortDescription { get; private set; }
        public bool IsMetadataRequest { get { return false; } }

        public TestRequestType(string id, string name, string description, string shortDescription)
        {
            Id = new Guid(id);
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