using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8629 // Nullable value type may be null.

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace EntityFrameworkCore.DuckDB.Tests;

public class DateTimeOffsetTests : BaseTest
{
    [ExcludeFromCodeCoverage]
    public class Entity
    {
        public DateTimeOffset  DateTime         { get; set; }
        public DateTimeOffset? DateTimeNullable { get; set; }
    }

    DBContext<Entity> ctx;

    [SetUp]
    public void Setup() =>
        ctx = new("Data Source=:memory:");

    [Test]
    public void UtcNow()
    {
        var query = ctx.Items.Where(p => p.DateTime < DateTimeOffset.UtcNow).ToQueryString();
        Assert.True(query.Split(Environment.NewLine).Last()
                         .EndsWith("< now()"));
    }

    [Test]
    public void Now()
    {
        var query = ctx.Items.Where(p => p.DateTime < DateTimeOffset.Now).ToQueryString();
        Assert.True(query.Split(Environment.NewLine).Last()
                         .EndsWith("< now()"));
    }

    [Test]
    public void PropertiesNotNullable()
    {
        var regex = @"datepart\('(\S+)', \S+\.DateTime\)\s*>\s*1";

        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTime.DayOfYear   > 1).ToQueryString(),                regex, "dayofyear"),    nameof(DateTimeOffset.DayOfYear));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTime.DayOfWeek   > DayOfWeek.Monday).ToQueryString(), regex, "dayofweek"),    nameof(DateTimeOffset.DayOfWeek));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTime.Year        > 1).ToQueryString(),                regex, "year"),         nameof(DateTimeOffset.Year));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTime.Month       > 1).ToQueryString(),                regex, "month"),        nameof(DateTimeOffset.Month));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTime.Day         > 1).ToQueryString(),                regex, "day"),          nameof(DateTimeOffset.Day));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTime.Hour        > 1).ToQueryString(),                regex, "hour"),         nameof(DateTimeOffset.Hour));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTime.Minute      > 1).ToQueryString(),                regex, "minute"),       nameof(DateTimeOffset.Minute));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTime.Second      > 1).ToQueryString(),                regex, "second"),       nameof(DateTimeOffset.Second));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTime.Millisecond > 1).ToQueryString(),                regex, "milliseconds"), nameof(DateTimeOffset.Millisecond));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTime.Microsecond > 1).ToQueryString(),                regex, "microseconds"), nameof(DateTimeOffset.Microsecond));
    }

    [Test]
    public void PropertiesNullable()
    {
        var regex = @"datepart\('(\S+)', \S+\.DateTimeNullable\)\s*>\s*1";

        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTimeNullable.Value.DayOfYear   > 1).ToQueryString(),                regex, "dayofyear"),    nameof(DateTimeOffset.DayOfYear));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTimeNullable.Value.DayOfWeek   > DayOfWeek.Monday).ToQueryString(), regex, "dayofweek"),    nameof(DateTimeOffset.DayOfWeek));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTimeNullable.Value.Year        > 1).ToQueryString(),                regex, "year"),         nameof(DateTimeOffset.Year));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTimeNullable.Value.Month       > 1).ToQueryString(),                regex, "month"),        nameof(DateTimeOffset.Month));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTimeNullable.Value.Day         > 1).ToQueryString(),                regex, "day"),          nameof(DateTimeOffset.Day));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTimeNullable.Value.Hour        > 1).ToQueryString(),                regex, "hour"),         nameof(DateTimeOffset.Hour));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTimeNullable.Value.Minute      > 1).ToQueryString(),                regex, "minute"),       nameof(DateTimeOffset.Minute));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTimeNullable.Value.Second      > 1).ToQueryString(),                regex, "second"),       nameof(DateTimeOffset.Second));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTimeNullable.Value.Millisecond > 1).ToQueryString(),                regex, "milliseconds"), nameof(DateTimeOffset.Millisecond));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTimeNullable.Value.Microsecond > 1).ToQueryString(),                regex, "microseconds"), nameof(DateTimeOffset.Microsecond));
    }

    [Test]
    public void MethodsNotNullable()
    {
        var regexNonConvertible = @"\S+\.DateTime\s*<\s*date_add\(\S+\.DateTime,\s*(\S+)\(1\)\)";

        // i.DateTime < date_add(i.DateTime, to_years(1))
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTime < p.DateTime.AddYears(1)).ToQueryString(), regexNonConvertible, "to_years"));

        // // i.DateTime < date_add(i.DateTime, to_months(1))
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTime < p.DateTime.AddMonths(1)).ToQueryString(), regexNonConvertible, "to_months"));

        var regexConvertible = @"\S+\.DateTime\s*<\s*date_add\(\S+\.DateTime,\s*to_microseconds\(CAST\(1.0\s*\*\s*(\S+)\s*AS bigint\)\)\)";
        // i.DateTime < date_add(i.DateTime, to_microseconds(CAST(1.0 * 86400000000.0 AS bigint)))
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTime < p.DateTime.AddDays(1)).ToQueryString(),         regexConvertible, "86400000000.0"));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTime < p.DateTime.AddHours(1)).ToQueryString(),        regexConvertible, "3600000000.0"));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTime < p.DateTime.AddMinutes(1)).ToQueryString(),      regexConvertible, "60000000.0"));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTime < p.DateTime.AddSeconds(1)).ToQueryString(),      regexConvertible, "1000000.0"));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTime < p.DateTime.AddMilliseconds(1)).ToQueryString(), regexConvertible, "1000.0"));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTime < p.DateTime.AddMicroseconds(1)).ToQueryString(), regexConvertible, "1.0"));
    }

    [Test]
    public void MethodsNullable()
    {
        var regexNonConvertible = @"\S+\.DateTimeNullable\s*<\s*date_add\(\S+\.DateTimeNullable,\s*(\S+)\(1\)\)";

        // i.DateTime < date_add(i.DateTime, to_years(1))
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTimeNullable < p.DateTimeNullable.Value.AddYears(1)).ToQueryString(), regexNonConvertible, "to_years"));

        // // i.DateTime < date_add(i.DateTime, to_months(1))
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTimeNullable < p.DateTimeNullable.Value.AddMonths(1)).ToQueryString(), regexNonConvertible, "to_months"));

        var regexConvertible = @"\S+\.DateTimeNullable\s*<\s*date_add\(\S+\.DateTimeNullable,\s*to_microseconds\(CAST\(1.0\s*\*\s*(\S+)\s*AS bigint\)\)\)";
        // i.DateTime < date_add(i.DateTime, to_microseconds(CAST(1.0 * 86400000000.0 AS bigint)))
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTimeNullable < p.DateTimeNullable.Value.AddDays(1)).ToQueryString(),         regexConvertible, "86400000000.0"));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTimeNullable < p.DateTimeNullable.Value.AddHours(1)).ToQueryString(),        regexConvertible, "3600000000.0"));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTimeNullable < p.DateTimeNullable.Value.AddMinutes(1)).ToQueryString(),      regexConvertible, "60000000.0"));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTimeNullable < p.DateTimeNullable.Value.AddSeconds(1)).ToQueryString(),      regexConvertible, "1000000.0"));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTimeNullable < p.DateTimeNullable.Value.AddMilliseconds(1)).ToQueryString(), regexConvertible, "1000.0"));
        Assert.IsTrue(ValidateFirstGroup(ctx.Items.Where(p => p.DateTimeNullable < p.DateTimeNullable.Value.AddMicroseconds(1)).ToQueryString(), regexConvertible, "1.0"));
    }
}