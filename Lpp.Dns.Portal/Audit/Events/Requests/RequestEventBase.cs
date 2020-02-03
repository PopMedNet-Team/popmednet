using System;
using System.Runtime.InteropServices;
using Lpp.Audit;
using System.ComponentModel;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal.Events
{
    public abstract class RequestEventBase
    {
        [AudProp( CommonProperties.Request )]
        public Guid Request { get; set; }

        //[AudProp( CommonProperties.RequestType )]
        //public Guid RequestType { get; set; }

        [AudProp( CommonProperties.Project )]
        public Guid Project { get; set; }

        public RequestEventBase( Request r )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();
            //this.Project = (r.Project ?? VirtualSecObjects.AllProjects).SID;
            //this.Request = r.ID;
            //this.RequestType = r.RequestTypeId;
        }
    }
}