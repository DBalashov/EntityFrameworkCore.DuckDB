using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBSharedTableConvention(ProviderConventionSetBuilderDependencies dependencies,
                                         RelationalConventionSetBuilderDependencies relationalDependencies) : SharedTableConvention(dependencies, relationalDependencies)
{
    protected override bool CheckConstraintsUniqueAcrossTables => false;
}