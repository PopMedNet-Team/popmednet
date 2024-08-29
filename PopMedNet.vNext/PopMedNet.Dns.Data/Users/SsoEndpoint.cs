using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PopMedNet.Utilities.Objects;

namespace PopMedNet.Dns.Data
{
    [Table("SsoEndpoints")]
    public class SsoEndpoint : EntityWithID
    {
        [Required, MaxLength(150)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required, MaxLength(255)]
        public string PostUrl { get; set; }

        [MaxLength(150)]
        public string oAuthKey { get; set; }

        [MaxLength(150)]
        public string oAuthHash { get; set; }

        public bool RequirePassword { get; set; }

        public Guid Group { get; set; }

        public int DisplayIndex { get; set; }

        public bool Enabled { get; set; }
    }
}
