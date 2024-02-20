using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBDateTimeMemberTranslator(ISqlExpressionFactory sqlExpressionFactory) : IMemberTranslator
{
    public SqlExpression? Translate(SqlExpression? instance, MemberInfo member, Type returnType, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (member.DeclaringType != typeof(DateTime))
            return null;

        if (sqlExpressionFactory.TryToDateTimePropertyMap(member, instance, out var expression))
            return expression;

        switch (member.Name)
        {
            case nameof(DateTime.UtcNow):
                // example: date_add(cast(now() as timestamp), to_minutes(240))
                var offset = (int) TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalMinutes;
                if (offset == 0)
                {
                    return sqlExpressionFactory.Fragment("cast(now() as timestamp)");
                }
                else
                {
                    return sqlExpressionFactory.Function("date_add",
                                                         new SqlExpression[]
                                                         {
                                                             new SqlFragmentExpression("cast(now() as timestamp)"),
                                                             new SqlFragmentExpression($"to_minutes({-offset})")
                                                         },
                                                         true,
                                                         new[] {true},
                                                         typeof(DateTime));
                }

            case nameof(DateTime.Now):
                return sqlExpressionFactory.Fragment("cast(now() as timestamp)");

            default:
                return null;
        }
    }
}