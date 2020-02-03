using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Dns.Data;
using System.Diagnostics.Contracts;
using Lpp.Mvc.Controls;
using Lpp.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Lpp.Dns.Portal.Models
{
    public class UserSignUpPostModel : CrudPostModel<User>
    {
        [Required(ErrorMessage="Please enter the organization")]
        public Guid OrganizationID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string NewUsername { get; set; }
        public string ChangePassword { get; set; }
        public string NewPassword { get; set; }
        public string RepeatPassword { get; set; }
        public string RoleRequested { get; set; }

        //public Guid SID { get; set; }
    }
}