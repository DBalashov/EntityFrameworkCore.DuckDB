using EntityFrameworkCore.DuckDB.Provider;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace EntityFrameworkCore.DuckDB.Tests;

public class DBContext<T>(string connectionString) : DbContext where T : class
{
    public DbSet<T> Items { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseDuckDB(connectionString);
}