using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities.Logging
{
    public interface INotifier
    {
        void Notify(IEnumerable<Notification> notifications);
    }
}
