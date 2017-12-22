using Lpp.Dns.DTO;
using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class ZipCodes : IVisualTerm
    {

        public Guid TermID
        {
            get { return ModelTermsFactory.ZipCodeID; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "zipcodes/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "zipcodes/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return "zipcodes/editstratifierview.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return "zipcodes/stratifierview.cshtml"; }
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
            get { return "Zip Codes"; }

        }

        public string Description
        {
            get { return "Filter and validate based Zip Codes"; }
        }

        public string Category
        {
            get
            {
                return "Demographic";
            }
        }


        public object ValueTemplate
        {
            get { return new ZipCodeValues(); }
        }
    }

    public class ZipCodeValues
    {
        public ZipCodeValues()
        {
            this.CodeValues = new List<CodeSelectorValueDTO>();
            this.Codes = new List<string>();
        }
        [Obsolete]
        public IEnumerable<string> Codes { get; set; }
        public IEnumerable<CodeSelectorValueDTO> CodeValues { get; set; }
    }
}