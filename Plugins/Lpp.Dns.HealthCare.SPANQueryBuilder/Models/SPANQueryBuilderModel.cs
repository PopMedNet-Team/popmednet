using System.Collections.Generic;
using System.Xml.Serialization;
//using Lpp.Data;

namespace Lpp.Dns.HealthCare.SPANQueryBuilder.Models
{
    public class SPANQueryBuilderModel : DnsPluginModel
    {
        // these nested submodels need to be simple properties so the reflection logic can find them
        public HealthCare.Models.EnrollmentSelectorPluginModel EnrollmentSelector { get; set; }

        public HealthCare.Models.AgeSelectorPluginModel ExclusionAgeSelector { get; set; }
        //public HealthCare.Models.CodeSelectorPluginModel ExclusionDxSelector { get; set; }
        //public HealthCare.Models.CodeSelectorPluginModel ExclusionPxSelector { get; set; }
        //public HealthCare.Models.CodeSelectorPluginModel ExclusionRxSelector { get; set; }

        //public HealthCare.Models.CodeSelectorPluginModel InclusionDxSelector { get; set; }
        //public HealthCare.Models.CodeSelectorPluginModel InclusionPxSelector { get; set; }
        //public HealthCare.Models.CodeSelectorPluginModel InclusionRxSelector { get; set; }

        public string IndexVariable { get; set; }
        public HealthCare.Models.AgeSelectorPluginModel IndexVariableAgeSelector { get; set; }
        public HealthCare.Models.BMISelectorPluginModel IndexVariableBMISelector { get; set; }
        //public HealthCare.Models.CodeSelectorPluginModel IndexVariableDxSelector { get; set; }
        //public HealthCare.Models.CodeSelectorPluginModel IndexVariablePxSelector { get; set; }
        //public HealthCare.Models.CodeSelectorPluginModel IndexVariableRxSelector { get; set; }

        public HealthCare.Models.ObservationPeriodPluginModel ObservationPeriod { get; set; }

        public HealthCare.Models.ReportSelectorPluginModel ReportSelector1 { get; set; }
        public HealthCare.Models.ReportSelectorPluginModel ReportSelector2 { get; set; }
        public HealthCare.Models.ReportSelectorPluginModel ReportSelector3 { get; set; }
        public HealthCare.Models.ReportSelectorPluginModel ReportSelector4 { get; set; }
        public HealthCare.Models.ReportSelectorPluginModel ReportSelector5 { get; set; }

        /// <summary>
        /// Returns a new SPANQueryBuilderModel with all submodels created (empty)
        /// </summary>
        public SPANQueryBuilderModel()
        {
            EnrollmentSelector = new HealthCare.Models.EnrollmentSelectorPluginModel();

            ExclusionAgeSelector = new HealthCare.Models.AgeSelectorPluginModel();
            //ExclusionDxSelector = new HealthCare.Models.CodeSelectorPluginModel();
            //ExclusionPxSelector = new HealthCare.Models.CodeSelectorPluginModel();
            //ExclusionRxSelector = new HealthCare.Models.CodeSelectorPluginModel();

            //InclusionDxSelector = new HealthCare.Models.CodeSelectorPluginModel();
            //InclusionPxSelector = new HealthCare.Models.CodeSelectorPluginModel();
            //InclusionRxSelector = new HealthCare.Models.CodeSelectorPluginModel();

            IndexVariableAgeSelector = new HealthCare.Models.AgeSelectorPluginModel();
            IndexVariableBMISelector = new HealthCare.Models.BMISelectorPluginModel();
            //IndexVariableDxSelector = new HealthCare.Models.CodeSelectorPluginModel();
            //IndexVariablePxSelector = new HealthCare.Models.CodeSelectorPluginModel();
            //IndexVariableRxSelector = new HealthCare.Models.CodeSelectorPluginModel();

            ObservationPeriod = new HealthCare.Models.ObservationPeriodPluginModel();

            ReportSelector1 = new HealthCare.Models.ReportSelectorPluginModel();
            ReportSelector2 = new HealthCare.Models.ReportSelectorPluginModel();
            ReportSelector3 = new HealthCare.Models.ReportSelectorPluginModel();
            ReportSelector4 = new HealthCare.Models.ReportSelectorPluginModel();
            ReportSelector5 = new HealthCare.Models.ReportSelectorPluginModel();
        }
    }
}
