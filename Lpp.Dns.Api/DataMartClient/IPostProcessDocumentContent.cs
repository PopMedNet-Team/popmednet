using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Api.DataMartClient
{
    /// <summary>
    /// Interface for all background tasks that should happen after a document has been uploaded.
    /// </summary>
    public interface IPostProcessDocumentContent
    {
        /// <summary>
        /// Method for Initializing the Post Processor
        /// </summary>
        /// <param name="db"></param>
        void Initialize(Data.DataContext db, string uploadDir);
        /// <summary>
        /// Method for Executing the Post Processor
        /// </summary>
        /// <param name="document"></param>
        /// <param name="stream"></param>
        Task ExecuteAsync(Data.Document document);
    }
}
