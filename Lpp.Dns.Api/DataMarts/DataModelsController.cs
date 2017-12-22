using Lpp.Dns.Data;
using Lpp.Dns.DTO;
using Lpp.Utilities;
using System.Data.Entity;
using Lpp.Utilities.WebSites.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lpp.Dns.DTO.Security;

namespace Lpp.Dns.Api.DataMarts
{
    /// <summary>
    /// Controller that services requests related to Data Models.
    /// </summary>
    public class DataModelsController : LppApiDataController<DataModel, DataModelDTO, DataContext, PermissionDefinition>
    {
        /// <summary>
        /// Gets a specific DataModel by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        public override async System.Threading.Tasks.Task<DataModelDTO> Get(Guid ID)
        {
            return await base.Get(ID);
        }

        /// <summary>
        /// Gets a list of items allowing oData Filtering criteria
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override IQueryable<DataModelDTO> List()
        {
            return (from m in DataContext.DataModels select m).Map<DataModel, DataModelDTO>();
        }

        /// <summary>
        /// Get a list of the supported Processors by datamodel.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<DataModelProcessorDTO> ListDataModelProcessors()
        {
            if (Lpp.QueryComposer.TermRegistration.TermsRegistrationManager.DataModelProcessors == null)
            {
                //This is a temporary work-around for when the Term Registration fails to retrieve DataModelProcessors.
                //We need to identify a proper solution to this.

                Guid queryComposerProcessor = new Guid("ae0da7b0-0f73-4d06-b70b-922032b7f0eb");
                Guid lppProcessor = new Guid("da5e7e0c-5bb7-435c-828b-568246ef97a9");
                Guid sasProcessor = new Guid("5d630771-8619-41f7-9407-696302e48237");
                Guid espSQLProcessor = new Guid("AE85D3E6-93F8-4CB5-BD45-D2F84AB85D83");
                Guid espQueryProcessor = new Guid("1BD526D9-46D8-4F66-9191-5731CB8189EE");
                Guid fileDistributionProcessor = new Guid("C8BC0BD9-A50D-4B9C-9A25-472827C8640A");
                Guid metadataSearchProcessor = new Guid("9D0CD143-7DCA-4953-8209-224BDD3AF718");
                Guid webserviceProcessor = new Guid("55C48A42-B800-4A55-8134-309CC9954D4C");
                Guid dataCheckerProcessor = new Guid("5DE1CF20-1CE0-49A2-8767-D8BC5D16D36F");
                Guid summaryQueryProcessor = new Guid("cc14e6a2-99a8-4ef8-b4cb-779a7b93a7bb");
                Guid conditionsProcessor = new Guid("D1C750B3-BA77-4F40-BA7E-F5FF28137CAF");

                var dmProcessors = (from p in DataContext.RequestTypeDataModels
                                    where p.RequestType.ProcessorID.HasValue
                                    select new
                                    {
                                        DataModelID = p.DataModelID,
                                        ProcessorID = p.RequestType.ProcessorID.Value
                                    }).Distinct();
                return (from p in dmProcessors
                        select new DataModelProcessorDTO()
                        {
                            ModelID = p.DataModelID,
                            ProcessorID = p.ProcessorID,
                            Processor = p.ProcessorID == queryComposerProcessor ? "Query Composer Model Processor"
                                            : p.ProcessorID == lppProcessor ? "Network Service Model Processor"
                                            : p.ProcessorID == sasProcessor ? "SAS Model Processor"
                                            : p.ProcessorID == espSQLProcessor ? "ESP SQL Distribution Model Processor"
                                            : p.ProcessorID == espQueryProcessor ? "ESP Query Builder Model Processor"
                                            : p.ProcessorID == fileDistributionProcessor ? "File Distribution Model Processor"
                                            : p.ProcessorID == metadataSearchProcessor ? "Metadata Query Model Processor"
                                            : p.ProcessorID == webserviceProcessor ? "Web Service Model Processor"
                                            : p.ProcessorID == dataCheckerProcessor ? "Data Checker Model Processor"
                                            : p.ProcessorID == summaryQueryProcessor ? "Summary Query Model Processor"
                                            : p.ProcessorID == conditionsProcessor ? "Conditions Model Processor"
                                            : "<Undefined>"
                        });
            }
            else
            {
                return Lpp.QueryComposer.TermRegistration.TermsRegistrationManager.DataModelProcessors;
            }
        }

    }
}
