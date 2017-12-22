using System;
using System.Collections.Generic;
using Lpp.Mvc;

namespace Lpp.Audit.UI
{
    public class DateValueSelector : IAuditPropertyValueSelector<DateTime>
    {
        public IEnumerable<IAuditProperty<DateTime>> AppliesTo { get; private set; }
        public DateValueSelector( IEnumerable<IAuditProperty<DateTime>> appliesTo ) { AppliesTo = appliesTo; }

        public ClientControlDisplay RenderSelector( DateTime initialState )
        {
            var sv = initialState.ToString( "MM/dd/yyyy" );
            return new ClientControlDisplay
            {
                ValueAsString = sv,
                Render = (html, onChange) => html.Partial<Views.DateValueSelector>().WithModel( new Models.DateValueModel
                {
                    Value = initialState, OnChangeFunction = onChange
                } )
            };
        }

        public DateTime ParsePostedValue( string value )
        {
            DateTime res;
            return DateTime.TryParse( value, out res ) ? res : default( DateTime );
        }
    }
}