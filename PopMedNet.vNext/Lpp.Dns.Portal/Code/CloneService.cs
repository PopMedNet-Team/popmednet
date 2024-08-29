using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Lpp.Dns.Model;
using System.ComponentModel.Composition;
using Lpp.Composition;
//using Lpp.Data;
using Lpp.Security;
using Lpp.Security.UI;
using System.Reflection;
using Lpp.Dns.Data;

namespace Lpp.Dns.Portal
{
    /// <summary>
    /// Base implmentation of ICloneService.
    /// </summary>
    /// <typeparam name="T">The object type to clone.</typeparam>
    internal abstract class CloneService<T> : ICloneService<T> where T : class, ISecurityObject , new()
    {
        [Import]
        internal DnsAclService DnsAclService { get; set; }
        [Import]
        internal ISecurityObjectHierarchyService<Lpp.Dns.Model.DnsDomain> SecurityHierarchy { get; set; }
        [Import]
        internal ISecurityService<Lpp.Dns.Model.DnsDomain> SecurityService { get; set; }

        //[Import]
        //public IUnitOfWork<DnsDomain> UnitOfWork { get; set; }

        /// <summary>
        /// Clone the specified object to a new object.
        /// </summary>
        /// <param name="existing">The object to clone.</param>
        /// <returns></returns>
        public abstract T Clone(T existing);

        /// <summary>
        /// Copies all the public properties of one object to another.
        /// </summary>
        /// <typeparam name="Tx">The type of object being copied.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="destination">The destination object.</param>
        /// <param name="skipProperties">A collection of properties to NOT copy.</param>
        protected virtual void CopyProperties<Tx>(Tx source, Tx destination, params string[] skipProperties)
        {
            var props = typeof(Tx).GetProperties();

            foreach (PropertyInfo propertyInfo in props)
            {
                if (skipProperties.Contains(propertyInfo.Name))
                    continue;

                object value = propertyInfo.GetValue(source, null);
                propertyInfo.SetValue(destination, value, null);
            }
        }

        /// <summary>
        /// Duplicates the existing string value and appends (Copy X) to the end of it, where X is the number of copies of the value.
        /// </summary>
        /// <param name="existing">The string value to copy.</param>
        /// <param name="checkExisting">A function that returns the number of already existing copied names that exist.</param>
        /// <returns></returns>
        protected virtual string CopyName(string existing, Func<int, int> checkExisting)
        {
            int index = existing.IndexOf("(Copy");
            if (index < 0)
            {
                index = existing.Length;
            }

            int count = checkExisting(index);

            return existing.Substring(0, index) + " (Copy " + count + ")";
        }

        /// <summary>
        /// Filters a collection of AclEntries and removes inherited rules.
        /// </summary>
        /// <param name="existingEntries">The collection of AclEntries to filter.</param>
        /// <returns></returns>
        protected static ILookup<BigTuple<Guid>, AclEntry> FilterForNonInheritedEntries(ILookup<BigTuple<Guid>, AnnotatedAclEntry> existingEntries)
        {
            return (from t in existingEntries
                    let target = t.Key
                    from e in t.Where(tt => tt.Entry.Subject != null)
                    where e.InheritedFrom == null
                    select new
                    {
                        target,
                        e.Entry
                    }).ToLookup(x => x.target, x => x.Entry);
        }
    }
}
