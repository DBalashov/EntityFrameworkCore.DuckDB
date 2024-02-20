using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBTimeOnlyMethodsTranslator(ISqlExpressionFactory sqlExpressionFactory) : IMethodCallTranslator
{
    public SqlExpression? Translate(SqlExpression? instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (method.DeclaringType != typeof(TimeOnly)) return null;

        if (instance == null)
            throw new ArgumentNullException(nameof(instance));

        if (arguments.Count != 1)
            throw new ArgumentNullException(nameof(arguments), "Must be only one argument");
        
        var arg = arguments[0];
        var toFunction = method.Name switch
                         {
                             nameof(TimeOnly.AddHours) =>
                                 // p.timeOnly.AddHours(p.hoursCount) -> date_add(timeOnly, to_microseconds(cast(p.hoursCount * 3_600_000_000.0 as bigint)))
                                 sqlExpressionFactory.Function("to_microseconds",
                                                               new[]
                                                               {
                                                                   sqlExpressionFactory.Convert(sqlExpressionFactory.Multiply(arg, sqlExpressionFactory.Constant(60 * 60 * (Int64) 1_000_000)),
                                                                                                typeof(Int64))
                                                               },
                                                               true,
                                                               new[] {true},
                                                               typeof(Int64)),
                             nameof(TimeOnly.AddMinutes) =>
                                 // p.timeOnly.AddMinutes(p.minutesCount) -> date_add(timeOnly, to_microseconds(cast(p.minutesCount * 60_000_000.0 as bigint)))
                                 sqlExpressionFactory.Function("to_microseconds",
                                                               new[]
                                                               {
                                                                   sqlExpressionFactory.Convert(sqlExpressionFactory.Multiply(arg, sqlExpressionFactory.Constant(60 * (Int64) 1_000_000)),
                                                                                                typeof(Int64))
                                                               },
                                                               true,
                                                               new[] {true},
                                                               typeof(Int64)),
                             _ => throw new NotImplementedException($"Method {method.Name} not implemented")
                         };

        return sqlExpressionFactory.Function("date_add", new[] {instance, toFunction}, true, new[] {true}, typeof(TimeOnly));
    }
}