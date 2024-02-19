using Microsoft.EntityFrameworkCore.Query;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBQueryTranslationPostprocessorFactory(QueryTranslationPostprocessorDependencies dependencies,
                                                        RelationalQueryTranslationPostprocessorDependencies relationalDependencies) : IQueryTranslationPostprocessorFactory
{
    public QueryTranslationPostprocessor Create(QueryCompilationContext ctx) =>
        new DuckDBQueryTranslationPostprocessor(dependencies, relationalDependencies, ctx);
}