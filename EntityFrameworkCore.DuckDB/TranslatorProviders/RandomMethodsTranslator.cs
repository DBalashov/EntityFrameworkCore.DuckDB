using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

// partial from https://duckdb.org/docs/sql/functions/numeric
sealed class DuckDBRandomMethodsTranslator(ISqlExpressionFactory sqlExpressionFactory) : IMethodCallTranslator
{
    public SqlExpression? Translate(SqlExpression? instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (method.DeclaringType != typeof(Random))
            return null;
        
        if (arguments.Count < 1)
            throw new ArgumentException("Arguments required", nameof(arguments));

        return method.Name switch
               {
                   nameof(Random.NextDouble) => sqlExpressionFactory.Function("random", Array.Empty<SqlExpression>(), true, Array.Empty<bool>(), method.ReturnType),
                   nameof(Random.Next)       => getRandomNextExpress(method, arguments),
                   _                         => null
               };
    }

    SqlExpression getRandomNextExpress(MethodInfo method, IReadOnlyList<SqlExpression> arguments)
    {
        switch (arguments.Count)
        {
            case 0:
            {
                // Random.Next() -> random()
                return sqlExpressionFactory.Function("random", Array.Empty<SqlExpression>(), true, Array.Empty<bool>(), method.ReturnType);
            }
            case 1:
            {
                // Random.Next(maxValue:100) -> cast(random() * 100 as int) 
                return sqlExpressionFactory.Convert(new SqlBinaryExpression(ExpressionType.Multiply,
                                                                            new SqlFunctionExpression("random", false, typeof(double), null),
                                                                            arguments[0],
                                                                            method.ReturnType,
                                                                            null),
                                                    typeof(int));
            }
            case 2:
            {
                // Random.Next(minValue:10, maxValue:100) -> cast(10 + random() * (100-10) as int);

                // random() * (100-10) as int
                var randomMultipliedWithDiff = new SqlBinaryExpression(ExpressionType.Multiply,
                                                                       new SqlFunctionExpression("random", false, typeof(double), null),
                                                                       new SqlBinaryExpression(ExpressionType.Subtract,
                                                                                               arguments[1],
                                                                                               arguments[0],
                                                                                               method.ReturnType,
                                                                                               null),
                                                                       method.ReturnType,
                                                                       null);

                return sqlExpressionFactory.Convert(new SqlBinaryExpression(ExpressionType.Add,
                                                                            arguments[0],
                                                                            randomMultipliedWithDiff,
                                                                            typeof(int),
                                                                            null),
                                                    typeof(int));
            }
            default:
                throw new NotImplementedException();
        }
    }
}