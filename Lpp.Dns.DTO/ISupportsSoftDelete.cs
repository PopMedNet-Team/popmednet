using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO
{
    /// <summary>
    /// Is Support soft Delete
    /// </summary>
    public interface ISupportsSoftDelete
    {
        /// <summary>
        /// Gets or sets to indicate whether Support soft has been deleted.
        /// </summary>
        bool Deleted { get; set; }
    }
}
