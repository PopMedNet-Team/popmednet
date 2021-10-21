using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.CDM.Population.CDM
{
    public interface IReplicaCDM
    {
        Task InitializeAsync();
        Task PopulateAsync(DbSchema[] schemas);
    }
}
