using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TrafficControl.Core
{
    public static class SQLHelpper
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering, string ordertype)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp;
            if (string.IsNullOrEmpty(ordertype))
            {
                ordertype = "ASC";
            }
            if (ordertype.ToLower().Equals("asc"))
            {
                resultExp = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            }
            else
            {
                resultExp = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            }
            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
