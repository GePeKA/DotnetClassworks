using System.Linq.Expressions;

namespace ClickHouseApi.Controllers.Helpers;

public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> BuildPredicate<T>(Dictionary<string, object?> filters)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        Expression? body = null;

        foreach (var (propertyName, value) in filters)
        {
            if (value == null)
                continue;

            var prop = Expression.Property(parameter, propertyName);
            var constant = Expression.Constant(value);

            Expression equal = Expression.Equal(
                prop,
                Expression.Convert(constant, prop.Type)
            );

            body = body == null ? equal : Expression.AndAlso(body, equal);
        }

        // Если нет условий, вернуть `x => true`
        return Expression.Lambda<Func<T, bool>>(body ?? Expression.Constant(true), parameter);
    }
}