using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Objects
{
    [DataContract]
    [CLSCompliant(false)]
    public abstract class EntityDtoWithID :EntityDto
    {
        public EntityDtoWithID() { }

        [DataMember, ReadOnly(true)]
        public Guid? ID { get; set; }
        [DataMember]
        public byte[]? Timestamp { get; set; }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is EntityDtoWithID)
            {
                var DTO = obj as EntityDtoWithID;
                return DTO.ID.HasValue && this.ID.HasValue && DTO.ID.Value == this.ID.Value;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (obj == null || !(obj is EntityDtoWithID))
                return -1;

            var ob = obj as EntityDtoWithID;

            if (this.ID == null || ob.ID == null)
                return -1;

            return this.ID.Value.CompareTo(ob.ID.Value);
        }

        public static implicit operator Guid(EntityDtoWithID o)
        {
            return o.ID.Value;
        }
    }
}
