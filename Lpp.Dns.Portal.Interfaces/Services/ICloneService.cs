using System;
using System.Collections.Generic;
using System.Linq;

namespace Lpp.Dns.Portal
{
    /// <summary>
    /// Service to clone an exiting object to an new instance.
    /// </summary>
    public interface ICloneService<T> where T: class
    {
        /// <summary>
        /// Clone an existing object of T to a new object of T.
        /// </summary>
        /// <param name="existing">The existing object to clone.</param>
        /// <returns></returns>
        T Clone(T existing);
    }
}
