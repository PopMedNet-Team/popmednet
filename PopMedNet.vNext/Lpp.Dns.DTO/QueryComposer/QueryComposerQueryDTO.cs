﻿using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.QueryComposer
{
    /// <summary>
    /// A single query.
    /// </summary>
    [DataContract]
    public class QueryComposerQueryDTO
    {
        /// <summary>
        /// Gets or set the header of Query composer
        /// </summary>
        [DataMember]
        public QueryComposerQueryHeaderDTO Header { get; set; }
        /// <summary>
        /// supports the Where clause
        /// </summary>
        [DataMember]
        public QueryComposerWhereDTO Where { get; set; }
        /// <summary>
        /// Gets or sets the QuerycomposerselectDTO object for select operation
        /// </summary>
        [DataMember]
        public QueryComposerSelectDTO Select { get; set; }
        /// <summary>
        /// A Collection of Temporal Events.
        /// </summary>
        [DataMember]
        public IEnumerable<QueryComposerTemporalEventDTO> TemporalEvents { get; set; }
    }
}
