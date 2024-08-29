using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PopMedNet.DMCS.Data.Model
{
    public class Response : IResponseMetadata
    {
        public Guid ID { get; set; }
        public Guid RequestDataMartID { get; set; }
        public virtual RequestDataMart RequestDataMart { get; set; }
        public string RespondedBy { get; set; }
        public DateTime? ResponseTime { get; set; }
        public int Count { get; set; }
        public string SubmitMessage { get; set; }
        public string ResponseMessage { get; set; }
        public byte[] PmnTimestamp { get; set; }

        public virtual IEnumerable<RequestDocument> Documents { get; set; }
        public virtual IEnumerable<Log> Logs { get; set; }
    }

    public interface IResponseMetadata
    {        
        Guid ID { get; }        
        Guid RequestDataMartID { get; }
        string RespondedBy { get; }        
        DateTime? ResponseTime { get; }
        int Count { get; }
        string ResponseMessage { get; }
        byte[] PmnTimestamp { get; }
    }

    public class ResponseMetadataEqualityComparer : IEqualityComparer<IResponseMetadata>
    {
        public bool Equals([AllowNull] IResponseMetadata rsp1, [AllowNull] IResponseMetadata rsp2)
        {
            if (rsp1 == null && rsp2 == null)
                return true;
            else if (rsp1 == null || rsp2 == null)
                return false;

            return rsp1.ID == rsp2.ID &&
                rsp1.RequestDataMartID == rsp2.RequestDataMartID &&
                rsp1.RespondedBy == rsp2.RespondedBy &&
                rsp1.ResponseTime == rsp2.ResponseTime &&
                rsp1.Count == rsp2.Count &&
                rsp1.ResponseMessage == rsp2.ResponseMessage &&
                ObjectExtensions.ByteEquals(rsp1.PmnTimestamp, rsp2.PmnTimestamp);
        }

        public int GetHashCode([DisallowNull] IResponseMetadata rsp)
        {
            var hCode = rsp.ID.GetHashCode() ^
                rsp.RequestDataMartID.GetHashCode() ^
                rsp.RespondedBy.Ensure().GetHashCode() ^
                rsp.ResponseTime.GetHashCode() ^
                rsp.Count.GetHashCode() ^
                rsp.ResponseMessage.Ensure().GetHashCode() ^
                rsp.PmnTimestamp.GetHashCode();

            return hCode.GetHashCode();
        }
    }
}
