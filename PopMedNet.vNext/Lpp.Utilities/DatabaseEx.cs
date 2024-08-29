using Lpp.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Lpp.Utilities
{
    public static class DatabaseEx
    {
        public static async Task LoadCollection<TEntity, TElement>(this DbContext dbContext, TEntity entity, Expression<Func<TEntity, ICollection<TElement>>> collection) 
        
            where TEntity : class
            where TElement: class
        {
            if (!dbContext.Entry(entity).Collection(collection).IsLoaded)
                await dbContext.Entry(entity).Collection(collection).LoadAsync();
        }

        public static async Task LoadReference<TEntity, TProperty>(this DbContext dbContext, TEntity entity, Expression<Func<TEntity, TProperty>> reference)
            where TEntity : class
            where TProperty : class
        {
            if (!dbContext.Entry(entity).Reference(reference).IsLoaded)
                await dbContext.Entry(entity).Reference(reference).LoadAsync();
        }

        public static IEnumerable<t> DistinctBy<t>(this IEnumerable<t> list, Func<t, object> propertySelector)
        {
            return list.GroupBy(propertySelector).Select(x => x.First());
        }

        /// <summary>
        /// Gets a new sequential GUID that can be stored in a primary key
        /// </summary>
        /// <returns></returns>
        public static Guid NewGuid()
        {
            byte[] guidArray = System.Guid.NewGuid().ToByteArray();

            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;

            // Get the days and milliseconds which will be used to build the byte string 
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = new TimeSpan(now.Ticks - (new DateTime(now.Year, now.Month, now.Day).Ticks));

            // Convert to a byte array 
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333 
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering 
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid 
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new System.Guid(guidArray);
        }
    }
}
