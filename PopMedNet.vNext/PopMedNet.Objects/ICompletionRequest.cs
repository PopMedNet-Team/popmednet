using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PopMedNet.Objects
{
    public interface ICompletionRequest<TDto>
        where TDto : EntityDtoWithID, new()
    {
        Guid? DemandActivityResultID { get; set; }
        TDto Dto { get; set; }
        string Data { get; set; }
        string Comment { get; set; }
    }
}
