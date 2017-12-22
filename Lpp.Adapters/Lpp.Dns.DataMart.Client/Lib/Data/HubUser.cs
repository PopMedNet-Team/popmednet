using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Lib.Classes
{
    public class HubUser
    {
        public string Email
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public bool IsDeleted
        {
            get;
            set;
        }

        public bool IsNewOrModifiedPassword
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public int OrganizationId
        {
            get;
            set;
        }

        public string OrganizationName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public int RoleTypeId
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public int UserId
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }
        
    }

}
