using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO.Schedule
{
    [DataContract]
    public class LegacySchedulerRequestDTO
    {
        /// <summary>
        /// The ID of the request to schedule
        /// </summary>
        [DataMember]
        public Guid? RequestID { get; set; }

        /// <summary>
        /// The adapter package version
        /// </summary>
        [DataMember]
        public string AdapterPackageVersion { get; set; }

        /// <summary>
        /// The schedule details
        /// </summary>
        [DataMember]
        public string ScheduleJSON { get; set; } 
    }
}
