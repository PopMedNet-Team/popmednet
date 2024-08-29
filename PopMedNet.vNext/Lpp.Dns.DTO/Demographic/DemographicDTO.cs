﻿using Lpp.Dns.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lpp.Objects.ValidationAttributes;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Demographic
    /// </summary>
    [DataContract]
    public class DemographicDTO
    {
        /// <summary>
        /// Country
        /// </summary>
        [MaxLength(2), DataMember]
        public string Country { get; set; }
        /// <summary>
        /// State
        /// </summary>
        [MaxLength(2), DataMember]
        public string State { get; set; }
        /// <summary>
        /// Town
        /// </summary>
        [MaxLength(50), DataMember]
        public string Town { get; set; }
        /// <summary>
        /// Region
        /// </summary>
        [MaxLength(50), DataMember]
        public string Region { get; set; }
        /// <summary>
        /// Gender
        /// </summary>
        [MaxLength(1), MinLength(1), DataMember]
        public string Gender { get; set; }
        /// <summary>
        /// Age Group
        /// </summary>
        [DataMember]
        public AgeGroups AgeGroup { get; set; }
        /// <summary>
        /// Ethnicity
        /// </summary>
        [DataMember]
        public Ethnicities Ethnicity { get; set; }
        /// <summary>
        /// Returns the Demographic count
        /// </summary>
        [DataMember]
        public int Count { get; set; }
    }
}
