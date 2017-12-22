using System.Collections.Generic;
using Lpp.Audit;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal.Models
{
    public class RegistrationEmailModel
    {
        public User User { get; set; }
        public string Status { get; set; }
    }
}