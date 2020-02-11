using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities.Objects
{
    public abstract class EntityWithID : Entity, IEntityWithID
    {
        public EntityWithID()
        {
            this.ID = DatabaseEx.NewGuid();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; }

        [Timestamp]
        public Byte[] Timestamp { get; set; }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is EntityWithID)
            {
                return ((EntityWithID)obj).ID == this.ID;
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
            if (obj == null || !(obj is EntityWithID))
                return -1;

            var ob = obj as EntityWithID;
            return this.ID.CompareTo(ob.ID);
        }

        public static implicit operator Guid(EntityWithID o)
        {
            return o.ID;
        }

    }
}
