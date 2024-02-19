using System.Data;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBStringTypeMapping : StringTypeMapping
{
    public DuckDBStringTypeMapping(string storeType, DbType? dbType = null, bool unicode = false, int? size = null) : base(storeType, dbType, unicode, size)
    {
    }

    DuckDBStringTypeMapping(RelationalTypeMappingParameters parameters) : base(parameters)
    {
    }

    protected override RelationalTypeMapping Clone(RelationalTypeMappingParameters parameters) => new DuckDBStringTypeMapping(parameters);

    protected override string GenerateNonNullSqlLiteral(object value) =>
        value switch
        {
            string s         => "'" + s.Replace("'", "''")             + "'",
            StringBuilder sb => "'" + sb.ToString().Replace("'", "''") + "'",
            null             => "NULL",
            _                => base.GenerateNonNullSqlLiteral(value)
        };
}