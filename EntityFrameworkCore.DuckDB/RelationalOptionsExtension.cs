using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable CS8618

namespace EntityFrameworkCore.DuckDB.Provider;

public sealed class DuckDBRelationalOptionsExtension : RelationalOptionsExtension
{
    protected override RelationalOptionsExtension Clone() => new DuckDBRelationalOptionsExtension();

    public override void ApplyServices(IServiceCollection services) => services.AddEntityFrameworkDuckDBProvider();

    public override DbContextOptionsExtensionInfo Info { get; }
}