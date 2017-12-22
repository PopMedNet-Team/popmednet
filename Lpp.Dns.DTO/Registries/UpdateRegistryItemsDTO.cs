using Lpp.Dns.DTO.Enums;
using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Update Registry items
    /// </summary>
    [DataContract]
    public class UpdateRegistryItemsDTO
    {
        /// <summary>
        /// Gets or sets the id of registry
        /// </summary>
        [DataMember]
        public Guid registryID;
        /// <summary>
        /// Available registry item definitions
        /// </summary>
        [DataMember]
        public IEnumerable<RegistryItemDefinitionDTO> registryItemDefinitions;
    }
}
