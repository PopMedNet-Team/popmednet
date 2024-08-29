using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Objects
{
    public interface IEntityWithDeleted
    {
        bool Deleted { get; }
    }
}
