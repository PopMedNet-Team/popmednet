using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PopMedNet.Objects
{
    public interface IEntityWithDeleted
    {
        bool Deleted { get; }
    }
}
