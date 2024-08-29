using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PopMedNet.DMCS
{
    public class ApplicationParameters
    {
        object _lock = new object();
        bool _dbMigrationComplete = false;

        public bool DbMigrationComplete
        {
            get
            {
                lock (_lock)
                {
                    return _dbMigrationComplete;
                }
            }

            set
            {
                lock (_lock)
                {
                    _dbMigrationComplete = value;
                }
            }
        }

        public TimeSpan SyncServiceInterval
        {
            get;
            set;
        }
    }
}
