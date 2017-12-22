using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;

namespace Lpp.Dns.HealthCare.ModularProgram
{
    public class ModularProgramRequestType : IDnsRequestType
    {
        public static IEnumerable<ModularProgramRequestType> All { get { return RequestTypes; } }

        public const string Modular_Program = "{BBB00001-16E2-4C53-8AEB-A22200FBAE28}";
        public const string Ad_Hoc = "{2C880001-5E3D-4032-9ADA-A22200FBC595}";
        public const string Testing = "{EC1A0001-C467-4F03-A2F7-A22200FBDE89}";

        // Define request types for the plugin model
        public static readonly IEnumerable< ModularProgramRequestType> RequestTypes = new[]
        {
            new  ModularProgramRequestType( Modular_Program, "Modular Program", "", "Modular Program Submission to DataMarts"),
            new  ModularProgramRequestType( Ad_Hoc, "Ad Hoc", "", "Ad Hoc Program Submission to DataMarts"),
            new  ModularProgramRequestType( Testing, "Testing", "", "Testing Program Submission to DataMarts")
        };

        public enum ModularProgramRequest
        { 
            ModularProgram,
            AdHoc,
            Test
        }

        public Guid ID { get; private set; }
        public string Name { get; private set; }
        public string Description { get { return _description; } }
        public bool IsMetadataRequest { get { return true; } 
        }
        public string ShortDescription { get; private set; }
        public ModularProgramRequest Type { get; private set; }

        public ModularProgramRequestType(string id, string name, string description, string shortDescription)
        {
            ID = new Guid( id );
            Name = name;
            //Description = description;
            ShortDescription = shortDescription;
            Type = id == Modular_Program ? ModularProgramRequest.ModularProgram : id == Ad_Hoc ? ModularProgramRequest.AdHoc : ModularProgramRequest.Test;
        }

        // TODO: How is the long and short description text displayed on a page?
        private const string _description = @"This page allows you to create a Modular Program Distribution request to distribute modular program packages.

<p>For more information on submitting requests, see <a href='https://popmednet.atlassian.net/wiki/display/DOC/Submitting+Requests'>PopMedNet User's Guide: Submitting Requests</a>.</p>

<p>For specific information regarding Modular Program Distribution requests, see <a href='https://popmednet.atlassian.net/wiki/display/DOC/Modular+Program+Distribution'>PopMedNet User's Guide: Modular Program Distribution</a>.</p>
";
    }

}