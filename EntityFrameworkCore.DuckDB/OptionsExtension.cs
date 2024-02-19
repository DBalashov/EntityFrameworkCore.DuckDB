using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFrameworkCore.DuckDB.Provider;

public class DuckDBOptionsExtension : RelationalOptionsExtension
{
    DbContextOptionsExtensionInfo? _info;

    public override DbContextOptionsExtensionInfo Info => _info ??= new ExtensionInfo(this);

    public DuckDBOptionsExtension()
    {
    }

    protected DuckDBOptionsExtension(DuckDBOptionsExtension copyFrom) : base(copyFrom)
    {
    }
    
    public override void ApplyServices(IServiceCollection services) => 
        services.AddEntityFrameworkDuckDBProvider();

    protected override RelationalOptionsExtension Clone() => 
        new DuckDBOptionsExtension(this);

    sealed class ExtensionInfo(IDbContextOptionsExtension extension) : RelationalExtensionInfo(extension)
    {
        string? logFragment;

        public override bool IsDatabaseProvider => true;

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => 
            other is ExtensionInfo;

        public override string LogFragment => 
            logFragment ??= base.LogFragment;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo) => 
            debugInfo["debug"] = "1";
    }
}