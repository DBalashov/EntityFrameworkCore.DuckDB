using System.Text;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.DuckDB.Provider;

sealed class DuckDBSqlGenerationHelper(RelationalSqlGenerationHelperDependencies dependencies) : RelationalSqlGenerationHelper(dependencies)
{
    public override string StartTransactionStatement => "BEGIN TRANSACTION" + StatementTerminator; // todo check

    public override string DelimitIdentifier(string name, string? schema) => name;

    public override void DelimitIdentifier(StringBuilder builder, string name, string? schema) => base.DelimitIdentifier(builder, name);

    public override string GenerateParameterName(string name) => "$" + name;

    public override string DelimitIdentifier(string identifier) => identifier;
}
