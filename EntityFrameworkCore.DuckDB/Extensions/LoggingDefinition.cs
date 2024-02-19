using Microsoft.EntityFrameworkCore.Diagnostics;

#pragma warning disable CS0649

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBLoggingDefinitions : RelationalLoggingDefinitions
{
    public EventDefinitionBase? LogSchemaConfigured;
    public EventDefinitionBase? LogSequenceConfigured;
    public EventDefinitionBase? LogUsingSchemaSelectionsWarning;
    public EventDefinitionBase? LogFoundColumn;
    public EventDefinitionBase? LogFoundForeignKey;
    public EventDefinitionBase? LogForeignKeyScaffoldErrorPrincipalTableNotFound;
    public EventDefinitionBase? LogFoundTable;
    public EventDefinitionBase? LogMissingTable;
    public EventDefinitionBase? LogPrincipalColumnNotFound;
    public EventDefinitionBase? LogFoundIndex;
    public EventDefinitionBase? LogFoundPrimaryKey;
    public EventDefinitionBase? LogFoundUniqueConstraint;
    public EventDefinitionBase? LogUnexpectedConnectionType;
    public EventDefinitionBase? LogTableRebuildPendingWarning;
    public EventDefinitionBase? LogCompositeKeyWithValueGeneration;
}