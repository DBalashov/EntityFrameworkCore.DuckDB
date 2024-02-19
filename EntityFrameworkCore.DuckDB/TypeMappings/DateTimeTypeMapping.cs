using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBDateTimeTypeMapping : DateTimeTypeMapping
{
    const string DateTimeFormatConst = @"'{0:yyyy\-MM\-dd HH\:mm\:ss.FFFFFFF}'";

    protected override string SqlLiteralFormatString => DateTimeFormatConst;

    public DuckDBDateTimeTypeMapping(string storeType, DbType? dbType = System.Data.DbType.DateTime) : base(storeType, dbType)
    {
    }

    DuckDBDateTimeTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
    {
    }

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters) =>
        new DuckDBDateTimeTypeMapping(parameters);

    public override ValueConverter? Converter { get; }= 
        new ValueConverter<DateTime, DateTime>(v => DateTime.SpecifyKind(v, DateTimeKind.Utc), // todo extract to parameters
                                               v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
}