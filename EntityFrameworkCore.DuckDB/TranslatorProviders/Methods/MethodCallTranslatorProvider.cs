using Microsoft.EntityFrameworkCore.Query;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBMethodCallTranslatorProvider : RelationalMethodCallTranslatorProvider
{
    public DuckDBMethodCallTranslatorProvider(RelationalMethodCallTranslatorProviderDependencies dependencies) : base(dependencies) =>
        AddTranslators(new IMethodCallTranslator[]
                       {
                           new DuckDBMathMethodsTranslator(dependencies.SqlExpressionFactory),
                           new DuckDBDoubleMethodsTranslator(dependencies.SqlExpressionFactory),
                           new DuckDBRandomMethodsTranslator(dependencies.SqlExpressionFactory),
                           new DuckDBStringMethodsTranslator(dependencies.SqlExpressionFactory),
                           new DuckDBDateTimeMethodsTranslator(dependencies.SqlExpressionFactory),
                           new DuckDBDateTimeOffsetMethodsTranslator(dependencies.SqlExpressionFactory),
                           new DuckDBTimeOnlyMethodsTranslator(dependencies.SqlExpressionFactory),
                           // new DuckDBTimeSpanMethodsTranslator(dependencies.SqlExpressionFactory), temporary removed
                           new DuckDBGuidMethodsTranslator(dependencies.SqlExpressionFactory)
                       });
}