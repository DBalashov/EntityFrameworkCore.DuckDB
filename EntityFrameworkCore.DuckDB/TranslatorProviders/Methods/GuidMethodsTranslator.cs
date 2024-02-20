using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBGuidMethodsTranslator(ISqlExpressionFactory sqlExpressionFactory) : IMethodCallTranslator
{
    public SqlExpression? Translate(SqlExpression? instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (method.DeclaringType != typeof(Guid)) return null;

        return method.Name switch
               {
                   nameof(Guid.NewGuid) => sqlExpressionFactory.Function("uuid", Array.Empty<SqlExpression>(), true, Array.Empty<bool>(), method.ReturnType),
                   nameof(Guid.Parse) => arguments.Count < 1
                                             ? throw new ArgumentException("Arguments required", nameof(arguments))
                                             : sqlExpressionFactory.Convert(arguments[0], typeof(Guid)),
                   _ => throw new NotImplementedException($"Method {method.Name} not implemented")
               };
    }
}