
namespace Lpp.Audit.UI
{
    public interface IAuditEventFilterUIFactory
    {
        IAuditEventFilterFactory FilterFactory { get; }
        bool AppliesToEventKind( AuditEventKind k );
        ClientControlDisplay RenderFilterUI( AuditEventKind eventKind, IAuditEventFilter initialState );

        IAuditEventFilter ParsePostedValue( string value );
    }
}