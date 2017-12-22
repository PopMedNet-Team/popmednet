using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using Lpp.Composition;
//using Xunit;
using Lpp.Dns.Model;
using System.Linq.Expressions;

namespace Lpp.Dns.Tests
{
    //public static class TestExtensions
    //{
    //    public static IQueryable<T> Entities<T>( this ICompositionService comp ) where T : class
    //    {
    //        throw new Lpp.Utilities.CodeToBeUpdatedException();

    //        //return comp.Get<IRepository<DnsDomain, T>>().All;
    //    }

    //    public static T ByName<T>( this IEnumerable<T> ts, string name ) where T : Lpp.Objects.IEntityWithName
    //    {
    //        return ts.FirstOrDefault( t => t.Name == name );
    //    }

    //    public static T ByName<T>(this IQueryable<T> ts, string name) where T : Lpp.Objects.IEntityWithName
    //    {
    //        var p = Expression.Parameter( typeof( T ) );
    //        var cmp = Expression.Lambda<Func<T,bool>>( Expression.Equal( Expression.Property( p, "Name" ), Expression.Constant( name ) ), p );
    //        return ts.FirstOrDefault( cmp );
    //    }

    //    public static T ByName<T>(this ICompositionService comp, string name) where T : class, Lpp.Objects.IEntityWithName
    //    {
    //        return comp.Entities<T>().ByName( name );
    //    }

    //    public static IEnumerable<T> FromNames<T>(this IEnumerable<string> ts) where T : Lpp.Objects.IEntityWithName, new()
    //    {
    //        return ts.Select( n =>
    //        {
    //            var t = new T();
    //            ((dynamic)t).Name = n;
    //            return t;
    //        } );
    //    }

    //    public static IList<T> InsertEntities<T>( this IEnumerable<T> ts, ICompositionService scope ) where T : class
    //    {
    //        throw new Lpp.Utilities.CodeToBeUpdatedException();
    //        //return ts.Select( scope.Get<IRepository<DnsDomain, T>>().Add ).ToList();
    //    }

    //    public static IList<T> InsertEntities<T>( this IEnumerable<T> ts, TestBase tb ) where T : class
    //    {
    //        return tb.DbTran( scope => ts.InsertEntities( scope ) );
    //    }
    //}
}