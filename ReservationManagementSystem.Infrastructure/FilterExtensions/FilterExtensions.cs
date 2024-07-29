using System.Linq.Expressions;
using System.Reflection;

namespace ReservationManagementSystem.Infrastructure.FilterExtensions;

public static class FilterExtensions
{
    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, Expression<Func<T, bool>>? filterExpression)
    {
        return filterExpression != null ? query.Where(filterExpression) : query;
    }

    public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, Expression<Func<T, object>>? sortExpression, bool isAscending)
    {
        if (sortExpression == null) return query;

        return isAscending ? query.OrderBy(sortExpression) : query.OrderByDescending(sortExpression);
    }

    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        var skipResults = (pageNumber - 1) * pageSize;
        return query.Skip(skipResults).Take(pageSize);
    }

    public static Expression<Func<T, bool>>? GetFilterExpression<T>(string? filterOn, string? filterQuery)
    {
        if (string.IsNullOrWhiteSpace(filterOn) || string.IsNullOrWhiteSpace(filterQuery))
        {
            return null;
        }

        var property = typeof(T).GetProperty(filterOn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (property == null)
        {
            return null;
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyExpression = Expression.Property(parameter, property.Name);
        var constant = Expression.Constant(filterQuery);
        Expression? body = null;

        if (property.PropertyType == typeof(string))
        {
            var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            body = Expression.Call(propertyExpression, method!, constant);
        }
        else if (property.PropertyType == typeof(DateTime) && DateTime.TryParse(filterQuery, out DateTime date))
        {
            var dateConstant = Expression.Constant(date);
            body = Expression.Equal(propertyExpression, dateConstant);
        }
        else if (property.PropertyType == typeof(int) && int.TryParse(filterQuery, out int number))
        {
            var numberConstant = Expression.Constant(number);
            body = Expression.Equal(propertyExpression, numberConstant);
        }

        return body != null ? Expression.Lambda<Func<T, bool>>(body, parameter) : null;
    }

    public static Expression<Func<T, object>>? GetSortExpression<T>(string? sortBy)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            return null;
        }

        var property = typeof(T).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        if (property == null)
        {
            return null;
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyExpression = Expression.Property(parameter, property.Name);

        return Expression.Lambda<Func<T, object>>(Expression.Convert(propertyExpression, typeof(object)), parameter);
    }
}
