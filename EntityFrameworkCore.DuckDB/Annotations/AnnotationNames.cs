namespace EntityFrameworkCore.DuckDB.Provider;

static class DuckDBAnnotationNames
{
    public const string Prefix              = "DuckDB:";
    public const string LegacyAutoincrement = "Autoincrement";
    public const string Autoincrement       = Prefix + LegacyAutoincrement;
}