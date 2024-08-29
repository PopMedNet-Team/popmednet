﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using System.Web.Routing;
using Lpp.Mvc;

namespace Lpp.Dns.General.SqlDistribution
{
    public class SqlDistributionRequestType : IDnsRequestType
    {
        public static IEnumerable<SqlDistributionRequestType> All { get { return RequestTypes; } }

        public const string Sql_Distribution = "{A6ED0001-F6AE-4D25-BEF3-A22200FCBABC}";

        public static readonly IEnumerable< SqlDistributionRequestType> RequestTypes = new[]
        {
            new  SqlDistributionRequestType( Sql_Distribution, "Sql Distribution", "", "Distribute Sql to DataMarts.")
        };


        public Guid ID { get; private set; }
        public string Name { get; private set; }
        public string Description { get { return _description; } }
        public string ShortDescription { get; private set; }
        public bool IsMetadataRequest { get { return false; } }
 
        public SqlDistributionRequestType(string id, string name, string description, string shortDescription)
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