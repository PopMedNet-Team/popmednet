using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PopMedNet.DMCS.Data.Model
{
    [Table("Requests")]
    public class Request : IRequestMetadata
    {
        public Guid ID { get; set; }
        public long Identifier { get; set; }
        public string MSRequestID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AdditionalInstructions { get; set; }
        public string Activity { get; set; }
        public string ActivityDescription { get; set; }
        public string RequestType { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string SubmittedBy { get; set; }
        public string Project { get; set; }
        public string PurposeOfUse { get; set; }
        public string PhiDisclosureLevel { get; set; }
        public string TaskOrder { get; set; }
        public string ActivityProject { get; set; }
        public string RequestorCenter { get; set; }
        public string WorkPlanType { get; set; }
        public string ReportAggregationLevel { get; set; }
        public string SourceActivity { get; set; }
        public string SourceActivityProject { get; set; }
        public string SourceTaskOrder { get; set; }
        public byte[] PmnTimestamp { get; set; }

        public virtual IEnumerable<RequestDataMart> Routes { get; set; }
    }

    public interface IRequestMetadata
    {
        Guid ID { get; }
        long Identifier { get; }
        string MSRequestID { get; }
        string Name { get; }
        string Description { get; }
        string AdditionalInstructions { get; }
        string Activity { get; }
        string ActivityDescription { get; }
        string ActivityProject { get; }
        string RequestType { get; }
        DateTime SubmittedOn { get; }
        string SubmittedBy { get; }
        string Project { get; }
        string PurposeOfUse { get; }
        string PhiDisclosureLevel { get; }
        string TaskOrder { get; }        
        string RequestorCenter { get; }
        string WorkPlanType { get; }
        string ReportAggregationLevel { get; }
        string SourceActivity { get; }
        string SourceActivityProject { get; }
        string SourceTaskOrder { get; }
        byte[] PmnTimestamp { get; }
    }

    public class RequestMetadataEqualityComparer : IEqualityComparer<IRequestMetadata>
    {
        public bool Equals([AllowNull] IRequestMetadata r1, [AllowNull] IRequestMetadata r2)
        {
            if (r1 == null && r2 == null)
                return true;
            else if (r1 == null || r2 == null)
                return false;

            return r1.ID == r2.ID &&
                r1.Identifier == r2.Identifier &&
                r1.MSRequestID == r2.MSRequestID &&
                r1.Name == r2.Name &&
                r1.Description == r2.Description &&
                r1.AdditionalInstructions == r2.AdditionalInstructions &&
                r1.Activity == r2.Activity &&
                r1.ActivityDescription == r2.ActivityDescription &&
                r1.ActivityProject == r2.ActivityProject &&
                r1.RequestType == r2.RequestType &&
                r1.SubmittedOn == r2.SubmittedOn &&
                r1.SubmittedBy == r2.SubmittedBy &&
                r1.Project == r2.Project &&
                r1.PurposeOfUse == r2.PurposeOfUse &&
                r1.PhiDisclosureLevel == r2.PhiDisclosureLevel &&
                r1.TaskOrder == r2.TaskOrder &&
                r1.RequestorCenter == r2.RequestorCenter &&
                r1.WorkPlanType == r2.WorkPlanType &&
                r1.ReportAggregationLevel == r2.ReportAggregationLevel &&
                r1.SourceActivity == r2.SourceActivity &&
                r1.SourceActivityProject == r2.SourceActivityProject &&
                r1.SourceTaskOrder == r2.SourceTaskOrder &&
                ObjectExtensions.ByteEquals(r1.PmnTimestamp, r2.PmnTimestamp);
        }

        public int GetHashCode([DisallowNull] IRequestMetadata r)
        {
            var stringValues = new System.Text.StringBuilder();
            stringValues.AppendJoin(string.Empty, r.MSRequestID,
                r.Name, r.Description, r.AdditionalInstructions, r.Activity, r.ActivityDescription, r.ActivityProject, r.RequestType,
                r.SubmittedBy, r.Project, r.PurposeOfUse, r.PhiDisclosureLevel, r.TaskOrder, r.RequestorCenter, r.WorkPlanType, r.ReportAggregationLevel,
                r.SourceActivity, r.SourceActivityProject);

            var hCode = r.ID.GetHashCode() ^
                r.Identifier.GetHashCode() ^
                stringValues.ToString().GetHashCode() ^
                r.SubmittedOn.GetHashCode() ^
                r.PmnTimestamp.GetHashCode();

            return hCode.GetHashCode();
                
        }
    }
}
