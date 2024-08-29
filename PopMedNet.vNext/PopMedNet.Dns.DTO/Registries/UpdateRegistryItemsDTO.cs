using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{
    /// <summary>
    /// Update Registry items
    /// </summary>
    [DataContract]
    public class UpdateRegistryItemsDTO
    {
        public UpdateRegistryItemsDTO()
        {
            RegistryItemDefinitions = new HashSet<RegistryItemDefinitionDTO>();
        }

        /// <summary>
        /// Gets or sets the id of registry
        /// </summary>
        [DataMember]
        public Guid RegistryID { get; set; }
        /// <summary>
        /// Available registry item definitions
        /// </summary>
        [DataMember]
        public IEnumerable<RegistryItemDefinitionDTO> RegistryItemDefinitions { get; set; }
    }
}
