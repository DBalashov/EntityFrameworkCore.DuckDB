using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

// https://duckdb.org/docs/sql/functions/numeric
sealed class DuckDBDoubleMethodsTranslator(ISqlExpressionFactory sqlExpressionFactory) : IMethodCallTranslator
{
    public SqlExpression? Translate(SqlExpression? instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (method.DeclaringType != typeof(double) || method.ReturnType != typeof(float))
            return null;

        if (arguments.Count != 1)
            throw new ArgumentException("Only one argument allowed", nameof(arguments));

        var nullabilityMask = new[] {true};
        var retType         = arguments[0].Type;

        return method.Name switch
               {
                   nameof(double.IsFinite)   => sqlExpressionFactory.Function("isfinite",   arguments, true, nullabilityMask, retType),
                   nameof(double.IsInfinity) => sqlExpressionFactory.Function("isinfinite", arguments, true, nullabilityMask, retType),
                   nameof(double.IsNaN)      => sqlExpressionFactory.Function("isnan",      arguments, true, nullabilityMask, retType),
                   _                         => null
               };
    }
}