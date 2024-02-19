using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBSqlTranslatingExpressionVisitor(RelationalSqlTranslatingExpressionVisitorDependencies dependencies,
                                                   QueryCompilationContext                               ctx,
                                                   QueryableMethodTranslatingExpressionVisitor           visitor)
    : RelationalSqlTranslatingExpressionVisitor(dependencies, ctx, visitor)
{
    protected override Expression VisitUnary(UnaryExpression unaryExpr)
    {
        switch (unaryExpr.NodeType)
        {
            case ExpressionType.ArrayLength when (unaryExpr.Operand.Type == typeof(byte[]) | unaryExpr.Operand.Type == typeof(MemoryStream)):
            {
                // https://duckdb.org/docs/sql/functions/blob
                return Visit(unaryExpr.Operand) is SqlExpression sqlExpression
                           ? Dependencies.SqlExpressionFactory.Function("octal_length", new[] {sqlExpression}, true, new[] {true}, typeof(int))
                           : QueryCompilationContext.NotTranslatedExpression;
            }

            case ExpressionType.Not when (unaryExpr.Operand.Type == typeof(decimal) ||
                                          unaryExpr.Operand.Type == typeof(double)  ||
                                          unaryExpr.Operand.Type == typeof(float)   ||
                                          unaryExpr.Operand.Type == typeof(int)     ||
                                          unaryExpr.Operand.Type == typeof(uint)    ||
                                          unaryExpr.Operand.Type == typeof(byte)    ||
                                          unaryExpr.Operand.Type == typeof(sbyte)):
            {
                return Visit(unaryExpr.Operand) is SqlExpression sqlExpression
                           ? Dependencies.SqlExpressionFactory.Not(sqlExpression)
                           : QueryCompilationContext.NotTranslatedExpression;
            }

            default:
                return base.VisitUnary(unaryExpr);
        }
    }
}