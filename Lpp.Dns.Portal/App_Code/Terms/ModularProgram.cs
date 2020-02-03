using Lpp.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lpp.Dns.Portal.App_Code.Terms
{
    public class ModularProgram : IVisualTerm
    {
        public Guid TermID
        {
            get { return ModelTermsFactory.ModularProgramID; }
        }

        public string Name
        {
            get { return ModelTermResources.ModularProgram_Name; }
        }

        public string Description
        {
            get { return ModelTermResources.ModularProgram_Description; }
        }

        public string Category
        {
            get { return "Criteria"; }
        }

        public string CriteriaEditRelativePath
        {
            get { return "modularprogram/edit.cshtml"; }
        }

        public string CriteriaViewRelativePath
        {
            get { return "modularprogram/view.cshtml"; }
        }

        public string StratifierEditRelativePath
        {
            get { return null; }
            //get { return "modularprogram/EditStratifierView.cshtml"; }
        }

        public string StratifierViewRelativePath
        {
            get { return null; }
            //get { return "modularprogram/StratifierView.cshtml"; }
        }

        public string ProjectionEditRelativePath
        {
            get { return null; }
        }

        public string ProjectionViewRelativePath
        {
            get { return null; }
        }

        public object ValueTemplate
        {
            get { return new ModularProgramUploadValues(); }
        }
    }

    public class ModularProgramUploadValues
    {
        public IEnumerable<ModularProgramDocumentModel> Documents { get; set; }
    }

    public class ModularProgramDocumentModel
    {
        public Guid RevisionSetID { get; set; }
    }
}