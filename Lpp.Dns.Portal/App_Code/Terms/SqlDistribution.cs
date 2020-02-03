using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class SqlDistribution : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.SqlDistributionID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "SqlDistribution/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "SqlDistribution/view.cshtml"; }
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
            get { return "Sql Distribution"; }

        }

        public string Description
        {
            get { return "Custom SQL to run against Data Partners"; }
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
            get { return new SqlDistributionValues(); }
        }
    }

    public class SqlDistributionValues
    {

        public SqlDistributionValues()
        {
            Sql = "";
        }

        public string Sql { get; set; }
    }
}