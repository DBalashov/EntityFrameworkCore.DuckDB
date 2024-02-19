using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBTimeOnlyMemberTranslator(ISqlExpressionFactory sqlExpressionFactory) : IMemberTranslator
{
    static readonly Dictionary<string, string> propertyMapping = new()
                                                                 {
                                                                     {nameof(TimeOnly.Hour), "hour"},
                                                                     {nameof(TimeOnly.Minute), "minute"},
                                                                     {nameof(TimeOnly.Second), "second"},
                                                                     {nameof(TimeOnly.Millisecond), "milliseconds"},
                                                                     {nameof(TimeOnly.Microsecond), "microseconds"}
                                                                 };

    public SqlExpression? Translate(SqlExpression? instance, MemberInfo member, Type returnType, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (member.DeclaringType != typeof(TimeSpan))
            return null;

        if (instance == null)
            throw new ArgumentNullException(nameof(instance));

        if (propertyMapping.TryGetValue(member.Name, out var mappedPropertyName)) // p.dt.Seconds -> datepart('second', dt)
            return sqlExpressionFactory.Function("datepart",
                                                 new[]
                                                 {
                                                     sqlExpressionFactory.Fragment($"'{mappedPropertyName}'"),
                                                     instance,
                                                 },
                                                 true,
                                                 new[] {true},
                                                 typeof(int));

        throw new NotImplementedException($"Property {member.Name} not implemented");
    }
}