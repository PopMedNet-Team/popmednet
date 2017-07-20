using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Lpp.Utilities.Legacy
{
	/// <summary>
	/// An IQueryable wrapper that allows us to visit the query's expression tree just before LINQ to SQL gets to it.
	/// </summary>
    /// <remarks>
    /// This code has been copied from the LINQKit project and then modified in order to enhance and reflect
    /// latest .NET FW and C# changes. For original work, refer to http://www.albahari.com/nutshell/linqkit.html and
    /// http://tomasp.net/blog/linq-expand.aspx
    /// </remarks>
	public sealed class ExpandableQuery<T> : IQueryable<T>, IOrderedQueryable<T>, IOrderedQueryable
	{
		ExpandableQueryProvider<T> _provider;
		IQueryable<T> _inner;
		internal IQueryable<T> InnerQuery { get { return _inner; } }

		internal ExpandableQuery (IQueryable<T> inner)
		{
            //Contract.Requires( inner != null );
			_inner = inner;
			_provider = new ExpandableQueryProvider<T> (this);
		}

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts." )]
        private void ObjectInvariant()
        {
            //Contract.Invariant( _inner != null );
            //Contract.Invariant( _provider != null );
        }

		Expression IQueryable.Expression { get { return _inner.Expression; } }
		Type IQueryable.ElementType { get { return typeof (T); } }
		IQueryProvider IQueryable.Provider { get { return _provider; } }
		public IEnumerator<T> GetEnumerator () { return _inner.GetEnumerator (); }
		IEnumerator IEnumerable.GetEnumerator () { return _inner.GetEnumerator (); }
		public override string ToString () { return _inner.ToString (); }
	}

	class ExpandableQueryProvider<T> : IQueryProvider
	{
		readonly ExpandableQuery<T> _query;

		internal ExpandableQueryProvider (ExpandableQuery<T> query)
		{
            //Contract.Requires( query != null );
			_query = query;
		}

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts." )]
        private void ObjectInvariant()
        {
            //Contract.Invariant( _query != null );
        }

		IQueryable<TElement> IQueryProvider.CreateQuery<TElement> (Expression expression)
		{
			return new ExpandableQuery<TElement> (_query.InnerQuery.Provider.CreateQuery<TElement> (expression.Expand()));
		}

		IQueryable IQueryProvider.CreateQuery (Expression expression)
		{
			return _query.InnerQuery.Provider.CreateQuery (expression.Expand());
		}

		TResult IQueryProvider.Execute<TResult> (Expression expression)
		{
			return _query.InnerQuery.Provider.Execute<TResult> (expression.Expand());
		}

		object IQueryProvider.Execute (Expression expression)
		{
			return _query.InnerQuery.Provider.Execute (expression.Expand());
		}
	}
}