using Lpp.Objects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities.Objects
{
    public static class EntityHelpers
    {
        /// <summary>
        /// Updates the fields of this object from the DTO passed to it.
        /// </summary>
        /// <param name="dto">The DTO to update the information from</param>
        /// <param name="skipDTOProperties"></param>
        [Obsolete]
        public static void SetFieldsFromDTO<TEntity, TDto, TDataContext>(this TEntity entity, TDto dto, TDataContext dbContext, params string[] skipDTOProperties)
            where TEntity : EntityWithID
            where TDto : EntityDtoWithID
            where TDataContext : DbContext
        {
            var DTOProperties = dto.GetType().GetProperties();
            var entry = dbContext.Entry(entity);

            foreach (var PropertyName in entry.CurrentValues.PropertyNames.Where(pn => pn != "ID" && pn != "TimeStamp" && pn != "CreatedOn" && !skipDTOProperties.Contains(pn)))
            {
                var property = DTOProperties.FirstOrDefault(p => p.Name == PropertyName);

                if (property != null)
                {
                    if (typeof(Enum).IsAssignableFrom(property.PropertyType))
                    {
                        if (property.GetValue(dto).IsNull())
                        {
                            entry.CurrentValues[PropertyName] = null;
                        }
                        else
                        {
                            entry.CurrentValues[PropertyName] = Convert.ChangeType(Enum.ToObject(property.PropertyType, property.GetValue(dto).ToInt32()), property.PropertyType);
                        }
                    }
                    else
                    {
                        entry.CurrentValues[PropertyName] = property.GetValue(dto);
                    }
                }
            }
        }

        [Obsolete]
        public static IQueryable<TDto> ReturnDTO<TEntity, TDto>(this IQueryable<TEntity> query)
            where TDto : EntityDto, new()
            where TEntity : Entity
        {

            var sourceProperties = typeof(TEntity).GetProperties().Where(p => p.CanRead);

            var destinationProperties = typeof(TDto).GetProperties().Where(p => p.CanWrite);

            var itemParam = Expression.Parameter(typeof(TEntity), "item");

            var propertyMap = from s in sourceProperties join d in destinationProperties on s.Name equals d.Name where s.GetType().FullName == d.GetType().FullName select new { Source = s, Dest = d };

            var memberBindings = propertyMap.Select(p => (MemberBinding)Expression.Bind(p.Dest, Expression.Property(itemParam, p.Source)));
            var newExpression = Expression.New(typeof(TDto));
            var memberInitExpression = Expression.MemberInit(newExpression, memberBindings);
            var projection = Expression.Lambda<Func<TEntity, TDto>>(memberInitExpression, itemParam);

            return query.Select(projection);
        }

        /// <summary>
        /// Returns a DTO from a single entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Obsolete]
        public static TDto ReturnDTO<TEntity, TDto>(this TEntity entity)
            where TDto : EntityDtoWithID, new()
            where TEntity : EntityWithID
        {
            var sourceProperties = typeof(TEntity).GetProperties().Where(p => p.CanRead);
            var destinationProperties = typeof(TDto).GetProperties().Where(p => p.CanWrite);

            var propertyMap = from s in sourceProperties join d in destinationProperties on s.Name equals d.Name select new { Source = s, Dest = d };

            var dto = new TDto();
            foreach (var property in propertyMap)
            {
                property.Dest.SetValue(dto, property.Source.GetValue(entity));
            }

            return dto;
        }
    }
}
