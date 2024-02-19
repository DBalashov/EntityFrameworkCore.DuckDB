using Microsoft.EntityFrameworkCore.Query;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBMemberTranslatorProvider : RelationalMemberTranslatorProvider
{
    public DuckDBMemberTranslatorProvider(RelationalMemberTranslatorProviderDependencies dependencies) : base(dependencies)
    {
        AddTranslators(new IMemberTranslator[]
                       {
                           new DuckDBDateTimeMemberTranslator(dependencies.SqlExpressionFactory),
                           new DuckDBDateTimeOffsetMemberTranslator(dependencies.SqlExpressionFactory),
                           new DuckDBTimeSpanMemberTranslator(dependencies.SqlExpressionFactory),
                           new DuckDBTimeOnlyMemberTranslator(dependencies.SqlExpressionFactory),
                       });
    }
}