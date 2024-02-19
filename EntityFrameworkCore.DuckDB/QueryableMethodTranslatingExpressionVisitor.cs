using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBQueryableMethodTranslatingExpressionVisitor(QueryableMethodTranslatingExpressionVisitorDependencies           dependencies,
                                                               RelationalQueryableMethodTranslatingExpressionVisitorDependencies relationalDependencies,
                                                               QueryCompilationContext                                           queryCompilationContext)
    : RelationalQueryableMethodTranslatingExpressionVisitor(dependencies, relationalDependencies, queryCompilationContext)
{
    static readonly HashSet<Type> allowedOrderByTypes =
    [
        typeof(DateOnly),
        typeof(DateTime),
        typeof(DateTimeOffset),
        typeof(TimeSpan),
        typeof(TimeOnly),
        typeof(byte),
        typeof(sbyte),
        typeof(int),
        typeof(uint),
        typeof(Int64),
        typeof(UInt64),
        typeof(decimal),
        typeof(float),
        typeof(double),
        typeof(string)
    ];

    protected override QueryableMethodTranslatingExpressionVisitor CreateSubqueryVisitor() => new DuckDBQueryableSubQueryTranslatingExpressionVisitor(this);

    protected override ShapedQueryExpression? TranslateOrderBy(ShapedQueryExpression source, LambdaExpression keySelector, bool ascending)
    {
        var translation = base.TranslateOrderBy(source, keySelector, ascending);
        if (translation == null) return null;

        var orderingExpression     = ((SelectExpression) translation.QueryExpression).Orderings.Last();
        var orderingExpressionType = orderingExpression.Expression.GetProviderType();

        return orderingExpressionType == null || !allowedOrderByTypes.Contains(orderingExpressionType)
                   ? throw new NotSupportedException($"OrderBy not supported: {orderingExpressionType?.ShortDisplayName() ?? "N/A"}")
                   : translation;
    }

    protected override ShapedQueryExpression? TranslateThenBy(ShapedQueryExpression source, LambdaExpression keySelector, bool ascending)
    {
        var translation = base.TranslateThenBy(source, keySelector, ascending);
        if (translation == null) return null;

        var orderingExpression     = ((SelectExpression) translation.QueryExpression).Orderings.Last();
        var orderingExpressionType = orderingExpression.Expression.GetProviderType();
        return orderingExpressionType == null || !allowedOrderByTypes.Contains(orderingExpressionType)
                   ? throw new NotSupportedException($"OrderBy not supported: {orderingExpressionType?.ShortDisplayName() ?? "N/A"}")
                   : translation;
    }
}