using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBModelValidator(ModelValidatorDependencies dependencies,
                                  RelationalModelValidatorDependencies relationalDependencies) : RelationalModelValidator(dependencies, relationalDependencies)
{
    public override void Validate(IModel model, IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
    {
    }

    protected override void ValidateValueGeneration(IEntityType                                           entityType,
                                                    IKey                                                  key,
                                                    IDiagnosticsLogger<DbLoggerCategory.Model.Validation> logger)
    {
        base.ValidateValueGeneration(entityType, key, logger);

        var keyProperties = key.Properties;
        if (key.IsPrimaryKey()                                                       &&
            keyProperties.Count(p => p.ClrType.UnwrapNullableType().IsInteger()) > 1 &&
            keyProperties.Any(p => p.ValueGenerated == ValueGenerated.OnAdd   &&
                                   p.ClrType.UnwrapNullableType().IsInteger() &&
                                   !p.TryGetDefaultValue(out _)               &&
                                   p.GetDefaultValueSql() == null             &&
                                   !p.IsForeignKey()))
        {
            // logger.CompositeKeyWithValueGeneration(key);
        }
    }
}