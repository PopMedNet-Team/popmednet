using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using Lpp.Composition;
using System.Web;
using Lpp.Security;
using Lpp.Dns.Model;
using Lpp.Data;

namespace Lpp.Dns.Model
{
    public static class ObjectKinds
    {
        public static readonly SecurityObjectKind Crud = Sec.ObjectKind( "CRUD" );
    }
}