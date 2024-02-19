using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBQueryableSubQueryTranslatingExpressionVisitor : RelationalQueryableMethodTranslatingExpressionVisitor
{
    public DuckDBQueryableSubQueryTranslatingExpressionVisitor(QueryableMethodTranslatingExpressionVisitorDependencies           dependencies,
                                                               RelationalQueryableMethodTranslatingExpressionVisitorDependencies relationalDependencies,
                                                               QueryCompilationContext                                           ctx)
        : base(dependencies, relationalDependencies, ctx)
    {
    }

    public DuckDBQueryableSubQueryTranslatingExpressionVisitor(DuckDBQueryableMethodTranslatingExpressionVisitor parentVisitor) : base(parentVisitor)
    {
    }

    // todo add subquery ordering
    protected override ShapedQueryExpression? TranslateOrderBy(ShapedQueryExpression source,
                                                               LambdaExpression      keySelector,
                                                               bool                  ascending) =>
        throw new NotSupportedException($"OrderBy not supported: {source}");

    protected override ShapedQueryExpression? TranslateThenBy(ShapedQueryExpression source,
                                                              LambdaExpression      keySelector,
                                                              bool                  ascending) =>
        throw new NotSupportedException($"OrderBy not supported: {source}");
}