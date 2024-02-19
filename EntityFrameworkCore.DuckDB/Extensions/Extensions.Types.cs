using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

static class ExtensionsTypes
{
    public static Type? GetProviderType(this SqlExpression? expression) => expression == null
                                                                               ? null
                                                                               : (expression.TypeMapping?.Converter?.ProviderClrType
                                                                               ?? expression.TypeMapping?.ClrType
                                                                               ?? expression.Type);
}