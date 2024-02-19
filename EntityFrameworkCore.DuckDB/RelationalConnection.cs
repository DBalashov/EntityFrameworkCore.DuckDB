using System.Data.Common;
using DuckDB.NET;
using DuckDB.NET.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.DuckDB.Provider;

class DuckDBRelationalConnection : RelationalConnection
{
    readonly string connectionString;

    public DuckDBRelationalConnection(RelationalConnectionDependencies dependencies) : base(dependencies)
    {
        var optionsExtension = dependencies.ContextOptions.Extensions.OfType<DuckDBOptionsExtension>().FirstOrDefault();
        if (optionsExtension == null) 
            throw new InvalidOperationException($"Can't find {nameof(DuckDBOptionsExtension)}");

        if (string.IsNullOrWhiteSpace(optionsExtension.ConnectionString))
            throw new ArgumentNullException(nameof(optionsExtension.ConnectionString));

        connectionString = optionsExtension.ConnectionString;
    }

    protected override DbConnection CreateDbConnection() =>
        new DuckDBConnection(connectionString);
}