using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lpp.Composition.Modules;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.Contracts;

namespace Lpp.Audit
{
    public class AuditEventKind : IComparable<AuditEventKind>, IEquatable<AuditEventKind>
    {
        public Guid ID { get; private set; }
        public string Name { get; private set; }
        public Security.SecurityTargetKind AppliesTo { get; private set; }
        public IEnumerable<IAuditProperty> Properties { get; private set; }

        public AuditEventKind( Guid id, string name, Security.SecurityTargetKind appliesTo, IEnumerable<IAuditProperty> properties )
        {
            //Contract.Requires( properties != null );
            //Contract.Requires( !String.IsNullOrEmpty( name ) );
            //Contract.Requires( appliesTo != null );
            AppliesTo = appliesTo;
            ID = id;
            Name = name;
            Properties = properties;
        }

        public bool Equals( AuditEventKind other ) 
        {
            return CompareTo( other ) == 0; 
        }

        public int CompareTo( AuditEventKind other ) 
        {
            return ReferenceEquals( other, null ) ? 1 : ID.CompareTo( other.ID ); 
        }

        public override bool Equals( object obj ) 
        {
            return Equals( obj as AuditEventKind ); 
        }

        public override int GetHashCode() 
        { 
            return ID.GetHashCode(); 
        }

        public static bool operator == ( AuditEventKind a, AuditEventKind b ) 
        {
            return ReferenceEquals( a, null ) ? ReferenceEquals( b, null ) : a.Equals( b ); 
        }

        public static bool operator != ( AuditEventKind a, AuditEventKind b ) 
        {
            return ReferenceEquals( a, null ) ? !ReferenceEquals( b, null ) : !a.Equals( b ); 
        }
    }
}