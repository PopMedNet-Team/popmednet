using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Objects
{
    public interface ICompletionResponse<TDto>
        where TDto : EntityDtoWithID, new()
    {
        string Uri { get; set; }
        TDto Entity { get; set; }
    }
}
