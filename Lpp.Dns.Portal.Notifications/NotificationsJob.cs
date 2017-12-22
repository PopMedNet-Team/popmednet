using Common.Logging;

namespace Lpp.Dns.Portal.Notifications
{
    public class NotificationsJob : PokeJob<INotificationsWcfService>
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(NotificationsJob));
        public NotificationsJob() : base( Properties.Settings.Default.NotificationsWcfServiceUrl ) { }
        protected override void Poke( INotificationsWcfService s ) { s.ProcessNotifications(); }
    }
}