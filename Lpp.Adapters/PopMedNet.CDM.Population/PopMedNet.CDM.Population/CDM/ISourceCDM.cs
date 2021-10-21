using System.Threading;
using System.Threading.Tasks;

namespace PopMedNet.CDM.Population.CDM
{
    public interface ISourceCDM
    {
        Task InitializeAsync();
        Task<DbSchema[]> RetrieveSourceRecordsAsync(CancellationToken cancellationToken);
    }
}
