using Lpp.Objects;
using Lpp.Utilities.Objects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities
{
    public abstract class EntityMappingConfiguration {}

    public abstract class EntityMappingConfiguration<TEntity, TDto> : EntityMappingConfiguration
        where TEntity : class
        where TDto: class, new()
    {

        public virtual Expression<Func<TEntity, TDto>> MapExpression
        {
            get
            {
                return null;
            }
        }
       

        public virtual string[] SkipPropertiesOnApply
        {
            get { return new string[] { }; }
        }

        public virtual Action<TDto, DbEntityEntry<TEntity>, PropertyInfo, object> ApplyProperty
        {
            get
            {
                return null;
            }
        }
    }

    public static class EntityMappingHelpers
    {
        static List<EntityMappingConfiguration> mappings = null;

        /// <summary>
        /// Mapps a list of objects from a source entity to a destination DTO using a custom mapping if available. Otherwise it defaults to mapping by Property Name.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IQueryable<TDestination> Map<TSource, TDestination>(this IQueryable<TSource> query)
            where TSource : class
            where TDestination : class, new()
        {
            var mapping = FindMappingConfiguration<TSource, TDestination>();

            if (mapping != null)
                return query.Select(mapping.MapExpression);

            var sourceProperties = typeof(TSource).GetProperties().Where(p => p.CanRead);

            var destinationProperties = typeof(TDestination).GetProperties().Where(p => p.CanWrite);

            var itemParam = Expression.Parameter(typeof(TSource), "item");

            var propertyMap = from s in sourceProperties join d in destinationProperties on new {s.Name, s.GetType().FullName} equals new {d.Name, d.GetType().FullName}  select new { Source = s, Dest = d };
            //TODO: for BUG need to add a cast to nullable if the source is not nullable but the destination is, may also need convert between datetime and datetimeoffset
            var memberBindings = propertyMap.Select(p => (MemberBinding)Expression.Bind(p.Dest, Expression.Property(itemParam, p.Source)));
            var newExpression = Expression.New(typeof(TDestination));
            var memberInitExpression = Expression.MemberInit(newExpression, memberBindings);
            var projection = Expression.Lambda<Func<TSource, TDestination>>(memberInitExpression, itemParam);

            return query.Select(projection);
        }

        /// <summary>
        /// Maps a source entity to a destination DTO using a custom mapping if available. Otherwise it defaults to mapping by Property Name.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static TDestination Map<TSource, TDestination>(this TSource entity)
            where TSource : class
            where TDestination : class, new()
        {
            var mapping = FindMappingConfiguration<TSource, TDestination>();

            if (mapping != null)
                return mapping.MapExpression.Compile()(entity);

            var sourceProperties = typeof(TSource).GetProperties().Where(p => p.CanRead);
            var destinationProperties = typeof(TDestination).GetProperties().Where(p => p.CanWrite);

            var propertyMap = from s in sourceProperties join d in destinationProperties on s.Name equals d.Name select new { Source = s, Dest = d };

            var dto = new TDestination();
            foreach (var property in propertyMap)
            {
                property.Dest.SetValue(dto, property.Source.GetValue(entity));
            }

            return dto;
        }

        /// <summary>
        /// Applies a dto's properties to an existing entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="dto"></param>
        /// <param name="entry"></param>
        public static void Apply<TEntity, TDto>(this TDto dto, DbEntityEntry<TEntity> entry)
            where TEntity : Entity
            where TDto : EntityDto, new()
        {
            
            var mapping = FindMappingConfiguration<TEntity, TDto>();
            if (mapping == null)
                throw new InvalidOperationException("There is no mapping between types " + typeof(TEntity).FullName + " and " + typeof(TDto).FullName + ".");

            var DTOProperties = dto.GetType().GetProperties();

            var skipDTOProperties = mapping.SkipPropertiesOnApply;

            foreach (var PropertyName in entry.CurrentValues.PropertyNames.Where(pn => pn != "ID" && pn != "TimeStamp" && pn != "CreatedOn" && !skipDTOProperties.Contains(pn)))
            {
                var property = DTOProperties.FirstOrDefault(p => p.Name == PropertyName);

                if (property == null)
                    continue;

                if (mapping.ApplyProperty != null)
                {
                    mapping.ApplyProperty(dto, entry, property, property.GetValue(dto));
                }
                else
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
                    else if ( (property.PropertyType == typeof(DateTimeOffset) || property.PropertyType == typeof(DateTimeOffset?))

                        && (entry.Entity.GetType().GetProperty(PropertyName).PropertyType == typeof(DateTime) || entry.Entity.GetType().GetProperty(PropertyName).PropertyType == typeof(DateTime?)))
                    {
                        entry.CurrentValues[PropertyName] = property.GetValue(dto) == null ? (DateTime?) null : ((DateTimeOffset) property.GetValue(dto)).UtcDateTime;
                    }
                    else
                    {
                        entry.CurrentValues[PropertyName] = property.GetValue(dto);
                    }
                }
            }
        }

        private static EntityMappingConfiguration<TSource, TDestination> FindMappingConfiguration<TSource, TDestination>()
            where TSource : class
            where TDestination : class, new()
        {

            if (mappings == null)
            {
                mappings = new List<EntityMappingConfiguration>();
                var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.GetName().FullName.StartsWith("System."));
                mappings.AddRange(assemblies.SelectMany(a => a.GetTypes().Where(type => type.BaseType != null &&
                            type.BaseType.IsGenericType &&
                            type.BaseType.GetGenericTypeDefinition() == typeof(EntityMappingConfiguration<,>)).Select(t => (EntityMappingConfiguration)Activator.CreateInstance(t))));
            }

            var mapping = mappings.ToArray().FirstOrDefault(m => m != null && m.GetType().BaseType == typeof(EntityMappingConfiguration<TSource, TDestination>));

            if (mapping == null)
                return null;
                
            
            return (EntityMappingConfiguration<TSource, TDestination>) mapping;
        }
    }
}
