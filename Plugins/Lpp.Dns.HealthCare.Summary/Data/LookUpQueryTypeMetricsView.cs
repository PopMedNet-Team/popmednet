using System;
using System.Collections.Generic;
using Lpp.Dns.DTO.Enums;

namespace Lpp.Dns.HealthCare.Summary
{
    public partial class LookUpQueryTypeMetricsView
    {
        public readonly static LookUpQueryTypeMetricsView Metric0 = new LookUpQueryTypeMetricsView(Metrics.Events, Guid.Parse(SummaryRequestType.MFU_ICD9Diagnosis));
        public readonly static LookUpQueryTypeMetricsView Metric1 = new LookUpQueryTypeMetricsView(Metrics.Events, Guid.Parse(SummaryRequestType.MFU_ICD9Procedures));
        public readonly static LookUpQueryTypeMetricsView Metric2 = new LookUpQueryTypeMetricsView(Metrics.Events, Guid.Parse(SummaryRequestType.MFU_HCPCSProcedures));
        public readonly static LookUpQueryTypeMetricsView Metric3 = new LookUpQueryTypeMetricsView(Metrics.Events, Guid.Parse(SummaryRequestType.MFU_ICD9Diagnosis_4_digit));
        public readonly static LookUpQueryTypeMetricsView Metric4 = new LookUpQueryTypeMetricsView(Metrics.Events, Guid.Parse(SummaryRequestType.MFU_ICD9Diagnosis_5_digit));
        public readonly static LookUpQueryTypeMetricsView Metric5 = new LookUpQueryTypeMetricsView(Metrics.Events, Guid.Parse(SummaryRequestType.MFU_ICD9Procedures_4_digit));
        public readonly static LookUpQueryTypeMetricsView Metric6 = new LookUpQueryTypeMetricsView(Metrics.Users, Guid.Parse(SummaryRequestType.MFU_GenericName));
        public readonly static LookUpQueryTypeMetricsView Metric7 = new LookUpQueryTypeMetricsView(Metrics.Users, Guid.Parse(SummaryRequestType.MFU_DrugClass));
        public readonly static LookUpQueryTypeMetricsView Metric8 = new LookUpQueryTypeMetricsView(Metrics.Users, Guid.Parse(SummaryRequestType.MFU_ICD9Diagnosis));
        public readonly static LookUpQueryTypeMetricsView Metric9 = new LookUpQueryTypeMetricsView(Metrics.Users, Guid.Parse(SummaryRequestType.MFU_ICD9Procedures));
        public readonly static LookUpQueryTypeMetricsView Metric10 = new LookUpQueryTypeMetricsView(Metrics.Users, Guid.Parse(SummaryRequestType.MFU_HCPCSProcedures));
        public readonly static LookUpQueryTypeMetricsView Metric11 = new LookUpQueryTypeMetricsView(Metrics.Users, Guid.Parse(SummaryRequestType.MFU_ICD9Diagnosis_4_digit));
        public readonly static LookUpQueryTypeMetricsView Metric12 = new LookUpQueryTypeMetricsView(Metrics.Users, Guid.Parse(SummaryRequestType.MFU_ICD9Diagnosis_5_digit));
        public readonly static LookUpQueryTypeMetricsView Metric13 = new LookUpQueryTypeMetricsView(Metrics.Users, Guid.Parse(SummaryRequestType.MFU_ICD9Procedures_4_digit));
        public readonly static LookUpQueryTypeMetricsView Metric14 = new LookUpQueryTypeMetricsView(Metrics.Dispensing_DrugOnly, Guid.Parse(SummaryRequestType.MFU_GenericName));
        public readonly static LookUpQueryTypeMetricsView Metric15 = new LookUpQueryTypeMetricsView(Metrics.Dispensing_DrugOnly, Guid.Parse(SummaryRequestType.MFU_DrugClass));
        public readonly static LookUpQueryTypeMetricsView Metric16 = new LookUpQueryTypeMetricsView(Metrics.DaysSupply_DrugOnly, Guid.Parse(SummaryRequestType.MFU_GenericName));
        public readonly static LookUpQueryTypeMetricsView Metric17 = new LookUpQueryTypeMetricsView(Metrics.DaysSupply_DrugOnly, Guid.Parse(SummaryRequestType.MFU_DrugClass));

        public readonly static LookUpQueryTypeMetricsView[] All = { Metric0, Metric1, Metric2, Metric3, Metric4, Metric5, Metric6, Metric7, Metric8, Metric9, Metric10, Metric11, Metric12, Metric13, Metric14, Metric15, Metric16, Metric17 };

        public Metrics MetricID { get; set; }
        public Guid RequestTypeID { get; set; }

        public LookUpQueryTypeMetricsView(Metrics metricID, Guid reqTypeID)
        {
            this.MetricID = metricID;
            this.RequestTypeID = reqTypeID;
        }
    }

}
