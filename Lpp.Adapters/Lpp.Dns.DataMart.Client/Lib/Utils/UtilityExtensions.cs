using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Reactive.Linq;

namespace Lpp
{
    public static class UtilityExtensions
    {
        public static TValue ValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue ))
        {
            //Contract.Requires( dictionary != null );

            TValue res;
            return dictionary.TryGetValue(key, out res) ? res : defaultValue;
        }

        public static string MemberName<T>(this Expression<T> expr)
        {
            //Contract.Requires( expr != null );
            return expr.Body.MemberName();
        }

        public static IDictionary<string, object> CallParameters<T>(this Expression<T> expr)
        {
            //Contract.Requires( expr != null );
            //Contract.Ensures( Contract.Result<IDictionary<string,object>>() != null );
            return expr.Body.CallParameters();
        }

        public static IDictionary<string, object> CallParameters(this Expression expr)
        {
            //Contract.Requires( expr != null );
            //Contract.Ensures( Contract.Result<IDictionary<string, object>>() != null );

            switch (expr.NodeType)
            {
                case ExpressionType.Call:
                    {
                        var mce = expr as MethodCallExpression;
                        return
                            mce.Arguments
                            .Zip(mce.Method.GetParameters(), (a, p) => new { param = p, value = a.Eval() })
                            //.SelectMany( x =>
                            //    ( x.param.ParameterType.IsPrimitive || x.param.ParameterType == typeof( string ) ) ?
                            //    EnumerableEx.Return( new { x.param.Name, x.value } ) :
                            //    TypeDescriptor.GetProperties( x.value ).Cast<PropertyDescriptor>()
                            //    .Select( p => new { p.Name, value = p.GetValue( x.value ) } ) )
                            .ToDictionary(x => x.param.Name, x => x.value);
                    }

                case ExpressionType.Convert: return (expr as UnaryExpression).Operand.CallParameters();
                default: throw new InvalidOperationException("Unsupported expression type: " + expr.NodeType);
            }
        }

        public static object Eval(this Expression expression)
        {
            return
                Expression.Lambda<Func<object>>(
                    Expression.Convert(expression, typeof(object))
                )
                .Compile()
                ();
        }

        public static string MemberName(this Expression expr)
        {
            switch (expr.NodeType)
            {
                case ExpressionType.MemberAccess: return (expr as MemberExpression).Member.Name;
                case ExpressionType.Call: return (expr as MethodCallExpression).Method.Name;
                case ExpressionType.Convert: return (expr as UnaryExpression).Operand.MemberName();
                default: return null;
                //throw new InvalidOperationException( "Unsupported expression type: " + expr.NodeType );
            }
        }

        public static void Raise(this EventHandler e, object sender = null)
        {
            if (e != null) e(sender, EventArgs.Empty);
        }

        public static void Raise<T>(this EventHandler<T> e, object sender, T args) where T : EventArgs
        {
            if (e != null) e(sender, args);
        }

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        public static ISet<T> ToSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> a, IDictionary<TKey, TValue> b)
        {
            if (a == null) return b;
            if (b == null) return a;

            var res = new SortedList<TKey, TValue>(a);
            foreach (var k in b) res[k.Key] = k.Value;

            return res;
        }

        public static IDictionary<T, V> MapValues<T, U, V>(this IDictionary<T, U> d, Func<U, V> selector)
        {
            //Contract.Requires( d != null );
            //Contract.Requires( selector != null );
            return d.ToDictionary(x => x.Key, x => selector(x.Value));
        }

        public static IDictionary<V, U> MapKeys<T, U, V>(this IDictionary<T, U> d, Func<T, V> selector)
        {
            //Contract.Requires( d != null );
            //Contract.Requires( selector != null );
            return d.ToDictionary(x => selector(x.Key), x => x.Value);
        }

        public static bool NullOrEmpty<T>(this IEnumerable<T> s) { return s == null || !s.Any(); }
        public static bool NullOrSpace(this string s) { return string.IsNullOrWhiteSpace(s); }

        public static ILookup<T, U> ToLookup<X, T, U>(this IEnumerable<X> source, Func<X, T> selectKey, Func<X, IEnumerable<U>> selectValues)
        {
            //Contract.Requires(source != null);
            //Contract.Requires( selectKey != null );
            //Contract.Requires( selectValues != null );

            return source
                .SelectMany(x => selectValues(x).EmptyIfNull().Select(value => new { key = selectKey(x), value }))
                .ToLookup(x => x.key, x => x.value);
        }

        public static T If<T>(this T source, bool condition, Func<T, T> then)
        {
            //Contract.Requires( then != null );
            return condition ? then(source) : source;
        }

        public static U If<T, U>(this T source, bool condition, Func<T, U> then, Func<T, U> fnElse)
        {
            //Contract.Requires( then != null );
            //Contract.Requires( fnElse != null );
            return (condition ? then : fnElse)(source);
        }

        public static void CallDispose(this IDisposable d)
        {
            if (d != null) d.Dispose();
        }

        public static ILookup<TKey, TValue> Merge<TKey, TValue>(this ILookup<TKey, TValue> a, IEnumerable<ILookup<TKey, TValue>> bs)
        {
            //Contract.Requires( a != null );
            //Contract.Requires( bs != null );
            //Contract.Ensures( Contract.Result<ILookup<TKey, TValue>>() != null );
            return flatten(a).Concat(bs.SelectMany(flatten)).ToLookup(k => k.Key, k => k.Value);
        }

        public static ILookup<TKey, TValue> Merge<TKey, TValue>(this ILookup<TKey, TValue> a, ILookup<TKey, TValue> b)
        {
            //Contract.Requires( a != null );
            //Contract.Requires( b != null );
            //Contract.Ensures( Contract.Result<ILookup<TKey, TValue>>() != null );
            return a.Merge(EnumerableEx.Return(b));
        }

        static IEnumerable<KeyValuePair<TKey, TValue>> flatten<TKey, TValue>(ILookup<TKey, TValue> l)
        {
            return from k in l
                   from v in k
                   select new KeyValuePair<TKey, TValue>(k.Key, v);
        }

        public static string ToQueryString(this System.Collections.Specialized.NameValueCollection values)
        {
            if (values == null && values.Count == 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            for (int i = 0; i < values.Count; i++)
            {
                string text = values.GetKey(i);
                {
                    text = System.Web.HttpUtility.UrlEncode(text);

                    string key = (!string.IsNullOrEmpty(text)) ? (text + "=") : string.Empty;
                    string[] vals = values.GetValues(i);

                    if (sb.Length > 0)
                        sb.Append('&');

                    if (vals == null || vals.Length == 0)
                        sb.Append(key);
                    else
                    {
                        if (vals.Length == 1)
                        {
                            sb.Append(key);
                            sb.Append(System.Web.HttpUtility.UrlEncode(vals[0]));
                        }
                        else
                        {
                            for (int j = 0; j < vals.Length; j++)
                            {
                                if (j > 0)
                                    sb.Append('&');

                                sb.Append(key);
                                sb.Append(System.Web.HttpUtility.UrlEncode(vals[j]));
                            }
                        }
                    }
                }
            }

            if (sb.Length > 0)
            {
                sb.Insert(0, "?");
            }

            return sb.ToString();
        }
    }
}