using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class MetadataRefresh : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.MetaDataRefreshID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return null; }
        }

        public string CriteriaViewRelativePath
        {
            get { return null; }
        }

        public string StratifierEditRelativePath
        {
            get { return "MetadataRefresh/EditStratifierView.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "MetadataRefresh/StratifierView.cshtml"; }
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
            get { return "Metadata Refresh"; }

        }

        public string Description
        {
            get { return "Refreshes Dates of the Metadata"; }
        }

        public string Category
        {
            get
            {
                return null;
            }
        }

        public object ValueTemplate
        {
            get
            {
                return null;
            }
        }
    }
}