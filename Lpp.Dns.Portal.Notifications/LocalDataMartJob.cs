namespace Lpp.Dns.Portal.Notifications
{
    public class LocalDataMartJob : PokeJob<ILocalDataMartWcfService>
    {
        public LocalDataMartJob() : base( Properties.Settings.Default.LocalDataMartWcfServiceUrl ) { }
        protected override void Poke( ILocalDataMartWcfService s ) { s.ProcessRequests(); }
    }
}