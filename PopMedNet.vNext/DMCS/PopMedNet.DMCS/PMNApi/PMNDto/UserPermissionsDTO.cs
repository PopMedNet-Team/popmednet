using System;

namespace PopMedNet.DMCS.PMNApi.PMNDto
{
    public class UserPermissionsDTO
    {
        public Guid DataMartID { get; set; }
        public bool CanView { get; set; }
        public bool AllowUnattendedProcessing { get; set; }
        public bool CanUpload { get; set; }
        public bool CanHold { get; set; }
        public bool CanReject { get; set; }
        public bool CanModifyResults { get; set; }
        public bool CanViewAttachments { get; set; }
        public bool CanModifyAttachments { get; set; }
    }
}
