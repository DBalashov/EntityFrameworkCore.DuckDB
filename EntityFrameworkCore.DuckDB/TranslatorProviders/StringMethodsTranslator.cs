using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

// https://duckdb.org/docs/sql/functions/char
sealed class DuckDBStringMethodsTranslator(ISqlExpressionFactory sqlExpressionFactory) : IMethodCallTranslator
{
    public SqlExpression? Translate(SqlExpression? instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (method.DeclaringType != typeof(string))
            return null;

        if (instance == null)
            throw new ArgumentNullException(nameof(instance));

        return method.Name switch
               {
                   nameof(string.StartsWith) => sqlExpressionFactory.Function("starts_with", arguments.Take(1), true, new[] {true}, typeof(string)),
                   nameof(string.EndsWith)   => sqlExpressionFactory.Function("ends_with",   arguments.Take(1), true, new[] {true}, typeof(string)),
                   "get_Chars" => sqlExpressionFactory.Function("array_extract",
                                                                new[] {instance, arguments.First()},
                                                                true,
                                                                new[] {true, true},
                                                                typeof(char)),
                   nameof(string.Substring) => getSubstringExpression(instance, arguments),
                   nameof(string.IndexOf) => sqlExpressionFactory.Subtract(sqlExpressionFactory.Function("instr",
                                                                                                         new[] {instance, arguments[0]},
                                                                                                         true,
                                                                                                         new[] {true, true},
                                                                                                         typeof(int)),
                                                                           new SqlConstantExpression(Expression.Constant(1), null)),
                   nameof(string.Length)           => sqlExpressionFactory.Function("strlen", new[] {instance}, true, new[] {true}, typeof(int)),
                   nameof(string.ToLower)          => sqlExpressionFactory.Function("lower",  new[] {instance}, true, new[] {true}, typeof(string)),
                   nameof(string.ToLowerInvariant) => sqlExpressionFactory.Function("lower",  new[] {instance}, true, new[] {true}, typeof(string)),
                   nameof(string.ToUpper)          => sqlExpressionFactory.Function("upper",  new[] {instance}, true, new[] {true}, typeof(string)),
                   nameof(string.ToUpperInvariant) => sqlExpressionFactory.Function("upper",  new[] {instance}, true, new[] {true}, typeof(string)),
                   _                               => throw new NotImplementedException($"Method {method.Name} not implemented")
               };
    }

    SqlExpression getSubstringExpression(SqlExpression instance, IReadOnlyList<SqlExpression> arguments)
    {
        switch (arguments.Count)
        {
            case 1: // "Hello".Substring(3)
                return sqlExpressionFactory.Function("array_slice",
                                                     new[] {instance, arguments[0], new SqlFragmentExpression("NULL")},
                                                     true, new[] {true, true}, typeof(string));

            case 2: // "Hello".Substring(3, 2)
                return sqlExpressionFactory.Function("array_slice",
                                                     new[] {instance, arguments[0], arguments[1]},
                                                     true, new[] {true, true, true}, typeof(string));

            default:
                throw new NotImplementedException($"Method {nameof(string.Substring)} with {arguments.Count} arguments not implemented");
        }
    }
}