using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBQuerySqlGenerator(QuerySqlGeneratorDependencies dependencies) : QuerySqlGenerator(dependencies)
{
    protected override string GetOperator(SqlBinaryExpression binaryExpression) =>
        binaryExpression.OperatorType == ExpressionType.Add && binaryExpression.Type == typeof(string)
            ? " || "
            : base.GetOperator(binaryExpression);

    protected override void GenerateLimitOffset(SelectExpression selectExpression)
    {
        if (selectExpression.Limit == null && selectExpression.Offset == null)
            return;

        Sql.AppendLine().Append("LIMIT ");
        Visit(selectExpression.Limit ?? new SqlConstantExpression(Expression.Constant(-1), selectExpression.Offset!.TypeMapping));

        if (selectExpression.Offset == null)
            return;

        Sql.Append(" OFFSET ");
        Visit(selectExpression.Offset);
    }
    
    //  doesn't support parentheses around set operation operands
    protected override void GenerateSetOperationOperand(SetOperationBase setOperation, SelectExpression operand) => Visit(operand);
}