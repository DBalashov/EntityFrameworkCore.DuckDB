using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

static class DateTimeMethodMappingExtenders
{
    // with 'int' argument -> no convert to microseconds, add as is
    static readonly Dictionary<string, string> methodNonConvertableMapping = new()
                                                                             {
                                                                                 {nameof(DateTime.AddYears), "to_years"},
                                                                                 {nameof(DateTime.AddMonths), "to_months"},
                                                                             };

    // with 'double' argument -> convert to microseconds, cast to Int64 and use to_microseconds function
    static readonly Dictionary<string, Int64> methodConvertableMapping = new()
                                                                         {
                                                                             {nameof(DateTime.AddDays), 24    * 60 * 60 * (Int64) 1_000_000},
                                                                             {nameof(DateTime.AddHours), 60   * 60 * (Int64) 1_000_000},
                                                                             {nameof(DateTime.AddMinutes), 60 * (Int64) 1_000_000},
                                                                             {nameof(DateTime.AddSeconds), 1_000_000},
                                                                             {nameof(DateTime.AddMilliseconds), 1_000},
                                                                             {nameof(DateTime.AddMicroseconds), 1},
                                                                         };

    public static bool TryToDateTimeMethodMap<R>(this ISqlExpressionFactory sqlExpressionFactory, MethodInfo method, SqlExpression instance, SqlExpression arg, out SqlExpression expression)
    {
        if (methodNonConvertableMapping.TryGetValue(method.Name, out var nonConvertableFunction))
        {
            // p.dt.AddDays(p.daysCount) -> date_add(dt, to_days(daysCount))
            expression = sqlExpressionFactory.Function("date_add",
                                                       new[] {instance, sqlExpressionFactory.Function(nonConvertableFunction, new[] {arg}, true, new[] {true}, typeof(int))},
                                                       true,
                                                       new[] {true},
                                                       typeof(R));
            return true;
        }

        if (methodConvertableMapping.TryGetValue(method.Name, out var multiplier))
        {
            // p.dt.AddHours(p.hoursCount) -> date_add(dt, to_microseconds(cast(p.hoursCount * 60000000.0 as bigint)))
            var toFunction = sqlExpressionFactory.Function("to_microseconds",
                                                           new[]
                                                           {
                                                               sqlExpressionFactory.Convert(sqlExpressionFactory.Multiply(arg, sqlExpressionFactory.Constant(multiplier)), typeof(Int64))
                                                           },
                                                           true,
                                                           new[] {true},
                                                           typeof(long));

            expression = sqlExpressionFactory.Function("date_add", new[] {instance, toFunction}, true, new[] {true}, typeof(R));
            return true;
        }

        expression = default!;
        return false;
    }
}