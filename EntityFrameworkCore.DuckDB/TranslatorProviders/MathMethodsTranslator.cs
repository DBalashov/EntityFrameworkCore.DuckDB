using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace EntityFrameworkCore.DuckDB.Provider;

// https://duckdb.org/docs/sql/functions/numeric
sealed class DuckDBMathMethodsTranslator(ISqlExpressionFactory sqlExpressionFactory) : IMethodCallTranslator
{
    public SqlExpression? Translate(SqlExpression? instance, MethodInfo method, IReadOnlyList<SqlExpression> arguments, IDiagnosticsLogger<DbLoggerCategory.Query> logger)
    {
        if (method.DeclaringType != typeof(Math))
            return null;

        if (arguments.Count < 1)
            throw new ArgumentException("Arguments required", nameof(arguments));

        var nullabilityMask = new[] {true, true};
        var retType         = arguments[0].Type;

        return method.Name switch
               {
                   nameof(Math.Abs)      => sqlExpressionFactory.Function("abs",   arguments, true, nullabilityMask, retType),
                   nameof(Math.Acos)     => sqlExpressionFactory.Function("acos",  arguments, true, nullabilityMask, retType),
                   nameof(Math.Asin)     => sqlExpressionFactory.Function("asin",  arguments, true, nullabilityMask, retType),
                   nameof(Math.Atan)     => sqlExpressionFactory.Function("atan",  arguments, true, nullabilityMask, retType),
                   nameof(Math.Atan2)    => sqlExpressionFactory.Function("atan2", arguments, true, nullabilityMask, retType),
                   nameof(Math.Ceiling)  => sqlExpressionFactory.Function("ceil",  arguments, true, nullabilityMask, retType),
                   nameof(Math.Cos)      => sqlExpressionFactory.Function("cos",   arguments, true, nullabilityMask, retType),
                   nameof(Math.Exp)      => sqlExpressionFactory.Function("exp",   arguments, true, nullabilityMask, retType),
                   nameof(Math.Floor)    => sqlExpressionFactory.Function("floor", arguments, true, nullabilityMask, retType),
                   nameof(Math.Log2)     => sqlExpressionFactory.Function("log2",  arguments, true, nullabilityMask, retType),
                   nameof(Math.Log10)    => sqlExpressionFactory.Function("log10", arguments, true, nullabilityMask, retType),
                   nameof(Math.Log)      => getLogarithmExpression(arguments, nullabilityMask, retType),
                   nameof(Math.Pow)      => sqlExpressionFactory.Function("pow", arguments, true, nullabilityMask, retType),
                   nameof(Math.Round)    => getRoundExpression(arguments, nullabilityMask, retType),
                   nameof(Math.Sign)     => sqlExpressionFactory.Function("sign", arguments, true, nullabilityMask, retType),
                   nameof(Math.Sin)      => sqlExpressionFactory.Function("sin",  arguments, true, nullabilityMask, retType),
                   nameof(Math.Sqrt)     => sqlExpressionFactory.Function("sqrt", arguments, true, nullabilityMask, retType),
                   nameof(Math.Tan)      => sqlExpressionFactory.Function("tan",  arguments, true, nullabilityMask, retType),
                   nameof(Math.Truncate) => sqlExpressionFactory.Function("tan",  arguments, true, nullabilityMask, retType),
                   _                     => throw new NotImplementedException($"{method.Name} not implemented")
               };

        // Unimplemented:
        // ------------
        // bit_count
        // cot
        // degrees
        // divide
        // even
        // factorial
        // fdiv
        // fmod
        // gamma
        // gcd
        // greatest
        // lcm
        // least
        // lgamma
        // radians
        // round_even
        // signbit
        // xor (arithmetic expression?)
    }

    SqlExpression getLogarithmExpression(IReadOnlyList<SqlExpression> arguments, bool[] nullabilityMask, Type retType)
    {
        if (arguments.Count > 1) throw new ArgumentException("Only one argument allowed", nameof(arguments));
        return sqlExpressionFactory.Function("ln", arguments, true, nullabilityMask, retType);
    }

    SqlExpression getRoundExpression(IReadOnlyList<SqlExpression> arguments, bool[] nullabilityMask, Type retType)
    {
        switch (arguments.Count)
        {
            case 1:
            {
                // Math.Round(1.23456) -> round(1.23456)
                return sqlExpressionFactory.Function("round", arguments, true, nullabilityMask, retType);
            }

            case 2:
            {
                // Math.Round(1.23456, 3) -> round(1.23456, 3)
                if (arguments[1].Type != typeof(int))
                    throw new ArgumentException("Second argument must be an integer", nameof(arguments));
                return sqlExpressionFactory.Function("round", arguments, true, nullabilityMask, retType);
            }

            default:
                throw new NotImplementedException($"Only one or two arguments allowed for {nameof(Math.Round)}");
        }
    }
}