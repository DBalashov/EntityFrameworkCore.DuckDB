using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBByteArrayTypeMapping : TimeSpanTypeMapping
{
    public DuckDBByteArrayTypeMapping(string storeType, DbType? dbType = System.Data.DbType.Binary) : base(storeType, dbType)
    {
    }

    DuckDBByteArrayTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
    {
    }

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters) =>
        new DuckDBByteArrayTypeMapping(parameters);

    // who is responsible for disposing the MemoryStream?
    public override ValueConverter? Converter { get; } =
        new ValueConverter<byte[], MemoryStream>(v => new MemoryStream(v),
                                                 v => v.ToArray());
}