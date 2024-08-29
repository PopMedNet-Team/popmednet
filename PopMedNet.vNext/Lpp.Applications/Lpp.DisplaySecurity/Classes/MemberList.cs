using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.DisplaySecurity
{
    class MemberList
    {
        public List<Member> ListOfMembers;

        public MemberList()
        {
            ListOfMembers = new List<Member>();
        }


        public void AddMember(string name, string id, string location)
        {
            ListOfMembers.Add(new Member
            {
                Name = name,
                ID =  id,
                LocationName = location
            });
        }
    }
}
