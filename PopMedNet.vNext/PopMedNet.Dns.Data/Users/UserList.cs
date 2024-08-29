using System.ComponentModel.DataAnnotations;

namespace PopMedNet.Dns.Data
{
    public class UserList
    {
        [Key]
        public Guid? UserID { get; set; }
    }
}
