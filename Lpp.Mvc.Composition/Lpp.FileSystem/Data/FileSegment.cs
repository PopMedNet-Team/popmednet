using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using Lpp.Composition;
//using Lpp.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using Lpp.Data.Composition;
using System.Data.Entity;

namespace Lpp.FileSystem
{
    public class FileSegment
    {
        [Key, Column(Order = 1)]
        public int FileId { get; set; }
        public virtual File File { get; set; }
        [Key, Column(Order = 2)]
        public int Index { get; set; }
        public byte[] Data { get; set; }
    }

    //public class FileSegmentPersistence<TDomain> : IPersistenceDefinition<TDomain>
    //{
    //    public void BuildModel(DbModelBuilder b)
    //    {
    //        b.Entity<FileSegment>();
    //    }
    //}

}