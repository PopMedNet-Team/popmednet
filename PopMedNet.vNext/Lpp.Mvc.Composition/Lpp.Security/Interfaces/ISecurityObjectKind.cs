using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Lpp.Security
{
    [ContractClass( typeof( Contracts.ISecurityObjectKindContract ) )]
    public interface ISecurityObjectKind
    {
        string Name { get; }
    }

    namespace Contracts
    {
        [ContractClassFor( typeof( ISecurityObjectKind ) )]
        abstract class ISecurityObjectKindContract : ISecurityObjectKind
        {
            public string Name
            {
                get
                {
                    Contract.Ensures( !String.IsNullOrEmpty( Contract.Result<string>() ) );
                    throw new NotImplementedException();
                }
            }
        }
    }
}