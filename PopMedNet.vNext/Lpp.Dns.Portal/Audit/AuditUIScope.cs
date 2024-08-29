using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Mvc.Application;
using Lpp.Dns.Model;
using Lpp.Mvc;
using System.ComponentModel.Composition;
using Lpp.Composition;
using Lpp.Audit;
using System.Net.Mail;

namespace Lpp.Dns.Portal
{
    static class AuditUIScope
    {
        public static readonly Guid Display = new Guid( "{BE16C994-61AD-4892-A9F8-B888F1B1C0EB}" );
        public static readonly Guid Email = new Guid( "{2D32F3DD-2748-424D-8850-4F1C0E58DF0F}" );
    }
}