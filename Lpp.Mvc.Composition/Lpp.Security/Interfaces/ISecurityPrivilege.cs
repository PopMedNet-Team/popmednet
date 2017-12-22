using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Lpp.Security
{
    [ContractClass( typeof( Contracts.ISecurityPrivilegeContract ) )]
    public interface ISecurityPrivilege
    {
        Guid SID { get; }
        string Name { get; }
        IEnumerable<SecurityTargetKind> ApplicableTo { get; }
    }

    namespace Contracts
    {
        [ContractClassFor( typeof( ISecurityPrivilege ) )]
        abstract class ISecurityPrivilegeContract : ISecurityPrivilege
        {
            public Guid SID
            {
                get { throw new NotImplementedException(); }
            }

            public string Name
            {
                get
                {
                    Contract.Ensures( !String.IsNullOrEmpty( Contract.Result<string>() ) );
                    throw new NotImplementedException();
                }
            }

            public IEnumerable<SecurityTargetKind> ApplicableTo
            {
                get
                {
                    Contract.Ensures( Contract.Result<IEnumerable<SecurityTargetKind>>() != null );
                    Contract.Ensures( Contract.Result<IEnumerable<SecurityTargetKind>>().Any() );
                    throw new NotImplementedException();
                }
            }
        }

    }
}