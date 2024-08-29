using PopMedNet.Objects;
using System.Runtime.Serialization;

namespace PopMedNet.Dns.DTO
{

    /// <summary>
    /// The return object for listing the buget info for a request
    /// </summary>
    [DataContract]
    public class RequestBudgetInfoDTO : EntityDtoWithID
    {
        /// <summary>
        /// The Buget Activity Identifier
        /// </summary>
        [DataMember]
        public Guid? BudgetActivityID { get; set; }
        /// <summary>
        /// The Name of the Budget Activity
        /// </summary>
        [DataMember]
        public string BudgetActivityDescription { get; set; }
        /// <summary>
        /// The Budget Activity Project Identifier
        /// </summary>
        [DataMember]
        public Guid? BudgetActivityProjectID { get; set; }
        /// <summary>
        /// The Budget Activity Project Name 
        /// </summary>
        [DataMember]
        public string BudgetActivityProjectDescription { get; set; }
        /// <summary>
        /// The Budget Task Order Identifier
        /// </summary>
        [DataMember]
        public Guid? BudgetTaskOrderID { get; set; }
        /// <summary>
        /// The Budget Task Order Name
        /// </summary>
        [DataMember]
        public string BudgetTaskOrderDescription { get; set; }
    }
}
