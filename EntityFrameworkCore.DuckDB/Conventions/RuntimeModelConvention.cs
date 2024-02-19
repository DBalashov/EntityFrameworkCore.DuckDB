using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBRuntimeModelConvention(ProviderConventionSetBuilderDependencies   dependencies,
                                          RelationalConventionSetBuilderDependencies relationalDependencies) : RelationalRuntimeModelConvention(dependencies, relationalDependencies)
{
    protected override void ProcessPropertyAnnotations(Dictionary<string, object?> annotations,
                                                       IProperty                   property,
                                                       RuntimeProperty             runtimeProperty,
                                                       bool                        runtime)
    {
        var columnName = property.GetColumnName();
        if (columnName.IndexOf(' ') >= 0)
        {
            columnName                                        = "\"" + columnName + "\"";
            annotations[RelationalAnnotationNames.ColumnName] = columnName;
        }

        base.ProcessPropertyAnnotations(annotations, property, runtimeProperty, runtime);
    }
}