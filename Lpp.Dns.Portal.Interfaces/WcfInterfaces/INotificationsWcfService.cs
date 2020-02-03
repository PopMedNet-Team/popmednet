using System.ServiceModel;
namespace Lpp.Dns.Portal
{
    [ServiceContract( Namespace="http://lincolnpeak.com/schemas/DNS4/Notifications" )]
    public interface INotificationsWcfService
    {
        [OperationContract]
        void ProcessNotifications();
    }
}