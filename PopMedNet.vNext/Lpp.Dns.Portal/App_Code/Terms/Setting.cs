using Lpp.Dns.DTO.Enums;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class Setting : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.SettingID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "Setting/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "Setting/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return null; }
            //get { return "Setting/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return null; }
            //get { return "Setting/stratifierview.cshtml"; }
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
            get { return "Settings"; }

        }

        public string Description
        {
            get { return "General lookup of Settings of the visit."; }
        }

        public string Category
        {
            get
            {
                return "Criteria";
            }
        }


        public object ValueTemplate
        {
            get { return new SettingValues(); }
        }
    }

    public class SettingValues
    {
        public Settings? Setting { get; set; }
    }
}