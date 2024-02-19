using Microsoft.EntityFrameworkCore.Query;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBSqlTranslatingExpressionVisitorFactory(RelationalSqlTranslatingExpressionVisitorDependencies dependencies) : IRelationalSqlTranslatingExpressionVisitorFactory
{
    public RelationalSqlTranslatingExpressionVisitor Create(QueryCompilationContext ctx, QueryableMethodTranslatingExpressionVisitor visitor) =>
        new DuckDBSqlTranslatingExpressionVisitor(dependencies, ctx, visitor);
}