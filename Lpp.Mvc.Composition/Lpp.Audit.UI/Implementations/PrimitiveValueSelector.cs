using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Lpp.Composition;
using Lpp.Mvc;

namespace Lpp.Audit.UI
{
    public class PrimitiveValueSelector<T> : IAuditPropertyValueSelector<T>
    {
        public IEnumerable<IAuditProperty<T>> AppliesTo { get; private set; }
        public PrimitiveValueSelector( IEnumerable<IAuditProperty<T>> appliesTo ) { AppliesTo = appliesTo; }

        public ClientControlDisplay RenderSelector( T initialState )
        {
            var sv = Convert.ToString( initialState );
            return new ClientControlDisplay
            {
                ValueAsString = sv,
                Render = (html, onChange) => html.Partial<Views.PrimitiveValueSelector>().WithModel( new Models.PrimitiveValueModel
                {
                    Value = sv, OnChangeFunction = onChange
                } )
            };
        }

        public T ParsePostedValue( string value )
        {
            var v = Convert.ChangeType( value, typeof( T ) );
            return v == null ? default( T ) : (T)v;
        }
    }
}