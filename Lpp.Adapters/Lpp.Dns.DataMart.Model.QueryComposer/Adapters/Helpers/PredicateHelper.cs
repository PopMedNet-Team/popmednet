using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters
{
    public class PredicateHelper
    {
        public static Expression<TDelegate> Negate<TDelegate>(Expression<TDelegate> expression)
        {
            return Expression.Lambda<TDelegate>(Expression.Not(expression.Body), expression.Parameters);
        }
    }
}
