using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

static class DateTimePropertyMappingExtenders
{
    // https://duckdb.org/docs/sql/functions/datepart
    // DateTime / DateTimeOffset property -> datepart specifiers mapping
    static readonly Dictionary<string, string> propertyMapping = new()
                                                                 {
                                                                     {nameof(DateTime.DayOfYear), "dayofyear"},
                                                                     {nameof(DateTime.DayOfWeek), "dayofweek"},
                                                                     {nameof(DateTime.Year), "year"},
                                                                     {nameof(DateTime.Month), "month"},
                                                                     {nameof(DateTime.Day), "day"},
                                                                     {nameof(DateTime.Hour), "hour"},
                                                                     {nameof(DateTime.Minute), "minute"},
                                                                     {nameof(DateTime.Second), "second"},
                                                                     {nameof(DateTime.Millisecond), "milliseconds"},
                                                                     {nameof(DateTime.Microsecond), "microseconds"}
                                                                 };

    public static bool TryToDateTimePropertyMap(this ISqlExpressionFactory sqlExpressionFactory, MemberInfo member, SqlExpression? instance, out SqlExpression expression)
    {
        if (!propertyMapping.TryGetValue(member.Name, out var mappedPropertyName))
        {
            expression = default!;
            return false;
        }

        expression = sqlExpressionFactory.Function("datepart",
                                                   new[] {sqlExpressionFactory.Fragment($"'{mappedPropertyName}'"), instance}!,
                                                   true,
                                                   new[] {true},
                                                   typeof(int));
        return true;
    }
}