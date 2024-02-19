using Microsoft.EntityFrameworkCore.Update;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBModificationCommandBatchFactory(ModificationCommandBatchFactoryDependencies dependencies) : IModificationCommandBatchFactory
{
    public ModificationCommandBatch Create() => 
        new SingularModificationCommandBatch(dependencies);
}