using Microsoft.EntityFrameworkCore.Query;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBMemberTranslatorProvider : RelationalMemberTranslatorProvider
{
    public DuckDBMemberTranslatorProvider(RelationalMemberTranslatorProviderDependencies dependencies) : base(dependencies)
    {
        var sqlExpressionFactory = (DuckDBSqlExpressionFactory)dependencies.SqlExpressionFactory;
        AddTranslators(new IMemberTranslator[]
                       {
                           new DuckDBDateTimeMemberTranslator(sqlExpressionFactory),
                           new DuckDBDateTimeOffsetMemberTranslator(sqlExpressionFactory),
                       });
    }
}
