using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBAnnotationProvider(RelationalAnnotationProviderDependencies dependencies) : RelationalAnnotationProvider(dependencies)
{
    public override IEnumerable<IAnnotation> For(IColumn column, bool designTime)
    {
        // Model validation ensures that these facets are the same on all mapped properties
        var property = column.PropertyMappings.First().Property;

        // Only return auto increment for integer single column primary key
        var primaryKey = property.DeclaringEntityType.FindPrimaryKey();
        if (primaryKey is {Properties.Count: 1}               &&
            primaryKey.Properties[0] == property              &&
            property.ValueGenerated  == ValueGenerated.OnAdd  &&
            property.ClrType.UnwrapNullableType().IsInteger() && !HasConverter(property)) yield return new Annotation(DuckDBAnnotationNames.Autoincrement, true);
    }

    static bool HasConverter(IProperty property) => property.FindTypeMapping()?.Converter != null;
}