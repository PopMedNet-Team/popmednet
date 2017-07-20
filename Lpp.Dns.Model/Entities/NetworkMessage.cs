using Lpp.Data.Composition;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lpp.Dns.Model
{
    [Table("NetworkMessages")]
    public class NetworkMessage : IHaveId<Guid>
    {
        public NetworkMessage()
        {
            Id = UserDefinedFunctions.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        public string MessageText { get; set; }
        public DateTime Created { get; set; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class NetworkMessagePersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            builder.Entity<NetworkMessage>();
        }
    }
}