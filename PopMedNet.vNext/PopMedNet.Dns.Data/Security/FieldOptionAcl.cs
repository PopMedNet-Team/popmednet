using System.ComponentModel.DataAnnotations;
using PopMedNet.Objects;
using PopMedNet.Dns.DTO.Enums;

namespace PopMedNet.Dns.Data
{
    public abstract class FieldOptionAcl : Entity
    {
        [MaxLength(80)]
        public string FieldIdentifier { get; set; }
        public FieldOptionPermissions Permission { get; set; }
        public bool Overridden { get; set; }
    }
}
