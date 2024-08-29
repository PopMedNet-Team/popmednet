using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lpp.Composition;
using System.Linq.Expressions;
using Lpp.Utilities.Legacy;

namespace Lpp.Audit.UI
{
    public class FormatEventVisualizer : IAuditEventVisualizer
    {
        public FormatEventVisualizer( Guid scopeId, AuditEventKind appliesTo, string format, params IAuditProperty[] formatArgs )
            : this( scopeId, new[] { appliesTo }, format, formatArgs )
        {
        }

        public FormatEventVisualizer( Guid scopeId, IEnumerable<AuditEventKind> appliesTo, string format, params IAuditProperty[] formatArgs )
        {
            //Contract.Requires( formatArgs != null );
            //Contract.Requires( !String.IsNullOrEmpty( format ) );
            //Contract.Requires( string.Format( format, formatArgs ) != null ); // This is here to make sure that the format string is correct
            //Contract.Requires( formatArgs.All( a => a != null ) );

            ScopeId = scopeId;
            AppliesToKinds = appliesTo;
            _format = format;
            _formatArgs = formatArgs;
        }

        private readonly string _format;
        private readonly IAuditProperty[] _formatArgs;
        public Guid ScopeId { get; private set; }
        public IEnumerable<AuditEventKind> AppliesToKinds { get; private set; }

        public Func<HtmlHelper, IHtmlString> Visualize( AuditEventView ev, AuditEventKind kind, 
            IDictionary<IAuditProperty, Func<HtmlHelper, IHtmlString>> visualizedProperties )
        {
            return html => new MvcHtmlString( string.Format( _format, 
                _formatArgs.Select(
                    a => from na in Maybe.Value( a )
                         from v in visualizedProperties.MaybeGet( na )
                         from str in v( html )
                         select str.ToHtmlString() 
                ) 
                .Select( a => a.ValueOrNull() )
                .Cast<object>()
                .ToArray()
            ) );
        }
    }

    public class FormatEventVisualizer<TEvent> : FormatEventVisualizer
    {
        public FormatEventVisualizer( Guid scopeId, string format, params Expression<Func<TEvent,object>>[] properties )
            : base( scopeId, Aud.Event<TEvent>(), format, properties.Select( Aud.UntypedProperty ).ToArray() )
        {
        }
    }
}