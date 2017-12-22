using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Lpp.Composition.Modules;
using System.Linq.Expressions;
using System.Linq;
using System.Xml.Linq;
using Lpp.Audit.Data;
using Lpp.Composition;
using System.ComponentModel.Composition;
using System.ComponentModel;
using System.Reflection;
using Lpp.Utilities.Legacy;

namespace Lpp.Audit
{
    public interface IPropertyFilterComponent
    {
        PropertyValueComparison Comparison { get; }
        object Value { get; }
        IAuditProperty Property { get; }
        Expression<Func<AuditEvent, bool>> Filter { get; }
    }

    public interface IPropertyFilter : IAuditEventFilter
    {
        IEnumerable<IPropertyFilterComponent> Components { get; }
        IPropertyFilter And<TProp>( IAuditProperty<TProp> prop, TProp value, PropertyValueComparison comparison = PropertyValueComparison.Eq );
    }

    class PropertyFilter : IPropertyFilter
    {
        public IAuditEventFilterFactory Factory { get; private set; }
        public IEnumerable<IPropertyFilterComponent> Components { get; private set; }
        public Expression<Func<AuditEvent, bool>> Filter
        {
            get { return Components.Any() ? Components.Select( c => c.Filter ).Fold( Expression.And ) : _ => true; }
        }

        public PropertyFilter( IEnumerable<IPropertyFilterComponent> comps, IAuditEventFilterFactory f )
        {
            Components = comps.EmptyIfNull().ToList().AsReadOnly();
            Factory = f;
        }

        public IPropertyFilter And<TProp>( IAuditProperty<TProp> prop, TProp value, PropertyValueComparison comparison )
        {
            return new PropertyFilter( Components.Concat( new[] { new PropertyFilterComponent<TProp>( prop, value, comparison ) } ), Factory );
        }
    }

    class PropertyFilterComponent<TProp> : IPropertyFilterComponent
    {
        public IAuditProperty Property { get; private set; }
        public PropertyValueComparison Comparison { get; private set; }
        public object Value { get; private set; }
        public Expression<Func<AuditEvent, bool>> Filter { get; private set; }

        public PropertyFilterComponent( IAuditProperty<TProp> prop, TProp value, PropertyValueComparison comparison )
        {
            Property = prop;
            Comparison = comparison;
            Value = value;

            Expression<Func<AuditPropertyValue,bool>> valuePredicate;
            if ( typeof( TProp ) == typeof( string ) && ( comparison == PropertyValueComparison.Contains || comparison == PropertyValueComparison.NotContains ) )
            {
                var asStr = Convert.ToString( value );
                valuePredicate = comparison == PropertyValueComparison.Contains 
                    ? Expr.Create( ( AuditPropertyValue v ) => v.StringValue.Contains( asStr ) ) 
                    : Expr.Create( ( AuditPropertyValue v ) => !v.StringValue.Contains( asStr ) );
            }
            else
            {
                var valueAccessor = AuditPropertyValue.GetAccessorExpression<TProp>();
                valuePredicate = Expression.Lambda<Func<AuditPropertyValue, bool>>(
                    Expression.MakeBinary( Op( Comparison ), valueAccessor.Body, Expression.Constant( Value ) ),
                    valueAccessor.Parameters );
            }

            Filter = e => e.PropertyValues.Any( p => p.PropertyId == Property.ID && valuePredicate.Invoke( p ) );
            Filter = Filter.Expand();
        }

        static ExpressionType Op( PropertyValueComparison c )
        {
            switch ( c )
            {
                case PropertyValueComparison.Eq: return ExpressionType.Equal;
                case PropertyValueComparison.Neq: return ExpressionType.NotEqual;
                case PropertyValueComparison.Gt: return ExpressionType.GreaterThan;
                case PropertyValueComparison.Gte: return ExpressionType.GreaterThanOrEqual;
                case PropertyValueComparison.Lt: return ExpressionType.LessThan;
                case PropertyValueComparison.Lte: return ExpressionType.LessThanOrEqual;
                default: throw new NotSupportedException( "Unsupported comparison type '" + c + "'" );
            }
        }
    }

    public class PropertyFilterFactory<TDomain> : IAuditEventFilterFactory<TDomain>
    {
        [Import] public IAuditService<TDomain> Audit { get; set; }
        [Import] public ICompositionService Composition { get; set; }

        public Guid Id { get; private set; }
        public PropertyFilterFactory( Guid id ) { Id = id; }

        public IPropertyFilter Create<TProp>( IAuditProperty<TProp> prop, TProp value, PropertyValueComparison comparison = PropertyValueComparison.Eq )
        {
            return new PropertyFilter( new[] { new PropertyFilterComponent<TProp>( prop, value, comparison ) }, this );
        }

        public IAuditEventFilter Deserialize( XElement x )
        {
            var comps = from xprop in x.Elements( "Prop" )
                        let comp = from aprop in Maybe.Value( xprop.Attribute( "Id" ) )
                                   from avalue in xprop.Attribute( "Value" )
                                   from acmp in xprop.Attribute( "Cmp" )
                                   from prop in Audit.AllProperties.MaybeGet( (Guid)aprop )
                                   from cmp in (PropertyValueComparison)Enum.Parse( typeof( PropertyValueComparison ), acmp.Value )
                                   select Activator.CreateInstance( typeof( PropertyFilterComponent<> ).MakeGenericType( prop.Type ),
                                             prop, Convert.ChangeType( avalue.Value, prop.Type ), cmp )
                                             as IPropertyFilterComponent
                        where comp.Kind == MaybeKind.Value
                        select comp.Value;

            return new PropertyFilter( comps, this );
        }

        public XElement Serialize( IAuditEventFilter f )
        {
            var pf = f as IPropertyFilter;
            return new XElement( "_",
                from c in pf.Components
                select new XElement( "Prop",
                    new XAttribute( "Id", c.Property.ID ),
                    new XAttribute( "Value", c.Value ),
                    new XAttribute( "Cmp", c.Comparison )
                )
            );
        }
    }

    public enum PropertyValueComparison
    {
        Eq, Neq, Gt, Gte, Lt, Lte, Contains, NotContains
    }
}