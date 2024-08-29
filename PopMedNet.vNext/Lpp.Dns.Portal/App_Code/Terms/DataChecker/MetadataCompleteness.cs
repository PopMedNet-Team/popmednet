using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms.DataChecker
{
    public class MetadataCompleteness : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.DC_MetadataCompleteness; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "datachecker/metadatacompleteness/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "datachecker/metadatacompleteness/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return null; }
        }

        public string StratifierViewRelativePath
        {
            get { return null; }
        }

        public string ProjectionEditRelativePath
        {
            get { return null; }
        }

        public string ProjectionViewRelativePath
        {
            get { return null; }
        }

        public string Name
        {
            get { return "Metadata Completeness"; }
        }

        public string Description
        {
            get { return "Metadata Completeness"; }
        }

        public string Category
        {
            get { return "Data Checker"; }
        }

        public object ValueTemplate
        {
            get { return new MetadataCompletenessValues(); }
        }

        public class MetadataCompletenessValues
        {
            public MetadataCompletenessValues()
            {
                MetadataCompletenesses = new List<int>();
            }

            public IEnumerable<int> MetadataCompletenesses { get; set; }
        }
    }
}