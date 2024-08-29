using PopMedNet.DMCS.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PopMedNet.DMCS.Data.Model
{
    [Table("RequestDataMarts")]
    public class RequestDataMart : IRequestDataMartMetadata
    {
        public Guid ID { get; set; }
        public Guid RequestID { get; set; }
        public Request Request { get; set; }
        public Guid DataMartID { get; set; }
        public DataMart DataMart { get; set; }
        public Guid ModelID { get; set; }
        public string ModelText { get; set; }
        public RoutingStatus Status { get; set; }
        public Priorities Priority { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime? DueDate { get; set; }
        public string RejectReason { get; set; }
        public RoutingType? RoutingType { get; set; }
        public byte[] PmnTimestamp { get; set; }

        public virtual IEnumerable<Response> Responses { get; set; }
    }

    public interface IRequestDataMartMetadata
    {
        Guid ID { get; }
        Guid RequestID { get; }
        Guid DataMartID { get; }
        Guid ModelID { get; }
        string ModelText { get; }
        RoutingStatus Status { get; }
        Priorities Priority { get; }
        DateTime UpdatedOn { get; }
        DateTime? DueDate { get; }
        string RejectReason { get; }
        RoutingType? RoutingType { get; }
        byte[] PmnTimestamp { get; }
    }

    public class RequestDataMartEqualityComparer : IEqualityComparer<IRequestDataMartMetadata>
    {
        public bool Equals([AllowNull] IRequestDataMartMetadata rdm1, [AllowNull] IRequestDataMartMetadata rdm2)
        {
            if (rdm1 == null && rdm2 == null)
                return true;
            else if (rdm1 == null || rdm2 == null)
                return false;

            return rdm1.ID == rdm2.ID &&
                rdm1.RequestID == rdm2.RequestID &&
                rdm1.DataMartID == rdm2.DataMartID &&
                rdm1.ModelID == rdm2.ModelID &&
                rdm1.ModelText == rdm2.ModelText &&
                rdm1.Status == rdm2.Status &&
                rdm1.Priority == rdm2.Priority &&
                rdm1.UpdatedOn == rdm2.UpdatedOn &&
                rdm1.DueDate == rdm2.DueDate &&
                rdm1.RejectReason == rdm2.RejectReason &&
                rdm1.RoutingType == rdm2.RoutingType &&
                ObjectExtensions.ByteEquals(rdm1.PmnTimestamp, rdm2.PmnTimestamp);
        }

        public int GetHashCode([DisallowNull] IRequestDataMartMetadata rdm)
        {
            var hCode = rdm.ID.GetHashCode() ^
                rdm.RequestID.GetHashCode() ^
                rdm.DataMartID.GetHashCode() ^
                rdm.ModelID.GetHashCode() ^
                rdm.ModelText.Ensure().GetHashCode() ^
                rdm.Status.GetHashCode() ^
                rdm.Priority.GetHashCode() ^
                rdm.UpdatedOn.GetHashCode() ^
                (rdm.DueDate.HasValue ? rdm.DueDate.Value.GetHashCode() : 0.GetHashCode()) ^
                rdm.RejectReason.Ensure().GetHashCode() ^
                (rdm.RoutingType.HasValue ? rdm.RoutingType.Value.GetHashCode() : 0) ^
                rdm.PmnTimestamp.GetHashCode();

            return hCode.GetHashCode();
        }
    }

}
