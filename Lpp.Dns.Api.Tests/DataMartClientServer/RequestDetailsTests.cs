using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Api.Tests.DataMartClientServer
{
    [TestClass]
    public class RequestDetailsTests
    {

        [TestMethod]
        public void GetRequestDetailList()
        {
            using(var db = new Dns.Data.DataContext())
            {
                db.Database.Log = (string message) => Console.WriteLine(message);

                //requests query: get all that have been submitted, and do not have a status of Draft, RequestRejected, Awaiting Request Approval, 
                // or Canceled and the status value is greater than 0 and is a QE file distribution request

                string FileDistributionTermID = Lpp.QueryComposer.ModelTermsFactory.FileUploadID.ToString("D");

                var requestsQuery = from r in db.Requests
                                    join rt in db.RequestTypes on r.RequestTypeID equals rt.ID
                                    where r.SubmittedOn != null
                                    && rt.WorkflowID.HasValue 
                                    && rt.Queries.Any(q => q.ComposerInterface == DTO.Enums.QueryComposerInterface.FileDistribution && q.Data.Contains(FileDistributionTermID))
                                    && r.DataMarts.Any(rdm => rdm.Status != DTO.Enums.RoutingStatus.Draft && rdm.Status != DTO.Enums.RoutingStatus.RequestRejected && rdm.Status != DTO.Enums.RoutingStatus.AwaitingRequestApproval && rdm.Status != DTO.Enums.RoutingStatus.Canceled && rdm.Status > 0)
                                    select r;

                foreach(var r in requestsQuery)
                {
                    Console.WriteLine($"Identifier: { r.Identifier }   Name: { r.Name }");
                }

            }
        }

    }
}
