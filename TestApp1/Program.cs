using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using DuckDB.NET.Data;
using EntityFrameworkCore.DuckDB.Provider;
using Microsoft.EntityFrameworkCore;

var fileName = Path.Combine(Path.GetTempPath(), "test1.db");
if (File.Exists(fileName))
    File.Delete(fileName);

using (var c = new DuckDBConnection("Data Source=" + fileName))
{
    c.Open();
    var cmd = c.CreateCommand();
    cmd.CommandText = File.ReadAllText("test1.sql");
    cmd.ExecuteNonQuery();
}

// using (var c = new DuckDBConnection("Data Source=" + fileName))
// {
//     c.Open();
//     var cmd = c.CreateCommand();
//     cmd.CommandText = "select * from test1";
//     using var reader = cmd.ExecuteReader();
//     while (reader.Read())
//     {
//         for (var i = 0; i < reader.FieldCount; i++)
//             Console.Write(reader.GetValue(i) + " ");
//         Console.WriteLine();
//     }
// }

using (var db = new CTX("Data Source=" + fileName))
{
    // var sql = db.Test1s.Where(p => p.test_timestamp_nn < DateTime.UtcNow).ToQueryString();
    // var sql  = db.Test1s.Where(p => p.test_integer_nn < Random.Shared.Next()).ToQueryString();
    // var sql2 = db.Test1s.Where(p => p.test_integer_nn < Random.Shared.Next(1000)).ToQueryString();
    // var sql3 = db.Test1s.Where(p => p.test_integer_nn < Random.Shared.Next(100, 200)).ToQueryString();
    // var sql   = db.Test1s.Where(p => p.test_timestamp_nn < DateTime.Now).ToQueryString();
    // var sql3 = db.Test1s.Where(p => p.test_integer_nn < Math.Round(p.test_decimal_nn)).ToQueryString();
    // var sql4 = db.Test1s.Where(p => p.test_integer_nn < Math.Round(p.test_decimal_nn, 2)).ToQueryString();
    //    var sql4 = db.Test1s.Where(p => p.test_varchar_nn.StartsWith("x1")).ToQueryString();
    var sql4 = db.Test1s.Where(p => p.test_varchar_nn.IndexOf("abc")>0).ToQueryString();

    var items = db.Test1s.Where(p => p.test_timestamp_nn < DateTime.UtcNow).ToArray();

    Console.ReadLine();
}

class CTX(string connectionString) : DbContext
{
    public DbSet<DBTest1> Test1s { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseDuckDB(connectionString);
}

[Table("test1")]
public class DBTest1
{
    public Int64? test_bigint_null { get; set; }
    public Int64  test_bigint_nn   { get; set; }

    public byte[]? test_blob_null { get; set; }
    public byte[]  test_blob_nn   { get; set; }

    public bool? test_boolean_null { get; set; }
    public bool  test_boolean_nn   { get; set; }

    public DateOnly? test_date_null { get; set; }
    public DateOnly  test_date_nn   { get; set; }

    public decimal? test_decimal_null { get; set; }
    public decimal  test_decimal_nn   { get; set; }

    public double? test_double_null { get; set; }
    public double  test_double_nn   { get; set; }

    public float? test_real_null { get; set; }
    public float  test_real_nn   { get; set; }

    public int? test_integer_null { get; set; }
    public int  test_integer_nn   { get; set; }

    public short? test_smallint_null { get; set; }
    public short  test_smallint_nn   { get; set; }

    public TimeOnly? test_time_null { get; set; }
    public TimeOnly  test_time_nn   { get; set; }

    public TimeSpan? test_timespan_null { get; set; }
    public TimeSpan  test_timespan_nn   { get; set; }

    public DateTime? test_timestamp_null { get; set; }
    public DateTime  test_timestamp_nn   { get; set; }

    public sbyte? test_tinyint_null { get; set; }
    public sbyte  test_tinyint_nn   { get; set; }

    public UInt64? test_ubigint_null { get; set; }
    public UInt64  test_ubigint_nn   { get; set; }

    public uint? test_uinteger_null { get; set; }
    public uint  test_uinteger_nn   { get; set; }

    public byte? test_utinyint_null { get; set; }
    public byte  test_utinyint_nn   { get; set; }

    // public Guid? test_uuid_null { get; set; }
    // public Guid  test_uuid_nn   { get; set; }

    public string? test_varchar_null { get; set; }
    public string  test_varchar_nn   { get; set; }
}