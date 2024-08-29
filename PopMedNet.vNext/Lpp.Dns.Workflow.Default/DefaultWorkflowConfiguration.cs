using Lpp.Dns.Workflow.Default.Activities;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Workflow.Default
{
    public class DefaultWorkflowConfiguration : IWorkflowConfiguration
    {
        public static readonly Guid WorkflowID = new Guid("F64E0001-4F9A-49F0-BF75-A3B501396946");
        public static readonly Guid NewRequestActivityID = new Guid("C1380001-4524-49BA-B4B6-A3B5013A3343");
        public static readonly Guid ReviewRequestActivityID = new Guid("73740001-A942-47B0-BF6E-A3B600E7D9EC");
        public static readonly Guid ViewRequestID = new Guid("ACBA0001-0CE4-4C00-8DD3-A3B5013A3344");
        public static readonly Guid ApproveResponseID = new Guid("6CE50001-A2B7-4721-890D-A3B600EDF917");
        public static readonly Guid CompletedID = new Guid("9392ACEF-1AF3-407C-B19C-BAE88C389BFC");
        public static readonly Guid TerminateRequestID = new Guid("CC2E0001-9B99-4C67-8DED-A3B600E1C696");



        public Dictionary<Guid, Type> Activities
        {
            get {
                var dict = new Dictionary<Guid, Type>();
                dict.Add(NewRequestActivityID, typeof(NewRequest));
                dict.Add(ReviewRequestActivityID, typeof(RequestReview));
                //view request is the routing listing
                dict.Add(ViewRequestID, typeof(ViewRequest));
                //don't think we need approve reponse step, wrapped into view response
                dict.Add(ApproveResponseID, typeof(ApproveResponse));
                dict.Add(TerminateRequestID, typeof(TerminateRequest));
                dict.Add(CompletedID, typeof(Completed));
                return dict;
            }
        }

        public Guid ID
        {
            get { return WorkflowID; }
        }
    }
}
