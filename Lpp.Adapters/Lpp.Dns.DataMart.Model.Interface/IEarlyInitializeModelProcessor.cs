using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Model
{
    /// <summary>
    /// Interface that inherits from IModelProcessor and support early initialization.
    /// </summary>
    public interface IEarlyInitializeModelProcessor : IModelProcessor
    {
        /// <summary>
        /// Initializes the ModelProcessor with the specified model ID and request documents.
        /// </summary>
        /// <param name="modelID">The ID of the model specified for the DataMart.</param>
        /// <param name="documents">The documents associated with the request.</param>
        void Initialize(Guid modelID, DocumentWithStream[] documents);
    }
}
