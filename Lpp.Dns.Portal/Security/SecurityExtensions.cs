using System;
using System.Diagnostics.Contracts;
using Lpp.Security;
using Lpp.Audit;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal
{
    public static class SecurityExtensions
    {
        public static ISecurityObject AsSecurityObject( this IDnsRequestType rt )
        {
            return new SecObject( rt.ID, rt.Name, Dns.RequestTypeSecObjectKind );
        }

        public static ISecurityObject AsSecurityObject( this AuditEventKind evt )
        {
            return new SecObject( evt.ID, evt.Name, SecTargetKinds.AuditEventObjectKind );
        }

        public static Lpp.Security.SecurityTarget AsSecurityTarget( this Request r )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return Sec.Target( r.Project ?? VirtualSecObjects.AllProjects, r.Organization, r.CreatedBy );
        }

        public static Lpp.Security.SecurityTarget AsEventTarget( this Request r )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return Sec.Target( r.Project ?? VirtualSecObjects.AllProjects, r.Organization, r.CreatedBy, r );
        }

        public static Lpp.Security.SecurityTarget AsEventTarget( this RequestDataMart r )
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return Sec.Target( r.Request.Project ?? VirtualSecObjects.AllProjects, r.DataMart.Organization, r.DataMart, r.Request );
        }

        class SecObject : ISecurityObject
        {
            private readonly string _name;

            public SecObject( Guid sid, string name, SecurityObjectKind kind ) 
            { 
                ID = sid; 
                _name = name; 
                Kind = kind; 
            }

            public Guid ID { get; private set; }

            public SecurityObjectKind Kind { get; private set; }

            public override string ToString() 
            { 
                return _name; 
            }
        }
    }
}