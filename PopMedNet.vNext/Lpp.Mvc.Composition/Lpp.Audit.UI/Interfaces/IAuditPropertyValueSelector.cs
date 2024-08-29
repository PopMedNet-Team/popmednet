using System.Collections.Generic;

namespace Lpp.Audit.UI
{
    public interface IAuditPropertyValueSelector<TProperty>
    {
        IEnumerable<IAuditProperty<TProperty>> AppliesTo { get; }
        ClientControlDisplay RenderSelector( TProperty initialState );
        TProperty ParsePostedValue( string value );
    }
}