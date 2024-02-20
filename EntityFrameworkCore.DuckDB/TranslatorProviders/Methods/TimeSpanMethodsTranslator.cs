using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBTimeSpanMethodsTranslator(ISqlExpressionFactory sqlExpressionFactory) : IMethodCallTranslator
{
    public SqlExpression? Translate(SqlExpression? instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (method.DeclaringType != typeof(TimeSpan)) return null;

        if (instance == null)
            throw new ArgumentNullException(nameof(instance));

        if (arguments.Count != 1)
            throw new ArgumentNullException(nameof(arguments), "Must be only one argument");

        var arg = arguments[0];
        
        // todo: date_diff('seconds', cast('12:34:56' as time), cast('22:35:13' as time));
        // how to make time from seconds / microseconds (?)
        
        throw new NotImplementedException($"Method {method.Name} not implemented");
    }
}