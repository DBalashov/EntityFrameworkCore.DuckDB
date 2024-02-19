using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBConventionSetBuilder(ProviderConventionSetBuilderDependencies dependencies, RelationalConventionSetBuilderDependencies relationalDependencies)
    : RelationalConventionSetBuilder(dependencies, relationalDependencies)
{
    public override ConventionSet CreateConventionSet()
    {
        var conventionSet = base.CreateConventionSet();

        conventionSet.Replace<SharedTableConvention>(new DuckDBSharedTableConvention(Dependencies, RelationalDependencies));
        conventionSet.Replace<RuntimeModelConvention>(new DuckDBRuntimeModelConvention(Dependencies, RelationalDependencies));

        return conventionSet;
    }
}