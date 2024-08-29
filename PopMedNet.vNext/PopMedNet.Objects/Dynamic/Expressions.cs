using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace PopMedNet.Objects.Dynamic
{
    /// <summary>
    /// Helper class for working with Expressions.
    /// </summary>
    public static class Expressions
    {
        public static readonly MethodInfo StringConcatMethodInfo = typeof(string).GetMethod("Concat", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(string), typeof(string), typeof(string) }, null);

        /// <summary>
        /// Concatenates 3 expressions that provide string values into a single expression.
        /// Equivalent of calling string.Concat(string1, string2, string3).
        /// </summary>
        /// <param name="stringExp1"></param>
        /// <param name="stringExp2"></param>
        /// <param name="stringExp3"></param>
        /// <returns>A MethodCallExpression that joins the strings provided by the parameter expressions.</returns>
        public static MethodCallExpression ConcatStrings(Expression stringExp1, Expression stringExp2, Expression stringExp3)
        {
            return Expression.Call(StringConcatMethodInfo, stringExp1, stringExp2, stringExp3); 
        }

        /// <summary>
        /// Creates a MemberExpression for the specified property on the parameter expressions type.
        /// </summary>
        /// <param name="parameter">The source parameter expression.</param>
        /// <param name="property">The property to bind to.</param>
        /// <returns></returns>
        public static MemberExpression MemberExpression(this ParameterExpression parameter, string property){
            return Expression.Property(parameter, parameter.Type.GetProperty(property));
        }

        /// <summary>
        /// Creates a MemberExpression for a child property of the source type. IE: Key.ID, or Nullable&gt;int>.HasValue, etc..
        /// </summary>
        /// <param name="source">The source expression providing the type to bind to.</param>
        /// <param name="sourceProperty">The property on the source type containing the proprerty utimately wanted.</param>
        /// <param name="innerProperty">The inner property the MemberExpression is for.</param>
        /// <returns></returns>
        public static MemberExpression ChildPropertyExpression(this Expression source, string sourceProperty, string innerProperty)
        {
            MemberExpression sourceExp = Expression.Property(source, source.Type.GetProperty(sourceProperty));
            MemberExpression innerExp = Expression.Property(sourceExp, sourceExp.Type.GetProperty(innerProperty));
            return innerExp;
        }

        /// <summary>
        /// Calls the default no parameter ToString function of the specified type on the supplied expression.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Expression CallToString<T>(Expression expression)
        {
            //TODO: this could likely be changed to look at the expression.Type instead of the generic parameter.
            return Expression.Call(expression, typeof(T).GetMethod("ToString", Type.EmptyTypes));
        }

        /// <summary>
        /// Creates an expression that make sure the value of the string selector expression is not null or string.Empty.
        /// </summary>
        /// <param name="stringSelector">The expression to get the string value.</param>
        /// <returns></returns>
        public static BinaryExpression StringIsNullOrEmpty(Expression stringSelector)
        {
            return Expression.AndAlso(Expression.NotEqual(stringSelector, Expression.Constant((string)null, typeof(string))), Expression.NotEqual(stringSelector, Expression.Constant((string)"", typeof(string))));
        }

        /// <summary>
        /// Calls string.StartsWith on the specified string selector expression and value.
        /// </summary>
        /// <param name="stringSelector">The string selector expression to see if starts with the specified value.</param>
        /// <param name="value">The value the string should start with.</param>
        /// <returns></returns>
        public static BinaryExpression StringStartsWith(Expression stringSelector, string value)
        {
            MethodCallExpression call = Expression.Call(stringSelector, "StartsWith", new[] { typeof(string) }, Expression.Constant(value, typeof(string)));
            return Expression.Equal(call, Expression.Constant(true, typeof(bool)));
        }

        /// <summary>
        /// Calls Enumerable.Count where the Type of the source parameter is of IEnumerable&lt;>.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static MethodCallExpression Count(Expression source)
        {
            Type enumerableType = GetGenericType(typeof(IEnumerable<>), source.Type);
            Type elementType = enumerableType.GetGenericArguments().First();
            return Expression.Call(typeof(Enumerable), "Count", new Type[] { elementType }, source);
        }

        /// <summary>
        /// Calls AsQueryable on the supplied source expression - will convert an IEnumerable to IQueryable.
        /// </summary>
        /// <param name="source">The source expression - should be an IEnumerable.</param>
        /// <returns></returns>
        public static MethodCallExpression AsQueryable(Expression source)
        {
            MethodCallExpression asQueryableExpression = Expression.Call(typeof(Queryable), "AsQueryable", new Type[] { source.Type.GetGenericArguments()[0] }, source);
            return asQueryableExpression;
        }

        /// <summary>
        /// Applies a where expression against the specified source expression using the specified predicate expression.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static MethodCallExpression Where(Expression source, LambdaExpression predicate)
        {
            MethodCallExpression call = Expression.Call(typeof(Queryable), "Where", new Type[]{ source.Type.GetGenericArguments()[0] }, source, Expression.Quote(predicate));
            return call;
        }

        /// <summary>
        /// Applies Select to the source expression using the specified selector expression.
        /// </summary>
        /// <param name="sourceType">The source type.</param>
        /// <param name="selectType">The select type, end result type.</param>
        /// <param name="source">The source expression.</param>
        /// <param name="selector">The selector expression.</param>
        /// <returns></returns>
        public static MethodCallExpression Select(Type sourceType, Type selectType, Expression source, LambdaExpression selector)
        {
            MethodCallExpression call = Expression.Call(typeof(Queryable), "Select", new Type[] { sourceType, selectType }, source, Expression.Quote(selector));
            return call;
        }

        /// <summary>
        /// Applies an order by ascending expression against the source based on the specified property selector.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keySelector">The selector expression for the property to order by.</param>
        /// <returns></returns>
        public static MethodCallExpression OrderByAscending(Expression source, LambdaExpression keySelector)
        {
            MethodCallExpression orderByMeasureOn = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { source.Type.GetGenericArguments()[0], keySelector.Body.Type }, source, Expression.Quote(keySelector));
            return orderByMeasureOn;
        }

        /// <summary>
        /// Applies an order by descending expression against the source based on the specified property selector.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keySelector">The selector expression for the property to order by.</param>
        /// <returns></returns>
        public static MethodCallExpression OrderByDescending(Expression source, LambdaExpression keySelector)
        {
            MethodCallExpression orderByMeasureOn = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { source.Type.GetGenericArguments()[0], keySelector.Body.Type }, source, Expression.Quote(keySelector));
            return orderByMeasureOn;
        }

        /// <summary>
        /// Calls Queryable.Contains using the specified values collection and property selector.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static MethodCallExpression Contains<T>(IQueryable<T> values, Expression selector)
        {
            //values will be a constant, selector will be a paramter expression for the value being compared to calling against Queryable.Contains

            Expression pe_values = Expression.Constant(values);
            return Expression.Call(typeof(Queryable), "Contains", new[] { typeof(T) }, new[] { pe_values, selector });
            
        }

        /// <summary>
        /// Applies FirstOrDefault to the source expression.
        /// </summary>
        /// <typeparam name="T">The return type of the call.</typeparam>
        /// <param name="source">The source expression.</param>
        /// <returns></returns>
        public static MethodCallExpression FirstOrDefault<T>(Expression source)
        {
            MethodCallExpression firstOrDefault = Expression.Call(typeof(Queryable), "FirstOrDefault", new Type[] { typeof(T) }, source);
            return firstOrDefault;
        }

        /// <summary>
        /// Applies a call to Math.Floor on the specified expression, if the specified generic type does not match the result type of the floor call, a convert will be attempted.
        /// </summary>
        /// <typeparam name="T">The desired type after the Floor call.</typeparam>
        /// <param name="expression">The expression to have Math.Floor applied to.</param>
        /// <returns></returns>
        public static Expression MathFloor<T>(Expression expression)
        {
            Expression floor = Expression.Call(typeof(Math), "Floor", Type.EmptyTypes, expression);

            if (floor.Type != typeof(T))
            {
                floor = Expression.Convert(floor, typeof(T));
            }

            return floor;
        }

        /// <summary>
        /// Applies a call to Math.Clieling on the specified expression, if the specified generic type does not match the result type of the ceiling call, a convert will be attempted.
        /// </summary>
        /// <typeparam name="T">The desired type after the Ceiling call.</typeparam>
        /// <param name="expression">The expression to have Math.Ceiling applied to.</param>
        /// <returns></returns>
        public static Expression MathCeiling<T>(Expression expression)
        {
            Expression ceiling = Expression.Call(typeof(Math), "Ceiling", Type.EmptyTypes, expression);

            if (ceiling.Type != typeof(T))
            {
                ceiling = Expression.Convert(ceiling, typeof(T));
            }

            return ceiling;
        }

        /// <summary>
        /// Calls System.Data.Entity.DbFunctions.DiffYears on the specified date expressions.
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static MethodCallExpression DbFunctionsDateDiff(Expression date1, Expression date2)
        {
            return Expression.Call(typeof(System.Data.Entity.DbFunctions), "DiffYears", Type.EmptyTypes, date1, date2);  
        }

        [Obsolete("Usage of this method should be limited since it is only supported by SqlServer.")]
        public static MethodCallExpression DbFunctionsCreateTruncatedDate(Expression year, Expression month)
        {
            //System.Data.Entity.DbFunctions.CreateDateTime(int? year, int? month, int? day, int? hour, int? minute, double? second);
            MethodCallExpression call = Expression.Call(typeof(System.Data.Entity.DbFunctions), "CreateDateTime", Type.EmptyTypes, year, month, Expression.Constant((int?)1, typeof(int?)), Expression.Constant((int?)0, typeof(int?)), Expression.Constant((int?)0, typeof(int?)), Expression.Constant((double?)0, typeof(double?)));
            return call;
        }

        /// <summary>
        /// Gets the generic type for the specified type.
        /// </summary>
        /// <param name="generic"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetGenericType(Type generic, Type type)
        {
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == generic) return type;
                if (generic.IsInterface)
                {
                    foreach (Type intfType in type.GetInterfaces())
                    {
                        Type found = GetGenericType(generic, intfType);

                        if (found != null)
                            return found;
                    }
                }
                type = type.BaseType;
            }
            return null;
        }
    }
}
