using Lpp.Data.Composition;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lpp.Dns.Model
{
    [Table("Documents")]
    public partial class Document
    {
        public static string DocumentKind_Request = "42479CA9-009D-4483-904C-89E9E7CF436E";
        public static string DocumentKind_User = "CD9DBBC7-AC17-48C7-ABBB-2D9ADD17E158";
        public static string DocumentKind_Link = "CA2F3102-1725-4C17-84FB-B8C62C460CC5";

        public Document()
        {
            this.ID = UserDefinedFunctions.NewGuid();
        }

        [Key]
        public Guid ID { get; set; }

        [Column(TypeName = "varchar"), MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255), Required, Index]
        public string FileName { get; set; }

        public bool IsViewable { get; set; }

        [MaxLength(50)]
        public string Kind { get; set; }

        [Required, MaxLength(100)]
        public string MimeType { get; set; }

        [NotMapped]
        private byte[] _data;

        public byte[] Data
        {
            get { return _data; }
            set
            {
                _data = value;

                Length = value.Length;
            }
        }

        public DateTime CreatedOn { get; set; }

        public long Length { get; set; }

        public Guid ItemID { get; set; }
    }

    [Export(typeof(IPersistenceDefinition<DnsDomain>))]
    public class DocumentPersistence : IPersistenceDefinition<DnsDomain>
    {
        public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        {
            var docs = builder.Entity<Document>();
        }
    }
}