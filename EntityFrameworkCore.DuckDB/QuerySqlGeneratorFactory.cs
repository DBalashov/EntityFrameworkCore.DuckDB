using Microsoft.EntityFrameworkCore.Query;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBQuerySqlGeneratorFactory(QuerySqlGeneratorDependencies dependencies) : IQuerySqlGeneratorFactory
{
    public QuerySqlGenerator Create() => new DuckDBQuerySqlGenerator(dependencies);
}