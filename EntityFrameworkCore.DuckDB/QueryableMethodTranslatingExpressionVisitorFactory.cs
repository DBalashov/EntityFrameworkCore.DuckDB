using Microsoft.EntityFrameworkCore.Query;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBQueryableMethodTranslatingExpressionVisitorFactory(QueryableMethodTranslatingExpressionVisitorDependencies           dependencies,
                                                                      RelationalQueryableMethodTranslatingExpressionVisitorDependencies relationalDependencies)
    : IQueryableMethodTranslatingExpressionVisitorFactory
{
    public QueryableMethodTranslatingExpressionVisitor Create(QueryCompilationContext queryCompilationContext) =>
        new DuckDBQueryableMethodTranslatingExpressionVisitor(dependencies, relationalDependencies, queryCompilationContext);
}