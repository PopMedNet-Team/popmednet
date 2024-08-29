using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.Models.Notifications
{
    public class ChangeNotification<T> where T : class
    {
        public ChangeNotification()
        {
            ChangeType = ChangeType.NoChange;
        }

        public ChangeNotification(ChangeType changeType, T entity)
        {
            Entity = entity;
            ChangeType = changeType;
        }

        public T Entity { get; set; }

        public ChangeType ChangeType { get; set; }
    }
}
