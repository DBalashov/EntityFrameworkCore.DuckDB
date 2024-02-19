using Microsoft.EntityFrameworkCore.Query;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBAggregateMethodCallTranslatorProvider : RelationalAggregateMethodCallTranslatorProvider
{
    public DuckDBAggregateMethodCallTranslatorProvider(RelationalAggregateMethodCallTranslatorProviderDependencies dependencies) : base(dependencies)
    {
        var sqlExpressionFactory = dependencies.SqlExpressionFactory;

        // AddTranslators(new IAggregateMethodCallTranslator[]
        //                {
        //                    new YdbQueryableAggregateMethodTranslator(sqlExpressionFactory),
        //                    new YdbStringAggregateMethodTranslator(sqlExpressionFactory)
        //                });
    }
}
