//using Lpp.Data.Composition;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Lpp.FileSystem
{
    //[Table("Files")]
    public class File
    {
        public File()
        {
            this.NumberOfSegments = 0;
            this.SegmentSize = 0x400000; // 4Mb
            this.LastSegmentFill = 0;
            this.Segments = new HashSet<FileSegment>();
            this.FileName = "";
            this.Created = DateTime.Now;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Mimetype { get; set; }
        public DateTime Created { get; set; }
        public int SegmentSize { get; set; }
        public int NumberOfSegments { get; set; }
        public int LastSegmentFill { get; set; }
        public virtual ICollection<FileSegment> Segments { get; set; }
        [NotMapped]
        public long Size { get { return Math.Max(0, (NumberOfSegments - 1)) * SegmentSize + LastSegmentFill; } }
    }

    //public class FilePersistence<TDomain> : IPersistenceDefinition<TDomain>
    //{
    //    public void BuildModel(DbModelBuilder b)
    //    {
    //        b.Entity<File>().HasMany(d => d.Segments).WithRequired(t => t.File).HasForeignKey(s => s.FileId).WillCascadeOnDelete();
    //    }
    //}

}