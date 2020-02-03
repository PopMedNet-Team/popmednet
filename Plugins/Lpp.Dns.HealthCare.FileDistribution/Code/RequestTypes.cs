using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;

namespace Lpp.Dns.HealthCare.FileDistribution
{
    public class FileDistributionRequestType : IDnsRequestType
    {
        public static IEnumerable<FileDistributionRequestType> All { get { return RequestTypes; } }

        public const string File_Distribution = "{B01C0001-8B6E-49E9-9A4B-A22200FC3147}";

        public static readonly IEnumerable< FileDistributionRequestType> RequestTypes = new[]
        {
            new  FileDistributionRequestType( File_Distribution, "File Distribution", "", "Distribute files to DataMarts.")
        };


        public Guid ID { get; private set; }
        public string Name { get; private set; }
        public string Description { get { return _description; } }
        public string ShortDescription { get; private set; }
        public bool IsMetadataRequest { get { return false; } }

        public FileDistributionRequestType(string id, string name, string description, string shortDescription)
        {
            ID = new Guid( id );
            Name = name;
//            Description = description;
            ShortDescription = shortDescription;
        }

        private const string _description = @"This page allows you to create a File Distribution request to distribute any type of file.

<p>For more information on submitting requests, see <a href=https://popmednet.atlassian.net/wiki/display/DOC/Submitting+Requests target=_blank>PopMedNet User's Guide: Submitting Requests</a>.</p>

<p>For specific information regarding Modular Program requests, see <a href=https://popmednet.atlassian.net/wiki/display/DOC/File+Distribution target=_blank>PopMedNet User's Guide: File Distribution</a>.</p>";
    }

}