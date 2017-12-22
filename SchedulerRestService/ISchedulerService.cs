using System.ServiceModel;

namespace Lpp.Dns.Scheduler
{
    [ServiceContract(Namespace="http://lincolnpeak.com/schemas/DNS4/API")] 
    public interface ISchedulerService
    {
        [OperationContract]
        void SubmitSchedulerRequest(string userId, string requestId);
    }
}
