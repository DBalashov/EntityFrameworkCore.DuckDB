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
        if (member.DeclaringType != typeof(DateTimeOffset)) return null;

        // todo: implement DateTimeOffset member translation
        // switch (member.Name)
        // {
        //     case "UtcNow":
        //         return sqlExpressionFactory.Function("CurrentTzTimestamp",
        //                                              new SqlExpression[] {new SqlConstantExpression(Expression.Constant("UTC"), new StringTypeMapping("string", DbType.String))},
        //                                              nullable: true,
        //                                              new[] {true},
        //                                              typeof(DateTimeOffset));
        //     
        //     case "Now":
        //         if (!TimeZoneInfo.TryConvertWindowsIdToIanaId(TimeZoneInfo.Local.Id, out var ianaId))
        //             throw new NotSupportedException($"Can't convert {TimeZoneInfo.Local.Id} to IANA ID");
        //         
        //         return sqlExpressionFactory.Function("CurrentTzTimestamp",
        //                                              new SqlExpression[] {new SqlConstantExpression(Expression.Constant(ianaId), new StringTypeMapping("string", DbType.String))},
        //                                              nullable: true,
        //                                              new[] {true},
        //                                              typeof(DateTimeOffset));
        //     default:
                return null;
        // }
    }
}