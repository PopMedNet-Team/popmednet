using System;
using System.Collections.Generic;
using Lpp.Security;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using Lpp.Data.Composition;

namespace Lpp.Dns.Model
{
    [Table("RequestSharedFolders")]
    public class RequestSharedFolder : ISecurityObject
    {
        public static readonly SecurityObjectKind ObjectKind = Sec.ObjectKind("RequestSharedFolder");

        public RequestSharedFolder()
        {
            this.Requests = new HashSet<Request>();
            this.SID = UserDefinedFunctions.NewGuid();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public Guid SID { get; set; }
        SecurityObjectKind ISecurityObject.Kind { get { return ObjectKind; } }
        public override string ToString() { return Name; }

        public virtual ICollection<Request> Requests { get; set; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class RequestSharedFolderPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            builder.Entity<RequestSharedFolder>();
        }
    }

}