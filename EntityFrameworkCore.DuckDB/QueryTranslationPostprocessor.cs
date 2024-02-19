using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBQueryTranslationPostprocessor(QueryTranslationPostprocessorDependencies           dependencies,
                                                 RelationalQueryTranslationPostprocessorDependencies relationalDependencies,
                                                 QueryCompilationContext                             ctx)
    : RelationalQueryTranslationPostprocessor(dependencies, relationalDependencies, ctx)
{
    readonly ApplyValidatingVisitor applyValidator = new();

    public override Expression Process(Expression query)
    {
        var result = base.Process(query);
        applyValidator.Visit(result);

        return result;
    }

    sealed class ApplyValidatingVisitor : ExpressionVisitor
    {
        protected override Expression VisitExtension(Expression extensionExpression)
        {
            if (extensionExpression is ShapedQueryExpression shapedQueryExpression)
            {
                Visit(shapedQueryExpression.QueryExpression);
                Visit(shapedQueryExpression.ShaperExpression);

                return extensionExpression;
            }

            if (extensionExpression is SelectExpression selectExpression && selectExpression.Tables.Any(t => t is CrossApplyExpression or OuterApplyExpression))
                throw new InvalidOperationException("Apply not supported");

            return base.VisitExtension(extensionExpression);
        }
    }
}