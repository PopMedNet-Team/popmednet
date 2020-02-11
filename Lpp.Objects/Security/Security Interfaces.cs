using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Security
{
    /****
     * These are slightly modified versions of legacy security interfaces needed to help with migration to new security.
     * Can remove at some point in future code refactoring.
     * **/

    public sealed class SecurityObjectKind
    {

        public SecurityObjectKind() { }

        public SecurityObjectKind(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public override string ToString()
        {
 	        return Name;
        }
    }

    public interface ISecurityObject
    {
        Guid ID { get; }
        SecurityObjectKind Kind { get; }        
    }

    public interface ISecuritySubject
    {
        Guid ID { get; }
        string DisplayName { get; }
    }

    public interface ISecurityObjectProvider<TDomain>
    {
        ISecurityObject Find(Guid id);
        SecurityObjectKind Kind { get; }
    }

    public interface ISecuritySubjectProvider<TDomain>
    {
        ISecuritySubject Find(Guid id);
    }

    public interface IPermissionDefinition
    {
        Guid ID { get; set; }
    }
}
