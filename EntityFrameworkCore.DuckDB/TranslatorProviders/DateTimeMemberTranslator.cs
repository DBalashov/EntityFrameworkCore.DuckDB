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
        if (member.DeclaringType != typeof(DateTime)) return null;

        switch (member.Name)
        {
            case "UtcNow":
                // example: date_add(cast(now() as timestamp), interval 2 hour)
                var offset = (int) TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalMinutes;
                if (offset == 0)
                {
                    return sqlExpressionFactory.Function("now", Array.Empty<SqlExpression>(), nullable: true, Array.Empty<bool>(), typeof(DateTime));
                }
                else
                {
                    return sqlExpressionFactory.Function("date_add",
                                                         new SqlExpression[]
                                                         {
                                                             new SqlFragmentExpression("cast(now() as timestamp)"),
                                                             new SqlFragmentExpression($"{(offset < 0 ? "-" : "")}interval {offset} minute")
                                                         },
                                                         nullable: true,
                                                         new[] {true},
                                                         typeof(DateTime));
                }
            case "Now":
                return sqlExpressionFactory.Function("now", Array.Empty<SqlExpression>(), nullable: true, Array.Empty<bool>(), typeof(DateTime));

            default:
                return null;
        }
    }
}