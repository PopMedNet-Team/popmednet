using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Dns.DTO.QueryComposer;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.SummaryQuery
{
    public class DummyModelAdapter : ModelAdapter
    {

        public DummyModelAdapter(RequestMetadata requestMetadata): base(QueryComposerModelMetadata.SummaryTableModelID, requestMetadata)
        {

        }

        public override void Dispose()
        {
           
        }

        public override QueryComposerResponseDTO Execute(QueryComposerRequestDTO request, bool viewSQL)
        {
            throw new NotImplementedException();
        }

        public override QueryComposerModelProcessor.DocumentEx[] OutputDocuments()
        {
            throw new NotImplementedException();
        }
    }
}
