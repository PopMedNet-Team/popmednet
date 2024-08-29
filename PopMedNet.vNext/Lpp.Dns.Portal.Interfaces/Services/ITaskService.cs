using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Lpp.Dns.Portal.Models;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal
{
    [ContractClass( typeof( Contracts.ITaskServiceContract ) )]
    public interface ITaskService
    {
        // TODO Currently no task object, so we simply return the only kind of task we have - user signing up.
        IQueryable<User> GetTasks();
    }

    namespace Contracts
    {
        [ContractClassFor( typeof( ITaskService ) )]
        abstract class ITaskServiceContract : ITaskService
        {
            public IQueryable<User> GetTasks()
            {
                //Contract.Ensures( //Contract.Result<IQueryable<User>>() != null );
                throw new NotImplementedException();
            }
        }
    }
}