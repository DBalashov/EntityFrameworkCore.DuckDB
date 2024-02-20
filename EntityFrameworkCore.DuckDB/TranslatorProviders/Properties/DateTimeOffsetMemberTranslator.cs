using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBDateTimeOffsetMemberTranslator(ISqlExpressionFactory sqlExpressionFactory) : IMemberTranslator
{
    public SqlExpression? Translate(SqlExpression? instance, MemberInfo member, Type returnType, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (member.DeclaringType != typeof(DateTimeOffset))
            return null;

        if (sqlExpressionFactory.TryToDateTimePropertyMap(member, instance, out var expression))
            return expression;

        switch (member.Name)
        {
            case nameof(DateTimeOffset.UtcNow):
                // todo investigate if this is correct
                return sqlExpressionFactory.Function("now", Array.Empty<SqlExpression>(), true, new[] {true}, typeof(DateTimeOffset));

            case nameof(DateTimeOffset.Now):
                return sqlExpressionFactory.Function("now", Array.Empty<SqlExpression>(), true, new[] {true}, typeof(DateTimeOffset));

            default:
                return null;
        }
    }
}