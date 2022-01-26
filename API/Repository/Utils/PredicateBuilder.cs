using System.Linq.Expressions;
using System.Reflection;
using API.Repository;

namespace API;

public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> True<T>()
    {
        return _ => true;
    }

    public static Expression<Func<T, bool>> False<T>()
    {
        return _ => false;
    }

    public static IQueryable<T> AdvancedSearch<T>(this IQueryable<T> query, Search search)
    {
        var predicate = False<T>();
        foreach (PropertyInfo propertyInfo in typeof(T).GetProperties()
            .Where(p => p.GetGetMethod()?.IsVirtual is false &&
                        search.Fields.Any(field => p.Name.Equals(field, StringComparison.OrdinalIgnoreCase))))
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            MemberExpression property = Expression.Property(parameter, propertyInfo);
            UnaryExpression propertyAsObject = Expression.Convert(property, typeof(object));
            BinaryExpression nullCheck = Expression.NotEqual(propertyAsObject, Expression.Constant(null, typeof(object)));
            MethodCallExpression propertyAsString = Expression.Call(property, "ToString", null, null);
            ConstantExpression keywordExpression = Expression.Constant(search.Keyword);
            MethodCallExpression contains = propertyInfo.PropertyType == typeof(string) ? Expression.Call(property, "Contains", null, keywordExpression) : Expression.Call(propertyAsString, "Contains", null, keywordExpression);
            LambdaExpression lambda = Expression.Lambda(Expression.AndAlso(nullCheck, contains), parameter);
            predicate = predicate.Or((Expression<Func<T, bool>>)lambda);
        }

        return query.Where(predicate);
    }

    public static IQueryable<T> SearchByKeyword<T>(this IQueryable<T> query, string keyword)
    {
        var predicate = False<T>();
        var properties = typeof(T).GetProperties();
        foreach (PropertyInfo propertyInfo in properties.Where(p => p.GetGetMethod()?.IsVirtual is false))
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            MemberExpression property = Expression.Property(parameter, propertyInfo);
            UnaryExpression propertyAsObject = Expression.Convert(property, typeof(object));
            BinaryExpression nullCheck = Expression.NotEqual(propertyAsObject, Expression.Constant(null, typeof(object)));
            MethodCallExpression propertyAsString = Expression.Call(property, "ToString", null, null);
            ConstantExpression keywordExpression = Expression.Constant(keyword);
            MethodCallExpression contains = propertyInfo.PropertyType == typeof(string) ? Expression.Call(property, "Contains", null, keywordExpression) : Expression.Call(propertyAsString, "Contains", null, keywordExpression);
            LambdaExpression lambda = Expression.Lambda(Expression.AndAlso(nullCheck, contains), parameter);
            predicate = predicate.Or((Expression<Func<T, bool>>)lambda);
        }

        return query.Where(predicate);
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        InvocationExpression invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
        return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
    }

    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        InvocationExpression invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
    }
}