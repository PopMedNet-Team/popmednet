using Lpp.Dns.Workflow.DataChecker.Activities;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Workflow.DataChecker
{
    public class DataCheckerWorkflowConfiguration : IWorkflowConfiguration
    {
        public static readonly Guid WorkflowID = new Guid("942A2B39-0E9C-4ECE-9E2C-C85DF0F42663");
        public static readonly Guid NewRequestActivityID = new Guid("11383C00-C270-4A46-97D2-5B1AC527B7F8");
        public static readonly Guid ReviewRequestActivityID = new Guid("3FFBCA99-5801-4045-9FB4-072136A845FC");
        public static readonly Guid ApproveResponseID = new Guid("1D0D4993-EA87-4C0D-9226-43F8BB83C952");
        public static readonly Guid CompletedID = new Guid("9392ACEF-1AF3-407C-B19C-BAE88C389BFC");
        public static readonly Guid TerminateRequestID = new Guid("CC2E0001-9B99-4C67-8DED-A3B600E1C696");
        public static readonly Guid ViewRequestRoutingsID = new Guid("ACBA0001-0CE4-4C00-8DD3-A3B5013A3344");

        public Dictionary<Guid, Type> Activities
        {
            get
            {
                var dict = new Dictionary<Guid, Type>();
                dict.Add(NewRequestActivityID, typeof(NewRequest));
                dict.Add(ReviewRequestActivityID, typeof(RequestReview));
                dict.Add(ViewRequestRoutingsID, typeof(ViewRequestRoutings));
                //don't think we need approve reponse step, wrapped into view response
                dict.Add(ApproveResponseID, typeof(ApproveResponse));
                dict.Add(CompletedID, typeof(Completed));
                dict.Add(TerminateRequestID, typeof(TerminateRequest));
                return dict;
            }
        }

        public Guid ID{
            get { return WorkflowID; }
        }
    }
}
