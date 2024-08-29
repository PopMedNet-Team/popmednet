using Lpp.Dns.Workflow.ModularProgram.Activities;
using Lpp.Workflow.Engine.Interfaces;
using System;
using System.Collections.Generic;

namespace Lpp.Dns.Workflow.ModularProgram
{
    public class SimpleModularProgramWorkflowConfiguration : IWorkflowConfiguration
    {
        public static readonly Guid WorkflowID = new Guid("931C0001-787C-464D-A90F-A64F00FB23E7");
        public static readonly Guid DistributeRequestID = new Guid("94E90001-A620-4624-9003-A64F0121D0D7");
        public static readonly Guid ViewStatusAndResultsID = new Guid("D0E659B8-1155-4F44-9728-B4B6EA4D4D55");
        public static readonly Guid CompletedID = new Guid("9392ACEF-1AF3-407C-B19C-BAE88C389BFC");
        public static readonly Guid TerminateRequestID = new Guid("CC2E0001-9B99-4C67-8DED-A3B600E1C696");

        public static readonly Guid ModularProgramTermID = new Guid("A1AE0001-E5B4-46D2-9FAD-A3D8014FFFD8");

        public Dictionary<Guid, Type> Activities
        {
            get
            {
                var dict = new Dictionary<Guid, Type>();
                dict.Add(DistributeRequestID, typeof(SimpleDistibuteRequest));
                dict.Add(ViewStatusAndResultsID, typeof(ViewStatusAndResults));
                dict.Add(CompletedID, typeof(Completed));
                dict.Add(TerminateRequestID, typeof(TerminateRequest));
                return dict;
            }
        }

        public Guid ID
        {
            get { return WorkflowID; }
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
