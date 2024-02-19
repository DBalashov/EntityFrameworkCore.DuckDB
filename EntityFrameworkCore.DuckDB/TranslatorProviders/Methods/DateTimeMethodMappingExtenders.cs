using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

static class DateTimeMethodMappingExtenders
{
    static readonly Dictionary<string, string> methodMapping = new()
                                                               {
                                                                   {nameof(DateTime.AddYears), "to_years"},
                                                                   {nameof(DateTime.AddMonths), "to_months"},
                                                                   {nameof(DateTime.AddDays), "to_days"},
                                                                   {nameof(DateTime.AddHours), "to_hours"},
                                                                   {nameof(DateTime.AddMinutes), "to_minutes"},
                                                                   {nameof(DateTime.AddSeconds), "to_seconds"},
                                                                   {nameof(DateTime.AddMilliseconds), "to_milliseconds"},
                                                                   {nameof(DateTime.AddMicroseconds), "to_microseconds"},
                                                               };

    public static bool TryToDateTimeMethodMap<R>(this ISqlExpressionFactory sqlExpressionFactory, MethodInfo method, SqlExpression instance, SqlExpression arg, out SqlExpression expression)
    {
        if (!methodMapping.TryGetValue(method.Name, out var mappedFunctionName))
        {
            expression = default!;
            return false;
        }

        // p.dt.AddDays(p.daysCount) -> date_add(dt, to_days(daysCount))
        expression = sqlExpressionFactory.Function("date_add",
                                                   new[] {instance, sqlExpressionFactory.Function(mappedFunctionName, new[] {arg}, true, new[] {true}, typeof(int))},
                                                   true,
                                                   new[] {true},
                                                   typeof(R));
        return true;
    }
}