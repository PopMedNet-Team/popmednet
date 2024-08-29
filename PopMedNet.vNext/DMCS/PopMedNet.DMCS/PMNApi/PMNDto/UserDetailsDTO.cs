using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.PMNApi.PMNDto
{
    public class UserDetailsDTO
    {
        public Guid ID { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
    }
}
