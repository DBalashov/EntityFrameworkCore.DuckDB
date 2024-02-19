using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

// https://duckdb.org/docs/sql/functions/char
sealed class DuckDBDateTimeOffsetMethodsTranslator(ISqlExpressionFactory sqlExpressionFactory) : IMethodCallTranslator
{
    public SqlExpression? Translate(SqlExpression? instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (method.DeclaringType != typeof(DateTime)       &&
            method.DeclaringType != typeof(DateTimeOffset) &&
            method.DeclaringType != typeof(DateOnly))
            return null;

        if (instance == null)
            throw new ArgumentNullException(nameof(instance));

        if (arguments.Count != 1)
            throw new ArgumentNullException(nameof(arguments), "Must be only one argument");

        var arg = arguments[0];

        if (sqlExpressionFactory.TryToDateTimeMethodMap<DateTimeOffset>(method, instance, arg, out var expression))
            return expression;

        throw new NotImplementedException($"Method {method.Name} not implemented");
    }
}