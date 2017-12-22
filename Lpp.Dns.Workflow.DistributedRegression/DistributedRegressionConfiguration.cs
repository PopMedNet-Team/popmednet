using Lpp.Dns.Workflow.DistributedRegression.Activities;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.Workflow.DistributedRegression
{
    public class DistributedRegressionConfiguration : IWorkflowConfiguration
    {
        public static readonly Guid WorkflowID = new Guid("E9656288-33FF-4D1F-BA77-C82EB0BF0192");
        public static readonly Guid DistributionID = new Guid("94E90001-A620-4624-9003-A64F0121D0D7");
        public static readonly Guid CompleteDistributionID = new Guid("D0E659B8-1155-4F44-9728-B4B6EA4D4D55");
        public static readonly Guid ConductAnalysisID = new Guid("370646FC-7A47-43B5-A4B3-659F90A188A9");
        public static readonly Guid CompletedID = new Guid("9392ACEF-1AF3-407C-B19C-BAE88C389BFC");
        public static readonly Guid TerminateRequestID = new Guid("CC2E0001-9B99-4C67-8DED-A3B600E1C696");
        public static readonly Guid ModularProgramTermID = new Guid("A1AE0001-E5B4-46D2-9FAD-A3D8014FFFD8");


        public Dictionary<Guid, Type> Activities
        {
            get
            {
                var dict = new Dictionary<Guid, Type>();
                dict.Add(DistributedRegressionConfiguration.DistributionID, typeof(Distribution));
                dict.Add(DistributedRegressionConfiguration.CompleteDistributionID, typeof(CompleteDistribution));
                dict.Add(DistributedRegressionConfiguration.ConductAnalysisID, typeof(ConductAnalysis));
                dict.Add(DistributedRegressionConfiguration.CompletedID, typeof(Completed));
                dict.Add(TerminateRequestID, typeof(Terminated));
                return dict;
            }
        }

        public Guid ID
        {
            get
            {
                return WorkflowID;
            }
        }
    }
    internal class ModularProgramTermValues
    {
        public IList<Document> Documents { get; set; }

        public class Document
        {
            public Guid RevisionSetID { get; set; }
        }
    }
}
