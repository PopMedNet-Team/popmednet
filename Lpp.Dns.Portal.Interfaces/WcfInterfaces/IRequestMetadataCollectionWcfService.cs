using System.ServiceModel;
namespace Lpp.Dns.Portal
{
    [ServiceContract( Namespace="http://lincolnpeak.com/schemas/DNS4/RequestMetadataCollection" )]
    public interface IRequestMetadataCollectionWcfService
    {
        [OperationContract]
        void RequestMetadata();
    }
}