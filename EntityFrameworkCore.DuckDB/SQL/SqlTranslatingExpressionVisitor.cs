using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBSqlTranslatingExpressionVisitor(RelationalSqlTranslatingExpressionVisitorDependencies dependencies,
                                                   QueryCompilationContext                               queryCompilationContext,
                                                   QueryableMethodTranslatingExpressionVisitor           queryableMethodTranslatingExpressionVisitor)
    : RelationalSqlTranslatingExpressionVisitor(dependencies, queryCompilationContext, queryableMethodTranslatingExpressionVisitor)
{
    protected override Expression VisitUnary(UnaryExpression unaryExpression)
    {
        if (unaryExpression.NodeType == ExpressionType.ArrayLength && unaryExpression.Operand.Type == typeof(byte[]))
            return Visit(unaryExpression.Operand) is SqlExpression sqlExpression
                       ? Dependencies.SqlExpressionFactory.Function("length", // todo extract to byte[] translation method class
                                                                    new[] {sqlExpression},
                                                                    nullable: true,
                                                                    argumentsPropagateNullability: new[] {true},
                                                                    typeof(int))
                       : QueryCompilationContext.NotTranslatedExpression;

        var visitedExpression = base.VisitUnary(unaryExpression);
        if (visitedExpression == QueryCompilationContext.NotTranslatedExpression)
            return QueryCompilationContext.NotTranslatedExpression;

        // if (visitedExpression is SqlUnaryExpression {OperatorType: ExpressionType.Negate} sqlUnary)
        // {
        //     var operandType = sqlUnary.Operand.GetProviderType();
        //     if (operandType == typeof(decimal))
        //         return Dependencies.SqlExpressionFactory.Function(name: "ef_negate", // sqlite stub, todo refactor
        //                                                           new[] {sqlUnary.Operand},
        //                                                           nullable: true,
        //                                                           new[] {true},
        //                                                           visitedExpression.Type);
        //
        //     if (operandType == typeof(TimeSpan))
        //         return QueryCompilationContext.NotTranslatedExpression;
        // }
        // else
        // {
        // }

        return visitedExpression;
    }

    // protected override Expression VisitBinary(BinaryExpression binaryExpression)
    // {
    //     if (!(base.VisitBinary(binaryExpression) is SqlExpression visitedExpression))
    //         return QueryCompilationContext.NotTranslatedExpression;
    //
    //     if (visitedExpression is SqlBinaryExpression sqlBinary)
    //     {
    //         if (sqlBinary.OperatorType == ExpressionType.Modulo &&
    //             (functionModuloTypes.Contains(sqlBinary.Left.GetProviderType()) || functionModuloTypes.Contains(sqlBinary.Right.GetProviderType())))
    //         {
    //             return Dependencies.SqlExpressionFactory.Function("ef_mod",
    //                                                               new[] {sqlBinary.Left, sqlBinary.Right},
    //                                                               nullable: true,
    //                                                               argumentsPropagateNullability: new[] {true, true},
    //                                                               visitedExpression.Type,
    //                                                               visitedExpression.TypeMapping);
    //         }
    //
    //         if (sqlBinary.AttemptDecimalCompare())
    //             return visitedExpression.DoDecimalCompare(sqlBinary.OperatorType, sqlBinary.Left, sqlBinary.Right, Dependencies.SqlExpressionFactory);
    //
    //         if (sqlBinary.AttemptDecimalArithmetic())
    //             return visitedExpression.doDecimalArithmetics(sqlBinary.OperatorType, sqlBinary.Left, sqlBinary.Right, Dependencies.SqlExpressionFactory);
    //
    //         if (restrictedBinaryExpressions.TryGetValue(sqlBinary.OperatorType, out var restrictedTypes) &&
    //             (restrictedTypes.Contains(sqlBinary.Left.GetProviderType()) || restrictedTypes.Contains(sqlBinary.Right.GetProviderType())))
    //             return QueryCompilationContext.NotTranslatedExpression;
    //     }
    //
    //     return visitedExpression;
    // }
}