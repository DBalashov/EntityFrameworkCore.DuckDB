using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBTimeSpanTypeMapping : TimeSpanTypeMapping
{
    public DuckDBTimeSpanTypeMapping(string storeType, DbType? dbType = System.Data.DbType.Time) : base(storeType, dbType)
    {
    }

    DuckDBTimeSpanTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
    {
    }

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters) =>
        new DuckDBTimeSpanTypeMapping(parameters);

    public override ValueConverter? Converter { get; } =
        new ValueConverter<TimeSpan, TimeOnly>(v => TimeOnly.FromTimeSpan(v),
                                               v => v.ToTimeSpan());
}