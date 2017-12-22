using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Lpp;
//using Lpp.Data;
using Lpp.Composition;
//using Lpp.Data.Composition;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lpp.Security.Data.Tuples
{
    //TO_REMOVE: Not used in 4.04 and will be removed in future

    //static class Tuples
    //{

        //public class Persistence<TDomain> : IPersistenceDefinition<TDomain>
        //{
        //    public void BuildModel(System.Data.Entity.DbModelBuilder builder)
        //    {

        //        builder.Entity<Tuple1>().HasKey(t => new { t.Id1, t.SubjectId, t.PrivilegeId });
        //        builder.Entity<Tuple1>().Map(m => m.ToTable("Security_Tuple1"));

        //        builder.Entity<Tuple2>().HasKey(t => new { t.Id1, t.Id2, t.SubjectId, t.PrivilegeId });
        //        builder.Entity<Tuple2>().Map(m => m.ToTable("Security_Tuple2"));

        //        builder.Entity<Tuple3>().HasKey(t => new { t.Id1, t.Id2, t.Id3, t.SubjectId, t.PrivilegeId });
        //        builder.Entity<Tuple3>().Map(m => m.ToTable("Security_Tuple3"));

        //        builder.Entity<Tuple4>().HasKey(t => new { t.Id1, t.Id2, t.Id3, t.Id4, t.SubjectId, t.PrivilegeId });
        //        builder.Entity<Tuple4>().Map(m => m.ToTable("Security_Tuple4"));
        //    }
        //}

        //public static IQueryable<TargetWithAclEntry> GetAllTuplesWithAcls<TDomain>(ICompositionService comp, int arity)
        //{
        //    switch (arity)
        //    {

        //        case 1: return comp.Get<IRepository<TDomain, Tuple1>>()
        //                                    .All
        //                                    .Select(t => new TargetWithAclEntry
        //                                    {
        //                                        TargetId = new BigTuple<Guid> { X0 = t.Id1 },
        //                                        SourceTargetId = new BigTuple<Guid> { X0 = t.ParentId1 },
        //                                        IsInherited = t.Id1 != t.ParentId1,
        //                                        PrivilegeId = t.PrivilegeId,
        //                                        SubjectId = t.SubjectId,
        //                                        ViaMembership = t.ViaMembership > 0,
        //                                        Allow = t.DeniedEntries == 0,
        //                                        ExplicitDeny = t.ExplicitDeniedEntries > 0,
        //                                        ExplicitAllow = t.ExplicitAllowedEntries > 0
        //                                    });

        //        case 2: return comp.Get<IRepository<TDomain, Tuple2>>()
        //                                    .All
        //                                    .Select(t => new TargetWithAclEntry
        //                                    {
        //                                        TargetId = new BigTuple<Guid> { X0 = t.Id1, X1 = t.Id2 },
        //                                        SourceTargetId = new BigTuple<Guid> { X0 = t.ParentId1, X1 = t.ParentId2 },
        //                                        IsInherited = t.Id1 != t.ParentId1 || t.Id2 != t.ParentId2,
        //                                        PrivilegeId = t.PrivilegeId,
        //                                        SubjectId = t.SubjectId,
        //                                        ViaMembership = t.ViaMembership > 0,
        //                                        Allow = t.DeniedEntries == 0,
        //                                        ExplicitDeny = t.ExplicitDeniedEntries > 0,
        //                                        ExplicitAllow = t.ExplicitAllowedEntries > 0
        //                                    });

        //        case 3: return comp.Get<IRepository<TDomain, Tuple3>>()
        //                                    .All
        //                                    .Select(t => new TargetWithAclEntry
        //                                    {
        //                                        TargetId = new BigTuple<Guid> { X0 = t.Id1, X1 = t.Id2, X2 = t.Id3 },
        //                                        SourceTargetId = new BigTuple<Guid> { X0 = t.ParentId1, X1 = t.ParentId2, X2 = t.ParentId3 },
        //                                        IsInherited = t.Id1 != t.ParentId1 || t.Id2 != t.ParentId2 || t.Id3 != t.ParentId3,
        //                                        PrivilegeId = t.PrivilegeId,
        //                                        SubjectId = t.SubjectId,
        //                                        ViaMembership = t.ViaMembership > 0,
        //                                        Allow = t.DeniedEntries == 0,
        //                                        ExplicitDeny = t.ExplicitDeniedEntries > 0,
        //                                        ExplicitAllow = t.ExplicitAllowedEntries > 0
        //                                    });

        //        case 4: return comp.Get<IRepository<TDomain, Tuple4>>()
        //                                    .All
        //                                    .Select(t => new TargetWithAclEntry
        //                                    {
        //                                        TargetId = new BigTuple<Guid> { X0 = t.Id1, X1 = t.Id2, X2 = t.Id3, X3 = t.Id4 },
        //                                        SourceTargetId = new BigTuple<Guid> { X0 = t.ParentId1, X1 = t.ParentId2, X2 = t.ParentId3, X3 = t.ParentId4 },
        //                                        IsInherited = t.Id1 != t.ParentId1 || t.Id2 != t.ParentId2 || t.Id3 != t.ParentId3 || t.Id4 != t.ParentId4,
        //                                        PrivilegeId = t.PrivilegeId,
        //                                        SubjectId = t.SubjectId,
        //                                        ViaMembership = t.ViaMembership > 0,
        //                                        Allow = t.DeniedEntries == 0,
        //                                        ExplicitDeny = t.ExplicitDeniedEntries > 0,
        //                                        ExplicitAllow = t.ExplicitAllowedEntries > 0
        //                                    });

        //    }
        //    throw new NotSupportedException("Security Object tuples of size " + arity + " are not supported.");
        //}
    //}

    //[Table("Security_Tuple1")]
    //public class Tuple1
    //{
    //    [Key, Column(Order = 0)]
    //    public Guid Id1 { get; set; }
    //    [Key, Column(Order = 1)]
    //    public Guid ParentId1 { get; set; }
    //    [Key, Column(Order = 2)]
    //    public Guid SubjectId { get; set; }
    //    [Key, Column(Order = 3)]
    //    public Guid PrivilegeId { get; set; }
    //    [Key, Column(Order = 4)]
    //    public int ViaMembership { get; set; }
    //    [Key, Column(Order = 5)]
    //    public int DeniedEntries { get; set; }
    //    [Key, Column(Order = 6)]
    //    public int ExplicitDeniedEntries { get; set; }
    //    [Key, Column(Order = 7)]
    //    public int ExplicitAllowedEntries { get; set; }
    //}

    //[Table("Security_Tuple2")]
    //public class Tuple2
    //{
    //    [Key, Column(Order = 0)]
    //    public Guid Id1 { get; set; }
    //    [Key, Column(Order = 1)]
    //    public Guid Id2 { get; set; }
    //    [Key, Column(Order = 2)]
    //    public Guid ParentId1 { get; set; }
    //    [Key, Column(Order = 3)]
    //    public Guid ParentId2 { get; set; }
    //    [Key, Column(Order = 4)]
    //    public Guid SubjectId { get; set; }
    //    [Key, Column(Order = 5)]
    //    public Guid PrivilegeId { get; set; }
    //    [Key, Column(Order = 6)]
    //    public int ViaMembership { get; set; }
    //    [Key, Column(Order = 7)]
    //    public int DeniedEntries { get; set; }
    //    [Key, Column(Order = 8)]
    //    public int ExplicitDeniedEntries { get; set; }
    //    [Key, Column(Order = 9)]
    //    public int ExplicitAllowedEntries { get; set; }
    //}

    //[Table("Security_Tuple3")]
    //public class Tuple3
    //{
    //    [Key, Column(Order = 0)]
    //    public Guid Id1 { get; set; }
    //    [Key, Column(Order = 1)]
    //    public Guid Id2 { get; set; }
    //    [Key, Column(Order = 2)]
    //    public Guid Id3 { get; set; }
    //    [Key, Column(Order = 3)]
    //    public Guid ParentId1 { get; set; }
    //    [Key, Column(Order = 4)]
    //    public Guid ParentId2 { get; set; }
    //    [Key, Column(Order = 5)]
    //    public Guid ParentId3 { get; set; }
    //    [Key, Column(Order = 6)]
    //    public Guid SubjectId { get; set; }
    //    [Key, Column(Order = 7)]
    //    public Guid PrivilegeId { get; set; }
    //    [Key, Column(Order = 8)]
    //    public int ViaMembership { get; set; }
    //    [Key, Column(Order = 9)]
    //    public int DeniedEntries { get; set; }
    //    [Key, Column(Order = 10)]
    //    public int ExplicitDeniedEntries { get; set; }
    //    [Key, Column(Order = 11)]
    //    public int ExplicitAllowedEntries { get; set; }
    //}

    //[Table("Security_Tuple4")]
    //public class Tuple4
    //{
    //    [Key, Column(Order = 0)]
    //    public Guid Id1 { get; set; }
    //    [Key, Column(Order = 1)]
    //    public Guid Id2 { get; set; }
    //    [Key, Column(Order = 2)]
    //    public Guid Id3 { get; set; }
    //    [Key, Column(Order = 3)]
    //    public Guid Id4 { get; set; }
    //    [Key, Column(Order = 4)]
    //    public Guid ParentId1 { get; set; }
    //    [Key, Column(Order = 5)]
    //    public Guid ParentId2 { get; set; }
    //    [Key, Column(Order = 6)]
    //    public Guid ParentId3 { get; set; }
    //    [Key, Column(Order = 7)]
    //    public Guid ParentId4 { get; set; }
    //    [Key, Column(Order = 8)]
    //    public Guid SubjectId { get; set; }
    //    [Key, Column(Order = 9)]
    //    public Guid PrivilegeId { get; set; }
    //    [Key, Column(Order = 10)]
    //    public int ViaMembership { get; set; }
    //    [Key, Column(Order = 11)]
    //    public int DeniedEntries { get; set; }
    //    [Key, Column(Order = 12)]
    //    public int ExplicitDeniedEntries { get; set; }
    //    [Key, Column(Order = 13)]
    //    public int ExplicitAllowedEntries { get; set; }
    //}
}
