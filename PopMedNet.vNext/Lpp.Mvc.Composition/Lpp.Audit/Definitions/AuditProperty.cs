using System;
using System.Diagnostics.Contracts;

namespace Lpp.Audit
{
    class AuditProperty<T> : IAuditProperty<T>, IComparable<IAuditProperty<T>>
    {
        public Guid ID { get; private set; }
        public string Name { get; private set; }
        Type IAuditProperty.Type { get { return typeof( T ); } }

        public AuditProperty( Guid id, string name )
        {
            //Contract.Requires( !String.IsNullOrEmpty( name ) );
            ID = id;
            Name = name;
        }

        public object GetValue( Data.AuditPropertyValue v ) 
        {
            return v.GetValue<T>(); 
        }

        public int CompareTo( IAuditProperty other ) 
        { 
            return ReferenceEquals( other, null ) ? 1 : ID.CompareTo( other.ID ); 
        }
        
        public int CompareTo( IAuditProperty<T> other ) 
        {
            return CompareTo( other as IAuditProperty ); 
        }
        
        public override bool Equals( object obj ) 
        {
            return CompareTo( obj as IAuditProperty ) == 0; 
        }
        
        public override int GetHashCode() 
        {
            return ID.GetHashCode(); 
        }
        
        public static bool operator == ( AuditProperty<T> a, AuditProperty<T> b ) 
        {
            return ReferenceEquals( a, null ) ? ReferenceEquals( b, null ) : a.CompareTo( b ) == 0; 
        }
        
        public static bool operator != ( AuditProperty<T> a, AuditProperty<T> b ) 
        { 
            return ReferenceEquals( a, null ) ? !ReferenceEquals( b, null ) : a.CompareTo( b ) != 0; 
        }
    }
}