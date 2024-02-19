using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBDateTimeOffsetTypeMapping : DateTimeOffsetTypeMapping
{
    const string DateTimeOffsetFormatConst = @"'{0:yyyy\-MM\-dd HH\:mm\:ss.FFFFFFFzzz}'";

    protected override string SqlLiteralFormatString => DateTimeOffsetFormatConst;

    public DuckDBDateTimeOffsetTypeMapping(string storeType, DbType? dbType = System.Data.DbType.DateTimeOffset) : base(storeType, dbType)
    {
    }

    DuckDBDateTimeOffsetTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
    {
    }

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters) => 
        new DuckDBDateTimeOffsetTypeMapping(parameters);
}